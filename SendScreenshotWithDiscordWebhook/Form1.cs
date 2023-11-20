using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;


namespace SendScreenshotWithDiscordWebhook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void sendScreenshot_btn_Click(object sender, EventArgs e)
        {
            var filePath = Environment.CurrentDirectory + "screenshot.png";

            var s_width = Screen.PrimaryScreen.Bounds.Width;
            var s_height = Screen.PrimaryScreen.Bounds.Height;

            Bitmap bitmap = new Bitmap(s_width, s_height);
            Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(rectangle.Left, rectangle.Top, 0, 0, rectangle.Size);
            bitmap.Save(filePath, ImageFormat.Png);

            HttpClient client = new HttpClient();
            MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();

            var file = File.ReadAllBytes(filePath);
            multipartFormDataContent.Add(new ByteArrayContent(file, 0, file.Length), Path.GetExtension(filePath), filePath);
            client.PostAsync("webhook-url", multipartFormDataContent).Wait();
            client.Dispose();
        }
    }
}
