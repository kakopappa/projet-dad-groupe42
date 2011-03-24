using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace WorkflowServices
{

    public sealed class VerificationBanque : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<Guid> UserID { get; set; }
        public OutArgument<Boolean> Success { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtenir la valeur runtime de l'argument d'entrée Text
            context.SetValue(Success, true);
        }
    }
}
