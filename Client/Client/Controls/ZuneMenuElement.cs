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
    ///     <MyNamespace:ZuneMenuElement/>
    ///
    /// </summary>
    public class ZuneMenuElement : Button
    {
        private Frame frame;
        private ZuneMenu menu;

        static ZuneMenuElement()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZuneMenuElement), new FrameworkPropertyMetadata(typeof(ZuneMenuElement)));
        }

        /// <summary>
        /// Initialise les event handlers
        /// </summary>
        public ZuneMenuElement()
        {
            this.Click += new RoutedEventHandler(ZuneMenuElement_Click);
            this.Loaded += new RoutedEventHandler(ZuneMenuElement_Loaded);
        }

        /// <summary>
        /// Navigue jusqu'à la page spécifiée par le MenuElement
        /// </summary>
        public void NavigateToPage()
        {
            if (this.frame != null && !this.IsMenuSelected)
            {
                Type t = Type.GetType(NavigateTo);
                object obj = Activator.CreateInstance(t);
                this.frame.Navigate(obj);
                this.frame.Navigated += new NavigatedEventHandler(frame_Navigated);
            }
        }

        /// <summary>
        /// Lors d'un clic, navigue vers la page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ZuneMenuElement_Click(object sender, RoutedEventArgs e)
        {
            if (this.frame != null /*&& !this.IsMenuSelected*/)
            {
                Type t = Type.GetType(NavigateTo);
                object obj = Activator.CreateInstance(t);
                this.Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    new Action(() =>
                        {
                            this.frame.Navigate(obj);
                        }));
                this.frame.Navigated += new NavigatedEventHandler(frame_Navigated);
            }
        }

        /// <summary>
        /// Au chargement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ZuneMenuElement_Loaded(object sender, RoutedEventArgs e)
        {
            // Ne plait pas à expression blend
            this.frame = ((ZuneMenu)this.Parent).MeinFrame;
            this.menu = (ZuneMenu)this.Parent;
        }

        /// <summary>
        /// Lorsqu'une navigation s'est terminée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void frame_Navigated(object sender, NavigationEventArgs e)
        {
            this.menu.SetCurrentPage(this);
        }

        //private Window GetParentWindow()
        //{
        //    DependencyObject dpParent = this.Parent;
        //    do
        //    {
        //        dpParent = LogicalTreeHelper.GetParent(dpParent);
        //    } while(dpParent.GetType().BaseType != typeof(Window));
        //    return dpParent as Window;
        //}

        public string NavigateTo
        {
            get { return (string)GetValue(NavigateToProperty); }
            set { SetValue(NavigateToProperty, value); }
        }

        /// <summary>
        /// Utilisé pour les DataTriggers
        /// </summary>
        public bool IsMenuSelected
        {
            get { return (bool)GetValue(IsMenuSelectedProperty); }
            set { SetValue(IsMenuSelectedProperty, value); }
        }

        public static readonly DependencyProperty NavigateToProperty = DependencyProperty.Register(
            "NavigateTo",
            typeof(string),
            typeof(ZuneMenuElement),
            new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsMenuSelectedProperty = DependencyProperty.Register(
            "IsMenuSelected",
            typeof(bool),
            typeof(ZuneMenuElement),
            new UIPropertyMetadata(false));
    }
}
