using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
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
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Microsoft.Win32;
using System.Drawing;

namespace Seminar3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool is_stream_opened_;
        private Capture camera_;
        private DispatcherTimer timer_;
        private HaarCascade haar_cascade_;
        public MainWindow()
        {
            InitializeComponent();
        }

        [DllImport("gdi32.dll")]
        private static extern int DeleteObject(IntPtr o);
        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr); //release the HBitmap
                return bs;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer_ = new DispatcherTimer();
            timer_.Interval = new TimeSpan(0,0,0,0);
            timer_.Tick += TimerElapsed;
            //haar_cascade_ = new HaarCascade(@"haarcascade_frontalface_alt_tree.xml");
        }
        private void TimerElapsed(object sender, EventArgs e)
        {
            if (camera_ == null)
                return;
            using (var frame = camera_.QueryFrame())
            {
                if (frame != null)
                {
                    Image<Gray, byte> prsimg = frame.Convert<Gray, byte>();
                    foreach(var face in prsimg.DetectHaarCascade(haar_cascade_)[0]){
                        frame.Draw(face.rect, new Bgr(0, double.MaxValue, 0), 3);
                        //Bitmap bitmap = new Bitmap(frame.Bitmap.Width, frame.Bitmap.Height);
                        //using(Graphics g = Graphics.FromImage(bitmap)){
                          //  g.DrawImage(frame.Bitmap, 0, 0);
                        //}
                    }
                    CaptureImage.Source = ToBitmapSource(frame);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (camera_ != null)
                camera_.Dispose();
            timer_.Stop();
        }

        private void OpenStreamButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofdialog = new OpenFileDialog();
            ofdialog.Filter = "Видео (*.jpg, *.png)|*.jpg;*.png";
            if (ofdialog.ShowDialog() != true)
                return;
            //{
                //camera_ = new Capture();
                //timer_.Start();
            haar_cascade_ = new HaarCascade(@"haarcascade_frontalface_default.xml");
            using (var frame = new Image<Bgr, byte>(ofdialog.FileName))
            {
                if (frame != null)
                {
                    Image<Gray, byte> prsimg = frame.Convert<Gray, byte>();
                    foreach (var face in prsimg.DetectHaarCascade(haar_cascade_)[0])
                    {
                        frame.Draw(face.rect, new Bgr(0, double.MaxValue, 0), 3);
                        //Bitmap bitmap = new Bitmap(frame.Bitmap.Width, frame.Bitmap.Height);
                        //using(Graphics g = Graphics.FromImage(bitmap)){
                        //  g.DrawImage(frame.Bitmap, 0, 0);
                        //}
                    }
                    CaptureImage.Source = ToBitmapSource(frame);
                }
            }
            //}
        }
        private void OpenStreamCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is RadioButton && ((RadioButton)sender).IsChecked == true)
                e.CanExecute = true;
        }
        private void OpenStreamCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender == CameraRadioButton)
            {
                camera_ = new Capture();
                timer_.Start();
            }
            else if (sender == FileRadioButton)
            {
                OpenFileDialog ofdialog = new OpenFileDialog();
                ofdialog.Filter = "Видео (*.mp4, *.avi)|*.mp4;*.avi";
                if(ofdialog.ShowDialog() == true){
                    camera_ = new Capture(ofdialog.FileName);
                    timer_.Start();
                }
            }
        }
        private void StopStreamCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = is_stream_opened_;
        }
        private void StopStreamCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            timer_.Stop();
            is_stream_opened_ ^= true;
        }
    }
}
