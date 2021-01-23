using HarmonyLib;
using MouseSupport.Game;
using MouseSupport.Helpers;

namespace MouseSupport.Patches
{
    public class GaleLogicOnePatch
    {
        [HarmonyPatch(typeof(GaleLogicOne), "_STATE_SlingshotAiming")]
        public class GaleLogicOneSlingshotAimingPatch
        {
            private static float backup_sling_reticule_speed = 1.8f;

            [HarmonyPrefix]
            public static bool Prefix(GaleLogicOne __instance)
            {
                if (MainEntry.Settings.EnableSlingshotMouseAim)
                {
                    backup_sling_reticule_speed = ReflectionHelper.GetMemberValue<float>(__instance, "_sling_reticule_speed");
                    ReflectionHelper.SetMemberValue(__instance, "_sling_reticule_speed", 0f);
                    MouseAiming.UpdateSlingshotAim(__instance);
                }
                return true;
            }

            [HarmonyPostfix]
            public static void Postfix(GaleLogicOne __instance)
            {
                if (MainEntry.Settings.EnableSlingshotMouseAim)
                {
                    ReflectionHelper.SetMemberValue(__instance, "_sling_reticule_speed", backup_sling_reticule_speed);
                }
            }
        }

        // Quells has added mouse based crossbow aiming to the game, so we don't need to add it anymore
        /*
        [HarmonyPatch(typeof(GaleLogicOne), "_STATE_CrossbowAiming")]
        public class GaleLogicOneCrossbowAimingPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(GaleLogicOne __instance)
            {
                if (PT2.save_file.gales_using_item_ID != PT2.save_file.tool_hud_ID || !PT2.director.control.TOOL_HELD)
                {
                    return true;
                }

                MouseAiming.UpdateCrossbowAim(__instance);
                return false;
            }
        }
        */
    }
}
