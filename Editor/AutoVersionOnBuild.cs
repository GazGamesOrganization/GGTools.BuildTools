using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System;
using System.IO;

public class AutoVersionOnBuild : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder => 0;
    

    string version;

    public void OnPreprocessBuild(BuildReport report)
    {
        version = DateTime.Now.ToString("yyyyMMdd_HHmm");
        PlayerSettings.bundleVersion = version;
        PlayerSettings.companyName = "Gaz Games";
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        var buildPath = report.summary.outputPath;

        var buildFolder = Path.GetDirectoryName(buildPath);

        var parent = Directory.GetParent(buildFolder).FullName;

        string projectName = PlayerSettings.productName;

        string newFolder =
            Path.Combine(parent, $"{version}_{projectName}");

        if (!Directory.Exists(newFolder))
        {
            Directory.Move(buildFolder, newFolder);
        }

    }


    //Method to retrieve the project name without the date. Since the project renaming functionality was removed, I kept the function saved.

#if false
    static string GetBaseProjectName()
    {
        string currentName = PlayerSettings.productName;

        if (System.Text.RegularExpressions.Regex.IsMatch(
            currentName,
            @"^\d{8}_\d{4}_"))
        {
            return currentName.Substring(14);
        }

        return currentName;
    }
#endif
}
