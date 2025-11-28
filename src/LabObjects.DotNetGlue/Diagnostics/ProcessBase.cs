using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Timers;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security;
using System.Security.Cryptography;
using LabObjects.DotNetGlue.SharedKernel;

namespace LabObjects.DotNetGlue.Diagnostics
{

    /// <summary>
    /// ProcessBase
    /// <para>Base class for Process based classes.</para>
    /// </summary>
    /// <remarks>
    /// <para>This class assumes that it is not being used on a Windows 98 or Windows Millennium Edition (Windows Me).</para>
    /// <para>This class has not been tested for Processes running on remote machines.</para>
    /// </remarks>
    /// <seealso cref="LabObjects.DotNetGlue.Diagnostics.HostProcess"/>
    /// <seealso cref="LabObjects.DotNetGlue.Diagnostics.Process"/>
    public class ProcessBase : DotNetGlueBase, IDisposable
    {
        #region Constructors
        /// <summary>
        /// ProcessBase
        /// </summary>
        /// <loApi lo_access="public" lo_constructor="true"></loApi>
        /// <param name="process">Proess Object</param>
        /// <seealso cref="LabObjects.DotNetGlue.Diagnostics.HostProcess"/>
        /// <seealso cref="LabObjects.DotNetGlue.Diagnostics.Process"/>
        public ProcessBase(System.Diagnostics.Process process)
        {
            Process = process;
        }
        #endregion

