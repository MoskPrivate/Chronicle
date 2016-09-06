using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapPreviewEditor : Editor {

    /*public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;
        if (DrawDefaultInspector())
        {
            mapGen.GenerateMap();
        }
        if(GUILayout.Button("Preview"))
        {
            mapGen.GenerateMap();
        }
    }*/
}
