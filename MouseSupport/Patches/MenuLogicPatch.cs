using HarmonyLib;
using UnityEngine;
using MouseSupport.Helpers;
using MouseSupport.Inventory;

namespace MouseSupport.Patches
{
    public class MenuLogicPatch
    {
        [HarmonyPatch(typeof(MenuLogic), "Update")]
        public class MenuLogicUpdatePatch
        {
            [HarmonyPostfix]
            public static void Postfix(MenuLogic __instance)
            {
                if (!__instance.is_active || !WindowState.IsActive)
                {
                    InventoryActions.ForceCursorUpdate();
                    ItemDragger.EnableDragging(false);
                    return;
                }

                ItemDragger.EnableDragging(__instance.item_grid.current_menu_type == MenuLogic.MENU_TYPE.P1_TOOLS_ITEMS);

                InventoryExtraIcons.Update(__instance.item_grid);

                InventoryActions.Update(__instance);
            }
        }
    }
}
