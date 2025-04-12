using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
            if (!String.IsNullOrEmpty(api.Text))
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
            else
            {
                MessageBox.Show($"Error: Input Your API KEY", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private Task<string> ShowGoogleAuthForm()
        {
            try
            {
                var authForm = new AuthForm(GenerateGoogleAuthUrl(), RedirectUri, Size.Width, Size.Height);
                return Task.FromResult(authForm.ShowDialog() == DialogResult.OK ? authForm.AuthCode : null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing Google authentication form: {ex.Message}", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return Task.FromResult<string>(null);
            }
        }

        private string GenerateGoogleAuthUrl()
        {
            try
            {
                return $"https://accounts.google.com/o/oauth2/auth?" +
                       $"scope=email%20profile&" +
                       $"redirect_uri={Uri.EscapeDataString(RedirectUri)}&" +
                       $"response_type=code&" +
                       $"client_id={ClientId}&" +
                       $"access_type=offline";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating Google authentication URL: {ex.Message}", "URL Generation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private async Task AuthenticateWithGoogleAsync(string authCode)
        {
            try
            {
                _tokenResponse = await ExchangeCodeForToken(authCode);
                _userInfo = await GetUserInfo(_tokenResponse.AccessToken);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error authenticating with Google: {ex.Message}", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private async Task<TokenResponse> ExchangeCodeForToken(string code)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error exchanging authorization code for token: {ex.Message}", "Token Exchange Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private static async Task<TokenResponse> HandleTokenResponse(HttpResponseMessage response)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling token response: {ex.Message}", "Token Response Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private async Task<UserInfo> GetUserInfo(string accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");
                return await HandleUserInfoResponse(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching user info: {ex.Message}", "User Info Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private static async Task<UserInfo> HandleUserInfoResponse(HttpResponseMessage response)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling user info response: {ex.Message}", "User Info Response Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
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
            try
            {
                Properties.Settings.Default.id = _userInfo.Id;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving user ID settings: {ex.Message}", "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task InsertUserIntoDatabase()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting user into database: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void exit_Click(object sender, EventArgs e) => Application.Exit();

        private void getapi_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://aistudio.google.com/apikey");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening API key page: {ex.Message}", "API Key Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}