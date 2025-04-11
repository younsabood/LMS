using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS
{
    public class SqlHelper : IDisposable
    {
        private readonly string _connectionString;
        private bool _disposed = false;

        // Initialize the database connection with the custom connection string
        public SqlHelper(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

            _connectionString = connectionString;
        }

        // Asynchronous method to execute non-query commands (INSERT, UPDATE, DELETE)
        public async Task<int> ExecuteNonQueryAsync(string query, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query cannot be null or empty.", nameof(query));

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(); // Open the connection asynchronously

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null && parameters.Length > 0)
                            command.Parameters.AddRange(parameters);

                        // Execute the query asynchronously and return the number of affected rows
                        return await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "ExecuteNonQueryAsync");
                throw; // Rethrow the exception after logging
            }
        }

        // Asynchronous method to execute scalar queries (e.g., COUNT, SUM)
        public async Task<object> ExecuteScalarAsync(string query, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query cannot be null or empty.", nameof(query));

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(); // Open the connection asynchronously

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null && parameters.Length > 0)
                            command.Parameters.AddRange(parameters);

                        // Execute the query asynchronously and return the scalar result
                        return await command.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "ExecuteScalarAsync");
                throw; // Rethrow the exception after logging
            }
        }

        // Asynchronous method to execute queries that return a DataTable (e.g., SELECT)
        public async Task<DataTable> ExecuteQueryAsync(string query, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query cannot be null or empty.", nameof(query));

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(); // Open the connection asynchronously

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null && parameters.Length > 0)
                            command.Parameters.AddRange(parameters);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            await Task.Run(() => adapter.Fill(dataTable)); // Fill the DataTable asynchronously
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "ExecuteQueryAsync");
                throw; // Rethrow the exception after logging
            }
        }

        // Error logging method
        private void LogError(Exception ex, string methodName)
        {
            // Log the error to a file, database, or monitoring system
            Console.WriteLine($"Error in {methodName}: {ex.Message}");
            // You can replace this with a proper logging framework like Serilog, NLog, etc.
        }

        // Dispose pattern implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here if needed
                }
                _disposed = true;
            }
        }

        ~SqlHelper()
        {
            Dispose(false);
        }
    }
}
