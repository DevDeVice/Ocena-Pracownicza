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
        public User LoggedUser { get; set; }
        public struct EvaluationRecord
        {
            public Evaluation evaluation;
            public override string ToString()
            {
                return evaluation.UserName;
            }
        }
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
                Question3 = Question3TextBox.Text,
                Question4 = Question4TextBox.Text,
                Question5 = Question5TextBox.Text,
                Question6 = Question6TextBox.Text
            };

            // 3. Zapisanie instancji w bazie danych:
            using (var context = new AppDbContext())
            {
                context.Evaluations.Add(evaluation);
                context.SaveChanges();
            }

            MessageBox.Show("Ankieta została pomyślnie zapisana!");
        }
        private void FormButton_Click(object sender, RoutedEventArgs e)
        {
            FormPanel.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
            MenuPanel.Visibility = Visibility.Collapsed;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
            MenuPanel.Visibility = Visibility.Collapsed;
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
            MenuPanel.Visibility = Visibility.Visible;
            UserPanel.Visibility = Visibility.Collapsed;
        }
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            UserPanel.Visibility = Visibility.Visible;
            EvaluationDetailsGrid.Visibility = Visibility.Collapsed;
        }
        private void OnLoginAttempt(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password; // Używaj Password dla PasswordBox

            if (ValidateLogin(username, password))
            {
                using (var context = new AppDbContext())
                {
                    var evaluations = context.Evaluations
                                             .Where(e => e.UserID == LoggedUser.UserID)
                                             .Select(e => new EvaluationRecord { evaluation = e } )
                                             .ToList();
                    UserEvaluationsListView.ItemsSource = evaluations;
                }
                //MessageBox.Show("Prawidłowe Hasło!");
                Powitanie.Text = $"Witaj, {LoggedUser.FullName}!";
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
                    LoggedUser = user;
                    return true;
                }
            }
            return false;
        }

        private void UserEvaluationsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(UserEvaluationsListView.SelectedItem.ToString());
            if (UserEvaluationsListView.SelectedItem is EvaluationRecord selectedEvaluationRecord)
            {
                var selectedEvaluation = selectedEvaluationRecord.evaluation;
                UserPanel.Visibility = Visibility.Collapsed;

                Question1Answer.Text = selectedEvaluation.Question1;
                Question2Answer.Text = selectedEvaluation.Question2;
                Question3Answer.Text = selectedEvaluation.Question3;
                Question4Answer.Text = selectedEvaluation.Question4;
                Question5Answer.Text = selectedEvaluation.Question5;
                Question6Answer.Text = selectedEvaluation.Question6;

                EvaluationDetailsGrid.Visibility = Visibility.Visible;
            }
        }
    }
}
