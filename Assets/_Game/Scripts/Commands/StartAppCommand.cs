using UnityEngine;

namespace Scripts.Commands
{
    public class StartAppCommand
    {
        public void Execute()
        {
            ConfigureApp();
        }

        private void ConfigureApp()
        {
            Application.targetFrameRate = 60;   
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}