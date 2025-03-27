using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ODP.ViewModels
{
    public enum PlayerStates { DragDrop,Loading,NoAudio, ReadyToPlay,Playing, InvalidCodec};

    public  class PlayerViewModel:BaseViewModel
    {


        public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(PlayerStates), typeof(PlayerViewModel), new PropertyMetadata(PlayerStates.DragDrop));
        public PlayerStates State
        {
            get { return (PlayerStates)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }



        public static readonly DependencyProperty TxRTPStreamProperty = DependencyProperty.Register("TxRTPStream", typeof(RTPStreamViewModel), typeof(PlayerViewModel), new PropertyMetadata(null));
        public RTPStreamViewModel TxRTPStream
        {
            get { return (RTPStreamViewModel)GetValue(TxRTPStreamProperty); }
            set { SetValue(TxRTPStreamProperty, value); }
        }
        
        public static readonly DependencyProperty RxRTPStreamProperty = DependencyProperty.Register("RxRTPStream", typeof(RTPStreamViewModel), typeof(PlayerViewModel), new PropertyMetadata(null));
        public RTPStreamViewModel RxRTPStream
        {
            get { return (RTPStreamViewModel)GetValue(RxRTPStreamProperty); }
            set { SetValue(RxRTPStreamProperty, value); }
        }

        public PlayerViewModel()
        {
            TxRTPStream = new RTPStreamViewModel(Array.Empty<byte>());
            RxRTPStream = new RTPStreamViewModel(Array.Empty<byte>());

        }



        public async Task LoadAsync(CDRMediaReportViewModel MediaReportViewModel)
        {
            State=PlayerStates.Loading;
            TxRTPStream = new RTPStreamViewModel(MediaReportViewModel.TxRTPBuffer) { SSRC = MediaReportViewModel.TxRTPssrcDisplay ?? "NA", Coder=MediaReportViewModel.Coder??"Undefined" ,RTPCount=MediaReportViewModel.TxRTPCount??0 };
            RxRTPStream = new RTPStreamViewModel(MediaReportViewModel.RxRTPBuffer) { SSRC = MediaReportViewModel.RxRTPssrcDisplay ?? "NA", Coder = MediaReportViewModel.Coder ?? "Undefined", RTPCount = MediaReportViewModel.RxRTPCount??0 };
            await Task.Delay(500);
            if ((TxRTPStream.Data.Length == 0) && (RxRTPStream.Data.Length == 0)) State = PlayerStates.NoAudio;
            else State = PlayerStates.ReadyToPlay;
        }


    }


}
