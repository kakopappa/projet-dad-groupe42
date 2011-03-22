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



namespace FournisseurTest
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }

        private static MainWindow instance;

        public MainWindow()
        {
            InitializeComponent();
            instance = this;
        }

        public static MainWindow GetInstance()
        {
            return instance;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
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
        
    }
}
