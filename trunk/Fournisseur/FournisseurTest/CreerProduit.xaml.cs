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
using System.Windows.Shapes;
using System.Windows.Navigation;
using FournisseurTest.DataServiceClient;
using System.Configuration;
using System.Windows.Threading;

namespace FournisseurTest
{
    /// <summary>
    /// Logique d'interaction pour CreateProduct.xaml
    /// </summary>
    public partial class CreerProduit : System.Windows.Controls.Page
    {
        public CreerProduit()
        {
            InitializeComponent();
            InitializeCategorie();
        }

        public void InitializeCategorie()
        {
            List<CATEGORIE> categories = new List<CATEGORIE>();
            try
            {
              string pouet = Properties.Settings.Default.DataServiceClient;
              var ctg = new DADEntities(new Uri(pouet));
              var qry = from ct in ctg.CATEGORIE
              select ct;
              categories = qry.ToList<CATEGORIE>();
             
              Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        this.listCat.ItemsSource = categories;
                       
                    }));
            }
            catch (Exception)
            {
            }
        }

        private void ValidationCreation(object sender, RoutedEventArgs args)
        {
            string reference, nom, marque, prix, stock, description, url;
            bool disponibilite;
            List<CATEGORIE> categ;

            reference = this.textBoxReference.Text;
            nom = this.textBoxNomProduit.Text;
            marque = this.textBoxMarque.Text;
            prix = this.textBoxPrix.Text;
            stock = this.textBoxStock.Text;
            description = this.textBoxDescription.Text;
            url = this.textBoxImage.Text;
            if (this.checkBoxDispo.IsChecked == true)
            {
                disponibilite = true;
            }
            else
            {
                disponibilite = false;
            }
            
            var maliste = this.listCat.SelectedItems;
            Guid[] catTab = (from CATEGORIE cat in maliste
                             select cat.id).ToArray<Guid>();

            int stockInt = 0;
            decimal prixDecimal = decimal.Zero;

            if (Decimal.TryParse(prix, out prixDecimal) && Int32.TryParse(stock, out stockInt))
            {
                //appel du Workflow
                WorkflowCreationProduit.ServiceClient client = null;

                try
                {
                    //On ajoute un produit à la BDD                
                    client = new WorkflowCreationProduit.ServiceClient();
                    if (client.SessionIDVerification(MainWindow.GetInstance().SessionId))
                    {
                        WorkflowCreationProduit.CreateProductState result = client.CreationProductData(reference, nom, marque, description, prixDecimal, stockInt, disponibilite,
                            catTab);
                        Console.WriteLine(result);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    if (client != null)
                    {
                        client.Close();
                        client.ChannelFactory.Close();
                    }
                }
                MainWindow.GetInstance().Frame.Navigate(new ListeProduit());

            }

            Console.WriteLine(nom + " " + reference + " " + marque + " " + prix + " " + stock + " " + description + " " + url + " " + disponibilite + " ");

        }
       
    }
}
