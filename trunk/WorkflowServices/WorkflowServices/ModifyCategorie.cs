using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;

namespace WorkflowServices
{
    public enum ModifyCategorieDataState
    {
        SERVICE_ERROR,
        DATA_ERROR,
        EXIST,
        NOT_EXIST,
        NOT_EXECUTED,
        EXECUTED
    }

    public sealed class ModifyCategorie : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<Guid> ParentID { get; set; }
        public InArgument<String> Name { get; set; }
        public InArgument<Boolean> Disponible { get; set; }
        public InArgument<Guid> CategorieID { get; set; }
        public OutArgument<ModifyCategorieDataState> State { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            Guid parentID = context.GetValue<Guid>(this.ParentID);
            String name = context.GetValue<String>(this.Name);
            Boolean disponible = context.GetValue<Boolean>(this.Disponible);
            Guid categorieID = context.GetValue<Guid>(this.CategorieID);
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
                    categorie.nom = name;
                    categorie.valide = disponible;

                    var nextParent = (from c in ctx.CATEGORIE.Expand("CATEGORIE1")
                                      where c.id == parentID
                                      select c).FirstOrDefault<CATEGORIE>();

                    var currentParent = categorie.CATEGORIE2;
                    
                    if ((nextParent != null && currentParent == null) || (nextParent == null && currentParent != null) 
                        || (currentParent != null && nextParent != null && currentParent.id != nextParent.id))
                    {
                        if (currentParent != null)
                        {
                            categorie.CATEGORIE2 = null;
                            currentParent.CATEGORIE1.Remove(categorie);
                            ctx.DeleteLink(currentParent, "CATEGORIE1", categorie);
                            ctx.UpdateObject(currentParent);
                        }
                        if (nextParent != null)
                        {
                            categorie.CATEGORIE2 = nextParent;
                            nextParent.CATEGORIE1.Add(categorie);
                            ctx.AddLink(nextParent, "CATEGORIE1", categorie);
                            ctx.UpdateObject(nextParent);
                        }
                    }
                    ctx.UpdateObject(categorie);
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
