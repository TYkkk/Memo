using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public static class WindowsTools
{
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(System.IntPtr hwnd, int nCmdShow);

    [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
    public static extern System.IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    public extern static bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, int bAlpha, uint dwFlags);
    public static uint LWA_COLORKEY = 0x00000001;
    public static uint LWA_ALPHA = 0x00000002;

    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int IParam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;
    const int SW_SHOWMINIMIZED = 2;
    const uint WS_CAPTION = 0x00C00000;

    public static void SetMinWindows()
    {
        ShowWindow(GetActiveWindow(), SW_SHOWMINIMIZED);
    }

    public static void SetNoFrameWindow(Rect rect)
    {
        //SetWindowLong(GetActiveWindow(), GWL_STYLE, 369164288 | 0x00000020 | 0x80000);
        SetWindowLong(GetActiveWindow(), GWL_STYLE, 369164288);
        SetWindowPos(GetActiveWindow(), 0, (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, SWP_SHOWWINDOW);
    }

    public static void SetWindowAlpha(int alpha)
    {
        SetLayeredWindowAttributes(GetActiveWindow(), 0, alpha, LWA_ALPHA);
    }

    public static void SetWindowPos(WindowsPosData rect)
    {
        SetWindowPos(GetActiveWindow(), 0, rect.x, rect.y, rect.width, rect.height, SWP_SHOWWINDOW);
    }

    public static void DragWindow(IntPtr window)
    {
        ReleaseCapture();
        SendMessage(window, 0xA1, 0x02, 0);
        SendMessage(window, 0x0202, 0, 0);
    }
}
