using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.CoreLib
{
	public class Call
	{
		public string? SIPCallId { get; set; }

		public List<SBCReport> SBCReports
		{
			get;
			set;
		}
		public List<MediaReport> MediaReports
		{
			get;
			set;
		}
		public Call()
		{
			SBCReports= new List<SBCReport>();
			MediaReports= new List<MediaReport>();
		}

		public void AddReport(Report Report)
		{

			if (Report == null) throw new ArgumentNullException(nameof(Report));

			switch (Report)
			{
				case SBCReport sbcReport:
					SBCReports.Add(sbcReport);
					break;
				case MediaReport mediaReport:
					MediaReports.Add(mediaReport);
					break;
			}

		}



	}
}
