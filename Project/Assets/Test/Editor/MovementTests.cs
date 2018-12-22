
using NUnit.Framework;

public class MovementTests {
    [Test]
    public void Input_X_Receive_And_Return_True()
    {
        //Arrange

        //Act

        //Assert
        Assert.AreEqual(1, new Movement(1).Calculate(1, 1).x, 0.1f);
    }
}
