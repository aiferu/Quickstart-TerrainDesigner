//
//  HEScriptKeywordReplace.cs
//  HEUnityExtensionLib
//
//  Created by YanghuiLiu on 04/09/2015.
//
//

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
#if UNITY_EDITOR
public class HEScriptKeywordReplace : UnityEditor.AssetModificationProcessor
{

    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        int index = path.LastIndexOf(".");
        if (index == -1)
            return;
        string file = path.Substring(index);
        if (file != ".cs" && file != ".js" && file != ".boo") return;
        string fileExtension = file;

        index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;
        file = System.IO.File.ReadAllText(path);

        file = file.Replace("#CREATIONDATE#", System.DateTime.Now.ToString("d"));
        file = file.Replace("#PROJECTNAME#", PlayerSettings.productName);
        file = file.Replace("#SMARTDEVELOPERS#", PlayerSettings.companyName);
        file = file.Replace("#FILEEXTENSION#", fileExtension);

        System.IO.File.WriteAllText(path, file);
        AssetDatabase.Refresh();
    }

}
#endif