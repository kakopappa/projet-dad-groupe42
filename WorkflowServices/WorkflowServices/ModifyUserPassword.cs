using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;

namespace WorkflowServices
{
    public enum ModifyPasswordState
    {
        SERVICE_ERROR,
        DATA_ERROR,
        EXIST,
        NOT_EXIST,
        NOT_EXECUTED,
        EXECUTED
    }

    public sealed class ModifyUserPassword : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<string> Password { get; set; }
        public InArgument<Guid> UserGuid { get; set; }
        public OutArgument<ModifyPasswordState> State { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtenir la valeur runtime de l'argument d'entrée Text
            string password = context.GetValue(this.Password);
            Guid guid = context.GetValue(this.UserGuid);
            ModifyPasswordState state = ModifyPasswordState.NOT_EXECUTED;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                ctx.IgnoreResourceNotFoundException = true;

                var qry = (from c in ctx.CLIENT
                           where c.id == guid
                           select c).FirstOrDefault<CLIENT>();

                if (qry != null)
                {
                    state = ModifyPasswordState.EXIST;
                    qry.password = password;

                    ctx.UpdateObject(qry);
                    ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
                    state = ModifyPasswordState.EXECUTED;
                }
                else
                {
                    state = ModifyPasswordState.NOT_EXIST;
                }
            }
            catch (Exception)
            {
                state = ModifyPasswordState.DATA_ERROR;
            }
            context.SetValue<ModifyPasswordState>(State, state);
        }
    }
}
