using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.CoreLib
{
	public class Session
	{
		public string? SessionId { get; set; }

		public List<Call> Calls
		{
			get;
			set;
		}

		public Session()
		{
			Calls = new List<Call>();
		}

		public void AddReport(Report Report)
		{
			Call? call;

			if (Report == null) throw new ArgumentNullException(nameof(Report));


			call = Calls.FirstOrDefault(item => item.SIPCallId == Report.SIPCallId);
			if (call == null)
			{
				call = new Call();
				call.SIPCallId = Report.SIPCallId;
				Calls.Add(call);
			}

			call.AddReport(Report);

		}


	}
}
