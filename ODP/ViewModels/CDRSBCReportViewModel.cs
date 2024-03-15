using LogLib;
using ODP.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class CDRSBCReportViewModel : CDRReportGenericViewModel<CDRSBCReport>
	{

		public string? SBCReportType
		{
			get => Model?.SBCReportType;
		}
		public string? EPTyp
		{
			get => Model?.EPTyp;
		}
		public string? Orig
		{
			get => Model?.Orig;
		}
		public string? SourceIp
		{
			get => Model?.SourceIp;
		}
		public string? SourcePort
		{
			get => Model?.SourcePort;
		}
		public string? DestIp
		{
			get => Model?.DestIp;
		}
		public string? DestPort
		{
			get => Model?.DestPort;
		}
		public string? TransportType
		{
			get => Model?.TransportType;
		}
		public string? SrcURI
		{
			get => Model?.SrcURI;
		}
		public string? SrcURIBeforeMap
		{
			get => Model?.SrcURIBeforeMap;
		}
		public string? DstURI
		{
			get => Model?.DstURI;
		}
		public string? DstURIBeforeMap
		{
			get => Model?.DstURIBeforeMap;
		}
		public string? Duration
		{
			get => Model?.Duration;
		}
		public string? TrmSd
		{
			get => Model?.TrmSd;
		}
		public string? TrmReason
		{
			get => Model?.TrmReason;
		}
		public string? TrmReasonCategory
		{
			get => Model?.TrmReasonCategory;
		}
		public DateTime? SetupTime
		{
			get => Model?.SetupTime;
		}
		public DateTime? ConnectTime
		{
			get => Model?.ConnectTime;
		}
		public DateTime? ReleaseTime
		{
			get => Model?.ReleaseTime;
		}
		public string? RedirectReason
		{
			get => Model?.RedirectReason;
		}
		public string? RedirectURINum
		{
			get => Model?.RedirectURINum;
		}
		public string? RedirectURINumBeforeMap
		{
			get => Model?.RedirectURINumBeforeMap;
		}
		public string? TxSigIPDiffServ
		{
			get => Model?.TxSigIPDiffServ;
		}
		public string? IPGroup
		{
			get => Model?.IPGroup;
		}
		public string? SrdId
		{
			get => Model?.SrdId;
		}
		public string? SIPInterfaceId
		{
			get => Model?.SIPInterfaceId;
		}
		public string? ProxySetId
		{
			get => Model?.ProxySetId;
		}
		public string? IpProfileId
		{
			get => Model?.IpProfileId;
		}
		public string? MediaRealmId
		{
			get => Model?.MediaRealmId;
		}
		public string? DirectMedia
		{
			get => Model?.DirectMedia;
		}
		public string? SIPTrmReason
		{
			get => Model?.SIPTrmReason;
		}
		public string? SIPTermDesc
		{
			get => Model?.SIPTermDesc;
		}
		public string? Caller
		{
			get => Model?.Caller;
		}
		public string? Callee
		{
			get => Model?.Callee;
		}
		public string? Trigger
		{
			get => Model?.Trigger;
		}
		public string? LegId
		{
			get => Model?.LegId;
		}
		public string? VoiceAIConnectorName
		{
			get => Model?.VoiceAIConnectorName;
		}

		public CDRSBCReportViewModel(CDRSBCReport Model) : base(Model)
		{
		}
	}
}
