using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rays.Common
{
    public class WebSnapshotsHelper
    {
        Bitmap m_Bitmap;
        string m_Url;
        int m_BrowserWidth, m_BrowserHeight, m_ThumbnailWidth, m_ThumbnailHeight;

        public WebSnapshotsHelper(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth,
            int ThumbnailHeight)
        {
            m_Url = Url;
            m_BrowserHeight = BrowserHeight;
            m_BrowserWidth = BrowserWidth;
            m_ThumbnailWidth = ThumbnailWidth;
            m_ThumbnailHeight = ThumbnailHeight;
        }

        public static Bitmap GetWebSiteThumbnail(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth,
            int ThumbnailHeight)
        {
            WebSnapshotsHelper thumbnailGenerator = new WebSnapshotsHelper(Url, BrowserWidth, BrowserHeight,
                ThumbnailWidth, ThumbnailHeight);
            return thumbnailGenerator.GenerateWebSiteThumbnailImage();
        }

        public Bitmap GenerateWebSiteThumbnailImage()
        {
            Thread m_thread = new Thread(new ThreadStart(_GenerateWebSiteThumbnailImage));
            m_thread.SetApartmentState(ApartmentState.STA);
            m_thread.Start();
            m_thread.Join();
            return m_Bitmap;
        }

        private void _GenerateWebSiteThumbnailImage()
        {
            WebBrowser m_WebBrowser = new WebBrowser();
            m_WebBrowser.ScrollBarsEnabled = false;
            m_WebBrowser.Navigate(m_Url);
            m_WebBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
            while (m_WebBrowser.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();
            m_WebBrowser.Dispose();
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser m_WebBrowser = (WebBrowser)sender;
            m_WebBrowser.ClientSize = new Size(this.m_BrowserWidth, this.m_BrowserHeight);
            m_WebBrowser.ScrollBarsEnabled = false;
            m_Bitmap = new Bitmap(m_WebBrowser.Bounds.Width, m_WebBrowser.Bounds.Height);
            m_WebBrowser.BringToFront();
            m_WebBrowser.DrawToBitmap(m_Bitmap, m_WebBrowser.Bounds);
            //m_Bitmap = (Bitmap)m_Bitmap.GetThumbnailImage(m_ThumbnailWidth, m_ThumbnailHeight, null, IntPtr.Zero);
            //WebBrowser webBrowser = (WebBrowser) sender;
            //{
            //    // 网页加载完毕才保存
            //    if (webBrowser.ReadyState == WebBrowserReadyState.Complete)
            //    {
            //        // 获取网页高度和宽度,也可以自己设置
            //        int height = webBrowser.Document.Body.ScrollRectangle.Height;
            //        int width = webBrowser.Document.Body.ScrollRectangle.Width;

            //        // 调节webBrowser的高度和宽度
            //        webBrowser.Height = height;
            //        webBrowser.Width = width;

            //        Bitmap bitmap = new Bitmap(width, height); // 创建高度和宽度与网页相同的图片
            //        Rectangle rectangle = new Rectangle(0, 0, width, height); // 绘图区域
            //        webBrowser.DrawToBitmap(bitmap, rectangle); // 截图

            //        // 保存图片
            //        m_Bitmap =
            //            (Bitmap)bitmap.GetThumbnailImage(m_ThumbnailWidth, m_ThumbnailHeight, null, IntPtr.Zero);


            //    }
            //}

        }
    }
}
