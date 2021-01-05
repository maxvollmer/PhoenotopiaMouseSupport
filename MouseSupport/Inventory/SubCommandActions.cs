using UnityEngine;
using TMPro;
using MouseSupport.Helpers;

namespace MouseSupport.Inventory
{
    public class SubCommandActions
    {
        public static void CloseSubCommands()
        {
            PT2.sub_commands.HandleCancelPress();
            InventoryActions.ForceCursorUpdate();
        }

        public static bool CheckCursorInSubmenu(SubCommandsLogic sub_commands, Vector3 cursorPos, bool isNewCursorPos)
        {
            var numCommands = ReflectionHelper.GetMemberValue<int>(sub_commands, "_command_list_size");
            if (numCommands == 0)
                return false;

            var text = sub_commands.GetComponentInChildren<TextMeshPro>();

            var textTransform = text.GetComponent<RectTransform>();

            float lineHeight = text.preferredHeight / numCommands;

            for (int commandIndex = 0; commandIndex < numCommands; commandIndex++)
            {
                Rect rect = new Rect()
                {
                    width = text.preferredWidth,
                    height = lineHeight
                };

                var textPosX = textTransform.position.x + text.preferredWidth * 0.5f;
                var textPosY = textTransform.position.y - lineHeight * (commandIndex + 0.5f);

                rect.center = new Vector2(textPosX, textPosY);

                if (rect.Contains(cursorPos))
                {
                    if (isNewCursorPos)
                    {
                        UpdateCommandIndex(sub_commands, commandIndex);
                    }
                    DoMouseInteractions(sub_commands, commandIndex);
                    return true;
                }
            }

            return false;
        }

        private static void DoMouseInteractions(SubCommandsLogic sub_commands, int commandIndex)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ReflectionHelper.InvokeMethod(sub_commands, "TIDAL_ExecuteSubCommand");
            }
        }

        private static void UpdateCommandIndex(SubCommandsLogic sub_commands, int commandIndex)
        {
            if (!ReflectionHelper.CompareMemberValue(sub_commands, "_command_selected_index", commandIndex))
            {
                PT2.sound_g.PlayGlobalCommonSfx(124, 1f, 1f, 2);
                ReflectionHelper.SetMemberValue(sub_commands, "_command_selected_index", commandIndex);

                var text = sub_commands.GetComponentInChildren<TextMeshPro>();
                text.text = ReflectionHelper.GetMethodValue<string>(sub_commands, "_GetSubCmdDisplayString");
            }
        }
    }
}
