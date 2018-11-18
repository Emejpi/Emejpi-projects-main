using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FreeFormDeformation))]
public class FreeFormDeformationEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FreeFormDeformation myScript = (FreeFormDeformation)target;
        if (GUILayout.Button("Inicialize"))
        {
            myScript.Inicialize();
        }
        if (GUILayout.Button("Reset"))
        {
            myScript.Reset();
        }
        if (GUILayout.Button("Finish"))
        {
            myScript.Finish();
        }
    }
}