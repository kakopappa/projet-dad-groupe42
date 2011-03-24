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
using Client.DataServiceClient;

namespace Client.Pages
{
    /// <summary>
    /// Logique d'interaction pour DetailCommande.xaml
    /// </summary>
    public partial class DetailCommande : Page
    {
        public COMMANDE_CLIENT Commande
        {
            get
            {
                return (COMMANDE_CLIENT)GetValue(CommandeProperty);
            }

            set
            {
                SetValue(CommandeProperty, value);
            }
        }

        public DetailCommande()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DetailCommande_Loaded);
        }

        void DetailCommande_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMe().SetWindowTheme(WindowTheme.Black);
        }

        public DetailCommande(COMMANDE_CLIENT commande_client)
        {
            InitializeComponent();
            this.Commande = commande_client;
        }

        private void AfficheCommande(COMMANDE_CLIENT commande_client)
        {
            
        }

        public static readonly DependencyProperty CommandeProperty = DependencyProperty.Register(
            "Commande",
            typeof(COMMANDE_CLIENT),
            typeof(DetailCommande));
    }
}
