using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SessionService
{
    [ServiceContract(Name = "Customer", SessionMode = SessionMode.Allowed, CallbackContract = typeof(IClient))]
    public interface IServiceClient
    {
        [OperationContract]
        bool DisconnectClient(Guid sessionID);

        [OperationContract]
        bool ActivateClient(Guid sessionID, Guid userID);

        [OperationContract]
        ItemState AddItemInCart(Guid sessionID, Guid itemID, int quantitie);

        [OperationContract]
        bool RemoveItemInCart(Guid sessionID, Guid itemID);

        [OperationContract]
        bool RemoveCartContentClient(Guid sessionID);

        [OperationContract]
        bool ChatUpdateClient(Guid sessionID, Guid correspondentID, ChatState state, string message);
    }

    [ServiceContract(Name = "Fournisseur", SessionMode = SessionMode.Allowed, CallbackContract = typeof(IFournisseur))]
    public interface IServiceFournisseur
    {
        [OperationContract]
        bool DisconnectFournisseur(Guid sessionID);

        [OperationContract]
        bool ActivateFournisseur(Guid sessionID, Guid userID);

        [OperationContract]
        bool ChatUpdateFournisseur(Guid sessionID, Guid correspondentID, ChatState state, string message);
    }

    [ServiceContract(Name = "Administrator", SessionMode = SessionMode.Allowed, CallbackContract = typeof(IAdministrator))]
    public interface IServiceAdministrator
    {
        [OperationContract]
        bool DisconnectAdministrateur(Guid sessionID);

        [OperationContract]
        Guid Connect();

        [OperationContract]
        bool ChatUpdateAdministrator(Guid sessionID, Guid correspondentID, ChatState state, string message);

    }

    [ServiceContract(Name = "Workflow")]
    public interface IServiceWorkflow
    {
        [OperationContract]
        Guid CreateSession(Guid userID, UserType type);

        [OperationContract]
        CartEntrie[] GetCart(Guid sessionID);
        
        [OperationContract]
        Guid GetUserID(Guid sessionID);

        [OperationContract]
        UserType GetSessionType(Guid sessionID);

        [OperationContract]
        void ChangeItemAvailability(Guid itemID, int quantity, bool available, bool exist);

        [OperationContract]
        void AdminProductModification(Guid itemID, Guid fournisseurID, bool deleted);

        [OperationContract]
        bool RemoveCartContent(Guid sessionID);
    }

    public interface IClient
    {
        [OperationContract(IsOneWay = true)]
        void CartNotification(Guid itemID, int newQuantity, ItemState state);

        [OperationContract(IsOneWay = true)]
        void OrderNotification(Guid orderID);

        [OperationContract(IsOneWay = true)]
        void DisconnectedClient();

        [OperationContract(IsOneWay = true)]
        void ChatNotificationClient(Guid correspondentID, string message, ChatState state);
    }

    public interface IAdministrator
    {
        [OperationContract(IsOneWay = true)]
        void CategorieAdded(Guid categorieID);

        [OperationContract(IsOneWay = true)]
        void DisconnectedAdministrator();

        [OperationContract(IsOneWay = true)]
        void ChatNotificationAdministrator(Guid correspondentID, string message, ChatState state);
    }

    public interface IFournisseur
    {
        [OperationContract(IsOneWay = true)]
        void NewOrder(Guid orderID);

        [OperationContract(IsOneWay = true)]
        void CategorieNotification(Guid categorieID, bool deleted);

        [OperationContract(IsOneWay = true)]
        void ProductNotification(Guid itemID, bool deleted);

        [OperationContract(IsOneWay = true)]
        void ChatNotificationFournisseur(Guid correspondentID, string message, ChatState state);

        [OperationContract(IsOneWay = true)]
        void DisconnectedFournisseur();
    }

    public enum UserType
    {
        ADMINISTRATOR = 0,
        CLIENT = 1,
        FOURNISSEUR = 2,
        UNKNOW = 3
    }

    public enum ItemState
    {
        OK = 0,
        UNAVAILABLE = 1,
        UNKNOW = 2,
        INSUFFICIENT = 3,
        NOT_VERIFIED = 4
    }

    public enum ChatState
    {
        MESSAGED = 0,
        CLOSED = 1,
        DISCONNECTED = 2
    }
}
