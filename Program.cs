using System;
using System.Data;
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
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Show the splash screen and check login status
                using (var splashScreen = new splash())
                {
                    DialogResult loginStatus = splashScreen.ShowDialog();

                    // If login is successful, proceed to the Home form
                    if (loginStatus == DialogResult.OK)
                    {
                        Application.Run(new LMSHome());
                    }
                    else
                    {
                        // Otherwise, show the LoginPage
                        using (var loginPage = new LoginPage())
                        {
                            if (loginPage.ShowDialog() == DialogResult.OK)
                            {
                                Application.Run(new LMSHome()); // Open Home after successful login
                            }
                            else
                            {
                                Application.Exit(); // Exit if login fails or is canceled
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
    }
}