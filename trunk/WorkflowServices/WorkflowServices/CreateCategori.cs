using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;
using WorkflowServices.Service;

namespace WorkflowServices
{
    public enum CreateCategorieState
    {
        SERVICE_ERROR,
        DATA_ERROR,
        NOT_EXECUTED,
        EXECUTED
    }

    public sealed class CreateCategori : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<Guid> ParentID { get; set; }
        public InArgument<String> Name { get; set; }
        public InArgument<Boolean> Disponible { get; set; }
        public InArgument<UserType> Type { get; set; }
        public OutArgument<CreateCategorieState> State { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            Guid parentID = context.GetValue<Guid>(this.ParentID);
            String name = context.GetValue<String>(this.Name);
            Boolean disponible = context.GetValue<Boolean>(this.Disponible);
            UserType type = context.GetValue<UserType>(this.Type);
            CreateCategorieState state = CreateCategorieState.NOT_EXECUTED;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                ctx.IgnoreResourceNotFoundException = true;
                CATEGORIE categorie = CATEGORIE.CreateCATEGORIE(Guid.NewGuid(), name, false);

                if (type == UserType.ADMINISTRATOR)
                {
                    categorie.valide = disponible;
                }

                var qry = (from cat in ctx.CATEGORIE.Expand("CATEGORIE1")
                          where cat.id == parentID
                          select cat).FirstOrDefault<CATEGORIE>();

                if (qry != null)
                {
                    ctx.AddRelatedObject(qry, "CATEGORIE1", categorie);
                    categorie.CATEGORIE2 = qry;
                    qry.CATEGORIE1.Add(categorie);
                }
                else
                {
                    ctx.AddToCATEGORIE(categorie);
                }

                ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
                state = CreateCategorieState.EXECUTED;
            }
            catch
            {
                state = CreateCategorieState.DATA_ERROR;
            }
            context.SetValue<CreateCategorieState>(this.State, state);
        }
    }
}
