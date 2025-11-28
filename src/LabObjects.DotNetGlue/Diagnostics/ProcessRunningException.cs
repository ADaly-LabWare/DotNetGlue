using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LabObjects.DotNetGlue.Diagnostics
{
    /// <summary>
    /// <para>Process Running Exception class.</para>
    /// </summary>
    /// <loApi lo_access="internal" lo_datatype="ProcessRunningException"></loApi>
    /// <remarks>
    /// </remarks>
    internal class ProcessRunningException: Exception
    {
        /// <summary>
        /// <para>Creates a ProcessRunningException object;</para>
        /// </summary>
        /// <loApi lo_access="public" lo_constructor="true"></loApi>
        public ProcessRunningException() : base("Process is Running") { }
        /// <summary>
        /// <para>Creates a ProcessRunningException onbkect using a IProcess object.</para>
        /// </summary>
        /// <loApi lo_access="public" lo_constructor="true"></loApi>
        public ProcessRunningException(IProcess Process) 
            : base($"Process is Running: {Process.ProcessName}, Id={Process.ProcessId}") { }

    }
}
