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
using System.Windows.Threading;

namespace Client.Pages
{
    /// <summary>
    /// Logique d'interaction pour SingleProduct.xaml
    /// </summary>
    public partial class SingleProduct : Page
    {
        public DataServiceClient.PRODUIT Product
        {
            get
            {
                return (DataServiceClient.PRODUIT)GetValue(ProductProperty);
            }

            set
            {
                SetValue(ProductProperty, value);
            }
        }
        private DataServiceClient.CATEGORIE category;
        private int backCurrentPage;

        public SingleProduct(DataServiceClient.PRODUIT prod, int page, DataServiceClient.CATEGORIE cat)
        {
            InitializeComponent();
            this.Product = prod;
            this.backCurrentPage = page;
            this.category = cat;
        }

        /// <summary>
        /// Quand la fenêtre est chargée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMe().SetWindowTheme(WindowTheme.Black);
            MainWindow.GetMe().menu.SetCurrentPage(MainWindow.GetMe().catalog);

            if (Product.stock > 0)
            {
                this.lblQuantite.Foreground = Brushes.Green;
                this.lblStock.Foreground = Brushes.Green;
            }
            else
            {
                this.addToCart.IsEnabled = false;
            }
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMe().menu.MeinFrame.Navigate(new Catalog(this.backCurrentPage, this.category));
        }

        public static readonly DependencyProperty ProductProperty = DependencyProperty.Register(
            "Product",
            typeof(DataServiceClient.PRODUIT),
            typeof(SingleProduct));

        /// <summary>
        /// Ajout d'un élément au panier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addToCart_Click(object sender, RoutedEventArgs e)
        {
            this.addToCart.IsEnabled = false;
            int quant = Int32.Parse(this.tbQuantite.Text);

            if (quant > this.Product.stock)
            {
                this.infoBox.Foreground = Brushes.Red;
                this.infoBox.Text = @"Il n'y a pas assez de produits en stock";
                this.addToCart.IsEnabled = true;
            }
            else
            {
                MainWindow.GetMe().progressBar.Visibility = Visibility.Visible;
                Core.Cart myCart = MainWindow.GetMe().Cart;
                DataServiceClient.PRODUIT prod = this.Product;

                ThreadPool.QueueUserWorkItem((state) =>
                    {
                        if (myCart.Add(prod, quant))
                        {
                            string q = myCart.CartItems.Count.ToString();

                            Dispatcher.BeginInvoke(
                                DispatcherPriority.Normal,
                                new Action(() =>
                                    {
                                        this.addToCart.IsEnabled = true;
                                        MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                                        MainWindow.GetMe().chrome.lblItems.Text = q;
                                    }));
                        }
                        else
                        {
                            Dispatcher.BeginInvoke(
                                DispatcherPriority.Normal,
                                new Action(() =>
                                {
                                    this.addToCart.IsEnabled = true;
                                    this.infoBox.Foreground = Brushes.Red;
                                    this.infoBox.Text = @"Impossible d'ajouter l'élément au panier";
                                    MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                                }));
                        }
                    });
            }
        }
    }
}
