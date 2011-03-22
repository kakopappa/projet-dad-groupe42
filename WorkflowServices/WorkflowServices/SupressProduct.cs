using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.Service;
using WorkflowServices.DataService;

namespace WorkflowServices
{

    public sealed class SupressProduct : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<Guid> UserID { get; set; }
        public InArgument<Guid> ItemID { get; set; }
        public InArgument<UserType> Type { get; set; }
        public OutArgument<ModifyProductDataState> State { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtenir la valeur runtime de l'argument d'entrée Text
            Guid itemID = context.GetValue(this.ItemID);
            Guid userID = context.GetValue(this.UserID);
            UserType type = context.GetValue(this.Type);
            ModifyProductDataState state = ModifyProductDataState.NOT_EXECUTED;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                ctx.IgnoreResourceNotFoundException = true;

                var product = (from p in ctx.PRODUIT.Expand("FOURNISSEUR")
                               where p.id == itemID
                               select p).FirstOrDefault<PRODUIT>();
                if (product != null)
                {
                    state = ModifyProductDataState.EXIST;
                    if (type == UserType.ADMINISTRATOR ||
                        (type == UserType.FOURNISSEUR && product.FOURNISSEUR.id == userID))
                    {
                        product.supprime = true;
                        ctx.UpdateObject(product);
                        ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
                        state = ModifyProductDataState.EXECUTED;
                    }
                    else
                        state = ModifyProductDataState.SERVICE_ERROR;
                }
                else
                    state = ModifyProductDataState.NOT_EXIST;
            }
            catch
            {
                state = ModifyProductDataState.DATA_ERROR;
            }
            context.SetValue<ModifyProductDataState>(this.State, state);
        }
    }
}
