using System.Runtime.InteropServices;

namespace SandblastedAcrylicShowcase
{
    public partial class Form1 : Form
    {
        // The only P/Invokes and struct definitions you need
        [StructLayout(LayoutKind.Sequential)]
        public struct AccentPolicy
        {
            public int AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WindowCompositionAttributeData
        {
            public int Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(
            IntPtr hwnd,
            ref WindowCompositionAttributeData data
        );


        public Form1()
        {
            InitializeComponent();

            this.Opacity = 0.98;
            EnableBlur();
        }

        // The magic happens here!
        private void EnableBlur()
        {
            try {
                var accent = new AccentPolicy
                { AccentState = 4, GradientColor = unchecked((int)0x9F000000) }; // Accentstate = 4 (Acrylic)
    
                var accentStructSize = Marshal.SizeOf(accent);
                var accentPtr = Marshal.AllocHGlobal(accentStructSize);
                Marshal.StructureToPtr(accent, accentPtr, false);
    
                var data = new WindowCompositionAttributeData
                {
                    Attribute = 19,
                    SizeOfData = accentStructSize,
                    Data = accentPtr
                };
    
                _ = SetWindowCompositionAttribute(this.Handle, ref data);
    
                Marshal.FreeHGlobal(accentPtr);
            } catch (Exception) {
                // your error handling code here
            } finally {
                // IMPORTANT: PREVENT MEMORY LEAK
                if (accentPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(accentPtr);
                }
            }
        }

        // Ignore this code
        // Window dragging

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void App_DragTitleBar(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Y <= 24)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0xA1, 0x2, 0); // 0xA1: WM_NCLBUTTONDOWN // 0x2: HT_CAPTION
            }
        }

        private void b_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 Form2 = new();
            Form2.Show();
        }
    }
}
