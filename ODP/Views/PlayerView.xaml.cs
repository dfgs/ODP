using GongSolutions.Wpf.DragDrop;
using NAudio.Wave;
using ODP.CoreLib;
using ODP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ODP.Views
{

    
    /// <summary>
    /// Logique d'interaction pour PlayerView.xaml
    /// </summary>
    public partial class PlayerView : UserControl,IDropTarget
    {
        private PlayerViewModel viewModel;
        private CancellationTokenSource? playerCancellationToken;

        public PlayerView()
        {
            InitializeComponent();
            
            viewModel = new PlayerViewModel();
            DataContext= viewModel;
        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            CDRMediaReportViewModel? sourceItem = dropInfo.Data as CDRMediaReportViewModel;

            if (sourceItem == null) return;

            if (viewModel.State == PlayerStates.Loading) return;

            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Copy;
            
        }

        async void IDropTarget.Drop(IDropInfo dropInfo)
        {
            CDRMediaReportViewModel? sourceItem = dropInfo.Data as CDRMediaReportViewModel;

            if (sourceItem == null) return;

            await StopPlayAsync();
            await viewModel.LoadAsync(sourceItem);
        }

       

        private async Task StopPlayAsync()
        {
            if (playerCancellationToken != null)
            {
                playerCancellationToken.Cancel();
                await Task.Delay(500);
            }
        }

        private async Task PlayRTPAsync(RTPStreamViewModel RTPStream, CancellationTokenSource CancellationToken)
        {
            WaveFormat format;

            if (RTPStream.Data.Length == 0) return;

            switch (RTPStream.Coder)
            {
                case "g711Ulaw64k":
                    format = WaveFormat.CreateMuLawFormat(8000, 1);
                    break;
                case "g711Alaw64k":
                    format = WaveFormat.CreateALawFormat(8000, 1);
                    break;
                default:
                    viewModel.State = PlayerStates.InvalidCodec;
                    return;

            }

 
            IWaveProvider provider = new RawSourceWaveStream(new MemoryStream(RTPStream.Data), format);
            WaveOutEvent waveOut = new WaveOutEvent();
            waveOut.Init(provider);

            viewModel.State = PlayerStates.Playing;
            RTPStream.IsPlaying= true;
            waveOut.Play();

            while ((!CancellationToken.IsCancellationRequested) && (waveOut.PlaybackState == PlaybackState.Playing))
            {
                await Task.Delay(500);
                RTPStream.Progress =(int)( 100*waveOut.GetPosition()/RTPStream.Data.LongLength);
            }

            waveOut.Stop();
            RTPStream.IsPlaying = false;
            viewModel.State = PlayerStates.ReadyToPlay;
            RTPStream.Progress = 0;

            waveOut.Dispose();


        }

      

        private void PlayRTPCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true; e.CanExecute = (e.Parameter as RTPStreamViewModel) != null;
        }

        private async void PlayRTPCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RTPStreamViewModel? rtpStream;

            rtpStream = e.Parameter as RTPStreamViewModel;
            if (rtpStream == null) return;

            await StopPlayAsync();

            playerCancellationToken = new CancellationTokenSource();
            await PlayRTPAsync(rtpStream, playerCancellationToken);
        }

        private void StopRTPCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true; e.CanExecute = playerCancellationToken!=null;
        }

        private async void StopRTPCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await StopPlayAsync();
        }


    }
}
