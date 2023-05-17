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
		public void AddPacketReorderReportShouldCheckParameter()
		{
			Session session;

			session= new Session();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => session.AddPacketReorderReport(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

		}
		[TestMethod]
		public void AddCDRReportShouldCheckParameter()
		{
			Session session;

			session = new Session();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => session.AddCDRReport(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

		}

		[TestMethod]
		public void AddPacketReorderReportShouldAddReport()
		{
			PacketReorderReport report;
			Session session;


			report = new PacketReorderReport() { SessionId = "Session1"};

			session = new Session() { SessionId = "Session1" };

			session.AddPacketReorderReport(report);
			Assert.AreEqual(1, session.PacketReorderReports.Count);
			Assert.AreEqual("Session1", session.PacketReorderReports[0].SessionId);
		}

		[TestMethod]
		public void AddCDRReportShouldAddSBCReport()
		{
			CDRReport report;
			Session session;


			report = new CDRSBCReport() { SessionId = "Session1", SIPCallId = "CallID1" };

			session = new Session() { SessionId = "Session1" };

			session.AddCDRReport(report);
			Assert.AreEqual(1, session.Calls.Count);
			Assert.AreEqual("CallID1", session.Calls[0].SIPCallId);
			Assert.AreEqual(1, session.Calls[0].SBCReports.Count);
			Assert.AreEqual(0, session.Calls[0].MediaReports.Count);

		}
		[TestMethod]
		public void AddCDRReportShouldAddMediaReport()
		{
			CDRReport report;
			Session session;


			report = new CDRMediaReport() { SessionId = "Session1", SIPCallId = "CallID1" };

			session = new Session() { SessionId = "Session1" };

			session.AddCDRReport(report);
			Assert.AreEqual(1, session.Calls.Count);
			Assert.AreEqual(0, session.Calls[0].SBCReports.Count);
			Assert.AreEqual(1, session.Calls[0].MediaReports.Count);

		}
		[TestMethod]
		public void AddSeveralCDRReportShouldAddDifferentsCalls()
		{
			CDRReport report;
			Session session;

			session = new Session() { SessionId = "Session1" };


			report = new CDRSBCReport() { SessionId = "Session1", SIPCallId = "CallID1" };

			session.AddCDRReport(report);
			Assert.AreEqual(1, session.Calls.Count);
			Assert.AreEqual("CallID1", session.Calls[0].SIPCallId);
			Assert.AreEqual(1, session.Calls[0].SBCReports.Count);
			Assert.AreEqual(0, session.Calls[0].MediaReports.Count);

			report = new CDRSBCReport() { SessionId = "Session1", SIPCallId = "CallID1" };

			session.AddCDRReport(report);
			Assert.AreEqual(1, session.Calls.Count);
			Assert.AreEqual("CallID1", session.Calls[0].SIPCallId);
			Assert.AreEqual(2, session.Calls[0].SBCReports.Count);
			Assert.AreEqual(0, session.Calls[0].MediaReports.Count);
			
			report = new CDRSBCReport() { SessionId = "Session1", SIPCallId = "CallID2" };

			session.AddCDRReport(report);
			Assert.AreEqual(2, session.Calls.Count);
			Assert.AreEqual("CallID2", session.Calls[1].SIPCallId);
			Assert.AreEqual(1, session.Calls[1].SBCReports.Count);
			Assert.AreEqual(0, session.Calls[1].MediaReports.Count);
		}



	}


}