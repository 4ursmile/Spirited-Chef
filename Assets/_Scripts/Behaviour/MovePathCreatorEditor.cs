
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class MovePathCreatorEditor : Editor
{
    // create button to add point to path
    public override void OnInspectorGUI()
    {
        var ctarget = (PathCreator)base.target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Add Out Point"))
        {
            ctarget.MovePathSO.AddOutPath(ctarget.position);
        }
        if (GUILayout.Button("Add In Point"))
        {
            ctarget.MovePathSO.AddInPath(ctarget.position);
        }
    }
}
