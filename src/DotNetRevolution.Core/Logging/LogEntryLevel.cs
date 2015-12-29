namespace DotNetRevolution.Core.Logging
{
    public enum LogEntryLevel
    {
        /// <summary>
        /// Messages that report parameters, settings, etc.
        /// </summary>
        Verbose,

        /// <summary>
        /// Debug messages such as method entry and exit, call stacks, etc.
        /// </summary>
        Debug,

        /// <summary>
        /// Application information
        /// </summary>
        Information,

        /// <summary>
        /// Warnings, alerts, possible unintended results
        /// </summary>
        Warning,

        /// <summary>
        /// Application error
        /// </summary>
        Error,

        /// <summary>
        /// Application crash
        /// </summary>
        Fatal
    }
}