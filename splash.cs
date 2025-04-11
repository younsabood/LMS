using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Timer = System.Windows.Forms.Timer;

namespace LMS
{
    public partial class splash : Form
    {
        public static SqlHelper sqlHelper = new SqlHelper(Properties.Settings.Default.ConnectionString);
        private bool isLoginCheckCompleted = false;
        private DialogResult loginResult = DialogResult.Abort;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }
        public splash()
        {
            try
            {
                InitializeComponent();

                // Initialize the timer
                splashTimer = new Timer
                {
                    Interval = 30
                };
                splashTimer.Tick += SplashTimer_Tick;
                splashTimer.Start();

                // Call the asynchronous method to check login status
                _ = CheckLoginAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل شاشة البداية: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SplashTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                progressBar.Value += 1;

                // Check if the progress bar has reached its maximum value
                if (progressBar.Value >= progressBar.Maximum)
                {
                    splashTimer.Stop();

                    // If the login check is already completed, close the form with the result
                    if (isLoginCheckCompleted)
                    {
                        this.DialogResult = loginResult;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحديث شاشة البداية: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CheckLoginAsync()
        {
            try
            {
                string query = "SELECT * FROM [auth].[Users]";

                // Execute the query
                DataTable result = await sqlHelper.ExecuteQueryAsync(query);

                // Process the results
                if (result.Rows.Count > 0)
                {
                    loginResult = DialogResult.OK; // Store the login result
                }
                else
                {
                    loginResult = DialogResult.Abort; // Store the login result
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء التحقق من حالة تسجيل الدخول: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                loginResult = DialogResult.Abort; // Default to Abort in case of error
            }
            finally
            {
                // Mark the login check as completed
                isLoginCheckCompleted = true;

                // If the timer has already finished, close the form with the result
                if (progressBar.Value >= progressBar.Maximum)
                {
                    this.DialogResult = loginResult;
                    this.Close();
                }
            }
        }
    }
}