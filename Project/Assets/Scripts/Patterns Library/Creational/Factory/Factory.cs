
namespace Patterns.Creational.Factory
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public class Factory : MonoBehaviour {
	    void Start () {
            print("Create Factory!");
            new Client().Main();
        }
    }

    //FACTORY METHOD

    //Ref link https://refactoring.guru/design-patterns/factory-method

    //1 Extract the common interface for all products.This interface should declare methods that make sense for every product.

    //2  Add an empty factory method inside the creator class. Its signature should return the product interface type.

    //3  Go over the creator's code and find all references to product constructors. One by one, replace them with calls to the factory method but extract product creation code to the factory method.
    //   You might need to add a temporary parameter to the factory method that will be used to control which product will be created.
    //   At this point, the factory method's code may look pretty ugly. It may have a large switch operator that picks which product class will be instantiated.
    //   But do not worry, we will fix it right away.

    //4  Now, override the factory method in subclasses and move there the appropriate case from the switch operator in the base method.

    //5 The control parameter used in the base creator's class can also be used in subclasses.
    //For instance, you may have a creator's hierarchy with a base class Mail and classes Air and Ground, plus the product classes: Plane, Truck and Train.
    //Air matches Plane just fine, but Ground matches both Truck and Train at the same time. You can create a new subclass to handle both cases, but there is another option.
    //The client code can pass an argument to the factory method of the Ground class to control which product it receives.

    //6 If the base factory method has become empty after all moves, you can make it abstract.



    //1 Extract the common interface for all products.
    interface IProduct
    {
        //1.1 This interface should declare methods that make sense for every product.
        string Operation();
    }

    //2  Add an empty factory method inside the creator class.
    abstract class ProductCreator
    {
        //2.1 Its signature should return the product interface type.
        public abstract IProduct FactoryMethod();

        //3
        public string SomeOperation()
        {
            var product = FactoryMethod();

            var result = "Creator: The same creator's code has just worked with " + product.Operation();

            return result;
        }

    }

    //4  Now, override the factory method in subclasses and move there the appropriate case from the switch operator in the base method.
    class TruckCreator : ProductCreator
    {
        public override IProduct FactoryMethod()
        {
            return new ConcreteTruck();
        }
    }

    class ShipCreator : ProductCreator
    {
        public override IProduct FactoryMethod()
        {
            return new ConcreteShip();
        }
    }

    class AirPlaneCreator : ProductCreator
    {
        public override IProduct FactoryMethod()
        {
            return new ConcreteAirPlane();
        }
    }


    //5
    class ConcreteTruck : IProduct
    {
        public string Operation()
        {
            return "{Truck created!}";
        }
    }

    class ConcreteShip : IProduct
    {
        public string Operation()
        {
            return "{Ship created!}";
        }
    }

    class ConcreteAirPlane : IProduct
    {
        public string Operation()
        {
            return "{AirPlane created!}";
        }
    }

    //6 CLIENT
    public class Client
    {
        void ClientMethod(ProductCreator creator)
        {
            // ...
            Debug.Log("Client: I'm not aware of the creator's class, but it still works.\n"
                              + creator.SomeOperation());
            // ...
        }

        public void Main()
        {
            Debug.Log("App: Launched with the TruckCreator.");
            ClientMethod(new TruckCreator());

            Debug.Log("");

            Debug.Log("App: Launched with the ShipCreator.");
            ClientMethod(new ShipCreator());
        }

    }

}