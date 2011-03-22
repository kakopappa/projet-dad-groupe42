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
using FournisseurTest.DataServiceClient;
using System.Configuration;
using System.Windows.Threading;

namespace FournisseurTest
{
    /// <summary>
    /// Logique d'interaction pour ListeCommande.xaml
    /// </summary>
    public partial class ListeCommande : Page
    {
        COMMANDE_FOURNISSEUR commande = new COMMANDE_FOURNISSEUR();

        public ListeCommande()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ListeCommande_Loaded);
        }

        void ListeCommande_Loaded(object sender, RoutedEventArgs e)
        {
            List<COMMANDE_FOURNISSEUR> commandeFournisseur = new List<COMMANDE_FOURNISSEUR>();
            try
            {
                string pouet = Properties.Settings.Default.DataServiceClient;
                var ctx = new DADEntities(new Uri(pouet));
                var cmdfnr = from cmd in ctx.COMMANDE_FOURNISSEUR
                             select cmd;

                commandeFournisseur = cmdfnr.ToList<COMMANDE_FOURNISSEUR>();

                Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                      new Action(() =>
                      {
                          this.listCommande.ItemsSource = commandeFournisseur;
                          this.listCommandeDetaille.ItemsSource = commandeFournisseur;
                      }));
            }
            catch (Exception)
            {
            }
        }
        //public void LoadModifierCommande(object sender, RoutedEventArgs args)
        //{
        //    MainWindow.GetInstance().Frame.Navigate(new ModifierCommande());
        //}

        public void CommandeSelection(object sender, RoutedEventArgs args)
        {
            commande = (COMMANDE_FOURNISSEUR)this.listCommande.SelectedItem;
        }
    }
}
