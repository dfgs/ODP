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


		[TestMethod]
		public async Task AddFileAsyncShouldCheckParameters()
		{
			Project project;

			project = new Project();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => project.AddFileAsync(null, Mock.Of<SyslogParser>(), new ReportParser(new DateTimeParser())));
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => project.AddFileAsync("FileName", null, new ReportParser(new DateTimeParser())));
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => project.AddFileAsync("FileName", Mock.Of<SyslogParser>(), null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

		}

		[TestMethod]
		public void AddReportShouldCheckParameter()
		{
			Project project;

			project = new Project();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => project.AddReport(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}

		[TestMethod]
		public void AddReportShouldAddSBCReport()
		{
			Report report;
			Project project;


			report = new SBCReport() { SessionId = "Session1", SIPCallId = "CallID1" };

			project = new Project();

			project.AddReport(report);
			Assert.AreEqual(1, project.Sessions.Count);
			Assert.AreEqual("Session1", project.Sessions[0].SessionId);
			Assert.AreEqual(1, project.Sessions[0].Calls.Count);
			Assert.AreEqual("CallID1", project.Sessions[0].Calls[0].SIPCallId);
			Assert.AreEqual(1, project.Sessions[0].Calls[0].SBCReports.Count);
			Assert.AreEqual(0, project.Sessions[0].Calls[0].MediaReports.Count);
		}


		[TestMethod]
		public void AddReportShouldAddMediaReport()
		{
			Report report;
			Project project;


			report = new MediaReport() { SessionId = "Session1", SIPCallId = "CallID1" };

			project = new Project();

			project.AddReport(report);
			Assert.AreEqual(1, project.Sessions.Count);
			Assert.AreEqual("Session1", project.Sessions[0].SessionId);
			Assert.AreEqual(1, project.Sessions[0].Calls.Count);
			Assert.AreEqual("CallID1", project.Sessions[0].Calls[0].SIPCallId);
			Assert.AreEqual(0, project.Sessions[0].Calls[0].SBCReports.Count);
			Assert.AreEqual(1, project.Sessions[0].Calls[0].MediaReports.Count);
		}

		[TestMethod]
		public void AddSeveralReportShouldAddDifferentsSessionsAndCalls()
		{
			Report report;
			Project project;

			project = new Project();

			report = new SBCReport() { SessionId = "Session1", SIPCallId = "CallID1" };
			project.AddReport(report);
			Assert.AreEqual(1, project.Sessions.Count);
			Assert.AreEqual("Session1", project.Sessions[0].SessionId);
			Assert.AreEqual(1, project.Sessions[0].Calls.Count);
			Assert.AreEqual("CallID1", project.Sessions[0].Calls[0].SIPCallId);
			Assert.AreEqual(1, project.Sessions[0].Calls[0].SBCReports.Count);
			Assert.AreEqual(0, project.Sessions[0].Calls[0].MediaReports.Count);


			report = new SBCReport() { SessionId = "Session2", SIPCallId = "CallID2" };
			project.AddReport(report);
			Assert.AreEqual(2, project.Sessions.Count);
			Assert.AreEqual("Session2", project.Sessions[1].SessionId);
			Assert.AreEqual(1, project.Sessions[1].Calls.Count);
			Assert.AreEqual("CallID2", project.Sessions[1].Calls[0].SIPCallId);
			Assert.AreEqual(1, project.Sessions[1].Calls[0].SBCReports.Count);
			Assert.AreEqual(0, project.Sessions[1].Calls[0].MediaReports.Count);

			report = new SBCReport() { SessionId = "Session2", SIPCallId = "CallID2" };
			project.AddReport(report);
			Assert.AreEqual(2, project.Sessions.Count);
			Assert.AreEqual("Session2", project.Sessions[1].SessionId);
			Assert.AreEqual(1, project.Sessions[1].Calls.Count);
			Assert.AreEqual("CallID2", project.Sessions[1].Calls[0].SIPCallId);
			Assert.AreEqual(2, project.Sessions[1].Calls[0].SBCReports.Count);
			Assert.AreEqual(0, project.Sessions[1].Calls[0].MediaReports.Count);
		}


	}


}