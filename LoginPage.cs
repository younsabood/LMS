using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Newtonsoft.Json;

namespace LMS
{
    public partial class LoginPage : Form
    {
        private static readonly SqlHelper SqlHelper = new SqlHelper(Properties.Settings.Default.ConnectionString);

        private const string ClientId = "965955892432-bjho48goiifr5vnq1v3c6efgfgu00str.apps.googleusercontent.com";
        private const string ClientSecret = "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";
        private const string RedirectUri = "http://localhost:8080/";
        private UserInfo _userInfo;
        private TokenResponse _tokenResponse;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void google_Click(object sender, EventArgs e)
        {
            try
            {
                var authCode = await ShowGoogleAuthForm();
                if (string.IsNullOrEmpty(authCode)) return;

                await AuthenticateWithGoogleAsync(authCode);
                await RegisterUserAsync();

                // Successfully logged in
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                // Handle errors without rethrowing
                MessageBox.Show($"Error: {ex.Message}", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Task<string> ShowGoogleAuthForm()
        {
            var authForm = new AuthForm(GenerateGoogleAuthUrl(), RedirectUri, Size.Width, Size.Height);
            return Task.FromResult(authForm.ShowDialog() == DialogResult.OK ? authForm.AuthCode : null);
        }

        private string GenerateGoogleAuthUrl()
        {
            return $"https://accounts.google.com/o/oauth2/auth?" +
                   $"scope=email%20profile&" +
                   $"redirect_uri={Uri.EscapeDataString(RedirectUri)}&" +
                   $"response_type=code&" +
                   $"client_id={ClientId}&" +
                   $"access_type=offline";
        }

        private async Task AuthenticateWithGoogleAsync(string authCode)
        {
            _tokenResponse = await ExchangeCodeForToken(authCode);
            _userInfo = await GetUserInfo(_tokenResponse.AccessToken);
        }

        private async Task<TokenResponse> ExchangeCodeForToken(string code)
        {
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("redirect_uri", RedirectUri),
                new KeyValuePair<string, string>("grant_type", "authorization_code")
            });

            var response = await client.PostAsync("https://oauth2.googleapis.com/token", content);
            return await HandleTokenResponse(response);
        }

        private static async Task<TokenResponse> HandleTokenResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Token exchange failed: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokenResponse>(json)
                   ?? throw new InvalidOperationException("Failed to deserialize token response");
        }

        private async Task<UserInfo> GetUserInfo(string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");
            return await HandleUserInfoResponse(response);
        }

        private static async Task<UserInfo> HandleUserInfoResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"User info request failed: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserInfo>(json)
                   ?? throw new InvalidOperationException("Failed to deserialize user info");
        }

        private async Task RegisterUserAsync()
        {
            try
            {
                SaveUserIdSetting();
                await InsertUserIntoDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Google registration failed: {ex.Message}",
                    "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void SaveUserIdSetting()
        {
            Properties.Settings.Default.id = _userInfo.Id;
            Properties.Settings.Default.Save();
        }

        private async Task InsertUserIntoDatabase()
        {
            var parameters = new[]
            {
                new SqlParameter("@Name", _userInfo.Name),
                new SqlParameter("@Email", _userInfo.Email),
                new SqlParameter("@ApiKey", api.Text),
                new SqlParameter("@GoogleId", _userInfo.Id),
                new SqlParameter("@PictureUrl", _userInfo.PictureUrl)
            };

            int x = await SqlHelper.ExecuteNonQueryAsync(
                "INSERT INTO [auth].[Users] ([Name], [Email], [ApiKey], [GoogleId], [PictureUrl]) " +
                "VALUES (@Name, @Email, @ApiKey, @GoogleId, @PictureUrl)", parameters);
            Properties.Settings.Default.googleAI = api.Text;
            Properties.Settings.Default.Save();
        }

        private void exit_Click(object sender, EventArgs e) => Application.Exit();
    }
}