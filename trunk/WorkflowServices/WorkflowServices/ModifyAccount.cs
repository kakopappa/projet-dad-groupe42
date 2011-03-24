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
        public InArgument<string> Adresse { get; set; }
        public InArgument<string> Ville { get; set; }
        public InArgument<string> Pays { get; set; }
        public InArgument<string> CodePostal { get; set; }
        public InArgument<Guid> UserGuid { get; set; }
        public OutArgument<ModifyAccountDataState> State { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            string nom = context.GetValue<string>(Nom);
            string prenom = context.GetValue<string>(Prenom);
            string phone = context.GetValue<string>(Phone);
            string adresse = context.GetValue<string>(Adresse);
            string ville = context.GetValue<string>(Ville);
            string pays = context.GetValue<string>(Pays);
            string codePostal = context.GetValue<string>(CodePostal);
            Guid guid = context.GetValue<Guid>(UserGuid);
            ModifyAccountDataState state = ModifyAccountDataState.NOT_EXECUTED;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                ctx.IgnoreResourceNotFoundException = true;

                var qry = (from c in ctx.CLIENT.Expand("ADRESSE_CLIENT")
                           where c.id == guid
                           select c).FirstOrDefault<CLIENT>();

                if(qry != null)
                {
                    state = ModifyAccountDataState.EXIST;
                    qry.nom = nom;
                    qry.phone = phone;
                    qry.prenom = prenom;

                    if (qry.ADRESSE_CLIENT.Count == 0)
                    {
                        ADRESSE_CLIENT adresseClient = ADRESSE_CLIENT.CreateADRESSE_CLIENT(Guid.NewGuid(), adresse, ville, codePostal, pays, false);
                        ctx.AddRelatedObject(qry, "ADRESSE_CLIENT", adresseClient);
                        qry.ADRESSE_CLIENT.Add(adresseClient);
                    }
                    else
                    {
                        ADRESSE_CLIENT adresseClient = qry.ADRESSE_CLIENT[0];
                        adresseClient.adresse = adresse;
                        adresseClient.pays = pays;
                        adresseClient.ville = ville;
                        adresseClient.code_postal = codePostal;
                        ctx.UpdateObject(adresseClient);
                    }

                    ctx.UpdateObject(qry);
                    ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
                    state = ModifyAccountDataState.EXECUTED;
                }
                else
                {
                    state = ModifyAccountDataState.NOT_EXIST;
                }
            }
            catch (Exception)
            {
                state = ModifyAccountDataState.DATA_ERROR;
            }
            context.SetValue<ModifyAccountDataState>(State, state);
        }
    }
}
