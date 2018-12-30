namespace Patterns.Behavioral.State
{
    using UnityEngine;

    //Context stores a reference to one of the concrete state objects and delegates to it all state-specific work.
    //The context communicates with the state object via the state interface. The context exposes a setter for passing it a new state object.
    class Context
    {
        private State _state = null;

        public Context(State state)
        {
            this.transitionTo(state);
        }

        public void transitionTo(State state)
        {
            Debug.Log("Context: Transition to " + state.ToString() + ".\n");
            this._state = state;
            this._state.setContext(this);
        }

        public void request1()
        {
            this._state.handle1();
        }

        public void request2()
        {
            this._state.handle2();
        }
    }

    //The State interface declares the state-specific methods.
    //These methods should make sense for all concrete states because you don’t want some of your states to have useless methods that will never be called.
    abstract class State
    {
        protected Context context;

        public void setContext(Context context)
        {
            this.context = context;
        }

        public abstract void handle1();

        public abstract void handle2();
    }

    //Concrete States provide their own implementations for the state-specific methods.
    //To avoid duplication of similar code across multiple states, you may provide intermediate abstract classes that encapsulate some common behavior.

    //State objects may store a backreference to the context object.
    //Through this reference, the state can fetch any required info from the context object, as well as initiate state transitions.

    class ConcreteStateA : State
    {
        public override void handle1()
        {
            Debug.Log("ConcreteStateA handles request1.\n");
            Debug.Log("ConcreteStateA wants to change the state of the context.\n");
            this.context.transitionTo(new ConcreteStateB());
        }

        public override void handle2()
        {
            Debug.Log("ConcreteStateA handles request2.\n");
        }
    }

    class ConcreteStateB : State
    {
        public override void handle1()
        {
            Debug.Log("ConcreteStateB handles request1.\n");
        }

        public override void handle2()
        {
            Debug.Log("ConcreteStateB handles request2.\n");
            Debug.Log("ConcreteStateB wants to change the state of the context.\n");
            this.context.transitionTo(new ConcreteStateA());
        }
    }

    //Both context and concrete states can set the next state of the context and perform the actual state transition by replacing the state object linked to the context.

    class Client
    {
        public static void ClientCode()
        {
            var context = new Context(new ConcreteStateA());
            context.request1();
            context.request2();
        }
    }


    public class StateProgram : MonoBehaviour
    {
        void Start()
        {
            Client.ClientCode();
        }
        
    }
}

