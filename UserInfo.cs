using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace LMS
{
    public class UserInfo
    {
        public static SqlHelper sqlHelper = new SqlHelper(Properties.Settings.Default.ConnectionString);

        [JsonProperty("sub")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("picture")]
        public string PictureUrl { get; set; }
        public static async Task<UserInfo> GetUserAsync()
        {
            try
            {
                // Use the shared SqlHelper instance from Program
                string query = "SELECT TOP 1 * FROM [auth].[Users]";
                DataTable result = await sqlHelper.ExecuteQueryAsync(query);

                // Check if any rows exist
                if (result.Rows.Count == 0)
                {
                    return null; // No user exists in the database
                }

                // Map the first row to a UserInfo object
                DataRow row = result.Rows[0];
                return new UserInfo
                {
                    Id = row["GoogleId"].ToString(), // Map GoogleId to Id (sub)
                    Name = row["Name"].ToString(),
                    Email = row["Email"].ToString(),
                    PictureUrl = row["PictureUrl"].ToString()
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Log the exception details for debugging (optional)
                Console.WriteLine(ex.ToString());
                return null; // Return null in case of error
            }
        }
    }
}