using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;

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
        public OutArgument<CreateCategorieState> State { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            Guid parentID = context.GetValue<Guid>(this.ParentID);
            String name = context.GetValue<String>(this.Name);
            CreateCategorieState state = CreateCategorieState.NOT_EXECUTED;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
            }
            catch
            {
            }
        }
    }
}
