using Ocena_Pracownicza.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            public Evaluation Evaluation { get; set; }
            public override string ToString()
            {
                return Evaluation.UserName;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            LoadAccounts();
            LoadEvaluationName();
        }
        private void LoadAccounts()
        {
            using (var context = new AppDbContext())
            {
                var accounts = context.Users.Select(user => user.FullName).ToList();
                AccountsComboBox.ItemsSource = accounts;
            }
        }
        private void LoadEvaluationName()
        {
            using (var context = new AppDbContext())
            {
                var evaluationNames = context.EvaluationNames.Select(e => e.EvaluatorName).ToList();
                EvaluationNamesComboBox.ItemsSource = evaluationNames;
                EvaluationNameComboBox.ItemsSource = evaluationNames;
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
            string currentEvaluationName;
            using (var context = new AppDbContext())
            {
                currentEvaluationName = context.GlobalSettings.FirstOrDefault()?.CurrentEvaluationName;
            }
            int? evaluatorNameID;
            using (var context = new AppDbContext())
            {
                evaluatorNameID = context.EvaluationNames
                                         .Where(en => en.EvaluatorName == currentEvaluationName)
                                         .Select(en => (int?)en.EvaluatorNameID)
                                         .FirstOrDefault();
            }
            if (!evaluatorNameID.HasValue)
            {
                MessageBox.Show("Nie znaleziono nazwy oceny w bazie danych!");
                return;
            }

            var evaluation = new Evaluation
            {
                UserName = NameTextBox.Text,
                UserID = selectedUserID.Value,
                EvaluatorNameID = evaluatorNameID.Value,
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
            AdminPanel.Visibility = Visibility.Collapsed;
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
                if (username.ToLower() == "admin")
                {
                    LoginPanel.Visibility = Visibility.Collapsed;
                    BackButton.Visibility = Visibility.Collapsed;
                    AdminPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    using (var context = new AppDbContext())
                    {
                        var evaluations = context.Evaluations
                                                 .Where(e => e.UserID == LoggedUser.UserID)
                                                 .Select(e => new EvaluationRecord { Evaluation = e } )
                                                 .ToList();
                        UserEvaluationsListView.ItemsSource = evaluations;
                    }
                    //MessageBox.Show("Prawidłowe Hasło!");
                    Powitanie.Text = $"Witaj, {LoggedUser.FullName}!";
                    LoginPanel.Visibility = Visibility.Collapsed;
                    BackButton.Visibility = Visibility.Collapsed;
                    UserPanel.Visibility = Visibility.Visible;
                }
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
            if (UserEvaluationsListView.SelectedItem is EvaluationRecord selectedEvaluationRecord)
            {
                var selectedEvaluation = selectedEvaluationRecord.Evaluation;
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
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = Search.Text.ToLower();
            using (var context = new AppDbContext())
            {
                var filteredEvaluations = context.Evaluations
                    .Where(ev => ev.UserName.ToLower().Contains(searchText))
                    .Select(ev => new EvaluationRecord { Evaluation = ev })
                    .ToList();

                UserEvaluationsListView.ItemsSource = filteredEvaluations;
            }
        }

        private void AddNewEvaluationName_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AddEvaluationNameTextBox.Text))
            {
                MessageBox.Show("Podaj nazwe!");
                return;
            }

            var evaluationName = new EvaluationName
            {
                EvaluatorName = AddEvaluationNameTextBox.Text
            };

            using (var context = new AppDbContext())
            {
                context.EvaluationNames.Add(evaluationName);
                context.SaveChanges();
            }
            LoadEvaluationName();
            MessageBox.Show($"Utworzono: {AddEvaluationNameTextBox.Text}");
        }
        private void AddNewUser_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AddImieNazwisko.Text)|| string.IsNullOrEmpty(AddLogin.Text) || string.IsNullOrEmpty(AddHaslo.Text))
            {
                MessageBox.Show("Uzupełnij pola!");
                return;
            }

            var user = new User
            {
                FullName = AddImieNazwisko.Text,
                Login = AddLogin.Text,
                Password = AddHaslo.Text,
            };

            using (var context = new AppDbContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
            LoadAccounts();
            MessageBox.Show($"Utworzono: {AddImieNazwisko.Text}");
        }

        private void EvaluationNamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EvaluationNamesComboBox.SelectedItem != null)
            {
                string selectedEvaluationName = EvaluationNamesComboBox.SelectedItem.ToString();

                // Zaktualizuj wartość w GlobalSettings:
                using (var context = new AppDbContext())
                {
                    var globalSetting = context.GlobalSettings.FirstOrDefault();

                    // Jeśli nie istnieje rekord w GlobalSettings, to stwórz nowy:
                    if (globalSetting == null)
                    {
                        globalSetting = new GlobalSettings();
                        context.GlobalSettings.Add(globalSetting);
                    }

                    globalSetting.CurrentEvaluationName = selectedEvaluationName;
                    context.SaveChanges();
                }
            }
        }

        private void EvaluationNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EvaluationNameComboBox.SelectedItem == null)
                return;

            string selectedEvaluationName = EvaluationNameComboBox.SelectedItem.ToString();

            using (var context = new AppDbContext())
            {
                // Pobierz ID wybranej nazwy oceny.
                int? selectedEvaluatorNameID = context.EvaluationNames
                                                      .Where(en => en.EvaluatorName == selectedEvaluationName)
                                                      .Select(en => (int?)en.EvaluatorNameID)
                                                      .FirstOrDefault();

                if (!selectedEvaluatorNameID.HasValue)
                {
                    MessageBox.Show("Wybrana nazwa oceny nie istnieje w bazie danych!");
                    return;
                }

                // Filtruj oceny według wybranego ID i wyświetl je w ListView.
                var filteredEvaluations = context.Evaluations
                                                 .Where(ev => ev.EvaluatorNameID == selectedEvaluatorNameID.Value)
                                                 .Select(ev => new EvaluationRecord { Evaluation = ev })
                                                 .ToList();

                UserEvaluationsListView.ItemsSource = filteredEvaluations;
            }
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                // Jeśli masz konkretny Grid, który chcesz wydrukować, zmień nazwę 'YourGridName' na odpowiednią nazwę.
                printDialog.PrintVisual(TestDruk, "Wydruk z aplikacji WPF");
            }
        }
    }
}
