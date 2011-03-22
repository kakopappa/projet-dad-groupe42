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
using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Net;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;

namespace Client.Pages
{
    /// <summary>
    /// Logique d'interaction pour Catalog.xaml
    /// </summary>
    public partial class Catalog : Page
    {
        public event EventHandler DataLoaded;

        private List<DataServiceClient.CATEGORIE> cat;
        private List<DataServiceClient.PRODUIT> prod;
        private int productsPerPage = 2;
        private int currentPage = 1;

        public Catalog()
        {
            InitializeComponent();
            this.Unloaded += new RoutedEventHandler(Catalog_Unloaded);
            this.DataLoaded += new EventHandler(Catalog_DataLoaded);
        }

        void Catalog_DataLoaded(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(
                            DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                this.listCat.ItemsSource = this.cat;
                                this.listProducts.ItemsSource = this.prod;
                                Storyboard s = (Storyboard) FindResource("MetroFadeSlide");
                                this.BeginStoryboard(s);
                                MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                            }));
        }

        /// <summary>
        /// Lorsque la page est déchargée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Catalog_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            if(MainWindow.GetMe().progressBar.Visibility == Visibility.Visible)
                                MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                        }));
        }

        /// <summary>
        /// Lorsque la page est chargée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Initialized(object sender, EventArgs e)
        {
            MainWindow.GetMe().SetWindowTheme(WindowTheme.Black);
            MainWindow.GetMe().progressBar.Visibility = Visibility.Visible;
            this.btnPrev.IsEnabled = false;

            ThreadPool.QueueUserWorkItem(state =>
                {
                    
                    DataServiceClient.DADEntities ent = new DataServiceClient.DADEntities(new Uri(Properties.Settings.Default.DataServiceClient));
                    try
                    {
                        this.cat = (from c in ent.CATEGORIE
                                    select c).ToList<DataServiceClient.CATEGORIE>();

                        // Récupération des produits
                        this.prod = GetProducts();

                        DataLoaded(this, null);
                    }
                    catch(WebException)
                    {
                        this.Dispatcher.BeginInvoke(
                            DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                                MessageBox.Show(MainWindow.GetMe(), "Impossible de se connecter au service");
                            }));
                    }
                });
        }

        /// <summary>
        /// Retourne les produits
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        private List<DataServiceClient.PRODUIT> GetProducts(int pageNumber = 1)
        {
            DataServiceClient.DADEntities ent = new DataServiceClient.DADEntities(new Uri(Properties.Settings.Default.DataServiceClient));

            var qry = (from p in ent.PRODUIT.Expand("IMAGE")
                       select p)
                       .Skip((pageNumber - 1) * this.productsPerPage).Take(this.productsPerPage);
            
            return qry.ToList<DataServiceClient.PRODUIT>();
        }

        /// <summary>
        /// Rafraichit la liste des produits
        /// </summary>
        private void RefreshProducts()
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() =>
                    {
                        this.listProducts.ItemsSource = this.prod;
                    }));
        }

        /// <summary>
        /// Clic sur le bouton page suivante
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMe().progressBar.Visibility = Visibility.Visible;
            this.btnNext.IsEnabled = false;

            ThreadPool.QueueUserWorkItem((state) => 
                {
                    this.prod = GetProducts(++this.currentPage);

                    Dispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action(() =>
                            {
                                this.listProducts.ItemsSource = this.prod;
                                if (this.currentPage > 1)
                                {
                                    this.btnNext.IsEnabled = true;
                                    this.btnPrev.IsEnabled = true;
                                }

                                MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                            }));
                });
        }

        /// <summary>
        /// Clic sur le bouton page précédente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMe().progressBar.Visibility = Visibility.Visible;
            this.btnPrev.IsEnabled = false;

            ThreadPool.QueueUserWorkItem((state) =>
            {
                this.prod = GetProducts(--this.currentPage);

                Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        this.listProducts.ItemsSource = this.prod;
                        if (this.currentPage > 1)
                        {
                            this.btnNext.IsEnabled = true;
                            this.btnPrev.IsEnabled = true;
                        }

                        MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                    }));
            });
        }
    }
}
