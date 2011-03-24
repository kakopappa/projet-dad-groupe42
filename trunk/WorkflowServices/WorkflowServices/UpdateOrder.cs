using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;
using WorkflowServices.Service;

namespace WorkflowServices
{
    public enum OrderUpdateAction
    {
        VALIDATE,
        SEND,
        RECEIVE
    }

    public enum UpdateOrderState
    {
        SERVICE_ERROR,
        DATA_ERROR,
        EXIST,
        NOT_EXIST,
        NOT_EXECUTED,
        EXECUTED
    }

    public sealed class UpdateOrder : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<OrderUpdateAction> Action { get; set; }
        public InArgument<Guid> OrderID { get; set; }
        public InArgument<Decimal> Estimation { get; set; }
        public InArgument<Guid> userID { get; set; }
        public InArgument<UserType> Type { get; set; }
        public OutArgument<UpdateOrderState> State { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtenir la valeur runtime de l'argument d'entrée Text
            OrderUpdateAction action = context.GetValue(this.Action);
            Guid orderID = context.GetValue(this.OrderID);
            Decimal estimation = context.GetValue(this.Estimation);
            Guid userID = context.GetValue(this.userID);
            UserType userType = context.GetValue(this.Type);
            UpdateOrderState state = UpdateOrderState.NOT_EXECUTED;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                ctx.IgnoreResourceNotFoundException = true;

                switch(userType)
                {
                    case UserType.FOURNISSEUR :
                        var qryF = (from o in ctx.COMMANDE_FOURNISSEUR.Expand("FOURNISSEUR")
                           where o.id == orderID && o.FOURNISSEUR.id == userID
                           select o).FirstOrDefault<COMMANDE_FOURNISSEUR>();
                        if (qryF != null)
                        {
                            state = UpdateOrderState.EXIST;
                            switch (action)
                            {
                                case OrderUpdateAction.VALIDATE :
                                    if(!qryF.date_validation.HasValue)
                                        qryF.date_validation = new DateTime?(DateTime.Now);
                                    break;
                                case OrderUpdateAction.SEND :
                                    if (qryF.date_validation.HasValue && !qryF.date_expedition.HasValue)
                                    {
                                        qryF.date_expedition = new DateTime?(DateTime.Now);
                                        qryF.duree_expedition_estimee = new Decimal?(estimation);
                                    }
                                    break;
                            }
                            ctx.UpdateObject(qryF);
                        }
                        else
                            state = UpdateOrderState.NOT_EXIST;
                        break;

                    case UserType.CLIENT :
                        var qryC = (from o in ctx.COMMANDE_FOURNISSEUR.Expand("COMMANDE_CLIENT/CLIENT")
                           where o.id == orderID && o.COMMANDE_CLIENT.CLIENT.id == userID
                           select o).FirstOrDefault<COMMANDE_FOURNISSEUR>();
                        if (qryC != null)
                        {
                            state = UpdateOrderState.EXIST;
                            switch (action)
                            {
                                case OrderUpdateAction.RECEIVE :
                                    if (qryC.date_expedition.HasValue && !qryC.date_reception.HasValue)
                                        qryC.date_reception = new DateTime?(DateTime.Now);
                                    break;
                            }
                            ctx.UpdateObject(qryC);
                        }
                        else
                            state = UpdateOrderState.NOT_EXIST;
                        break;
                }

                ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
                state = UpdateOrderState.EXECUTED;
            }
            catch
            {
                state = UpdateOrderState.DATA_ERROR;
            }
            context.SetValue<UpdateOrderState>(State, state);
        }
    }
}
