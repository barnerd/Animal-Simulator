using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    public State<T> currentState;
    public State<T> previousState;

    public virtual void Initialize(T _owner)
    {
        currentState = GetInitialState();
        if (currentState != null)
            currentState.Enter(_owner);
    }

    public void ExecuteInput(T _owner)
    {
        if (currentState != null)
        {
            State<T> newState;

            newState = currentState.ExecuteInput(_owner);

            if (newState != null)
            {
                ChangeState(_owner, newState);
            }
        }
    }

    public void Execute(T _owner)
    {
        if (currentState != null)
        {
            State<T> newState;

            newState = currentState.Execute(_owner);

            if (newState != null)
            {
                ChangeState(_owner, newState);
            }
        }
    }

    public void ExecutePhysics(T _owner)
    {
        if (currentState != null)
        {
            State<T> newState;

            newState = currentState.ExecutePhysics(_owner);

            if (newState != null)
            {
                ChangeState(_owner, newState);
            }
        }
    }

    protected virtual State<T> GetInitialState()
    {
        return null;
    }

    public void ChangeState(T _owner, State<T> _new)
    {
        //Debug.Log("CHANGE STATE: owner: " + _owner + "; previous: " + previousState + "; current: " + currentState + "; new: " + _new);

        if (currentState == _new) return;

        if(currentState != null)
        {
            currentState.Exit(_owner);
        }

        previousState = currentState;
        currentState = _new;
        currentState.Enter(_owner);
    }
}

public class StateMachine<T, U>
{
    public State<T, U> currentState;
    public State<T, U> previousState;

    public virtual void Initialize(T _owner, U _data)
    {
        currentState = GetInitialState();
        if (currentState != null)
            currentState.Enter(_owner, _data);
    }

    public void ExecuteInput(T _owner, U _data)
    {
        if (currentState != null)
        {
            State<T, U> newState;

            newState = currentState.ExecuteInput(_owner, _data);

            if (newState != null)
            {
                ChangeState(_owner, _data, newState);
            }
        }
    }

    public void Execute(T _owner, U _data)
    {
        if (currentState != null)
        {
            State<T, U> newState;

            newState = currentState.Execute(_owner, _data);

            if (newState != null)
            {
                ChangeState(_owner, _data, newState);
            }
        }
    }

    public void ExecutePhysics(T _owner, U _data)
    {
        if (currentState != null)
        {
            State<T, U> newState;

            newState = currentState.ExecutePhysics(_owner, _data);

            if (newState != null)
            {
                ChangeState(_owner, _data, newState);
            }
        }
    }

    protected virtual State<T, U> GetInitialState()
    {
        return null;
    }

    public void ChangeState(T _owner, U _data, State<T, U> _new)
    {
        //Debug.Log("CHANGE STATE: owner: " + _owner + "; data: " + _data + "; previous: " + previousState + "; current: " + currentState + "; new: " + _new);

        if (currentState == _new) return;

        if (currentState != null)
        {
            currentState.Exit(_owner, _data);
        }

        previousState = currentState;
        currentState = _new;
        currentState.Enter(_owner, _data);
    }
}
