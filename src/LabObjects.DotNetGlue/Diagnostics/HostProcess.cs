using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using LabObjects.DotNetGlue.SharedKernel;

namespace LabObjects.DotNetGlue.Diagnostics
{
    /// <summary>
    /// HostProcess Class - a class that wraps a Process Host Process 
    /// and serves as a Process Factory and Dictionary.
    /// 
    /// <para>As a class and dictionary, hosts
    /// can create Processs with an assigned key. The dictionary may be queried for a Process
    /// using the method: GetProcess(key). </para>
    /// </summary>
    /// <remarks>
    /// <para>Inherits from DotNetGlueBase.</para>
    /// <para>Many HostProcess Process properties are exposed as 'getters' to avoid destabalizing hosts</para>
    /// <para>This class can also be used on "Exit" of a Host application to check for running processes and 
    /// termiate them if needed.</para>
    /// <para>Note: use of Exited event for the host is not applicable - the host termination also ends the lifetime 
    /// of this class.</para>
    /// <para>Host Processes that have not been started with the Component Model may not have all STartInfo information available.</para>
    /// </remarks>
    /// <seealso cref="System.Diagnostics.Process"/>
    /// <seealso cref="LabObjects.DotNetGlue.Diagnostics.Process"/> 
    /// <seealso cref="LabObjects.DotNetGlue.Diagnostics.ProcessBase"/>
    /// <seealso cref="LabObjects.DotNetGlue.SharedKernel.DotNetGlueBase"/>
    /// <namespace>LabObjects.DotNetGlue.Diagnostics</namespace>
    public  class HostProcess:ProcessBase
    {
        //private readonly Process _hostProcess;
        //private readonly Dictionary<String, Process> _processDictionary = new Dictionary<String, Process>();
        //private static Dictionary<String, Process> _processDictionary = new Dictionary<String, Process>();

        #region Constructors
        /// <summary>
        /// <para>Creates a HostProcess for the host library hosting application using .Net <c>System.Diagnostics.Process</c> <c>GetCurrentProcess()</c> method.</para>
        /// </summary>
        /// <loApi lo_access="public" lo_constructor="true"></loApi>
        /// <seealso cref="System.Diagnostics.Process"/>
        /// <seealso cref="System.Diagnostics.Process.GetCurrentProcess"/>
        /// <seealso cref="LabObjects.DotNetGlue.Diagnostics.ProcessBase"/>
        /// <seealso cref="LabObjects.DotNetGlue.Diagnostics.Process"/>
        public HostProcess():base(System.Diagnostics.Process.GetCurrentProcess())
        {
            GetFileVersionInfo();
            GetProcessStartInfo();
            GetStartupProperties();
            RefreshRealtimeProperties();
        }
        #endregion

        #region Public Properties

 
        public override String Arguments {  get { return base.Arguments; } }

        public override Boolean CreateNoWindow { get => base.CreateNoWindow;  }

        public override String Domain { get => base.Domain; }

        public override Boolean ErrorDialog { get => base.ErrorDialog;  }

        public override String FileName { get => base.FileName;  }

   

        /// <summary>
        /// 
        /// </summary>
        public override Boolean LoadUserProfile { get => base.LoadUserProfile; }

        public override Boolean RedirectStandardError { get => base.RedirectStandardError; }

        public override Boolean RedirectStandardInput { get => base.RedirectStandardInput; }

        public override Boolean RedirectStandardOutput { get => base.RedirectStandardOutput; }
        /// <summary>
        /// Gets the Wisdows user account to use to run ethe process. 
        /// </summary>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.UserName"/>
        /// <seealso cref="ProcessBase.SetPassword(string)"/>
        /// <seealso cref="ProcessBase.Domain"/>
        public override String UserName { get { return base.UserName; } }

        /// <summary>
        /// Gets  whether the process should be run using the operating system shell.
        /// </summary>
        /// <remarks>if the UserName property is null or an empty string, UseShellExecute must be false.</remarks>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.UseShellExecute"/>
        public override Boolean UseShellExecute { get { return base.UseShellExecute; } }

        public override ProcessWindowStyle WindowStyle { get => base.WindowStyle; }
        /// <summary>
        /// 
        /// </summary>
        public override String WorkingDirectory {  get { return base.WorkingDirectory; } }
  
        #endregion


    //    #region Public Methods
    //    /// <summary>
    //    /// CreateProcess
    //    /// </summary>
    //    /// <returns></returns>
    //    public static LabObjects.DotNetGlue.Diagnostics.Process CreateProcess()
    //    {
 
    //        // factories can also provide overloads allowing for fileName, etc.          
    //        //System.Diagnostics.Process process = new System.Diagnostics.Process();
    //        //LabObjects.DotNetGlue.Diagnostics.Process Process = new LabObjects.DotNetGlue.Diagnostics.Process(process);
    //        //_processDictionary.Add(Process.Key, Process);
    //        //return Process;
    //        LabObjects.DotNetGlue.Diagnostics.Process process = new LabObjects.DotNetGlue.Diagnostics.Process(new System.Diagnostics.Process());
    //        _processDictionary.Add(process.Key, process);

    //        return process;
    //    }

    //    /// <summary>
    //    /// GetProcess
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns></returns>
    //    public static Process GetProcess(String key)
    //    {
    //        if (_processDictionary.ContainsKey(key))
    //        {
    //            var p = _processDictionary[key];
    //            if (p == null)
    //            {
    //                _processDictionary.Remove(key);
    //                SetLastError($"Process for key: {key} is null");
    //                return null;
    //            }
    //            else
    //                return p;
    //        }
    //        else
    //        {
    //            SetLastError($"Process not found for key: {key}");
    //            return null;
    //        }
    //    }

    //    /// <summary>
    //    /// GetProcess
    //    /// </summary>
    //    /// <param name="key">GUID string that is a key to Process Dictrionary</param>
    //    /// <returns></returns>
    //    public static Process GetProcess(Guid key)
    //    {
    //        if (key != Guid.Empty)
    //        {
    //            return GetProcess(key.ToString());
    //        }
    //        else
    //        {
    //            SetLastError($"key is not a valid GUID: {key.ToString()}");
    //            return null;
    //        }
    //    }
    //    #endregion


    //    #region private methods
    //    private static void ProcessScavenger()
    //    {
    //        foreach (var item in _processDictionary)
    //        {
    //            if (item.Value == null)
    //                _processDictionary.Remove(item.Key);                   
    //        }
    //    }
    //    #endregion

    }
}
