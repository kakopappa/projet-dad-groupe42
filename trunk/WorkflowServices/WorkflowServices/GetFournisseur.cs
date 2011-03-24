using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.Service;
using WorkflowServices.DataService;

namespace WorkflowServices
{

    public sealed class GetFournisseur : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<CartEntrie[]> CartEntries { get; set; }
        public OutArgument<Guid[]> FournisseursID { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtenir la valeur runtime de l'argument d'entrée Text
            CartEntrie[] cartEntries = context.GetValue(this.CartEntries);
            List<Guid> fournisseursID = new List<Guid>();
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                ctx.IgnoreResourceNotFoundException = true;

                List<PRODUIT> produits = new List<PRODUIT>();
                foreach (CartEntrie item in cartEntries)
                {
                    var qry1 = from p in ctx.PRODUIT.Expand("FOURNISSEUR")
                               where p.id == item.ObjectID
                               select p;

                    var qry2 = qry1.FirstOrDefault();
                    if(qry2 != null)
                        produits.Add(qry2);
                }

                fournisseursID = (from PRODUIT p in produits
                                  select p.FOURNISSEUR.id).Distinct().ToList<Guid>();
            }
            catch
            {
            }

            context.SetValue(FournisseursID, fournisseursID.ToArray());
        }
    }
}
