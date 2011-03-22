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
                                MainWindow.GetInstance().UserId = new Guid(userId);
                                MainWindow.GetInstance().SessionId = new Guid(sessionId);
                                MainWindow.GetInstance().btnListProduct.IsEnabled = true;
                                MainWindow.GetInstance().btnLstCommand.IsEnabled = true;
                                MainWindow.GetInstance().btnNewProduct.IsEnabled = true;
                                
                                break;

                            case WorkflowConnection.CheckIfPasswordMatchResult.NOT_MATCH:
                                MainWindow.GetInstance().btnListProduct.IsEnabled = false;
                                MainWindow.GetInstance().btnLstCommand.IsEnabled = false;
                                MainWindow.GetInstance().btnNewProduct.IsEnabled = false;

                                break;

                            default:
                                break;
                        }
                        break;

                    case WorkflowConnection.CheckIfFournisseurExistResult.NOT_EXIST:
                            MainWindow.GetInstance().btnListProduct.IsEnabled = false;
                            MainWindow.GetInstance().btnLstCommand.IsEnabled = false;
                            MainWindow.GetInstance().btnNewProduct.IsEnabled = false;
                        break;

                    default:
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
             MainWindow.GetInstance().btnListProduct.IsEnabled = false;
             MainWindow.GetInstance().btnLstCommand.IsEnabled = false;
             MainWindow.GetInstance().btnNewProduct.IsEnabled = false;
         }
        }
    }
}
