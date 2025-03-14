using BigEndianReaderLib;
using EthernetFrameReaderLib;
using RTCPFrameReaderLib;
using RTPFrameReaderLib;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace ODP.CoreLib
{
	public class Project
	{
		
		public List<Session> Sessions
		{
			get;
			set;
		}

		public List<PacketLossReport> PacketLossReports
		{
			get;
			set;
		}

		

		public Project()
		{
			Sessions= new List<Session>();
			PacketLossReports= new List<PacketLossReport>();
			//RTPPackets=new Dictionary<uint, SortedList<uint,RTP>>();
			
		}

		public void AddCDRReport(CDRReport Report)
		{
			Session? session;
			
			if (Report==null) throw new ArgumentNullException(nameof(Report));

			session = Sessions.FirstOrDefault(item => item.SessionId == Report.SessionId);
			if (session==null)
			{
				session = new Session();
				session.SessionId= Report.SessionId;
				Sessions.Add(session);
			}
			session.AddCDRReport(Report);

		}
		public void AddRTCPReport(RTCPReport Report)
		{
			Session? session;

			if (Report == null) throw new ArgumentNullException(nameof(Report));

			session = Sessions.FirstOrDefault(item => string.Equals(item.SessionId, Report.SessionId, StringComparison.OrdinalIgnoreCase));
			if (session == null) return;
			
			session.AddRTCPReport(Report);

		}

		public void AddPacketReorderReport(PacketReorderReport Report)
		{
			Session? session;

			if (Report == null) throw new ArgumentNullException(nameof(Report));

			session = Sessions.FirstOrDefault(item => Report.SessionId == item.SessionId );
			if (session == null)
			{
				session = new Session();
				session.SessionId = Report.SessionId;
				Sessions.Add(session);
			}
			session.AddPacketReorderReport(Report);

		}

		public void AddPacketLossReport(PacketLossReport Report)
		{
			if (Report == null) throw new ArgumentNullException(nameof(Report));
			PacketLossReports.Add(Report);
		}


        public void AddRTP(RTP RTP)
        {
			CDRMediaReport? mediaReport;

            if (RTP == null) throw new ArgumentNullException(nameof(RTP));
            
			mediaReport=Sessions.SelectMany(session=>session.Calls).SelectMany(call=>call.MediaReports).FirstOrDefault(item=>item.TxRTPssrc==RTP.SSRC);
			if (mediaReport!=null)
			{
				if (mediaReport.TxRTPPackets.ContainsKey(RTP.SequenceNumber)) return; //duplicate packet
				mediaReport.TxRTPPackets.Add(RTP.SequenceNumber, RTP);
				return;
			}
            mediaReport = Sessions.SelectMany(session => session.Calls).SelectMany(call => call.MediaReports).FirstOrDefault(item => item.RxRTPssrc == RTP.SSRC);
            if (mediaReport != null)
            {
                if (mediaReport.RxRTPPackets.ContainsKey(RTP.SequenceNumber)) return; //duplicate packet
                mediaReport.RxRTPPackets.Add(RTP.SequenceNumber, RTP);
                return;
            }


        }

        public async Task SaveAsync(string Path)
		{
			XmlSerializer serializer;

			serializer = new XmlSerializer(typeof(Project));
			using (FileStream stream = new FileStream(Path, FileMode.OpenOrCreate))
			{
				await Task.Run(() => serializer.Serialize(stream, this));
			}
		}
		public static async Task<Project> LoadAsync(string Path)
		{
			XmlSerializer serializer;
			Project result;
			object? data;

			serializer = new XmlSerializer(typeof(Project));
			using (FileStream stream = new FileStream(Path, FileMode.Open))
			{
				data = await Task.Run<object?>(() => serializer.Deserialize(stream));
				if (data == null) throw new InvalidOperationException("Failed to deserialize project");
				result =(Project)data;
			}
			return result;
		}


	}
}