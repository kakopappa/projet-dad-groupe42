﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.1
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.Service {
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ItemState", Namespace="http://schemas.datacontract.org/2004/07/SessionService")]
    public enum ItemState : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        OK = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UNAVAILABLE = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UNKNOW = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        INSUFFICIENT = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NOT_VERIFIED = 4,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ChatState", Namespace="http://schemas.datacontract.org/2004/07/SessionService")]
    public enum ChatState : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        MESSAGED = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CLOSED = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DISCONNECTED = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UserType", Namespace="http://schemas.datacontract.org/2004/07/SessionService")]
    public enum UserType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ADMINISTRATOR = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CLIENT = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FOURNISSEUR = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UNKNOW = 3,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Service.Customer", CallbackContract=typeof(Client.Service.CustomerCallback))]
    public interface Customer {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Customer/ActivateClient", ReplyAction="http://tempuri.org/Customer/ActivateClientResponse")]
        bool ActivateClient(System.Guid sessionID, System.Guid userID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Customer/AddItemInCart", ReplyAction="http://tempuri.org/Customer/AddItemInCartResponse")]
        Client.Service.ItemState AddItemInCart(System.Guid sessionID, System.Guid itemID, int quantitie);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Customer/RemoveItemInCart", ReplyAction="http://tempuri.org/Customer/RemoveItemInCartResponse")]
        bool RemoveItemInCart(System.Guid sessionID, System.Guid itemID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Customer/RemoveCartContentClient", ReplyAction="http://tempuri.org/Customer/RemoveCartContentClientResponse")]
        bool RemoveCartContentClient(System.Guid sessionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Customer/ChatUpdateClient", ReplyAction="http://tempuri.org/Customer/ChatUpdateClientResponse")]
        bool ChatUpdateClient(System.Guid sessionID, System.Guid correspondentID, Client.Service.ChatState state, string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CustomerCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Customer/CartNotification")]
        void CartNotification(System.Guid itemID, int newQuantity, Client.Service.ItemState state);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Customer/OrderNotification")]
        void OrderNotification(System.Guid orderID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Customer/Disconnected")]
        void Disconnected();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Customer/ChatNotification")]
        void ChatNotification(System.Guid correspondentID, string message, Client.Service.ChatState state);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CustomerChannel : Client.Service.Customer, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CustomerClient : System.ServiceModel.DuplexClientBase<Client.Service.Customer>, Client.Service.Customer {
        
        public CustomerClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public CustomerClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public CustomerClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public CustomerClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public CustomerClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public bool ActivateClient(System.Guid sessionID, System.Guid userID) {
            return base.Channel.ActivateClient(sessionID, userID);
        }
        
        public Client.Service.ItemState AddItemInCart(System.Guid sessionID, System.Guid itemID, int quantitie) {
            return base.Channel.AddItemInCart(sessionID, itemID, quantitie);
        }
        
        public bool RemoveItemInCart(System.Guid sessionID, System.Guid itemID) {
            return base.Channel.RemoveItemInCart(sessionID, itemID);
        }
        
        public bool RemoveCartContentClient(System.Guid sessionID) {
            return base.Channel.RemoveCartContentClient(sessionID);
        }
        
        public bool ChatUpdateClient(System.Guid sessionID, System.Guid correspondentID, Client.Service.ChatState state, string message) {
            return base.Channel.ChatUpdateClient(sessionID, correspondentID, state, message);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Service.Administrator", CallbackContract=typeof(Client.Service.AdministratorCallback))]
    public interface Administrator {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Administrator/Connect", ReplyAction="http://tempuri.org/Administrator/ConnectResponse")]
        System.Guid Connect();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Administrator/ChatUpdateAdministrator", ReplyAction="http://tempuri.org/Administrator/ChatUpdateAdministratorResponse")]
        bool ChatUpdateAdministrator(System.Guid sessionID, System.Guid correspondentID, Client.Service.ChatState state, string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface AdministratorCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Administrator/CategorieAdded")]
        void CategorieAdded(System.Guid categorieID);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface AdministratorChannel : Client.Service.Administrator, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AdministratorClient : System.ServiceModel.DuplexClientBase<Client.Service.Administrator>, Client.Service.Administrator {
        
        public AdministratorClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public AdministratorClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public AdministratorClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public AdministratorClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public AdministratorClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public System.Guid Connect() {
            return base.Channel.Connect();
        }
        
        public bool ChatUpdateAdministrator(System.Guid sessionID, System.Guid correspondentID, Client.Service.ChatState state, string message) {
            return base.Channel.ChatUpdateAdministrator(sessionID, correspondentID, state, message);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Service.Fournisseur", CallbackContract=typeof(Client.Service.FournisseurCallback))]
    public interface Fournisseur {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Fournisseur/ActivateFournisseur", ReplyAction="http://tempuri.org/Fournisseur/ActivateFournisseurResponse")]
        bool ActivateFournisseur(System.Guid sessionID, System.Guid userID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Fournisseur/ChatUpdateFournisseur", ReplyAction="http://tempuri.org/Fournisseur/ChatUpdateFournisseurResponse")]
        bool ChatUpdateFournisseur(System.Guid sessionID, System.Guid correspondentID, Client.Service.ChatState state, string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface FournisseurCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Fournisseur/NewOrder")]
        void NewOrder(System.Guid orderID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Fournisseur/CategorieNotification")]
        void CategorieNotification(System.Guid categorieID, bool deleted);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Fournisseur/ProductNotification")]
        void ProductNotification(System.Guid itemID, bool deleted);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface FournisseurChannel : Client.Service.Fournisseur, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FournisseurClient : System.ServiceModel.DuplexClientBase<Client.Service.Fournisseur>, Client.Service.Fournisseur {
        
        public FournisseurClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public FournisseurClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public FournisseurClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public FournisseurClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public FournisseurClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public bool ActivateFournisseur(System.Guid sessionID, System.Guid userID) {
            return base.Channel.ActivateFournisseur(sessionID, userID);
        }
        
        public bool ChatUpdateFournisseur(System.Guid sessionID, System.Guid correspondentID, Client.Service.ChatState state, string message) {
            return base.Channel.ChatUpdateFournisseur(sessionID, correspondentID, state, message);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Service.Workflow")]
    public interface Workflow {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Workflow/CreateSession", ReplyAction="http://tempuri.org/Workflow/CreateSessionResponse")]
        System.Guid CreateSession(System.Guid userID, Client.Service.UserType type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Workflow/GetUserID", ReplyAction="http://tempuri.org/Workflow/GetUserIDResponse")]
        System.Guid GetUserID(System.Guid sessionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Workflow/GetSessionType", ReplyAction="http://tempuri.org/Workflow/GetSessionTypeResponse")]
        Client.Service.UserType GetSessionType(System.Guid sessionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Workflow/ChangeItemAvailability", ReplyAction="http://tempuri.org/Workflow/ChangeItemAvailabilityResponse")]
        void ChangeItemAvailability(System.Guid itemID, int quantity, bool available, bool exist);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Workflow/AdminProductModification", ReplyAction="http://tempuri.org/Workflow/AdminProductModificationResponse")]
        void AdminProductModification(System.Guid itemID, System.Guid fournisseurID, bool deleted);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Workflow/RemoveCartContent", ReplyAction="http://tempuri.org/Workflow/RemoveCartContentResponse")]
        bool RemoveCartContent(System.Guid sessionID);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WorkflowChannel : Client.Service.Workflow, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WorkflowClient : System.ServiceModel.ClientBase<Client.Service.Workflow>, Client.Service.Workflow {
        
        public WorkflowClient() {
        }
        
        public WorkflowClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WorkflowClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WorkflowClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WorkflowClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Guid CreateSession(System.Guid userID, Client.Service.UserType type) {
            return base.Channel.CreateSession(userID, type);
        }
        
        public System.Guid GetUserID(System.Guid sessionID) {
            return base.Channel.GetUserID(sessionID);
        }
        
        public Client.Service.UserType GetSessionType(System.Guid sessionID) {
            return base.Channel.GetSessionType(sessionID);
        }
        
        public void ChangeItemAvailability(System.Guid itemID, int quantity, bool available, bool exist) {
            base.Channel.ChangeItemAvailability(itemID, quantity, available, exist);
        }
        
        public void AdminProductModification(System.Guid itemID, System.Guid fournisseurID, bool deleted) {
            base.Channel.AdminProductModification(itemID, fournisseurID, deleted);
        }
        
        public bool RemoveCartContent(System.Guid sessionID) {
            return base.Channel.RemoveCartContent(sessionID);
        }
    }
}
