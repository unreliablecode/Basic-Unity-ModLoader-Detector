using System;
using System.IO;
using UnityEngine;

public class AntiCheatSystem : MonoBehaviour
{
    // Base class to define a generic check
    public abstract class CheatCheck
    {
        public abstract bool IsCheatDetected();
    }

    // Derived class to check for specific directories
    public class DirectoryCheck : CheatCheck
    {
        private string directoryPath;

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
        private string filePath;

        public FileCheck(string filePath)
        {
            this.filePath = filePath;
        }

        public override bool IsCheatDetected()
        {
            return File.Exists(filePath);
        }
    }

    // Class to manage and execute all checks
    public class CheatDetectionManager
    {
        private CheatCheck[] checks;

        public CheatDetectionManager(CheatCheck[] checks)
        {
            this.checks = checks;
        }

        public bool IsCheatDetected()
        {
            foreach (var check in checks)
            {
                if (check.IsCheatDetected())
                {
                    return true;
                }
            }
            return false;
        }
    }

    // Unity's Start method where the Anti-Cheat System is initialized
    void Start()
    {
        // Define paths for directories and files to be checked
        string[] directoriesToCheck = {
            Path.Combine(Application.dataPath, "MelonLoader"),
            Path.Combine(Application.dataPath, "BepInEx")
        };

        string[] filesToCheck = {
            Path.Combine(Application.dataPath, "version.dll"),
            Path.Combine(Application.dataPath, "winmm.dll"),
            Path.Combine(Application.dataPath, "winhttp.dll")
        };

        // Create an array of checks
        CheatCheck[] checks = new CheatCheck[directoriesToCheck.Length + filesToCheck.Length];

        // Add directory checks
        for (int i = 0; i < directoriesToCheck.Length; i++)
        {
            checks[i] = new DirectoryCheck(directoriesToCheck[i]);
        }

        // Add file checks
        for (int i = 0; i < filesToCheck.Length; i++)
        {
            checks[directoriesToCheck.Length + i] = new FileCheck(filesToCheck[i]);
        }

        // Initialize the manager with the checks
        CheatDetectionManager cheatDetectionManager = new CheatDetectionManager(checks);

        // If a cheat is detected, close the game
        if (cheatDetectionManager.IsCheatDetected())
        {
            Application.Quit();
        }
    }
}
