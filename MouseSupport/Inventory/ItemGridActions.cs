using UnityEngine;
using MouseSupport.Helpers;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MouseSupport.Inventory
{
    public class ItemGridActions
    {
        private static Dictionary<MouseButton, IComparer<Item>> SortTypes = new Dictionary<MouseButton, IComparer<Item>>()
        {
            {MouseButton.PRIMARY, new Item.HealComparer() },
            {MouseButton.SECONDARY, new Item.PriceComparer() },
        };

        public static void Discard(ItemGridLogic item_grid, WhichPane whichPane, int cursorIndex)
        {
            if (whichPane != WhichPane.LEFT)
                return;

            ReflectionHelper.SetMemberValue(item_grid, "_cursor_in_left_pane", true);
            ReflectionHelper.SetMemberValue(item_grid, "_cursor_index", cursorIndex);
            ReflectionHelper.InvokeMethod(item_grid, "_SetCursorPosition", true);
            item_grid.FromSubCommands_Discard();
        }

        public static bool Sort(ItemGridLogic item_grid, MouseButton mouseButton)
        {
            if (!SortTypes.TryGetValue(mouseButton, out var itemComparer))
                return false;

            SpriteRenderer[] sprites = ReflectionHelper.GetMemberValue<SpriteRenderer[]>(item_grid, "_left_pane_sprites");
            int[] itemIDs = ReflectionHelper.GetMemberValue<int[]>(PT2.save_file, "_item_IDs");
            int[] itemCounts = ReflectionHelper.GetMemberValue<int[]>(PT2.save_file, "_item_ID_count");
            int itemLimit = ReflectionHelper.GetMemberValue<int>(item_grid, "_L_EFFECTIVE_ITEM_LIMIT");

            Dictionary<int, Item> items = new Dictionary<int, Item>();

            for (int i = 0; i < itemLimit; i++)
            {
                if (items.ContainsKey(itemIDs[i]))
                {
                    items[itemIDs[i]].count += itemCounts[i];
                }
                else
                {
                    items.Add(itemIDs[i], new Item(id: itemIDs[i], count: itemCounts[i]));
                }
                PT2.juicer.J_TimeFree_ScaleSineWobble(sprites[i].transform, Vector3.one);
            }

            var sortedItems = items.Values.OrderByDescending(i => i, itemComparer).ToList();
            for (int i = 0; i < sortedItems.Count; i++)
            {
                if (sortedItems[i].count > sortedItems[i].maxcount)
                {
                    var toomuch = sortedItems[i].count - sortedItems[i].maxcount;
                    sortedItems[i].count = sortedItems[i].maxcount;
                    sortedItems.Insert(i + 1, new Item(sortedItems[i].id, toomuch));
                }
            }

            while (sortedItems.Count < itemLimit)
            {
                sortedItems.Add(new Item(0, 0));
            }

            // this should never happen, but just to be extra sure...
            if (sortedItems.Count > itemLimit)
            {
                PT2.sound_g.PlayGlobalCommonSfx(123, 1f, 1f, 2);
                MainEntry.Mod.Logger.Warning("Something went wrong while sorting, got " + sortedItems.Count + " sorted items, but limit is " + itemLimit + ". Canceled sort.");
                return false;
            }

            PT2.sound_g.PlayGlobalCommonSfx(125, 1f, 1f, 2);

            for (int i = 0; i < sortedItems.Count; i++)
            {
                sprites[i].sprite = sortedItems[i].sprite;
                ReflectionHelper.SetMemberArrayValue(PT2.save_file, "_item_IDs", i, sortedItems[i].id);
                ReflectionHelper.SetMemberArrayValue(PT2.save_file, "_item_ID_count", i, sortedItems[i].count);
            }

            PT2.save_file.gales_using_item_DI = sortedItems.FindIndex(item => item.id == PT2.save_file.gales_using_item_ID);
            PT2.save_file.tool_hud_DI = sortedItems.FindIndex(item => item.id == PT2.save_file.tool_hud_ID);

            for (int i = 0; i < 8; i++)
            {
                var index = sortedItems.FindIndex(item => item.id == ReflectionHelper.GetMemberArrayValue<int>(PT2.save_file, "_wheel_IDs", i));
                ReflectionHelper.SetMemberArrayValue(PT2.save_file, "_wheel_DIs", i, index);
            }

            PT2.save_file.SLOTS_USED = sortedItems.Where(item => item.id != 0).Count();

            item_grid.UpdateSlotsText();
            PT2.thing_wheel.UpdateToolHudGraphics(swapping_equip: false, item_depleted: false);
            PT2.thing_wheel.UpdateWheelGraphics();
            ReflectionHelper.InvokeMethod(item_grid, "_Grid_TurnOnNumbers");

            return true;
        }
    }
}
