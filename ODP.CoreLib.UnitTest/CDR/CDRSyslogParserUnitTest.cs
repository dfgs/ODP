namespace ODP.CoreLib.UnitTest
{
	[TestClass]
	public class CDRSyslogParserUnitTest
	{


		[DataTestMethod]
		[DataRow("10:35:36.653  10.0.10.11  local1.info[S = 122] |CALL_START     |SBC       |4341598405122022185752@10.0.2.11                                |c62532:22:625           |LCL  |10.0.2.11           |5060         |10.0.2.12           |5060       |TCP             |+33001@10.0.1.11                         |+33001@10.0.1.11                         |2001@10.0.1.12                           |2001@10.0.1.11                           |0       |UNKN |REASON N/A                              |UNKNOWN          |18:57:52.320  UTC Mon Dec 05 2022  |                                   |                                   |-1             |                                         |                                         |24             |SBC2                            |SRD1                            |WAN                             |SBC2                            |WAN                             |DefaultRealm                    |no         |            |                          |                                                   |                                     |Normal  |2    |", "CALL_START     |SBC       |4341598405122022185752@10.0.2.11                                |c62532:22:625           |LCL  |10.0.2.11           |5060         |10.0.2.12           |5060       |TCP             |+33001@10.0.1.11                         |+33001@10.0.1.11                         |2001@10.0.1.12                           |2001@10.0.1.11                           |0       |UNKN |REASON N/A                              |UNKNOWN          |18:57:52.320  UTC Mon Dec 05 2022  |                                   |                                   |-1             |                                         |                                         |24             |SBC2                            |SRD1                            |WAN                             |SBC2                            |WAN                             |DefaultRealm                    |no         |            |                          |                                                   |                                     |Normal  |2    |")]
		[DataRow("10:35:43.991  10.0.10.11  local1.info[S = 123] |CALL_CONNECT   |SBC       |0f522c2a55774a12                                                |c62532:22:625           |RMT  |10.0.1.1            |53848        |10.0.1.11           |5060       |TCP             |+33001@10.0.1.11                         |1001@10.0.1.11                           |2001@10.0.1.11                           |2001@10.0.1.11                           |0       |UNKN |REASON N/A                              |UNKNOWN          |18:57:52.320  UTC Mon Dec 05 2022  |18:57:59.616  UTC Mon Dec 05 2022  |                                   |-1             |                                         |                                         |24             |LAN                             |SRD1                            |LAN                             |LAN                             |LAN                             |DefaultRealm                    |no         |            |                          |                                                   |                                     |Normal  |1    |", "CALL_CONNECT   |SBC       |0f522c2a55774a12                                                |c62532:22:625           |RMT  |10.0.1.1            |53848        |10.0.1.11           |5060       |TCP             |+33001@10.0.1.11                         |1001@10.0.1.11                           |2001@10.0.1.11                           |2001@10.0.1.11                           |0       |UNKN |REASON N/A                              |UNKNOWN          |18:57:52.320  UTC Mon Dec 05 2022  |18:57:59.616  UTC Mon Dec 05 2022  |                                   |-1             |                                         |                                         |24             |LAN                             |SRD1                            |LAN                             |LAN                             |LAN                             |DefaultRealm                    |no         |            |                          |                                                   |                                     |Normal  |1    |")]
		[DataRow("10:36:14.064  10.0.10.11  local1.info[S = 129] |CALL_END       |SBC       |4341598405122022185752@10.0.2.11                                |c62532:22:625           |LCL  |10.0.2.11           |5060         |10.0.2.12           |5060       |TCP             |+33001@10.0.1.11                         |+33001@10.0.1.11                         |2001@10.0.1.12                           |2001@10.0.1.11                           |30      |RMT  |GWAPP_NORMAL_CALL_CLEAR                 |NORMAL_CALL_CLEAR|18:57:52.320  UTC Mon Dec 05 2022  |18:57:59.615  UTC Mon Dec 05 2022  |18:58:29.657  UTC Mon Dec 05 2022  |-1             |                                         |                                         |24             |SBC2                            |SRD1                            |WAN                             |SBC2                            |WAN                             |DefaultRealm                    |no         |BYE         |                          |                                                   |                                     |Normal  |2    |", "CALL_END       |SBC       |4341598405122022185752@10.0.2.11                                |c62532:22:625           |LCL  |10.0.2.11           |5060         |10.0.2.12           |5060       |TCP             |+33001@10.0.1.11                         |+33001@10.0.1.11                         |2001@10.0.1.12                           |2001@10.0.1.11                           |30      |RMT  |GWAPP_NORMAL_CALL_CLEAR                 |NORMAL_CALL_CLEAR|18:57:52.320  UTC Mon Dec 05 2022  |18:57:59.615  UTC Mon Dec 05 2022  |18:58:29.657  UTC Mon Dec 05 2022  |-1             |                                         |                                         |24             |SBC2                            |SRD1                            |WAN                             |SBC2                            |WAN                             |DefaultRealm                    |no         |BYE         |                          |                                                   |                                     |Normal  |2    |")]
		[DataRow("10:36:14.064  10.0.10.11  local1.info[S = 125] |MediaReportType|SIPCallId                                                       |SessionId               |Cid  |MediaType |Coder          |Intrv|LocalRtpIp          |LocalRtpPort   |RemoteRtpIp         |RemoteRtpPort|InPackets |OutPackets|LocalPackLoss|RemotePackLoss|RTPdelay |RTPjitter|TxRTPssrc     |RxRTPssrc     |LocalRFactor|RemoteRFactor|LocalMosCQ|RemoteMosCQ|TxRTPIPDiffServ|LatchedRtpIp        |LatchedRtpPort |LatchedT38Ip        |LatchedT38Port |CoderTranscoding |LegId", "MediaReportType|SIPCallId                                                       |SessionId               |Cid  |MediaType |Coder          |Intrv|LocalRtpIp          |LocalRtpPort   |RemoteRtpIp         |RemoteRtpPort|InPackets |OutPackets|LocalPackLoss|RemotePackLoss|RTPdelay |RTPjitter|TxRTPssrc     |RxRTPssrc     |LocalRFactor|RemoteRFactor|LocalMosCQ|RemoteMosCQ|TxRTPIPDiffServ|LatchedRtpIp        |LatchedRtpPort |LatchedT38Ip        |LatchedT38Port |CoderTranscoding |LegId")]
		[DataRow("10:48:42.539  10.0.10.11  local1.info[S = 144] |MEDIA_END      |14575679645122022191045@10.0.2.11                               |c62532:22:710           |10   |AUDIO     |g711Ulaw64k    |20   |10.0.2.11           |6000           |10.0.2.12           |6000         |397       |367       |0            |23            |0        |4        |3770524341    |2947629489    |127         |127          |127       |127        |46             |                    |0              |                    |0              |NO_TRANSCODING   |2", "MEDIA_END      |14575679645122022191045@10.0.2.11                               |c62532:22:710           |10   |AUDIO     |g711Ulaw64k    |20   |10.0.2.11           |6000           |10.0.2.12           |6000         |397       |367       |0            |23            |0        |4        |3770524341    |2947629489    |127         |127          |127       |127        |46             |                    |0              |                    |0              |NO_TRANSCODING   |2")]
		[DataRow("10:53:22.554  10.0.10.11  local1.info[S = 151] |MEDIA_UPDATE   |8719331645122022191524@10.0.2.11                                |c62532:22:743           |8    |AUDIO     |g711Ulaw64k    |20   |10.0.2.11           |6004           |10.0.2.12           |6004         |209       |188       |0            |22            |0        |4        |3073419205    |3900521493    |127         |127          |127       |127        |46             |                    |0              |                    |0              |NO_TRANSCODING   |2", "MEDIA_UPDATE   |8719331645122022191524@10.0.2.11                                |c62532:22:743           |8    |AUDIO     |g711Ulaw64k    |20   |10.0.2.11           |6004           |10.0.2.12           |6004         |209       |188       |0            |22            |0        |4        |3073419205    |3900521493    |127         |127          |127       |127        |46             |                    |0              |                    |0              |NO_TRANSCODING   |2")]
		public void ShouldParseValidReport(string Syslog,string ExpectedReportLine)
		{
			CDRSyslogParser syslogParser;
			string? reportLine;

			syslogParser = new CDRSyslogParser();

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
			CDRSyslogParser syslogParser;
			string? cdrLine;

			syslogParser = new CDRSyslogParser();

			cdrLine = syslogParser.Parse(Syslog);
			Assert.IsNull(cdrLine);
		}


	}
}