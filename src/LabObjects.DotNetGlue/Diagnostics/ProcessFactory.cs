using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using LabObjects.DotNetGlue.SharedKernel;


namespace LabObjects.DotNetGlue.Diagnostics
{
    /// <summary>
    /// Process Factory - static helper functions for creating, destroying processes, and tracking processes.
    /// </summary>
    public class ProcessFactory
    {
        private static readonly Dictionary<String, Process> _processDictionary = new Dictionary<String, Process>();

        private static string _lastErrorMessage = "";

        public static int GetProcessCount()
        {
            ProcessScavenger();
            int cnt = _processDictionary.Count;
            return cnt;
        }

        public static string[] GetProcessKeys()
        {
            string[] keysArray;
            try
            {
                ProcessScavenger();
                int numKeys = _processDictionary.Count;
                if (numKeys == 0)
                    return null;

                keysArray = new string[numKeys];
                int n = 0;
                foreach ( var p in _processDictionary)
                {
                    keysArray[n++] = p.Key;
                }
                return keysArray;
            }
            catch (Exception ex)
            {
                SetLastError(ex.Message);
                return null;
            }
        }


        /// <summary>
        /// CreateProcess
        /// </summary>
        /// <returns></returns>
        public static LabObjects.DotNetGlue.Diagnostics.Process CreateProcess()
        {
         
            LabObjects.DotNetGlue.Diagnostics.Process process = new LabObjects.DotNetGlue.Diagnostics.Process(new System.Diagnostics.Process());
            _processDictionary.Add(process.Key, process);

            return process;
        }

        /// <summary>
        /// Destroy all processes. The process will be terminated if it is found to be running (IsRunning=true)
        /// </summary>
        /// <returns>True if all processes destroyed. Check LastError when false.</returns>
        public static Boolean DestroyAll(int milliseconds)
        {
            bool status = true;
            string[] keysArray = _processDictionary.Keys.ToArray();

            foreach (var key in keysArray)
                status &= DestroyProcess(key, milliseconds);

            return status;
        }
        /// <summary>
        /// Destroy all processes. The process will be terminated if it is found to be running (IsRunning=true)
        /// </summary>
        /// <returns>True if all processes destroyed. Check LastError when false.</returns>
        public static Boolean DestroyAll()
        {
            return DestroyAll(10000);
        }

        /// <summary>
        /// Destroy a process. Terminates the process if it found to be running and removed it from the internal dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Boolean DestroyProcess(string key)
        {
            return DestroyProcess(key, 10000);
        }

        /// <summary>
        /// Destroy a process. Terminates the process if it found to be running and removed it from the internal dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="msec"></param>
        /// <returns></returns>
        public static Boolean DestroyProcess(string key, int msec)
        {
            bool status = true;
            try
            {
                if (_processDictionary.ContainsKey(key))
                {
                    try
                    {
                        // get the .Net Glue Process
                        var p = _processDictionary[key];
                        status = ProcessHunter(p,msec);
                        if (status)
                        {
                            _processDictionary.Remove(key);
                        }
                    }
                    catch (Exception ex)
                    {
                        _processDictionary.Remove(key);
                    }

                }
            }
            catch 
            {
                // failed on accessing the item defined by key in the dictionary - it has been removed
            }
            return status;
        }

        private static bool ProcessHunter(DotNetGlue.Diagnostics.Process p, int milliseconds)
        {
            bool status = true;
            try
            {
                if (p != null)
                {
                    bool hasExited = p.HasExited;
                    bool IsRunning = p.IsRunning;
                    int pid = p.ProcessId;
                    string key = p.Key;
                    
                    System.Diagnostics.Process innerProcess = System.Diagnostics.Process.GetProcessById(pid);
                    if (p.IsRunning)
                    {
                        try
                        {

                            bool canCloseMainWindow = p.CloseMainWindow();
                            if (canCloseMainWindow)
                                p.WaitForExit(milliseconds);

                            if (!p.HasExited)
                            {
                                // try again, but wait a shorter time
                                if (canCloseMainWindow)
                                    p.WaitForExit(500);     // half a second

                                if (!p.HasExited)
                                    status = p.Kill();
                            }
                        }
                        catch (Exception ex)
                        {
                           // if an exception - it must be dead
                        }
                    }

                    if (status)
                    {
                      
                        try
                        {
                            p.Close();
                            p.Dispose();
                        }
                        catch
                        {

                        }
                    }
                }
            }
            catch
            { 
                // there should be no exceptions if process is a viable object and if not - the process is destroyed.
            }
            return status;
        }


        /// <summary>
        /// GetProcess
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Process GetProcess(String key)
        {
            if (_processDictionary.ContainsKey(key))
            {
                var p = _processDictionary[key];
                if (p == null)
                {
                    _processDictionary.Remove(key);
                    SetLastError($"Process for key: {key} is null");
                    return null;
                }
                else
                    return p;
            }
            else
            {
                SetLastError($"Process not found for key: {key}");
                return null;
            }
        }

        /// <summary>
        /// GetProcess
        /// </summary>
        /// <param name="key">GUID string that is a key to Process Dictrionary</param>
        /// <returns></returns>
        public static Process GetProcess(Guid key)
        {
            if (key != Guid.Empty)
            {
                return GetProcess(key.ToString());
            }
            else
            {
                SetLastError($"key is not a valid GUID: {key.ToString()}");
                return null;
            }
        }


        private static void ProcessScavenger()
        {
            foreach (var item in _processDictionary)
            {
                if (item.Value == null)
                {
                    try
                    {
                        _processDictionary.Remove(item.Key);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private  static void SetLastError(string errorMessage)
        {
            _lastErrorMessage = errorMessage;
        }

        public static string LastError {  get { return _lastErrorMessage; } }

    }
}
