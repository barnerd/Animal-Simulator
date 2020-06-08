using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Runtime;

/*
 * Pulled from Ryan Hipple's talk at Unite Austin 2017
 * blog post: https://unity3d.com/how-to/architect-with-scriptable-objects
 * video: https://www.youtube.com/watch?v=raQ3iHhE_Kk&feature=youtu.be
 */
// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
//
// Modifications: https://github.com/Sipstaff/Unity-SO-Events-With-Data
// ----------------------------------------------------------------------------

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
    private GameEvent _event;

    void OnEnable()
    {
        _event = (GameEvent)target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        GUI.enabled = false;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"), true, new GUILayoutOption[0]);
        GUI.enabled = true;

        GUILayout.BeginHorizontal();
        GUILayout.Label("Description");
        _event.Description = GUILayout.TextArea(_event.Description);
        GUILayout.EndHorizontal();

        EditorGUILayout.HelpBox("This Game-Event passes along a Scriptable Object", MessageType.Info, true);
        EditorGUILayout.HelpBox("The receiver of this event can potentially manipulate the Scriptable Object that has been sent", MessageType.Warning, true);

        GUI.enabled = Application.isPlaying;
        GUILayout.BeginHorizontal();
        _event.debugSOParameter = (ScriptableObject)EditorGUILayout.ObjectField("Debug Parameter", _event.debugSOParameter, typeof(ScriptableObject), true);

        if (GUILayout.Button("Raise", GUILayout.Width(40)))
        {
            _event.Raise(_event.debugSOParameter);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        _event.debugMBParameter = (MonoBehaviour)EditorGUILayout.ObjectField("Debug Parameter", _event.debugMBParameter, typeof(MonoBehaviour), true);

        if (GUILayout.Button("Raise", GUILayout.Width(40)))
        {
            _event.Raise(_event.debugMBParameter);
        }
        GUILayout.EndHorizontal();
        GUI.enabled = true;
    }
}
