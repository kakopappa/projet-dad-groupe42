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
            Console.WriteLine(prod.IMAGE[0].url);
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
        }

        
    }
}

