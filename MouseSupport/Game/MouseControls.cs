using Rewired;
using UnityEngine;
using System.Collections.Generic;

namespace MouseSupport.Game
{
    public class MouseControls
    {
        private static Dictionary<string, MouseControl> Bindings { get; } = new Dictionary<string, MouseControl>();

        public static void ParseMouseControls()
        {
            if (ReInput.players.Players.Count == 0)
                return;

            Bindings.Clear();

            foreach (var map in ReInput.players.Players[0].controllers.maps.GetAllMaps(ControllerType.Mouse))
            {
                foreach (var elementMap in map.GetElementMaps())
                {
                    var actionName = elementMap.actionDescriptiveName;
                    int mouseButton = elementMap.elementIndex;

                    Bindings.Add(actionName, new MouseControl(mouseButton, GetMouseInputType(elementMap.elementType, elementMap.axisRange)));
                }
            }
        }

        public static bool GetMouseButtonDown(string actionName)
        {
            if (Bindings.TryGetValue(actionName, out MouseControl mouseControl))
            {
                if (mouseControl.type == MouseInputType.BUTTON)
                {
                    return Input.GetMouseButtonDown(mouseControl.button);
                }
                else if (mouseControl.type == MouseInputType.SCROLL_DOWN)
                {
                    return Input.mouseScrollDelta.y < 0;
                }
                else if (mouseControl.type == MouseInputType.SCROLL_UP)
                {
                    return Input.mouseScrollDelta.y > 0;
                }
            }
            return false;
        }

        public static bool GetMouseButton(string actionName)
        {
            if (Bindings.TryGetValue(actionName, out MouseControl mouseControl))
            {
                if (mouseControl.type == MouseInputType.BUTTON)
                {
                    return Input.GetMouseButton(mouseControl.button);
                }
                else if (mouseControl.type == MouseInputType.SCROLL_DOWN)
                {
                    return Input.mouseScrollDelta.y < 0;
                }
                else if (mouseControl.type == MouseInputType.SCROLL_UP)
                {
                    return Input.mouseScrollDelta.y > 0;
                }
            }
            return false;
        }

        private static MouseInputType GetMouseInputType(ControllerElementType elementType, AxisRange axisRange)
        {
            if (elementType == ControllerElementType.Axis)
            {
                switch (axisRange)
                {
                    case AxisRange.Negative:
                        return MouseInputType.SCROLL_DOWN;
                    case AxisRange.Positive:
                        return MouseInputType.SCROLL_UP;

                }
            }
            return MouseInputType.BUTTON;
        }
    }
}
