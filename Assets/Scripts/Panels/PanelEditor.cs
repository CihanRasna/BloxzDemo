using UnityEditor;

namespace Panels
{
    
    [CustomEditor(typeof(Panel), true)]
    public class PanelEditor : Editor
    {

        private Panel panel;



        #region Life Cycle

        private void OnEnable()
        {
            panel = target as Panel;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Safe Area Settings", EditorStyles.boldLabel);
            var conformation = (Panel.SafeAreaConformation)EditorGUILayout.EnumFlagsField("Conformation", panel.safeAreaConformation);
            if (conformation != panel.safeAreaConformation)
            {
                panel.safeAreaConformation = conformation;
                EditorUtility.SetDirty(target);
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            base.OnInspectorGUI();
        }

        #endregion

    }
}