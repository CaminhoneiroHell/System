

//1 Add to the class a private static field that will hold the singleton instance.

//2 Declare public static creation method that will be used for retrieving singleton instance.

//3 Implement "lazy initialization" inside the creation method.It should create a new instance on the first call and put it into the static field.
//The method should return that instance in all subsequent calls.

//4 Make class constructor private.

//5 In client code replace all direct constructor calls with calls to the static creation method.


namespace Patterns.Creational.Singleton
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    //1 Add to the class a private static field that will hold the singleton instance.
    class Singleton
    {
        //2 Declare public static creation method that will be used for retrieving singleton instance.
        private static Singleton instance;


        //3 Implement "lazy initialization" inside the creation method.It should create a new instance on the first call and put it into the static field.
        //The method should return that instance in all subsequent calls.
        private static object obj = new object();

        //4 Make class constructor private.
        private Singleton() { }

        public static Singleton GetInstace()
        {
            lock (obj)
            {
                if (instance == null)
                    instance = new Singleton();
            }

            return instance;
        }

    }

    class Client
    {
        //5 In client code replace all direct constructor calls with calls to the static creation method.
        public void ClientCode()
        {
            Singleton s1 = Singleton.GetInstace();
            Singleton s2 = Singleton.GetInstace();
            Singleton s3 = Singleton.GetInstace();
            Singleton s4 = Singleton.GetInstace();


            if (s1 == s2 && s3 == s4)
            {
                Debug.Log("Singleton works, both variables contain the same instance.");
            }
            else
            {
                Debug.Log("Singleton failed, variables contain different instances.");
            }

        }
    }

    class Program: MonoBehaviour
    {
        void Start()
        {

            Debug.Log("Singleton start");

            Client client = new Client();
            client.ClientCode();
        }
    }

}
