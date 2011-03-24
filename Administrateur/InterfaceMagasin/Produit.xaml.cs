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
using System.Data.Services.Client;

namespace InterfaceMagasin
{
    /// <summary>
    /// Logique d'interaction pour Produit.xaml
    /// </summary>
    public partial class Produit : Page
    {
        //Variables
        List<PRODUIT> maListProduits;
        PRODUIT produit;

        List<CATEGORIE> maListCategoriesProduit;
        CATEGORIE categorieProduit;
        
        List<CATEGORIE> maListCategoriesExist;
        CATEGORIE categorieExist;

        //Constructeur de la classe
        public Produit()
        {
            InitializeComponent();
            initiateFormulaire();

            //Les champs ne sont pas modifiables
            this.TextDescription.IsEnabled = false;
            this.TextMarque.IsEnabled = false;
            this.TextNom.IsEnabled = false;
            this.TextPrix.IsEnabled = false;
            this.TextReference.IsEnabled = false;
            this.TextStock.IsEnabled = false;
        }

        ////////////////////////////////////////////////
        /////       méthode d'initialisation       /////
        ////////////////////////////////////////////////
        private void initiateFormulaire()
        {
            //On remplit la liste à l'ouverture
            try
            {
                DADEntities entities = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                //Par défaut, on sélectionne tout
                var query = from p in entities.PRODUIT
                            where p.supprime == false
                            orderby p.nom
                            select p;
                
                //On récupère la liste des produits
                maListProduits = query.ToList<PRODUIT>();

                //On associe la liste de produits à la Listbox                    
                this.ListProduits.ItemsSource = maListProduits;
              
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            //On vide les listes
            maListCategoriesProduit = null;
            this.ListCategoriesProduit.ItemsSource = maListCategoriesProduit;
            categorieProduit = null;

            maListCategoriesExist = null;
            this.ListCategoriesExiste.ItemsSource = maListCategoriesExist;
            categorieExist = null;

            //Boutons inactifs
            this.ButtonAjout.IsEnabled = false;
            this.ButtonEnleve.IsEnabled = false;
            this.ButtonSupprimer.IsEnabled = false;
            MainWindow.GetInstance().modifCompteur();
        }



        //////////////////////////////////////////////////////////
        /////       sélection d'un produit dans la liste     /////
        //////////////////////////////////////////////////////////

        private void listProduit_Selection(object sender, RoutedEventArgs args)
        {
            chargementInfoEtCategorie();
        }



        ///////////////////////////////////////////////////////////////
        /////  sélection d'une categorie dans la liste des dispos /////
        ///////////////////////////////////////////////////////////////

        private void listDispo_Selection(object sender, RoutedEventArgs args)
        {
            //On sélectionne la categorie
            categorieExist = (CATEGORIE)this.ListCategoriesExiste.SelectedItem;
            categorieProduit = null;

            //Le bouton de Suppression est actif
            this.ButtonSupprimer.IsEnabled = true;
            //Le bouton d'enlèvement est inactif
            this.ButtonEnleve.IsEnabled = false;
            //Le bouton d'ajout est actif
            this.ButtonAjout.IsEnabled = true;
            //Les objets de la liste d'en face sont deselectionné
            this.ListCategoriesProduit.SelectedItem = null;
        }


        ///////////////////////////////////////////////////////////////////////////////
        /////  sélection d'une categorie dans la liste des categories du produits /////
        ///////////////////////////////////////////////////////////////////////////////

        private void listCategorie_Selection(object sender, RoutedEventArgs args)
        {
            //On sélectionne la categorie
            categorieProduit = (CATEGORIE)this.ListCategoriesProduit.SelectedItem;
            categorieExist = null;

            //Le bouton de Suppression est actif
            this.ButtonSupprimer.IsEnabled = true;
            //Le bouton d'enlèvement est actif
            this.ButtonEnleve.IsEnabled = true;
            //Le bouton d'ajout est inactif
            this.ButtonAjout.IsEnabled = false;
            //Les objets de la liste d'en face sont deselectionné
            this.ListCategoriesExiste.SelectedItem = null;
            
        }

        /////////////////////////////////////////////////////////
        /////Event pour l'ajout d'une categorie à un produit/////
        /////////////////////////////////////////////////////////

        private void ajoutCategorie(object sender, RoutedEventArgs args)
        {
            if (categorieExist != null)
            {
                ProductModification.ServiceClient client = null;

                try
                {
                    DADEntities entitiesmModif = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                    entitiesmModif.IgnoreResourceNotFoundException = true;

                    //Manip de modification
                    client = new ProductModification.ServiceClient();

                    if (client.SessionIDVerification(MainWindow.GetInstance().SessionId))
                    {
                        //On ajoute la categorie sélectionnée à la liste des catégories du produit
                        maListCategoriesProduit.Add(categorieExist);

                        //On récupère les id des catégories
                        var idCat = from l in maListCategoriesProduit
                                    select l.id;

                        //On place le tout dans un tableau
                        Guid[] tabId = idCat.ToArray<Guid>();

                        ProductModification.ModifyProductDataState state = client.ModifyProductData("", "", "", "", 0, 0, produit.id, produit.disponible, tabId);

                        //Mise à jour du produit
                        var monproduit = (from p in entitiesmModif.PRODUIT
                                         where p.id == produit.id
                                         select p).Single<PRODUIT>();

                        produit = monproduit;

                        //Si ce n'est pas exécuté
                        if (state != ProductModification.ModifyProductDataState.EXECUTED)
                        {

                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            chargementInfoEtCategorie();
        }


        //////////////////////////////////////////////////////////////
        /////Event pour la modif de la disponibilité d'un produit/////
        //////////////////////////////////////////////////////////////

        private void modifDispo(object sender, RoutedEventArgs args)
        {
            ProductModification.ServiceClient client = null;

           try
           {
               DADEntities entitiesmModif = new DADEntities(new Uri(Properties.Settings.Default.DataService));
               entitiesmModif.IgnoreResourceNotFoundException = true;

               //Manip de modification
               client = new ProductModification.ServiceClient();

               if (client.SessionIDVerification(MainWindow.GetInstance().SessionId))
               {
                   //On récupère les id des catégories
                   var idCat = from l in maListCategoriesProduit
                               select l.id;

                   //On place le tout dans un tableau
                   Guid[] tabId = idCat.ToArray<Guid>();

                   //On récupère la valeur du check
                   bool dispo;
                   
                   if(this.CheckDispo.IsChecked == true)
                   {
                       dispo = true;
                   }
                   else
                   {
                       dispo = false;
                   }

                   ProductModification.ModifyProductDataState state = client.ModifyProductData("", "", "", "", 0, 0, produit.id, dispo, tabId);

                   //Mise a jour de la liste de produit
                   var mesproduit = from p in entitiesmModif.PRODUIT
                                    where p.supprime == false
                                    orderby p.nom
                                    select p;

                    //On récupère la liste des produits
                    maListProduits = mesproduit.ToList<PRODUIT>();
                    //On associe la liste de produits à la Listbox                    
                    this.ListProduits.ItemsSource = maListProduits;
                   
                   //Si ce n'est pas exécuté
                   if (state != ProductModification.ModifyProductDataState.EXECUTED)
                   {

                   }
               }
           }

           catch (Exception e)
           {
               Console.WriteLine(e);
           }

        }

        ////////////////////////////////////////////////////
        ///// Chargement des infos et des categories  //////
        ////////////////////////////////////////////////////

        private void chargementInfoEtCategorie()
        {
            //On sélectionne le produit
            produit = (PRODUIT)this.ListProduits.SelectedItem;

            try
            {
                DADEntities entitiesCategorie = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                entitiesCategorie.IgnoreResourceNotFoundException = true;

                //On sélectionne les categories de ce produit
                var selectCatProd = (from p in entitiesCategorie.PRODUIT.Expand("CATEGORIE")
                                     where p.id == produit.id
                                     select p).FirstOrDefault();

                //On  génère la liste des categories du produit
                maListCategoriesProduit = selectCatProd.CATEGORIE.ToList<CATEGORIE>();

                //On sélectionne toutes les categories
                var selectCatExist = from cat in entitiesCategorie.CATEGORIE
                                     select cat;

                //On  génère la liste des categories non attachées au produit
                maListCategoriesExist = selectCatExist.ToList<CATEGORIE>();

                //On créer une nouvelle liste
                List<CATEGORIE> listDifference = new List<CATEGORIE>();

                foreach (CATEGORIE categorie in maListCategoriesExist)
                {
                    if (!maListCategoriesProduit.Exists(delegate(CATEGORIE catProd) { return catProd.id == categorie.id; }))
                    {
                        listDifference.Add(categorie);
                    }
                }

                //On associe la liste des categories existantes à la Listbox                    
                this.ListCategoriesExiste.ItemsSource = listDifference;
                //On associe la liste des categories du produit à la Listbox                    
                this.ListCategoriesProduit.ItemsSource = maListCategoriesProduit;
                //Si c'est Disponible, on check la box
                if (produit.disponible == true)
                {
                    this.CheckDispo.IsChecked = true;
                }
                else
                {
                    this.CheckDispo.IsChecked = false;
                }

            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            //Le bouton de Suppression est actif
            this.ButtonSupprimer.IsEnabled = true;
        }


        ///////////////////////////////////////////////
        ////Event pour la supression d'une categorie///
        ///////////////////////////////////////////////

        private void enleveCategorie(object sender, RoutedEventArgs args)
        {
            if (categorieProduit != null)
            {
                ProductModification.ServiceClient client = null;

                try
                {
                    DADEntities entitiesmModif = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                    entitiesmModif.IgnoreResourceNotFoundException = true;

                    //Manip de modification
                    client = new ProductModification.ServiceClient();

                    if (client.SessionIDVerification(MainWindow.GetInstance().SessionId))
                    {
                        List<CATEGORIE> maListTemporaire = new List<CATEGORIE>(maListCategoriesProduit);                        
                        
                        //On supprime la categorie sélectionnée à la liste des catégories du produit
                        foreach (CATEGORIE cat in maListTemporaire)
                        {
                            if (cat.id == categorieProduit.id)
                            {
                                maListCategoriesProduit.Remove(cat);
                            }
                        }


                        //On récupère les id des catégories
                        var idCat = from l in maListCategoriesProduit
                                    select l.id;

                        //On place le tout dans un tableau
                        Guid[] tabId = idCat.ToArray<Guid>();

                        ProductModification.ModifyProductDataState state = client.ModifyProductData("", "", "", "", 0, 0, produit.id, produit.disponible, tabId);

                        //Mise à jour du produit
                        var monproduit = (from p in entitiesmModif.PRODUIT
                                          where p.id == produit.id
                                          select p).Single<PRODUIT>();

                        produit = monproduit;
                        
                        //Si ce n'est pas exécuté
                        if (state != ProductModification.ModifyProductDataState.EXECUTED)
                        {

                        }
                    }
                }

                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            chargementInfoEtCategorie();
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
                var query = from p in entities.PRODUIT
                            where p.supprime == false
                            orderby p.nom
                            select p;

                //Si le champ selectionner est rempli
                if (selection != "")
                {
                    query = from p in entities.PRODUIT
                            where p.nom == selection && p.supprime == false
                            orderby p.nom
                            select p;
                }

                //On récupère la liste des produits
                maListProduits = query.ToList<PRODUIT>();
                //On associe la liste de produits à la Listbox                    
                this.ListProduits.ItemsSource = maListProduits;

                //On associe la liste des categories existantes à la Listbox                    
                this.ListCategoriesExiste.ItemsSource = null;
                //On associe la liste des categories du produit à la Listbox                    
                this.ListCategoriesProduit.ItemsSource = null;

                //Boutons inactifs
                this.ButtonAjout.IsEnabled = false;
                this.ButtonEnleve.IsEnabled = false;
                this.ButtonSupprimer.IsEnabled = false;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }



        ///////////////////////////////////////////////
        ////Event pour la supression d'un produit   ///
        ///////////////////////////////////////////////

        private void btnSupprimer_Click(object sender, RoutedEventArgs args)
        {
            ProductSuppresion.ServiceClient client = null;

            try
            {
                DADEntities entitiesSupp = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                entitiesSupp.IgnoreResourceNotFoundException = true;

                //Manip de modification
                client = new ProductSuppresion.ServiceClient();

                if (client.SessionIDVerification(MainWindow.GetInstance().SessionId))
                {
                    ProductSuppresion.ModifyProductDataState state = client.SuppressProduct(produit.id);

                    //Si ce n'est pas exécuté
                    if (state != ProductSuppresion.ModifyProductDataState.EXECUTED)
                    {

                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            initiateFormulaire();
        }


    }
}
