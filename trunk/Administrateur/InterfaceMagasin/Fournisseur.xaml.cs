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
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace InterfaceMagasin
{
    /// <summary>
    /// Logique d'interaction pour Fournisseur.xaml
    /// </summary>
    public partial class Fournisseur : Page
    {

        //Variables
        List<FOURNISSEUR> maListFournisseurs;
        FOURNISSEUR fournisseur;

        //Constructeur du fournisseur
        public Fournisseur()
        {
            InitializeComponent();
            initiateFormulaire();
        }

        private void initiateFormulaire()
        {
            //On remplit la liste à l'ouverture
            try
            {
                DADEntities entities = new DADEntities(new Uri(Properties.Settings.Default.DataService));

                //Par défaut, on sélectionne tout
                var query = from f in entities.FOURNISSEUR
                            where f.supprime == false
                            orderby f.nom
                            select f;

                //On récupère la liste des fournisseurs
                maListFournisseurs = query.ToList<FOURNISSEUR>();

                //On associe la liste de fournisseurs à la Listbox                    
                this.ListFournisseurs.ItemsSource = maListFournisseurs;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            //Le bouton de modification n'est pas actif
            this.ButtonModif.IsEnabled = false;
            //Le bouton de Suppression n'est pas actif
            this.ButtonSupprimer.IsEnabled = false;
            //On active le password
            this.TextPassword.IsEnabled = true;
            //L'erreur est cachééééeee
            this.LabelErreur.Visibility = Visibility.Hidden;
            //Email invalide
            this.LabelEmail.Visibility = Visibility.Hidden;
        
        }

        static string HashPassword(string email, string password)
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            SHA1 sha1;

            sha1 = new SHA1CryptoServiceProvider();
            originalBytes = Encoding.Unicode.GetBytes(email + "alligator21" + password);
            encodedBytes = sha1.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes).Replace("-", String.Empty);
        }


        /////Event pour le bouton Creer/////
        private void btnCreer_Click(object sender, RoutedEventArgs args)
        {
            //On récupère les champs texte
            string nom, mail, phone, adresse, ville, cp, pays, password;

            nom = this.TextNom.Text;
            mail = this.TextMail.Text;
            phone = this.TextPhone.Text;
            adresse = this.TextAdresse.Text;
            ville = this.TextVille.Text;
            cp = this.TextCP.Text;
            pays = this.TextPays.Text;
            password = HashPassword(mail,this.TextPassword.Password);

            //Si un champs est vide, on lance pas la création
            if (nom == "" || mail == "" || phone == "" || adresse == "" || ville == "" || cp == "" || pays == "" || password == "")
            {
                //L'erreur est visible !!
                this.LabelErreur.Visibility = Visibility.Visible;
                //Email invalide
                this.LabelEmail.Visibility = Visibility.Hidden;
            }

            else
            {
                if (isEmail(mail) == true)
                {
                    //On prépare la création du fournisseur
                    FournisseurCreation.ServiceClient client = null;

                    try
                    {
                        //On ajoute le fournisseur à la BDD                
                        client = new FournisseurCreation.ServiceClient();
                        if (client.SessionIDVerification(MainWindow.GetInstance().SessionId))
                        {
                            FournisseurCreation.CheckIfFournisseurExistResult result = client.CreateFournisseur(nom, mail,
                                password, adresse, ville, phone, cp, pays);

                            //Si Le client n'existe pas
                            if (result == FournisseurCreation.CheckIfFournisseurExistResult.NOT_EXIST)
                            {
                                Guid id = (Guid)client.GuidRequest();
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    //On ferme le client
                    finally
                    {
                        if (client != null)
                        {
                            client.Close();
                            client.ChannelFactory.Close();
                        }
                    }

                    //On réinitialise le formulaire
                    initiateFormulaire();
                }

                else
                {
                    //Email invalide
                    this.LabelEmail.Visibility = Visibility.Visible;
                }
            }
        }



        /////Event pour le bouton Selectionner/////
        private void btnSelectionner_Click(object sender, RoutedEventArgs args)
        {
            string selection = this.TextSelection.Text;

            try
            {
                DADEntities entities = new DADEntities(new Uri(Properties.Settings.Default.DataService));

                //Par défaut, on sélectionne tout
                var query = from f in entities.FOURNISSEUR
                            where f.supprime == false
                            orderby f.nom
                            select f;

                //Si le champ selectionner est rempli
                if (selection != "")
                {
                    query = from f in entities.FOURNISSEUR
                            where f.nom == selection && f.supprime == false
                            orderby f.nom
                            select f;
                }

                //On récupère la liste des fournisseurs
                maListFournisseurs = query.ToList<FOURNISSEUR>();
                //Pour chaque fournisseur, on ajoute son nom dans la liste                    
                this.ListFournisseurs.ItemsSource = maListFournisseurs;
                //On désactive la modif
                this.ButtonModif.IsEnabled = false;
                //Le bouton de Suppression n'est pas actif
                this.ButtonSupprimer.IsEnabled = false;
                //On active la création
                this.ButtonCreer.IsEnabled = true;
                //On active le password
                this.TextPassword.IsEnabled = true;
                //L'erreur est cachééééeee
                this.LabelErreur.Visibility = Visibility.Hidden;
                //Email invalide
                this.LabelEmail.Visibility = Visibility.Hidden;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        /////Event pour la sélection d'un fournisseur dans la liste/////
        private void listFournisseur_Selection(object sender, RoutedEventArgs args)
        {
            //On sélectionne le fournisseur
            fournisseur = (FOURNISSEUR)this.ListFournisseurs.SelectedItem;

            //On désactive le password
            this.TextPassword.IsEnabled = false;
            //On désactive le Bouton de création
            this.ButtonCreer.IsEnabled = false;
            //On active le Bouton de modification
            this.ButtonModif.IsEnabled = true;
            //Le bouton de Suppression est actif
            this.ButtonSupprimer.IsEnabled = true;
            //L'erreur est cachééééeee
            this.LabelErreur.Visibility = Visibility.Hidden;
            //Email invalide
            this.LabelEmail.Visibility = Visibility.Hidden;
        }


        /////Event pour vider les champs texte/////
        private void btnNouveau_Click(object sender, RoutedEventArgs args)
        {
            this.ListFournisseurs.SelectedItem = null;
            //On désactive la modif
            this.ButtonModif.IsEnabled = false;
            //On active la création
            this.ButtonCreer.IsEnabled = true;
            //Le bouton de Suppression n'est pas actif
            this.ButtonSupprimer.IsEnabled = false;
            //On active le password
            this.TextPassword.IsEnabled = true;
            //L'erreur est cachééééeee
            this.LabelErreur.Visibility = Visibility.Hidden;
            //Email invalide
            this.LabelEmail.Visibility = Visibility.Hidden;
        }


        /////Event pour le bouton Modifier/////
        private void btnModifier_Click(object sender, RoutedEventArgs args)
        {
            if (this.ListFournisseurs.SelectedItem != null)
            {
                FournisseurModification.ServiceClient client = null;

                //On change les valeurs du fournisseur
                fournisseur.nom = this.TextNom.Text;
                fournisseur.email = this.TextMail.Text;
                fournisseur.phone = this.TextPhone.Text;
                fournisseur.adresse = this.TextAdresse.Text;
                fournisseur.ville = this.TextVille.Text;
                fournisseur.code_postal = this.TextCP.Text;
                fournisseur.pays = this.TextPays.Text;

                //Si un champs est vide, on lance pas la création
                if (fournisseur.nom == "" || fournisseur.email == "" || fournisseur.phone == "" || fournisseur.adresse == "" || fournisseur.ville == "" || fournisseur.code_postal == "" || fournisseur.pays == "")
                {
                    //L'erreur est visible !!
                    this.LabelErreur.Visibility = Visibility.Visible;
                }

                else
                {
                    if (isEmail(fournisseur.email) == true)
                    {

                        try
                        {
                            //Manip de modification
                            client = new FournisseurModification.ServiceClient();

                            if (client.SessionIDVerification(MainWindow.GetInstance().SessionId))
                            {
                                FournisseurModification.ModifyFournisseurDataState state = client.ModifyFournisseurData(fournisseur.nom,
                                    fournisseur.phone, fournisseur.adresse,
                                    fournisseur.ville, fournisseur.code_postal,
                                    fournisseur.pays, fournisseur.id);

                                //Si ce n'est pas exécuté
                                if (state != FournisseurModification.ModifyFournisseurDataState.EXECUTED)
                                {

                                }
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                        //On ferme le client
                        finally
                        {
                            if (client != null)
                            {
                                client.Close();
                                client.ChannelFactory.Close();
                            }
                        }
                    }

                    //Si email non valide
                    else
                    {
                        //L'erreur est cachéée !!
                        this.LabelErreur.Visibility = Visibility.Hidden;
                        //Email invalide
                        this.LabelEmail.Visibility = Visibility.Visible;
                    }
                }
            }
        }


        /////Event pour le bouton Supprimer/////
        private void btnSupprimer_Click(object sender, RoutedEventArgs args)
        {
            if (this.ListFournisseurs.SelectedItem != null)
            {

                FournisseurSuppression.ServiceClient client = null;

                try
                {
                    DADEntities entitiesSupp = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                    entitiesSupp.IgnoreResourceNotFoundException = true;

                    //Manip de modification
                    client = new FournisseurSuppression.ServiceClient();

                    if (client.SessionIDVerification(MainWindow.GetInstance().SessionId))
                    {
                        FournisseurSuppression.ModifyFournisseurDataState state = client.DeleteFournisseur(fournisseur.id);

                        //Si ce n'est pas exécuté
                        if (state != FournisseurSuppression.ModifyFournisseurDataState.EXECUTED)
                        {

                        }
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }

            //On réinitialise le formulaire
            initiateFormulaire();
        }


        public static bool isEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

    }
}
