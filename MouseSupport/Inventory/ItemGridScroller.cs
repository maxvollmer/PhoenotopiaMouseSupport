using UnityEngine;
using MouseSupport.Helpers;

namespace MouseSupport.Inventory
{
    public class ItemGridScroller
    {
        public static void ScrollPane(ItemGridLogic item_grid, Vector3 cursorPos, SpriteRenderer cursorSprite, float dir)
        {
            var leftPaneRect = GetPaneRect(item_grid, cursorSprite, WhichPane.LEFT);
            var rightPaneRect = GetPaneRect(item_grid, cursorSprite, WhichPane.RIGHT);

            if (cursorPos.x > leftPaneRect.xMin && cursorPos.x < leftPaneRect.xMax)
            {
                ActuallyScrollPane(item_grid, cursorSprite, dir, leftPaneRect, WhichPane.LEFT);
            }
            else if (cursorPos.x > rightPaneRect.xMin && cursorPos.x < rightPaneRect.xMax)
            {
                ActuallyScrollPane(item_grid, cursorSprite, dir, rightPaneRect, WhichPane.RIGHT);
            }
        }

        private static void ActuallyScrollPane(ItemGridLogic item_grid, SpriteRenderer cursorSprite, float dir, Rect paneRect, WhichPane whichPane)
        {
            var paneAnchor = ReflectionHelper.GetMemberValue<Vector3>(item_grid, whichPane == WhichPane.LEFT ? "_left_pane_anchor" : "_right_pane_anchor");
            var curPos = ReflectionHelper.GetMemberValue<Vector3>(item_grid, whichPane == WhichPane.LEFT ? "_ideal_left_pane_pos" : "_ideal_right_pane_pos");

            var newPos = curPos + new Vector3(0f, dir, 0f);

            if (newPos.y > paneAnchor.y + paneRect.height - 9)
                newPos.y = paneAnchor.y + paneRect.height - 9;

            if (newPos.y < paneAnchor.y)
                newPos.y = paneAnchor.y;

            ReflectionHelper.SetMemberValue(item_grid, whichPane == WhichPane.LEFT ? "_ideal_left_pane_pos" : "_ideal_right_pane_pos", newPos);
        }

        private static Rect GetPaneRect(ItemGridLogic item_grid, SpriteRenderer cursorSprite, WhichPane whichPane)
        {
            int itemLimit = ReflectionHelper.GetMemberValue<int>(item_grid, whichPane == WhichPane.LEFT ? "_L_EFFECTIVE_ITEM_LIMIT" : "_R_EFFECTIVE_ITEM_LIMIT");
            SpriteRenderer[] paneSprites = ReflectionHelper.GetMemberValue<SpriteRenderer[]>(item_grid, whichPane == WhichPane.LEFT ? "_left_pane_sprites" : "_right_pane_sprites");

            if (paneSprites == null || paneSprites.Length == 0)
                return Rect.zero;

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            for (int index = 0; index < itemLimit && index < paneSprites.Length; index++)
            {
                if (paneSprites[index] == null)
                    continue;

                Rect rect = new Rect()
                {
                    width = cursorSprite.size.x * 2,
                    height = cursorSprite.size.y * 2
                };

                rect.center = paneSprites[index].transform.position;

                minX = System.Math.Min(minX, rect.xMin);
                minY = System.Math.Min(minY, rect.yMin);
                maxX = System.Math.Max(maxX, rect.xMax);
                maxY = System.Math.Max(maxY, rect.yMax);

                MainEntry.Mod.Logger.Log("sprite.transform.position: " + paneSprites[index].transform.position
                    + ", rect: " + rect
                    + ", minX: " + minX
                    + ", minY: " + minY
                    + ", maxX: " + maxX
                    + ", maxY: " + maxY);
            }

            return Rect.MinMaxRect(minX, minY, maxX, maxY);
        }
    }
}
