using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;

namespace WorkflowServices
{

    public sealed class CreateFournisseur : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<string> Nom { get; set; }
        public InArgument<string> Email { get; set; }
        public InArgument<string> Password { get; set; }
        public InArgument<string> Phone { get; set; }
        public InArgument<string> Ville { get; set; }
        public InArgument<string> Pays { get; set; }
        public InArgument<string> Adresse { get; set; }
        public InArgument<string> CodePostal { get; set; }
        public OutArgument<Guid> UserGuid { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            string nom = context.GetValue<string>(Nom);
            string email = context.GetValue<string>(Email);
            string password = context.GetValue<string>(Password);
            string phone = context.GetValue<string>(Phone);
            string adresse = context.GetValue<string>(Adresse);
            string ville = context.GetValue<string>(Ville);
            string codePostal = context.GetValue<string>(CodePostal);
            string pays = context.GetValue<string>(Pays);
            Guid guid = Guid.NewGuid();
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                FOURNISSEUR fournisseur = FOURNISSEUR.CreateFOURNISSEUR(guid, nom, email, phone, password, adresse, ville, codePostal, pays);
                ctx.AddToFOURNISSEUR(fournisseur);
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
