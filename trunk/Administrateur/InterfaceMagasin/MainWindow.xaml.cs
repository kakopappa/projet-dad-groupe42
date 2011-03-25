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
using InterfaceMagasin.DataService;
using System.Data.Services.Client;
using InterfaceMagasin.ServiceSession;

namespace InterfaceMagasin
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, AdministratorCallback
    {
        public Guid SessionId { get; set; }
        private static MainWindow instance;
        AdministratorClient admin;
        
        public MainWindow()
        {
            InitializeComponent();
            instance = this;
            initiateAppli();
            modifCompteur();
        }

        public static MainWindow GetInstance()
        {
            return instance;
        }

        public void modifCompteur()
        {
            //On remplit la liste à l'ouverture
            try
            {
                DADEntities entities = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                entities.IgnoreResourceNotFoundException = true;

                //Par défaut, on sélectionne les categories
                int compteur = (from cat in entities.CATEGORIE
                                   where !cat.valide
                                   select cat).Count();



                this.TextCompteur.Text = ""+compteur;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        private void initiateAppli()
        {
            admin = new AdministratorClient(new System.ServiceModel.InstanceContext(this));
            try
            {
                this.SessionId = admin.Connect();
            }
            catch
            {
                Console.WriteLine("Erreur de connexion au service");
            }
        }

        //Affichage des fournisseurs
        private void btnFournisseur_Click(object sender, RoutedEventArgs args)
        {
            Frame.Navigate(new Fournisseur());
        }

        //Affichage des produits
        private void btnProduit_Click(object sender, RoutedEventArgs args)
        {
            Frame.Navigate(new Produit());
        }

        //Affichage des categories
        private void btnCategories_Click(object sender, RoutedEventArgs args)
        {
            Frame.Navigate(new Categorie());
        }

        #region AdministratorCallback Membres

        public void CategorieAdded(Guid categorieID)
        {
            throw new NotImplementedException();
        }

        public void DisconnectedAdministrator()
        {
            throw new NotImplementedException();
        }

        public void ChatNotificationAdministrator(Guid correspondentID, string message, ChatState state)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
