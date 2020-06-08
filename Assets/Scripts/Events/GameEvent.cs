using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

[CreateAssetMenu(menuName = "Game Event/Basic Game Event", order = 0)]
public class GameEvent : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string Description = "";

    // used in the editor to raise test events
    public ScriptableObject debugSOParameter;
    public MonoBehaviour debugMBParameter;
#endif

    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    protected List<GameEventListener> listeners = new List<GameEventListener>();

    public void RegisterListener(GameEventListener _listener)
    {
        if (!listeners.Contains(_listener))
            listeners.Add(_listener);
    }

    public void UnregisterListener(GameEventListener _listener)
    {
        if (listeners.Contains(_listener))
            listeners.Remove(_listener);
    }

    protected void OnDisable()
    {
        listeners.Clear();
    }

    public void Raise(ScriptableObject obj = null)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(obj);
        }
    }

    public void Raise(MonoBehaviour obj)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(obj);
        }
    }
}
