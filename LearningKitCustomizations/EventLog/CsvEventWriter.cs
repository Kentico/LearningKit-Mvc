using CMS;
using CMS.Base;
using CMS.Core;

using System;
using System.IO;

using EventLogCustomizations;


[assembly: RegisterImplementation(typeof(IEventLogWriter), typeof(CsvErrorEventWriter))]
namespace EventLogCustomizations
{
    public class CsvErrorEventWriter : IEventLogWriter
    {
        public void WriteLog(EventLogData eventLogData)
        {
            if (eventLogData.EventType == EventTypeEnum.Error)
            {
                // Checks if the error event contains an exception
                string exception = eventLogData.Exception != null ? eventLogData.Exception.ToString() : "No exception logged.";

                string eventData = $"Error, {eventLogData.EventCode}, {DateTime.Now}, {eventLogData.EventDescription}, {exception}{Environment.NewLine}";

                // Writes logged error events into a 'errors.csv' file in the application's root directory
                File.AppendAllText(SystemContext.WebApplicationPhysicalPath + "\\errors.csv", eventData);
            }
        }
    }
}