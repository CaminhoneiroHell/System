using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.Build;
using NSubstitute;
//using Zenject;

namespace Tests
{
    public class MovementTester
    {
        // A Test behaves as an ordinary method
        [Test]
        public void MovementTesterSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator MovementTesterWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.

            ////Arrange
            //var player = new GameObject().AddComponent<Player>();
            //var uService = Substitute.For<IUnityService>();
            //player.unityService = uService;

            ////Act
            //player.speed = 1;
            //uService.GetAxis("Horizontal").Returns(1);
            //uService.GetDeltaTime().Returns(1);

            yield return null; // skips 1 frame

            ////Assert
            //Assert.AreEqual(1, player.transform.position.x);

        }
    }
}
