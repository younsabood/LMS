using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace LMS
{
    public class SqlHelper : IDisposable
    {
        private readonly string _connectionString;
        private bool _disposed = false;

        public SqlHelper(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

            _connectionString = connectionString;
        }

        public Task<int> ExecuteNonQueryAsync(string query, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query cannot be null or empty.", nameof(query));

            var tcs = new TaskCompletionSource<int>();
            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) =>
            {
                var args = (Tuple<string, SqlParameter[]>)e.Argument;
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(args.Item1, conn))
                        {
                            if (args.Item2 != null)
                                cmd.Parameters.AddRange(args.Item2);
                            e.Result = cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex, "ExecuteNonQueryAsync");
                    e.Result = ex;
                }
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                if (e.Error != null)
                    tcs.SetException(e.Error);
                else if (e.Result is Exception ex)
                    tcs.SetException(ex);
                else
                    tcs.SetResult((int)e.Result);
                worker.Dispose();
            };

            worker.RunWorkerAsync(Tuple.Create(query, parameters));
            return tcs.Task;
        }

        public Task<object> ExecuteScalarAsync(string query, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query cannot be null or empty.", nameof(query));

            var tcs = new TaskCompletionSource<object>();
            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) =>
            {
                var args = (Tuple<string, SqlParameter[]>)e.Argument;
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(args.Item1, conn))
                        {
                            if (args.Item2 != null)
                                cmd.Parameters.AddRange(args.Item2);
                            e.Result = cmd.ExecuteScalar();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex, "ExecuteScalarAsync");
                    e.Result = ex;
                }
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                if (e.Error != null)
                    tcs.SetException(e.Error);
                else if (e.Result is Exception ex)
                    tcs.SetException(ex);
                else
                    tcs.SetResult(e.Result);
                worker.Dispose();
            };

            worker.RunWorkerAsync(Tuple.Create(query, parameters));
            return tcs.Task;
        }

        public Task<DataTable> ExecuteQueryAsync(string query, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query cannot be null or empty.", nameof(query));

            var tcs = new TaskCompletionSource<DataTable>();
            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) =>
            {
                var args = (Tuple<string, SqlParameter[]>)e.Argument;
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(args.Item1, conn))
                        {
                            if (args.Item2 != null)
                                cmd.Parameters.AddRange(args.Item2);
                            using (var adapter = new SqlDataAdapter(cmd))
                            {
                                var dt = new DataTable();
                                adapter.Fill(dt);
                                e.Result = dt;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex, "ExecuteQueryAsync");
                    e.Result = ex;
                }
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                if (e.Error != null)
                    tcs.SetException(e.Error);
                else if (e.Result is Exception ex)
                    tcs.SetException(ex);
                else
                    tcs.SetResult((DataTable)e.Result);
                worker.Dispose();
            };

            worker.RunWorkerAsync(Tuple.Create(query, parameters));
            return tcs.Task;
        }

        private void LogError(Exception ex, string methodName)
        {
            Console.WriteLine($"Error in {methodName}: {ex.Message}");
        }

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
                    // Dispose managed resources if any
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