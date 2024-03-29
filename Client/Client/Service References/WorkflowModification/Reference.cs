﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.1
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.WorkflowModification {
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ModifyAccountDataState", Namespace="http://schemas.datacontract.org/2004/07/WorkflowServices")]
    public enum ModifyAccountDataState : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SERVICE_ERROR = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DATA_ERROR = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EXIST = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NOT_EXIST = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NOT_EXECUTED = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EXECUTED = 5,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WorkflowModification.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/SessionIDVerification", ReplyAction="http://tempuri.org/IService/SessionIDVerificationResponse")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="Result")]
        bool SessionIDVerification(System.Guid SessionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/ModifyAccountData", ReplyAction="http://tempuri.org/IService/ModifyAccountDataResponse")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="ModificationState")]
        Client.WorkflowModification.ModifyAccountDataState ModifyAccountData(string Phone, string Prenom, string Nom, string Adresse, string Ville, string CodePostal, string Pays);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : Client.WorkflowModification.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<Client.WorkflowModification.IService>, Client.WorkflowModification.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool SessionIDVerification(System.Guid SessionID) {
            return base.Channel.SessionIDVerification(SessionID);
        }
        
        public Client.WorkflowModification.ModifyAccountDataState ModifyAccountData(string Phone, string Prenom, string Nom, string Adresse, string Ville, string CodePostal, string Pays) {
            return base.Channel.ModifyAccountData(Phone, Prenom, Nom, Adresse, Ville, CodePostal, Pays);
        }
    }
}
