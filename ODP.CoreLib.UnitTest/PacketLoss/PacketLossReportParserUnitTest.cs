using System.Globalization;
using System.Security.Cryptography;

namespace ODP.CoreLib.UnitTest
{
	[TestClass]
	public class PacketLossReportParserUnitTest
	{
		[TestMethod]
		public void ShouldCheckConstructorParameters()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => new PacketLossReportParser(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}//*/

		[DataTestMethod]
		[DataRow("17:15:52.048")]
		public void ShouldParseDateTime(string Line)
		{
			// HH:mm:ss.fff  zzz ddd MMM dd yyyy
			DateTime.ParseExact(Line, "HH:mm:ss.fff", CultureInfo.InvariantCulture);
		}

		[DataTestMethod]
		[DataRow("17:15:52.048  172.200.0.10  local0.warn    [S=1030955] [BID=c64c90:24]  Packets-Loss report [PL range]=#media-legs: [No PL]=1, [up to 0.5%]=2, [0.5% - 1%]=3, [1% - 2%]=4, [2% - 5%]=5, [5% - 100%]=6 [Time:20-02@17:15:50.930]\r\n")]
		public void ShouldParseValidReportLine(string Line)
		{
			PacketLossReportParser reportParser;
			PacketLossReport? report;

			reportParser = new PacketLossReportParser(new DateTimeParser());

			report = reportParser.Parse(Line);
			Assert.IsNotNull(report);
			Assert.AreEqual(17, report.ReportTime.Hour);
			Assert.AreEqual(15, report.ReportTime.Minute);
			Assert.AreEqual(52, report.ReportTime.Second);
			Assert.AreEqual(1u, report.CallsCountLevel0);
			Assert.AreEqual(2u, report.CallsCountLevel1);
			Assert.AreEqual(3u, report.CallsCountLevel2);
			Assert.AreEqual(4u, report.CallsCountLevel3);
			Assert.AreEqual(5u, report.CallsCountLevel4);
			Assert.AreEqual(6u, report.CallsCountLevel5);
		}



		[DataTestMethod]
		[DataRow("17:15:52.048  172.200.0.10  local0.warn    [S=1030955] [BID=c64c90:24]  Packets-Loss report [PL range]=#media-legs: [No PL]=1, [0.5% - 1%]=3, [1% - 2%]=4, [2% - 5%]=5, [5% - 100%]=6 [Time:20-02@17:15:50.930]\r\n")]
		[DataRow("172.200.0.10  local0.warn[S = 1030955][BID = c64c90:24]  Packets - Loss report[PL range] =#media-legs: [No PL]=1, [up to 0.5%]=2, [0.5% - 1%]=3, [1% - 2%]=4, [2% - 5%]=5, [5% - 100%]=6 [Time:20-02@17:15:50.930]\r\n" )]
		[DataRow("CALL_END       |SBC       |4341598405122022185752@10.0.2.11                                |c62532:22:625           |LCL  |10.0.2.11           |5060         |10.0.2.12           |5060       |TCP             |+33001@10.0.1.11                         |+33001@10.0.1.11                         |2001@10.0.1.12                           |2001@10.0.1.11                           |30      |RMT  |GWAPP_NORMAL_CALL_CLEAR                 |NORMAL_CALL_CLEAR|18:57:52.320  UTC Mon Dec 05 2022  |18:57:59.615  UTC Mon Dec 05 2022  |18:58:29.657  UTC Mon Dec 05 2022  |-1             |                                         |                                         |24             |SBC2                            |SRD1                            |WAN                             |SBC2                            |WAN                             |DefaultRealm                    |no         |BYE         |                          |                                                   |                                     |Normal  |2    | newcol | newcol")]
		public void ShouldNotParseInvalidReportLine(string Line)
		{
			PacketLossReportParser reportParser;

			reportParser = new PacketLossReportParser(new DateTimeParser());

			Assert.ThrowsException<InvalidDataException>(()=> reportParser.Parse(Line));
		}


		[TestMethod]
		public void ShouldNotParseNullReportLine()
		{
			PacketLossReportParser reportParser;

			reportParser = new PacketLossReportParser(new DateTimeParser());

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => reportParser.Parse(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

		}



	}
}