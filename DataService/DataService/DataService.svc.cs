using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace DataService
{
    public class DataService : DataService<DADEntities>
    {
        // Cette méthode n'est appelée qu'une seule fois pour initialiser les stratégies au niveau des services.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: définissez des règles pour indiquer les jeux d'entités et opérations de service visibles, pouvant être mis à jour, etc.
            // Exemples :
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }
    }
}
