using System;

namespace RadialMenuControl
{
    public enum GaugeModeEnum
    {
        Auto,
        Cascade,
        Manual
    };
    // I mean the position of textvalue against the range arc
    public enum TextValuePosition
    { 
        Outer, // means outside of arc relative to circle
        Inner
    };

    public enum SubMenuBtnCtrlState
    { 
        SubmenuOpened, // Submenu is clicked and collapsed
        SubMenuClosedWithItems,
        SubmenuClosedWithNoItems
    };
    
}
