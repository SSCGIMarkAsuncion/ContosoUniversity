﻿using System;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using ContosoUniversity.Logging;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.SqlClient;

namespace ContosoUniversity.DAL
{
    public class SchoolInterceptorTransientErrors : DbCommandInterceptor
    {
        private int _counter = 0;
        private Logging.ILogger _logger = new Logger();

        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            bool throwTransientErrors = false;
            _logger.Information("{0} {1}", command.Parameters.Count, command.Parameters.ToString());
            if (command.Parameters.Count > 0 && command.Parameters[0].Value.ToString() == "%Throw%")
            {
                throwTransientErrors = true;
                //command.Parameters[0].Value = "%an%";
                //command.Parameters[1].Value = "%an%";
            }

            _logger.Information("ShowTransientErrros {0}", throwTransientErrors);
            if (throwTransientErrors)
            {
                _logger.Information("Returning transient error for command: {0}", command.CommandText);
                _counter++;
                //throw CreateDummySqlException();
            }

            return base.ReaderExecuting(command, eventData, result);
        }

        //private SqlException CreateDummySqlException()
        //{
        //    // The instance of SQL Server you attempted to connect to does not support encryption
        //    var sqlErrorNumber = 20;

        //    var sqlErrorCtor = typeof(SqlError).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Where(c => c.GetParameters().Count() == 7).Single();
        //    var sqlError = sqlErrorCtor.Invoke(new object[] { sqlErrorNumber, (byte)0, (byte)0, "", "", "", 1 });

        //    var errorCollection = Activator.CreateInstance(typeof(SqlErrorCollection), true);
        //    var addMethod = typeof(SqlErrorCollection).GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic);
        //    addMethod.Invoke(errorCollection, new[] { sqlError });

        //    var sqlExceptionCtor = typeof(SqlException).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Where(c => c.GetParameters().Count() == 4).Single();
        //    var sqlException = (SqlException)sqlExceptionCtor.Invoke(new object[] { "Dummy", errorCollection, null, Guid.NewGuid() });

        //    return sqlException;
        //}
    }
}
