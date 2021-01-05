using UnityEngine;
using MouseSupport.Helpers;

namespace MouseSupport.Inventory
{
    public class ThingyWheel
    {
        public static bool IsSlotUnderMouse(Vector2 cursorPos, SpriteRenderer cursorSprite, out int slot)
        {
            var wheelSprites = ReflectionHelper.GetMemberValue<SpriteRenderer[]>(PT2.thing_wheel, "_wheel_sprites");

            for (int i = 0; i < 8; i++)
            {
                if (CursorSpriteHelper.IsCursorAtSprite(cursorPos, cursorSprite, wheelSprites[i]))
                {
                    slot = i;
                    return true;
                }
            }
            slot = -1;
            return false;
        }

        public static void EquipWheelSlot(int wheelIndex, int cursorIndex, int item_or_tool_id)
        {
            PT2.director.control.RIGHT_STICK_CLICK = false;

            ReflectionHelper.SetMemberValue(PT2.thing_wheel, "_right_stick_index", wheelIndex);
            ReflectionHelper.SetMemberValue(PT2.thing_wheel, "_ANIMATE_stick_index", wheelIndex);
            ReflectionHelper.InvokeMethod(PT2.thing_wheel, "_Animate_Diamond", false, true);
            ReflectionHelper.InvokeMethod(PT2.thing_wheel, "_Animate_Sprites", false, false, 0.2f, 1f);

            PT2.thing_wheel.HandleEquipOnWheelEvent(item_or_tool_id, cursorIndex, false, wheelIndex);
            PT2.thing_wheel.JuiceTweenDiamond(false);
        }
    }
}
