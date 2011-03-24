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

namespace FournisseurTest
{
    /// <summary>
    /// Logique d'interaction pour CreationCategorie.xaml
    /// </summary>
    public partial class CreationCategorie : Page
    {
        public CreationCategorie()
        {
            InitializeComponent();
        }
        
        private void CreateCategorie(object sender, RoutedEventArgs e)
        {
            string nomCat;
            bool vu = false;
            nomCat = this.textBoxNewCategorie.Text;
            WorkflowCreationCategorie.ServiceClient client = null;
            try
            {
                client = new WorkflowCreationCategorie.ServiceClient();
                if (client.SessionIDVerification(MainWindow.GetInstance().SessionId))
                {
                    WorkflowCreationCategorie.CreateCategorieState result = client.CreationCategorie(Guid.Empty, nomCat, vu);
                    Console.WriteLine(result);
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
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
    }
}
