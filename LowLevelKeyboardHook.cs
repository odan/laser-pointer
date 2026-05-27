using System.Runtime.InteropServices;

namespace LaserPointer;

public class LowLevelKeyboardHook
{
    public class KeyEventArgs : EventArgs
    {
        public Keys KeyCode { get; set; }
    }

    public event EventHandler<KeyEventArgs> OnKeyDown;
    public event EventHandler<KeyEventArgs> OnKeyUp;

    private IntPtr hookID = IntPtr.Zero;
    private LowLevelKeyboardProc proc;

    public LowLevelKeyboardHook()
    {
        proc = HookCallback;
    }

    public void Install()
    {
        hookID = SetWindowsHookEx(WH_KEYBOARD_LL, proc, IntPtr.Zero, 0);
    }

    public void Uninstall()
    {
        if (hookID != IntPtr.Zero)
            UnhookWindowsHookEx(hookID);
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            var key = (Keys)Marshal.ReadInt32(lParam);

            if (wParam == (IntPtr)WM_KEYDOWN)
                OnKeyDown?.Invoke(this, new KeyEventArgs { KeyCode = key });

            else if (wParam == (IntPtr)WM_KEYUP)
                OnKeyUp?.Invoke(this, new KeyEventArgs { KeyCode = key });
        }

        return CallNextHookEx(hookID, nCode, wParam, lParam);
    }

    #region WinAPI
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern bool DestroyIcon(IntPtr hIcon);
    #endregion
}