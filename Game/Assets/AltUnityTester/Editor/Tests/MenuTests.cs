using NUnit.Framework;
using Altom.AltUnityDriver;

public class MenuTests
{
    public AltUnityDriver altUnityDriver;
    //Before any test it connects with the socket
    [OneTimeSetUp]
    public void SetUp()
    {
        altUnityDriver = new AltUnityDriver();
    }

    //At the end of the test closes the connection with the socket
    [OneTimeTearDown]
    public void TearDown()
    {
        altUnityDriver.Stop();
    }

    [Test]
    public void TestStart()
    {
        //Here you can write the test
        altUnityDriver.LoadScene("MainMenu");
        altUnityDriver.FindObject(By.NAME, "StartText").Click();
        altUnityDriver.WaitForCurrentSceneToBe("MainLevel");
    }
}