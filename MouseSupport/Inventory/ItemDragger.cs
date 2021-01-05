using UnityEngine;
using MouseSupport.Helpers;

namespace MouseSupport.Inventory
{
    public class ItemDragger
    {
        private static bool isDraggingEnabled = false;

        private static int draggedCursorIndex = -1;
        private static GameObject draggedItemSprite = null;
        private static GameObject selectedItemSprite = null;
        private static WhichPane whichPane = WhichPane.INVALID;

        public static void EnableDragging(bool enabled)
        {
            if (!enabled && IsDragging())
            {
                StopDragging(false);
            }
            isDraggingEnabled = enabled;
        }

        public static void StartDragging(SpriteRenderer selectedItemSprite, int cursorindex, WhichPane whichPane)
        {
            if (!isDraggingEnabled)
                return;

            StopDragging(false);
            draggedCursorIndex = cursorindex;
            draggedItemSprite = Object.Instantiate(selectedItemSprite.gameObject);
            draggedItemSprite.transform.position = selectedItemSprite.transform.position;
            draggedItemSprite.transform.localScale = selectedItemSprite.transform.lossyScale;
            foreach (var spriteRenderer in draggedItemSprite.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.sortingOrder = 2000;
            }
            ItemDragger.selectedItemSprite = selectedItemSprite.gameObject;
            ItemDragger.selectedItemSprite.SetActive(false);
            ItemDragger.whichPane = whichPane;
        }

        public static void StopDragging(bool wobble)
        {
            if (wobble && isDraggingEnabled && selectedItemSprite != null)
            {
                PT2.juicer.J_TimeFree_ScaleSineWobble(selectedItemSprite.transform, Vector3.one);
            }

            draggedCursorIndex = -1;
            Object.Destroy(draggedItemSprite);
            draggedItemSprite = null;
            selectedItemSprite?.SetActive(true);
            selectedItemSprite = null;
            whichPane = WhichPane.INVALID;
        }

        public static void FinishDragging(ItemGridLogic item_grid, int cursorindex, WhichPane whichPane)
        {
            if (isDraggingEnabled && draggedCursorIndex != cursorindex && ItemDragger.whichPane == whichPane)
            {
                if (!ReflectionHelper.GetMemberValue<bool>(item_grid, "_in_swap_mode")
                        || !ReflectionHelper.CompareMemberValue(item_grid, "_swap_index", draggedCursorIndex))
                {
                    ReflectionHelper.SetMemberValue(item_grid, "_in_swap_mode", true);
                    ReflectionHelper.SetMemberValue(item_grid, "_swap_index", draggedCursorIndex);
                }
                item_grid.HandleSwap();
            }

            StopDragging(false);
        }

        public static bool IsDragging()
        {
            return isDraggingEnabled && draggedCursorIndex >= 0;
        }

        public static WhichPane GetPane()
        {
            return isDraggingEnabled ? whichPane : WhichPane.INVALID;
        }

        public static int CursorIndex()
        {
            return isDraggingEnabled ? draggedCursorIndex : -1;
        }

        public static bool IsPane(WhichPane whichPane)
        {
            return isDraggingEnabled && ItemDragger.whichPane == whichPane;
        }

        public static void Update(Vector2 cursorPos)
        {
            if (!isDraggingEnabled)
                return;

            draggedItemSprite.transform.position = new Vector3(cursorPos.x, cursorPos.y, draggedItemSprite.transform.position.z);
        }
    }
}
