using UnityEngine;
using System;
using System.Collections;

namespace EventArgsChildren{

    public class MouseButtonsEventArgs : EventArgs
    {
        public KeyCode Button { get; set; }
        public MouseButtonsEventArgs(int button)
        {
            switch (button)
            {
                case 0: { Button = KeyCode.Mouse0; break; }
                case 1: { Button = KeyCode.Mouse1; break; }
                case 2: { Button = KeyCode.Mouse2; break; }
                default: { Button = KeyCode.Mouse0; break; }
            }
        }
    }
}
