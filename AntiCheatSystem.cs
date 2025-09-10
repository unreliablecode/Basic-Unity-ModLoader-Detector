using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;

public class AntiCheatSystem : MonoBehaviour
{
    #region Native Methods
    // Imports the EnumProcesses function from psapi.dll to enumerate all running processes.
    [DllImport("psapi.dll")]
    private static extern bool EnumProcesses(int[] pProcessIds, int cb, out int pBytesReturned);

    // Imports the OpenProcess function from kernel32.dll to get a handle to a process.
    [DllImport("kernel32.dll")]
    private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    // Imports the EnumProcessModules function from psapi.dll to enumerate all modules in a process.
    [DllImport("psapi.dll")]
    private static extern bool EnumProcessModules(IntPtr hProcess, IntPtr[] lphModule, int cb, out int lpcbNeeded);
    
    // Imports the GetModuleBaseName function from psapi.dll to get the name of a module.
    [DllImport("psapi.dll")]
    private static extern uint GetModuleBaseName(IntPtr hProcess, IntPtr hModule, StringBuilder lpBaseName, uint nSize);
    
    // Imports the TerminateProcess function from kernel32.dll to terminate a process.
    [DllImport("kernel32.dll")]
    private static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

    // Imports the CloseHandle function from kernel32.dll to close an open object handle.
    [DllImport("kernel32.dll")]
    private static extern bool CloseHandle(IntPtr hObject);

    // Constants for process access rights
    private const uint PROCESS_QUERY_INFORMATION = 0x0400;
    private const uint PROCESS_VM_READ = 0x0010;
    private const uint PROCESS_TERMINATE = 0x0001;
    #endregion

    #region Cheat Check Classes
    // Base class to define a generic check
    public abstract class CheatCheck
    {
        public abstract bool IsCheatDetected();
    }

    // Derived class to check for specific directories
    public class DirectoryCheck : CheatCheck
    {
        private readonly string directoryPath;

        public DirectoryCheck(string directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        public override bool IsCheatDetected()
        {
            return Directory.Exists(directoryPath);
        }
    }

    // Derived class to check for specific files
    public class FileCheck : CheatCheck
    {
        private readonly string filePath;

        public FileCheck(string filePath)
        {
            this.filePath = filePath;
        }

        public override bool IsCheatDetected()
        {
            return File.Exists(filePath);
        }
    }
    
    // NEW: Derived class to check running processes for specific modules
    public class ProcessModuleCheck : CheatCheck
    {
        private readonly string[] moduleKeywords;

        public ProcessModuleCheck(string[] keywords)
        {
            moduleKeywords = keywords;
        }

        public override bool IsCheatDetected()
        {
            int[] processIds = new int[1024];
            if (!EnumProcesses(processIds, sizeof(int) * processIds.Length, out int bytesReturned))
            {
                // Failed to enumerate processes
                return false;
            }

            int numProcesses = bytesReturned / sizeof(int);

            for (int i = 0; i < numProcesses; i++)
            {
                IntPtr hProcess = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ | PROCESS_TERMINATE, false, processIds[i]);
                if (hProcess == IntPtr.Zero) continue;
                
                try
                {
                    IntPtr[] moduleHandles = new IntPtr[1024];
                    if (EnumProcessModules(hProcess, moduleHandles, sizeof(IntPtr) * moduleHandles.Length, out int modulesBytesReturned))
                    {
                        int numModules = modulesBytesReturned / sizeof(IntPtr);
                        for (int j = 0; j < numModules; j++)
                        {
                            StringBuilder moduleName = new StringBuilder(256);
                            if (GetModuleBaseName(hProcess, moduleHandles[j], moduleName, (uint)moduleName.Capacity) > 0)
                            {
                                string name = moduleName.ToString().ToLower();
                                foreach (var keyword in moduleKeywords)
                                {
                                    if (name.Contains(keyword.ToLower()))
                                    {
                                        Debug.LogWarning($"Detected '{keyword}' in process ID {processIds[i]} ({name}). Terminating process.");
                                        TerminateProcess(hProcess, 1);
                                        return true; // Cheat detected and handled
                                    }
                                }
                            }
                        }
                    }
                }
                finally
                {
                    // Ensure the handle is always closed
                    CloseHandle(hProcess);
                }
            }
            return false; // No cheat modules found in any process
        }
    }
    #endregion
}
