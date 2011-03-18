using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;

namespace WorkflowServices
{
    public enum ItemState
    {
        OK = 0,
        UNAVAILABLE = 1,
        UNKNOW = 2,
        INSUFFICIENT = 3,
        NOT_VERIFIED = 4
    }

    public sealed class ItemVerification : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<Guid> ItemID { get; set; }
        public InArgument<Int32> Quantity { get; set; }
        public OutArgument<ItemState> Result { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtenir la valeur runtime de l'argument d'entrée Text
            Guid itemID = context.GetValue(this.ItemID);
            Int32 quantity = context.GetValue(this.Quantity);
            ItemState state = ItemState.NOT_VERIFIED;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                ctx.IgnoreResourceNotFoundException = true;
                var qry = (from p in ctx.PRODUIT
                           where p.id == itemID
                           select p).FirstOrDefault<PRODUIT>();

                if(qry != null)
                {
                    if(qry.disponible)
                    {
                        if(qry.stock >= quantity)
                        {
                            state = ItemState.OK;
                        }
                        else
                        {
                            state = ItemState.INSUFFICIENT;
                        }
                    }
                    else
                    {
                        state = ItemState.UNAVAILABLE;
                    }
                }
                else
                {
                    state = ItemState.UNKNOW;
                }

            }
            catch(Exception)
            {
            }
            context.SetValue<ItemState>(Result, state);
        }
    }
}
