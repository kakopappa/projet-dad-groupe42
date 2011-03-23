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
using FournisseurTest.SessionService;



namespace FournisseurTest
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, FournisseurCallback
    {
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public FournisseurClient SessionService { get; set; }

        private static MainWindow instance;

        public MainWindow()
        {
            InitializeComponent();
            instance = this;

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        public static MainWindow GetInstance()
        {
            return instance;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.SessionService = new FournisseurClient(new System.ServiceModel.InstanceContext(this), "WSDualHttpBinding_Fournisseur");
            this.SessionService.Open();
        }

        private void LoadListeProduit(object sender, RoutedEventArgs args)
        {
            Frame.Navigate(new ListeProduit());
        }
        private void LoadCreerProduit(object sender, RoutedEventArgs args)
        {
            Frame.Navigate(new CreerProduit());
        }
        private void LoadListeCommande(object sender, RoutedEventArgs args)
        {
            Frame.Navigate(new ListeCommande());
        }
        private void LoadLogin(object sender, RoutedEventArgs args)
        {
            Frame.Navigate(new Login());
        }


        public void NewOrder(Guid orderID)
        {
            throw new NotImplementedException();
        }

        public void CategorieNotification(Guid categorieID, bool deleted)
        {
            throw new NotImplementedException();
        }

        public void ProductNotification(Guid itemID, bool deleted)
        {
            throw new NotImplementedException();
        }

        public void ChatNotificationFournisseur(Guid correspondentID, string message, ChatState state)
        {
            throw new NotImplementedException();
        }

        public void DisconnectedFournisseur()
        {
            throw new NotImplementedException();
        }
    }
}
