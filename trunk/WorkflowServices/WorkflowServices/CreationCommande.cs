using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.Service;
using WorkflowServices.DataService;

namespace WorkflowServices
{

    public sealed class CreationCommande : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<Guid> UserID { get; set; }
        public InArgument<Guid[]> FournisseursID { get; set; }
        public InArgument<CartEntrie[]> CartEntries { get; set; }
        public OutArgument<Boolean> CommandeOK { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtenir la valeur runtime de l'argument d'entrée Text
            Guid userID = context.GetValue(this.UserID);
            Guid[] fournisseursID = context.GetValue(this.FournisseursID);
            CartEntrie[] cartEntries = context.GetValue(this.CartEntries);

            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                ctx.IgnoreResourceNotFoundException = true;
                DateTime date = DateTime.Now;

                var client = (from c in ctx.CLIENT.Expand("ADRESSE_CLIENT")
                              where c.id == userID
                              select c).FirstOrDefault();

                COMMANDE_CLIENT commande = COMMANDE_CLIENT.CreateCOMMANDE_CLIENT(Guid.NewGuid(), date, decimal.Zero);
                commande.CLIENT = client;
                commande.ADRESSE_CLIENT = client.ADRESSE_CLIENT[0];
                ctx.AddRelatedObject(client, "COMMANDE_CLIENT", commande);
                ctx.AddLink(client.ADRESSE_CLIENT[0], "COMMANDE_CLIENT", commande);

                List<PRODUIT> produits = new List<PRODUIT>();
                foreach (CartEntrie item in cartEntries)
                {
                    var qry = (from p in ctx.PRODUIT.Expand("FOURNISSEUR")
                               where p.id == item.ObjectID
                               select p).FirstOrDefault();
                    if(qry != null)
                        produits.Add(qry);
                }

                List<FOURNISSEUR> fournisseurs = new List<FOURNISSEUR>();
                foreach (Guid fournisseurID in fournisseursID)
                {
                    var qry = (from f in ctx.FOURNISSEUR
                               where f.id == fournisseurID
                               select f).FirstOrDefault();
                    if (qry != null)
                        fournisseurs.Add(qry);
                }

                List<COMMANDE_FOURNISSEUR> commandeFournisseurs = new List<COMMANDE_FOURNISSEUR>();
                List<COMMANDER> commanders = new List<COMMANDER>();
                foreach (FOURNISSEUR fournisseur in fournisseurs)
                {
                    COMMANDE_FOURNISSEUR commandeFournisseur = COMMANDE_FOURNISSEUR.CreateCOMMANDE_FOURNISSEUR(Guid.NewGuid(), date, decimal.Zero);
                    commandeFournisseur.FOURNISSEUR = fournisseur;
                    ctx.AddRelatedObject(commande, "COMMANDE_FOURNISSEUR", commandeFournisseur);
                    ctx.AddLink(fournisseur, "COMMANDE_FOURNISSEUR", commandeFournisseur);

                    var qry = (from PRODUIT p in produits
                              where p.FOURNISSEUR.id == fournisseur.id
                              select p).ToList<PRODUIT>();


                    foreach (PRODUIT product in qry)
                    {
                        var quantity = (from CartEntrie c in cartEntries
                                       where c.ObjectID == product.id
                                       select c.Quantity).FirstOrDefault();

                        COMMANDER commander = COMMANDER.CreateCOMMANDER(fournisseur.id, product.id, quantity);
                        commander.COMMANDE_FOURNISSEUR = commandeFournisseur;
                        commander.PRODUIT = product;
                        product.stock -= commander.quantite;
                        ctx.UpdateObject(product);
                        ctx.AddRelatedObject(commandeFournisseur, "COMMANDER", commander);
                        ctx.AddLink(product, "COMMANDER", commander);
                        commanders.Add(commander);
                    }

                    commandeFournisseur.prix_total = (from COMMANDER c in commanders
                                                      where c.COMMANDE_FOURNISSEUR == commandeFournisseur
                                                      select (c.quantite * c.PRODUIT.prix)).Sum();

                    commandeFournisseurs.Add(commandeFournisseur);
                }

                commande.prix_total = (from COMMANDE_FOURNISSEUR c in commandeFournisseurs
                                       select c.prix_total).Sum();

                ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
                context.SetValue(CommandeOK, true);
            }
            catch
            {
                context.SetValue(CommandeOK, false);
            }
        }
    }
}
