using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;

namespace WorkflowServices
{

    public sealed class DeleteCategorie : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<Guid> CategorieID { get; set; }
        public OutArgument<ModifyCategorieDataState> State { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            Guid categorieID = context.GetValue(this.CategorieID);
            ModifyCategorieDataState state = ModifyCategorieDataState.NOT_EXECUTED;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                ctx.IgnoreResourceNotFoundException = true;

                var categorie = (from c in ctx.CATEGORIE.Expand("CATEGORIE2/CATEGORIE1")
                                 where c.id == categorieID
                                 select c).FirstOrDefault<CATEGORIE>();

                if (categorie != null)
                {
                    state = ModifyCategorieDataState.EXIST;
                    if (categorie.CATEGORIE2 != null)
                    {
                        categorie.CATEGORIE2.CATEGORIE1.Remove(categorie);
                        ctx.DeleteLink(categorie.CATEGORIE2, "CATEGORIE1", categorie);
                        categorie.CATEGORIE2 = null;
                        ctx.UpdateObject(categorie.CATEGORIE2);
                    }
                    ctx.DeleteObject(categorie);
                    ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
                    state = ModifyCategorieDataState.EXECUTED;
                }
                else
                {
                    state = ModifyCategorieDataState.NOT_EXIST;
                }
            }
            catch
            {
                state = ModifyCategorieDataState.DATA_ERROR;
            }
            context.SetValue<ModifyCategorieDataState>(this.State, state);
        }
    }
}
