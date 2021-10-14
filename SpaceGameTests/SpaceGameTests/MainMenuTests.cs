using Altom.AltUnityDriver;
using System;
using Xunit;

namespace VoxelGameTests
{
    public class MainMenuTests : IDisposable
    {
        AltUnityDriver altUnityDriver;

        public void Dispose()
        {
            altUnityDriver.Stop();
        }

        public MainMenuTests()
        {
            altUnityDriver = new AltUnityDriver();
            altUnityDriver.LoadScene("MainMenu");
        }

        [Fact]
        public void StartGame()
        {
            altUnityDriver.FindObject(By.NAME, "StartText").Click();
            altUnityDriver.WaitForCurrentSceneToBe("MainLevel");
        }
    }
}
