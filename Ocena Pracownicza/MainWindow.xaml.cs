using Ocena_Pracownicza.DataModels;
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
            LoadAccounts();
        }
        private void LoadAccounts()
        {
            using (var context = new AppDbContext())
            {
                var accounts = context.Users.Select(user => user.FullName).ToList();
                AccountsComboBox.ItemsSource = accounts;
            }
        }
        private void FormButton_Click(object sender, RoutedEventArgs e)
        {
            MenuPanel.Visibility = Visibility.Collapsed;
            FormPanel.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
        }
        private void SaveFormButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Walidacja wprowadzonych danych:
            if (string.IsNullOrEmpty(NameTextBox.Text) ||
                AccountsComboBox.SelectedItem == null ||
                string.IsNullOrEmpty(Question1TextBox.Text) ||
                string.IsNullOrEmpty(Question2TextBox.Text) ||
                string.IsNullOrEmpty(Question3TextBox.Text) ||
                string.IsNullOrEmpty(Question4TextBox.Text) ||
                string.IsNullOrEmpty(Question5TextBox.Text) ||
                string.IsNullOrEmpty(Question6TextBox.Text)
                )
            {
                MessageBox.Show("Wszystkie pola muszą być wypełnione!");
                return;
            }

            int? selectedUserID = null;
            string selectedUserName = AccountsComboBox.SelectedItem.ToString();
            using (var context = new AppDbContext())
            {
                selectedUserID = context.Users
                            .Where(u => u.FullName == selectedUserName)
                            .Select(u => (int?)u.UserID) // zwracanie UserID jako nullable int
                            .FirstOrDefault();

                if (!selectedUserID.HasValue) // jeśli nie znaleziono UserID
                {
                    MessageBox.Show("Wybrany użytkownik nie istnieje w bazie danych!");
                    return;
                }
            }

            // 2. Stworzenie instancji klasy Evaluation i przypisanie jej wartości:
            var evaluation = new Evaluation
            {
                UserName = NameTextBox.Text,
                UserID = selectedUserID.Value,
                EvaluatorName = "TestowaAnkieta",
                Date = DateTime.Now,
                Question1 = Question1TextBox.Text,
                Question2 = Question2TextBox.Text,
                Question3 = Question2TextBox.Text,
                Question4 = Question2TextBox.Text,
                Question5 = Question2TextBox.Text,
                Question6 = Question2TextBox.Text
            };

            // 3. Zapisanie instancji w bazie danych:
            using (var context = new AppDbContext())
            {
                context.Evaluations.Add(evaluation);
                context.SaveChanges();
            }

            MessageBox.Show("Ankieta została pomyślnie zapisana!");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            MenuPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MenuPanel.Visibility = Visibility.Visible;
            LoginPanel.Visibility = Visibility.Collapsed;
            FormPanel.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Collapsed;
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserPanel.Visibility = Visibility.Collapsed;
            MenuPanel.Visibility = Visibility.Visible;
        }
        private void OnLoginAttempt(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password; // Używaj Password dla PasswordBox

            if (ValidateLogin(username, password))
            {
                MessageBox.Show("Prawidłowe Hasło!");
                LoginPanel.Visibility = Visibility.Collapsed;
                BackButton.Visibility = Visibility.Collapsed;
                UserPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Nieprawidłowy login lub hasło!");
            }
        }

        private bool ValidateLogin(string username, string password)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Login == username);
                if (user != null && user.Password == password)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