        #region private methods
        /// <summary>GetEncodingFromName
        /// <para></para>Gets an Encoding based on a Name defined by the string encodingName which can be one of the predefined encodings
        /// in the System.Text.Encoding class. When GetEncodingFromName makes the determination it uses a case sensitive comparision of the 
        /// name to one of the following values: ASCII, Unicode, UTF8, UTF32, UTF7, BigEndianUnicode
        /// </summary>
        /// <param name="encodingName">String defining a Encoding name to retrieve</param>
        /// <returns>Encoding or null depending on whether the encodingName matched one of the encodings supported. </returns>
        protected Encoding GetEncodingFromName(string encodingName)
        {
            Encoding enc = null;
            switch (encodingName.ToLower())
            {
                case "ascii":
                    enc = Encoding.ASCII;
                    break;
                case "unicode":
                    enc = Encoding.Unicode;
                    break;
                case "utf7":
                    enc = Encoding.UTF7;
                    break;
                case "utf8":
                    enc = Encoding.UTF8;
                    break;
                case "utf32":
                    enc = Encoding.UTF32;
                    break;
                case "bigendianunicode":
                    enc = Encoding.BigEndianUnicode;
                    break;
                default:
                    enc = null;
                    throw new InvalidEnumArgumentException($"SetErrorOutputEncoding: Encoding not supported: {encodingName}");

            }
            return enc;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// GetFileVersionInfo
        /// </summary>
        protected void GetFileVersionInfo()
        {
            try
            {
                FileVersionInfo fvi = Process.MainModule.FileVersionInfo;
                Comments = fvi.Comments;
                CompanyName = fvi.CompanyName;
                FileDescription = fvi.FileDescription;
                FileMajorPart = fvi.FileMajorPart;
                FileBuildPart = fvi.FileBuildPart;
                FileMinorPart = fvi.FileMinorPart;
                FilePrivatePart = fvi.FilePrivatePart;
                FileVersion = fvi.FileVersion;
                Language = fvi.Language;
                InternalName = fvi.InternalName;
                LegalCopyright = fvi.LegalCopyright;
                LegaTrademarks = fvi.LegalTrademarks;
                OriginalFileName = fvi.OriginalFilename;
                ProductName = fvi.ProductName;
                ProductVersion = fvi.ProductVersion;
            }
            catch (Exception ex)
            {
                SetLastError($"SetFileVersionInfo failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the ProcessStartInfo properties and writes them to this classes properties.
        /// </summary>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo"/>
        protected void GetProcessStartInfo()
        {
            Arguments = Process.StartInfo.Arguments ?? Process.StartInfo.Arguments;
            CreateNoWindow = Process.StartInfo.CreateNoWindow;
            Domain = Process.StartInfo.Domain ?? Process.StartInfo.Domain;
            EnvironmentVariables = Process.StartInfo.EnvironmentVariables ?? Process.StartInfo.EnvironmentVariables;
            ErrorDialog = Process.StartInfo.ErrorDialog;
            FileName = Process.StartInfo.FileName ?? Process.StartInfo.FileName;

            LoadUserProfile = Process.StartInfo.LoadUserProfile;
            Password = Process.StartInfo.Password ?? Process.StartInfo.Password;
            RedirectStandardError = Process.StartInfo.RedirectStandardError;
            RedirectStandardInput = Process.StartInfo.RedirectStandardInput;
            RedirectStandardOutput = Process.StartInfo.RedirectStandardOutput;
            StandardErrorEncoding = Process.StartInfo.StandardErrorEncoding ?? Process.StartInfo.StandardErrorEncoding;
            StandardOutputEncoding = Process.StartInfo.StandardOutputEncoding ?? Process.StartInfo.StandardOutputEncoding;
            UserName = Process.StartInfo.UserName ?? Process.StartInfo.UserName;
            UseShellExecute = Process.StartInfo.UseShellExecute;
            Verb = Process.StartInfo.Verb;
            Verbs = Process.StartInfo.Verbs;

            if (Process.StartInfo != null)
                WindowStyle = Process.StartInfo.WindowStyle;
            WorkingDirectory = Process.StartInfo.WorkingDirectory ?? Process.StartInfo.WorkingDirectory;
        }

        /// <summary>
        /// GetStartupProperties
        /// </summary>
        protected void GetStartupProperties()
        {
            try
            {
                ProcessId = Process.Id;
                Handle = Process.Handle.ToInt32();
                SessionId = Process.SessionId;
                StartTime = Process.StartTime;
                ProcessName = Process.ProcessName;
                ProcessorAffinity = Process.ProcessorAffinity;
                EnvironmentVariables = Process.StartInfo.EnvironmentVariables;
                GetFileVersionInfo();
                RefreshRealtimeProperties();
            }
            catch (Exception ex)
            {
                SetLastError($"SetStartupProperties failed: {ex.Message}");
            }
        }

        /// <summary>
        /// RefreshRealtimeProperties
        /// </summary>
        protected Boolean RefreshRealtimeProperties()
        {
            try
            {
                HandleCount = Process.HandleCount;
                NonpagedSystemMemorySize64 = Process.NonpagedSystemMemorySize64;
                PagedMemorySize64 = Process.PagedMemorySize64;
                PagedSystemMemorySize64 = Process.PagedSystemMemorySize64;
                PeakPagedMemorySize64 = Process.PeakPagedMemorySize64;
                PeakVirtualMemorySize64 = Process.PeakVirtualMemorySize64;
                PeakWorkingSet64 = Process.PeakWorkingSet64;
                PrivateMemorySize64 = Process.PrivateMemorySize64;
                PrivilegedProcessTime = Process.PrivilegedProcessorTime;
                ThreadCount = Process.Threads.Count;
                TotalProcessorTime = Process.TotalProcessorTime;
                UserProcessorTime = Process.UserProcessorTime;
                VirtualMemorySize64 = Process.VirtualMemorySize64;
                WorkingSet64 = Process.WorkingSet64;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>SetErrorOutputEncoding
        /// Sets the process Standard Error Output encoding based on a encoding name. 
        /// Currently supported (case insensitive) values: ASCII, Unicode, UTF8, UTF32, UTF7, BigEndianUnicode
        /// The process can not be running.
        /// </summary>
        /// <param name="encodingName">Case insensitive string defining one of the known Encoding values: ASCII, Unicode, UTF8, UTF32, UTF7, BigEndianUnicode</param>
        /// <returns>True if the encoding was set to the encodingName, otherwise the value is false and LastError and LastErrorDetail contain the details of the failure.</returns>
        public Boolean SetErrorOutputEncoding(string encodingName)
        {
            Encoding enc = null;
            if (!IsRunning)
            {
                try
                {
                    enc = GetEncodingFromName(encodingName);
                    StandardErrorEncoding = enc;
                    return true;
                }
                catch (InvalidEnumArgumentException ex)
                {
                    SetLastError($"Set Error Output Encoding Failed: {ex.Message}");
                    return false;
                }
            }
            else
            {
                SetLastError($"Set Error Output Encoding Failed: Process is Running");
                return false;
            }
        }

        /// <summary>SetOutputEncoding
        /// Sets the process Standard Output encoding based on a encoding name. 
        /// Currently supported (case insensitive) values: ASCII, Unicode, UTF8, UTF32, UTF7, BigEndianUnicode
        /// The process can not be running
        /// </summary>
        /// <param name="encodingName">Case insensitive string defining one of the known Encoding values: ASCII, Unicode, UTF8, UTF32, UTF7, BigEndianUnicode</param>
        /// <returns>True if the encoding was set to the encodingName, otherwise the value is false and LastError and LastErrorDetail contain the details of the failure.</returns>
        public Boolean SetOutputEncoding(string encodingName)
        {
            Encoding enc = null;
            if (!IsRunning)
            {
                try
                {
                    enc = GetEncodingFromName(encodingName);
                    StandardErrorEncoding = enc;
                    return true;
                }
                catch (InvalidEnumArgumentException ex)
                {
                    SetLastError($"Set Output Encoding Failed: {ex.Message}");
                    return false;
                }
            }
            else
            {
                SetLastError($"Set Output Encoding Failed: Process is Running");
                return false;
            }
        }

        /// <summary>
        /// Set the Class properties corresponding to the ProcessStartInfo properties. 
        /// This would typically be called before a the Process is started or 'fetched' (in a GetProcess scenario).
        /// </summary>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo"/>
        protected void SetProcessStartInfo()
        {
            Process.StartInfo.Arguments = Arguments;
            Process.StartInfo.CreateNoWindow = CreateNoWindow;
            Process.StartInfo.Domain = Domain;
            Process.StartInfo.ErrorDialog = ErrorDialog;
            Process.StartInfo.FileName = FileName;
            Process.StartInfo.LoadUserProfile = LoadUserProfile;

            if (Password.Length > 0)
                Process.StartInfo.Password = Password;
            else if (Process.StartInfo.Password != null && Process.StartInfo.Password.Length > 0)
                Process.StartInfo.Password.Clear();

            Process.StartInfo.RedirectStandardError = RedirectStandardError;
            Process.StartInfo.RedirectStandardInput = RedirectStandardInput;
            Process.StartInfo.RedirectStandardOutput = RedirectStandardOutput;
            Process.StartInfo.StandardErrorEncoding = StandardErrorEncoding;
            Process.StartInfo.StandardOutputEncoding = StandardOutputEncoding;
            Process.StartInfo.UserName = UserName;
            Process.StartInfo.UseShellExecute = UseShellExecute;
            Process.StartInfo.Verb = Verb;
            
            Process.StartInfo.WindowStyle = WindowStyle;
            Process.StartInfo.WorkingDirectory = WorkingDirectory;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Dispose 
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// IDisposable.Dispose method overload method.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual new void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    if (Process != null)
                    {
                        if (IsRunning)
                        {
                            Process.Kill();
                            IsRunning = false;
                        }
                        Process.Close();
                        Process.Dispose();
                    }
                }
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// GetEnvironmentVariables Method - gets the environment variables as a two dimensional string
        /// </summary>
        /// <returns>Two dimensional array of strings</returns>
        public string[,] GetEnvironmentVariables()
        {
            int numVars = Process.StartInfo.EnvironmentVariables.Count;
            string[,] stringArray = new string[numVars, 2];
            System.Collections.DictionaryEntry[] dicArray = new System.Collections.DictionaryEntry[numVars];
            Process.StartInfo.EnvironmentVariables.CopyTo(dicArray, 0);
            for (int i = 0; i < numVars; i++)
            {
                stringArray[i, 0] = dicArray[i].Key.ToString();
                stringArray[i, 1] = dicArray[i].Value.ToString();
            }
            return stringArray;
        }

        /// <summary>
        /// Refresh
        /// <para>Calls the Process.Refresh() function and also updates the ProcessBase realtime properties.</para>
        /// </summary>
        /// <seealso cref="Process.Refresh"/>
        /// <returns returnType="System.Boolean">True if successful.</returns>
        public Boolean Refresh()
        {
            try
            {
                if (!Process.HasExited)
                {
                    Process.Refresh();
                    return RefreshRealtimeProperties();
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public Boolean ResetPassword()
        {
            try
            {
                if (Process.StartInfo.Password != null)
                    Process.StartInfo.Password.Clear();
                return true;
            }
            catch (ObjectDisposedException ex)
            {
                SetLastError($"ResetPassword Failed: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                SetLastError($"ResetPassword Failed: {ex.Message}");
            }
            return false;
        }
        /// <summary>
        /// Sets or Clears the internal secure string used for the password. 
        /// If an Empty String is passed ("") the secure password is cleared. If a none-zero length string is passed then the password is set.
        /// The Password will only be used by a process if the User Name and Domain is specified and it is not an Empty or null string (i.e.,Length &gt; 0)
        /// it is recommend that the working directory also be specified else the working directory will be %SYSTEMROOT%\system32
        /// </summary>
        /// <remarks>https://msdn.microsoft.com/en-us/library/system.diagnostics.processstartinfo.password(v=vs.110).aspx</remarks>
        /// <param name="pwd">Process Password</param>
        /// <returns>true if password is set or cleared otherwise false</returns>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.Password"/>
        /// <seealso cref="System.Security.SecureString"/>
        public Boolean SetPassword(string pwd)
        {
            try
            {
                if (pwd == String.Empty && Password.Length > 0)
                {
                    Password.Clear();
                    return true;
                }
                else if (pwd.Length > 0)
                {
                    if (Password.Length > 0)
                        Password.Clear();

                    for (int i = 0; i < pwd.Length; i++)
                        Password.AppendChar(pwd.ElementAt<char>(i));

                    return true;
                }
            }
            catch (CryptographicException ex)
            {
                SetLastError(string.Format("Crytopgraphic Exception: {0}", ex.Message), ex.InnerException);
            }
            catch (InvalidOperationException ex)
            {
                SetLastError(string.Format("Secure String is Read Only: {0}", ex.Message));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                SetLastError(string.Format("String is too long (i.e., > 65,536 characters): {0}", ex.Message));

            }
            catch (Exception ex)
            {
                SetLastError(ex.Message, ex.InnerException);
            }
            return false;
        }

        /// <summary>SetPriorityClass
        /// <para>Sets the process ProrityClass based on a string (priorityClassName) as long as the (case insensitive) string matches a value in the
        /// enumeration ProcessPriorityClass:
        /// Normal, Idle, High, RealTime, AboveNormal, BelowNormal
        /// </para>
        /// </summary>
        /// <param name="priorityClassName"></param>
        /// <returns type="System.Boolean"></returns>
        public Boolean SetPriorityClass(string priorityClassName)
        {

            try
            {
                switch (priorityClassName.ToLower())
                {
                    case "normal":
                        PriorityClass = ProcessPriorityClass.Normal;
                        break;
                    case "idle":
                        PriorityClass = ProcessPriorityClass.Idle;
                        break;
                    case "high":
                        PriorityClass = ProcessPriorityClass.High;
                        break;
                    case "realtime":
                        PriorityClass = ProcessPriorityClass.RealTime;
                        break;
                    case "abovenormal":
                        PriorityClass = ProcessPriorityClass.AboveNormal;
                        break;
                    case "belownormal":
                        PriorityClass = ProcessPriorityClass.BelowNormal;
                        break;
                    default:
                        throw new InvalidEnumArgumentException($"Invalid Priority Class: {priorityClassName}");
                }
                return true;
            }
            catch (PlatformNotSupportedException ex)
            {
                SetLastError($"SetPriorityClass: {ex.Message}");
            }
            catch (Win32Exception ex)
            {
                SetLastError($"SetPriorityClass: {ex.Message}");
            }
            catch (NotSupportedException ex)
            {
                SetLastError($"SetPriorityClass: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                SetLastError($"SetPriorityClass: {ex.Message}");
            }
            catch (InvalidEnumArgumentException ex)
            {
                SetLastError($"SetPriorityClass: {ex.Message}");
            }
            catch (Exception ex)
            {
                SetLastError($"SetPriorityClass: {ex.Message}");
            }
            return false;
        }

        /// <summary>SetProcessorAffinity
        /// <para>Sets the Process affinity for a runnng/non-running process; this method must be used when the process is running.</para>
        /// </summary>
        /// <param name="processorAffinity"></param>
        /// <returns>True if successfully updated Process affinity otherwise false and LastError should be checked.</returns>
        public Boolean SetProcessorAffinity(int processorAffinity)
        {
            try
            { 
                ProcessorAffinity = (IntPtr)processorAffinity;
                return true;
            }
            catch (Win32Exception ex)
            {
                SetLastError($"SetProcessorAffinity: {ex.Message}");
            }
            catch (NotSupportedException ex)
            {
                SetLastError($"SetProcessorAffinity: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                SetLastError($"SetProcessorAffinity: {ex.Message}");
            }
            catch (Exception ex)
            {
                SetLastError($"SetProcessAffinity: {ex.Message}");
            }
            return false;
        }
 
        /// <summary>
        /// Method to set the process WindowStyle based a on string value. 
        /// The string value must be a case insensitive version of one of the ProcessWindowStyle enumerations.
        /// Supported (case-insensitive) values: Normal, Hidden, Maximized, Minimized
        /// </summary>
        /// <param name="windowStyleString"></param>
        /// <returns type="System.Boolean">
        /// True if the process is not running and the value passed can be matched to a ProcessWindowStyle value.
        /// If false is returned, the LastError property will contain the error message.
        /// </returns>
        /// <seealso cref="ProcessBase.WindowStyle"/>
        /// <seealso cref="System.Diagnostics.ProcessWindowStyle"/>
        public Boolean SetWindowStyle(String windowStyleString )
        {
            try
            {
                if (IsRunning)
                    throw new ProcessRunningException();
                switch (windowStyleString.ToLower())
                {
                    case "normal":
                        WindowStyle = ProcessWindowStyle.Normal;
                        break;
                    case "hidden":
                        WindowStyle = ProcessWindowStyle.Hidden;
                        break;
                    case "maximized":
                        WindowStyle = ProcessWindowStyle.Maximized;
                        break;
                    case "minimized":
                        WindowStyle = ProcessWindowStyle.Minimized;
                        break;
                    default:
                        throw new InvalidOperationException($"Invalid Window Style: {windowStyleString}");
                }
                return true;
            }
            catch (InvalidOperationException ex)
            {
                SetLastError($"SetWindowStyle failed: {ex.Message}");
            }
            return false;
        }
        #endregion

        #region  Properties
        /// <summary>
        /// Command line arguments
        /// </summary>
        /// <remarks>Not all Processes have command line arguments available if not set through this class or dervied classes.</remarks>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.Arguments"/>
        public virtual String Arguments { get; set; } = String.Empty;

        /// <summary>
        /// BasePriority
        /// </summary>
        /// <seealso cref="Process.BasePriority"/>
        public Int32 BasePriority { get; protected set; } = 0;

        /// <summary>
        /// Comments
        /// </summary>
        /// <seealso cref="FileVersionInfo.Comments"/>
        /// <remarks>a value of ERROR indicates an error occurred</remarks>
        public String Comments { get; private set; } = String.Empty;

        /// <summary>
        /// CompanyName
        /// </summary>
        /// <seealso cref="FileVersionInfo.CompanyName"/>
        public String CompanyName { get; private set; } = String.Empty;
        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.CreateNoWindow"/>
        public virtual Boolean CreateNoWindow { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.Domain"/>
        public virtual String Domain { get; set; } = null;


        /// <summary>
        /// EnvironmentVariables
        /// </summary>
        /// <seealso cref="ProcessStartInfo.EnvironmentVariables"/>
        public virtual StringDictionary EnvironmentVariables { get; protected set; }

        /// <summary>
        /// Gets or, if private or protected, sets whether an error dialog is displayed if the process can't be started.
        /// </summary>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.ErrorDialog"/>
        public virtual Boolean ErrorDialog { get; set; } = false;

        /// <summary>
        /// The ExitCode for the process run.
        /// </summary>
        /// <seealso cref="System.Diagnostics.Process.ExitCode"/>
        public Int32 ExitCode { get; protected set; }

        public DateTime ExitTime { get; protected set; }
        /// <summary>
        /// FileName
        /// </summary>
        public virtual String FileName { get; set; } = String.Empty;

        /// <summary>
        /// FileDescription
        /// </summary>
        /// <seealso cref="FileVersionInfo.FileDescription"/>
        public String FileDescription { get; protected set; } = String.Empty;

        /// <summary>
        /// FileVersion
        /// </summary>
        /// <seealso cref="FileVersionInfo.FileVersion"/>
        public String FileVersion { get; protected set; } = String.Empty;

        /// <summary>
        /// FileMajorPart
        /// </summary>
        /// <seealso cref="FileVersionInfo.FileMajorPart"/>
        public Int32 FileMajorPart { get; protected set; } = 0;

        /// <summary>
        /// FileMinorPart
        /// </summary>
        /// <seealso cref="FileVersionInfo.FileMinorPart"/>
        public Int32 FileMinorPart { get; protected set; } = 0;

        /// <summary>
        /// FilePrivatePart
        /// </summary>
        /// <seealso cref="FileVersionInfo.FilePrivatePart"/>
        public Int32 FilePrivatePart { get; protected set; } = 0;

        /// <summary>
        /// FileBuildPart
        /// </summary>
        /// <seealso cref="FileVersionInfo.FileBuildPart"/>
        public Int32 FileBuildPart { get; protected set; } = 0;

        /// <summary>
        /// Handle
        /// </summary>
        /// <remarks>A value of -1 indicates the Process is invalid</remarks>
        /// <seealso cref="Process.Handle"/>
        public Int32 Handle { get; protected set; } = -1;

        /// <summary>
        /// HandleCount
        /// </summary>
        public Int32 HandleCount { get; protected set; } = 0;

        public Boolean HasExited { get; protected set; } = false;

        /// <summary>
        /// InternalName
        /// </summary>
        /// <seealso cref="FileVersionInfo.InternalName"/>
        public String InternalName { get; protected set; } = String.Empty;

        /// <summary>
        /// Indicates whether the current provess is running. This can be used to protect from exceptions for setters.
        /// </summary>
        public Boolean IsRunning { get; protected set; }
        /// <summary>
        /// Language
        /// </summary>
        /// <remarks>a value of ERROR indicates an error occurred</remarks>
        public String Language { get; protected set; } = String.Empty;

        /// <summary>
        /// LegalCopyright
        /// </summary>
        /// <remarks>a value of ERROR indicates an error occurred</remarks>
        public String LegalCopyright { get; protected set; } = String.Empty;

        /// <summary>
        /// LegalTrademarks
        /// </summary>
        /// <remarks>a value of ERROR indicates an error occurred</remarks>
        public String LegaTrademarks { get; protected set; } = String.Empty;

        /// <summary>
        /// Gets or, if private or protected, sets whether the User's Windows profile should be loaded on Process start.
        /// The default value is false;
        /// </summary>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.LoadUserProfile"/>
        public virtual Boolean LoadUserProfile { get; set; } = false;

        /// <summary>
        /// NonpagedSystemMemorySize64
        /// </summary>
        /// <seealso cref="Process.NonpagedSystemMemorySize64"/>
        public Int32 NonpagedSystemMemorySize { get { return (NonpagedSystemMemorySize64 > Int32.MaxValue ? Int32.MaxValue : (Int32)NonpagedSystemMemorySize64); } }
        /// <summary>
        /// NonpagedSystemMemorySize64
        /// </summary>
        /// <seealso cref="Process.NonpagedSystemMemorySize64"/>
        public Int64 NonpagedSystemMemorySize64 { get; protected set; } = 0;

        /// <summary>
        /// OriginalFileName
        /// </summary>
        /// <remarks>a value of ERROR indicates an error occurred</remarks>
        public String OriginalFileName { get; protected set; } = String.Empty;

        /// <summary>
        /// The Password to use for the the specified UserName
        /// </summary>
        /// <seealso cref="ProcessBase.SetPassword(string)"/>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.Password"/>
        /// <seealso cref="System.Security.SecureString"/>
        protected SecureString Password { get; set; } = new SecureString();

        /// <summary>
        /// PagedMemorySize
        /// </summary>
        /// <seealso cref="ProcessBase.PagedMemorySize64"/>
        /// <remarks>
        /// This Process property is obsolete but it retained for use in scripting languages that do not support 64 bit integers. 
        /// Used in this context the down-casting of a 64-bit integer may result in issues. 
        /// The intended use of this library does not expect processes that exceed a 32-bit max of 2,147,483,647 (0x7FFFFFFF) 
        /// </remarks>
        public Int32 PagedMemorySize { get { return (PagedMemorySize64 > Int32.MaxValue ? Int32.MaxValue : (Int32)PagedMemorySize64); } }
        /// <summary>
        /// PeakPagedMemorySize64
        /// </summary>
        /// <seealso cref="System.Diagnostics.Process.PagedMemorySize64"/>
        public Int64 PagedMemorySize64 { get; protected set; } = 0;

        /// <summary>
        /// PagedMemorySize
        /// </summary>
        /// <seealso cref="ProcessBase.PagedMemorySize64"/>
        /// <remarks>
        /// This Process property is obsolete but it retained for use in scripting languages that do not support 64 bit integers. 
        /// Used in this context the down-casting of a 64-bit integer may result in issues. 
        /// The intended use of this library does not expect processes that exceed a 32-bit max of 2,147,483,647 (0x7FFFFFFF) 
        /// </remarks>
        public Int32 PagedSystemMemorySize { get { return (PagedSystemMemorySize64 > Int32.MaxValue ? Int32.MaxValue : (Int32)PagedSystemMemorySize64); } }
        /// <summary>
        /// PeakPagedMemorySize64
        /// </summary>
        /// <seealso cref="System.Diagnostics.Process.PagedMemorySize64"/>
        public Int64 PagedSystemMemorySize64 { get; protected set; } = 0;

        /// <summary>
        /// PeakPagedMemorySize
        /// </summary>
        /// <seealso cref="ProcessBase.PeakPagedMemorySize64"/>
        /// <remarks>
        /// This Process property is obsolete but it retained for use in scripting languages that do not support 64 bit integers. 
        /// Used in this context the down-casting of a 64-bit integer may result in issues. 
        /// The intended use of this library does not expect processes that exceed a 32-bit max of 2,147,483,647 (0x7FFFFFFF) 
        /// </remarks>
        public Int32 PeakPagedMemorySize { get { return (PeakPagedMemorySize64 > Int32.MaxValue ? Int32.MaxValue : (Int32)PeakPagedMemorySize64); } }

        /// <summary>
        /// PeakPagedMemorySize64
        /// </summary>
        /// <seealso cref="System.Diagnostics.Process.PeakPagedMemorySize64"/>
        public Int64 PeakPagedMemorySize64 { get; protected set; } = 0;

        /// <summary>
        /// PeakVirtualMemorySize
        /// </summary>
        /// <seealso cref="ProcessBase.PeakVirtualMemorySize64"/>
        /// <remarks>
        /// This Process property is obsolete but it retained for use in scripting languages that do not support 64 bit integers. 
        /// Used in this context the down-casting of a 64-bit integer may result in issues. 
        /// The intended use of this library does not expect processes that exceed a 32-bit max of 2,147,483,647 (0x7FFFFFFF) 
        /// </remarks>
        public Int32 PeakVirtualMemorySize { get { return (PeakVirtualMemorySize64 > Int32.MaxValue ? Int32.MaxValue : (Int32)PeakVirtualMemorySize64); } }

        /// <summary>
        /// PeakVirtualMemorySize64
        /// </summary>
        /// <seealso cref="System.Diagnostics.Process.PeakVirtualMemorySize64"/>
        public Int64 PeakVirtualMemorySize64 { get; protected set; } = 0;

        /// <summary>
        /// PeakWorkingSet
        /// </summary>
        /// <seealso cref="ProcessBase.PeakPagedMemorySize64"/>
        /// <remarks>
        /// This Process property is obsolete but it retained for use in scripting languages that do not support 64 bit integers. 
        /// Used in this context the down-casting of a 64-bit integer may result in issues. 
        /// The intended use of this library does not expect processes that exceed a 32-bit max of 2,147,483,647 (0x7FFFFFFF) 
        /// </remarks>        
        public Int32 PeakWorkingSet { get { return (PeakWorkingSet64 > Int32.MaxValue ? Int32.MaxValue : (Int32)PeakWorkingSet64); } }

        /// <summary>
        /// PeakWorkingSet64
        /// </summary>
        /// <seealso cref="Process.PeakPagedMemorySize64"/>
        public Int64 PeakWorkingSet64 { get; protected set; } = 0;

        // TODO fix PriorityBootEnabled
        /// <summary>
        /// PriorityBoostEnableds
        /// </summary>
        public Boolean PriorityBoostEnabled { get; } = false;

        /// <summary>
        /// ProcessPriorityClass
        /// </summary>
        /// <seealso cref="System.Diagnostics.ProcessPriorityClass"/>
        /// TODO: add a SetPrirority Method (should this be in the base or in the Process?)
        public ProcessPriorityClass PriorityClass { get; protected set; } = ProcessPriorityClass.Normal;

        /// <summary>
        /// PrivateMemorySize
        /// </summary>
        /// <seealso cref="ProcessBase.PrivateMemorySize64"/>
        /// <remarks>
        /// This Process property is obsolete but it retained for use in scripting languages that do not support 64 bit integers. 
        /// Used in this context the down-casting of a 64-bit integer may result in issues. 
        /// The intended use of this library does not expect processes that exceed a 32-bit max of 2,147,483,647 (0x7FFFFFFF) 
        /// </remarks>        
        public Int32 PrivateMemorySize { get { return (PrivateMemorySize64 > Int32.MaxValue ? Int32.MaxValue : (Int32)PrivateMemorySize64); } }
        /// <summary>
        /// PrivateMemorySize64
        /// </summary>
        /// <seealso cref="System.Diagnostics.Process.PrivateMemorySize64"/>
        public Int64 PrivateMemorySize64 { get; protected set; } = 0;

        /// <summary>
        /// PrivilegedProcessTime
        /// </summary>
        public TimeSpan PrivilegedProcessTime { get; protected set; } = TimeSpan.Zero;



        /// <summary>
        /// Process
        /// </summary>
        /// <seealso cref="System.Diagnostics.Process"/>
        protected System.Diagnostics.Process Process { get; set; }



        /// <summary>
        /// Process Id
        /// </summary>
        /// <seealso cref="System.Diagnostics.Process.Id"/>
        public Int32 ProcessId { get; protected set; } = -1;

        /// <summary>
        /// ProcessName
        /// </summary>
        /// <seealso cref="System.Diagnostics.Process.ProcessName"/>
        public String ProcessName { get; protected set; } = String.Empty;
        /// <summary>
        /// ProcessAffinity
        /// </summary>
        //TODO fix Process affinity
        public IntPtr ProcessorAffinity { get; protected set; } = IntPtr.Zero;

        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; protected set; }

        /// <summary>
        /// ProductName
        /// </summary>
        /// <remarks>a value of ERROR indicates an error occurred</remarks>
        public String ProductName { get; protected set; } = String.Empty;

        /// <summary>
        /// ProductVersion
        /// </summary>
        /// <remarks>a value of ERROR indicates an error occurred</remarks>
        public String ProductVersion { get; protected set; } = String.Empty;

        /// <summary>
        /// Gets or, if private or protected, sets a value that indicates whether the error output of an application is written to the Process.StandardError stream.
        /// </summary>
        /// <remarks>This feature depends on the ability of the process being run to generate error output to standard I/O.</remarks>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.RedirectStandardError"/>
        /// <seealso cref="System.Diagnostics.Process.StandardError"/>
        public virtual Boolean RedirectStandardError { get; set; }

        /// <summary>
        /// Gets or, if private or protected, sets a value that indicates whether the error output of an application is written to the Process.StandardInput stream.
        /// </summary>
        /// <remarks>This feature depends on the ability of the process being run to accept input from standard I/O.</remarks>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.RedirectStandardInput"/>
        /// <seealso cref="System.Diagnostics.Process.StandardInput"/>
        public virtual Boolean RedirectStandardInput { get; set; }

        /// <summary>
        /// Gets or, if private or protected, sets a value that indicates whether the error output of an application is written to the Process.StandardOutput stream.
        /// </summary>
        /// <remarks>This feature depends on the ability of the process being run to generate output to standard I/O.</remarks>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.RedirectStandardOutput"/>
        /// <seealso cref="System.Diagnostics.Process.StandardOutput"/>
        public virtual Boolean RedirectStandardOutput { get; set; }

        /// <summary>ProcessAffinity
        /// <paraa>Gets or sets the Processs on which the threads in this process can be scheduled to run when the process is not running. 
        /// When the process is running use the method SetProcessAffinity().</paraa>
        /// </summary>
        public Int32 SessionId { get; protected set; } = -1;

        /// <summary>
        /// Gets or, if this class or an inherited class, sets the preferred encoding for standard error output.
        /// The default value is null.
        /// </summary>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.StandardErrorEncoding"/>
        public Encoding StandardErrorEncoding { get; protected set; } = null;
        /// <summary>
        /// Gets or, if this class or an inherited class, sets the preferred encoding for standard  output.
        /// The default value is null.
        /// </summary>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.StandardOutputEncoding"/>
        public Encoding StandardOutputEncoding { get; protected set; } = null;

        /// <summary>
        /// ThreadCount
        /// </summary>
        public Int32 ThreadCount { get; protected set; } = 0;

        /// <summary>
        /// TotalProcessTime
        /// </summary>
        public TimeSpan TotalProcessorTime { get; protected set; } = TimeSpan.Zero;

        /// <summary>
        /// VirtualMemorySize
        /// </summary>
        public Int32 VirtualMemorySize { get { return (VirtualMemorySize64 > Int32.MaxValue ? Int32.MaxValue : (Int32)VirtualMemorySize64); } }
        /// <summary>
        /// VirtualMemorySize64
        /// </summary>
        public Int64 VirtualMemorySize64 { get; protected set; }

        /// <summary>
        /// The working set memory value for this process.
        /// </summary>
        /// <remarks>
        /// This (Process) property is obsolete and the 64-bit version of this property should be used. 
        /// If the 64-bit version of this property exceeds 32 bits the value returned shall be clamped to the maximum In32 value allowed.
        /// </remarks>
        /// <seealso cref="System.Diagnostics.Process.WorkingSet"/>
        public Int32 WorkingSet { get { return (WorkingSet64 > Int32.MaxValue ? Int32.MaxValue : (Int32)WorkingSet64); } }

        /// <summary>
        /// The working set memory value for this process.
        /// </summary>
        /// <seealso cref="System.Diagnostics.Process.WorkingSet64"/>
        public Int64 WorkingSet64 { get; protected set; } = 0;

        /// <summary>
        /// Gets or sets the Wisdows user account to use to run ethe process. 
        /// </summary>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.UserName"/>
        /// <seealso cref="ProcessBase.SetPassword(string)"/>
        /// <seealso cref="ProcessBase.Domain"/>
        public virtual String UserName { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets whether the process should be run using the operating system shell.
        /// </summary>
        /// <remarks>if the UserName property is null or an empty string, UseShellExecute must be false.</remarks>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.UseShellExecute"/>
        public virtual Boolean UseShellExecute { get; set; } = false;

        /// <summary>
        /// UserProcessTime
        /// </summary>
        public TimeSpan UserProcessorTime { get; protected set; } = TimeSpan.Zero;


        /// <summary>
        /// 
        /// </summary>
        public virtual String Verb { get; set; }

        public String[] Verbs { get; protected set; } 


        /// <summary>
        /// Sets ot gets the Windows Style for the process on startup.
        /// </summary>
        /// <seealso cref="System.Diagnostics.ProcessStartInfo.WindowStyle"/>
        /// <seealso cref="System.Diagnostics.ProcessWindowStyle"/>
        public virtual ProcessWindowStyle WindowStyle { get; set; } = ProcessWindowStyle.Normal;
        /// <summary>
        /// 
        /// </summary>
        public virtual String WorkingDirectory { get; set; } = String.Empty;
    
        #endregion

    }
}
