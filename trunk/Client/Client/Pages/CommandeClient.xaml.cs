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
    /// Logique d'interaction pour CommandeClient.xaml
    /// </summary>
    public partial class CommandeClient : Page
    {
        //Variable
        COMMANDE_CLIENT commande_client;

        public CommandeClient()
        {
            InitializeComponent();
            ListeCommande_Loaded();
            this.ButtonDetail.IsEnabled = false;
        }

        private void ListeCommande_Loaded()
        {
            MainWindow.GetMe().SetWindowTheme(WindowTheme.Black);

            try
            {
                string pouet = Properties.Settings.Default.DataServiceClient;
                var ctx = new DADEntities(new Uri(pouet));
                
                var queryCommande = from cmd in ctx.COMMANDE_CLIENT.Expand("CLIENT")
                             where cmd.CLIENT.id == MainWindow.GetMe().Client.id
                             select cmd;

                List<COMMANDE_CLIENT> commandeClient = queryCommande.ToList<COMMANDE_CLIENT>();

                this.ListCommande.ItemsSource = commandeClient;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            this.ButtonDetail.IsEnabled = false;
        }

        private void clickDetail(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMe().menu.MeinFrame.Navigate(new DetailCommande(commande_client));
        }

        private void ListCommande_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            commande_client = (COMMANDE_CLIENT)this.ListCommande.SelectedItem;
            this.ButtonDetail.IsEnabled = true;
        }
    }
}
