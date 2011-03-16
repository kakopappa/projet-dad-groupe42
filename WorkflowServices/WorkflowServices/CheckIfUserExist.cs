using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;

namespace WorkflowServices
{
    public enum CheckIfUserExistResult
    {
        SERVICE_ERROR,
        DATA_ERROR,
        EXIST,
        NOT_EXIST,
        NOT_TESTED
    }

    public sealed class CheckIfUserExist : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<string> Identifier { set; get; }
        public OutArgument<Guid> UserGuid { set; get; }
        public OutArgument<CheckIfUserExistResult> Exist { set; get; }
        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtenir la valeur runtime de l'argument d'entrée Text
            string identifier = context.GetValue(this.Identifier);
            CheckIfUserExistResult result = CheckIfUserExistResult.NOT_TESTED;
            Guid guid = Guid.Empty;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                var qry = from c in ctx.CLIENT
                          where c.email == identifier
                          select c;
                result = qry.ToList<CLIENT>().Count > 0 ? CheckIfUserExistResult.EXIST : CheckIfUserExistResult.NOT_EXIST;
                guid = qry.ToList<CLIENT>()[0].id;
            }
            catch (Exception e)
            {
                result = CheckIfUserExistResult.DATA_ERROR;
            }
            context.SetValue(this.Exist, result);
            context.SetValue(this.UserGuid, guid);
        }
    }
}
