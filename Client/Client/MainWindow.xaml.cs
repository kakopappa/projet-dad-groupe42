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
using System.Data.Services.Client;
using System.Threading;
using Client.Core;
using System.ServiceModel;
using Client.Service;

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
    public partial class MainWindow : Window, CustomerCallback
    {
        private const int WM_SYSCOMMAND = 0x112;
        private HwndSource hwndSource;
        private WindowTheme currentTheme = WindowTheme.White;
        public static MainWindow instance;

        public Connection Connection { get; set; }
        public Client.Core.Cart Cart { get; set; }
        public DataServiceClient.CLIENT Client { get; set; }
        public Guid SessionId { get; set; }
        public CustomerClient MeinService { get; set; }

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
            this.Connection = new Connection();
            this.Cart = new Core.Cart();

            this.MeinService = new CustomerClient(new InstanceContext(this), "WSDualHttpBinding_Customer");

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);

            this.Connection.Success += new Events.ObjectEventHandler(Connection_Success);

            this.SourceInitialized += new EventHandler(InitializeWindowSource);
            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
        }

        /// <summary>
        /// Au chargement de la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(Properties.Settings.Default.AutoConnect)
            {
                this.Connection.WrongCredentials += new EventHandler(Connection_Error);
                this.Connection.ServiceErrors += new EventHandler(Connection_Error);
                this.progressBar.Visibility = Visibility.Visible;
                ThreadPool.QueueUserWorkItem((state) =>
                    {
                        this.MeinService.Open();

                        this.Connection.Connect(
                            Properties.Settings.Default.ClientMail,
                            Properties.Settings.Default.ClientPassword);

                        this.Connection.WrongCredentials -= Connection_Error;
                        this.Connection.ServiceErrors -= Connection_Error;
                    });
            }
        }

        /// <summary>
        /// Problème de connexion avec autoconnect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Connection_Error(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() =>
                    {
                        this.progressBar.Visibility = Visibility.Hidden;
                    }));

            Console.WriteLine("Erreur de connexion");
        }

        /// <summary>
        /// Met à jour l'utilisateur
        /// </summary>
        public void RefreshUser()
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        this.progressBar.Visibility = Visibility.Visible;
                    }));

                DataServiceClient.DADEntities ctx = new DataServiceClient.DADEntities(new Uri(Properties.Settings.Default.DataServiceClient));
                ctx.IgnoreResourceNotFoundException = true;

                this.Client = (from c in ctx.CLIENT
                               where c.id == this.Client.id
                               select c).Single();

                Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        this.chrome.LoadUserInfo();
                        this.progressBar.Visibility = Visibility.Hidden;
                    }));
            });
        }

        /// <summary>
        /// Succès de la connexion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Connection_Success(object sender, Events.ObjectEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((state) =>
                {
                    DataServiceClient.DADEntities ctx = new DataServiceClient.DADEntities(new Uri(Properties.Settings.Default.DataServiceClient));
                    ctx.IgnoreResourceNotFoundException = true;

                    // Pouet le session id
                    this.SessionId = ((Guid[])e.Data)[1];

                    this.Client = (from c in ctx.CLIENT
                                   where c.id == ((Guid[])e.Data)[0]
                                 select c).Single();

                    // Active la session
                    try
                    {
                        bool activateProperlyDone = this.MeinService.ActivateClient(this.SessionId, this.Client.id);
                    }
                    catch
                    {
                        Console.WriteLine("Erreur d'activation du compte");
                    }

                    Dispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action(() =>
                            {
                                this.commandes.Visibility = Visibility.Visible;
                                this.chrome.LoadUserInfo();
                                this.progressBar.Visibility = Visibility.Hidden;
                            }));
                });
        }

        /// <summary>
        /// Fermeture de la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.MeinService.Close();
            Properties.Settings.Default.Save();
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
                                    this.menuRectangle.SetResourceReference(Shape.FillProperty, "BgColorBrush");
                                    this.yoo1.Visibility = Visibility.Hidden;
                                    this.progressBar.SetResourceReference(ForegroundProperty, "BgColorBrush");
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
                                    this.menuRectangle.SetResourceReference(Shape.FillProperty, "BgColorWhiteBrush");
                                    this.yoo1.Visibility = Visibility.Visible;
                                    this.progressBar.SetResourceReference(ForegroundProperty, "BgColorWhiteBrush");
                                })
                            );
                        break;
                }
            }

            this.currentTheme = theme;
        }

        /// <summary>
        /// Lorsque la fenêtre est chargée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.home.Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() =>
                    {
                        this.home.NavigateToPage();
                    }));
        }

        /// <summary>
        /// Charge un client dans un thread séparé
        /// </summary>
        /// <param name="id"></param>
        public void LoadClient(Guid id)
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() =>
                    {
                        this.progressBar.Visibility = Visibility.Visible;
                    }));

            ThreadPool.QueueUserWorkItem((state) =>
                {
                    Dispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            this.progressBar.Visibility = Visibility.Hidden;
                        }));
                });
        }

        #region ClientCallback Membres

        public void CartNotification(Guid itemID, int newQuantity, Client.Service.ItemState state)
        {
            throw new NotImplementedException();
        }

        public void OrderNotification(Guid orderID)
        {
            throw new NotImplementedException();
        }

        public void ChatNotification(Guid correspondentID, string message, Client.Service.ChatState state)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region CustomerCallback Membres


        public void DisconnectedClient()
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() =>
                {
                    this.progressBar.Visibility = Visibility.Hidden;
                    this.commandes.Visibility = Visibility.Hidden;
                    MessageBox.Show("Vous avez été déconnecté !");
                    this.menu.MeinFrame.Navigate(new Home());
                }));
        }

        public void ChatNotificationClient(Guid correspondentID, string message, ChatState state)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
