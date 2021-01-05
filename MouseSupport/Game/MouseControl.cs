using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MouseSupport.Game
{
    public struct MouseControl
    {
        public readonly int button;
        public readonly MouseInputType type;
        public MouseControl(int button, MouseInputType type)
        {
            this.button = button;
            this.type = type;
        }
    }
}
