using SoftRectangle.Config;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using System;
using System.Diagnostics;
using System.Numerics;

namespace SoftRectangle;
public class KeyState
{
    // Action state encoded into 32 bits, bottom 16 bits are the buttons
    private UInt32 actionState = 0;

    // Whether or not we need to update the controller report
    private Boolean isDirty = false;
    private IXbox360Controller controller;
    private KeyConfig config;

    public KeyState(KeyConfig config, IXbox360Controller controller)
    {
        this.config = config;
        this.controller = controller;
    }

    public void SetActionState(Action action)
    {
        UInt32 oldState = actionState;

        switch (action)
        {
            // Stick action will unset the opposite side bit
            case Action.LeftStickUp:
                actionState &= ~(UInt32)Action.LeftStickDown;
                break;

            case Action.LeftStickDown:
                actionState &= ~(UInt32)Action.LeftStickUp;
                break;

            case Action.LeftStickLeft:
                actionState &= ~(UInt32)Action.LeftStickRight;
                break;

            case Action.LeftStickRight:
                actionState &= ~(UInt32)Action.LeftStickLeft;
                break;

            case Action.RightStickUp:
                actionState &= ~(UInt32)Action.RightStickDown;
                break;

            case Action.RightStickDown:
                actionState &= ~(UInt32)Action.RightStickUp;
                break;

            case Action.RightStickLeft:
                actionState &= ~(UInt32)Action.RightStickRight;
                break;

            case Action.RightStickRight:
                actionState &= ~(UInt32)Action.RightStickLeft;
                break;
        }

        actionState |= (UInt32)action;

        if (oldState != actionState)
        {
            isDirty = true;
        }
    }

    public void UnsetActionState(Action action)
    {
        UInt32 oldState = actionState;

        actionState &= ~(UInt32)action;

        if (oldState != actionState)
        {
            isDirty = true;
        }
    }

    
    // @TODO: Test right stick and remove
    public Boolean isTesting = true;
    public short testX = 0;
    public short testY = 0;
    // end

    // @TODO: Make this configurable?
    // This is a magic number that Dolphin appears to use as its stick cardinal
    // limit to have accurate coordinates input, we need to map to specific
    // values
    private static readonly short stickLimit = 20641;

    private void SetStickCoords(Xbox360Axis xAxis, Xbox360Axis yAxis, Vector2 coords)
    {
        short x = (short)Math.Ceiling(stickLimit * coords.X);
        controller.SetAxisValue(xAxis, x);

        short y = (short)Math.Ceiling(stickLimit * coords.Y);
        controller.SetAxisValue(yAxis, y);

        Debug.WriteLine("coords: {0}, x: {1}, y: {2}", coords, x, y);
    }

