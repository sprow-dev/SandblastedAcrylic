using System.Runtime.InteropServices;

namespace SandblastedAcrylicShowcase
{
    public partial class Form2 : Form
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


        public Form2()
        {
            InitializeComponent();

            this.Opacity = 0.98;
            EnableBlur();
        }

        // The magic happens here!
        private void EnableBlur()
        {
            var accent = new AccentPolicy
            { AccentState = 4, GradientColor = unchecked((int)0x9F000000) };

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
        }
    }
}
