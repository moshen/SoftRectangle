using System;

namespace SoftRectangle;

/// <summary>
/// Bit masks representing the available actions on an XBox 360 controller.
/// Used to dictate and dechipher the state of our virtual controller.
///
/// The Bottom 16 bits are used directly within
/// <see cref="IXbox360Controller.SetButtonsFull"/>
/// </summary>
public enum Action : UInt32
{
    // This section flags are meant to match the 16 bits of flags from
    // IXbox360Controller.SetButtonsFull
    DPadUp            = 0x00000001,
    DPadDown          = 0x00000002,
    DPadLeft          = 0x00000004,
    DPadRight         = 0x00000008,
    Start             = 0x00000010,
    Back              = 0x00000020,
    LeftThumb         = 0x00000040,
    RightThumb        = 0x00000080,
    LeftShoulder      = 0x00000100,
    RightShoulder     = 0x00000200,
    Guide             = 0x00000400,
    A                 = 0x00001000,
    B                 = 0x00002000,
    X                 = 0x00004000,
    Y                 = 0x00008000,
    // End matching section

    LeftStickUp       = 0x00010000,
    LeftStickDown     = 0x00020000,
    LeftStickLeft     = 0x00040000,
    LeftStickRight    = 0x00080000,

    RightStickUp      = 0x00100000,
    RightStickDown    = 0x00200000,
    RightStickLeft    = 0x00400000,
    RightStickRight   = 0x00800000,

    LeftTriggerLight  = 0x01000000,
    LeftTriggerMid    = 0x02000000,
    LeftTriggerHard   = 0x04000000,
    RightTriggerLight = 0x08000000,
    RightTriggerMid   = 0x10000000,
    RightTriggerHard  = 0x20000000,

    ModX              = 0x40000000,
    ModY              = 0x80000000,
}

