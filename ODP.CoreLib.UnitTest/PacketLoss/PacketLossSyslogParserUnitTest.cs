namespace ODP.CoreLib.UnitTest
{
	[TestClass]
	public class PacketLossSyslogParserUnitTest
	{


		[DataTestMethod]
		[DataRow("17:15:52.048  172.200.0.10  local0.warn    [S=1030955] [BID=c64c90:24]  Packets-Loss report [PL range]=#media-legs: [No PL]=17, [up to 0.5%]=0, [0.5% - 1%]=1, [1% - 2%]=0, [2% - 5%]=0, [5% - 100%]=0 [Time:20-02@17:15:50.930]", "17:15:52.048  172.200.0.10  local0.warn    [S=1030955] [BID=c64c90:24]  Packets-Loss report [PL range]=#media-legs: [No PL]=17, [up to 0.5%]=0, [0.5% - 1%]=1, [1% - 2%]=0, [2% - 5%]=0, [5% - 100%]=0 [Time:20-02@17:15:50.930]")]
		[DataRow("2023-03-06 08:40:00 local0.warning 100.112.70.10  [S=40600129][BID=cd91eb:43] Packets-Loss report[PL range] =#media-legs: [No PL]=64, [up to 0.5%]=1, [0.5% - 1%]=0, [1% - 2%]=0, [2% - 5%]=0, [5% - 100%]=1 [Time:06-03@08:40:45.035]", "2023-03-06 08:40:00 local0.warning 100.112.70.10  [S=40600129][BID=cd91eb:43] Packets-Loss report[PL range] =#media-legs: [No PL]=64, [up to 0.5%]=1, [0.5% - 1%]=0, [1% - 2%]=0, [2% - 5%]=0, [5% - 100%]=1 [Time:06-03@08:40:45.035]")]
		public void ShouldParseValidReport(string Syslog,string ExpectedReportLine)
		{
			PacketLossSyslogParser syslogParser;
			string? reportLine;

			syslogParser = new PacketLossSyslogParser();

			reportLine = syslogParser.Parse(Syslog);
			Assert.IsNotNull(reportLine);
			Assert.AreEqual(ExpectedReportLine, reportLine);
		}

		[DataTestMethod]
		[DataRow("10:35:36.653  10.0.10.11  local1.info[S = 122] )")]
		[DataRow("")]
		[DataRow(null)]
		[DataRow("2023-01-10 10:55:08 local3.notice 100.112.70.10  [S=23795891] [SID=cd91eb:42:7998878]  31 185.221.88.103 42436 typ host\r\na=candidate:882788167 2 udp 2130706430 185.221.88.103 42437 typ host\r\na=rtpmap:104 SILK/16000\r\na=fmtp:104 maxaveragebitrate=50000; useinbandfec=0; minptime=20\r\na=rtpmap:0 PCMU/8000\r\na=rtpmap:13 CN/8000\r\na=crypto:1 AES_CM_128_HMAC_SHA1_80 inline:agsbgxh8qa/r+VPfy1dwhRxSUw3uNyv1pOwhCDet|2^31\r\na=crypto:2 AES_CM_128_HMAC_SHA1_32 inline:6mCe4hfZCghah2q7VyzwOomoNMhHe+yX9fZkH9SI|2^31\r\na=crypto:3 AES_256_CM_HMAC_SHA1_80 inline:1kD4bZOXbuFPFbmqYagUWqDWABqGMuoLtkTd5/M8u10RTi7PACgb62OdLxv9dg==|2^31\r\na=crypto:4 AES_256_CM_HMAC_SHA1_32 inline:ow9OLLf2dnOigwqXH4E1GI5CZI6jCQvbq2FvM/F8Oi5WbjUkVDLHXUTH9X2ztA==|2^31\r\n [Time:10-01@10:55:07.699]")]
		[DataRow("14:35:40.053  10.0.10.11  local1.info    [S=1] |SBCReportType  |EPTyp     |SIPCallId                                                       |SessionId               |Orig |SourceIp            |SourcePort   |DestIp              |DestPort   |TransportType   |SrcURI                                   |SrcURIBeforeMap                          |DstURI                                   |DstURIBeforeMap                          |Duration|TrmSd|TrmReason                               |TrmReasonCategory|SetupTime                          |ConnectTime                        |ReleaseTime                        |RedirectReason |RedirectURINum                           |RedirectURINumBeforeMap                  |TxSigIPDiffServ|IPGroup (name)                  |SrdId (name)                    |SIPInterfaceId (name)           |ProxySetId (name)               |IpProfileId (name)              |MediaRealmId (name)             |DirectMedia|SIPTrmReason|SIPTermDesc               |Caller                                             |Callee                               |Trigger |LegId|VoiceAIConnectorName")]
		public void ShouldNotParseInvalidReport(string Syslog)
		{
			PacketLossSyslogParser syslogParser;
			string? reportLine;

			syslogParser = new PacketLossSyslogParser();

			reportLine = syslogParser.Parse(Syslog);
			Assert.IsNull(reportLine);
		}


	}
}