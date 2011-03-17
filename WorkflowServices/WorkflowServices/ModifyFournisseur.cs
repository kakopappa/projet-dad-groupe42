using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;

namespace WorkflowServices
{
    public enum ModifyFournisseurDataState
    {
        SERVICE_ERROR,
        DATA_ERROR,
        EXIST,
        NOT_EXIST,
        NOT_EXECUTED,
        EXECUTED
    }

    public sealed class ModifyFournisseur : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<string> Nom { get; set; }
        public InArgument<string> Phone { get; set; }
        public InArgument<string> Adresse { get; set; }
        public InArgument<string> Ville { get; set; }
        public InArgument<string> CodePostal { get; set; }
        public InArgument<string> Pays { get; set; }
        public InArgument<Guid> FournisseurGuid { get; set; }
        public OutArgument<ModifyFournisseurDataState> State { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            string nom = context.GetValue<string>(Nom);
            string phone = context.GetValue<string>(Phone);
            string adresse = context.GetValue<string>(Adresse);
            string ville = context.GetValue<string>(Ville);
            string pays = context.GetValue<string>(Pays);
            string codePostal = context.GetValue<string>(CodePostal);
            Guid guid = context.GetValue<Guid>(FournisseurGuid);
            ModifyFournisseurDataState state = ModifyFournisseurDataState.NOT_EXECUTED;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                ctx.IgnoreResourceNotFoundException = true;

                var qry = (from f in ctx.FOURNISSEUR
                           where f.id == guid
                           select f).FirstOrDefault<FOURNISSEUR>();

                if (qry != null)
                {
                    state = ModifyFournisseurDataState.EXIST;
                    qry.nom = nom;
                    qry.phone = phone;
                    qry.adresse = adresse;
                    qry.code_postal = codePostal;
                    qry.pays = pays;
                    qry.ville = ville;

                    ctx.UpdateObject(qry);
                    ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
                    state = ModifyFournisseurDataState.EXECUTED;
                }
                else
                {
                    state = ModifyFournisseurDataState.NOT_EXIST;
                }
            }
            catch (Exception)
            {
                state = ModifyFournisseurDataState.DATA_ERROR;
            }
            context.SetValue<ModifyFournisseurDataState>(State, state);
        }
    }
}
