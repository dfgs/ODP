﻿using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODP.CoreLib.UnitTest
{
	[TestClass]
	public class CallUnitTest
	{


		[TestMethod]
		public void AddReportShouldCheckParameter()
		{
			Call call;

			call= new Call();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => call.AddCDRReport(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

		}

		[TestMethod]
		public void AddReportShouldAddSBCReport()
		{
			CDRReport report;
			Call call;

			report = new CDRSBCReport() { SessionId="Session1",SIPCallId="CallID1" };

			call = new Call() { SIPCallId = "CallID1" };

			call.AddCDRReport(report);
			Assert.AreEqual(1, call.SBCReports.Count);
			Assert.AreEqual(0, call.MediaReports.Count);

		}
		[TestMethod]
		public void AddReportShouldAddMediaReport()
		{
			CDRReport report;
			Call call;

			report = new CDRMediaReport() { SessionId = "Session1", SIPCallId = "CallID1" };

			call = new Call() { SIPCallId = "CallID1" };

			call.AddCDRReport(report);
			Assert.AreEqual(0, call.SBCReports.Count);
			Assert.AreEqual(1, call.MediaReports.Count);

		}

	}


}