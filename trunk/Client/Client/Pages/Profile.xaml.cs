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
using System.Windows.Threading;

namespace Client.Pages
{
    /// <summary>
    /// Logique d'interaction pour Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        public DataServiceClient.CLIENT Client
        {
            get
            {
                return (DataServiceClient.CLIENT)GetValue(ClientProperty);
            }

            set
            {
                SetValue(ClientProperty, value);
            }
        }

        public Profile()
        {
            InitializeComponent();
            this.Client = MainWindow.GetMe().Client;
            this.Loaded += new RoutedEventHandler(Profile_Loaded);
        }

        /// <summary>
        /// Page chargée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Profile_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMe().SetWindowTheme(WindowTheme.Black);
            MainWindow.GetMe().menu.SetCurrentPage(null);

            MainWindow.GetMe().progressBar.Visibility = Visibility.Visible;

            ThreadPool.QueueUserWorkItem((state) =>
                {
                    DataServiceClient.DADEntities ent = new DataServiceClient.DADEntities(
                        new Uri(Properties.Settings.Default.DataServiceClient));
                    var q = (from a in ent.ADRESSE_CLIENT
                             where a.CLIENT.id == MainWindow.GetMe().Client.id
                             select a).FirstOrDefault();

                    if (q != null)
                    {
                        Dispatcher.BeginInvoke(
                            DispatcherPriority.Normal,
                            new Action(() =>
                                {
                                    this.adresse.Text = q.adresse;
                                    this.ville.Text = q.ville;
                                    this.codePostal.Text = q.code_postal;
                                    this.pays.Text = q.pays;
                                }));
                    }

                    Dispatcher.BeginInvoke(
                            DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                            }));
                });
        }

        /// <summary>
        /// Clic sur le bouton sauvegarde
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_Click(object sender, RoutedEventArgs e)
        {
            this.infoBox.Text = "";
            this.save.IsEnabled = false;
            MainWindow.GetMe().progressBar.Visibility = Visibility.Visible;

            string nom = this.nom.Text;
            string prenom = this.prenom.Text;
            string phone = this.phone.Text;
            string adresse = this.adresse.Text;
            string ville = this.ville.Text;
            string cp = this.codePostal.Text;
            string pays = this.pays.Text;
            string password = this.password.Password;

            ThreadPool.QueueUserWorkItem((state) =>
                {
                    WorkflowModification.ServiceClient svc = null;

                    try
                    {
                        svc = new WorkflowModification.ServiceClient();
                        if (svc.SessionIDVerification(MainWindow.GetMe().SessionId))
                        {
                            WorkflowModification.ModifyAccountDataState result = svc.ModifyAccountData(
                                phone,
                                prenom,
                                nom,
                                adresse,
                                ville,
                                cp,
                                pays
                                );

                            switch (result)
                            {
                                case WorkflowModification.ModifyAccountDataState.EXECUTED:
                                    Dispatcher.BeginInvoke(
                                        DispatcherPriority.Normal,
                                        new Action(() =>
                                        {
                                            MainWindow.GetMe().RefreshUser();
                                            MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                                            this.infoBox.Foreground = Brushes.Green;
                                            this.infoBox.Text = @"Succès: La modification s'est terminée avec succès";
                                            this.save.IsEnabled = true;
                                        }));
                                    break;

                                default:
                                    Dispatcher.BeginInvoke(
                                        DispatcherPriority.Normal,
                                        new Action(() =>
                                        {
                                            MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                                            this.infoBox.Foreground = Brushes.Red;
                                            this.infoBox.Text = @"Erreur: Un problème est survenu durant la modification";
                                            this.save.IsEnabled = true;
                                        }));
                                    break;
                            }
                        }
                        else
                        {
                            Dispatcher.BeginInvoke(
                                DispatcherPriority.Normal,
                                new Action(() =>
                                    {
                                        MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                                        this.infoBox.Foreground = Brushes.Red;
                                        this.infoBox.Text = @"Erreur: Votre session n'a pas pu être vérifiée";
                                        this.save.IsEnabled = true;
                                    }));
                        }
                    }
                    catch
                    {
                        Dispatcher.BeginInvoke(
                                DispatcherPriority.Normal,
                                new Action(() =>
                                {
                                    MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                                    this.infoBox.Foreground = Brushes.Red;
                                    this.infoBox.Text = @"Erreur: Accès au service impossible";
                                    this.save.IsEnabled = true;
                                }));
                    }
                    finally
                    {
                        if (svc != null)
                        {
                            svc.Close();
                            svc.ChannelFactory.Close();
                        }
                    }

                    // PASSWWWWWWWWWWWWWWWOOOOOOOOOOOOOOORRRRRRRRRRDDDDDDDDDDDDDDDD
                    if (password != "")
                    {
                        Dispatcher.BeginInvoke(
                            DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                MainWindow.GetMe().progressBar.Visibility = Visibility.Visible;
                                this.save.IsEnabled = false;
                            }));

                        WorflowChangePassword.ServiceClient sli = null;

                        try
                        {
                            sli = new WorflowChangePassword.ServiceClient();
                            if (sli.SessionIDVerification(MainWindow.GetMe().SessionId))
                            {
                                WorflowChangePassword.CheckIfPasswordMatchResult pouet = sli.CurrentPasswordVerification(MainWindow.GetMe().Client.password);

                                switch (pouet)
                                {
                                    case WorflowChangePassword.CheckIfPasswordMatchResult.MATCH:
                                        WorflowChangePassword.ModifyPasswordState res = sli.PasswordModification(
                                            Tools.Tools.HashPassword(
                                                MainWindow.GetMe().Client.email,
                                                password));

                                        switch (res)
                                        {
                                            case WorflowChangePassword.ModifyPasswordState.EXECUTED:
                                                Dispatcher.BeginInvoke(
                                                    DispatcherPriority.Normal,
                                                    new Action(() =>
                                                    {
                                                        this.infoBox.Foreground = Brushes.Green;
                                                        this.infoBox.Text = @"Succès: La modification s'est terminée avec succès";
                                                        this.save.IsEnabled = true;
                                                        MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                                                    }));

                                                ThreadPool.QueueUserWorkItem((st) =>
                                                    {
                                                        MainWindow.GetMe().chrome.UnloadUserInfo();
                                                        MainWindow.GetMe().Client = null;
                                                        MainWindow.GetMe().MeinService.DisconnectClient(MainWindow.GetMe().SessionId);
                                                    });
                                                break;

                                            default:
                                                Dispatcher.BeginInvoke(
                                                    DispatcherPriority.Normal,
                                                    new Action(() =>
                                                    {
                                                        this.infoBox.Foreground = Brushes.Red;
                                                        this.infoBox.Text = @"Erreur: La modification du mot de passe a échoué";
                                                        this.save.IsEnabled = true;
                                                        MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                                                    }));
                                                break;
                                        }
                                        break;

                                    default:
                                        Dispatcher.BeginInvoke(
                                            DispatcherPriority.Normal,
                                            new Action(() =>
                                            {
                                                this.infoBox.Foreground = Brushes.Red;
                                                this.infoBox.Text = @"Erreur: Pouet l'erreur";
                                                this.save.IsEnabled = true;
                                                MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                                            }));
                                        break;
                                }
                            }
                        }
                        catch
                        {
                            Dispatcher.BeginInvoke(
                            DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                                this.infoBox.Foreground = Brushes.Red;
                                this.infoBox.Text = @"Erreur: Accès au service impossible";
                                this.save.IsEnabled = true;
                            }));
                        }
                        finally
                        {
                            if (sli != null)
                            {
                                sli.Close();
                                sli.ChannelFactory.Close();
                            }
                        }
                    }

                });
        }

        public static readonly DependencyProperty ClientProperty = DependencyProperty.Register(
            "Client",
            typeof(DataServiceClient.CLIENT),
            typeof(Profile));
    }
}
