﻿using System.Reflection;
using System.Runtime.InteropServices;
using System;
using System.Windows.Forms;
using SoftRectangle.Config;

namespace SoftRectangle;
public class KeyboardHook
{
    #region pinvoke details

    private enum HookType : int
    {
        WH_JOURNALRECORD = 0,
        WH_JOURNALPLAYBACK = 1,
        WH_KEYBOARD = 2,
        WH_GETMESSAGE = 3,
        WH_CALLWNDPROC = 4,
        WH_CBT = 5,
        WH_SYSMSGFILTER = 6,
        WH_MOUSE = 7,
        WH_HARDWARE = 8,
        WH_DEBUG = 9,
        WH_SHELL = 10,
        WH_FOREGROUNDIDLE = 11,
        WH_CALLWNDPROCRET = 12,
        WH_KEYBOARD_LL = 13,
        WH_MOUSE_LL = 14
    }

    public struct KBDLLHOOKSTRUCT
    {
        public UInt32 vkCode;
        public UInt32 scanCode;
        public UInt32 flags;
        public UInt32 time;
        public IntPtr extraInfo;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowsHookEx(
        HookType code,
        HookProc func,
        IntPtr instance,
        int threadID
    );

    [DllImport("user32.dll")]
    private static extern int UnhookWindowsHookEx(IntPtr hook);

    [DllImport("user32.dll")]
    private static extern int CallNextHookEx(
        IntPtr hook,
        int code,
        IntPtr wParam,
        ref KBDLLHOOKSTRUCT lParam
    );

    #endregion

    HookType _hookType = HookType.WH_KEYBOARD_LL;
    IntPtr _hookHandle = IntPtr.Zero;
    HookProc _hookFunction = null;

    // Hook method called by system
    private delegate int HookProc(int code, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

    private AppConfig _appConfig;

    // Events
    public delegate void HookEventHandler(object sender, HookEventArgs e);
    public event HookEventHandler KeyDown;
    public event HookEventHandler KeyUp;

    public KeyboardHook(AppConfig appConfig)
    {
        _appConfig = appConfig;
        _hookFunction = new HookProc(HookCallback);
    }

    // Hook function called by system
    private int HookCallback(int code, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam)
    {
        if (code < 0)
            return CallNextHookEx(_hookHandle, code, wParam, ref lParam);

        if (_appConfig.PassthroughKeys.Contains((Keys)lParam.vkCode))
        {
            return CallNextHookEx(_hookHandle, code, wParam, ref lParam);
        }

        // KeyUp event
        if ((lParam.flags & 0x80) != 0 && this.KeyUp != null)
            this.KeyUp(this, new HookEventArgs(lParam.vkCode));

        // KeyDown event
        if ((lParam.flags & 0x80) == 0 && this.KeyDown != null)
            this.KeyDown(this, new HookEventArgs(lParam.vkCode));

        // Do not process the keypress further
        return -1;
    }

    public void Install()
    {
        // Make sure not already installed
        if (_hookHandle != IntPtr.Zero)
        {
            return;
        }

        // Need instance handle to module to create a system-wide hook
        Module[] list = System.Reflection.Assembly.GetExecutingAssembly().GetModules();
        System.Diagnostics.Debug.Assert(list != null && list.Length > 0);

        // Install system-wide hook
        _hookHandle = SetWindowsHookEx(
            _hookType,
            _hookFunction,
            Marshal.GetHINSTANCE(list[0]),
            0
        );
    }

    public void Uninstall()
    {
        if (_hookHandle != IntPtr.Zero)
        {
            // Uninstall system-wide hook
            UnhookWindowsHookEx(_hookHandle);
            _hookHandle = IntPtr.Zero;
        }
    }
}

// The callback method converts the low-level keyboard data into something more
// .NET friendly with the HookEventArgs class.

public class HookEventArgs : EventArgs
{
    // Using Windows.Forms.Keys instead of Input.Key since the Forms.Keys maps
    // to the Win32 KBDLLHOOKSTRUCT virtual key member, where Input.Key does not
    public Keys Key;
    public bool Alt;
    public bool Control;
    public bool Shift;

    public HookEventArgs(UInt32 keyCode)
    {
        // Detect what modifier keys are pressed, using 
        // Windows.Forms.Control.ModifierKeys instead of Keyboard.Modifiers
        // Since Keyboard.Modifiers does not correctly get the state of the 
        // modifier keys when the application does not have focus
        this.Key = (Keys)keyCode;
        this.Alt = (System.Windows.Forms.Control.ModifierKeys & Keys.Alt) != 0;
        this.Control = (System.Windows.Forms.Control.ModifierKeys & Keys.Control) != 0;
        this.Shift = (System.Windows.Forms.Control.ModifierKeys & Keys.Shift) != 0;
    }
}

