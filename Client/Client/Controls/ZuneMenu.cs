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
    ///     <MyNamespace:ZuneMenu/>
    ///
    /// </summary>
    public class ZuneMenu : ItemsControl
    {
        static ZuneMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZuneMenu), new FrameworkPropertyMetadata(typeof(ZuneMenu)));
        }

        /// <summary>
        /// Définit la page en cours pour la surbrillance
        /// </summary>
        /// <param name="currentEle"></param>
        public void SetCurrentPage(ZuneMenuElement currentEle)
        {
            foreach(ZuneMenuElement item in Items)
            {
                if (item == currentEle)
                {
                    item.IsMenuSelected = true;
                }
                else
                {
                    item.IsMenuSelected = false;
                }
            }
        }

        /// <summary>
        /// Retourne la frame du composant
        /// </summary>
        public Frame MeinFrame
        {
            get
            {
                return (Frame)this.Template.FindName("frame", this);
            }
        }
    }
}
