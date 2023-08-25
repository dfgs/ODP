using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.CoreLib.UnitTest
{
	[TestClass]
	public class ProjectUnitTest
	{


		/*[TestMethod]
		public async Task AddFileAsyncShouldCheckParameters()
		{
			Project project;

			project = new Project();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => project.AddFileAsync(null, Mock.Of<CDRSyslogParser>(), new CDRReportParser(new DateTimeParser()), Mock.Of<PacketLossSyslogParser>(),new PacketLossReportParser(new DateTimeParser()), Mock.Of<PacketReorderSyslogParser>(), new PacketReorderReportParser(new DateTimeParser()), new Progress<long>()));
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => project.AddFileAsync("FileName", null, new CDRReportParser(new DateTimeParser()), Mock.Of<PacketLossSyslogParser>(), new PacketLossReportParser(new DateTimeParser()), Mock.Of<PacketReorderSyslogParser>(), new PacketReorderReportParser(new DateTimeParser()) ,new Progress<long>()));
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => project.AddFileAsync("FileName", Mock.Of<CDRSyslogParser>(), null, Mock.Of<PacketLossSyslogParser>(), new PacketLossReportParser(new DateTimeParser()), Mock.Of<PacketReorderSyslogParser>(), new PacketReorderReportParser(new DateTimeParser()),new Progress<long>()));
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => project.AddFileAsync("FileName", Mock.Of<CDRSyslogParser>(), new CDRReportParser(new DateTimeParser()), null, new PacketLossReportParser(new DateTimeParser()), Mock.Of<PacketReorderSyslogParser>(), new PacketReorderReportParser(new DateTimeParser()), new Progress<long>()));
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => project.AddFileAsync("FileName", Mock.Of<CDRSyslogParser>(), new CDRReportParser(new DateTimeParser()), Mock.Of<PacketLossSyslogParser>(), null, Mock.Of<PacketReorderSyslogParser>(), new PacketReorderReportParser(new DateTimeParser()) ,new Progress<long>()));
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => project.AddFileAsync(null, Mock.Of<CDRSyslogParser>(), new CDRReportParser(new DateTimeParser()), Mock.Of<PacketLossSyslogParser>(), new PacketLossReportParser(new DateTimeParser()), null, new PacketReorderReportParser(new DateTimeParser()), new Progress<long>()));
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => project.AddFileAsync(null, Mock.Of<CDRSyslogParser>(), new CDRReportParser(new DateTimeParser()), Mock.Of<PacketLossSyslogParser>(), new PacketLossReportParser(new DateTimeParser()), Mock.Of<PacketReorderSyslogParser>(), null, new Progress<long>()));
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => project.AddFileAsync("FileName", Mock.Of<CDRSyslogParser>(), new CDRReportParser(new DateTimeParser()), Mock.Of<PacketLossSyslogParser>(), new PacketLossReportParser(new DateTimeParser()), Mock.Of<PacketReorderSyslogParser>(), new PacketReorderReportParser(new DateTimeParser()), null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

		}*/

		[TestMethod]
		public void AddCDRReportShouldCheckParameter()
		{
			Project project;

			project = new Project();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => project.AddCDRReport(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}

		[TestMethod]
		public void AddCDRReportShouldAddSBCReport()
		{
			CDRReport report;
			Project project;


			report = new CDRSBCReport() { SessionId = "Session1", SIPCallId = "CallID1" };

			project = new Project();

			project.AddCDRReport(report);
			Assert.AreEqual(1, project.Sessions.Count);
			Assert.AreEqual("Session1", project.Sessions[0].SessionId);
			Assert.AreEqual(1, project.Sessions[0].Calls.Count);
			Assert.AreEqual("CallID1", project.Sessions[0].Calls[0].SIPCallId);
			Assert.AreEqual(1, project.Sessions[0].Calls[0].SBCReports.Count);
			Assert.AreEqual(0, project.Sessions[0].Calls[0].MediaReports.Count);
		}


		[TestMethod]
		public void AddCDRReportShouldAddMediaReport()
		{
			CDRReport report;
			Project project;


			report = new CDRMediaReport() { SessionId = "Session1", SIPCallId = "CallID1" };

			project = new Project();

			project.AddCDRReport(report);
			Assert.AreEqual(1, project.Sessions.Count);
			Assert.AreEqual("Session1", project.Sessions[0].SessionId);
			Assert.AreEqual(1, project.Sessions[0].Calls.Count);
			Assert.AreEqual("CallID1", project.Sessions[0].Calls[0].SIPCallId);
			Assert.AreEqual(0, project.Sessions[0].Calls[0].SBCReports.Count);
			Assert.AreEqual(1, project.Sessions[0].Calls[0].MediaReports.Count);
		}

		[TestMethod]
		public void AddSeveralCDRReportShouldAddDifferentsSessionsAndCalls()
		{
			CDRReport report;
			Project project;

			project = new Project();

			report = new CDRSBCReport() { SessionId = "Session1", SIPCallId = "CallID1" };
			project.AddCDRReport(report);
			Assert.AreEqual(1, project.Sessions.Count);
			Assert.AreEqual("Session1", project.Sessions[0].SessionId);
			Assert.AreEqual(1, project.Sessions[0].Calls.Count);
			Assert.AreEqual("CallID1", project.Sessions[0].Calls[0].SIPCallId);
			Assert.AreEqual(1, project.Sessions[0].Calls[0].SBCReports.Count);
			Assert.AreEqual(0, project.Sessions[0].Calls[0].MediaReports.Count);


			report = new CDRSBCReport() { SessionId = "Session2", SIPCallId = "CallID2" };
			project.AddCDRReport(report);
			Assert.AreEqual(2, project.Sessions.Count);
			Assert.AreEqual("Session2", project.Sessions[1].SessionId);
			Assert.AreEqual(1, project.Sessions[1].Calls.Count);
			Assert.AreEqual("CallID2", project.Sessions[1].Calls[0].SIPCallId);
			Assert.AreEqual(1, project.Sessions[1].Calls[0].SBCReports.Count);
			Assert.AreEqual(0, project.Sessions[1].Calls[0].MediaReports.Count);

			report = new CDRSBCReport() { SessionId = "Session2", SIPCallId = "CallID2" };
			project.AddCDRReport(report);
			Assert.AreEqual(2, project.Sessions.Count);
			Assert.AreEqual("Session2", project.Sessions[1].SessionId);
			Assert.AreEqual(1, project.Sessions[1].Calls.Count);
			Assert.AreEqual("CallID2", project.Sessions[1].Calls[0].SIPCallId);
			Assert.AreEqual(2, project.Sessions[1].Calls[0].SBCReports.Count);
			Assert.AreEqual(0, project.Sessions[1].Calls[0].MediaReports.Count);
		}

		[TestMethod]
		public void AddPacketLossReportShouldCheckParameter()
		{
			Project project;

			project = new Project();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => project.AddPacketLossReport(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}

		[TestMethod]
		public void AddPacketLossReportShouldAddPacketLossReport()
		{
			PacketLossReport report;
			Project project;


			report = new PacketLossReport() {  ReportTime=DateTime.MinValue, CallsCountLevel0 = 0, CallsCountLevel1 = 1, CallsCountLevel2 = 2, CallsCountLevel3 = 3, CallsCountLevel4 = 4, CallsCountLevel5 = 5 };

			project = new Project();

			project.AddPacketLossReport(report);
			Assert.AreEqual(1, project.PacketLossReports.Count);
			Assert.AreEqual(DateTime.MinValue, project.PacketLossReports[0].ReportTime);
			Assert.AreEqual(0u, project.PacketLossReports[0].CallsCountLevel0);
			Assert.AreEqual(1u, project.PacketLossReports[0].CallsCountLevel1);
			Assert.AreEqual(2u, project.PacketLossReports[0].CallsCountLevel2);
			Assert.AreEqual(3u, project.PacketLossReports[0].CallsCountLevel3);
			Assert.AreEqual(4u, project.PacketLossReports[0].CallsCountLevel4);
			Assert.AreEqual(5u, project.PacketLossReports[0].CallsCountLevel5);
		}


		[TestMethod]
		public void AddPacketReorderReportShouldCheckParameter()
		{
			Project project;

			project = new Project();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => project.AddPacketReorderReport(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}

		[TestMethod]
		public void AddPacketReorderReportShouldAddSession()
		{
			PacketReorderReport report;
			Project project;


			report = new PacketReorderReport() { SessionId = "Session1"};

			project = new Project();

			Assert.AreEqual(0, project.Sessions.Count);
			project.AddPacketReorderReport(report);
			Assert.AreEqual(1, project.Sessions.Count);
			Assert.AreEqual("Session1", project.Sessions[0].SessionId);
			Assert.AreEqual(1, project.Sessions[0].PacketReorderReports.Count);
		}

		[TestMethod]
		public void AddPacketReorderReportShouldUseExistingSession()
		{
			Session session;
			PacketReorderReport report;
			Project project;

			session = new Session() { SessionId = "Session1" };

			report = new PacketReorderReport() { SessionId = "Session1" };

			project = new Project();
			project.Sessions.Add(session);

			Assert.AreEqual(1, project.Sessions.Count);
			project.AddPacketReorderReport(report);
			Assert.AreEqual(1, project.Sessions.Count);
			Assert.AreEqual("Session1", project.Sessions[0].SessionId);
			Assert.AreEqual(1, project.Sessions[0].PacketReorderReports.Count);
		}


	}


}