
using HarmonyLib;
using UnityEngine;
using MouseSupport.Helpers;

namespace MouseSupport.Patches
{
    public class ControlAdapterPatch
    {
        [HarmonyPatch(typeof(ControlAdapter), "Update")]
        public class ControlAdapterUpdatePatch
        {
            [HarmonyPostfix]
            public static void Postfix(ControlAdapter __instance)
            {
                if (MenuStateDetector.IsInAnyMenu())
                {
                    __instance.OPTIONS_MENU_DOWN = __instance.OPTIONS_MENU_DOWN || Input.mouseScrollDelta.y < 0;
                    __instance.OPTIONS_MENU_UP = __instance.OPTIONS_MENU_UP || Input.mouseScrollDelta.y > 0;
                }
            }
        }

        [HarmonyPatch(typeof(Rewired.Player), "GetButtonDown", typeof(string))]
        public class RewiredPlayerGetButtonDownPatch
        {
            [HarmonyPostfix]
            public static void Postfix(string actionName, ref bool __result)
            {
                if (__result)
                    return;

                if (MenuStateDetector.IsInAnyMenu())
                    return;

                __result = Game.MouseControls.GetMouseButtonDown(actionName);
            }
        }

        [HarmonyPatch(typeof(Rewired.Player), "GetButton", typeof(string))]
        public class RewiredPlayerGetButtonPatch
        {
            [HarmonyPostfix]
            public static void Postfix(string actionName, ref bool __result)
            {
                if (__result)
                    return;

                if (MenuStateDetector.IsInAnyMenu())
                    return;

                __result = Game.MouseControls.GetMouseButton(actionName);
            }
        }
    }
}
