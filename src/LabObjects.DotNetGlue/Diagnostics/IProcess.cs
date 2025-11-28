using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace LabObjects.DotNetGlue.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProcess
    {
        string Arguments { get; set; }
        int BasePriority { get; }
        void Close();
        bool CloseMainWindow();
        int CPUTimeMilliseconds { get; }

 


        string Comments { get; }
        string CompanyName { get; }
        bool CreateNoWindow { get; set; }
        bool DidTimeout { get; }


        void Dispose();

        string Domain { get; set; }
        bool DynamicTimeout { get; set; }
        StringDictionary EnvironmentVariables { get; }
        bool ErrorDialog { get; set; }
        int ExitCode { get; }
        string ErrorOutput { get; }
        string ErrorOutputRead();
        DateTime ExitTime { get; }
        int FileBuildPart { get; }
        string FileDescription { get; }
        int FileMajorPart { get; }
        int FileMinorPart { get; }
        string FileName { get; set; }
        int FilePrivatePart { get; }
        string FileVersion { get; }
        string[,] GetEnvironmentVariables();
        int Handle { get; }
        int HandleCount { get; }
        bool HasExited { get; }

        bool InputWrite(string inputData);
        bool InputWriteLine(string inputData);
        string InternalName { get; }

        bool IsErrorAvailable { get; }
        bool IsOutputAvailable { get; }
        bool IsRunning { get; }
        string Key { get; }
        bool Kill();

        string Language { get; }


        string LegalCopyright { get; }
        string LegaTrademarks { get; }
        bool LoadUserProfile { get; set; }
        int NonpagedSystemMemorySize { get; }
        long NonpagedSystemMemorySize64 { get; }
        string OriginalFileName { get; }
        string Output { get; }
        string OutputRead();
        int PagedMemorySize { get; }
        long PagedMemorySize64 { get; }
        int PagedSystemMemorySize { get; }
        long PagedSystemMemorySize64 { get; }
        int PeakPagedMemorySize { get; }
        long PeakPagedMemorySize64 { get; }
        int PeakVirtualMemorySize { get; }
        long PeakVirtualMemorySize64 { get; }
        int PeakWorkingSet { get; }
        long PeakWorkingSet64 { get; }
        bool PriorityBoostEnabled { get; }
        ProcessPriorityClass PriorityClass { get; }
        int PrivateMemorySize { get; }
        long PrivateMemorySize64 { get; }
        TimeSpan PrivilegedProcessTime { get; }
        int ProcessId { get; }
        string ProcessName { get; }
        IntPtr ProcessorAffinity { get; }
        string ProductName { get; }
        string ProductVersion { get; }
        bool RedirectStandardError { get; set; }
        bool RedirectStandardInput { get; set; }
        bool RedirectStandardOutput { get; set; }
        bool Refresh();
        bool ResetPassword();
        bool RunHidden();
        bool RunHidden(string processInput);

        int RunTimeMilliseconds { get; }
        int SessionId { get; }
        bool SetEnvironmentVariable(string name, string value);
        bool SetErrorOutputEncoding(string encodingName);
        bool SetOutputEncoding(string encodingName);
        bool SetPassword(string pwd);
        bool SetPriorityClass(string priorityClassName);
        bool SetProcessorAffinity(int ProcessAffinity);
        bool SetWindowStyle(string windowStyleString);
        Encoding StandardErrorEncoding { get; }
        Encoding StandardOutputEncoding { get; }
        bool Start();
        DateTime StartTime { get; }
        int ThreadCount { get; }

        int TimeoutMilliSeconds { get; set; }

        TimeSpan TotalProcessorTime { get; }
        string UserName { get; set; }
        TimeSpan UserProcessorTime { get; }
        bool UseShellExecute { get; set; }
        string Verb { get; set; }
        string[] Verbs { get; }
        int VirtualMemorySize { get; }
        long VirtualMemorySize64 { get; }
        bool WaitForExit();
        bool WaitForExit(int millseconds);
        ProcessWindowStyle WindowStyle { get; set; }
        string WorkingDirectory { get; set; }
        int WorkingSet { get; }
        long WorkingSet64 { get; }
  

  
 
  
 

 
    }
}