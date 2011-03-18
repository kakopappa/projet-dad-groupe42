﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.1
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkflowServices.Service {
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UserType", Namespace="http://schemas.datacontract.org/2004/07/SessionService")]
    public enum UserType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ADMINISTRATOR = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CLIENT = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FOURNISSEUR = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Service.Client", CallbackContract=typeof(WorkflowServices.Service.ClientCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface Client {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Client/Activate", ReplyAction="http://tempuri.org/Client/ActivateResponse")]
        WorkflowServices.Service.ActivateResponse Activate(WorkflowServices.Service.ActivateRequest request);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ClientCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/Client/Disconnected")]
        void Disconnected(WorkflowServices.Service.Disconnected request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Activate", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class ActivateRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public System.Guid userID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public System.Guid sessionID;
        
        public ActivateRequest() {
        }
        
        public ActivateRequest(System.Guid userID, System.Guid sessionID) {
            this.userID = userID;
            this.sessionID = sessionID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ActivateResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class ActivateResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public bool ActivateResult;
        
        public ActivateResponse() {
        }
        
        public ActivateResponse(bool ActivateResult) {
            this.ActivateResult = ActivateResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Disconnected", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class Disconnected {
        
        public Disconnected() {
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Service.Workflow")]
    public interface Workflow {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Workflow/CreateSession", ReplyAction="http://tempuri.org/Workflow/CreateSessionResponse")]
        WorkflowServices.Service.CreateSessionResponse CreateSession(WorkflowServices.Service.CreateSessionRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CreateSession", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class CreateSessionRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public System.Guid userID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public WorkflowServices.Service.UserType type;
        
        public CreateSessionRequest() {
        }
        
        public CreateSessionRequest(System.Guid userID, WorkflowServices.Service.UserType type) {
            this.userID = userID;
            this.type = type;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CreateSessionResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class CreateSessionResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public System.Guid CreateSessionResult;
        
        public CreateSessionResponse() {
        }
        
        public CreateSessionResponse(System.Guid CreateSessionResult) {
            this.CreateSessionResult = CreateSessionResult;
        }
    }
}
