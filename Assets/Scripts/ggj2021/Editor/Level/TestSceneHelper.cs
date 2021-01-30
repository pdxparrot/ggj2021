using pdxpartyparrot.gg2021.Level;

using UnityEditor;

using UnityEngine;

namespace pdxpartyparrot.gg2021.Editor.Level
{
    [CustomEditor(typeof(TestSceneHelper))]
    public sealed class TestSceneHelperEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TestSceneHelper helper = (TestSceneHelper)target;

            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            if(GUILayout.Button("Spawn Sheep")) {
                helper.SpawnSheep();
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}
