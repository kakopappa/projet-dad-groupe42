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
    /// Logique d'interaction pour Cart.xaml
    /// </summary>
    public partial class Cart : Page
    {
        public Cart()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Page chargée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMe().SetWindowTheme(WindowTheme.Black);
            MainWindow.GetMe().menu.SetCurrentPage(null);

            this.listCartItems.ItemsSource = MainWindow.GetMe().Cart.CartItems;
        }

        // Mise à jour de la quantité
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox s = (TextBox)sender;
            Client.Core.Cart.CartItem d = (Client.Core.Cart.CartItem)s.DataContext;
            MainWindow.GetMe().progressBar.Visibility = Visibility.Visible;
            string v = s.Text;

            if (v != "")
            {
                ThreadPool.QueueUserWorkItem((state) =>
                    {
                        MainWindow.GetMe().Cart.Update(d, Int32.Parse(v));

                        Dispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            ((TextBlock)((StackPanel)s.Parent).FindName("total")).Text = String.Format("€{0:F2}", d.Total);
                            MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                        }));
                    });
            }
        }

        // Suppression d'un article
        private void removeItem_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement a = (FrameworkElement)sender;
            Client.Core.Cart.CartItem d = (Client.Core.Cart.CartItem)a.DataContext;
            a.IsEnabled = false;
            MainWindow.GetMe().progressBar.Visibility = Visibility.Visible;

            ThreadPool.QueueUserWorkItem((s) =>
                {
                    MainWindow.GetMe().Cart.Remove(d);

                    Dispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action(() =>
                            {
                                this.listCartItems.ItemsSource = null;
                                this.listCartItems.ItemsSource = MainWindow.GetMe().Cart.CartItems;
                                MainWindow.GetMe().chrome.lblItems.Text = MainWindow.GetMe().Cart.CartItems.Count.ToString();
                                MainWindow.GetMe().progressBar.Visibility = Visibility.Hidden;
                            }));
                });
        }
    }
}
