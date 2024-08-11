using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResourceManager))]
public class ResourceManagerEditor : Editor
{
    // creates a button in Inspector under resource manager
    // runs addresource
    // use to playtest (only works and exists in inspector)
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (ResourceManager) target;
        if (GUILayout.Button("Add resource"))
        {
            script.AddResource(new Resource (ResourceType.Wood,1000));
        }
    }
}
