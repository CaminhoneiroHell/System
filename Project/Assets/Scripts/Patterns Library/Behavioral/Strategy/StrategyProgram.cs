
 
//1 Identify an algorithm that the client would prefer to access through a "flex point."
 
//2 Declare the common interface for all variations of the algorithm.
 
//3 One-by-one, extract all algorithms into their own classes.They all should follow the common Strategy interface.
 
//4  Add a field for storing a reference to the current strategy, as well as a setter to change it, into the Context class.
//The Context should work with this object only using the Strategy interface.
 
//5 Context's clients must provide it with a proper strategy objects when they need the Context to perform the work in a certain way.
 
 

namespace Patterns.Behavioral.Strategy
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    //The Context maintains a reference to one of the concrete strategies and communicates with this object only via the strategy interface.
    class Context
    {
        private IStrategy _strategy;

        public Context()
        {

        }

        public Context(IStrategy strategy)
        {
            this._strategy = strategy;
        }

        public void setStrategy(IStrategy strategy)
        {
            this._strategy = strategy;
        }

        public void DoSomeBusinessLogic()
        {
            Debug.Log("Context: Sorting data using the strategy (not sure how it'll do it)\n");
            var result = this._strategy.DoAlgorithm(new List<string> { "a", "b", "c", "d", "e" });

            string result_str = string.Empty;
            foreach (var element in result as List<string>)
            {
                result_str += element + ",";
            }

            Debug.Log(result_str);
        }

    }

    //The Strategy interface is common to all concrete strategies. It declares a method the context uses to execute a strategy.
    interface IStrategy
    {
        object DoAlgorithm(object data);
    }

    //Concrete Strategies implement different variations of an algorithm the context uses.
    class ConcreteStrategyA : IStrategy
    {
        public object DoAlgorithm(object data)
        {
            var list = data as List<string>;
            list.Sort();

            return list;
        }
    }

    class ConcreteStrategyB : IStrategy
    {
        public object DoAlgorithm(object data)
        {
            var list = data as List<string>;
            list.Sort();
            list.Reverse();

            return list;
        }
    }
    //The context calls the execution method on the linked strategy object each time it needs to run the algorithm. 
    //The context doesn’t know what type of strategy it works with or how the algorithm is executed.



    /*****************************************************************************************************************************************/
    //The Client creates a specific strategy object and passes it to the context. 
    //The context exposes a setter which lets clients replace the strategy associated with the context at runtime.
    class Client
    {
        public static void ClientCode()
        {
            var context = new Context();

            Debug.Log("Client: Strategy is set to normal sorting.\n");
            context.setStrategy(new ConcreteStrategyA());
            context.DoSomeBusinessLogic();
            Debug.Log("\n");
            Debug.Log("Client: Strategy is set to reverse sorting.\n");
            context.setStrategy(new ConcreteStrategyB());
            context.DoSomeBusinessLogic();

            Debug.Log("\n");
        }
    }


    class StrategyProgram : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("Start Strategy!");

            Client.ClientCode();
        }
    }

}
