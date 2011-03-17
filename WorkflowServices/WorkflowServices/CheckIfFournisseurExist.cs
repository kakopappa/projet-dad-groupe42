using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;

namespace WorkflowServices
{
    public enum CheckIfFournisseurExistResult
    {
        SERVICE_ERROR,
        DATA_ERROR,
        EXIST,
        NOT_EXIST,
        NOT_TESTED
    }

    public sealed class CheckIfFournisseurExist : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<string> Identifier { set; get; }
        public OutArgument<Guid> UserGuid { set; get; }
        public OutArgument<CheckIfFournisseurExistResult> Exist { set; get; }
        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtenir la valeur runtime de l'argument d'entrée Text
            string identifier = context.GetValue(this.Identifier);
            CheckIfFournisseurExistResult result = CheckIfFournisseurExistResult.NOT_TESTED;
            Guid guid = Guid.Empty;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                var qry = (from f in ctx.FOURNISSEUR
                           where f.email == identifier
                           select f).Count<FOURNISSEUR>();
                result = qry > 0 ? CheckIfFournisseurExistResult.EXIST : CheckIfFournisseurExistResult.NOT_EXIST;
                if (result == CheckIfFournisseurExistResult.EXIST)
                {
                    var fournisseur = (from f in ctx.FOURNISSEUR
                                  where f.email == identifier
                                  select f).Single<FOURNISSEUR>();
                    guid = fournisseur.id;
                }
            }
            catch (Exception)
            {
                result = CheckIfFournisseurExistResult.DATA_ERROR;
            }
            context.SetValue(this.Exist, result);
            context.SetValue(this.UserGuid, guid);
        }
    }
}
