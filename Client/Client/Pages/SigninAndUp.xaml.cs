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
using System.Threading;
using Client.Events;
using System.Windows.Threading;
using System.IO;
using Client.Core;

namespace Client.Pages
{

    /// <summary>
    /// Logique d'interaction pour SigninAndUp.xaml
    /// </summary>
    public partial class SigninAndUp : Page
    {
        private Connection conn;

        /// <summary>
        /// Constructeur
        /// </summary>
        public SigninAndUp()
        {
            InitializeComponent();
            this.conn = MainWindow.GetMe().Connection;
            this.Loaded += new RoutedEventHandler(SigninAndUp_Loaded);
            this.Unloaded += new RoutedEventHandler(SigninAndUp_Unloaded);
        }

        /// <summary>
        /// Déchargement de la page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SigninAndUp_Unloaded(object sender, RoutedEventArgs e)
        {
            this.conn.WrongCredentials -= conn_WrongCredentials;
            this.conn.ServiceErrors -= conn_ServiceErrors;
            this.conn.Success -= conn_Success;
        }

        /// <summary>
        /// Chargement de la page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SigninAndUp_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMe().SetWindowTheme(WindowTheme.Black);
            MainWindow.GetMe().menu.SetCurrentPage(null);

            this.conn.WrongCredentials += new EventHandler(conn_WrongCredentials);
            this.conn.ServiceErrors += new EventHandler(conn_ServiceErrors);
            this.conn.Success += new ObjectEventHandler(conn_Success);
        }

        /// <summary>
        /// gaygay !
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void conn_Success(object sender, ObjectEventArgs e)
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() =>
                {
                    Properties.Settings.Default.ClientPassword = this.siPassword.Password;
                    MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                    this.siConnect.IsEnabled = true;
                }));
        }

        /// <summary>
        /// Erreur de service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void conn_ServiceErrors(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() =>
                {
                    MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                    this.siInfoBox.Foreground = Brushes.Red;
                    this.siInfoBox.Text = @"Erreur: la connexion au service a échoué, veuillez réessayer ultérieurement";
                    this.siConnect.IsEnabled = true;
                }));
        }

        /// <summary>
        /// Identifiants erronés
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void conn_WrongCredentials(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() =>
                    {
                        MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                        this.siInfoBox.Foreground = Brushes.Red;
                        this.siInfoBox.Text = @"Erreur: identifiants erronnés";
                        this.siConnect.IsEnabled = true;
                    }));
        }

        /// <summary>
        /// Lors d'un clic sur le bouton connect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void siConnect_Click(object sender, RoutedEventArgs e)
        {
            if (this.siMail.Text != "" && Tools.Tools.isEmail(this.siMail.Text) && this.siPassword.Password != "")
            {
                MainWindow.GetMe().progressBar.Visibility = Visibility.Visible;
                this.siInfoBox.Text = "";
                this.siConnect.IsEnabled = false;

                string mail = this.siMail.Text;
                string password = this.siPassword.Password;

                // Connexion !
                ThreadPool.QueueUserWorkItem((state) => this.conn.Connect(mail, password));
            }
            else
            {
                this.siInfoBox.Foreground = Brushes.Red;
                this.siInfoBox.Text = @"Erreur: Veuillez renseigner tous les champs correctement";
            }
        }

        private void suCreate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
