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

namespace Client.Pages
{
    /// <summary>
    /// Logique d'interaction pour Catalog.xaml
    /// </summary>
    public partial class Catalog : Page
    {
        public event EventHandler DataLoaded;
        private List<DataServiceClient.CATEGORIE> cat;

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

            ThreadPool.QueueUserWorkItem(state =>
                {
                    
                    DataServiceClient.DADEntities ent = new DataServiceClient.DADEntities(new Uri(Properties.Settings.Default.DataServiceClient));
                    try
                    {
                        this.cat = (from c in ent.CATEGORIE
                                    select c).ToList<DataServiceClient.CATEGORIE>();

                        DataLoaded(this, null);
                    }
                    catch(WebException exc)
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
    }
}
