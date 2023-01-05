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

		public void AddReport(Report Report)
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


		public async Task AddFileAsync(string FileName,ISyslogParser SyslogParser,IReportParser ReportParser)
		{
			string? syslogLine;
			string? reportLine;
			Report? report;

			if (FileName== null) throw new ArgumentNullException(nameof(FileName));
			if (SyslogParser == null) throw new ArgumentNullException(nameof(SyslogParser));
			if (ReportParser == null) throw new ArgumentNullException(nameof(ReportParser));

			using (FileStream stream = new FileStream(FileName, FileMode.Open))
			{
				StreamReader reader= new StreamReader(stream);
				while (!reader.EndOfStream)
				{
					syslogLine = await reader.ReadLineAsync();
					reportLine=SyslogParser.Parse(syslogLine);
					if (reportLine == null) continue;

					report=ReportParser.Parse(reportLine);
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
				serializer.Serialize(stream, this);
				await Task.Yield();
			}
		}


	}
}