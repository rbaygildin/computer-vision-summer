using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace GrayScaleImage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage originalImage;
        private byte[] originalImageBytes;

        public MainWindow()
        {
            InitializeComponent();
        }

        // open file from the finder
        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg)|*.jpg; *.jpeg";
            if (openFileDialog.ShowDialog() == true){
                showImage(openFileDialog.FileName);
                RobertsOperatorButton.IsEnabled = true;
            }
        }

        // show image on the window
        private void showImage(string filename) {
            originalImage = ImageConvertor.FilenameToImage(filename);            
            originalImageBytes = ImageConvertor.ImageToByteArray(filename);
            originalPanel.Source = originalImage;
        }

        // click on processing buttons
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = (sender as Button).Content.ToString();
            ImageProcessing process = new ImageProcessing();
            byte[] processedImageBytes;
            //try
            //{
                processedImageBytes = process.FindContours(originalImageBytes, originalImage.PixelHeight, originalImage.PixelWidth);
                grayscalePanel.Source = ImageConvertor.ByteArrayToImage(processedImageBytes, originalImage.PixelWidth, originalImage.PixelHeight, 1);
            //}
            //catch (Exception ex) {
                //MessageBox.Show("Smth went so wrong...");
            //}

        }

        private void originalPanel_Drop(object sender, DragEventArgs e)
        {
            ImageSource img_source = e.Data.GetData(typeof(ImageSource)) as ImageSource;
            if (img_source != null)
                originalPanel.Source = img_source;
        }
    }
}
