namespace ODP.CoreLib.UnitTest
{
	[TestClass]
	public class ReportParserUnitTest
	{


		[DataTestMethod]
		[DataRow("CALL_START     |SBC       |4341598405122022185752@10.0.2.11                                |c62532:22:625           |LCL  |10.0.2.11           |5060         |10.0.2.12           |5060       |TCP             |+33001@10.0.1.11                         |+33001@10.0.1.11                         |2001@10.0.1.12                           |2001@10.0.1.11                           |0       |UNKN |REASON N/A                              |UNKNOWN          |18:57:52.320  UTC Mon Dec 05 2022  |                                   |                                   |-1             |                                         |                                         |24             |SBC2                            |SRD1                            |WAN                             |SBC2                            |WAN                             |DefaultRealm                    |no         |            |                          |                                                   |                                     |Normal  |2    |")]
		[DataRow("CALL_CONNECT   |SBC       |0f522c2a55774a12                                                |c62532:22:625           |RMT  |10.0.1.1            |53848        |10.0.1.11           |5060       |TCP             |+33001@10.0.1.11                         |1001@10.0.1.11                           |2001@10.0.1.11                           |2001@10.0.1.11                           |0       |UNKN |REASON N/A                              |UNKNOWN          |18:57:52.320  UTC Mon Dec 05 2022  |18:57:59.616  UTC Mon Dec 05 2022  |                                   |-1             |                                         |                                         |24             |LAN                             |SRD1                            |LAN                             |LAN                             |LAN                             |DefaultRealm                    |no         |            |                          |                                                   |                                     |Normal  |1    |")]
		[DataRow("CALL_END       |SBC       |4341598405122022185752@10.0.2.11                                |c62532:22:625           |LCL  |10.0.2.11           |5060         |10.0.2.12           |5060       |TCP             |+33001@10.0.1.11                         |+33001@10.0.1.11                         |2001@10.0.1.12                           |2001@10.0.1.11                           |30      |RMT  |GWAPP_NORMAL_CALL_CLEAR                 |NORMAL_CALL_CLEAR|18:57:52.320  UTC Mon Dec 05 2022  |18:57:59.615  UTC Mon Dec 05 2022  |18:58:29.657  UTC Mon Dec 05 2022  |-1             |                                         |                                         |24             |SBC2                            |SRD1                            |WAN                             |SBC2                            |WAN                             |DefaultRealm                    |no         |BYE         |                          |                                                   |                                     |Normal  |2    |")]
		public void ShouldParseValidSBCReportLine(string Line)
		{
			ReportParser reportParser;
			Report? report;

			reportParser = new ReportParser();

			report = reportParser.Parse(Line);
			Assert.IsNotNull(report);
			Assert.IsInstanceOfType(report, typeof(SBCReport));
		}

		[DataTestMethod]
		[DataRow("MEDIA_START    |349551015122022195348@10.0.2.11                                 |c62532:22:998           |7    |AUDIO     |g711Ulaw64k    |20   |10.0.2.11           |6008           |10.0.2.12           |6008         |0         |0         |0            |0             |0        |0        |0             |0             |127         |127          |127       |127        |46             |                    |0              |                    |0              |NO_TRANSCODING   |2")]
		[DataRow("MEDIA_END      |14575679645122022191045@10.0.2.11                               |c62532:22:710           |10   |AUDIO     |g711Ulaw64k    |20   |10.0.2.11           |6000           |10.0.2.12           |6000         |397       |367       |0            |23            |0        |4        |3770524341    |2947629489    |127         |127          |127       |127        |46             |                    |0              |                    |0              |NO_TRANSCODING   |2")]
		[DataRow("MEDIA_UPDATE   |8719331645122022191524@10.0.2.11                                |c62532:22:743           |8    |AUDIO     |g711Ulaw64k    |20   |10.0.2.11           |6004           |10.0.2.12           |6004         |209       |188       |0            |22            |0        |4        |3073419205    |3900521493    |127         |127          |127       |127        |46             |                    |0              |                    |0              |NO_TRANSCODING   |2")]
		public void ShouldParseValidMediaReportLine(string Line)
		{
			ReportParser reportParser;
			Report? report;

			reportParser = new ReportParser();

			report = reportParser.Parse(Line);
			Assert.IsNotNull(report);
			Assert.IsInstanceOfType(report, typeof(MediaReport));
		}

		[DataTestMethod]
		[DataRow("SBCReportType  |EPTyp     |SIPCallId                                                       |SessionId               |Orig |SourceIp            |SourcePort   |DestIp              |DestPort   |TransportType   |SrcURI                                   |SrcURIBeforeMap                          |DstURI                                   |DstURIBeforeMap                          |Duration|TrmSd|TrmReason                               |TrmReasonCategory|SetupTime                          |ConnectTime                        |ReleaseTime                        |RedirectReason |RedirectURINum                           |RedirectURINumBeforeMap                  |TxSigIPDiffServ|IPGroup (name)                  |SrdId (name)                    |SIPInterfaceId (name)           |ProxySetId (name)               |IpProfileId (name)              |MediaRealmId (name)             |DirectMedia|SIPTrmReason|SIPTermDesc               |Caller                                             |Callee                               |Trigger |LegId|VoiceAIConnectorName")]
		public void ShouldParseValidSBCReportHeader(string Line)
		{
			ReportParser reportParser;
			Report? report;

			reportParser = new ReportParser();

			report = reportParser.Parse(Line);
			Assert.IsNull(report);
		}

