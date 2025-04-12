using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                sql();
                CheckTrialPeriod();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                using (var splashScreen = new splash())
                {
                    DialogResult loginStatus = splashScreen.ShowDialog();
                    if (loginStatus == DialogResult.OK)
                    {
                        Application.Run(new LMSHome());
                    }
                    else
                    {
                        using (var loginPage = new LoginPage())
                        {
                            if (loginPage.ShowDialog() == DialogResult.OK)
                            {
                                Application.Run(new LMSHome());
                            }
                            else
                            {
                                Application.Exit();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while running the application: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        private static void CheckTrialPeriod()
        {
            DateTime? storedStartDate = GetTrialStartDate();
            DateTime currentDate = DateTime.Today;

            if (storedStartDate == null)
            {
                SaveTrialStartDate(currentDate);
                storedStartDate = currentDate;
            }

            DateTime expirationDate = storedStartDate.Value.AddDays(30);

            if (currentDate > expirationDate)
            {
                MessageBox.Show("Your 30-day trial has expired. Please purchase a license.", "Trial Expired", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }
        }

        private static DateTime? GetTrialStartDate()
        {
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 StartDate FROM auth.TrialInfo", conn); // Add schema here
                var result = cmd.ExecuteScalar();
                return result == null ? (DateTime?)null : Convert.ToDateTime(result);
            }
        }

        private static void SaveTrialStartDate(DateTime startDate)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO auth.TrialInfo (StartDate) VALUES (@StartDate)", conn); // Add schema here
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.ExecuteNonQuery();
            }
        }

        public static void sql()
        {
            string path = Path.GetFullPath(Environment.CurrentDirectory);

            string databaseName = "LMS.mdf";

            Properties.Settings.Default.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + @"\" + databaseName + ";Integrated Security=True";
        }
    }
}