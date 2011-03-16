using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace DataServiceClient
{
    public class DataServiceClient : DataService<DADEntities>
    {
        // Cette méthode n'est appelée qu'une seule fois pour initialiser les stratégies au niveau des services.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: définissez des règles pour indiquer les jeux d'entités et opérations de service visibles, pouvant être mis à jour, etc.
            // Exemples :
            config.SetEntitySetAccessRule("CLIENT", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("COMMANDER", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PRODUIT", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("CATEGORIE", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("FOURNISSEUR", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ADRESSE_CLIENT", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("COMMANDE_CLIENT", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("COMMANDE_FOURNISSEUR", EntitySetRights.AllRead);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }
    }
}
