#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SceneManagerTool : EditorWindow
{
    private Vector2 scrollPosition;

    [MenuItem("Tools/Scene Manager")]
    public static void ShowWindow()
    {
        GetWindow<SceneManagerTool>("Scene Manager");
    }

    private void OnGUI()
    {
        GUILayout.Label("Build Settings Scenes", EditorStyles.boldLabel);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

        if (scenes.Length == 0)
        {
            EditorGUILayout.HelpBox("No scenes in Build Settings", MessageType.Info);
            return;
        }

        foreach (var scene in scenes)
        {
            EditorGUILayout.BeginHorizontal();

            // 显示场景启用状态
            bool enabled = scene.enabled;
            enabled = EditorGUILayout.Toggle(enabled, GUILayout.Width(20));

            // 显示场景名称
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);
            EditorGUILayout.LabelField(sceneName);

            // 显示场景路径
            EditorGUILayout.LabelField(scene.path, EditorStyles.miniLabel, GUILayout.Width(300));

            EditorGUILayout.EndHorizontal();

            // 更新场景状态
            if (scene.enabled != enabled)
            {
                scene.enabled = enabled;
                SaveBuildSettings();
            }
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("Changes to scene enable/disable status will take effect immediately. " +
                                "Use the 'Refresh Scene List' button to update inspector dropdowns.",
            MessageType.Info);
    }

    private void SaveBuildSettings()
    {
        EditorBuildSettings.scenes = EditorBuildSettings.scenes;
    }
}
#endif