
using Microsoft.Web.WebView2.WinForms;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Web;

namespace LMS
{
    public partial class AuthForm : Form
    {
        private readonly string redirectUri;
        private WebView2 webView;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string AuthCode { get; private set; }

        public AuthForm(string authUrl, string redirectUri, int width, int height)
        {
            this.redirectUri = redirectUri;

            try
            {
                InitializeWebView(authUrl);

                // Set size and remove taskbar/icon
                this.Size = new Size(width + 100, height + 100);
                this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                this.Text = "Google Login";
                this.ShowInTaskbar = false;
                this.StartPosition = FormStartPosition.CenterScreen; // Optional: Center on parent
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing the form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw; // Re-throw the exception if you want the calling code to handle it
            }
        }

        private async void InitializeWebView(string authUrl)
        {
            try
            {
                webView = new WebView2
                {
                    Dock = DockStyle.Fill,
                    Source = new Uri(authUrl)
                };

                // Initialize WebView2
                await webView.EnsureCoreWebView2Async(null);
                webView.CoreWebView2.NavigationCompleted += WebView_NavigationCompleted;
                Controls.Add(webView);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing WebView2: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw; // Re-throw the exception if you want the calling code to handle it
            }
        }

        private void WebView_NavigationCompleted(object sender, global::Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            try
            {
                if (webView.Source.AbsoluteUri.StartsWith(redirectUri))
                {
                    var uri = webView.Source;
                    var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);

                    if (!string.IsNullOrEmpty(queryParams["code"]))
                    {
                        AuthCode = queryParams["code"];
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else if (!string.IsNullOrEmpty(queryParams["error"]))
                    {
                        DialogResult = DialogResult.Abort;
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during navigation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw; // Re-throw the exception if you want the calling code to handle it
            }
        }
    }
}
