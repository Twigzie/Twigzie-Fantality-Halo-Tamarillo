using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using PropertyChanged;
using Tamarillo.Classes.Helpers;

namespace Tamarillo.Controls.Windows {

    [AddINotifyPropertyChangedInterface]
    public partial class Home : Window {

        #region Interop

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            switch (msg) {
                case 0x0024:
                    Native.WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
                case 0x0046:

                    var pos = (Native.WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(Native.WINDOWPOS));
                    if ((pos.flags & (int)Native.SWP.NOMOVE) != 0) {
                        return IntPtr.Zero;
                    }

                    var wnd = (Window)HwndSource.FromHwnd(hwnd)?.RootVisual;
                    if (wnd == null) {
                        return IntPtr.Zero;
                    }

                    var changed = false;
                    if (pos.cx < MinWidth) {
                        pos.cx = (int)MinWidth;
                        changed = true;
                    }
                    if (pos.cy < MinHeight) {
                        pos.cy = (int)MinHeight;
                        changed = true;
                    }

                    if (!changed) {
                        return IntPtr.Zero;
                    }

                    Marshal.StructureToPtr(pos, lParam, true);
                    handled = true;
                    break;
            }
            return (IntPtr)0;
        }

        #endregion
        #region Overides

        protected override void OnSourceInitialized(EventArgs e) {

            base.OnSourceInitialized(e);

            var handle = (new WindowInteropHelper(this)).Handle;
            if (handle != IntPtr.Zero) {
                HwndSource.FromHwnd(handle)?.AddHook(WindowProc);
            }

        }

        #endregion

        public Home() {
            InitializeComponent();
        }

        private void OnMenuGithub(object sender, RoutedEventArgs e) {
            Process.Start("https://github.com/Twigzie/Twigzie-Fantality-Halo-Tamarillo");
        }
        private void OnMenuAbout(object sender, RoutedEventArgs e) {
            MessageBox.Show("Coded with love by: Twigzie\r\n\nReason: I'm too lazy to keep renaming map files everytime I want to enable or disable them...\r\n\nSketchness: Absolutely none.. Application doesn't read any sensitive data or mine any bitcoin lol. The source is hosted on Github (... > Github) but if you still don't believe me, you can always compile it yourself (if you haven't already)", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        [SuppressPropertyChangedWarnings]
        private void OnWindowChanged(object sender, EventArgs e) {
            switch (WindowState) {
                case WindowState.Normal:
                    PartBtnRestore.Visibility = Visibility.Collapsed;
                    PartBtnMaximize.Visibility = Visibility.Visible;
                    break;
                case WindowState.Maximized:
                    PartBtnRestore.Visibility = Visibility.Visible;
                    PartBtnMaximize.Visibility = Visibility.Collapsed;
                    break;
                case WindowState.Minimized:
                default:
                    break;
            }
        }
        private void OnWindowMinimize(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }
        private void OnWindowRestore(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Normal;
        }
        private void OnWindowMaximize(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Maximized;
        }
        private void OnWindowClose(object sender, RoutedEventArgs e) {
            Environment.Exit(0);
        }

    }

}