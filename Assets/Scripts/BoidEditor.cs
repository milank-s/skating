using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SkaterManager))]
public class BoidEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SkaterManager myTarget = (SkaterManager)target;

         DrawDefaultInspector();

        if(GUILayout.Button("Save Boid Values"))
        {
            myTarget.SaveValuesToPrefab();
        }
    }
}
