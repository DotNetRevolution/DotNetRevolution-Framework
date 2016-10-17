using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using System.Threading;
using System.Collections.ObjectModel;

namespace DotNetRevolution.Logging.Serilog.Sink
{
    public class DatabaseSink : PeriodicBatchingSink
    {
        private static readonly Collection<string> IgnoreProperties;
        private const string TableName = "log.Entry";

        private readonly string _connectionString;
        private readonly CancellationTokenSource _token = new CancellationTokenSource();

        static DatabaseSink()
        {
            IgnoreProperties = CreateIgnoreProperties();
        }
        
        public DatabaseSink(string connectionString)
            : base(50, TimeSpan.FromSeconds(5))
        {
            Contract.Requires(string.IsNullOrWhiteSpace(connectionString) == false);

            _connectionString = connectionString;
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            // create new data table
            var table = CreateDataTable();

            // Copy the events to the data table
            FillDataTable(table, events);
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(_token.Token).ConfigureAwait(false);

                using (var copy = CreateSqlBulkCopy(connection))
                {
                    await copy.WriteToServerAsync(table, _token.Token).ConfigureAwait(false);
                }
            }
        }
        
        private static void FillDataTable(DataTable table, IEnumerable<LogEvent> events)
        {
            Contract.Requires(table != null);
            Contract.Requires(events != null);
            Contract.Requires(Contract.ForAll(events, ev => ev?.Properties != null));

            // Add the new rows to the collection. 
            foreach (var logEvent in events)
            {
                Contract.Assume(logEvent?.Properties != null);

                var row = table.NewRow();

                row["Timestamp"] = logEvent.Timestamp.DateTime;
                row["Level"] = logEvent.Level;
                row["SourceContext"] = GetLogEventProperty(logEvent, "SourceContext");
                row["Message"] = GetLogEventProperty(logEvent, "Message");
                row["ApplicationUser"] = GetLogEventProperty(logEvent, "UserName");
                row["SessionId"] = GetLogEventProperty(logEvent, "SessionId");
                row["Properties"] = ConvertPropertiesToXmlStructure(logEvent.Properties);

                if (logEvent.Exception != null)
                {
                    row["Exception"] = logEvent.Exception.ToString();
                }

                table.Rows.Add(row);
            }

            table.AcceptChanges();
        }

        private static string GetLogEventProperty(LogEvent logEvent, string propertyName)
        {
            Contract.Requires(logEvent != null);
            Contract.Requires(logEvent.Properties != null);
            Contract.Ensures(Contract.Result<string>() != null);

            LogEventPropertyValue propertyValue;

            if (logEvent.Properties.TryGetValue(propertyName, out propertyValue))
            {
                Contract.Assume(propertyValue != null);

                var scalarValue = propertyValue as ScalarValue;
                
                if (scalarValue == null)
                {
                    return string.Empty;
                }

                var value = scalarValue.Value;
                Contract.Assume(value != null);

                return value.ToString();
            }

            return string.Empty;
        }

        private static string ConvertPropertiesToXmlStructure(IEnumerable<KeyValuePair<string, LogEventPropertyValue>> properties)
        {
            Contract.Requires(properties != null);
            
            var propertiesToSave = properties.Where(property => !IgnoreProperties.Contains(property.Key))
                                             .ToList();

            if (propertiesToSave.Any() == false)
            {
                return null;
            }

            var sb = new StringBuilder();

            sb.Append("<pp>");

            foreach (var property in propertiesToSave)
            {
                sb.AppendFormat("<p key='{0}'>{1}</p>", property.Key, property.Value);
            }

            sb.Append("</pp>");

            return sb.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            _token.Cancel();
            
            base.Dispose(disposing);
        }
        
        private static Collection<string> CreateIgnoreProperties()
        {
            return new Collection<string>
                {
                    "Message",
                    "MachineName",
                    "SourceContext",
                    "UserName",
                    "SessionId",
                    "Level",
                    "Timestamp"
                };
        }

        private static SqlBulkCopy CreateSqlBulkCopy(SqlConnection connection)
        {
            var copy = new SqlBulkCopy(connection)
            {
                DestinationTableName = TableName
            };

            var columnMappings = copy.ColumnMappings;
            Contract.Assume(columnMappings != null);

            columnMappings.Add("Timestamp", "Timestamp");
            columnMappings.Add("Level", "Level");
            columnMappings.Add("SourceContext", "SourceContext");
            columnMappings.Add("Message", "Message");
            columnMappings.Add("Exception", "Exception");
            columnMappings.Add("ApplicationUser", "ApplicationUser");
            columnMappings.Add("SessionId", "SessionId");
            columnMappings.Add("Properties", "Properties");

            return copy;
        }

        private static DataTable CreateDataTable()
        {
            Contract.Ensures(Contract.Result<DataTable>() != null);

            var eventsTable = new DataTable(TableName);

            eventsTable.Columns.Add(new DataColumn
            {
                DataType = typeof(DateTime),
                ColumnName = "Timestamp"
            });

            eventsTable.Columns.Add(new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "Level"
            });

            eventsTable.Columns.Add(new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "SourceContext"
            });

            eventsTable.Columns.Add(new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "Message"
            });

            eventsTable.Columns.Add(new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "Exception",
                AllowDBNull = true
            });

            eventsTable.Columns.Add(new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "ApplicationUser"
            });

            eventsTable.Columns.Add(new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "SessionId"
            });

            eventsTable.Columns.Add(new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "Properties",
                AllowDBNull = true
            });

            return eventsTable;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_token != null);
        }
    }
}
