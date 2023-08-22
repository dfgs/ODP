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

		public List<CDRSBCReport> SBCReports
		{
			get;
			set;
		}
		public List<CDRMediaReport> MediaReports
		{
			get;
			set;
		}

		

		public Call()
		{
			SBCReports= new List<CDRSBCReport>();
			MediaReports= new List<CDRMediaReport>();
		}

		public void AddCDRReport(CDRReport Report)
		{

			if (Report == null) throw new ArgumentNullException(nameof(Report));

			switch (Report)
			{
				case CDRSBCReport sbcReport:
					SBCReports.Add(sbcReport);
					break;
				case CDRMediaReport mediaReport:
					MediaReports.Add(mediaReport);
					break;
			}

		}
		
		

	}
}
