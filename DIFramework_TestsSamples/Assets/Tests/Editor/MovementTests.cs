
using NUnit.Framework;

namespace Tests
{
    public class MovementTests {
        [Test]
        public void Movement_Receive_Values_For_Input_AndDeltaTime_AndReturns_True()
        {
            Assert.AreEqual(1, new Movement(1).Calculate(1, 1).x, 0.1f);
        }
    }
}