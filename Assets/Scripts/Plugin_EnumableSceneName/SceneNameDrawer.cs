#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 确保属性是字符串类型
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        // 获取Build Settings中的场景名称列表
        List<string> sceneNames = GetSceneNames();

        if (sceneNames.Count == 0)
        {
            EditorGUI.LabelField(position, label.text, "No scenes in Build Settings");
            return;
        }

        // 获取当前值在列表中的索引
        string currentValue = property.stringValue;
        int currentIndex = sceneNames.IndexOf(currentValue);
        if (currentIndex < 0) currentIndex = 0;

        // 创建下拉菜单
        int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, sceneNames.ToArray());

        // 更新属性值
        if (selectedIndex >= 0 && selectedIndex < sceneNames.Count)
        {
            property.stringValue = sceneNames[selectedIndex];
        }
    }

    private List<string> GetSceneNames()
    {
        List<string> names = new List<string>();

        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                // 从路径中提取场景名称
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);
                names.Add(sceneName);
            }
        }

        return names;
    }
}
#endif
