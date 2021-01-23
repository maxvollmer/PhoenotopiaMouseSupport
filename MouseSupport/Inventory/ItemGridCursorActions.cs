using UnityEngine;
using MouseSupport.Helpers;

namespace MouseSupport.Inventory
{
    public class ItemGridCursorActions
    {

        public static void CheckCursorInGrid(ItemGridLogic item_grid, Vector3 cursorPos, SpriteRenderer cursorSprite, bool isNewCursorPos)
        {
            int[] spriteIndicesToSkip = null;
            if (item_grid.current_menu_type == MenuLogic.MENU_TYPE.P1_STATUS)
            {
                spriteIndicesToSkip = PT2.save_file.FetchData(MenuLogic.MENU_TYPE.P1_STATUS, true, null);
            }

            CheckCursorInGridPane(item_grid, cursorPos, isNewCursorPos, cursorSprite, spriteIndicesToSkip, WhichPane.LEFT);

            if (!ReflectionHelper.GetMemberValue<bool>(item_grid, "_both_panes_in_action"))
                return;

            CheckCursorInGridPane(item_grid, cursorPos, isNewCursorPos, cursorSprite, spriteIndicesToSkip, WhichPane.RIGHT);
        }

        private static void CheckCursorInGridPane(ItemGridLogic item_grid, Vector3 cursorPos, bool isNewCursorPos, SpriteRenderer cursorSprite, int[] spriteIndicesToSkip, WhichPane whichPane)
        {
            SpriteRenderer[] paneSprites = ReflectionHelper.GetMemberValue<SpriteRenderer[]>(item_grid, whichPane == WhichPane.LEFT ? "_left_pane_sprites" : "_right_pane_sprites");
            if (paneSprites == null)
                return;

            int itemLimit = ReflectionHelper.GetMemberValue<int>(item_grid, whichPane == WhichPane.LEFT ? "_L_EFFECTIVE_ITEM_LIMIT" : "_R_EFFECTIVE_ITEM_LIMIT");
            int cursorIndex = CursorSpriteHelper.FindSpriteAtCursor(cursorPos, cursorSprite, paneSprites, itemLimit, spriteIndicesToSkip);
            if (cursorIndex >= 0)
            {
                if (isNewCursorPos)
                {
                    UpdateCursor(item_grid, cursorIndex, whichPane);
                }
                DoMouseInteractions(item_grid, cursorIndex, whichPane, paneSprites[cursorIndex]);
                return;
            }
        }

        private static void UpdateCursor(ItemGridLogic item_grid, int cursorindex, WhichPane whichPane)
        {
            if (ReflectionHelper.CompareMemberValue(item_grid, "_cursor_in_left_pane", whichPane == WhichPane.LEFT)
                && ReflectionHelper.CompareMemberValue(item_grid, "_cursor_index", cursorindex))
            {
                // Already at this cursor index, skipping
                return;
            }

            if (!ReflectionHelper.CompareMemberValue(item_grid, "_cursor_in_left_pane", whichPane == WhichPane.LEFT)
                && ReflectionHelper.GetMemberValue<bool>(item_grid, "_in_swap_mode"))
            {
                ReflectionHelper.InvokeMethod(item_grid, "_CancelSwap");
            }

            PT2.sound_g.PlayGlobalCommonSfx(124, 1f, src_index: 2);

            ReflectionHelper.SetMemberValue(item_grid, "_cursor_in_left_pane", whichPane == WhichPane.LEFT);
            ReflectionHelper.SetMemberValue(item_grid, "_cursor_index", cursorindex);

            ReflectionHelper.InvokeMethod(item_grid, "_SetCursorPosition", true);
        }

        private static void DoMouseInteractions(ItemGridLogic item_grid, int cursorindex, WhichPane whichPane, SpriteRenderer selectedItemSprite)
        {
            if (Input.GetMouseButtonDown(1))
            {
                item_grid.HandleConfirm();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (ReflectionHelper.GetMemberValue<bool>(item_grid, "_in_swap_mode"))
                {
                    ReflectionHelper.InvokeMethod(item_grid, "_CancelSwap");
                }
                ItemDragger.StartDragging(selectedItemSprite, cursorindex, whichPane);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (ItemDragger.IsDragging())
                {
                    if (!ItemDragger.IsPane(whichPane))
                    {
                        ItemDragger.StopDragging(true);
                    }
                    else
                    {
                        ItemDragger.FinishDragging(item_grid, cursorindex, whichPane);
                    }
                }
            }
        }
    }
}
