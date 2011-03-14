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
    /// Logique d'interaction pour PanControl.xaml
    /// </summary>
    public partial class PanControl : UserControl
    {
        public PanControl()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(PanControl_Loaded);
        }

        void PanControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
