using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using Client.Pages;
using Client.Controls;
using System.Windows.Threading;

namespace Client
{
    public enum WindowTheme
    {
        Black,
        White
    };

    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int WM_SYSCOMMAND = 0x112;
        private HwndSource hwndSource;
        private WindowTheme currentTheme = WindowTheme.White;
        public static MainWindow instance;

        public static MainWindow GetMe()
        {
            return instance;
        }

        public enum ResizeDirection
        {
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8,
        }

        public MainWindow()
        {
            InitializeComponent();
            MainWindow.instance = this;
            this.SourceInitialized += new EventHandler(InitializeWindowSource);
        }

        private void InitializeWindowSource(object sender, EventArgs e)
        {
            hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(hwndSource.Handle, WM_SYSCOMMAND, (IntPtr)(61440 + direction), IntPtr.Zero);
        }

        private void ResetCursor(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Resize(object sender, MouseButtonEventArgs e)
        {
            Rectangle clickedRectangle = sender as Rectangle;

            switch (clickedRectangle.Name)
            {
                case "top":
                    this.Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Top);
                    break;
                case "bottom":
                    this.Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Bottom);
                    break;
                case "left":
                    this.Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Left);
                    break;
                case "right":
                    this.Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Right);
                    break;
                case "topLeft":
                    this.Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.TopLeft);
                    break;
                case "topRight":
                    this.Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.TopRight);
                    break;
                case "bottomLeft":
                    this.Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.BottomLeft);
                    break;
                case "bottomRight":
                    this.Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.BottomRight);
                    break;
                default:
                    break;
            }
        }

        private void DisplayResizeCursor(object sender, MouseEventArgs e)
        {
            Rectangle clickedRectangle = sender as Rectangle;

            switch (clickedRectangle.Name)
            {
                case "top":
                    this.Cursor = Cursors.SizeNS;
                    break;
                case "bottom":
                    this.Cursor = Cursors.SizeNS;
                    break;
                case "left":
                    this.Cursor = Cursors.SizeWE;
                    break;
                case "right":
                    this.Cursor = Cursors.SizeWE;
                    break;
                case "topLeft":
                    this.Cursor = Cursors.SizeNWSE;
                    break;
                case "topRight":
                    this.Cursor = Cursors.SizeNESW;
                    break;
                case "bottomLeft":
                    this.Cursor = Cursors.SizeNESW;
                    break;
                case "bottomRight":
                    this.Cursor = Cursors.SizeNWSE;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Définit le thème à afficher
        /// </summary>
        /// <param name="theme"></param>
        public void SetWindowTheme(WindowTheme theme)
        {
            if (theme != this.currentTheme)
            {
                switch (theme)
                {
                    case WindowTheme.Black:
                        this.Dispatcher.BeginInvoke(
                            DispatcherPriority.Normal,
                            new Action(() =>
                                {
                                    foreach (ZuneMenuElement child in this.menu.Items)
                                    {
                                        child.SetResourceReference(ForegroundProperty, "ThemeBlackColorBrush");
                                    }
                                    this.rootGrid.SetResourceReference(BackgroundProperty, "BgColorWhiteBrush");
                                    this.chrome.SetWindowTheme(theme);
                                    this.yoo1.Visibility = Visibility.Hidden;
                                }));
                        break;

                    case WindowTheme.White:
                        this.Dispatcher.BeginInvoke(
                            DispatcherPriority.Normal,
                            new Action(() =>
                                {
                                    foreach (ZuneMenuElement child in this.menu.Items)
                                    {
                                        child.SetResourceReference(ForegroundProperty, "ThemeWhiteColorBrush");
                                    }
                                    this.rootGrid.SetResourceReference(BackgroundProperty, "BgColorBrush");
                                    this.chrome.SetWindowTheme(theme);
                                    this.yoo1.Visibility = Visibility.Visible;
                                })
                            );
                        break;
                }
            }

            this.currentTheme = theme;
        }
    }
}
