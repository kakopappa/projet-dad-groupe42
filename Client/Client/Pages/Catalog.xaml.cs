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
        private DataServiceClient.CATEGORIE currentCat = null;

        private int productsPerPage = 6;
        private int currentPage;
        private bool isAlready = false;
        private bool canGoNext = false;

        public Catalog()
        {
            InitializeComponent();
            this.currentPage = 1;
            this.Unloaded += new RoutedEventHandler(Catalog_Unloaded);
            this.DataLoaded += new EventHandler(Catalog_DataLoaded);
        }

        public Catalog(int page, DataServiceClient.CATEGORIE cat)
        {
            InitializeComponent();
            this.currentCat = cat;
            this.currentPage = page;
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
                                if (!this.isAlready)
                                {
                                    Storyboard s = (Storyboard)FindResource("MetroFadeSlide");
                                    this.BeginStoryboard(s);
                                    this.isAlready = true;
                                }
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
            MainWindow.GetMe().progressBar.Visibility = Visibility.Visible;
            if(this.currentPage < 1)
                this.btnPrev.IsEnabled = false;

            ThreadPool.QueueUserWorkItem(state =>
                {
                    
                    DataServiceClient.DADEntities ent = new DataServiceClient.DADEntities(new Uri(Properties.Settings.Default.DataServiceClient));
                    try
                    {
                        this.cat = (from c in ent.CATEGORIE
                                    select c).ToList<DataServiceClient.CATEGORIE>();

                        // Récupération des produits
                        if (this.currentPage > 1)
                        {
                            Dispatcher.BeginInvoke(
                                DispatcherPriority.Normal,
                                new Action(() =>
                                    {
                                        this.btnPrev.IsEnabled = true;
                                    }));
                        }

                        this.prod = GetProducts(this.currentCat, this.currentPage);

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
        private List<DataServiceClient.PRODUIT> GetProducts(DataServiceClient.CATEGORIE cat = null, int pageNumber = 1)
        {
            DataServiceClient.DADEntities ent = new DataServiceClient.DADEntities(new Uri(Properties.Settings.Default.DataServiceClient));

            if (cat != null)
            {

                var qry = (from c in ent.CATEGORIE.Expand("PRODUIT/IMAGE")
                          where c.id == cat.id
                          select c)
                          .Single<DataServiceClient.CATEGORIE>();
                
                var page = (from DataServiceClient.PRODUIT p in qry.PRODUIT
                            where p.disponible == true && p.supprime == false
                            select p)
                            .Skip((pageNumber - 1) * this.productsPerPage).Take(this.productsPerPage);

                if (page.Count() == 0)
                    this.canGoNext = false;
                else
                    this.canGoNext = true;

                return page.ToList<DataServiceClient.PRODUIT>();
            }
            else
            {
                var qry = (from p in ent.PRODUIT.Expand("IMAGE")
                           where p.disponible == true && p.supprime == false
                           select p)
                           .Skip((pageNumber - 1) * this.productsPerPage).Take(this.productsPerPage);


                if (qry.Count() == 0)
                    this.canGoNext = false;
                else
                    this.canGoNext = true;

                return qry.ToList<DataServiceClient.PRODUIT>();
            }
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
                    this.prod = GetProducts(this.currentCat, ++this.currentPage);

                    Dispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action(() =>
                            {
                                if (canGoNext)
                                    this.listProducts.ItemsSource = this.prod;
                                if (this.currentPage > 1)
                                {
                                    this.btnNext.IsEnabled = true;
                                    this.btnPrev.IsEnabled = true;
                                }
                                if (!canGoNext)
                                {
                                    currentPage--;
                                    this.btnNext.IsEnabled = false;
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
                this.prod = GetProducts(this.currentCat, --this.currentPage);

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
        /// Lorsque la selection change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataServiceClient.PRODUIT prod = (DataServiceClient.PRODUIT)this.listProducts.SelectedItem;
            if (prod != null)
                MainWindow.GetMe().menu.MeinFrame.Navigate(new SingleProduct(prod, this.currentPage, this.currentCat));
        }

        /// <summary>
        /// Lorsqu'on choisit la catégorie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataServiceClient.CATEGORIE cat = (DataServiceClient.CATEGORIE)this.listCat.SelectedItem;
            this.currentCat = cat;
            this.currentPage = 1;

            this.btnPrev.IsEnabled = false;
            this.btnNext.IsEnabled = true;

            MainWindow.GetMe().progressBar.Visibility = Visibility.Visible;

            ThreadPool.QueueUserWorkItem((state) =>
                {
                    this.prod = GetProducts(
                        cat,
                        this.currentPage);

                    DataLoaded(this, null);
                });
        }

        /// <summary>
        /// Lorsque la page est chargée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            MainWindow.GetMe().SetWindowTheme(WindowTheme.Black);
            MainWindow.GetMe().menu.SetCurrentPage(MainWindow.GetMe().catalog);
        }
    }
}
