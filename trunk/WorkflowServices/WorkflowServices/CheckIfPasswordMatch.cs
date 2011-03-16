using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;

namespace WorkflowServices
{
    public enum CheckIfPasswordMatchResult
    {
        SERVICE_ERROR,
        DATA_ERROR,
        MATCH,
        NOT_MATCH,
        NOT_TESTED
    }

    public sealed class CheckIfPasswordMatch : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<string> Password { get; set; }
        public InArgument<Guid> UserGuid { get; set; }
        public OutArgument<CheckIfPasswordMatchResult> Match { set; get; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtenir la valeur runtime de l'argument d'entrée Text
            string password = context.GetValue(this.Password);
            Guid guid = context.GetValue(this.UserGuid);
            CheckIfPasswordMatchResult result = CheckIfPasswordMatchResult.NOT_TESTED;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                var user = (from c in ctx.CLIENT
                            where c.id == guid
                            select c).First();
                result = user.password == password ? CheckIfPasswordMatchResult.MATCH : CheckIfPasswordMatchResult.NOT_MATCH;
            }
            catch (Exception e)
            {
                result = CheckIfPasswordMatchResult.DATA_ERROR;
            }
            context.SetValue(this.Match, result);
        }
    }
}
