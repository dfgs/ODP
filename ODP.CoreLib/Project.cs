using BigEndianReaderLib;
using EthernetFrameReaderLib;
using RTCPFrameReaderLib;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace ODP.CoreLib
{
	public class Project
	{
		
		public List<Session> Sessions
		{
			get;
			set;
		}

		public List<PacketLossReport> PacketLossReports
		{
			get;
			set;
		}


		public Project()
		{
			Sessions= new List<Session>();
			PacketLossReports= new List<PacketLossReport>();
		}

		public void AddCDRReport(CDRReport Report)
		{
			Session? session;
			
			if (Report==null) throw new ArgumentNullException(nameof(Report));

			session = Sessions.FirstOrDefault(item => item.SessionId == Report.SessionId);
			if (session==null)
			{
				session = new Session();
				session.SessionId= Report.SessionId;
				Sessions.Add(session);
			}
			session.AddCDRReport(Report);

		}
		public void AddRTCPReport(RTCPReport Report)
		{
			Session? session;

			if (Report == null) throw new ArgumentNullException(nameof(Report));

			session = Sessions.FirstOrDefault(item => string.Equals(item.SessionId, Report.SessionId, StringComparison.OrdinalIgnoreCase));
			if (session == null) return;
			
			session.AddRTCPReport(Report);

		}

		public void AddPacketReorderReport(PacketReorderReport Report)
		{
			Session? session;

			if (Report == null) throw new ArgumentNullException(nameof(Report));

			session = Sessions.FirstOrDefault(item => Report.SessionId == item.SessionId );
			if (session == null)
			{
				session = new Session();
				session.SessionId = Report.SessionId;
				Sessions.Add(session);
			}
			session.AddPacketReorderReport(Report);

		}

		public void AddPacketLossReport(PacketLossReport Report)
		{
			if (Report == null) throw new ArgumentNullException(nameof(Report));
			PacketLossReports.Add(Report);
		}

		


		public async Task AddWiresharkFileAsync(string FileName,
			IFrameReader FrameReader,IPacketReader PacketReader, 
			IUDPSegmentReader UDPSegmentReader,
			IACDRReader ACDRReader,IRTCPReader RTCPReader,
			IProgress<long> Progress)
		{
			long percent, oldPercent = -1;
			Frame frame;
			Packet packet;
			UDPSegment udpSegment;
			ACDR acdr;
			IBigEndianReader bigEndianReader;
			
			SenderReport? senderReport ;
			SourceDescription? sourceDescription ;
			RTCPReport rtcpReport;
			string? sourceName;
			SDESItem sdesItem;

			if (FileName == null) throw new ArgumentNullException(nameof(FileName));
			if (Progress == null) throw new ArgumentNullException(nameof(Progress));

			using (PcapngFile.Reader reader = new PcapngFile.Reader(FileName))
			{
				await foreach (var block in reader.EnhancedPacketBlocks.AsAsyncEnumerable())
				{
					if (reader.BaseStreamPosition.HasValue && reader.BaseStreamLength.HasValue)	percent = reader.BaseStreamPosition.Value * 100 / reader.BaseStreamLength.Value;
					else percent = 0;

					if (percent > oldPercent)
					{
						oldPercent = percent;
						Progress.Report(percent);
					}

					// Trying to load packet
					try
					{
						frame = FrameReader.Read(block.Data);
						packet = PacketReader.Read(frame.Payload);

						if (packet.Header.Protocol != Protocols.UDP) continue; // not acdr

						udpSegment = UDPSegmentReader.Read(packet.Payload);
						if (udpSegment.Header.DestinationPort != 925) continue; // not acdr

						acdr = ACDRReader.Read(udpSegment.Payload);

						if (acdr.Header.MediaType != MediaTypes.ACDR_RTCP) continue; // not rtcp

						bigEndianReader=new BigEndianReader(acdr.Payload);

						senderReport = RTCPReader.Read(bigEndianReader) as SenderReport;
						if (senderReport == null) continue; // not sender report
						if (senderReport.Header.ReceptionReportCount == 0) continue;

						sourceDescription = RTCPReader.Read(bigEndianReader) as SourceDescription;
						if (sourceDescription == null) continue; // not source description
						if (sourceDescription.Header.SourceCount == 0) continue;

						sdesItem=sourceDescription.Chunks.SelectMany(chunk=>chunk.Items).FirstOrDefault(item=>item.Type==SDESItemTypes.CNAME);
						if (string.IsNullOrEmpty(sdesItem.Text)) sourceName = "anonymous";
						else sourceName = sdesItem.Text;

						rtcpReport = new RTCPReport() {
							SourceName = sourceName,
							SessionId = acdr.FullSessionID.ToString(),
							TimeStamp = block.GetTimestamp().ToLocalTime(),
							SSRC = senderReport.Header.SenderSSRC,
							Jitter = senderReport.ReceptionReports[0].InterarrivalJitter,
							PacketLossPercent = (byte)(senderReport.ReceptionReports[0].FractionLost * 100 / 255),
						};
					}
					catch (Exception ex)
					{
						throw new Exception($"Failed to read packet block", ex);
					}

					AddRTCPReport(rtcpReport);

				}

				reader.Reset();

			}
		}

		public async Task SaveAsync(string Path)
		{
			XmlSerializer serializer;

			serializer = new XmlSerializer(typeof(Project));
			using (FileStream stream = new FileStream(Path, FileMode.OpenOrCreate))
			{
				await Task.Run(() => serializer.Serialize(stream, this));
			}
		}
		public static async Task<Project> LoadAsync(string Path)
		{
			XmlSerializer serializer;
			Project result;
			object? data;

			serializer = new XmlSerializer(typeof(Project));
			using (FileStream stream = new FileStream(Path, FileMode.Open))
			{
				data = await Task.Run<object?>(() => serializer.Deserialize(stream));
				if (data == null) throw new InvalidOperationException("Failed to deserialize project");
				result =(Project)data;
			}
			return result;
		}


	}
}