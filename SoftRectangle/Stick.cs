using Nefarius.ViGEm.Client.Targets.Xbox360;
using System;

namespace SoftRectangle;

/// <summary>
/// Constant structs for Left and Right stick bit masks and axis identifiers
/// </summary>
/// <see cref="LeftStick"/>
/// <see cref="RightStick"/>
public readonly struct Stick
{
    public static readonly Stick LeftStick = new(
        "LeftStick",
        Xbox360Axis.LeftThumbX,
        Xbox360Axis.LeftThumbY,
        (UInt32)Action.LeftStickUp,
        (UInt32)Action.LeftStickDown,
        (UInt32)Action.LeftStickLeft,
        (UInt32)Action.LeftStickRight,
        (UInt32)Action.LeftStickLeft | (UInt32)Action.LeftStickRight,
        (UInt32)Action.LeftStickUp | (UInt32)Action.LeftStickDown,
        (UInt32)Action.LeftStickUp | (UInt32)Action.LeftStickLeft,
        (UInt32)Action.LeftStickUp | (UInt32)Action.LeftStickRight,
        (UInt32)Action.LeftStickDown | (UInt32)Action.LeftStickLeft,
        (UInt32)Action.LeftStickDown | (UInt32)Action.LeftStickRight,
        (UInt32)Action.LeftStickUp |
            (UInt32)Action.LeftStickDown |
            (UInt32)Action.LeftStickLeft |
            (UInt32)Action.LeftStickRight
    );

    public static readonly Stick RightStick = new(
        "RightStick",
        Xbox360Axis.RightThumbX,
        Xbox360Axis.RightThumbY,
        (UInt32)Action.RightStickUp,
        (UInt32)Action.RightStickDown,
        (UInt32)Action.RightStickLeft,
        (UInt32)Action.RightStickRight,
        (UInt32)Action.RightStickLeft | (UInt32)Action.RightStickRight,
        (UInt32)Action.RightStickUp | (UInt32)Action.RightStickDown,
        (UInt32)Action.RightStickUp | (UInt32)Action.RightStickLeft,
        (UInt32)Action.RightStickUp | (UInt32)Action.RightStickRight,
        (UInt32)Action.RightStickDown | (UInt32)Action.RightStickLeft,
        (UInt32)Action.RightStickDown | (UInt32)Action.RightStickRight,
        (UInt32)Action.RightStickUp |
            (UInt32)Action.RightStickDown |
            (UInt32)Action.RightStickLeft |
            (UInt32)Action.RightStickRight
    );


    public readonly string Name;
    public readonly Xbox360Axis AxisX;
    public readonly Xbox360Axis AxisY;
    public readonly UInt32 Up;
    public readonly UInt32 Down;
    public readonly UInt32 Left;
    public readonly UInt32 Right;
    public readonly UInt32 LeftOrRight;
    public readonly UInt32 UpOrDown;
    public readonly UInt32 TopLeft;
    public readonly UInt32 TopRight;
    public readonly UInt32 BottomLeft;
    public readonly UInt32 BottomRight;
    public readonly UInt32 Any;

    private Stick(
        string Name,
        Xbox360Axis AxisX,
        Xbox360Axis AxisY,
        UInt32 Up,
        UInt32 Down,
        UInt32 Left,
        UInt32 Right,
        UInt32 LeftOrRight,
        UInt32 UpOrDown,
        UInt32 TopLeft,
        UInt32 TopRight,
        UInt32 BottomLeft,
        UInt32 BottomRight,
        UInt32 Any
    )
    {

        this.Name = Name;
        this.AxisX = AxisX;
        this.AxisY = AxisY;
        this.Up = Up;
        this.Down = Down;
        this.Left = Left;
        this.Right = Right;
        this.LeftOrRight = LeftOrRight;
        this.UpOrDown = UpOrDown;
        this.TopLeft = TopLeft;
        this.TopRight = TopRight;
        this.BottomLeft = BottomLeft;
        this.BottomRight = BottomRight;
        this.Any = Any;
    }
}

