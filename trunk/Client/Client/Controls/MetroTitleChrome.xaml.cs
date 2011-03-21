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
using Client.Pages;

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

        /// <summary>
        /// Lorsque le contrôle est chargé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the parent window
            this.parentWindow = Window.GetWindow(userControlGrid);
        }

        /// <summary>
        /// Permet de déplacer la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userControlGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Start dragging the window
            this.parentWindow.DragMove();
        }

        /// <summary>
        /// Minimize la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            // Minimize the window
            this.parentWindow.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Maximize la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Ferme la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            this.parentWindow.Close();
        }

        /// <summary>
        /// Agrandit la fenêtre lors d'un double clic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowTitle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnMaximize_Click(sender, null);
        }

        /// <summary>
        /// Définit le thème de la fenêtre
        /// </summary>
        /// <param name="theme"></param>
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
                                this.signin.SetResourceReference(ForegroundProperty, "ThemeBlackColorBrush");
                                this.signup.SetResourceReference(ForegroundProperty, "ThemeBlackColorBrush");
                                this.logout.SetResourceReference(ForegroundProperty, "ThemeBlackColorBrush");
                                this.pseudo.SetResourceReference(ForegroundProperty, "ThemeBlackColorBrush");
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
                                this.signin.SetResourceReference(ForegroundProperty, "ThemeWhiteColorBrush");
                                this.signup.SetResourceReference(ForegroundProperty, "ThemeWhiteColorBrush");
                                this.logout.SetResourceReference(ForegroundProperty, "ThemeWhiteColorBrush");
                                this.pseudo.SetResourceReference(ForegroundProperty, "ThemeWhiteColorBrush");
                            }));
                    break;
            }
            this.currentTheme = theme;
        }

        /// <summary>
        /// Affiche quelques infos utilisateur
        /// </summary>
        public void LoadUserInfo()
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() =>
                    {
                        this.signin.Visibility = Visibility.Hidden;
                        this.pseudo.Content = String.Format("{0} {1}", MainWindow.GetMe().Client.prenom, MainWindow.GetMe().Client.nom);
                        this.pseudo.Visibility = Visibility.Visible;
                        this.signup.Visibility = Visibility.Hidden;
                        this.logout.Visibility = Visibility.Visible;
                    }));
        }

        /// <summary>
        /// Décharge les infos utilisateurs.
        /// </summary>
        public void UnloadUserInfo()
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() =>
                {
                    this.signin.Visibility = Visibility.Visible;
                    this.pseudo.Visibility = Visibility.Hidden;
                    this.signup.Visibility = Visibility.Visible;
                    this.logout.Visibility = Visibility.Hidden;
                }));
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

        /// <summary>
        /// Connexion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void signin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMe().menu.MeinFrame.Navigate(new SigninAndUp(SignFocus.SIGNIN));
        }

        /// <summary>
        /// Deconnexion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logout_Click(object sender, RoutedEventArgs e)
        {
            this.UnloadUserInfo();
            MainWindow.GetMe().Client = null;
        }

        /// <summary>
        /// Clique sur le pseudo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pseudo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMe().menu.MeinFrame.Navigate(new Profile());
        }

        /// <summary>
        /// Enregistrement d'un nouvel utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void signup_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMe().menu.MeinFrame.Navigate(new SigninAndUp(SignFocus.SIGNUP));
        }
    }
}
