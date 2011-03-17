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
                var qry = (from c in ctx.CLIENT
                          where c.email == identifier
                          select c).Count<CLIENT>();
                result = qry > 0 ? CheckIfUserExistResult.EXIST : CheckIfUserExistResult.NOT_EXIST;
                if (result == CheckIfUserExistResult.EXIST)
                {
                    var client = (from c in ctx.CLIENT
                               where c.email == identifier
                               select c).Single<CLIENT>();
                    guid = client.id;
                }
            }
            catch (Exception)
            {
                result = CheckIfUserExistResult.DATA_ERROR;
            }
            context.SetValue(this.Exist, result);
            context.SetValue(this.UserGuid, guid);
        }
    }
}
