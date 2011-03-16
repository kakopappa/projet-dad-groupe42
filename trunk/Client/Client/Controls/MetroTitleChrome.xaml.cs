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
using Client;
using System.Windows.Threading;

namespace ChooseForMe
{
    /// <summary>
    /// Logique d'interaction pour MetroTitleChrome.xaml
    /// </summary>
    public partial class MetroTitleChrome : UserControl
    {
        private WindowTheme currentTheme = WindowTheme.White;
        private Window parentWindow;

        public MetroTitleChrome()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the parent window
            this.parentWindow = Window.GetWindow(userControlGrid);
        }

        private void userControlGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Start dragging the window
            this.parentWindow.DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            // Minimize the window
            this.parentWindow.WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.parentWindow.WindowState == WindowState.Normal)
            {
                this.parentWindow.WindowState = WindowState.Maximized;
                pathMaxRes.SetResourceReference(Path.DataProperty, "PathRestore");
            }
            else if (this.parentWindow.WindowState == WindowState.Maximized)
            {
                this.parentWindow.WindowState = WindowState.Normal;
                pathMaxRes.SetResourceReference(Path.DataProperty, "PathMaximize");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            this.parentWindow.Close();
        }

        private void windowTitle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnMaximize_Click(sender, null);
        }

        public void SetWindowTheme(WindowTheme theme)
        {
            switch (theme)
            {
                case WindowTheme.Black:
                    // Let's go
                    this.Dispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action(() =>
                            {
                                this.windowTitle.SetResourceReference(ForegroundProperty, "ThemeBlackColorBrush");
                                this.pathMin.SetResourceReference(Shape.FillProperty, "ThemeBlackColorBrush");
                                this.pathMaxRes.SetResourceReference(Shape.FillProperty, "ThemeBlackColorBrush");
                                this.pathClose.SetResourceReference(Shape.FillProperty, "ThemeBlackColorBrush");
                            }));
                    break;
                case WindowTheme.White:
                    // Let's go
                    this.Dispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action(() =>
                            {
                                this.windowTitle.SetResourceReference(ForegroundProperty, "ThemeWhiteColorBrush");
                                this.pathMin.SetResourceReference(Shape.FillProperty, "ThemeWhiteColorBrush");
                                this.pathMaxRes.SetResourceReference(Shape.FillProperty, "ThemeWhiteColorBrush");
                                this.pathClose.SetResourceReference(Shape.FillProperty, "ThemeWhiteColorBrush");
                            }));
                    break;
            }
            this.currentTheme = theme;
        }

        public string WindowTitle
        {
            get { return (string)GetValue(WindowTitleProperty); }
            set { SetValue(WindowTitleProperty, value); }
        }

        public static readonly DependencyProperty WindowTitleProperty = DependencyProperty.Register(
            "WindowTitle",
            typeof(string),
            typeof(MetroTitleChrome),
            new UIPropertyMetadata("WINDOW NAME"));
    }
}
