
//1 Make sure that you have the common steps of building the product, as well as variations of the steps that lead to the creation of various representations of products.

//2 Create the Builder interface class and declare production steps in it.

//3 Create a Concrete Builder class for each of the product representations.Implement their construction steps.

//4 Think about creating a Director class. Its methods should create different product configurations, using different steps of the same builder instance.

//5 The client code creates both Builder and Director objects. It creates a builder instance first and then passes it either to the director's constructor or its production methods.

//6 The client should call a production method of a Director object to begin the construction process.

//7 The result can be obtained from the Director object only if all products have a common interface. In the opposite case, each Builder must have its own method of retrieving the result.



namespace Patterns.Creational.Builder
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    //1 Make sure that you have the common steps of building the product, as well as variations of the steps that lead to the creation of various representations of products.
    public class Product
    {
        List<object> parts = new List<object>();

        public void Add(string part)
        {
            parts.Add(part);
        }

        public string ListParts()
        {
            string str = string.Empty;

            for (int i = 0; i < parts.Count; i++)
            {
                str += parts[i] + ", ";
            }

            str = str.Remove(str.Length - 2); // removing last ",c"

            return "Product parts: " + str + "\n";
        }
    }


    //2 Create the Builder interface class and declare production steps in it.
    public abstract class Builder
    {
        public abstract void BuildPartA();

        public abstract void BuildPartB();

        public abstract void BuildPartC();

        public abstract Product GetProduct();
    }


    //3 Create a Concrete Builder class for each of the product representations.Implement their construction steps.

    public class ConcreteBuilder : Builder
    {
        Product product = new Product();

        public override void BuildPartA()
        {
            product.Add("PartA1");
        }

        public override void BuildPartB()
        {
            product.Add("PartB1");
        }

        public override void BuildPartC()
        {
            product.Add("PartC1");
        }

        public override Product GetProduct()
        {
            Product result = product;

            this.Reset();

            return result;
        }

        public void Reset()
        {
            product = new Product();
        }
    }

    //4 Think about creating a Director class. Its methods should create different product configurations, using different steps of the same builder instance.
    public class Director
    {
        Builder builder;

        public Director(Builder builder)
        {
            this.builder = builder;
        }

        public void buildMinimalViableProduct()
        {
            builder.BuildPartA();
        }

        public void buildFullFeaturedProduct()
        {
            builder.BuildPartA();
            builder.BuildPartB();
            builder.BuildPartC();
        }
    }

    //5 The client code creates both Builder and Director objects. It creates a builder instance first and then passes it either to the director's constructor or its production methods.
    public class Client
    {
        //6 The client should call a production method of a Director object to begin the construction process.
        public void ClientCode(Director director, Builder builder)
        {
            Debug.Log("Standart basic product:");
            director.buildMinimalViableProduct();
            Debug.Log(builder.GetProduct().ListParts());

            Debug.Log("Standart full featured product:");
            director.buildFullFeaturedProduct();
            Debug.Log(builder.GetProduct().ListParts());

            Debug.Log("Custom product:");
            builder.BuildPartA();
            builder.BuildPartC();
            Debug.Log(builder.GetProduct().ListParts());
        }
    }


    //7 The result can be obtained from the Director object only if all products have a common interface. In the opposite case, each Builder must have its own method of retrieving the result.
    class Program : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("Builder start!");

            Builder builder = new ConcreteBuilder();
            Director director = new Director(builder);

            Client client = new Client();
            client.ClientCode(director, builder);
        }
    }
}
