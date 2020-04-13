using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Context
{
    // A reference to the current state of the Context.
    private StateMachine _state = null;

    public Context(StateMachine state)
    {
        this.TransitionTo(state);
    }

    // The Context allows changing the State object at runtime.
    public void TransitionTo(StateMachine state)
    {
        Debug.Log($"Context: Transition to {state.GetType().Name}.");
        this._state = state;
        this._state.SetContext(this);
    }

    // The Context delegates part of its behavior to the current State
    // object.
    public void Request1()
    {
        this._state.Handle1();
    }

    public void Request2()
    {
        this._state.Handle2();
    }
}

// The base State class declares methods that all Concrete State should
// implement and also provides a backreference to the Context object,
// associated with the State. This backreference can be used by States to
// transition the Context to another State.
abstract class StateMachine
{
    protected Context _context;

    public void SetContext(Context context)
    {
        this._context = context;
    }

    public abstract void Handle1();

    public abstract void Handle2();
}

// Concrete States implement various behaviors, associated with a state of
// the Context.
class ConcreteStateA : StateMachine
{
    public override void Handle1()
    {
        Debug.Log("ConcreteStateA handles request1.");
        Debug.Log("ConcreteStateA wants to change the state of the context.");
        this._context.TransitionTo(new ConcreteStateB());
    }

    public override void Handle2()
    {
        Debug.Log("ConcreteStateA handles request2.");
    }
}

class ConcreteStateB : StateMachine
{
    public override void Handle1()
    {
        Debug.Log("ConcreteStateB handles request1.");
    }

    public override void Handle2()
    {
        Debug.Log("ConcreteStateB handles request2.");
        Debug.Log("ConcreteStateB wants to change the state of the context.");
        this._context.TransitionTo(new ConcreteStateA());
    }
}

public class StatePattern : MonoBehaviour
{
    private void Start()
    {    
        // The client code.
        var context = new Context(new ConcreteStateA());
        context.Request1();
        context.Request2();
    }
}
