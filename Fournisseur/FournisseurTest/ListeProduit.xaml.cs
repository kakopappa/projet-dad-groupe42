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
    /// Logique d'interaction pour ListeProduit.xaml
    /// </summary>
    public partial class ListeProduit : Page
    {

        PRODUIT prod = new PRODUIT();
        public ListeProduit()
        {
            InitializeComponent();
            InitializeProduct();
            InitializeCategorie();
        }

        public void InitializeProduct()
        {        
            List<PRODUIT> produits = new List<PRODUIT>();
            List<IMAGE> images = new List<IMAGE>();
            
            try
            {
              string pouet = Properties.Settings.Default.DataServiceClient;
              var ctx = new DADEntities(new Uri(pouet));
              var qry = from p in ctx.PRODUIT.Expand("IMAGE,CATEGORIE,FOURNISSEUR")
              where p.FOURNISSEUR.id == MainWindow.GetInstance().UserId
              select p;
              produits = qry.ToList<PRODUIT>();
             
              Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        this.list.ItemsSource = produits;
                        this.listDetaille.ItemsSource = produits;
                      
                    }));
            }
            catch (Exception)
            {
            }
         }

        //public void LoadModifierProduit(object sender, RoutedEventArgs args)
        //{
        //    MainWindow.GetInstance().Frame.Navigate(new ModifierProduit());
        //}
        
        public void ProduitSelection(object sender, RoutedEventArgs args)
        {
            prod  = (PRODUIT)this.list.SelectedItem;
            //Console.WriteLine(prod.IMAGE[0].url);
            if (prod.disponible == true)
            {
                this.checkBoxDisponible.IsChecked = true;
            }
            else
            {
                this.checkBoxDisponible.IsChecked = false;
            }
            
             List<CATEGORIE> categ = new List<CATEGORIE>();
             categ = prod.CATEGORIE.ToList<CATEGORIE>();
             this.listBoxCategorie.ItemsSource = categ;
             this.buttonModifier.IsEnabled = true;
             this.buttonSupprimerProduit.IsEnabled = true;

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

        public void ProduitModification(object sender, RoutedEventArgs args)
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
            if (this.checkBoxDisponible.IsChecked == true)
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

            if (Decimal.TryParse(prix.Substring(1).Replace(".".ToCharArray()[0], ",".ToCharArray()[0]) , out prixDecimal) 
                && Int32.TryParse(stock, out stockInt))
            {
                //appel du Workflow
                WorkflowModificationProduit.ServiceClient client = null;
                try
                {
                    //On ajoute un produit à la BDD                
                    client = new WorkflowModificationProduit.ServiceClient();
                    if (client.SessionIDVerification(MainWindow.GetInstance().SessionId))
                    {
                        WorkflowModificationProduit.ModifyProductDataState result = client.ModifyProductData(reference, nom, marque, description, prixDecimal, stockInt, prod.id, disponibilite,
                            catTab, url);
                        //WorkflowCreationProduit.CreateProductState result = client.CreationProductData(reference, nom, marque, description, prixDecimal, stockInt, disponibilite,
                        //    catTab);
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

        public void LoadCreateCat(object sender, RoutedEventArgs args)
        {

            MainWindow.GetInstance().Frame.Navigate(new CreationCategorie());
        }
    }
}


