using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.CoreLib.UnitTest
{
	[TestClass]
	public class SessionUnitTest
	{


		[TestMethod]
		public void AddReportShouldCheckParameter()
		{
			Session session;

			session= new Session();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => session.AddReport(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

		}

		[TestMethod]
		public void AddReportShouldAddSBCReport()
		{
			CDRReport report;
			Session session;


			report = new CDRSBCReport() { SessionId = "Session1", SIPCallId = "CallID1" };

			session = new Session() { SessionId = "Session1" };

			session.AddReport(report);
			Assert.AreEqual(1, session.Calls.Count);
			Assert.AreEqual("CallID1", session.Calls[0].SIPCallId);
			Assert.AreEqual(1, session.Calls[0].SBCReports.Count);
			Assert.AreEqual(0, session.Calls[0].MediaReports.Count);

		}
		[TestMethod]
		public void AddReportShouldAddMediaReport()
		{
			CDRReport report;
			Session session;


			report = new CDRMediaReport() { SessionId = "Session1", SIPCallId = "CallID1" };

			session = new Session() { SessionId = "Session1" };

			session.AddReport(report);
			Assert.AreEqual(1, session.Calls.Count);
			Assert.AreEqual(0, session.Calls[0].SBCReports.Count);
			Assert.AreEqual(1, session.Calls[0].MediaReports.Count);

		}
		[TestMethod]
		public void AddSeveralReportShouldAddDifferentsCalls()
		{
			CDRReport report;
			Session session;

			session = new Session() { SessionId = "Session1" };


			report = new CDRSBCReport() { SessionId = "Session1", SIPCallId = "CallID1" };

			session.AddReport(report);
			Assert.AreEqual(1, session.Calls.Count);
			Assert.AreEqual("CallID1", session.Calls[0].SIPCallId);
			Assert.AreEqual(1, session.Calls[0].SBCReports.Count);
			Assert.AreEqual(0, session.Calls[0].MediaReports.Count);

			report = new CDRSBCReport() { SessionId = "Session1", SIPCallId = "CallID1" };

			session.AddReport(report);
			Assert.AreEqual(1, session.Calls.Count);
			Assert.AreEqual("CallID1", session.Calls[0].SIPCallId);
			Assert.AreEqual(2, session.Calls[0].SBCReports.Count);
			Assert.AreEqual(0, session.Calls[0].MediaReports.Count);
			
			report = new CDRSBCReport() { SessionId = "Session1", SIPCallId = "CallID2" };

			session.AddReport(report);
			Assert.AreEqual(2, session.Calls.Count);
			Assert.AreEqual("CallID2", session.Calls[1].SIPCallId);
			Assert.AreEqual(1, session.Calls[1].SBCReports.Count);
			Assert.AreEqual(0, session.Calls[1].MediaReports.Count);
		}



	}


}