
using UnityEngine;

namespace MouseSupport.Helpers
{
    public class MenuStateDetector
    {
        public static bool IsInMainMenu()
        {
            return Resources.FindObjectsOfTypeAll<OpeningMenuLogic>().Length > 1;
        }

        public static bool IsInOptionsMenu()
        {
            return PT2.game_paused && !PT2.menu.is_active;
        }

        public static bool IsInInventoryMenu()
        {
            return PT2.game_paused && PT2.menu.is_active;
        }

        public static bool IsInAnyMenu()
        {
            return IsInMainMenu() || IsInOptionsMenu() || IsInInventoryMenu();
        }
    }
}
