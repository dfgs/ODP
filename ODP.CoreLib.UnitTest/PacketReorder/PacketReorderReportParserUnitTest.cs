using System.Globalization;
using System.Security.Cryptography;

namespace ODP.CoreLib.UnitTest
{
	[TestClass]
	public class PacketReorderReportParserUnitTest
	{
		[TestMethod]
		public void ShouldCheckConstructorParameters()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => new PacketReorderReportParser(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}//*/

		[DataTestMethod]
		[DataRow("17:15:52.048")]
		[DataRow("2023-03-06 08:40:00")]
		public void ShouldParseDateTime(string Line)
		{
			// HH:mm:ss.fff
			DateTime.ParseExact(Line,new string[] { "yyyy-MM-dd HH:mm:ss", "HH:mm:ss.fff" }, CultureInfo.InvariantCulture);
		}

		[DataTestMethod]
		[DataRow("2023-03-06 12:12:43 local0.warning 100.112.70.10  [S=41028913] [SID=cd91eb:43:20928800]  RTP packets reorder: CID=633 SeqNum=34649 LastSeqRecv=34650 SrcIP=213.190.78.157 SrcPort=50016 [Code:0x600b] [CID:633] [Time:06-03@12:13:27.833]")]
		public void ShouldParseValidReportLine(string Line)
		{
			PacketReorderReportParser reportParser;
			PacketReorderReport? report;

			reportParser = new PacketReorderReportParser(new DateTimeParser());

			report = reportParser.Parse(Line);
			Assert.IsNotNull(report);
			Assert.AreEqual(12, report.ReportTime.Hour);
			Assert.AreEqual(12, report.ReportTime.Minute);
			Assert.AreEqual(43, report.ReportTime.Second);
			Assert.AreEqual("cd91eb:43:20928800", report.SessionId);
			Assert.AreEqual("213.190.78.157", report.SourceIP);
			Assert.AreEqual((ushort)50016, report.SourcePort);
			Assert.AreEqual(34649u, report.SequenceNumber);
			Assert.AreEqual(34650u, report.LastSequenceNumber);
		}



		[DataTestMethod]
		[DataRow("2023-03-06 12:12:43 local0.warning 100.112.70.10  [S=41028913] RTP packets reorder: CID=633 SeqNum=34649 LastSeqRecv=34650 SrcIP=213.190.78.157 SrcPort=50016 [Code:0x600b] [CID:633] [Time:06-03@12:13:27.833]")]
		[DataRow("2023-03-06 12:12:43 local0.warning 100.112.70.10  [S=41028913] [SID=cd91eb:43:20928800]  RTP packets reorder: CID=633 SeqNum=34649 LastSeqRecv=34650 SrcIP=213.78.157 SrcPort=50016 [Code:0x600b] [CID:633] [Time:06-03@12:13:27.833]")]
		[DataRow("2023-03-06 12:12:43 local0.warning 100.112.70.10  [S=41028913] [SID=cd91eb:43:20928800]  RTP packets reorder: CID=633 SeqNum=34649 LastSeqRecv=34650 SrcIP=213.190.78.157 SrcPort=abc [Code:0x600b] [CID:633] [Time:06-03@12:13:27.833]")]
		public void ShouldNotParseInvalidReportLine(string Line)
		{
			PacketReorderReportParser reportParser;

			reportParser = new PacketReorderReportParser(new DateTimeParser());

			Assert.ThrowsException<InvalidDataException>(()=> reportParser.Parse(Line));
		}


		[TestMethod]
		public void ShouldNotParseNullReportLine()
		{
			PacketReorderReportParser reportParser;

			reportParser = new PacketReorderReportParser(new DateTimeParser());

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => reportParser.Parse(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

		}



	}
}