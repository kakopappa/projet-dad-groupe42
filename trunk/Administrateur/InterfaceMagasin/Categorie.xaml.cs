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
                //On associe la comboBox au categorie existantes
                this.ComboCategorie.ItemsSource = maListCategorie;
                categorie = null;


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

        ////////////////////////////////////////////////
        ////Event pour la recherche d'une categorie/////
        ////////////////////////////////////////////////
        private void btnSelectionner_Click(object sender, RoutedEventArgs args)
        {
            string selection = this.TextSelection.Text;

            try
            {
                DADEntities entities = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                entities.IgnoreResourceNotFoundException = true;

                //Par défaut, on sélectionne tout
                var query = from c in entities.CATEGORIE.Expand("CATEGORIE2")
                            where c.valide
                            select c;

                //Si le champ selectionner est rempli
                if (selection != "")
                {
                    query = from c in entities.CATEGORIE.Expand("CATEGORIE2")
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

        /////////////////////////////////////////////////////////////////////////
        /////Event pour la sélection d'une categorie existante dans la liste/////
        /////////////////////////////////////////////////////////////////////////
        private void listCatExist_Selection(object sender, RoutedEventArgs args)
        {
            //On déselectionne la liste en attente
            this.ListEnAttente.SelectedItem = null;
            
            //On sélectionne la categorie
            categorie = (CATEGORIE)this.ListCategories.SelectedItem;

            if (categorie != null)
            {
                this.TextNom.Text = categorie.nom;
                this.ComboCategorie.SelectedItem = categorie.CATEGORIE2;
                this.CheckValide.IsChecked = categorie.valide;

                //Boutons actifs/inactifs
                this.ButtonCreer.IsEnabled = false;
                this.ButtonNouveau.IsEnabled = true;
                this.ButtonSupprimer.IsEnabled = true;
                this.ButtonModif.IsEnabled = true;
            }
        }


        //////////////////////////////////////////////////////////////////////////
        /////Event pour la sélection d'une categorie en attente dans la liste/////
        //////////////////////////////////////////////////////////////////////////
        private void listAttente_Selection(object sender, RoutedEventArgs args)
        {
            //On déselectionne la liste
            this.ListCategories.SelectedItem = null;

            //On sélectionne le la categorie
            categorie = (CATEGORIE)this.ListEnAttente.SelectedItem;

            if (categorie != null)
            {

                this.TextNom.Text = categorie.nom;
                this.ComboCategorie.SelectedItem = categorie.CATEGORIE2;
                this.CheckValide.IsChecked = categorie.valide;

                //Boutons actifs/inactifs
                this.ButtonCreer.IsEnabled = false;
                this.ButtonNouveau.IsEnabled = true;
                this.ButtonSupprimer.IsEnabled = true;
                this.ButtonModif.IsEnabled = true;

            }

        }


        //////////////////////////////////////
        /////Event pour le bouton nouveau/////
        //////////////////////////////////////
        private void click_Nouveau(object sender, RoutedEventArgs args)
        {
            categorie = null;
            this.TextNom.Text = "";
            this.ComboCategorie.SelectedItem = null;
            this.CheckValide.IsChecked = false;
        }

        ////////////////////////////////////
        /////Event pour le bouton créer/////
        ////////////////////////////////////
        private void click_Creer(object sender, RoutedEventArgs args)
        {
                CategorieCreation.ServiceClient client = null;

                try
                {
                    DADEntities entitiescreate = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                    entitiescreate.IgnoreResourceNotFoundException = true;

                    //Manip de modification
                    client = new CategorieCreation.ServiceClient();

                    //On récupère les infos;
                    CATEGORIE catParent = (CATEGORIE)this.ComboCategorie.SelectedItem;
                    string name = this.TextNom.Text;
                    bool valide;

                    if (this.CheckValide.IsChecked == true)
                    {
                        valide = true;
                    }
                    else
                    {
                        valide = false;
                    }

                    if (client.SessionIDVerification(MainWindow.GetInstance().SessionId))
                    {
                        CategorieCreation.CreateCategorieState state = client.CreationCategorie(catParent.id, name, valide);

                        //Si ce n'est pas exécuté
                        if (state != CategorieCreation.CreateCategorieState.EXECUTED)
                        {

                        }
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                initiateFormulaire();
                this.TextNom.Text = null;
                this.ComboCategorie.SelectedItem = null;
                this.CheckValide.IsChecked = false;

        }
            


        ///////////////////////////////////////
        /////Event pour le bouton modifier/////
        ///////////////////////////////////////
        private void click_Modifier(object sender, RoutedEventArgs args)
        {
            if (this.ListCategories.SelectedItem != null || this.ListEnAttente.SelectedItem != null)
            {

                CategorieModification.ServiceClient client = null;

                try
                {
                    DADEntities entitiesModif = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                    entitiesModif.IgnoreResourceNotFoundException = true;

                    //Manip de modification
                    client = new CategorieModification.ServiceClient();

                    //On récupère les infos;
                    CATEGORIE catParent = (CATEGORIE)this.ComboCategorie.SelectedItem;
                    string name = this.TextNom.Text;
                    bool valide;
                    Guid ID = categorie.id;

                    if(this.CheckValide.IsChecked == true)
                    {
                        valide = true;
                    }
                    else
                    {
                        valide = false;
                    }

                    if (client.SessionIDVerification(MainWindow.GetInstance().SessionId))
                    {
                        CategorieModification.ModifyCategorieDataState state = client.ModificationCategorie(catParent.id,name,valide,ID);

                        //Si ce n'est pas exécuté
                        if (state != CategorieModification.ModifyCategorieDataState.EXECUTED)
                        {

                        }
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }

            initiateFormulaire();
            this.TextNom.Text = null;
            this.ComboCategorie.SelectedItem = null;
            this.CheckValide.IsChecked = false;
        }


        /////Event pour le bouton supprimer/////
        private void click_Supprimer(object sender, RoutedEventArgs args)
        {
            if (this.ListCategories.SelectedItem != null || this.ListEnAttente.SelectedItem != null)
            {

                CategorieSuppression.ServiceClient client = null;

                try
                {
                    DADEntities entitiesSupp = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                    entitiesSupp.IgnoreResourceNotFoundException = true;

                    //Manip de modification
                    client = new CategorieSuppression.ServiceClient();

                    //On récupère les infos;
                    Guid ID = categorie.id;

                    if (client.SessionIDVerification(MainWindow.GetInstance().SessionId))
                    {
                        CategorieSuppression.ModifyCategorieDataState state = client.DeleteCategorie(ID);
                        
                        
                        //Si ce n'est pas exécuté
                        if (state != CategorieSuppression.ModifyCategorieDataState.EXECUTED)
                        {

                        }
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }

            initiateFormulaire();
            this.TextNom.Text = null;
            this.ComboCategorie.SelectedItem = null;
            this.CheckValide.IsChecked = false;
        }

    }
}
