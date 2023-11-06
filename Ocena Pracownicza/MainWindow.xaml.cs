﻿using Ocena_Pracownicza.DataModels;
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
        public struct EvaluationRecordB
        {
            public EvaluationBiuro EvaluationB { get; set; }
            public override string ToString()
            {
                return EvaluationB.UserName;
            }
        }
        public struct EvaluationRecordP
        {
            public EvaluationProdukcja EvaluationP { get; set; }
            public override string ToString()
            {
                return EvaluationP.UserName;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            LoadAccounts();
            LoadEvaluationName();
            LoadComboBoxData(); 
            LoadDelatedUser();
        }
        private void LoadAccounts()
        {
            using (var context = new AppDbContext())
            {
                var accounts = context.Users
                                      .Where(user => user.FullName != "Administrator" && user.Enabled != false)
                                      .Select(user => user.FullName)
                                      .ToList();
                AccountsComboBoxP.ItemsSource = accounts;
                AccountsComboBoxB.ItemsSource = accounts;
                AccountsComboBoxDelete.ItemsSource = accounts;
                AccountsComboBoxResetPassword.ItemsSource = accounts;
                AccountsComboBoxAdd.ItemsSource = accounts;
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
        private void LoadComboBoxData()
        {
            using (var context = new AppDbContext())
            {
                var evaluatorNames = context.EvaluationNames.Select(en => en.EvaluatorName).ToList();

                // Dodaj opcję "Wszystkie" na początek listy
                evaluatorNames.Insert(0, "Wszystkie");

                EvaluationNameComboBox.ItemsSource = evaluatorNames;
                EvaluationNameComboBox.SelectedIndex = 0;
            }
        }
        private void LoadDelatedUser()
        {
            using (var context = new AppDbContext())
            {
                var accounts = context.Users
                                      .Where(user => user.FullName != "Administrator" && user.Enabled == false)
                                      .Select(user => user.FullName)
                                      .ToList();
                AccountsComboBoxRestore.ItemsSource = accounts;
            }
        }

        private void ClearTextBoxesInGrid()
        {
            NameTextBoxB.Text = string.Empty;
            NameTextBoxP.Text = string.Empty;
            AccountsComboBoxB.SelectedItem = null;
            AccountsComboBoxP.SelectedItem = null;
            for (int i = 1; i <= 11; i++)
            {
                var textBoxName = $"Question{i}TextBoxB";
                var textBox = this.FindName(textBoxName) as TextBox;
                if (textBox != null) 
                {
                    textBox.Text = string.Empty;
                }
            }
            for (int i = 1; i <= 4; i++)
            {
                var textBoxName = $"Question{i}TextBoxP";
                var textBox = this.FindName(textBoxName) as TextBox;
                if (textBox != null)
                {
                    textBox.Text = string.Empty;
                }
            }
            UsernameBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;
            //admin panel
            EvaluationNamesComboBox.SelectedItem = null; 
            AddEvaluationNameTextBox.Text = string.Empty;
            AddImieNazwisko.Text = string.Empty;
            AddLogin.Text = string.Empty;
            AddHaslo.Text = string.Empty;
        }

        private void SaveFormButtonB_Click(object sender, RoutedEventArgs e)
        {
            // 1. Walidacja wprowadzonych danych:
            if (string.IsNullOrEmpty(NameTextBoxB.Text) ||
                AccountsComboBoxB.SelectedItem == null ||
                string.IsNullOrEmpty(Question1TextBoxB.Text) ||
                string.IsNullOrEmpty(Question2TextBoxB.Text) ||
                string.IsNullOrEmpty(Question3TextBoxB.Text) ||
                string.IsNullOrEmpty(Question4TextBoxB.Text) ||
                string.IsNullOrEmpty(Question5TextBoxB.Text) ||
                string.IsNullOrEmpty(Question6TextBoxB.Text) ||
                string.IsNullOrEmpty(Question7TextBoxB.Text) ||
                string.IsNullOrEmpty(Question8TextBoxB.Text) ||
                string.IsNullOrEmpty(Question9TextBoxB.Text) ||
                string.IsNullOrEmpty(Question10TextBoxB.Text) ||
                string.IsNullOrEmpty(Question11TextBoxB.Text)
                )
            {
                MessageBox.Show("Wszystkie pola muszą być wypełnione!");
                return;
            }

            int? selectedUserID = null;
            string selectedUserName = AccountsComboBoxB.SelectedItem.ToString();
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

            var evaluation = new EvaluationBiuro
            {
                UserName = NameTextBoxB.Text,
                UserID = selectedUserID.Value,
                EvaluatorNameID = evaluatorNameID.Value,
                Date = DateTime.Now,
                Question1 = Question1TextBoxB.Text,
                Question2 = Question2TextBoxB.Text,
                Question3 = Question3TextBoxB.Text,
                Question4 = Question4TextBoxB.Text,
                Question5 = Question5TextBoxB.Text,
                Question6 = Question6TextBoxB.Text,
                Question7 = Question7TextBoxB.Text,
                Question8 = Question8TextBoxB.Text,
                Question9 = Question9TextBoxB.Text,
                Question10 = Question10TextBoxB.Text,
                Question11 = Question11TextBoxB.Text,
            };

            // 3. Zapisanie instancji w bazie danych:
            using (var context = new AppDbContext())
            {
                context.Evaluations.Add(evaluation);
                context.SaveChanges();
            }
            MessageBox.Show("Ankieta została pomyślnie zapisana!");
        }
        private void SaveFormButtonP_Click(object sender, RoutedEventArgs e)
        {
            // 1. Walidacja wprowadzonych danych:
            if (string.IsNullOrEmpty(NameTextBoxP.Text) ||
                AccountsComboBoxP.SelectedItem == null ||
                string.IsNullOrEmpty(Question1TextBoxP.Text) ||
                string.IsNullOrEmpty(Question2TextBoxP.Text) ||
                string.IsNullOrEmpty(Question3TextBoxP.Text) ||
                string.IsNullOrEmpty(Question4TextBoxP.Text)
                )
            {
                MessageBox.Show("Wszystkie pola muszą być wypełnione!");
                return;
            }

            int? selectedUserID = null;
            string selectedUserName = AccountsComboBoxP.SelectedItem.ToString();
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

            var evaluation = new EvaluationProdukcja
            {
                UserName = NameTextBoxP.Text,
                UserID = selectedUserID.Value,
                EvaluatorNameID = evaluatorNameID.Value,
                Date = DateTime.Now,
                Question1 = Question1TextBoxP.Text,
                Question2 = Question2TextBoxP.Text,
                Question3 = Question3TextBoxP.Text,
                Question4 = Question4TextBoxP.Text
            };

            // 3. Zapisanie instancji w bazie danych:
            using (var context = new AppDbContext())
            {
                context.EvaluationsProdukcja.Add(evaluation);
                context.SaveChanges();
            }
            MessageBox.Show("Ankieta została pomyślnie zapisana!");
        }
        private void FormBiuroButton_Click(object sender, RoutedEventArgs e)
        {
            FormPanelBiuro.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
            MenuPanel.Visibility = Visibility.Collapsed;
            Login.Visibility = Visibility.Collapsed;
        }
        private void FormProdukcjaButton_Click(object sender, RoutedEventArgs e)
        {
            FormPanelProdukcja.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
            MenuPanel.Visibility = Visibility.Collapsed;
            Login.Visibility = Visibility.Collapsed;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
            Login.Visibility = Visibility.Collapsed;
            MenuPanel.Visibility = Visibility.Collapsed;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MenuPanel.Visibility = Visibility.Visible;
            Login.Visibility = Visibility.Visible;
            LoginPanel.Visibility = Visibility.Collapsed;
            FormPanelBiuro.Visibility = Visibility.Collapsed;
            FormPanelProdukcja.Visibility= Visibility.Collapsed;
            BackButton.Visibility = Visibility.Collapsed;
            ClearTextBoxesInGrid();
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MenuPanel.Visibility = Visibility.Visible;
            UserPanel.Visibility = Visibility.Collapsed;
            AdminPanel.Visibility = Visibility.Collapsed;
            Login.Visibility = Visibility.Visible;
            ClearTextBoxesInGrid();
        }
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            UserPanel.Visibility = Visibility.Visible;
            EvaluationDetailsGridB.Visibility = Visibility.Collapsed;
            EvaluationDetailsGridP.Visibility = Visibility.Collapsed;
            ClearTextBoxesInGrid();
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
                        var evaluationsB = context.Evaluations
                          .Where(e => e.UserID == LoggedUser.UserID)
                          .Select(e => new EvaluationRecordB { EvaluationB = e })
                          .ToList();
                        UserEvaluationsBListView.ItemsSource = evaluationsB;

                        var evaluationsP = context.EvaluationsProdukcja
                                                  .Where(e => e.UserID == LoggedUser.UserID)
                                                  .Select(e => new EvaluationRecordP { EvaluationP = e })
                                                  .ToList();

                        UserEvaluationsPListView.ItemsSource = evaluationsP;
                    }
                    //MessageBox.Show("Prawidłowe Hasło!");
                    Powitanie.Text = $"Witaj, {LoggedUser.FullName}!";
                    LoginPanel.Visibility = Visibility.Collapsed;
                    BackButton.Visibility = Visibility.Collapsed;
                    UserPanel.Visibility = Visibility.Visible;

                    List<User> subordinates;
                    using (var context = new AppDbContext())
                    {
                        subordinates = context.Users
                                              .Where(u => u.ManagerId == LoggedUser.UserID)
                                              .ToList();
                    }
                    List<EvaluationRecordB> allEvaluationsB = new List<EvaluationRecordB>();
                    List<EvaluationRecordP> allEvaluationsP = new List<EvaluationRecordP>();

                    foreach (var subordinate in subordinates)
                    {
                        using (var context = new AppDbContext())
                        {
                            var evaluationsB = context.Evaluations
                                                      .Where(e => e.UserID == subordinate.UserID)
                                                      .Select(e => new EvaluationRecordB { EvaluationB = e })
                                                      .ToList();
                            allEvaluationsB.AddRange(evaluationsB);

                            var evaluationsP = context.EvaluationsProdukcja
                                                      .Where(e => e.UserID == subordinate.UserID)
                                                      .Select(e => new EvaluationRecordP { EvaluationP = e })
                                                      .ToList();
                            allEvaluationsP.AddRange(evaluationsP);
                        }
                    }
                    UserEvaluationsBListViewAll.ItemsSource = allEvaluationsB;
                    UserEvaluationsPListViewAll.ItemsSource = allEvaluationsP;
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
                if (user != null && user.Password == password && user.Enabled != false)
                {
                    LoggedUser = user;
                    return true;
                }
            }
            return false;
        }

        private void UserEvaluationsBListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UserEvaluationsBListView.SelectedItem is EvaluationRecordB selectedEvaluationRecord)
            {
                var selectedEvaluation = selectedEvaluationRecord.EvaluationB;
                UserPanel.Visibility = Visibility.Collapsed;

                Question1AnswerB.Text = selectedEvaluation.Question1;
                Question2AnswerB.Text = selectedEvaluation.Question2;
                Question3AnswerB.Text = selectedEvaluation.Question3;
                Question4AnswerB.Text = selectedEvaluation.Question4;
                Question5AnswerB.Text = selectedEvaluation.Question5;
                Question6AnswerB.Text = selectedEvaluation.Question6;
                Question7AnswerB.Text = selectedEvaluation.Question7;
                Question8AnswerB.Text = selectedEvaluation.Question8;
                Question9AnswerB.Text = selectedEvaluation.Question9;
                Question10AnswerB.Text = selectedEvaluation.Question10;
                Question11AnswerB.Text = selectedEvaluation.Question11;

                EvaluationDetailsGridB.Visibility = Visibility.Visible;
            }
        }
        private void UserEvaluationsPListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UserEvaluationsPListView.SelectedItem is EvaluationRecordP selectedEvaluationRecord)
            {
                var selectedEvaluation = selectedEvaluationRecord.EvaluationP;
                UserPanel.Visibility = Visibility.Collapsed;

                Question1AnswerP.Text = selectedEvaluation.Question1;
                Question2AnswerP.Text = selectedEvaluation.Question2;
                Question3AnswerP.Text = selectedEvaluation.Question3;
                Question4AnswerP.Text = selectedEvaluation.Question4;

                EvaluationDetailsGridP.Visibility = Visibility.Visible;
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
            
            using (var context = new AppDbContext())
            {
                string? selectedManagerName = AccountsComboBoxAdd.SelectedItem.ToString();

                var manager = context.Users.FirstOrDefault(u => u.FullName == selectedManagerName);

                var user = new User
                {
                    FullName = AddImieNazwisko.Text,
                    Login = AddLogin.Text,
                    Password = AddHaslo.Text,
                    Enabled = true,
                    ManagerId = manager.UserID,
                };

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


        private void SearchAndFilterEvaluations()
        {
            string searchText = Search.Text.ToLower();

            string selectedEvaluationName = EvaluationNameComboBox.SelectedItem?.ToString();
            int? selectedEvaluatorNameID = null;

            using (var context = new AppDbContext())
            {
                if (selectedEvaluationName != null && selectedEvaluationName != "Wszystkie")
                {
                    // Pobierz ID wybranej nazwy oceny.
                    selectedEvaluatorNameID = context.EvaluationNames
                                                     .Where(en => en.EvaluatorName == selectedEvaluationName)
                                                     .Select(en => (int?)en.EvaluatorNameID)
                                                     .FirstOrDefault();

                    if (!selectedEvaluatorNameID.HasValue)
                    {
                        MessageBox.Show("Wybrana nazwa oceny nie istnieje w bazie danych!");
                        return;
                    }
                }

                // Filtruj oceny według wyszukiwanego tekstu oraz wybranego ID (jeśli jest wybrane).
                IQueryable<EvaluationBiuro> evaluationsQueryB = context.Evaluations.Where(ev => ev.UserName.ToLower().Contains(searchText));
                IQueryable<EvaluationProdukcja> evaluationsQueryP = context.EvaluationsProdukcja.Where(ev => ev.UserName.ToLower().Contains(searchText));

                if (selectedEvaluatorNameID.HasValue)
                {
                    evaluationsQueryB = evaluationsQueryB.Where(ev => ev.EvaluatorNameID == selectedEvaluatorNameID.Value);
                    evaluationsQueryP = evaluationsQueryP.Where(ev => ev.EvaluatorNameID == selectedEvaluatorNameID.Value);
                }

                if (LoggedUser != null)
                {
                    evaluationsQueryB = evaluationsQueryB.Where(ev => ev.UserID == LoggedUser.UserID);
                    evaluationsQueryP = evaluationsQueryP.Where(ev => ev.UserID == LoggedUser.UserID);
                }


                var filteredEvaluationsB = evaluationsQueryB
                                          .Select(ev => new EvaluationRecordB { EvaluationB = ev })
                                          .ToList();
                var filteredEvaluationsP = evaluationsQueryP
                                          .Select(ev => new EvaluationRecordP { EvaluationP = ev })
                                          .ToList();

                UserEvaluationsBListView.ItemsSource = filteredEvaluationsB;
                UserEvaluationsPListView.ItemsSource = filteredEvaluationsP;
            }
        }


        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchAndFilterEvaluations();
        }

        private void EvaluationNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchAndFilterEvaluations();
        }

        private void PrintButtonB_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(TestDrukB, "Wydruk z aplikacji WPF");
            }
        }
        private void PrintButtonP_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(TestDrukP, "Wydruk z aplikacji WPF");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AccountsDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Odczytaj wartość z ComboBox
            var selectedUserName = AccountsComboBoxDelete.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedUserName))
            {
                MessageBox.Show("Wybierz użytkownika z listy!");
                return;
            }

            using (var context = new AppDbContext())
            {
                // 2. Znajdź użytkownika o wybranej nazwie
                var user = context.Users.FirstOrDefault(u => u.FullName == selectedUserName);

                if (user == null)
                {
                    MessageBox.Show("Nie znaleziono użytkownika!");
                    return;
                }

                // 3. Zmień wartość Enabled na 0
                user.Enabled = false;

                // 4. Zapisz zmiany w bazie danych
                context.SaveChanges();
            }
            MessageBox.Show($"Użytkownik {selectedUserName} został dezaktywowany!");
            LoadAccounts();
            LoadDelatedUser();
        }
        private void AccountsRestoryButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUserName = AccountsComboBoxRestore.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedUserName))
            {
                MessageBox.Show("Wybierz użytkownika z listy!");
                return;
            }

            using (var context = new AppDbContext())
            {
                // 2. Znajdź użytkownika o wybranej nazwie
                var user = context.Users.FirstOrDefault(u => u.FullName == selectedUserName);

                if (user == null)
                {
                    MessageBox.Show("Nie znaleziono użytkownika!");
                    return;
                }

                // 3. Zmień wartość Enabled na 0
                user.Enabled = true;

                // 4. Zapisz zmiany w bazie danych
                context.SaveChanges();
            }
            MessageBox.Show($"Użytkownik {selectedUserName} został aktywowany!");
            LoadAccounts();
            LoadDelatedUser();
        }

        private void AccountsResetButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUserName = AccountsComboBoxResetPassword.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedUserName))
            {
                MessageBox.Show("Wybierz użytkownika z listy!");
                return;
            }

            using (var context = new AppDbContext())
            {
                // 2. Znajdź użytkownika o wybranej nazwie
                var user = context.Users.FirstOrDefault(u => u.FullName == selectedUserName);

                if (user == null)
                {
                    MessageBox.Show("Nie znaleziono użytkownika!");
                    return;
                }

                // 3. Zmień wartość Enabled na 0
                user.Password = ResetPassword.Text;

                // 4. Zapisz zmiany w bazie danych
                context.SaveChanges();
            }
            MessageBox.Show($"Hasło użytkownika {selectedUserName} zostało zmienione!");
        }
    }
}
