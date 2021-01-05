using UnityEngine;

namespace MouseSupport.Helpers
{
    public class WindowState : MonoBehaviour
    {
        private static bool hasFocus = true;
        private static bool isPaused = false;

        public static bool IsActive { get { return hasFocus && !isPaused; } }

        public void OnApplicationFocus(bool hasFocus)
        {
            WindowState.hasFocus = hasFocus;
        }

        public void OnApplicationPause(bool isPaused)
        {
            WindowState.isPaused = isPaused;
        }
    }
}
