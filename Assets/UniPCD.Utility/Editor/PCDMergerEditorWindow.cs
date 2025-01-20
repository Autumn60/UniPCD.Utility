using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniPCD.Utility.Editor
{
  public class PCDMergerEditorWindow : EditorWindow
  {
    [SerializeField]
    private PCDObject[] sources;

    [MenuItem("UniPCD/Utility/Merge PCD")]
    public static void ShowWindow()
    {
      GetWindow<PCDMergerEditorWindow>("PCD Merger");
    }

    private void OnGUI()
    {
      GUILayout.Label("PCD Merger", EditorStyles.boldLabel);
      SerializedObject so = new SerializedObject(this);
      EditorGUILayout.PropertyField(so.FindProperty("sources"), true);
      so.ApplyModifiedProperties();

      if (sources == null || sources.Length <= 1)
      {
        EditorGUILayout.HelpBox("Please select at least 2 PCD objects to merge.", MessageType.Info);
        return;
      }
      List<PCD> valid_sources = new List<PCD>();
      for (int i = 0; i < sources.Length; i++)
      {
        if (sources[i] == null)
        {
          EditorGUILayout.HelpBox("Please select valid PCD objects to merge.", MessageType.Error);
          return;
        }
        valid_sources.Add(sources[i].pcd);
      }

      EditorGUILayout.Space();
      if (GUILayout.Button("Merge"))
      {
        PCD merged;
        if (PCDMerger.Merge(valid_sources.ToArray(), out merged))
        {
          PCDObject pcdObject = ScriptableObject.CreateInstance<PCDObject>();
          pcdObject.pcd = merged;
          AssetDatabase.CreateAsset(pcdObject, $"Assets/Merged.asset");
          AssetDatabase.SaveAssets();
          AssetDatabase.Refresh();
          Debug.Log($"Merged PCD object is saved as {AssetDatabase.GetAssetPath(pcdObject)}");
        }
        else
        {
          EditorUtility.DisplayDialog("Error", "Failed to merge PCD objects.", "OK");
        }
      }
    }
  }
}