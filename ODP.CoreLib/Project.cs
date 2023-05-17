using System.Reflection.Metadata;
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

		public async Task AddFileAsync(string FileName,
			ICDRSyslogParser CDRSyslogParser,ICDRReportParser CDRReportParser,
			IPacketLossSyslogParser PacketLossSyslogParser, IPacketLossReportParser PacketLossReportParser,
			IPacketReorderSyslogParser PacketReorderSyslogParser, IPacketReorderReportParser PacketReorderReportParser,
			IProgress<long> Progress)
		{
			string? syslogLine;
			string? reportLine;
			CDRReport? CDRReport;
			PacketLossReport packetLossReport;
			PacketReorderReport packetReorderReport;
			long percent,oldPercent=-1;

			if (FileName== null) throw new ArgumentNullException(nameof(FileName));
			if (CDRSyslogParser == null) throw new ArgumentNullException(nameof(CDRSyslogParser));
			if (CDRReportParser == null) throw new ArgumentNullException(nameof(CDRReportParser));
			if (PacketLossSyslogParser == null) throw new ArgumentNullException(nameof(PacketLossSyslogParser));
			if (PacketLossReportParser == null) throw new ArgumentNullException(nameof(PacketLossReportParser));
			if (PacketReorderSyslogParser == null) throw new ArgumentNullException(nameof(PacketReorderSyslogParser));
			if (PacketReorderReportParser == null) throw new ArgumentNullException(nameof(PacketReorderReportParser));
			if (Progress == null) throw new ArgumentNullException(nameof(Progress));

			using (FileStream stream = new FileStream(FileName, FileMode.Open,FileAccess.Read,FileShare.ReadWrite))
			{
				StreamReader reader= new StreamReader(stream);
				while (!reader.EndOfStream)
				{
					syslogLine = await reader.ReadLineAsync();

					percent = stream.Position * 100 / stream.Length;
					if (percent > oldPercent)
					{
						oldPercent = percent;
						Progress.Report(percent);
					}

					// Trying to load CDR report
					try
					{
						reportLine = CDRSyslogParser.Parse(syslogLine);
						
					}
					catch(Exception ex)
					{
						throw new Exception($"Failed to parse syslog line: {syslogLine}",ex);
					}

					if (reportLine != null)
					{
						try
						{
							CDRReport = CDRReportParser.Parse(reportLine);
							if (CDRReport!=null) AddCDRReport(CDRReport);
							continue;
						}
						catch (Exception ex)
						{
							throw new Exception($"Failed to parse report line: {reportLine}", ex);
						}
					}

					// Trying to load PacketLoss report
					try
					{
						reportLine = PacketLossSyslogParser.Parse(syslogLine);

					}
					catch (Exception ex)
					{
						throw new Exception($"Failed to parse syslog line: {syslogLine}", ex);
					}

					if (reportLine != null)
					{
						try
						{
							packetLossReport = PacketLossReportParser.Parse(reportLine);
							AddPacketLossReport(packetLossReport);
							continue;
						}
						catch (Exception ex)
						{
							throw new Exception($"Failed to parse report line: {reportLine}", ex);
						}
					}

					// Trying to load PacketReorder report
					try
					{
						reportLine = PacketReorderSyslogParser.Parse(syslogLine);

					}
					catch (Exception ex)
					{
						throw new Exception($"Failed to parse syslog line: {syslogLine}", ex);
					}

					if (reportLine != null)
					{
						try
						{
							packetReorderReport = PacketReorderReportParser.Parse(reportLine);
							AddPacketReorderReport(packetReorderReport);
							continue;
						}
						catch (Exception ex)
						{
							throw new Exception($"Failed to parse report line: {reportLine}", ex);
						}
					}

					// no report detected



				}

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