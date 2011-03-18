using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Events;

namespace Client.Core
{
    public class Connection
    {
        public event EventHandler WrongCredentials;
        public event EventHandler ServiceErrors;
        public event ObjectEventHandler Success;

        public void Connect(string mail, string password)
        {
            WorkflowConnection.ServiceClient svc = null;

            try
            {
                svc = new WorkflowConnection.ServiceClient();
                WorkflowConnection.GetUserIdentifierResponse uresp = svc.GetUserIdentifier(mail);

                // Retour du check user
                switch (uresp.UserExist)
                {
                    case WorkflowConnection.CheckIfUserExistResult.EXIST:
                        WorkflowConnection.GetUserPasswordResponse presp = svc.GetUserPassword(
                            Tools.Tools.HashPassword(mail, password));

                        // Retour du check password
                        switch (presp.PasswordMatch)
                        {
                            case WorkflowConnection.CheckIfPasswordMatchResult.MATCH:
                                Guid id = new Guid(svc.GuidRequest());
                                OnSuccess(new ObjectEventArgs(id));
                                break;

                            case WorkflowConnection.CheckIfPasswordMatchResult.NOT_MATCH:
                                OnWrongCredentials(new EventArgs());
                                break;

                            default:
                                OnServiceErrors(new EventArgs());
                                break;
                        }
                        break;

                    case WorkflowConnection.CheckIfUserExistResult.NOT_EXIST:
                        OnWrongCredentials(new EventArgs());
                        break;

                    default:
                        OnServiceErrors(new EventArgs());
                        break;
                }
            }
            catch
            {
                OnServiceErrors(new EventArgs());
            }
            finally
            {
                if (svc != null)
                {
                    svc.Close();
                    svc.ChannelFactory.Close();
                }
            }
        }

        /// <summary>
        /// Credentials erronnés
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnWrongCredentials(EventArgs e)
        {
            if (WrongCredentials != null)
                WrongCredentials(this, e);
        }

        /// <summary>
        /// Erreur de service
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnServiceErrors(EventArgs e)
        {
            if(ServiceErrors != null)
                ServiceErrors(this, e);
        }

        /// <summary>
        /// Connexion réussie
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSuccess(ObjectEventArgs e)
        {
            if (Success != null)
                Success(this, e);
        }
    }
}
