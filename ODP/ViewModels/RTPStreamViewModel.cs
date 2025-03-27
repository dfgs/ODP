using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ODP.ViewModels
{
    public class RTPStreamViewModel:BaseViewModel
    {


        public static readonly DependencyProperty SSRCProperty = DependencyProperty.Register("SSRC", typeof(string), typeof(RTPStreamViewModel), new PropertyMetadata("NA"));
        public string SSRC
        {
            get { return (string)GetValue(SSRCProperty); }
            set { SetValue(SSRCProperty, value); }
        }


        public static readonly DependencyProperty CoderProperty = DependencyProperty.Register("Coder", typeof(string), typeof(RTPStreamViewModel), new PropertyMetadata(null));

        public string Coder
        {
            get { return (string)GetValue(CoderProperty); }
            set { SetValue(CoderProperty, value); }
        }




        public static readonly DependencyProperty RTPCountProperty = DependencyProperty.Register("RTPCount", typeof(int), typeof(RTPStreamViewModel), new PropertyMetadata(0));
        public int RTPCount
        {
            get { return (int)GetValue(RTPCountProperty); }
            set { SetValue(RTPCountProperty, value); }
        }




        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(int), typeof(RTPStreamViewModel), new PropertyMetadata(0));
        public int Progress
        {
            get { return (int)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }



        public static readonly DependencyProperty IsPlayingProperty = DependencyProperty.Register("IsPlaying", typeof(bool), typeof(RTPStreamViewModel), new PropertyMetadata(false));
        public bool IsPlaying
        {
            get { return (bool)GetValue(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); }
        }






        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(byte[]), typeof(RTPStreamViewModel), new PropertyMetadata(null));
        public byte[] Data
        {
            get { return (byte[])GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }


        public RTPStreamViewModel(byte[] Data)
        {
            this.Data = Data;
        }

    }
}
