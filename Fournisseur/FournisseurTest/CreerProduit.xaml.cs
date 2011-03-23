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
       
    }
}
