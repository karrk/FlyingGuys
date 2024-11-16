using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 프로젝트의 모든 설정까지 같이 빌드하기 위한 설정
/// </summary>
public static class ExportPackage
{
    [MenuItem("Export/Export with tags and layers, Input settings")]
    public static void export()
    {
        string[] projectContent = new string[] { "Assets", "ProjectSettings/TagManager.asset", "ProjectSettings/InputManager.asset", "ProjectSettings/ProjectSettings.asset" };
        AssetDatabase.ExportPackage(projectContent, "Done.unitypackage", ExportPackageOptions.Interactive | ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
        Debug.Log("Project Exported");
    }
}