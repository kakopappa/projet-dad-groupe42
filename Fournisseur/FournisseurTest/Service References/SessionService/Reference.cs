﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.1
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FournisseurTest.SessionService {
    using System.Runtime.Serialization;
    using System;
    
    
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CartEntrie", Namespace="http://schemas.datacontract.org/2004/07/SessionService")]
    [System.SerializableAttribute()]
    public partial class CartEntrie : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid ObjectIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int QuantityField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid ObjectID {
            get {
                return this.ObjectIDField;
            }
            set {
                if ((this.ObjectIDField.Equals(value) != true)) {
                    this.ObjectIDField = value;
                    this.RaisePropertyChanged("ObjectID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Quantity {
            get {
                return this.QuantityField;
            }
            set {
                if ((this.QuantityField.Equals(value) != true)) {
                    this.QuantityField = value;
                    this.RaisePropertyChanged("Quantity");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SessionService.Customer", CallbackContract=typeof(FournisseurTest.SessionService.CustomerCallback))]
    public interface Customer {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Customer/DisconnectClient", ReplyAction="http://tempuri.org/Customer/DisconnectClientResponse")]
        bool DisconnectClient(System.Guid sessionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Customer/ActivateClient", ReplyAction="http://tempuri.org/Customer/ActivateClientResponse")]
        bool ActivateClient(System.Guid sessionID, System.Guid userID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Customer/AddItemInCart", ReplyAction="http://tempuri.org/Customer/AddItemInCartResponse")]
        FournisseurTest.SessionService.ItemState AddItemInCart(System.Guid sessionID, System.Guid itemID, int quantitie);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Customer/RemoveItemInCart", ReplyAction="http://tempuri.org/Customer/RemoveItemInCartResponse")]
        bool RemoveItemInCart(System.Guid sessionID, System.Guid itemID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Customer/RemoveCartContentClient", ReplyAction="http://tempuri.org/Customer/RemoveCartContentClientResponse")]
        bool RemoveCartContentClient(System.Guid sessionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Customer/ChatUpdateClient", ReplyAction="http://tempuri.org/Customer/ChatUpdateClientResponse")]
        bool ChatUpdateClient(System.Guid sessionID, System.Guid correspondentID, FournisseurTest.SessionService.ChatState state, string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CustomerCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Customer/CartNotification")]
        void CartNotification(System.Guid itemID, int newQuantity, FournisseurTest.SessionService.ItemState state);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Customer/OrderNotification")]
        void OrderNotification(System.Guid orderID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Customer/DisconnectedClient")]
        void DisconnectedClient();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Customer/ChatNotificationClient")]
        void ChatNotificationClient(System.Guid correspondentID, string message, FournisseurTest.SessionService.ChatState state);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CustomerChannel : FournisseurTest.SessionService.Customer, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CustomerClient : System.ServiceModel.DuplexClientBase<FournisseurTest.SessionService.Customer>, FournisseurTest.SessionService.Customer {
        
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
        
        public bool DisconnectClient(System.Guid sessionID) {
            return base.Channel.DisconnectClient(sessionID);
        }
        
        public bool ActivateClient(System.Guid sessionID, System.Guid userID) {
            return base.Channel.ActivateClient(sessionID, userID);
        }
        
        public FournisseurTest.SessionService.ItemState AddItemInCart(System.Guid sessionID, System.Guid itemID, int quantitie) {
            return base.Channel.AddItemInCart(sessionID, itemID, quantitie);
        }
        
        public bool RemoveItemInCart(System.Guid sessionID, System.Guid itemID) {
            return base.Channel.RemoveItemInCart(sessionID, itemID);
        }
        
        public bool RemoveCartContentClient(System.Guid sessionID) {
            return base.Channel.RemoveCartContentClient(sessionID);
        }
        
        public bool ChatUpdateClient(System.Guid sessionID, System.Guid correspondentID, FournisseurTest.SessionService.ChatState state, string message) {
            return base.Channel.ChatUpdateClient(sessionID, correspondentID, state, message);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SessionService.Administrator", CallbackContract=typeof(FournisseurTest.SessionService.AdministratorCallback))]
    public interface Administrator {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Administrator/DisconnectAdministrateur", ReplyAction="http://tempuri.org/Administrator/DisconnectAdministrateurResponse")]
        bool DisconnectAdministrateur(System.Guid sessionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Administrator/Connect", ReplyAction="http://tempuri.org/Administrator/ConnectResponse")]
        System.Guid Connect();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Administrator/ChatUpdateAdministrator", ReplyAction="http://tempuri.org/Administrator/ChatUpdateAdministratorResponse")]
        bool ChatUpdateAdministrator(System.Guid sessionID, System.Guid correspondentID, FournisseurTest.SessionService.ChatState state, string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface AdministratorCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Administrator/CategorieAdded")]
        void CategorieAdded(System.Guid categorieID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Administrator/DisconnectedAdministrator")]
        void DisconnectedAdministrator();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Administrator/ChatNotificationAdministrator")]
        void ChatNotificationAdministrator(System.Guid correspondentID, string message, FournisseurTest.SessionService.ChatState state);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface AdministratorChannel : FournisseurTest.SessionService.Administrator, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AdministratorClient : System.ServiceModel.DuplexClientBase<FournisseurTest.SessionService.Administrator>, FournisseurTest.SessionService.Administrator {
        
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
        
        public bool DisconnectAdministrateur(System.Guid sessionID) {
            return base.Channel.DisconnectAdministrateur(sessionID);
        }
        
        public System.Guid Connect() {
            return base.Channel.Connect();
        }
        
        public bool ChatUpdateAdministrator(System.Guid sessionID, System.Guid correspondentID, FournisseurTest.SessionService.ChatState state, string message) {
            return base.Channel.ChatUpdateAdministrator(sessionID, correspondentID, state, message);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SessionService.Fournisseur", CallbackContract=typeof(FournisseurTest.SessionService.FournisseurCallback))]
    public interface Fournisseur {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Fournisseur/DisconnectFournisseur", ReplyAction="http://tempuri.org/Fournisseur/DisconnectFournisseurResponse")]
        bool DisconnectFournisseur(System.Guid sessionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Fournisseur/ActivateFournisseur", ReplyAction="http://tempuri.org/Fournisseur/ActivateFournisseurResponse")]
        bool ActivateFournisseur(System.Guid sessionID, System.Guid userID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Fournisseur/ChatUpdateFournisseur", ReplyAction="http://tempuri.org/Fournisseur/ChatUpdateFournisseurResponse")]
        bool ChatUpdateFournisseur(System.Guid sessionID, System.Guid correspondentID, FournisseurTest.SessionService.ChatState state, string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface FournisseurCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Fournisseur/NewOrder")]
        void NewOrder(System.Guid orderID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Fournisseur/CategorieNotification")]
        void CategorieNotification(System.Guid categorieID, bool deleted);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Fournisseur/ProductNotification")]
        void ProductNotification(System.Guid itemID, bool deleted);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Fournisseur/ChatNotificationFournisseur")]
        void ChatNotificationFournisseur(System.Guid correspondentID, string message, FournisseurTest.SessionService.ChatState state);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Fournisseur/DisconnectedFournisseur")]
        void DisconnectedFournisseur();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface FournisseurChannel : FournisseurTest.SessionService.Fournisseur, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FournisseurClient : System.ServiceModel.DuplexClientBase<FournisseurTest.SessionService.Fournisseur>, FournisseurTest.SessionService.Fournisseur {
        
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
        
        public bool DisconnectFournisseur(System.Guid sessionID) {
            return base.Channel.DisconnectFournisseur(sessionID);
        }
        
        public bool ActivateFournisseur(System.Guid sessionID, System.Guid userID) {
            return base.Channel.ActivateFournisseur(sessionID, userID);
        }
        
        public bool ChatUpdateFournisseur(System.Guid sessionID, System.Guid correspondentID, FournisseurTest.SessionService.ChatState state, string message) {
            return base.Channel.ChatUpdateFournisseur(sessionID, correspondentID, state, message);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SessionService.Workflow")]
    public interface Workflow {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Workflow/CreateSession", ReplyAction="http://tempuri.org/Workflow/CreateSessionResponse")]
        System.Guid CreateSession(System.Guid userID, FournisseurTest.SessionService.UserType type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Workflow/GetCart", ReplyAction="http://tempuri.org/Workflow/GetCartResponse")]
        FournisseurTest.SessionService.CartEntrie[] GetCart(System.Guid sessionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Workflow/GetUserID", ReplyAction="http://tempuri.org/Workflow/GetUserIDResponse")]
        System.Guid GetUserID(System.Guid sessionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Workflow/GetSessionType", ReplyAction="http://tempuri.org/Workflow/GetSessionTypeResponse")]
        FournisseurTest.SessionService.UserType GetSessionType(System.Guid sessionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Workflow/ChangeItemAvailability", ReplyAction="http://tempuri.org/Workflow/ChangeItemAvailabilityResponse")]
        void ChangeItemAvailability(System.Guid itemID, int quantity, bool available, bool exist);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Workflow/AdminProductModification", ReplyAction="http://tempuri.org/Workflow/AdminProductModificationResponse")]
        void AdminProductModification(System.Guid itemID, System.Guid fournisseurID, bool deleted);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Workflow/RemoveCartContent", ReplyAction="http://tempuri.org/Workflow/RemoveCartContentResponse")]
        bool RemoveCartContent(System.Guid sessionID);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WorkflowChannel : FournisseurTest.SessionService.Workflow, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WorkflowClient : System.ServiceModel.ClientBase<FournisseurTest.SessionService.Workflow>, FournisseurTest.SessionService.Workflow {
        
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
        
        public System.Guid CreateSession(System.Guid userID, FournisseurTest.SessionService.UserType type) {
            return base.Channel.CreateSession(userID, type);
        }
        
        public FournisseurTest.SessionService.CartEntrie[] GetCart(System.Guid sessionID) {
            return base.Channel.GetCart(sessionID);
        }
        
        public System.Guid GetUserID(System.Guid sessionID) {
            return base.Channel.GetUserID(sessionID);
        }
        
        public FournisseurTest.SessionService.UserType GetSessionType(System.Guid sessionID) {
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
