using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ContosoUniversity.Logging;

namespace ContosoUniversity.DAL
{
    public class SchoolInterceptorLogging : DbCommandInterceptor
    {
        private readonly Logging.ILogger _logger = new Logger();
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            _stopwatch.Restart();
            return base.ReaderExecuting(command, eventData, result);
        }

        public override DbDataReader ReaderExecuted(
            DbCommand command,
            CommandExecutedEventData eventData,
            DbDataReader result)
        {
            _stopwatch.Stop();

            _logger.TraceApi("SQL Database", "SchoolInterceptor.ReaderExecuted", _stopwatch.Elapsed, "Command: {0}", command.CommandText);
            return base.ReaderExecuted(command, eventData, result);
        }

        public override InterceptionResult<int> NonQueryExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<int> result)
        {
            _stopwatch.Restart();
            return base.NonQueryExecuting(command, eventData, result);
        }

        public override int NonQueryExecuted(
            DbCommand command,
            CommandExecutedEventData eventData,
            int result)
        {
            _stopwatch.Stop();

            _logger.TraceApi("SQL Database", "SchoolInterceptor.NonQueryExecuted", _stopwatch.Elapsed, "Command: {0}", command.CommandText);

            return base.NonQueryExecuted(command, eventData, result);
        }

        public override InterceptionResult<object> ScalarExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<object> result)
        {
            _stopwatch.Restart();
            return base.ScalarExecuting(command, eventData, result);
        }

        public override object ScalarExecuted(
            DbCommand command,
            CommandExecutedEventData eventData,
            object result)
        {
            _stopwatch.Stop();

            _logger.TraceApi("SQL Database", "SchoolInterceptor.ScalarExecuted", _stopwatch.Elapsed, "Command: {0}", command.CommandText);

            return base.ScalarExecuted(command, eventData, result);
        }
    }
}