    public void Update()
    {
        // @TODO: Test right stick and remove
        if (isTesting)
        {
            controller.ResetReport();
            controller.SetAxisValue(Xbox360Axis.LeftThumbX, testX);
            controller.SetAxisValue(Xbox360Axis.LeftThumbY, testY);
            controller.SubmitReport();
            return;
        }

        if (!isDirty)
        {
            return;
        }

        controller.ResetReport();

        // This depends on the bit masks being in most populated to less
        // order. That way, we match the desired stick state first and bail.
        // Yes, this means on every update the worst case is that we read
        // through every potential state before failing into the cardinal and
        // quadrant defaults.
        //
        // There is the possibility of a user could add SO MANY mappings that
        // controller update is slowed, but atm I'm not worried about that
        (UInt32, Vector2) leftStickValue = config.LeftStickActions.GetStickAction(actionState);

        if (leftStickValue.Item1 != 0)
        {
            SetStickCoords(Xbox360Axis.LeftThumbX, Xbox360Axis.LeftThumbY, leftStickValue.Item2);
        }
        // 45 degree angles for left stick directions
        else if ((actionState & Stick.LeftStick.TopLeft) == Stick.LeftStick.TopLeft)
        {
            // Top left quadrant
            SetStickCoords(Xbox360Axis.LeftThumbX, Xbox360Axis.LeftThumbY, new Vector2(-.7f, .7f));
        }
        else if ((actionState & Stick.LeftStick.TopRight) == Stick.LeftStick.TopRight)
        {
            // Top right quadrant
            SetStickCoords(Xbox360Axis.LeftThumbX, Xbox360Axis.LeftThumbY, new Vector2(.7f, .7f));
        }
        else if ((actionState & Stick.LeftStick.BottomLeft) == Stick.LeftStick.BottomLeft)
        {
            // Bottom left quadrant
            SetStickCoords(Xbox360Axis.LeftThumbX, Xbox360Axis.LeftThumbY, new Vector2(-.7f, -.7f));
        }
        else if ((actionState & Stick.LeftStick.BottomRight) == Stick.LeftStick.BottomRight)
        {
            // Bottom right quadrant
            SetStickCoords(Xbox360Axis.LeftThumbX, Xbox360Axis.LeftThumbY, new Vector2(.7f, -.7f));
        }
        // Cardinal directions
        else if ((actionState & Stick.LeftStick.Up) != 0)
        {
            controller.SetAxisValue(Xbox360Axis.LeftThumbY, stickLimit);
        }
        else if ((actionState & Stick.LeftStick.Down) != 0)
        {
            controller.SetAxisValue(Xbox360Axis.LeftThumbY, (short)-stickLimit);
        }
        else if ((actionState & Stick.LeftStick.Left) != 0)
        {
            controller.SetAxisValue(Xbox360Axis.LeftThumbX, (short)-stickLimit);
        }
        else if ((actionState & Stick.LeftStick.Right) != 0)
        {
            controller.SetAxisValue(Xbox360Axis.LeftThumbX, stickLimit);
        }

        // Only process the right stick if it wasn't already used
        // for additional left stick support
        //
        // @TODO: Confirm that this is the correct behavior
        if ((leftStickValue.Item1 & Stick.RightStick.Any) == 0)
        {
            (UInt32, Vector2) rightStickValue = config.RightStickActions.GetStickAction(actionState);

            if (rightStickValue.Item1 != 0)
            {
                SetStickCoords(Xbox360Axis.RightThumbX, Xbox360Axis.RightThumbY, rightStickValue.Item2);
            }
            // 45 degree angles for left stick directions
            else if ((actionState & Stick.RightStick.TopLeft) == Stick.RightStick.TopLeft)
            {
                // Top left quadrant
                SetStickCoords(Xbox360Axis.RightThumbX, Xbox360Axis.RightThumbY, new Vector2(-.7f, .7f));
            }
            else if ((actionState & Stick.RightStick.TopRight) == Stick.RightStick.TopRight)
            {
                // Top right quadrant
                SetStickCoords(Xbox360Axis.RightThumbX, Xbox360Axis.RightThumbY, new Vector2(.7f, .7f));
            }
            else if ((actionState & Stick.RightStick.BottomLeft) == Stick.RightStick.BottomLeft)
            {
                // Bottom left quadrant
                SetStickCoords(Xbox360Axis.RightThumbX, Xbox360Axis.RightThumbY, new Vector2(-.7f, -.7f));
            }
            else if ((actionState & Stick.RightStick.BottomRight) == Stick.RightStick.BottomRight)
            {
                // Bottom right quadrant
                SetStickCoords(Xbox360Axis.RightThumbX, Xbox360Axis.RightThumbY, new Vector2(.7f, -.7f));
            }
            // Cardinal directions
            else if ((actionState & Stick.RightStick.Up) != 0)
            {
                controller.SetAxisValue(Xbox360Axis.RightThumbY, stickLimit);
            }
            else if ((actionState & Stick.RightStick.Down) != 0)
            {
                controller.SetAxisValue(Xbox360Axis.RightThumbY, (short)-stickLimit);
            }
            else if ((actionState & Stick.RightStick.Left) != 0)
            {
                controller.SetAxisValue(Xbox360Axis.RightThumbX, (short)-stickLimit);
            }
            else if ((actionState & Stick.RightStick.Right) != 0)
            {
                controller.SetAxisValue(Xbox360Axis.RightThumbX, stickLimit);
            }
        }

        // Use the bottom 16 bits of actionState to set the button state of the controller
        controller.SetButtonsFull((ushort)actionState);

        if ((actionState & (UInt32)Action.LeftTriggerHard) != 0)
        {
            controller.SetSliderValue(Xbox360Slider.LeftTrigger, (byte)(byte.MaxValue * config.LeftTriggerHard));
        }
        else if ((actionState & (UInt32)Action.LeftTriggerMid) != 0)
        {
            controller.SetSliderValue(Xbox360Slider.LeftTrigger, (byte)(byte.MaxValue * config.LeftTriggerMid));
        }
        else if ((actionState & (UInt32)Action.LeftTriggerLight) != 0)
        {
            controller.SetSliderValue(Xbox360Slider.LeftTrigger, (byte)(byte.MaxValue * config.LeftTriggerLight));
        }

        if ((actionState & (UInt32)Action.RightTriggerHard) != 0)
        {
            controller.SetSliderValue(Xbox360Slider.RightTrigger, (byte)(byte.MaxValue * config.RightTriggerHard));
        }
        else if ((actionState & (UInt32)Action.RightTriggerMid) != 0)
        {
            controller.SetSliderValue(Xbox360Slider.RightTrigger, (byte)(byte.MaxValue * config.RightTriggerMid));
        }
        else if ((actionState & (UInt32)Action.RightTriggerLight) != 0)
        {
            controller.SetSliderValue(Xbox360Slider.RightTrigger, (byte)(byte.MaxValue * config.RightTriggerLight));
        }

        controller.SubmitReport();

        isDirty = false;
    }
}

