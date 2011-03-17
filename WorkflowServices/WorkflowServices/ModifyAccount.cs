using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;

namespace WorkflowServices
{

    public enum ModifyAccountDataState
    {
        SERVICE_ERROR,
        DATA_ERROR,
        EXIST,
        NOT_EXIST,
        NOT_EXECUTED,
        EXECUTED
    }

    public sealed class ModifyAccount : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<string> Nom { get; set; }
        public InArgument<string> Prenom { get; set; }
        public InArgument<string> Phone { get; set; }
        public InArgument<Guid> UserGuid { get; set; }
        public OutArgument<ModifyAccountDataState> State { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            string nom = context.GetValue<string>(Nom);
            string prenom = context.GetValue<string>(Prenom);
            string phone = context.GetValue<string>(Phone);
            Guid guid = context.GetValue<Guid>(UserGuid);
            ModifyAccountDataState state = ModifyAccountDataState.NOT_EXECUTED;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                ctx.IgnoreResourceNotFoundException = true;

                var qry = (from c in ctx.CLIENT
                           where c.id == guid
                           select c).FirstOrDefault<CLIENT>();

                if(qry != null)
                {
                    state = ModifyAccountDataState.EXIST;
                    qry.nom = nom;
                    qry.phone = phone;
                    qry.prenom = prenom;

                    ctx.UpdateObject(qry);
                    ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
                    state = ModifyAccountDataState.EXECUTED;
                }
                else
                {
                    state = ModifyAccountDataState.NOT_EXIST;
                }
            }
            catch (Exception e)
            {
                state = ModifyAccountDataState.DATA_ERROR;
            }
            context.SetValue<ModifyAccountDataState>(State, state);
        }
    }
}
