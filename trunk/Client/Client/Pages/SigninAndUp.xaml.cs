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
    public enum SignFocus
    {
        SIGNIN,
        SIGNUP
    }

    /// <summary>
    /// Logique d'interaction pour SigninAndUp.xaml
    /// </summary>
    public partial class SigninAndUp : Page
    {
        public event ObjectEventHandler ConnectionEvent;
        private Connection conn;

        /// <summary>
        /// Constructeur
        /// </summary>
        public SigninAndUp(SignFocus focus)
        {
            InitializeComponent();
            this.conn = MainWindow.GetMe().Connection;
            this.Loaded += new RoutedEventHandler(SigninAndUp_Loaded);
            this.Unloaded += new RoutedEventHandler(SigninAndUp_Unloaded);

            if (focus == SignFocus.SIGNIN)
            {
                this.siMail.Focus();
                this.siConnect.IsDefault = true;
            }
            else
            {
                this.suNom.Focus();
                this.suCreate.IsDefault = true;
            }
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

        void SigninAndUp_ConnectionEvent(object sender, ObjectEventArgs e)
        {
            switch ((WorflowCreation.CheckIfUserExistResult)e.Data)
            {
                case WorflowCreation.CheckIfUserExistResult.EXIST:
                    Dispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                            this.suInfoBox.Foreground = Brushes.Red;
                            this.suInfoBox.Text = @"Erreur: Un utilisateur avec cet email existe déjà";
                        }));
                    break;

                case WorflowCreation.CheckIfUserExistResult.NOT_EXIST:
                    Dispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                            this.suInfoBox.Foreground = Brushes.Green;
                            this.suInfoBox.Text = @"Succès: Votre compte a bien été créé ! Vous pouvez désormais vous connecter";
                        }));
                    break;
            }
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

            this.ConnectionEvent += new ObjectEventHandler(SigninAndUp_ConnectionEvent);
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
                    MainWindow.GetMe().menu.MeinFrame.Navigate(new Home());
                    MainWindow.GetMe().menu.SetCurrentPage(MainWindow.GetMe().home);
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

        /// <summary>
        /// Clic sur le bouton de connexion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void suCreate_Click(object sender, RoutedEventArgs e)
        {
            if (suNom.Text != "" && suPhone.Text != "" && suPrenom.Text != "" && suPassword.Password != "" && suMail.Text != ""
                && Tools.Tools.isEmail(suMail.Text))
            {
                MainWindow.GetMe().progressBar.Visibility = Visibility.Visible;

                string prenom = this.suPrenom.Text;
                string nom = this.suNom.Text;
                string mail = this.suMail.Text;
                string phone = this.suPhone.Text;
                string password = Tools.Tools.HashPassword(mail, this.suPassword.Password);

                ThreadPool.QueueUserWorkItem((state) =>
                    {
                        WorflowCreation.ServiceClient svc = null;
                        
                        try
                        {
                            svc = new WorflowCreation.ServiceClient();
                            WorflowCreation.CheckIfUserExistResult resp = svc.CreateAccount(
                                prenom,
                                nom,
                                mail,
                                phone,
                                password
                            );

                            this.ConnectionEvent(this, new ObjectEventArgs(resp));
                        }
                        catch
                        {
                            Dispatcher.BeginInvoke(
                                DispatcherPriority.Normal,
                                new Action(() =>
                                    {
                                        MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                                        this.suInfoBox.Foreground = Brushes.Red;
                                        this.suInfoBox.Text = @"Erreur: Erreur lors de l'accès au service";
                                    }));
                        }
                        finally
                        {
                            svc.Close();
                            svc.ChannelFactory.Close();
                        }
                    });
            }
            else
            {
                this.suInfoBox.Foreground = Brushes.Red;
                this.suInfoBox.Text = @"Erreur: Veuillez renseigner tous les champs correctement";
            }
        }
    }
}
