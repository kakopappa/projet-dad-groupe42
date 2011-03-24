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

namespace InterfaceMagasin
{
    /// <summary>
    /// Logique d'interaction pour DetailCommande.xaml
    /// </summary>
    public partial class DetailCommande : Page
    {
        public DetailCommande()
        {
            InitializeComponent();
        }

        public DetailCommande(COMMANDE_CLIENT commande_client)
        {
            InitializeComponent();
            AfficheCommande(commande_client);

        }

        private void AfficheCommande(COMMANDE_CLIENT commande_client)
        {
            this.
        }
    }
}
