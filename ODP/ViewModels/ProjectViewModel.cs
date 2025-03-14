using ACDRFrameReaderLib;
using BigEndianReaderLib;
using EthernetFrameReaderLib;
using LogLib;
using Microsoft.Windows.Themes;
using ODP.CoreLib;
using PcapReaderLib;
using RTCPFrameReaderLib;
using RTPFrameReaderLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class ProjectViewModel:GenericViewModel<Project>
	{
		public event EventHandler? SessionsChanged;
		public event EventHandler? FilteredSessionsChanged;

		private List<string> loadedFiles;
		private List<string> loadedDebugRecordingFiles;

		public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(ProjectViewModel), new PropertyMetadata("New project"));
		public string Name
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}


		public static readonly DependencyProperty RunningTaskProperty = DependencyProperty.Register("RunningTask", typeof(string), typeof(ProjectViewModel), new PropertyMetadata(null));
		public string? RunningTask
		{
			get { return (string?)GetValue(RunningTaskProperty); }
			set { SetValue(RunningTaskProperty, value); }
		}

		public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string), typeof(ProjectViewModel), new PropertyMetadata(null));
		public string Path
		{
			get { return (string)GetValue(PathProperty); }
			set { SetValue(PathProperty, value); }
		}
		
		public static readonly DependencyProperty PacketLossReportsProperty = DependencyProperty.Register("PacketLossReports", typeof(PacketLossReportViewModelCollection), typeof(ProjectViewModel), new PropertyMetadata(null));
		public PacketLossReportViewModelCollection PacketLossReports
		{
			get { return (PacketLossReportViewModelCollection)GetValue(PacketLossReportsProperty); }
			set { SetValue(PacketLossReportsProperty, value); }
		}


		/*public static readonly DependencyProperty SessionsProperty = DependencyProperty.Register("Sessions", typeof(SessionViewModelCollection), typeof(ProjectViewModel), new PropertyMetadata(null));
		public SessionViewModelCollection Sessions
		{
			get { return (SessionViewModelCollection)GetValue(SessionsProperty); }
			set { SetValue(SessionsProperty, value); }
		}*/

		public static readonly DependencyProperty FilteredSessionsProperty = DependencyProperty.Register("FilteredSessions", typeof(SessionViewModelCollection), typeof(ProjectViewModel), new PropertyMetadata(null));
		public SessionViewModelCollection FilteredSessions
		{
			get { return (SessionViewModelCollection)GetValue(FilteredSessionsProperty); }
			set { SetValue(FilteredSessionsProperty, value); }
		}


		/*public static readonly DependencyProperty FiltersProperty = DependencyProperty.Register("Filters", typeof(ViewModelCollection<FilterViewModel>), typeof(ProjectViewModel), new PropertyMetadata(null));
		public ViewModelCollection<FilterViewModel> session
		{
			get { return (ViewModelCollection<FilterViewModel>)GetValue(FiltersProperty); }
			set { SetValue(FiltersProperty, value); }
		}*/



		public static readonly DependencyProperty GlobalFilterProperty = DependencyProperty.Register("GlobalFilter", typeof(GlobalFilterViewModel), typeof(ProjectViewModel), new PropertyMetadata(null));
		public GlobalFilterViewModel GlobalFilter
		{
			get { return (GlobalFilterViewModel)GetValue(GlobalFilterProperty); }
			set { SetValue(GlobalFilterProperty, value); }
		}




		public ProjectViewModel(Project Model) : base(Model)
		{

			loadedFiles = new List<string>();
			loadedDebugRecordingFiles = new List<string>();
			PacketLossReports = new PacketLossReportViewModelCollection(Model.PacketLossReports);
			//Sessions = new SessionViewModelCollection(Model.Sessions);
			//session = new ViewModelCollection<FilterViewModel>(Logger);
			GlobalFilter = new GlobalFilterViewModel();
			OnSessionsChanged();
			OnFilteredSessionsChanged();
			RefreshFilters();
			RefreshSessions();
		}
		
		protected void OnSessionsChanged()
		{
			SessionsChanged?.Invoke(this,EventArgs.Empty);
		}
		protected void OnFilteredSessionsChanged()
		{
			FilteredSessionsChanged?.Invoke(this, EventArgs.Empty);
		}

		

		public void RefreshFilters()
		{
			string name;

			foreach(string? ipGroup in Model.Sessions.SelectMany(session => session.Calls).SelectMany(call => call.SBCReports).Select(item=>item.IPGroup).Distinct())
			{
				if (string.IsNullOrEmpty(ipGroup)) name = "N/A"; else name = ipGroup;
				if (GlobalFilter.IPGroupFilters.Select(filter=>filter.Name).Contains(ipGroup)) continue;
				GlobalFilter.IPGroupFilters.Add(new IPGroupFilterViewModel() { Name=name,Value=ipGroup });
			}

			foreach (string? sipInterface in Model.Sessions.SelectMany(session => session.Calls).SelectMany(call => call.SBCReports).Select(item=> item.SIPInterfaceId).Distinct())
			{
				if (string.IsNullOrEmpty(sipInterface)) name = "N/A"; else name = sipInterface;
				if (GlobalFilter.SIPInterfaceFilters.Select(filter => filter.Name).Contains(sipInterface)) continue;
				GlobalFilter.SIPInterfaceFilters.Add(new SIPInterfaceFilterViewModel() { Name = name, Value = sipInterface });
			}

			// filter REASON N/A
			foreach (string? termReason in Model.Sessions.SelectMany(session => session.Calls).SelectMany(call => call.SBCReports).Select(item => item.TrmReason).Where(item => (item!= "REASON N/A")).Distinct())
			{
				if (string.IsNullOrEmpty(termReason)) name = "N/A"; else name = termReason;
				if (GlobalFilter.TermReasonFilters.Select(filter => filter.Name).Contains(termReason)) continue;
				GlobalFilter.TermReasonFilters.Add(new TermReasonFilterViewModel() { Name =name,Value= termReason });
			}
		}


		public void RefreshSessions()
		{
			FilteredSessions = new SessionViewModelCollection( Model.Sessions,GlobalFilter );
			OnFilteredSessionsChanged();

		}


		private  async Task AddFileAsync(string FileName,
			ICDRSyslogParser CDRSyslogParser, ICDRReportParser CDRReportParser,
			IPacketLossSyslogParser PacketLossSyslogParser, IPacketLossReportParser PacketLossReportParser,
			IPacketReorderSyslogParser PacketReorderSyslogParser, IPacketReorderReportParser PacketReorderReportParser,
			IProgress<long> Progress)
		{
			FileStream? stream=null;
			StreamReader? reader=null;
			string? syslogLine=null;
			string? reportLine=null;
			CDRReport? CDRReport = null;
			PacketLossReport? packetLossReport=null;
			PacketReorderReport? packetReorderReport=null;
			long percent, oldPercent = -1;

			if (FileName == null) throw new ArgumentNullException(nameof(FileName));
			if (CDRSyslogParser == null) throw new ArgumentNullException(nameof(CDRSyslogParser));
			if (CDRReportParser == null) throw new ArgumentNullException(nameof(CDRReportParser));
			if (PacketLossSyslogParser == null) throw new ArgumentNullException(nameof(PacketLossSyslogParser));
			if (PacketLossReportParser == null) throw new ArgumentNullException(nameof(PacketLossReportParser));
			if (PacketReorderSyslogParser == null) throw new ArgumentNullException(nameof(PacketReorderSyslogParser));
			if (PacketReorderReportParser == null) throw new ArgumentNullException(nameof(PacketReorderReportParser));
			if (Progress == null) throw new ArgumentNullException(nameof(Progress));

			if (Model == null)
			{
				Log(LogLevels.Error, "Model is not loaded");
				throw new InvalidOperationException("Model is not loaded");
			}

			try
			{
				Try(() => new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite,4096, FileOptions.Asynchronous)).Then(result=>stream=result).OrThrow("Failed to open file");
				Try(() => new StreamReader(stream!)).Then(result=>reader=result).OrThrow("Failed to create reader");
				
				while (!reader!.EndOfStream)
				{
					//await reader.ReadLineAsync() 

					if (!await TryAsync(async () => await reader.ReadLineAsync()).Then(result => syslogLine = result).OrAlert("Error while reading line in file")) break;
					//await Task.Delay(100);
					//syslogLine = "test";

					percent = stream!.Position * 100 / stream.Length;
					if (percent > oldPercent)
					{
						oldPercent = percent;
						Progress.Report(percent);
					}

					// Trying to load CDR report
					if (Try(() => CDRSyslogParser.Parse(syslogLine)).Then(result => reportLine = result).OrAlert($"Failed to parse syslog line: {syslogLine}"))
					{
						if (reportLine != null)
						{
							if (!Try(() => CDRReportParser.Parse(reportLine)).Then(result => CDRReport = result).OrAlert($"Failed to parse report line: {reportLine}")) continue;
							if (CDRReport != null)
							{
								Model.AddCDRReport(CDRReport);
								continue;
							}
						}
					}

					// Trying to load PacketLoss report
					if (Try(() => PacketLossSyslogParser.Parse(syslogLine)).Then(result => reportLine = result).OrAlert($"Failed to parse syslog line: {syslogLine}"))
					{
						if (reportLine != null)
						{
							if (!Try(() => PacketLossReportParser.Parse(reportLine)).Then(result => packetLossReport = result).OrAlert($"Failed to parse report line: {reportLine}")) continue;
							if (packetLossReport != null)
							{
								Model.AddPacketLossReport(packetLossReport);
								continue;
							}
						}
					}


					// Trying to load PacketReorder report
					if (Try(() => PacketReorderSyslogParser.Parse(syslogLine)).Then(result => reportLine = result).OrAlert($"Failed to parse syslog line: {syslogLine}"))
					{
						if (reportLine != null)
						{
							if (!Try(() => PacketReorderReportParser.Parse(reportLine)).Then(result => packetReorderReport = result).OrAlert($"Failed to parse report line: {reportLine}")) continue;
							if (packetReorderReport != null)
							{
								Model.AddPacketReorderReport(packetReorderReport);
								continue;
							}
						}
					}

										

				}
			}
			finally
			{
				if (stream != null) stream.Dispose();
			}

				 
				

			
			
			
		}

		public async Task AddFilesAsync(IEnumerable<string> FileNames,IProgress<long> Progress)
		{
			ICDRSyslogParser CDRSyslogParser;
			ICDRReportParser CDRReportParser;
			IPacketLossSyslogParser PacketLossSyslogParser;
			IPacketLossReportParser PacketLossReportParser;
			IPacketReorderSyslogParser PacketReorderSyslogParser;
			IPacketReorderReportParser PacketReorderReportParser;
			int index,count;


			Log(LogLevels.Information, $"Adding syslog files to project");
			if (Model == null)
			{
				Log(LogLevels.Error, "Model is not loaded");
				throw new InvalidOperationException("Model is not loaded");
			}


			CDRSyslogParser = new CDRSyslogParser();
			CDRReportParser = new CDRReportParser(new DateTimeParser());
			PacketLossSyslogParser = new PacketLossSyslogParser();
			PacketLossReportParser = new PacketLossReportParser(new DateTimeParser());
			PacketReorderSyslogParser = new PacketReorderSyslogParser();
			PacketReorderReportParser = new PacketReorderReportParser(new DateTimeParser());

			index = 1;count = FileNames.Count();
			await foreach(string fileName in FileNames.AsAsyncEnumerable())
			{
				Log(LogLevels.Information, $"Loading file {fileName}");
				RunningTask = $"Loading file ({index}/{count})...";
				if (loadedFiles.Contains(fileName))
				{
					Log(LogLevels.Warning, $"Project already contains file {fileName}");
					continue;
				}
				loadedFiles.Add(fileName);
				await TryAsync(()=> AddFileAsync(fileName,
					CDRSyslogParser,CDRReportParser,
					PacketLossSyslogParser,PacketLossReportParser,
					PacketReorderSyslogParser,PacketReorderReportParser,
					Progress)).OrThrow("Failed to add file in project");
				index++;
			}

			PacketLossReports = new PacketLossReportViewModelCollection(Model.PacketLossReports);
			//Sessions = new SessionViewModelCollection(Model.Sessions);
				

			OnSessionsChanged();
			RefreshFilters();
			RefreshSessions();
			RunningTask = null;
			
		}

		private void ReadRTCP(IRTCPReader RTCPReader, ACDR ACDR,DateTime Timestamp)
		{
			IBigEndianReader bigEndianReader;
            SenderReport? senderReport = null;
            SourceDescription? sourceDescription;
            RTCPReport rtcpReport;
            string? sourceName;
            SDESItem sdesItem;

            bigEndianReader = new BigEndianReader(ACDR.Payload);

            if (!Try(() => RTCPReader.Read(bigEndianReader)).Then(result => senderReport = result as SenderReport).OrAlert("Failed to read RTCP")) return;
            if (senderReport == null) return; // not sender report

            if (senderReport.Header.ReceptionReportCount == 0) return;

            sourceDescription = RTCPReader.Read(bigEndianReader) as SourceDescription;
            if (sourceDescription == null) return; // not source description
            if (sourceDescription.Header.SourceCount == 0) return;

            sdesItem = sourceDescription.Chunks.SelectMany(chunk => chunk.Items).FirstOrDefault(item => item.Type == SDESItemTypes.CNAME);
            if (string.IsNullOrEmpty(sdesItem.Text)) sourceName = "anonymous";
            else sourceName = sdesItem.Text;

            rtcpReport = new RTCPReport()
            {
                SourceName = sourceName,
                SessionId = ACDR.FullSessionID.ToString(),
                TimeStamp =Timestamp,
                SSRC = senderReport.Header.SenderSSRC,
                Jitter = senderReport.ReceptionReports[0].InterarrivalJitter,
                PacketLossPercent = (byte)(senderReport.ReceptionReports[0].FractionLost * 100 / 255),
            };
            Model.AddRTCPReport(rtcpReport);
        }
        private void ReadRTP(IRTPReader RTPReader, ACDR ACDR)
        {
            IBigEndianReader bigEndianReader;
            RTP? rtp=default;

            bigEndianReader = new BigEndianReader(ACDR.Payload);

            if (!Try(() => RTPReader.Read(bigEndianReader)).Then(result => rtp = result ).OrAlert("Failed to read RTP")) return;
			if (rtp == null) return;

			Model.AddRTP(rtp);

            
        }

        private async Task AddDebugRecordingAsync(string FileName,
			IFrameReader FrameReader, IPacketReader PacketReader,
			IUDPSegmentReader UDPSegmentReader,
			IACDRReader ACDRReader, IRTCPReader RTCPReader,IRTPReader RTPReader,
			IProgress<long> Progress)
		{
			long percent, oldPercent = -1;
			Frame frame=new Frame();
			Packet packet=new Packet();
			UDPSegment udpSegment = new UDPSegment();
			ACDR acdr=new ACDR();
			IPcapReader pcapReader=new PcapReader();
			BinaryReader? binaryReader=null;
			FileHeader? header=null;
			PacketRecord? packetRecord = null;

            if (FileName == null) throw new ArgumentNullException(nameof(FileName));
			if (Progress == null) throw new ArgumentNullException(nameof(Progress));

			if (Model == null)
			{
				Log(LogLevels.Error, "Model is not loaded");
				throw new InvalidOperationException("Model is not loaded");
			}
			
			Try(() => new FileStream(FileName,FileMode.Open)).Then(result => binaryReader=new BinaryReader(result)).OrThrow("Failed to open file");
			if (binaryReader == null) return;

			Try(() => pcapReader.ReadHeader(binaryReader)).Then(result => header = result).OrThrow("Failed to read pcap file header");
			if (header == null) return;	
			while(binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
			{
				percent = binaryReader.BaseStream.Position* 100 / binaryReader.BaseStream.Length;
				if (percent > oldPercent)
				{
					oldPercent = percent;
					Progress.Report(percent);
				}
                


                if (! await TryAsync(()=> Task.FromResult(pcapReader.ReadPacketRecord(binaryReader))).Then(result=>packetRecord=result).OrAlert("Failed to read packet record")) continue;
				if (packetRecord == null) return;
					
                // Trying to load packet
                if (!Try(() => FrameReader.Read(packetRecord.PacketData)).Then(result => frame = result).OrAlert("Failed to read frame")) continue;
				if (!Try(() => PacketReader.Read(frame.Payload)).Then(result => packet = result).OrAlert("Failed to read packet")) continue;

				if (packet.Header.Protocol != Protocols.UDP) continue; // not acdr

				if (!Try(() => UDPSegmentReader.Read(packet.Payload)).Then(result => udpSegment = result).OrAlert("Failed to read segment")) continue;
				if (udpSegment.Header.DestinationPort != 925) continue; // not acdr

				if (!Try(() => ACDRReader.Read(udpSegment.Payload)).Then(result => acdr = result).OrAlert("Failed to read acdr")) continue;

									 
				if (acdr.Header.MediaType == MediaTypes.ACDR_RTCP) ReadRTCP(RTCPReader, acdr, header.GetTimeTimeUTC(packetRecord).ToLocalTime());

                if ( (acdr.Header.MediaType==MediaTypes.ACDR_RTP) && ((acdr.Header.TracePoint == TracePoints.VoipDecoder) || (acdr.Header.TracePoint == TracePoints.NetEncoder))) ReadRTP(RTPReader,acdr);

            }
			
		}

		public async Task AddDebugRecordingAsync(IEnumerable<string> FileNames, IProgress<long> Progress)
		{
			int index, count;
			IFrameReader frameReader;
			IPacketReader packetReader;
			IUDPSegmentReader udpSegmentReader;
			IACDRReader acdrReader;
            IRTPReader rtpReader;
            IRTCPReader rtcpReader;

            if (Model == null) throw new InvalidOperationException("Model is not loaded");

			frameReader = new FrameReader();
			packetReader = new PacketReader();
			udpSegmentReader=new UDPSegmentReader();
			acdrReader = new ACDRReader();
			rtpReader=new RTPReader();
			rtcpReader=new RTCPReader();

			index = 1; count = FileNames.Count();
			await foreach (string fileName in FileNames.AsAsyncEnumerable())
			{
				RunningTask = $"Loading file ({index}/{count})...";
				if (loadedDebugRecordingFiles.Contains(fileName)) continue;
				loadedDebugRecordingFiles.Add(fileName);

				await TryAsync(() => AddDebugRecordingAsync(fileName,
					frameReader,packetReader,udpSegmentReader,acdrReader,rtcpReader,rtpReader,
					Progress)).OrThrow($"Failed to read debug recording file {fileName}");
				index++;
			}

			PacketLossReports = new PacketLossReportViewModelCollection(Model.PacketLossReports);
			//Sessions = new SessionViewModelCollection(Model.Sessions);

			OnSessionsChanged();
			RefreshFilters();
			RefreshSessions();
			RunningTask = null;

		}

		public bool FindNext(MatchProperty Criteria,string Value)
		{
			int index;

			if (FilteredSessions.SelectedItem == null) index = -1;
			else index = FilteredSessions.IndexOf(FilteredSessions.SelectedItem);
			
			for(int t=index+1;t<FilteredSessions.Count;t++)
			{
				if (FilteredSessions[t].Match(Criteria,Value))
				{
					FilteredSessions.SelectedItem= FilteredSessions[t];
					return true;
				}
			}

			return false;
		}
		public bool FindPrevious(MatchProperty Criteria, string Value)
		{
			int index;

			if (FilteredSessions.SelectedItem == null) index = FilteredSessions.Count;
			else index = FilteredSessions.IndexOf(FilteredSessions.SelectedItem);

			for (int t = index - 1; t >= 0; t--)
			{
				if (FilteredSessions[t].Match(Criteria, Value))
				{
					FilteredSessions.SelectedItem = FilteredSessions[t];
					return true;
				}
			}
			return false;
		}

		public async Task SaveAsync(string Path)
		{
			if (Path == null) throw new ArgumentNullException(nameof(Path));
			if (Model==null) throw new InvalidOperationException("Model is not loaded");

			RunningTask = $"Saving project...";

			this.Path= Path;
			this.Name=System.IO.Path.GetFileName(Path);
			await TryAsync(() => Model.SaveAsync(Path)).OrThrow("Failed to save project file");
			RunningTask = null;

		}

		public static async Task<ProjectViewModel> LoadAsync(string Path)
		{
			Project project;
			ProjectViewModel projectViewModel;

			project = await Project.LoadAsync(Path);
			
			projectViewModel = new ProjectViewModel(project);
			
			return projectViewModel;
		}


	}
}
