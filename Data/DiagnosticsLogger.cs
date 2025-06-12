using System.Collections.Concurrent;
using System.Globalization;
using System.Text;

namespace Data
{
    internal class DiagnosticsLogger : IDisposable
    {
        private readonly BlockingCollection<string> _logQueue = new(new ConcurrentQueue<string>(), 1000);
        private readonly StreamWriter _writer;
        private readonly CancellationTokenSource _cts = new();
        private readonly Task _loggingTask;
        private bool _disposed = false;

        public DiagnosticsLogger(string baseFileName)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = Path.GetFileNameWithoutExtension(baseFileName);
            string extension = Path.GetExtension(baseFileName);
            string fullName = Path.Combine(@"C:\Logs2", $"{fileName}_{timestamp}{extension}");

            if (!Directory.Exists(Path.GetDirectoryName(fullName)!))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullName)!);
            }

            _writer = new StreamWriter(fullName, append: false, encoding: Encoding.ASCII);
            _loggingTask = Task.Run(() => ProcessLogQueue(_cts.Token));
        }
        //public void LogBallState(IBall ball, IVector position, IVector velocity)
        //{
        //    string message = $"[{DateTime.Now:HH:mm:ss.fff}] Ball ID={ball.GetHashCode()}, " +
        //                     $"Pos=({position.x.ToString("F2", CultureInfo.InvariantCulture)},{position.y.ToString("F2", CultureInfo.InvariantCulture)}), " +
        //                     $"Vel=({velocity.x.ToString("F2", CultureInfo.InvariantCulture)},{velocity.y.ToString("F2", CultureInfo.InvariantCulture)})";
        //    Log(message);
        //}

        public void LogWallCollision(IBall ball, IVector position, string wall)
        {
            string logEntry = $"[{DateTime.UtcNow:O}] WALL COLLISION - Ball ID: {ball.GetHashCode()}, " +
                              $"Wall: {wall}, Position: ({position.x:F2}, {position.y:F2})";
            Log(logEntry);
        }

        public void LogBallCollision(IBall b1, IBall b2, IVector position)
        {
            string logEntry = $"[{DateTime.UtcNow:O}] BALL COLLISION - Ball1 ID: {b1.GetHashCode()}, Ball2 ID: {b2.GetHashCode()}, " +
                              $"Position: ({position.x:F2}, {position.y:F2})";
            Log(logEntry);
        }


        private void Log(string logEntry)
        {
            if (_disposed || _logQueue.IsAddingCompleted) return;

            try
            {
                _logQueue.TryAdd(logEntry);
            }
            catch (ObjectDisposedException) { }
        }

        private void ProcessLogQueue(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested || !_logQueue.IsCompleted)
                {
                    if (_logQueue.TryTake(out var logLine, TimeSpan.FromMilliseconds(100)))
                    {
                        _writer.WriteLine(logLine);
                        _writer.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Logger error: " + ex.Message);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            _cts.Cancel();
            _logQueue.CompleteAdding();
            _loggingTask.Wait();

            _writer.Dispose();
            _logQueue.Dispose();
            _cts.Dispose();
        }
    }
}
