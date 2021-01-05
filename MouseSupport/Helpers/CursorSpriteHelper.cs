using UnityEngine;

namespace MouseSupport.Helpers
{
    public class CursorSpriteHelper
    {

        public static int FindSpriteAtCursor(Vector2 cursorPos, SpriteRenderer cursorSprite, SpriteRenderer[] sprites, int itemLimit, int[] spriteIndicesToSkip)
        {
            for (int cursorindex = 0; cursorindex < sprites.Length && cursorindex < itemLimit; cursorindex++)
            {
                if (sprites[cursorindex] == null)
                    continue;

                if (spriteIndicesToSkip != null && cursorindex < spriteIndicesToSkip.Length && spriteIndicesToSkip[cursorindex] == 1)
                    continue;

                if (IsCursorAtSprite(cursorPos, cursorSprite, sprites[cursorindex]))
                    return cursorindex;
            }

            return -1;
        }

        public static bool IsCursorAtSprite(Vector2 cursorPos, SpriteRenderer cursorSprite, SpriteRenderer sprite)
        {
            Rect rect = new Rect()
            {
                width = cursorSprite.size.x * 2,
                height = cursorSprite.size.y * 2
            };
            rect.center = sprite.transform.position;

            return rect.Contains(cursorPos);
        }
    }
}
