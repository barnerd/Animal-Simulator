using UnityEngine;
using UnityEngine.Events;
/*
 * Pulled from Ryan Hipple's talk at Unite Austin 2017
 * blog post: https://unity3d.com/how-to/architect-with-scriptable-objects
 * video: https://www.youtube.com/watch?v=raQ3iHhE_Kk&feature=youtu.be
 */

[System.Serializable]
public class ScriptablebjectUnityEvent : UnityEvent<ScriptableObject> { }

[System.Serializable]
public class MonoBehaviourUnityEvent : UnityEvent<MonoBehaviour> { }

public class GameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public ScriptablebjectUnityEvent SOResponse;

    [Tooltip("Response to invoke when Event is raised.")]
    public MonoBehaviourUnityEvent MBResponse;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(ScriptableObject obj = null)
    {
        SOResponse.Invoke(obj);
    }

    public void OnEventRaised(MonoBehaviour obj = null)
    {
        MBResponse.Invoke(obj);
    }
}
