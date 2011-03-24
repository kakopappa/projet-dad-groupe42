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
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private List<DataServiceClient.PRODUIT> prod;

        public Home()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Page_Loaded);
            this.Initialized += new EventHandler(Home_Initialized);
        }

        /// <summary>
        /// Initialisation de la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Home_Initialized(object sender, EventArgs e)
        {
            MainWindow.GetMe().progressBar.Visibility = Visibility.Visible;

            ThreadPool.QueueUserWorkItem((state) =>
                {
                    DataServiceClient.DADEntities ent = new DataServiceClient.DADEntities(new Uri(Properties.Settings.Default.DataServiceClient));
                    
                    var news = (from p in ent.PRODUIT.Expand("IMAGE")
                                where p.disponible == true && p.supprime == false
                              select p)
                              .Take(6);

                    this.prod = news.ToList<DataServiceClient.PRODUIT>();

                    Dispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action(() =>
                            {
                                this.listNouveautes.ItemsSource = this.prod;
                                MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                            }));
                });
        }

        /// <summary>
        /// Au chargement de la page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMe().SetWindowTheme(WindowTheme.White);

            MainWindow.GetMe().progressBar.Visibility = Visibility.Visible;

            ThreadPool.QueueUserWorkItem((state) =>
            {
                DataServiceClient.DADEntities ent = new DataServiceClient.DADEntities(new Uri(Properties.Settings.Default.DataServiceClient));

                var news = (from p in ent.PRODUIT.Expand("IMAGE")
                            select p)
                          .Take(6);

                this.prod = news.ToList<DataServiceClient.PRODUIT>();

                Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        this.listNouveautes.ItemsSource = this.prod;
                        MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                    }));
            });
        }

        private void listNouveautes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataServiceClient.PRODUIT prod = (DataServiceClient.PRODUIT)this.listNouveautes.SelectedItem;

            if (prod != null)
                MainWindow.GetMe().menu.MeinFrame.Navigate(new SingleProduct(prod, 1, null));
        }
    }
}
