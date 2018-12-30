//1 Declare the visitor interface with a set of “visiting” methods, one per each concrete component class that exists in the program.

//2 Declare the component interface. If you’re working with an existing component hierarchy, add the abstract “acceptance” method to the base class of the hierarchy. 
// This method should accept a visitor object as an argument.

//3 Implement the acceptance methods in all concrete components. 
//These methods must simply redirect the call to a visiting method on the incoming visitor object which matches the class of the current component.

//4 The component classes should only work with visitors via the visitor interface. 
//Visitors, however, must be aware of all concrete component classes, referenced as parameter types of the visiting methods.

//5 For each behavior that can’t be implemented inside the component hierarchy, create a new concrete visitor class and implement all of the visiting methods.

//You might encounter a situation where the visitor will need access to some private members of the component class. 
//In this case, you can either make these fields or methods public, violating the component’s encapsulation, or nest the visitor class in the component class. 
//The latter is only possible if you’re lucky to work with a programming language that supports nested classes.

//6 The client must create visitor objects and pass them into components via “acceptance” methods.


namespace Patterns.Behavioral.Visitor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    //The IVisitorAcceptance interface declares a method for “accepting” visitors.This method should have one parameter declared with the type of the visitor interface.
    interface IVisitorAcceptance
    {
        void accept(IVisitor visitor);
    }

    //Each Concrete Component must implement the acceptance method.
    //The purpose of this method is to redirect the call to the proper visitor’s method corresponding to the current component class. 
    //Be aware that even if a base component class implements this method, 
    //all subclasses must still override this method in their own classes and call the appropriate method on the visitor object.
    public class ConcreteComponentA : IVisitorAcceptance
    {
        public void accept(IVisitor visitor)
        {
            visitor.visitConcreteComponentA(this);
        }

        public string exclusiveMethodOfConcreteComponentA()
        {
            return "A";
        }
    }

    public class ConcreteComponentB : IVisitorAcceptance
    {
        public void accept(IVisitor visitor)
        {
            visitor.visitConcreteComponentB(this);
        }

        public string specialMethodOfConcreteComponentB()
        {
            return "B";
        }
    }

    //The Visitor interface declares a set of visiting methods that can take concrete components as arguments.
    //These methods may have the same names if the program is written in a language that supports overloading, but the type of their parameters must be different.
    public interface IVisitor
    {
        void visitConcreteComponentA(ConcreteComponentA el);

        void visitConcreteComponentB(ConcreteComponentB el);
    }

    //Each Concrete Visitor implements several versions of the same behaviors, tailored for different concrete component classes.
    class ConcreteVisitor1 : IVisitor
    {
        public void visitConcreteComponentA(ConcreteComponentA el)
        {
            Debug.Log(el.exclusiveMethodOfConcreteComponentA() + " + ConcreteVisitor1\n");
        }

        public void visitConcreteComponentB(ConcreteComponentB el)
        {
            Debug.Log(el.specialMethodOfConcreteComponentB() + " + ConcreteVisitor1\n");
        }
    }

    class ConcreteVisitor2 : IVisitor
    {
        public void visitConcreteComponentA(ConcreteComponentA el)
        {
            Debug.Log(el.exclusiveMethodOfConcreteComponentA() + " + ConcreteVisitor2\n");
        }

        public void visitConcreteComponentB(ConcreteComponentB el)
        {
            Debug.Log(el.specialMethodOfConcreteComponentB() + " + ConcreteVisitor2\n");
        }
    }

    //The Client usually represents a collection or some other complex object (for example, a Composite tree). 
    //Usually, clients aren’t aware of all the concrete component classes because they work with objects from that collection via some abstract interface.
    internal class Client
    {
        internal static void ClientCode(List<IVisitorAcceptance> components, IVisitor visitor)
        {
            foreach (var component in components)
            {
                component.accept(visitor);
            }
        }
    }


    public class VisitorProgram : MonoBehaviour
    {
        void Start()
        {
            List<IVisitorAcceptance> components = new List<IVisitorAcceptance>
            {
                new ConcreteComponentA(),
                new ConcreteComponentB()
            };

            Debug.Log("The client code works with all visitors via the base Visitor interface:");
            var visitor1 = new ConcreteVisitor1();
            Client.ClientCode(components, visitor1);

            Debug.Log("It allows the same client code to work with different types of visitors:");
            var visitor2 = new ConcreteVisitor2();
            Client.ClientCode(components, visitor2);
        }
    }
}
