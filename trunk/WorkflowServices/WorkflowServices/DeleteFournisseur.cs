using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;

namespace WorkflowServices
{

    public sealed class DeleteFournisseur : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<Guid> FournisseurID { get; set; }
        public OutArgument<ModifyFournisseurDataState> State { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtenir la valeur runtime de l'argument d'entrée Text
            Guid fournisseurID = context.GetValue(this.FournisseurID);
            ModifyFournisseurDataState state = ModifyFournisseurDataState.NOT_EXECUTED;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                ctx.IgnoreResourceNotFoundException = true;

                var fournisseur = (from f in ctx.FOURNISSEUR
                               where f.id == fournisseurID
                               select f).FirstOrDefault<FOURNISSEUR>();
                if (fournisseur != null)
                {
                    state = ModifyFournisseurDataState.EXIST;
                    fournisseur.supprime = true;
                    ctx.UpdateObject(fournisseur);
                    ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
                    state = ModifyFournisseurDataState.EXECUTED;
                }
                else
                    state = ModifyFournisseurDataState.NOT_EXIST;
            }
            catch
            {
                state = ModifyFournisseurDataState.DATA_ERROR;
            }
            context.SetValue<ModifyFournisseurDataState>(this.State, state);
        }
    }
}
