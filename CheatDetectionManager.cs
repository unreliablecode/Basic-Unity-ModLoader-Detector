using UnityEngine;

// Class to manage and execute all checks
public class CheatDetectionManager
{
    private readonly CheatCheck[] checks;

    public CheatDetectionManager(CheatCheck[] checks)
    {
        this.checks = checks;
    }

    public bool RunChecks()
    {
        foreach (var check in checks)
        {
            if (check.IsCheatDetected())
            {
                Debug.LogError($"Cheat detected by {check.GetType().Name}.");
                return true;
            }
        }
        return false;
    }
}
