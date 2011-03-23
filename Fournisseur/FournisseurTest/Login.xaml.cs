using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FournisseurTest.DataServiceClient;
using System.Configuration;
using System.Windows.Threading;

namespace FournisseurTest
{
    /// <summary>
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        public void LoadConnexion(object sender, RoutedEventArgs args)
        {

            if (UserLoginBox.Text != "" && UserPasswordBox.Password != "")
            {
                WorkflowConnection.ServiceClient svc = null;
                try
                {
                    svc = new WorkflowConnection.ServiceClient();
                    WorkflowConnection.GetUserIdentifierResponse result = svc.GetUserIdentifier(UserLoginBox.Text);

                    switch (result.UserExist)
                    {
                        case WorkflowConnection.CheckIfFournisseurExistResult.EXIST:
                        WorkflowConnection.GetUserPasswordResponse resp = svc.GetUserPassword(UserPasswordBox.Password);

                        switch (resp.PasswordMatch)
                        {
                            case WorkflowConnection.CheckIfPasswordMatchResult.MATCH:
                                string sessionId;
                                string userId = svc.GuidRequest(out sessionId);
                                bool validation = MainWindow.GetInstance().SessionService.ActivateFournisseur(new Guid(sessionId),
                                    new Guid(userId));
                                Console.WriteLine(validation);
                                MainWindow.GetInstance().UserId = new Guid(userId);
                                MainWindow.GetInstance().SessionId = new Guid(sessionId);
                                MainWindow.GetInstance().btnListProduct.IsEnabled = true;
                                MainWindow.GetInstance().btnLstCommand.IsEnabled = true;
                                MainWindow.GetInstance().btnNewProduct.IsEnabled = true;
                                MainWindow.GetInstance().buttonDeconnexion.IsEnabled = true;
                                MainWindow.GetInstance().Frame.Navigate(new ListeCommande());
                                                                break;

                            case WorkflowConnection.CheckIfPasswordMatchResult.NOT_MATCH:
                                LabelError.Content = "Erreur de password";
                                break;

                            default:
                                LabelError.Content = "Erreur de service";
                                break;
                        }
                        break;

                    case WorkflowConnection.CheckIfFournisseurExistResult.NOT_EXIST:
                        LabelError.Content = "L'utilisateur n'existe pas";
                        break;

                    default:
                        LabelError.Content = "Erreur de service";
                        break;
                }
            }
            catch
            {
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
         else
         {
             LabelError.Content = "Veuillez remplir les champs";
         }
        }
    }
}
