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
using InterfaceMagasin.DataService;

namespace InterfaceMagasin
{
    /// <summary>
    /// Logique d'interaction pour Categorie.xaml
    /// </summary>
    public partial class Categorie : Page
    {
        //Les Variables
        CATEGORIE categorie;
        List<CATEGORIE> maListCategorie;

        CATEGORIE categorieAttente;
        List<CATEGORIE> maListCategorieAttente;


        public Categorie()
        {
            InitializeComponent();
            initiateFormulaire();
        }

        //méthode d'initialisation
        private void initiateFormulaire()
        {
            //On remplit la liste à l'ouverture
            try
            {
                DADEntities entities = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                entities.IgnoreResourceNotFoundException = true;

                //Par défaut, on sélectionne les categories valides
                var query = from c in entities.CATEGORIE
                            where c.valide
                            select c;

                //On récupère la liste des categories
                maListCategorie = query.ToList<CATEGORIE>();

                //Par défaut, on sélectionne les categories valides
                var queryAttente = from cat in entities.CATEGORIE
                            where !cat.valide
                            select cat;

                //On récupère la liste des categories
                maListCategorieAttente = queryAttente.ToList<CATEGORIE>();

                //On associe la liste de categories en attente à la Listbox                    
                this.ListEnAttente.ItemsSource = maListCategorieAttente;
                //On associe la liste des categories existantes à la List Box
                this.ListCategories.ItemsSource = maListCategorie;

                //Boutons actifs
                this.ButtonCreer.IsEnabled = true;
                this.ButtonNouveau.IsEnabled = true;
                //Boutons inactifs
                this.ButtonSupprimer.IsEnabled = false;
                this.ButtonModif.IsEnabled = false;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        ////Event pour la recherche d'une categorie/////
        private void btnSelectionner_Click(object sender, RoutedEventArgs args)
        {
            string selection = this.TextSelection.Text;

            try
            {
                DADEntities entities = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                entities.IgnoreResourceNotFoundException = true;

                //Par défaut, on sélectionne tout
                var query = from c in entities.CATEGORIE
                            where c.valide
                            select c;

                //Si le champ selectionner est rempli
                if (selection != "")
                {
                    query = from c in entities.CATEGORIE
                            where c.valide && c.nom == selection
                            select c;
                }

                //On récupère la liste des categories
                maListCategorie = query.ToList<CATEGORIE>();
                //On associe la liste des categories existantes à la List Box
                this.ListCategories.ItemsSource = maListCategorie;

                this.TextNom.Text = null;
                this.ComboCategorie.SelectedItem = null;
                this.CheckValide.IsChecked = false;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /////Event pour la sélection d'une categorie existante dans la liste/////
        private void listCatExist_Selection(object sender, RoutedEventArgs args)
        {
            
            //On sélectionne la categorie
            categorie = (CATEGORIE)this.ListCategories.SelectedItem;

            if (categorie != null)
            {
                this.TextNom.Text = categorie.nom;
                this.CheckValide.IsChecked = categorie.valide;

                //Boutons actifs/inactifs
                this.ButtonCreer.IsEnabled = false;
                this.ButtonNouveau.IsEnabled = true;
                this.ButtonSupprimer.IsEnabled = true;
                this.ButtonModif.IsEnabled = true;
            }
            
        }



        /////Event pour la sélection d'une categorie en attente dans la liste/////
        private void listAttente_Selection(object sender, RoutedEventArgs args)
        {
            //On sélectionne le fournisseur
            categorieAttente = (CATEGORIE)this.ListEnAttente.SelectedItem;
            this.TextNom.Text = categorieAttente.nom;
            this.CheckValide.IsChecked = categorieAttente.valide;

            //Boutons actifs/inactifs
            this.ButtonCreer.IsEnabled = false;
            this.ButtonNouveau.IsEnabled = true;
            this.ButtonSupprimer.IsEnabled = true;
            this.ButtonModif.IsEnabled = true;

        }



        /////Event pour le bouton nouveau/////
        private void click_Nouveau(object sender, RoutedEventArgs args)
        {
            this.ListCategories.SelectedItem = null;
            this.ListEnAttente.SelectedItem = null;
            this.TextNom.Text = null;
            this.ComboCategorie.SelectedItem = null;
            this.CheckValide.IsChecked = false;
            categorie = null;
            categorieAttente = null;

            //Boutons actifs/inactifs
            this.ButtonCreer.IsEnabled = true;
            this.ButtonNouveau.IsEnabled = true;
            this.ButtonSupprimer.IsEnabled = false;
            this.ButtonModif.IsEnabled = false;
        }


        /////Event pour le bouton créer/////
        private void click_Creer(object sender, RoutedEventArgs args)
        {
            initiateFormulaire();
        }


        /////Event pour le bouton modifier/////
        private void click_Modifier(object sender, RoutedEventArgs args)
        {
            initiateFormulaire();
            this.TextNom.Text = null;
            this.ComboCategorie.SelectedItem = null;
            this.CheckValide.IsChecked = false;
        }


        /////Event pour le bouton supprimer/////
        private void click_Supprimer(object sender, RoutedEventArgs args)
        {
            initiateFormulaire();
        }
    }
}