		[DataTestMethod]
		[DataRow("MediaReportType|SIPCallId                                                       |SessionId               |Cid  |MediaType |Coder          |Intrv|LocalRtpIp          |LocalRtpPort   |RemoteRtpIp         |RemoteRtpPort|InPackets |OutPackets|LocalPackLoss|RemotePackLoss|RTPdelay |RTPjitter|TxRTPssrc     |RxRTPssrc     |LocalRFactor|RemoteRFactor|LocalMosCQ|RemoteMosCQ|TxRTPIPDiffServ|LatchedRtpIp        |LatchedRtpPort |LatchedT38Ip        |LatchedT38Port |CoderTranscoding |LegId")]
		public void ShouldParseValidMediaReportHeader(string Line)
		{
			ReportParser reportParser;
			Report? report;

			reportParser = new ReportParser();

			report = reportParser.Parse(Line);
			Assert.IsNull(report);
		}

		[DataTestMethod]
		[DataRow("SBC       |4341598405122022185752@10.0.2.11                                |c62532:22:625           |LCL  |10.0.2.11           |5060         |10.0.2.12           |5060       |TCP             |+33001@10.0.1.11                         |+33001@10.0.1.11                         |2001@10.0.1.12                           |2001@10.0.1.11                           |0       |UNKN |REASON N/A                              |UNKNOWN          |18:57:52.320  UTC Mon Dec 05 2022  |                                   |                                   |-1             |                                         |                                         |24             |SBC2                            |SRD1                            |WAN                             |SBC2                            |WAN                             |DefaultRealm                    |no         |            |                          |                                                   |                                     |Normal  |2    |")]
		[DataRow("CALL_CONNECT   |0f522c2a55774a12                                                |RMT  |10.0.1.1            |53848        |10.0.1.11           |5060       |TCP             |+33001@10.0.1.11                         |1001@10.0.1.11                           |2001@10.0.1.11                           |2001@10.0.1.11                           |0       |UNKN |REASON N/A                              |UNKNOWN          |18:57:52.320  UTC Mon Dec 05 2022  |18:57:59.616  UTC Mon Dec 05 2022  |                                   |-1             |                                         |                                         |24             |LAN                             |SRD1                            |LAN                             |LAN                             |LAN                             |DefaultRealm                    |no         |            |                          |                                                   |                                     |Normal  |1" )]
		[DataRow("CALL_END       |SBC       |4341598405122022185752@10.0.2.11                                |c62532:22:625           |LCL  |10.0.2.11           |5060         |10.0.2.12           |5060       |TCP             |+33001@10.0.1.11                         |+33001@10.0.1.11                         |2001@10.0.1.12                           |2001@10.0.1.11                           |30      |RMT  |GWAPP_NORMAL_CALL_CLEAR                 |NORMAL_CALL_CLEAR|18:57:52.320  UTC Mon Dec 05 2022  |18:57:59.615  UTC Mon Dec 05 2022  |18:58:29.657  UTC Mon Dec 05 2022  |-1             |                                         |                                         |24             |SBC2                            |SRD1                            |WAN                             |SBC2                            |WAN                             |DefaultRealm                    |no         |BYE         |                          |                                                   |                                     |Normal  |2    | newcol | newcol")]
		public void ShouldNotParseInvalidSBCReportLine(string Line)
		{
			ReportParser reportParser;

			reportParser = new ReportParser();

			Assert.ThrowsException<InvalidDataException>(()=> reportParser.Parse(Line));
		}

		[DataTestMethod]
		[DataRow("349551015122022195348@10.0.2.11                                 |c62532:22:998           |7    |AUDIO     |g711Ulaw64k    |20   |10.0.2.11           |6008           |10.0.2.12           |6008         |0         |0         |0            |0             |0        |0        |0             |0             |127         |127          |127       |127        |46             |                    |0              |                    |0              |NO_TRANSCODING   |2")]
		[DataRow("MEDIA_END      |c62532:22:710           |10   |AUDIO     |g711Ulaw64k    |20   |10.0.2.11           |6000           |10.0.2.12           |6000         |397       |367       |0            |23            |0        |4        |3770524341    |2947629489    |127         |127          |127       |127        |46             |                    |0              |                    |0              |NO_TRANSCODING   |2")]
		[DataRow("MEDIA_UPDATE   |8719331645122022191524@10.0.2.11                                |c62532:22:743           |8    |AUDIO     |g711Ulaw64k    |20   |10.0.2.11           |6004           |10.0.2.12           |6004         |209       |188       |0            |22            |0        |4        |3073419205    |3900521493    |127         |127          |127       |127        |46             |                    |0              |                    |0              |NO_TRANSCODING   |2 | test")]
		public void ShouldNotParseInvalidMediaReportLine(string Line)
		{
			ReportParser reportParser;

			reportParser = new ReportParser();

			Assert.ThrowsException<InvalidDataException>(() => reportParser.Parse(Line));
		}



	}
}