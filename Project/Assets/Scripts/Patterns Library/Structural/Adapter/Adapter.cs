//Adapter is a structural design pattern that allows objects with incompatible interfaces to collaborate.
 
//1 Make sure that you have two actors:
// >Useful service objects.
 
// >Application code that has to use service objects.The application should not be able to use the service objects directly, because of incompatible interfaces or data formats.
 
//2 Declare the client interface that the future adapter class will be following.The application will use this interface to communicate with an adapter.
 
//3 Create an adapter class, make it implementing the client interface, but leave all methods empty for now.
 
//4 Add a field into the adapter class that will store a reference to a service object. In most cases, this field gets the value in a constructor. In simpler cases, the adaptee can be passed directly to the adapter methods.
 
//5 One by one, implement all methods of the client interface in adapter class. These methods should pass execution to appropriate methods of the service object, while converting any passed data to a proper format.
 
//6 Once the adapter class is ready, use it in application code via the client interface

namespace Patterns.Structural.Adapter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    //1 ...
    //2 Declare the client interface that the future adapter class will be following.The application will use this interface to communicate with an adapter.
    interface ITarget //// >Useful service objects.
    {
        string GetRequest();
    }
    class Adaptee // >Application code that has to use service objects.The application should not be able to use the service objects directly, because of incompatible interfaces or data formats.
    {
        public string GetSpecificRequest()
        {
            return "Specific request.";
        }
    }

    //3 Create an adapter class, make it implementing the client interface.
    class Adapter : ITarget
    {
        //4 Add a field into the adapter class that will store a reference to a service object. In most cases, this field gets the value in a constructor.
        //In simpler cases, the adaptee can be passed directly to the adapter methods.

        private readonly Adaptee _adaptee;

        public Adapter(Adaptee adaptee)
        {
            _adaptee = adaptee;
        }

        //5 One by one, implement all methods of the client interface in adapter class.
        //These methods should pass execution to appropriate methods of the service object, while converting any passed data to a proper format.
        public string GetRequest()
        {
            return $"This is '{_adaptee.GetSpecificRequest()}'";
        }
    }
    class Client
    {
        public void Main()
        {
            Adaptee adaptee = new Adaptee();

            ITarget target = new Adapter(adaptee);

            Debug.Log("Adaptee interface is incompatible with the client.");
            Debug.Log("But with adapter client can call it's method.");

            Debug.Log(target.GetRequest());
        }
    }


    class Program: MonoBehaviour
    {
        static void Main(string[] args)
        {
            Debug.Log("Adapter Start!");

            new Client().Main();

        }
    }

}
