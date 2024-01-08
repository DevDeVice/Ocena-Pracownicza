using Ocena_Pracownicza.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BCrypt.Net;

namespace Ocena_Pracownicza
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User LoggedUser { get; set; }
        public int FormVersion { get; set; }
        public int EvaluationIDToAnswer { get; set; }
        public EvaluationBiuro historyEvaluationB {get;set;}
        public int? historyAnswerBID { get; set; }
        public EvaluationProdukcja historyEvaluationP {get;set;}
        public int? historyAnswerPID { get; set; }
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
            LoggedUser= new User();
            LoggedUser.UserID = 0;
            ConnectionCheck();
            InitializeComponent();
            LoadAccounts();
            LoadEvaluationName();
            LoadComboBoxData(); 
            LoadDelatedUser();
        }
        private void ConnectionCheck()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var data = context.EvaluationBiuro.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie udało się połączyć z bazą danych: {ex.Message}", "Błąd Połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                AccountsComboBoxToChangeManager.ItemsSource = accounts;
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
            for (int i = 1; i <= 5; i++)
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
            if (FormVersion == 2)
            {
                if (
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
                
                var evaluation = new EvaluationBiuroAnswer
                {
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
                using (var context = new AppDbContext())
                {
                    context.EvaluationBiuroAnswers.Add(evaluation);
                    context.SaveChanges();

                    var user = context.EvaluationBiuro.FirstOrDefault(u => u.EvaluationID == EvaluationIDToAnswer);
                    if (user != null)
                    {
                        user.EvaluationAnswerID = evaluation.EvaluationID;
                        OdpowiedzB.Content = "Sprawdz odpowiedz";
                        historyAnswerBID = user.EvaluationAnswerID;
                        context.SaveChanges();
                        MessageBox.Show("Ankieta została pomyślnie zapisana!");
                    }
                }
            }
            else
            {
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
                    EvaluationAnswerID = 0,
                };
                using (var context = new AppDbContext())
                {
                    context.EvaluationBiuro.Add(evaluation);
                    context.SaveChanges();
                }
                MessageBox.Show("Ankieta została pomyślnie zapisana!");
            }            
        }
        private void SaveFormButtonP_Click(object sender, RoutedEventArgs e)
        {
            if (FormVersion == 2)
            {
                if (string.IsNullOrEmpty(Question1TextBoxP.Text) ||
                 string.IsNullOrEmpty(Question2TextBoxP.Text) ||
                 string.IsNullOrEmpty(Question3TextBoxP.Text) ||
                 string.IsNullOrEmpty(Question4TextBoxP.Text)
                 )
                {
                    MessageBox.Show("Wszystkie pola muszą być wypełnione!");
                    return;
                }
                var evaluation = new EvaluationProdukcjaAnswer
                {
                    Question1 = Question1TextBoxP.Text,
                    Question2 = Question2TextBoxP.Text,
                    Question3 = Question3TextBoxP.Text,
                    Question4 = Question4TextBoxP.Text,
                    Question5 = Question5TextBoxP.Text,
                };

                // 3. Zapisanie instancji w bazie danych:
                using (var context = new AppDbContext())
                {
                    context.EvaluationProdukcjaAnswers.Add(evaluation);
                    context.SaveChanges();

                    var user = context.EvaluationsProdukcja.FirstOrDefault(u => u.EvaluationID == EvaluationIDToAnswer);
                    if(user != null)
                    {
                        user.EvaluationAnswerID = evaluation.EvaluationID;
                        OdpowiedzP.Content = "Sprawdz odpowiedz";
                        historyAnswerPID = user.EvaluationAnswerID;
                        MessageBox.Show("Ankieta została pomyślnie zapisana!");
                        context.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Nie znaleziono uzytkownika $");
                    }
                }
                
            } 
            else
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
                    Question4 = Question4TextBoxP.Text,
                    Question5 = Question5TextBoxP.Text,
                    EvaluationAnswerID = 0,
                };

                // 3. Zapisanie instancji w bazie danych:
                using (var context = new AppDbContext())
                {
                    context.EvaluationsProdukcja.Add(evaluation);
                    context.SaveChanges();
                }
                MessageBox.Show("Ankieta została pomyślnie zapisana!");
            }
        }
        private void FormBiuroButton_Click(object sender, RoutedEventArgs e)
        {
            FormVersion = 1;
            ToHideB1.Visibility = Visibility.Visible;
            ToHideB2.Visibility = Visibility.Visible;
            FormPanelBiuro.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
            MenuPanel.Visibility = Visibility.Collapsed;
            Login.Visibility = Visibility.Collapsed;
        }
        private void FormProdukcjaButton_Click(object sender, RoutedEventArgs e)
        {
            FormVersion = 1;
            ToHideP1.Visibility = Visibility.Visible;
            ToHideP2.Visibility = Visibility.Visible; 
            FormPanelProdukcja.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
            MenuPanel.Visibility = Visibility.Collapsed; 
            Login.Visibility = Visibility.Collapsed;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            FormVersion = 2;
            ToHideB1.Visibility = Visibility.Collapsed;
            ToHideB2.Visibility = Visibility.Collapsed;
            ToHideP1.Visibility = Visibility.Collapsed;
            ToHideP2.Visibility = Visibility.Collapsed;
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


        private bool ValidateLogin(string username, string password)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Login == username);

                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    LoggedUser = user; // Załóżmy, że istnieje globalna zmienna LoggedUser do przechowywania zalogowanego użytkownika
                    return true;
                }
            }
            return false;
        }

        /* Wersja bez zapisywania
        private void UserEvaluationsBListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UserEvaluationsBListView.SelectedItem is EvaluationRecordB selectedEvaluationRecord)
            {
                var selectedEvaluation = selectedEvaluationRecord.EvaluationB;
                EvaluationIDToAnswer = selectedEvaluation.EvaluationID;

                if (selectedEvaluation.EvaluationAnswerID > 1)
                {
                    OdpowiedzB.Content = "Sprawdz odpowiedz";
                }
                else
                {
                    OdpowiedzB.Content = "Odpowiedz";
                }

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
        }*/

        private void UserEvaluationsBListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UserEvaluationsBListView.SelectedItem is EvaluationRecordB selectedEvaluationRecord)
            {
                var selectedEvaluation = selectedEvaluationRecord.EvaluationB;
                historyEvaluationB = selectedEvaluation;  // Przypisanie do historyEvaluationB
                historyAnswerBID = selectedEvaluation.EvaluationAnswerID;

                EvaluationIDToAnswer = selectedEvaluation.EvaluationID;

                if (selectedEvaluation.EvaluationAnswerID > 1)
                {
                    OdpowiedzB.Content = "Sprawdz odpowiedz";
                }
                else
                {
                    OdpowiedzB.Content = "Odpowiedz";
                }

                UserPanel.Visibility = Visibility.Collapsed;
                Question0AnswerB.Text = selectedEvaluation.UserName;
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

        private void UserEvaluationsBListViewAll_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (UserEvaluationsBListViewAll.SelectedItem is EvaluationRecordB selectedEvaluationRecord)
            {
                var selectedEvaluation = selectedEvaluationRecord.EvaluationB;
                historyEvaluationB = selectedEvaluation;
                historyAnswerBID = selectedEvaluation.EvaluationAnswerID;

                EvaluationIDToAnswer = selectedEvaluation.EvaluationID;

                if (selectedEvaluation.EvaluationAnswerID > 1)
                {
                    OdpowiedzB.Content = "Sprawdz odpowiedz";
                }
                else
                {
                    OdpowiedzB.Content = "Odpowiedz";
                }

                UserPanel.Visibility = Visibility.Collapsed;

                Question0AnswerB.Text = selectedEvaluation.UserName;
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
                historyEvaluationP = selectedEvaluation;
                historyAnswerPID = selectedEvaluation.EvaluationAnswerID;

                EvaluationIDToAnswer = selectedEvaluation.EvaluationID;

                if (selectedEvaluation.EvaluationAnswerID > 1)
                {
                    OdpowiedzP.Content = "Sprawdz odpowiedz";
                }
                else
                {
                    OdpowiedzP.Content = "Odpowiedz";
                }
                UserPanel.Visibility = Visibility.Collapsed;

                Question0AnswerP.Text = selectedEvaluation.UserName;
                Question1AnswerP.Text = selectedEvaluation.Question1;
                Question2AnswerP.Text = selectedEvaluation.Question2;
                Question3AnswerP.Text = selectedEvaluation.Question3;
                Question4AnswerP.Text = selectedEvaluation.Question4;
                Question5AnswerP.Text = selectedEvaluation.Question5;

                EvaluationDetailsGridP.Visibility = Visibility.Visible;
            }
        }
        private void UserEvaluationsPListViewAll_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UserEvaluationsPListViewAll.SelectedItem is EvaluationRecordP selectedEvaluationRecord)
            {
                var selectedEvaluation = selectedEvaluationRecord.EvaluationP;
                historyEvaluationP = selectedEvaluation;
                historyAnswerPID = selectedEvaluation.EvaluationAnswerID;

                EvaluationIDToAnswer = selectedEvaluation.EvaluationID;

                if (selectedEvaluation.EvaluationAnswerID > 1)
                {
                    OdpowiedzP.Content = "Sprawdz odpowiedz";
                }
                else
                {
                    OdpowiedzP.Content = "Odpowiedz";
                }
                UserPanel.Visibility = Visibility.Collapsed;

                Question0AnswerP.Text = selectedEvaluation.UserName;
                Question1AnswerP.Text = selectedEvaluation.Question1;
                Question2AnswerP.Text = selectedEvaluation.Question2;
                Question3AnswerP.Text = selectedEvaluation.Question3;
                Question4AnswerP.Text = selectedEvaluation.Question4;
                Question5AnswerP.Text = selectedEvaluation.Question5;

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
            if (string.IsNullOrEmpty(AddImieNazwisko.Text) || string.IsNullOrEmpty(AddLogin.Text) || string.IsNullOrEmpty(AddHaslo.Text))
            {
                MessageBox.Show("Uzupełnij pola!");
                return;
            }

            using (var context = new AppDbContext())
            {
                bool loginExists = context.Users.Any(u => u.Login == AddLogin.Text);

                if (loginExists)
                {
                    MessageBox.Show("Użytkownik o takim loginie już istnieje.");
                    return;
                }

                string? selectedManagerName = AccountsComboBoxAdd.SelectedItem?.ToString();

                User? manager = null;
                if (!string.IsNullOrEmpty(selectedManagerName))
                {
                    manager = context.Users.FirstOrDefault(u => u.FullName == selectedManagerName);
                }

                // Haszowanie hasła przed zapisaniem do bazy
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(AddHaslo.Text);

                var user = new User
                {
                    FullName = AddImieNazwisko.Text,
                    Login = AddLogin.Text,
                    Password = passwordHash, // Zapisujemy haszowane hasło
                    ManagerId = manager?.UserID,
                    Enabled = true
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
                        var evaluationsB = context.EvaluationBiuro
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

                    FetchEvaluationsAndSubordinates(LoggedUser.UserID, allEvaluationsB, allEvaluationsP);

                    UserEvaluationsBListViewAll.ItemsSource = allEvaluationsB;
                    UserEvaluationsPListViewAll.ItemsSource = allEvaluationsP;
                }
            }
            else
            {
                MessageBox.Show("Nieprawidłowy login lub hasło!");
            }
        }
        private void FetchEvaluationsAndSubordinates(int userId, List<EvaluationRecordB> allEvaluationsB, List<EvaluationRecordP> allEvaluationsP)
        {
            using (var context = new AppDbContext())
            {
                // Pobierz podwładnych użytkownika
                var subordinates = context.Users
                                          .Where(u => u.ManagerId == userId)
                                          .ToList();

                // Pobierz oceny tylko dla podwładnych
                foreach (var subordinate in subordinates)
                {
                    var evaluationsB = context.EvaluationBiuro
                                              .Where(e => e.UserID == subordinate.UserID)
                                              .Select(e => new EvaluationRecordB { EvaluationB = e })
                                              .ToList();
                    allEvaluationsB.AddRange(evaluationsB);

                    var evaluationsP = context.EvaluationsProdukcja
                                              .Where(e => e.UserID == subordinate.UserID)
                                              .Select(e => new EvaluationRecordP { EvaluationP = e })
                                              .ToList();
                    allEvaluationsP.AddRange(evaluationsP);

                    // Rekurencyjne wywołanie dla każdego podwładnego
                    FetchEvaluationsAndSubordinates(subordinate.UserID, allEvaluationsB, allEvaluationsP);
                }
            }
        }


        private void SearchAndFilterEvaluations()
        {
            if (LoggedUser == null)
            {
                MessageBox.Show("Brak zalogowanego użytkownika!");
                return;
            }


            string searchText = Search.Text.ToLower();
            string selectedEvaluationName = EvaluationNameComboBox.SelectedItem?.ToString();

            int? selectedEvaluatorNameID = null;

            using (var context = new AppDbContext())
            {
                if (selectedEvaluationName != null && selectedEvaluationName != "Wszystkie")
                {
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

                List<EvaluationRecordB> allEvaluationsB = new List<EvaluationRecordB>();
                List<EvaluationRecordP> allEvaluationsP = new List<EvaluationRecordP>();

                FilterEvaluationsForUserAndSubordinates(LoggedUser.UserID, searchText, selectedEvaluatorNameID, allEvaluationsB, allEvaluationsP);

                // Update the list views
                UserEvaluationsBListViewAll.ItemsSource = allEvaluationsB;
                UserEvaluationsPListViewAll.ItemsSource = allEvaluationsP;
            }
        }



        private void FilterEvaluationsForUserAndSubordinates(int userId, string searchText, int? evaluatorNameId, List<EvaluationRecordB> allEvaluationsB, List<EvaluationRecordP> allEvaluationsP)
        {
            using (var context = new AppDbContext())
            {
                var evaluationsQueryB = context.EvaluationBiuro
                                               .Where(ev => ev.UserID == userId && ev.UserName.ToLower().Contains(searchText));

                var evaluationsQueryP = context.EvaluationsProdukcja
                                               .Where(ev => ev.UserID == userId && ev.UserName.ToLower().Contains(searchText));

                if (evaluatorNameId.HasValue)
                {
                    evaluationsQueryB = evaluationsQueryB.Where(ev => ev.EvaluatorNameID == evaluatorNameId.Value);
                    evaluationsQueryP = evaluationsQueryP.Where(ev => ev.EvaluatorNameID == evaluatorNameId.Value);
                }

                var filteredEvaluationsB = evaluationsQueryB.Select(ev => new EvaluationRecordB { EvaluationB = ev }).ToList();
                var filteredEvaluationsP = evaluationsQueryP.Select(ev => new EvaluationRecordP { EvaluationP = ev }).ToList();

                if(userId == LoggedUser.UserID)
                {
                    UserEvaluationsBListView.ItemsSource = filteredEvaluationsB;
                    UserEvaluationsPListView.ItemsSource = filteredEvaluationsP;
                }
                else
                {
                    allEvaluationsB.AddRange(filteredEvaluationsB);
                    allEvaluationsP.AddRange(filteredEvaluationsP);
                }

                // Rekurencyjne wywołanie dla każdego podwładnego
                var subordinates = context.Users.Where(u => u.ManagerId == userId).ToList();
                foreach (var subordinate in subordinates)
                {
                    FilterEvaluationsForUserAndSubordinates(subordinate.UserID, searchText, evaluatorNameId, allEvaluationsB, allEvaluationsP);
                }
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

            if (string.IsNullOrEmpty(ResetPassword.Text))
            {
                MessageBox.Show("Wprowadź nowe hasło!");
                return;
            }

            using (var context = new AppDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.FullName == selectedUserName);

                if (user == null)
                {
                    MessageBox.Show("Nie znaleziono użytkownika!");
                    return;
                }

                // Hashowanie nowego hasła przed zapisaniem
                string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(ResetPassword.Text);
                user.Password = newHashedPassword;

                context.SaveChanges();
            }
            MessageBox.Show($"Hasło użytkownika {selectedUserName} zostało zmienione!");
        }

        private void AccountsChangeManager_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AccountsComboBoxToChangeManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AccountsComboBoxNewManager.SelectedItem = null;
            using (var context = new AppDbContext())
            {
                string selectedFullName = AccountsComboBoxToChangeManager.SelectedItem?.ToString();
                var accounts = context.Users
                                      .Where(user => user.FullName != "Administrator" && user.Enabled != false && user.FullName != selectedFullName)
                                      .Select(user => user.FullName)
                                      .ToList();
                AccountsComboBoxNewManager.ItemsSource = accounts;
            }
            
        }

        private void OdpowiedzP_Click(object sender, RoutedEventArgs e)
        {
            if (OdpowiedzP.Content.ToString() == "Sprawdz odpowiedz")
            {
                using (var context = new AppDbContext())
                {
                    var evaluationAnswer = context.EvaluationProdukcjaAnswers
                                                  .FirstOrDefault(ea => ea.EvaluationID == historyAnswerPID);

                    if (evaluationAnswer != null)
                    {
                        Question1AnswerP.Text = evaluationAnswer.Question1;
                        Question2AnswerP.Text = evaluationAnswer.Question2;
                        Question3AnswerP.Text = evaluationAnswer.Question3;
                        Question4AnswerP.Text = evaluationAnswer.Question4;
                        Question5AnswerP.Text = evaluationAnswer.Question5;
                        OdpowiedzP.Content = "Sprawdz pytanie";
                    }
                    else
                    {
                        MessageBox.Show("Brak danych odpowiedzi.");
                    }
                }
            }
            else if (OdpowiedzP.Content.ToString() == "Sprawdz pytanie")
            {
                Question1AnswerP.Text = historyEvaluationP.Question1;
                Question2AnswerP.Text = historyEvaluationP.Question2;
                Question3AnswerP.Text = historyEvaluationP.Question3;
                Question4AnswerP.Text = historyEvaluationP.Question4;
                Question5AnswerP.Text = historyEvaluationP.Question5;
                OdpowiedzP.Content = "Sprawdz odpowiedz";
            }
            else
            {
                FormPanelProdukcja.Visibility = Visibility.Visible;
                BackButton2.Visibility = Visibility.Visible;
                EvaluationDetailsGridP.Visibility = Visibility.Collapsed;
            } 
        }

        private void OdpowiedzB_Click(object sender, RoutedEventArgs e)
        {
            if (OdpowiedzB.Content.ToString() == "Sprawdz odpowiedz")
            {
                using (var context = new AppDbContext()) // Załóżmy, że AppDbContext to twoja klasa kontekstu EF
                {
                    var evaluationAnswer = context.EvaluationBiuroAnswers
                                                  .FirstOrDefault(ea => ea.EvaluationID == historyAnswerBID);

                    if (evaluationAnswer != null)
                    {
                        Question1AnswerB.Text = evaluationAnswer.Question1;
                        Question2AnswerB.Text = evaluationAnswer.Question2;
                        Question3AnswerB.Text = evaluationAnswer.Question3;
                        Question4AnswerB.Text = evaluationAnswer.Question4; 
                        Question5AnswerB.Text = evaluationAnswer.Question5;
                        Question6AnswerB.Text = evaluationAnswer.Question6;
                        Question7AnswerB.Text = evaluationAnswer.Question7;
                        Question8AnswerB.Text = evaluationAnswer.Question8;
                        Question9AnswerB.Text = evaluationAnswer.Question9;
                        Question10AnswerB.Text = evaluationAnswer.Question10;
                        Question11AnswerB.Text = evaluationAnswer.Question11;
                        OdpowiedzB.Content = "Sprawdz pytanie";
                    }
                    else
                    {
                        MessageBox.Show("Brak danych odpowiedzi.");
                    }
                }
            }else if(OdpowiedzB.Content.ToString() == "Sprawdz pytanie")
            {
                Question1AnswerB.Text = historyEvaluationB.Question1;
                Question2AnswerB.Text = historyEvaluationB.Question2;
                Question3AnswerB.Text = historyEvaluationB.Question3;
                Question4AnswerB.Text = historyEvaluationB.Question4;
                Question5AnswerB.Text = historyEvaluationB.Question5;
                Question6AnswerB.Text = historyEvaluationB.Question6;
                Question7AnswerB.Text = historyEvaluationB.Question7;
                Question8AnswerB.Text = historyEvaluationB.Question8;
                Question9AnswerB.Text = historyEvaluationB.Question9;
                Question10AnswerB.Text = historyEvaluationB.Question10;
                Question11AnswerB.Text = historyEvaluationB.Question11;
                OdpowiedzB.Content = "Sprawdz odpowiedz";
            }
            else
            {
                FormPanelBiuro.Visibility = Visibility.Visible;
                BackButton3.Visibility = Visibility.Visible;
                EvaluationDetailsGridB.Visibility = Visibility.Collapsed;
            }
        }

           private void Back2_Click(object sender, RoutedEventArgs e)
        {
            FormPanelProdukcja.Visibility = Visibility.Collapsed;
            BackButton2.Visibility = Visibility.Collapsed;
            EvaluationDetailsGridP.Visibility = Visibility.Visible;
        }
        private void Back3_Click(object sender, RoutedEventArgs e)
        {
            FormPanelBiuro.Visibility = Visibility.Collapsed;
            BackButton3.Visibility = Visibility.Collapsed;
            EvaluationDetailsGridB.Visibility = Visibility.Visible;
        }
    }
}
