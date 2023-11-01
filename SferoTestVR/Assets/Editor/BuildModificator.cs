using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildModificator : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }    
    public void OnPostprocessBuild(BuildReport report)
    {
        #region copy config file
        string folder = Path.GetDirectoryName(report.summary.outputPath);                
        if(File.Exists(folder + "/" + MapLoader.CONFIG_FILE))
        {
            File.Delete(folder + "/" + MapLoader.CONFIG_FILE);
        }
        File.Copy(MapLoader.CONFIG_FILE, folder+"/"+ MapLoader.CONFIG_FILE);
        #endregion

#if UNITY_EDITOR
        int a = 2;
#endif

        #region create directory with prefab images

        var rooms = MapConfigManager.GetRoomsList();
        foreach (var room in rooms)
        {
            Object roomObject = Resources.Load(MapLoader.FINAL_ROOMS_PATH + room);
            Texture2D preview = null;            
            preview = AssetPreview.GetAssetPreview(roomObject);
            while (AssetPreview.IsLoadingAssetPreview(roomObject.GetInstanceID()))
            {
                preview = AssetPreview.GetAssetPreview(roomObject);
            }

            //first Make sure you're using RGB24 as your texture format            

            //then Save To Disk as PNG
            byte[] bytes = preview.EncodeToPNG();
            var dirPath = folder + "/Previews";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string previewPath = Path.Combine(dirPath, room + ".png");

            if (File.Exists(previewPath))
            {
                File.Delete(previewPath);
            }

            File.WriteAllBytes(previewPath, bytes);
        }
        
        #endregion  
    }
}
