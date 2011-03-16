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

namespace TestNavigation
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //DataService.FOURNISSEUR f = DataService.FOURNISSEUR.CreateFOURNISSEUR(Guid.Empty, "george", "pouet@mail.com", "+33235859988", "aligator21", "adre", "vil",
                //"54878");
            DataService.FOURNISSEUR f = new DataService.FOURNISSEUR();
            f.nom = "George";
            f.email = "pouet@mail.com";
            f.phone = "+332658977845";
            f.password = "alig";
            f.adresse = "add";
            f.ville = "ddd";
            f.code_postal = "21545";

            DataService.DADEntities ent = new DataService.DADEntities(new Uri("http://nanaki-sandbox:8080/DataService.svc"));
            ent.AddToFOURNISSEUR(f);
            ent.SaveChanges();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new Uri("Page1.xaml", UriKind.Relative));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(frame.NavigationService.CanGoBack);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new Uri("Page2.xaml", UriKind.Relative));
        }
    }
}
