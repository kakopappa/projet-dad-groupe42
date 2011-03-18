using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SessionService
{
    [ServiceContract(Name = "Client", SessionMode = SessionMode.Required, CallbackContract = typeof(IClient))]
    public interface IServiceClient
    {
        [OperationContract]
        bool Activate(Guid sessionID, Guid userID);

        [OperationContract]
        ItemState AddItemInCart(Guid sessionID, Guid itemID, int quantitie);

        [OperationContract]
        bool RemoveItemInCart(Guid sessionID, Guid itemID);

        [OperationContract]
        bool RemoveCartContent(Guid sessionID);
    }

    [ServiceContract(Name = "Workflow")]
    public interface IServiceWorkflow
    {
        [OperationContract]
        Guid CreateSession(Guid userID, UserType type);

        [OperationContract]
        void ChangeItemAvailability(Guid itemID, int quantity, bool available, bool exist);

        [OperationContract]
        bool RemoveCartContent(Guid sessionID);

        [OperationContract]
        Guid RemoveCartContent(Guid sessionID);
    }

    public interface IClient
    {
        [OperationContract(IsOneWay = true)]
        void Disconnected();

        [OperationContract(IsOneWay = true)]
        void CartNotification(Guid itemID, int newQuantity, ItemState state);
    }

    public enum UserType
    {
        ADMINISTRATOR = 0,
        CLIENT = 1,
        FOURNISSEUR
    }

    public enum ItemState
    {
        OK = 0,
        UNAVAILABLE = 1,
        UNKNOW = 2,
        INSUFFICIENT = 3,
        NOT_VERIFIED = 4
    }
}
