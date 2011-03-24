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
using FournisseurTest.WorkflowGestionCommande;

namespace FournisseurTest
{
    /// <summary>
    /// Logique d'interaction pour ListeCommande.xaml
    /// </summary>
    public partial class ListeCommande : Page
    {
        COMMANDE_FOURNISSEUR commande = new COMMANDE_FOURNISSEUR();


        public ListeCommande()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ListeCommande_Loaded);
        }

        void ListeCommande_Loaded(object sender, RoutedEventArgs e)
        {
            List<COMMANDE_FOURNISSEUR> commandeFournisseur = new List<COMMANDE_FOURNISSEUR>();
            
          
            try
            {
                string pouet = Properties.Settings.Default.DataServiceClient;
                var ctx = new DADEntities(new Uri(pouet));
                var cmdfnr = from cmd in ctx.COMMANDE_FOURNISSEUR
                             where cmd.FOURNISSEUR.id == MainWindow.GetInstance().UserId
                             select cmd;

                commandeFournisseur = cmdfnr.ToList<COMMANDE_FOURNISSEUR>();
                

                Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                      new Action(() =>
                      {
                          this.listCommande.ItemsSource = commandeFournisseur;
                          
                          
                          Console.WriteLine(cmdfnr);
                     
                      }));
            }
            catch (Exception)
            {
            }
        }
        //public void LoadModifierCommande(object sender, RoutedEventArgs args)
        //{
        //    MainWindow.GetInstance().Frame.Navigate(new ModifierCommande());
        //}

        public void CommandeSelection(object sender, RoutedEventArgs args)
        {
            commande = (COMMANDE_FOURNISSEUR)this.listCommande.SelectedItem;

            // Detail commande à gauche :
            string Valid, Exped;
            Valid = this.textBoxDateValidation.Text;
            Exped = this.textBoxDateExpedition.Text;

            if (string.IsNullOrWhiteSpace(Valid))
            {
                this.checkBoxValiderCommande.IsEnabled = true;
            }
            else{}

            if (string.IsNullOrWhiteSpace(Exped) && !string.IsNullOrWhiteSpace(Valid))
            {
                this.checkBoxExpedierCommande.IsEnabled = true;
            }
            else{}

            if (!string.IsNullOrWhiteSpace(Valid))
            {
                this.textBoxDureeExpedition.IsEnabled = true;
            }
            else
            {
                this.textBoxDureeExpedition.IsEnabled = false;
            }

            //Requete pour afficher les details de la commande
            COMMANDE_FOURNISSEUR commandeFournisseurDetaille;
            COMMANDE_FOURNISSEUR commandeFournisseurClientDetaille;
            try
            {
                string pouet = Properties.Settings.Default.DataServiceClient;
                var ctx = new DADEntities(new Uri(pouet));
                var cmdfn = (from cmd in ctx.COMMANDE_FOURNISSEUR.Expand("COMMANDER/PRODUIT,COMMANDE_CLIENT/ADRESSE_CLIENT/CLIENT")
                             where cmd.FOURNISSEUR.id == MainWindow.GetInstance().UserId
                             select cmd).FirstOrDefault<COMMANDE_FOURNISSEUR>();
                commandeFournisseurDetaille = cmdfn;
                commandeFournisseurClientDetaille = cmdfn;
                Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                      new Action(() =>
                      {
                          this.listCommandeDetaille.ItemsSource = commandeFournisseurDetaille.COMMANDER;
                          this.listCommandeC.DataContext = commandeFournisseurClientDetaille.COMMANDE_CLIENT.ADRESSE_CLIENT.CLIENT;
                          this.textBoxPrenom.DataContext = commandeFournisseurClientDetaille.COMMANDE_CLIENT.ADRESSE_CLIENT.CLIENT;
                          this.textBoxAdresse.DataContext = commandeFournisseurClientDetaille.COMMANDE_CLIENT.ADRESSE_CLIENT;
                          this.textBoxVille.DataContext = commandeFournisseurClientDetaille.COMMANDE_CLIENT.ADRESSE_CLIENT;
                          this.textBoxCodePostal.DataContext = commandeFournisseurClientDetaille.COMMANDE_CLIENT.ADRESSE_CLIENT;
                          this.textBoxPays.DataContext = commandeFournisseurClientDetaille.COMMANDE_CLIENT.ADRESSE_CLIENT;
                          Console.WriteLine(cmdfn);

                      }));
            }
            catch (Exception)
            {
            }

        }

        public void GestionCommande(object sender, RoutedEventArgs args)
        {
            //Récupération des données
            string dureeEstimee;
            bool Validation, Expedition;
            decimal dureeDecimal = decimal.Zero;
            dureeEstimee = this.textBoxDureeExpedition.Text;
            if (this.checkBoxValiderCommande.IsChecked == true)
            {
                Validation = true;
            }
            else
            {
                Validation = false;
            }
            if (this.checkBoxExpedierCommande.IsChecked == true)
            {
                Expedition = true;
            }
            else
            {
                Expedition = false;
            }
            
                //Appel du workflow
                WorkflowGestionCommande.ServiceClient client = null;
                try
                {
                    //On ajoute un produit à la BDD                
                    client = new WorkflowGestionCommande.ServiceClient();
                    if (client.SessionIDVerification(MainWindow.GetInstance().SessionId))
                    {
                        WorkflowGestionCommande.UpdateOrderState result = client.UpdateOrder(commande.id, OrderUpdateAction.VALIDATE, decimal.Zero);
                        Console.WriteLine(result);
                    }
                }
                catch (Exception er)
                {
                    Console.WriteLine(er);
                }
                finally
                {
                    if (client != null)
                    {
                        client.Close();
                        client.ChannelFactory.Close();
                    }
                }
                MainWindow.GetInstance().Frame.Navigate(new ListeCommande());

                if (Decimal.TryParse(dureeEstimee.Replace(".".ToCharArray()[0], ",".ToCharArray()[0]), out dureeDecimal))
                {
                    WorkflowGestionCommande.ServiceClient client1 = null;
                    try
                    {
                        client1 = new WorkflowGestionCommande.ServiceClient();
                        if (client1.SessionIDVerification(MainWindow.GetInstance().SessionId))
                        {
                            WorkflowGestionCommande.UpdateOrderState result1 = client1.UpdateOrder(commande.id, OrderUpdateAction.SEND, dureeDecimal);
                            Console.WriteLine(result1);
                        }
                    }

                    catch (Exception eroor)
                    {
                        Console.WriteLine(eroor);
                    }
                    finally
                    {
                        if (client1 != null)
                        {
                            client1.Close();
                            client1.ChannelFactory.Close();
                        }
                    }
                }
                MainWindow.GetInstance().Frame.Navigate(new ListeCommande());
            //}
        }

    }
}
