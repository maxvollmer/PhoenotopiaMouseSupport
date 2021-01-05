using UnityEngine;

namespace MouseSupport.Helpers
{
    public class MouseCursor
    {
        private static Vector3 cursorPos = default;
        private static bool isNewCursorPos = false;
        private static int lastCursorUpdateFrame = 0;

        private static Vector3 worldCursorPos = default;
        private static bool isNewWorldCursorPos = false;
        private static int lastWorldCursorUpdateFrame = 0;

        public static Vector2 GetCursorPos(out bool isNewCursorPos)
        {
            if (lastCursorUpdateFrame != Time.frameCount)
            {
                var screenCursorPos = Input.mousePosition;
                screenCursorPos.z = 10;
                var cursorPos = Camera.current.ScreenToWorldPoint(screenCursorPos);

                MouseCursor.isNewCursorPos = cursorPos != MouseCursor.cursorPos;
                MouseCursor.cursorPos = cursorPos;
                MouseCursor.lastCursorUpdateFrame = Time.frameCount;
            }

            isNewCursorPos = MouseCursor.isNewCursorPos;
            return MouseCursor.cursorPos;
        }

        public static Vector2 GetGameWorldCursorPos(out bool isNewCursorPos)
        {
            if (lastWorldCursorUpdateFrame != Time.frameCount)
            {
                var screenCursorPos = Input.mousePosition;
                screenCursorPos.z = 10;
                var worldCursorPos = Camera.main.ScreenToWorldPoint(screenCursorPos);

                MouseCursor.isNewWorldCursorPos = worldCursorPos != MouseCursor.worldCursorPos;
                MouseCursor.worldCursorPos = worldCursorPos;
                MouseCursor.lastWorldCursorUpdateFrame = Time.frameCount;
            }

            isNewCursorPos = MouseCursor.isNewWorldCursorPos;
            return MouseCursor.worldCursorPos;
        }
    }
}
