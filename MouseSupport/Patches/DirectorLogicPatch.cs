using HarmonyLib;
using Rewired;
using System.Text;
using MouseSupport.Helpers;

namespace MouseSupport.Patches
{
    public class DirectorLogicPatch
    {
        [HarmonyPatch(typeof(DirectorLogic), "_Control_View", typeof(StringBuilder))]
        public class DirectorLogicControlViewPatch
        {
            [HarmonyPostfix]
            public static void Postfix(StringBuilder sb, DirectorLogic __instance)
            {
                // TODO: Add mouse bindings to view menu (this only shows bindings!)
                MainEntry.Mod.Logger.Log("__OPT_rebind_msg5: " + DB.TRANSLATE_map["__OPT_rebind_msg5"]);
                MainEntry.Mod.Logger.Log("__OPT_gearring_joy: " + DB.TRANSLATE_map["__OPT_gearring_joy"]);
                MainEntry.Mod.Logger.Log("__OPT_gearring_key: " + DB.TRANSLATE_map["__OPT_gearring_key"]);
                MainEntry.Mod.Logger.Log("__OPT_CONTROL_text2: " + DB.TRANSLATE_map["__OPT_CONTROL_text2"]);
                MainEntry.Mod.Logger.Log("ReadKeyBindingByType: " + __instance.control.ReadKeyBindingByType(ControllerType.Keyboard, "Attack"));
                MainEntry.Mod.Logger.Log("sb: " + sb.ToString());
            }
        }

        [HarmonyPatch(typeof(DirectorLogic), "_ControlRebind_OpenControlMapper")]
        public class DirectorLogicControlRebindOpenControlMapperPatch
        {
            [HarmonyPostfix]
            public static void Postfix(DirectorLogic __instance)
            {
                if (ReInput.players.Players.Count == 0)
                    return;

                ReInput.players.Players[0].controllers.hasMouse = true;
                __instance.control.control_mapper.showMouse = true;
            }
        }

        [HarmonyPatch(typeof(DirectorLogic), "_ControlRebind_ClosedControlMapper")]
        public class DirectorLogicControlRebindCloseControlMapperPatch
        {
            [HarmonyPostfix]
            public static void Postfix(DirectorLogic __instance)
            {
                Game.MouseControls.ParseMouseControls();

                __instance.control.control_mapper.showMouse = false;

                if (ReInput.players.Players.Count > 0)
                    ReInput.players.Players[0].controllers.hasMouse = false;
            }
        }
    }
}
