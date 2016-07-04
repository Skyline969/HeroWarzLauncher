using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HeroWarzLauncher.Extensions
{
    internal static class TextboxExtension
    {
        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);

        internal static void SetPlaceholderText(this TextBox textbox, string text)
        {
            SendMessage(textbox.Handle, EM_SETCUEBANNER, 0, text);
        }
    }
}
