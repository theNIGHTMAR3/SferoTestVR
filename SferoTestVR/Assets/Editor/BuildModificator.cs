using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildModificator : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }    
    public void OnPostprocessBuild(BuildReport report)
    {        
        string folder = Path.GetDirectoryName(report.summary.outputPath);                
        File.Copy(MapLoader.CONFIG_FILE, folder+"/"+ MapLoader.CONFIG_FILE);
    }
}
