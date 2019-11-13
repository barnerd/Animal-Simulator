using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapDisplay))]
public class MapDisplayEditor : Editor
{
	public override void OnInspectorGUI()
	{
        MapDisplay mapGen = (MapDisplay)target;

        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.DrawMapInEditor();
            }
        }

		if (GUILayout.Button("Generate Map"))
		{
			mapGen.DrawMapInEditor();
		}
	}
}
