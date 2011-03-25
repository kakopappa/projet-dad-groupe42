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
using Client.DataServiceClient;

namespace Client.Pages
{
    /// <summary>
    /// Logique d'interaction pour DetailCommande.xaml
    /// </summary>
    public partial class DetailCommande : Page
    {
        public class CommandeF
        {
            public List<Object> Products { get; set; }
            public string NomFournisseur { get; set; }
            public string Etat { get; set; }
            public int Quantite { get; set; }
            public decimal Prix { get; set; }
        }

        public COMMANDE_CLIENT Commande
        {
            get
            {
                return (COMMANDE_CLIENT)GetValue(CommandeProperty);
            }

            set
            {
                SetValue(CommandeProperty, value);
            }
        }
        public List<CommandeF> CommandesFournisseurs
        {
            get;
            set;
        }

        public DetailCommande()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DetailCommande_Loaded);
        }

        void DetailCommande_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMe().SetWindowTheme(WindowTheme.Black);

            var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataServiceClient));
            ctx.IgnoreResourceNotFoundException = true;

            var commandeFournisseurs = (from c in ctx.COMMANDE_FOURNISSEUR.Expand("FOURNISSEUR,COMMANDER/PRODUIT")
                                        where c.COMMANDE_CLIENT.id == this.Commande.id
                                        select c).ToList<COMMANDE_FOURNISSEUR>();
            this.CommandesFournisseurs = new List<CommandeF>();

            foreach (COMMANDE_FOURNISSEUR commandeFournisseur in commandeFournisseurs)
            {
                string etat = String.Empty;
                if (commandeFournisseur.date_reception.HasValue)
                    etat = "RECU";
                else if (commandeFournisseur.date_expedition.HasValue)
                    etat = "EXPEDIE";
                else if (commandeFournisseur.date_validation.HasValue)
                    etat = "EN COURS DE PREPARATION";
                else
                    etat = "EN COURS DE VALIDATION";

                CommandeF commandeF = new CommandeF();
                commandeF.Etat = etat;
                commandeF.NomFournisseur = commandeFournisseur.FOURNISSEUR.nom;
                commandeF.Products = (from COMMANDER c in commandeFournisseur.COMMANDER
                                      select (Object)new {nom = c.PRODUIT.nom, prix = c.PRODUIT.prix, quantite = c.quantite}).ToList();

                this.CommandesFournisseurs.Add(commandeF);
            }

            this.listCommandes.ItemsSource = this.CommandesFournisseurs;
        }

        public DetailCommande(COMMANDE_CLIENT commande_client)
        {
            InitializeComponent();
            this.Commande = commande_client;
        }

        public static readonly DependencyProperty CommandeProperty = DependencyProperty.Register(
            "Commande",
            typeof(COMMANDE_CLIENT),
            typeof(DetailCommande));
    }
}
