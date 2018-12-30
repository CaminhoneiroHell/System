
//Observer is a behavioral design pattern that lets you define a subscription mechanism to notify multiple objects about any events that happen to the object they’re observing.

//1 Differentiate between the core(or independent) functionality and the optional(or dependent) functionality.The former will act as a publisher, and the later will serve as subscribers.

//2 Create the Subscriber interface. In most cases, a single update method is enough.

//3 Create the Publisher interface and describe the operations for starting and terminating subscription in it.Remember that publisher should work with subscribers via their common interface.

//4 You have to decide where you put the actual subscription list and the implementation of the subscribtion methods.
//Usually, this code looks the same for all types of publishers. So the obvious place to put it is a base abstract class derived directly from a Publisher interface.
//But if you are integrating Observer to an existing class hierarchy, it might be more convenient to create a small helper class that will be maintaining the subscription for specific publishers.

//4 Create Concrete Publisher classes.They should send notifications to the whole list of subscribers each time when something important happens inside the object.

//5 Implement update methods in Concrete Subscribers. Most subscribers would need some context data about the event.
//It can be passed as an argument to the update method. But there is another option.
//Upon receiving a notification, the subscriber can fetch any data directly from the publisher object.
//In this case, the publisher must pass itself via the update method.The less flexible option is to link a publisher to the subscriber permanently via the constructor.

//6 The Client code must create all necessary subscribers and register them with proper publishers

namespace Patterns.Behavioral.Observer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    //2 Create the Subscriber interface. In most cases, a single update method is enough.
    public interface ISplObserver{
        void update(ISplSubject subject);
    }

    //3 Create the Publisher interface and describe the operations for starting and terminating subscription in it.Remember that publisher should work with subscribers via their common interface.
    public interface ISplSubject{
        void attach(ISplObserver observer);

        void detach(ISplObserver observer);

        void notify();
    }

    //4 Create Concrete Publisher classes.They should send notifications to the whole list of subscribers each time when something important happens inside the object.
    public class Subject : ISplSubject  //******PUBLISHER*******
    {
        public int State { get; set; } = -0;

        private List<ISplObserver> _observers = new List<ISplObserver>();

        public void attach(ISplObserver observer)
        {
            Debug.Log("Subject: Attached an observer.\n");
            this._observers.Add(observer);
        }

        public void detach(ISplObserver observer)
        {
            foreach (var elem in _observers)
            {
                if (elem == observer)
                {
                    _observers.Remove(observer);
                    Debug.Log("Subject: Detached an observer.\n");
                    break;
                }
            }
        }

        public void notify()
        {
            Debug.Log("Subject: Notifying observers...\n");

            foreach (var observer in _observers)
            {
                observer.update(this);
            }
        }


        public IEnumerator someBusinessLogic()
        {
            Debug.Log("\nSubject: I'm doing something important.\n");
            this.State = Random.Range(0,10);

            yield return new WaitForSeconds(2);
            //Thread.Sleep(15);

            Debug.Log("Subject: My state has just changed to: " + this.State + "\n");
            this.notify();
            
        }

    }


    class ConcreteObserverA : ISplObserver
    {
        public void update(ISplSubject subject)
        {
            if (!(subject is Subject))
            {
                return;
            }

            if ((subject as Subject).State < 3)
            {
                Debug.Log("ConcreteObserverA: Reacted to the event.\n");
            }
        }
    }

    class ConcreteObserverB : ISplObserver
    {
        public void update(ISplSubject subject)
        {
            if (!(subject is Subject))
            {
                return;
            }

            if ((subject as Subject).State == 0 || (subject as Subject).State >= 2)
            {
                Debug.Log("ConcreteObserverB: Reacted to the event.\n");
            }
        }
    }

    class Observer: MonoBehaviour
    {
        void Start()
        {
            Debug.Log("Start Observer!");
            var subj = new Subject();
            var o1 = new ConcreteObserverA();
            subj.attach(o1);

            var o2 = new ConcreteObserverB();
            subj.attach(o2);

            StartCoroutine(subj.someBusinessLogic());

            //subj.detach(o2);

            //StartCoroutine(subj.someBusinessLogic());

        }
    }
}
