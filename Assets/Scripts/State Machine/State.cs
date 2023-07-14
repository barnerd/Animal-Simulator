using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// from https://medium.com/c-sharp-progarmming/make-a-basic-fsm-in-unity-c-f7d9db965134

public class State<T>
{
    public string stateName;

    protected StateMachine<T> stateMachine;

    public State(string _name, StateMachine<T> _sm)
    {
        stateName = _name;
        stateMachine = _sm;
    }

    public virtual void Enter(T _owner) { }
    public virtual State<T> ExecuteInput(T _owner) { return null; }
    public virtual State<T> Execute(T _owner) { return null; }
    public virtual State<T> ExecutePhysics(T _owner) { return null; }
    public virtual void Exit(T _owner) { }
}

public class State<T, U>
{
    public string stateName;

    protected StateMachine<T, U> stateMachine;

    public State(string _name, StateMachine<T, U> _sm)
    {
        stateName = _name;
        stateMachine = _sm;
    }

    public virtual void Enter(T _owner, U _data) { }
    public virtual State<T, U> ExecuteInput(T _owner, U _data) { return null; }
    public virtual State<T, U> Execute(T _owner, U _data) { return null; }
    public virtual State<T, U> ExecutePhysics(T _owner, U _data) { return null; }
    public virtual void Exit(T _owner, U _data) { }
}
