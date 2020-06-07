using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public virtual void Initialize(GameObject obj) { }
    public abstract void ProcessInput(GameObject obj);
}