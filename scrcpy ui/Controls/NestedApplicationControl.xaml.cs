using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace scrcpy_ui.Controls
{
    /// <summary>
    /// Interaction logic for NestedApplicationControl.xaml
    /// </summary>
    public partial class NestedApplicationControl : UserControl
    {
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        private static IntPtr hWndOriginalParent;
        private static IntPtr hWndDocked;
        public static System.Windows.Forms.Panel Panel;



        public Process DockedProcess
        {
            get { return (Process)GetValue(DockedProcessProperty); }
            set
            {
                SetValue(DockedProcessProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for DockedProcess.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DockedProcessProperty =
            DependencyProperty.Register(
                "DockedProcess", typeof(Process), typeof(NestedApplicationControl), 
                new PropertyMetadata(default,new PropertyChangedCallback(OnDockedProcessChanged))
                );

        private static void OnDockedProcessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DockIt((Process)e.NewValue);
        }

        public NestedApplicationControl()
        {
            InitializeComponent();
            Panel = new System.Windows.Forms.Panel();
            Host.Child = Panel;
            //Wire up the event to keep the window sized to match the control
            SizeChanged += window_SizeChanged;
        }

        private static void DockIt(Process process)
        {
            //if (hWndDocked != IntPtr.Zero) //don't do anything if there's already a window docked.
                //return;
            
            while (hWndDocked == IntPtr.Zero)
            {
                //process.WaitForInputIdle(1000); //wait for the window to be ready for input;
                process.Refresh();              //update process info
                if (process.HasExited)
                {
                    return; //abort if the process finished before we got a handle.
                }
                hWndDocked = process.MainWindowHandle;  //cache the window handle
            }
            //Windows API call to change the parent of the target window.
            //It returns the hWnd of the window's parent prior to this call.
            hWndOriginalParent = SetParent(hWndDocked, Panel.Handle);

            
            //Perform an initial call to set the size.
            AlignToPanel();
        }


        private static void AlignToPanel()
        {
            MoveWindow(hWndDocked, 0, 0, Panel.Width, Panel.Height, true);
        }

        void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AlignToPanel();
        }
    }
}
