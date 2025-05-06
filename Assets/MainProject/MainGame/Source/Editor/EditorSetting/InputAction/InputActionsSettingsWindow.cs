using UnityEditor;
using UnityEngine;

namespace MainProject.MainGame.Source.Editor.EditorSetting.InputAction
{
    public class InputActionsSettingsWindow : EditorWindow
    {
        [MenuItem("Tools/Input Actions Settings")]
        private static void ShowWindow()
        {
            var window = GetWindow<InputActionsSettingsWindow>();
            window.titleContent = new GUIContent("Input Actions Settings");
            window.Show();
        }

        private UnityEditor.Editor _editor;

        private void OnEnable()
        {
            _editor = UnityEditor.Editor.CreateEditor(EditorInputActionsDataSettings.instance);
        }
        
        private void OnGUI()
        {
            if(!_editor)
                return;
            
            EditorGUI.BeginChangeCheck();
            _editor.DrawDefaultInspector();
            
            if(EditorGUI.EndChangeCheck())
                EditorInputActionsDataSettings.instance.Save();
        }
    }
}