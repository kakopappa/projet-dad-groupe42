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
using System.Windows.Threading;

namespace Client.Controls
{
    /// <summary>
    /// Suivez les étapes 1a ou 1b puis 2 pour utiliser ce contrôle personnalisé dans un fichier XAML.
    ///
    /// Étape 1a) Utilisation de ce contrôle personnalisé dans un fichier XAML qui existe dans le projet actif.
    /// Ajoutez cet attribut XmlNamespace à l'élément racine du fichier de balisage où il doit 
    /// être utilisé :
    ///
    ///     xmlns:MyNamespace="clr-namespace:Client.Controls"
    ///
    ///
    /// Étape 1b) Utilisation de ce contrôle personnalisé dans un fichier XAML qui existe dans un autre projet.
    /// Ajoutez cet attribut XmlNamespace à l'élément racine du fichier de balisage où il doit 
    /// être utilisé :
    ///
    ///     xmlns:MyNamespace="clr-namespace:Client.Controls;assembly=Client.Controls"
    ///
    /// Vous devrez également ajouter une référence du projet contenant le fichier XAML
    /// à ce projet et régénérer pour éviter des erreurs de compilation :
    ///
    ///     Cliquez avec le bouton droit sur le projet cible dans l'Explorateur de solutions, puis sur
    ///     "Ajouter une référence"->"Projets"->[Recherchez et sélectionnez ce projet]
    ///
    ///
    /// Étape 2)
    /// Utilisez à présent votre contrôle dans le fichier XAML.
    ///
    ///     <MyNamespace:ZunePanControl/>
    ///
    /// </summary>
    public class ZunePanControl : ItemsControl
    {
        private ScrollViewer scrollViewer;
        private StackPanel stackPanel;

        static ZunePanControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZunePanControl), new FrameworkPropertyMetadata(typeof(ZunePanControl)));
        }

        /// <summary>
        /// Initialise les event handlers
        /// </summary>
        public ZunePanControl()
        {
            this.Loaded += new RoutedEventHandler(ZunePanControl_Loaded);
            this.MouseMove += new MouseEventHandler(ZunePanControl_MouseMove);
        }

        /// <summary>
        /// Au chargement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ZunePanControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.scrollViewer = (ScrollViewer)this.Template.FindName("scrollViewer", this);
            this.stackPanel = (StackPanel)this.Template.FindName("stackPanel", this);
        }

        /// <summary>
        /// Lorsque la souris bouge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ZunePanControl_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePosition = Mouse.GetPosition(this);
            this.scrollViewer.Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() =>
                    {
                        this.scrollViewer.ScrollToHorizontalOffset(
                            mousePosition.X * (this.stackPanel.ActualWidth - this.scrollViewer.ActualWidth) / this.scrollViewer.ActualWidth);
                    }));
        }
    }
}
