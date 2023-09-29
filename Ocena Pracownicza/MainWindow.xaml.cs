using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ocena_Pracownicza
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void FormButton_Click(object sender, RoutedEventArgs e)
        {
            // Ustaw kontrolkę formularza jako aktywną zawartość
            MainContent.Content = new EvaluationControl();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Ustaw kontrolkę logowania jako aktywną zawartość
            MainContent.Content = new LoginControl();
        }
    }
}
