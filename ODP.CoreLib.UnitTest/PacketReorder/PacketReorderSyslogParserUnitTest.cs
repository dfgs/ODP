namespace ODP.CoreLib.UnitTest
{
	[TestClass]
	public class PacketReorderSyslogParserUnitTest
	{


		[DataTestMethod]
		[DataRow("2023-03-06 12:12:43 local0.warning 100.112.70.10  [S=41028913] [SID=cd91eb:43:20928800]  RTP packets reorder: CID=633 SeqNum=34649 LastSeqRecv=34650 SrcIP=213.190.78.157 SrcPort=50016 [Code:0x600b] [CID:633] [Time:06-03@12:13:27.833]", "2023-03-06 12:12:43 local0.warning 100.112.70.10  [S=41028913] [SID=cd91eb:43:20928800]  RTP packets reorder: CID=633 SeqNum=34649 LastSeqRecv=34650 SrcIP=213.190.78.157 SrcPort=50016 [Code:0x600b] [CID:633] [Time:06-03@12:13:27.833]")]
		public void ShouldParseValidReport(string Syslog,string ExpectedReportLine)
		{
			PacketReorderSyslogParser syslogParser;
			string? reportLine;

			syslogParser = new PacketReorderSyslogParser();

			reportLine = syslogParser.Parse(Syslog);
			Assert.IsNotNull(reportLine);
			Assert.AreEqual(ExpectedReportLine, reportLine);
		}

		[DataTestMethod]
		[DataRow("10:35:36.653  10.0.10.11  local1.info[S = 122] )")]
		[DataRow("")]
		[DataRow(null)]
		[DataRow("local0.warning 100.112.70.10  [S=41028913] [SID=cd91eb:43:20928800]  RTP packets: CID=633 SeqNum=34649 LastSeqRecv=34650 SrcIP=213.190.78.157 SrcPort=50016 [Code:0x600b] [CID:633] [Time:06-03@12:13:27.833]")]
		[DataRow("2023-03-06 12:12:43 local0.warning 100.112.70.10  [S=41028913] [SID=cd91eb:43:20928800]  packets reorder: CID=633 SeqNum=34649 LastSeqRecv=34650 SrcIP=213.190.78.157 SrcPort=50016 [Code:0x600b] [CID:633] [Time:06-03@12:13:27.833]")]
		public void ShouldNotParseInvalidReport(string Syslog)
		{
			PacketReorderSyslogParser syslogParser;
			string? reportLine;

			syslogParser = new PacketReorderSyslogParser();

			reportLine = syslogParser.Parse(Syslog);
			Assert.IsNull(reportLine);
		}


	}
}