using System.IO;

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
