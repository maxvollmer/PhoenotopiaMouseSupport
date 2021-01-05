using UnityEngine;
using MouseSupport.Helpers;

namespace MouseSupport.Inventory
{
    public class InventoryActions
    {
        private static bool forceCursorUpdate = false;

        public static void ForceCursorUpdate()
        {
            forceCursorUpdate = true;
        }

        public static void Update(MenuLogic menuLogic)
        {
            var cursorSprite = ReflectionHelper.GetMemberValue<SpriteRenderer>(menuLogic.item_grid, "_cursor_sprite");
            if (cursorSprite == null)
                return;

            var cursorPos = Helpers.MouseCursor.GetCursorPos(out bool isNewCursorPos);

            if (forceCursorUpdate)
            {
                isNewCursorPos = true;
                forceCursorUpdate = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (PT2.sub_commands.IsActive())
                {
                    SubCommandActions.CloseSubCommands();
                }
                else
                {
                    if (ItemDragger.IsDragging())
                    {
                        ItemDragger.StopDragging(false);
                    }
                    else
                    {
                        if (ThingyWheel.IsSlotUnderMouse(cursorPos, cursorSprite, out int wheelIndex))
                        {
                            ReflectionHelper.SetMemberValue(menuLogic.item_grid, "_intent_to_wheel_equip", false);
                            ThingyWheel.EquipWheelSlot(wheelIndex, ItemDragger.CursorIndex(), 0);
                            menuLogic.item_grid._FindWheelIndexToHighlight();
                        }
                    }
                    if (ReflectionHelper.GetMemberValue<bool>(menuLogic.item_grid, "_in_swap_mode"))
                    {
                        ReflectionHelper.InvokeMethod(menuLogic.item_grid, "_CancelSwap");
                    }
                }
            }

            if (PT2.sub_commands.IsActive())
            {
                bool isInSubMenu = SubCommandActions.CheckCursorInSubmenu(PT2.sub_commands, cursorPos, isNewCursorPos);
                if (!isInSubMenu)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        SubCommandActions.CloseSubCommands();
                    }
                }
            }
            else
            {
                ItemGridActions.CheckCursorInGrid(menuLogic.item_grid, cursorPos, cursorSprite, isNewCursorPos);
            }

            if (ItemDragger.IsDragging())
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (ThingyWheel.IsSlotUnderMouse(cursorPos, cursorSprite, out int wheelIndex))
                    {
                        ReflectionHelper.SetMemberValue(menuLogic.item_grid, "_intent_to_wheel_equip", false);
                        int item_or_tool_id = PT2.save_file.FetchData(menuLogic.item_grid.current_menu_type, ItemDragger.IsPane(WhichPane.LEFT), null)[ItemDragger.CursorIndex()];
                        ThingyWheel.EquipWheelSlot(wheelIndex, ItemDragger.CursorIndex(), item_or_tool_id);
                        menuLogic.item_grid._FindWheelIndexToHighlight();
                    }
                    else if (InventoryExtraIcons.IsMouseOver(InventoryExtraIcons.IconID.TRASHCAN, cursorPos, cursorSprite))
                    {
                        ItemGridActions.Discard(menuLogic.item_grid, ItemDragger.GetPane(), ItemDragger.CursorIndex());
                    }

                    ItemDragger.StopDragging(true);
                }
                else
                {
                    ItemDragger.Update(cursorPos);
                }
            }
            else
            {
                HandleMouseWheel(menuLogic, cursorPos, cursorSprite);

                if (Input.GetMouseButtonDown(0)
                    && InventoryExtraIcons.IsMouseOver(InventoryExtraIcons.IconID.SORT, cursorPos, cursorSprite))
                {
                    ItemGridActions.Sort(menuLogic.item_grid);
                }
            }
        }

        private static void HandleMouseWheel(MenuLogic menuLogic, Vector3 cursorPos, SpriteRenderer cursorSprite)
        {
            if (Input.mouseScrollDelta.y == 0)
                return;

            if (PT2.sub_commands.IsActive())
            {
                PT2.sub_commands.MoveCursor(Input.mouseScrollDelta.y > 0 ? DIRECTION.UP : DIRECTION.DOWN);
            }
            else
            {
                ItemGridScroller.ScrollPane(menuLogic.item_grid, cursorPos, cursorSprite, -Input.mouseScrollDelta.y);
            }
        }
    }
}
