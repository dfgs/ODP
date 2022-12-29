using System.Text.RegularExpressions;

namespace ODP.CoreLib
{
	public class Project
	{
		
		public string? Name { get; set; }


		public List<Report> Reports
		{
			get;
			set;
		}

		public Project()
		{
			Reports= new List<Report>(); 
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
					
					Reports.Add(report);
				}

			}
		}


	}
}