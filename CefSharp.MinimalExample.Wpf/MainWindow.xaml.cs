using HtmlAgilityPack;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CefSharp.MinimalExample.Wpf
{
    public partial class MainWindow : Window
    {
        private static bool ITWORK = false;
        public MainWindow()
        {
            InitializeComponent();
            //Browser.Loaded += Browser_Loaded;
            Browser.FrameLoadEnd += Browser_FrameLoadEnd;
            Browser.Address = "api.monobank.ua";
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                while(!ITWORK)
                {
                    Browser.ViewSource();
                    Browser.GetSourceAsync().ContinueWith(taskHtml =>
                    {
                        try
                        {
                            var html = taskHtml.Result;
                            var doc = new HtmlDocument();
                            doc.LoadHtml(html);

                            var imageContainer = doc.GetElementbyId("qrcode");
                            var image = imageContainer.LastChild;
                            string base64 = image.GetAttributeValue("src", null);

                            byte[] binaryData = Convert.FromBase64String(base64);

                            BitmapImage bi = new BitmapImage();
                            bi.BeginInit();
                            bi.StreamSource = new MemoryStream(binaryData);
                            bi.EndInit();
                            qrcodeImage.Source = bi;
                            ITWORK = true;
                        }
                        catch { }
                    });
                }
            }
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
