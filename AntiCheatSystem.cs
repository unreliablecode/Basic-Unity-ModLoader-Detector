using UnityEngine;
using System.IO;

public class AntiCheatSystem : MonoBehaviour
{
    // Unity's Start method where the Anti-Cheat System is initialized
    void Start()
    {
        // Define paths and keywords for checks
        string[] directoriesToCheck = { "MelonLoader", "BepInEx" };
        string[] filesToCheck = { "version.dll", "winmm.dll", "winhttp.dll" };
        string[] processKeywordsToCheck = { "MelonLoader", "BepInEx" };
        
        // Use a list for easier management
        var checkList = new System.Collections.Generic.List<CheatCheck>();
        
        // Add directory checks
        foreach(var dir in directoriesToCheck)
        {
            checkList.Add(new DirectoryCheck(Path.Combine(Application.dataPath, "..", dir)));
        }

        // Add file checks
        foreach(var file in filesToCheck)
        {
            checkList.Add(new FileCheck(Path.Combine(Application.dataPath, "..", file)));
        }
        
        // Add the new process module check
        checkList.Add(new ProcessModuleCheck(processKeywordsToCheck));

        // Initialize the manager with all checks
        CheatDetectionManager cheatDetectionManager = new CheatDetectionManager(checkList.ToArray());

        // If a cheat is detected, close the game
        if (cheatDetectionManager.RunChecks())
        {
            // A cheat was found. The process check might have already terminated the offending process.
            // Quitting the game ensures that local file/directory cheats are also handled.
            Debug.LogError("Anti-Cheat system triggered. Quitting application.");
            Application.Quit();
        }
    }
}

