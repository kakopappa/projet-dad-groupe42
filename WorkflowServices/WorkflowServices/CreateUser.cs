using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;

namespace WorkflowServices
{

    public sealed class CreateUser : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<string> Nom { get; set; }
        public InArgument<string> Prenom { get; set; }
        public InArgument<string> Email { get; set; }
        public InArgument<string> Password { get; set; }
        public InArgument<string> Phone { get; set; }
        public OutArgument<Guid> UserGuid { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            string nom = context.GetValue<string>(Nom);
            string prenom = context.GetValue<string>(Prenom);
            string email = context.GetValue<string>(Email);
            string password = context.GetValue<string>(Password);
            string phone = context.GetValue<string>(Phone);
            Guid guid = Guid.NewGuid();
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                CLIENT client = CLIENT.CreateCLIENT(guid, nom, prenom, phone, email, password);
                ctx.AddToCLIENT(client);
                ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
            }
            catch (Exception)
            {
                guid = Guid.Empty;
            }
            context.SetValue<Guid>(UserGuid, guid);
        }
    }
}
