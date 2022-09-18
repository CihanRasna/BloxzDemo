using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Letters")]
public class TableMatrix : SerializedScriptableObject
{
    public int defaultHorizontalCount = 9;
    public int defaultVerticalCount = 12;
    [TableMatrix(HorizontalTitle = "Custom Cell Drawing", DrawElementMethod = "DrawColoredEnumElement", ResizableColumns = false, RowHeight = 16, Transpose = true)]
    public bool[,] CustomCellDrawing;

    private bool[,] savedMatrix;

    private static bool DrawColoredEnumElement(Rect rect, bool value)
    {
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            value = !value;
            GUI.changed = true;
            Event.current.Use();
        }

        UnityEditor.EditorGUI.DrawRect(rect.Padding(1), value ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));

        return value;
    }

    [OnInspectorInit]
    private void CreateData()
    {
        CustomCellDrawing ??= savedMatrix ?? new bool[defaultVerticalCount, defaultHorizontalCount];
    }

    [Button]
    private void SaveData()
    {
        savedMatrix = CustomCellDrawing;
    }
    
    [Button]
    private void ClearDataOrRefresh()
    {
        savedMatrix = null;
        CustomCellDrawing = new bool[defaultHorizontalCount,defaultVerticalCount];
    }
}