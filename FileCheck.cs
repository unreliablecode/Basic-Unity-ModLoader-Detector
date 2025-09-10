using System.IO;

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
