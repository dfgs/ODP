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


		public Project()
		{
			Sessions= new List<Session>();
		}

		public void AddReport(CDRReport Report)
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
			session.AddReport(Report);

		}


		public async Task AddFileAsync(string FileName,ICDRSyslogParser SyslogParser,ICDRReportParser ReportParser, IProgress<long> Progress)
		{
			string? syslogLine;
			string? reportLine;
			CDRReport? report;
			long percent,oldPercent=-1;

			if (FileName== null) throw new ArgumentNullException(nameof(FileName));
			if (SyslogParser == null) throw new ArgumentNullException(nameof(SyslogParser));
			if (ReportParser == null) throw new ArgumentNullException(nameof(ReportParser));
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
					//await Task.Delay(10);
					try
					{
						reportLine = SyslogParser.Parse(syslogLine);
					}
					catch(Exception ex)
					{
						throw new Exception($"Failed to parse syslog line: {syslogLine}",ex);
					}
					if (reportLine == null) continue;
					try
					{
						report = ReportParser.Parse(reportLine);
					}
					catch(Exception ex)
					{
						throw new Exception($"Failed to parse report line: {reportLine}", ex);
					}
					if (report == null) continue;
					AddReport(report);
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