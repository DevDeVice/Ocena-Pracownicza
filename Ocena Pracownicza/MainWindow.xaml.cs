using Ocena_Pracownicza.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BCrypt.Net;
using System.Windows.Documents;
using Ocena_Pracownicza.Migrations;

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
            LoadDataForRestore();
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
                                      .OrderBy(user => user.FullName)
                                      .Select(user => user.FullName)
                                      .ToList();
                AccountsComboBoxP1.ItemsSource = accounts;
                AccountsComboBoxB1.ItemsSource = accounts;
                AccountsComboBoxDelete.ItemsSource = accounts;
                AccountsComboBoxResetPassword.ItemsSource = accounts;
                AccountsComboBoxAdd.ItemsSource = accounts;
                AccountsComboBoxToChangeManager.ItemsSource = accounts;
                DepartmentComboBoxAdd.ItemsSource = accounts;
                DepartmentDeleteComboBox1.ItemsSource = accounts;
                DepartmentComboBoxRestore2.ItemsSource = accounts;
                DepartmentChangeComboBox1.ItemsSource = accounts;
                DepartmentChangeComboBox3.ItemsSource = accounts;
            }
        }
        private void LoadEvaluationName()
        {
            using (var context = new AppDbContext())
            {
                var evaluationNames = context.EvaluationNames
                    .OrderBy(e => e.EvaluatorName)
                    .Select(e => e.EvaluatorName)
                    .ToList();
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
            AccountsComboBoxB1.SelectedItem = null;
            AccountsComboBoxP1.SelectedItem = null;
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
                string.IsNullOrEmpty(Question10TextBoxB.Text)
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
                        Back3_Click(null, null);
                        MessageBox.Show("Ankieta została pomyślnie zapisana!");
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(NameTextBoxB.Text) ||
                AccountsComboBoxB1.SelectedItem == null ||
                AccountsComboBoxB2.SelectedItem == null ||
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
                string.IsNullOrEmpty(StanowiskoTextBoxB.Text)
                )
                {
                    MessageBox.Show("Wszystkie pola muszą być wypełnione!");
                    return;
                }
                int? selectedUserID = null;
                string selectedUserName = AccountsComboBoxB1.SelectedItem.ToString();
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
                var selectedDepartment = AccountsComboBoxB2.SelectedItem as Department;
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
                    DepartmentID = selectedDepartment.DepartmentID,
                    Stanowisko = StanowiskoTextBoxB.Text
                };
                using (var context = new AppDbContext())
                {
                    context.EvaluationBiuro.Add(evaluation);
                    context.SaveChanges();
                }

                var customMessageBox = new Window1();
                bool? wynik = customMessageBox.ShowDialog();
                if (wynik == true)
                {
                    // Użytkownik kliknął OK
                }
                else
                {
                    // Użytkownik kliknął DRUKUJ
                    PrintButtonB1_Click();
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
                        context.SaveChanges();
                        Back2_Click(null, null);
                        MessageBox.Show("Ankieta została pomyślnie zapisana!");
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
                AccountsComboBoxP1.SelectedItem == null ||
                AccountsComboBoxP2.SelectedItem == null ||
                string.IsNullOrEmpty(Question1TextBoxP.Text) ||
                string.IsNullOrEmpty(Question2TextBoxP.Text) ||
                string.IsNullOrEmpty(Question3TextBoxP.Text) ||
                string.IsNullOrEmpty(Question4TextBoxP.Text) ||
                string.IsNullOrEmpty(StanowiskoTextBoxP.Text)
                )
                {
                    MessageBox.Show("Wszystkie pola muszą być wypełnione!");
                    return;
                }

                int? selectedUserID = null;
                string selectedUserName = AccountsComboBoxP1.SelectedItem.ToString();
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
                var nameDepart = AccountsComboBoxP2;

                var selectedDepartment = AccountsComboBoxP2.SelectedItem as Department;

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
                    DepartmentID = selectedDepartment.DepartmentID,
                    Stanowisko = StanowiskoTextBoxP.Text
                };

                // 3. Zapisanie instancji w bazie danych:
                using (var context = new AppDbContext())
                {
                    context.EvaluationsProdukcja.Add(evaluation);
                    context.SaveChanges();
                }

                var customMessageBox = new Window1();
                bool? wynik = customMessageBox.ShowDialog();
                if (wynik == true)
                {
                    // Użytkownik kliknął OK
                }
                else
                {
                    // Użytkownik kliknął DRUKUJ
                    PrintButtonP1_Click();
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
            PrintButtonBClean.Visibility = Visibility.Visible;
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
            PrintButtonPClean.Visibility = Visibility.Visible;
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
            PrintButtonBClean.Visibility = Visibility.Collapsed;
            PrintButtonPClean.Visibility = Visibility.Collapsed;
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
            ChangeAnswerP1();
            ChangeAnswerB1();
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
            AddEvaluationNameTextBox.Text = String.Empty;

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
            AddImieNazwisko.Text = String.Empty; 
            AddLogin.Text = String.Empty; 
            AddHaslo.Text= String.Empty;
            AccountsComboBoxAdd.SelectedItem = null;
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
                        globalSetting = new DataModels.GlobalSettings(); //global
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
                } else if (username.ToLower() == "mobrzud" || username.ToLower() == "tlyson" || username.ToLower() == "rkrawczyk" || username.ToLower() == "blyson")
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

                    List<EvaluationRecordB> allEvaluationsB = new List<EvaluationRecordB>();
                    List<EvaluationRecordP> allEvaluationsP = new List<EvaluationRecordP>();

                    FetchEvaluationsAndSubordinatesAdmin(LoggedUser.UserID, allEvaluationsB, allEvaluationsP);

                    UserEvaluationsBListViewAll.ItemsSource = allEvaluationsB;
                    UserEvaluationsPListViewAll.ItemsSource = allEvaluationsP;
                }else
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
        private void FetchEvaluationsAndSubordinatesAdmin(int userId, List<EvaluationRecordB> allEvaluationsB, List<EvaluationRecordP> allEvaluationsP)
        {
            using (var context = new AppDbContext())
            {
                // Pobierz podwładnych użytkownika
                var subordinates = context.Users.ToList();

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
                }
            }
        }


        private void SearchAndFilterEvaluations()
        {
            string username = UsernameBox.Text;
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

                if (username.ToLower() == "mobrzud" || username.ToLower() == "tlyson" || username.ToLower() == "rkrawczyk" || username.ToLower() == "blyson")
                {
                    FilterEvaluationsForUserAndSubordinatesAdmin(LoggedUser.UserID, searchText, selectedEvaluatorNameID, allEvaluationsB, allEvaluationsP);
                }
                else
                {
                    FilterEvaluationsForUserAndSubordinates(LoggedUser.UserID, searchText, selectedEvaluatorNameID, allEvaluationsB, allEvaluationsP);
                }
                    

                // Update the list views
                UserEvaluationsBListViewAll.ItemsSource = allEvaluationsB;
                UserEvaluationsPListViewAll.ItemsSource = allEvaluationsP;
                
                UserEvaluationsBListView.ItemsSource = allEvaluationsB.Where(ev => ev.EvaluationB.UserID == LoggedUser.UserID).ToList(); ;
                UserEvaluationsPListView.ItemsSource = allEvaluationsP.Where(ev => ev.EvaluationP.UserID == LoggedUser.UserID).ToList(); ;
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

                if (userId == LoggedUser.UserID)
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
        private void FilterEvaluationsForUserAndSubordinatesAdmin(int userId, string searchText, int? evaluatorNameId, List<EvaluationRecordB> allEvaluationsB, List<EvaluationRecordP> allEvaluationsP)
        {
            using (var context = new AppDbContext())
            {
                // Pobierz wszystkich użytkowników (nie tylko podwładnych)
                var users = context.Users.ToList();

                foreach (var user in users)
                {
                    // Twórz zapytanie z filtrowaniem dla Biuro
                    var evaluationsQueryB = context.EvaluationBiuro
                                                   .Where(ev => ev.UserID == user.UserID);

                    // Twórz zapytanie z filtrowaniem dla Produkcja
                    var evaluationsQueryP = context.EvaluationsProdukcja
                                                   .Where(ev => ev.UserID == user.UserID);

                    // Dodaj filtrowanie po nazwie oceny, jeśli jest wybrana
                    if (evaluatorNameId.HasValue)
                    {
                        evaluationsQueryB = evaluationsQueryB.Where(ev => ev.EvaluatorNameID == evaluatorNameId.Value);
                        evaluationsQueryP = evaluationsQueryP.Where(ev => ev.EvaluatorNameID == evaluatorNameId.Value);
                    }

                    // Dodaj filtrowanie po tekście wyszukiwania
                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        evaluationsQueryB = evaluationsQueryB.Where(ev => ev.UserName.ToLower().Contains(searchText));
                        evaluationsQueryP = evaluationsQueryP.Where(ev => ev.UserName.ToLower().Contains(searchText));
                    }

                    // Dodaj wyniki do list
                    var filteredEvaluationsB = evaluationsQueryB.Select(ev => new EvaluationRecordB { EvaluationB = ev }).ToList();
                    var filteredEvaluationsP = evaluationsQueryP.Select(ev => new EvaluationRecordP { EvaluationP = ev }).ToList();

                    allEvaluationsB.AddRange(filteredEvaluationsB);
                    allEvaluationsP.AddRange(filteredEvaluationsP);
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
                ChangePrintB();
                DrukOdpB.Visibility = Visibility.Visible;        
                printDialog.PrintVisual(DrukOdpB, "Wydruk z aplikacji WPF");
                DrukOdpB.Visibility = Visibility.Collapsed;
            }
        }
        private void PrintButtonB1_Click()
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                ChangePrintB1();
                DrukOdpB.Visibility = Visibility.Visible;
                printDialog.PrintVisual(DrukOdpB, "Wydruk z aplikacji WPF");
                DrukOdpB.Visibility = Visibility.Collapsed;
            }
        }
        private void PrintButtonBClean_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                ChangePrintB2();
                DrukOdpB.Visibility = Visibility.Visible;
                printDialog.PrintVisual(DrukOdpB, "Wydruk z aplikacji WPF");
                DrukOdpB.Visibility = Visibility.Collapsed;
            }
        }
        private void ChangePrintB()
        {
            using (var context = new AppDbContext())
            {
                var department = context.Department.FirstOrDefault(d => d.DepartmentID == historyEvaluationB.DepartmentID);
                if (department != null)
                {
                    PrintB41.Text = department.DepartmentName;
                }
                var user = context.Users.FirstOrDefault(d => d.UserID == historyEvaluationB.UserID);
                if (user != null)
                {
                    PrintB43.Text = user.FullName;
                }
            }
            PrintB00.Content = DetailBTextBlock.Text;
            PrintB11.Text = historyEvaluationB.Date.ToString("yyyy-MM-dd");
            PrintB20.Content = AnswerB1.Text;
            PrintB21.Text = Question0AnswerB.Text;//imie nazwisko 
            PrintB30.Content = "Stanowisko:";
            PrintB31.Text = historyEvaluationB.Stanowisko; 
            PrintB40.Content = "Dział:";
            //PrintB41.Text = //Wykonywane wczesniej
            PrintB42.Content = "Rozmowe przeprowadził:";
            //PrintB43.Content = //Wykonywane wczesniej - rozmowe przeprowadzil
            PrintB50.Text = AnswerB2.Text; 
            PrintB51.Text = Question1AnswerB.Text;//Jakie są rezultaty Twojej pracy (konkretne wyniki)?
            PrintB60.Text = AnswerB3.Text;
            PrintB61.Text = Question2AnswerB.Text;//Jakie Twoje działania określił(a)byś jako pozytywne?"
            PrintB70.Text = AnswerB4.Text;//Jak oceniasz swoje działania i zachowania w kontekście wartości firmy tu bez odpowiedzi
            PrintB80.Content = AnswerB01.Text;
            PrintB81.Text = Question3AnswerB.Text;//Uczciwość:
            PrintB90.Content = AnswerB02.Text;
            PrintB91.Text = Question4AnswerB.Text;//Odpowiedzialność
            PrintB100.Content = AnswerB03.Text;
            PrintB101.Text = Question5AnswerB.Text;//Zaangażowanie
            PrintB110.Content = AnswerB04.Text;
            PrintB111.Text = Question6AnswerB.Text;//Bliskie relacje
            PrintB120.Content = AnswerB05.Text;
            PrintB121.Text = Question7AnswerB.Text;//Innowacyjność
            PrintB130.Text = AnswerB5.Text;
            PrintB131.Text = Question8AnswerB.Text;//Jakie Twoje działania określił(a)byś jako utrudniające uzyskanie dobrych rezultatów?
            PrintB140.Text = AnswerB6.Text;
            PrintB141.Text = Question9AnswerB.Text;//Nad czym chcesz pracować (jakie elementy zachowania/umiejetności chcesz rozwijać/jakie sobie stawiasz cele)?
            PrintB150.Text = AnswerB7.Text;
            PrintB151.Text = Question10AnswerB.Text;//Określ sposób i czas monitorowania dążenia do tych celów (kiedy i po czym poznasz, że zostały one zrealizowane):
            PrintB160.Content = "Uwagi:";
            PrintB161.Text = Question11AnswerB.Text;//uwagi
        }
        private void ChangePrintB1()
        {
            using (var context = new AppDbContext())
            {
                var selectedDepartment = AccountsComboBoxB2.SelectedItem as Department;
                var department = context.Department.FirstOrDefault(d => d.DepartmentID == selectedDepartment.DepartmentID);
                if (department != null)
                {
                    PrintB41.Text = department.DepartmentName;
                }
            }
            /*
                var user = context.Users.FirstOrDefault(d => d.UserID == historyEvaluationB.UserID);
                if (user != null)
                {
                    PrintB43.Text = user.FullName;
                }
            }*/
            PrintB00.Content = "A";
            PrintB11.Text = DateTime.Now.ToString();
            PrintB20.Content = "Imie Nazwisko:";
            PrintB21.Text = NameTextBoxB.Text;//imie nazwisko 
            PrintB30.Content = "Stanowisko:";
            PrintB31.Text = StanowiskoTextBoxB.Text;
            PrintB40.Content = "Dział:";
            //PrintB41.Text = "";//Wykonywane wczesniej
            PrintB42.Content = "Rozmowe przeprowadził:";
            PrintB43.Text = "";
            PrintB50.Text = "Jakie są rezultaty Twojej pracy (konkretne wyniki)?";
            PrintB51.Text = Question1TextBoxB.Text;//Jakie są rezultaty Twojej pracy (konkretne wyniki)?
            PrintB60.Text = "Jakie Twoje działania określił(a)byś jako pozytywne?";
            PrintB61.Text = Question2TextBoxB.Text;//Jakie Twoje działania określił(a)byś jako pozytywne?"
            PrintB70.Text = "Jak oceniasz swoje działania i zachowania w kontekście wartości firmy?";//Jak oceniasz swoje działania i zachowania w kontekście wartości firmy tu bez odpowiedzi
            PrintB80.Content = "Uczciwość:";
            PrintB81.Text = Question3TextBoxB.Text;//Uczciwość:
            PrintB90.Content = "Odpowiedzialność:";
            PrintB91.Text = Question4TextBoxB.Text;//Odpowiedzialność
            PrintB100.Content = "Zaangażowanie:";
            PrintB101.Text = Question5TextBoxB.Text;//Zaangażowanie
            PrintB110.Content = "Bliskie relacje:";
            PrintB111.Text = Question6TextBoxB.Text;//Bliskie relacje
            PrintB120.Content = "Innowacyjność:";
            PrintB121.Text = Question7TextBoxB.Text;//Innowacyjność
            PrintB130.Text = "Jakie Twoje działania określił(a)byś jako utrudniające uzyskanie dobrych rezultatów?";
            PrintB131.Text = Question8TextBoxB.Text;//Jakie Twoje działania określił(a)byś jako utrudniające uzyskanie dobrych rezultatów?
            PrintB140.Text = "Nad czym chcesz pracować (jakie elementy zachowania/umiejetności chcesz rozwijać/jakie sobie stawiasz cele)?";
            PrintB141.Text = Question9TextBoxB.Text;//Nad czym chcesz pracować (jakie elementy zachowania/umiejetności chcesz rozwijać/jakie sobie stawiasz cele)?
            PrintB150.Text = "Określ sposób i czas monitorowania dążenia do tych celów (kiedy i po czym poznasz, że zostały one zrealizowane):";
            PrintB151.Text = Question10TextBoxB.Text;//Określ sposób i czas monitorowania dążenia do tych celów (kiedy i po czym poznasz, że zostały one zrealizowane):
            PrintB160.Content = "Uwagi:";
            PrintB161.Text = Question11TextBoxB.Text;//uwagi
        }
        private void ChangePrintB2()
        {
            PrintB00.Content = "A";
            PrintB11.Text = "";
            PrintB20.Content = "Imie Nazwisko:";
            PrintB21.Text = "";
            PrintB30.Content = "Stanowisko:";
            PrintB31.Text = "";
            PrintB40.Content = "Dział:";
            PrintB41.Text = "";
            PrintB42.Content = "Rozmowe przeprowadził:";
            PrintB43.Text = "";
            PrintB50.Text = "Jakie są rezultaty Twojej pracy (konkretne wyniki)?";
            PrintB51.Text = "";
            PrintB60.Text = "Jakie Twoje działania określił(a)byś jako pozytywne?";
            PrintB61.Text = "";
            PrintB70.Text = "Jak oceniasz swoje działania i zachowania w kontekście wartości firmy?";//Jak oceniasz swoje działania i zachowania w kontekście wartości firmy tu bez odpowiedzi
            PrintB80.Content = "Uczciwość:";
            PrintB81.Text = "";
            PrintB90.Content = "Odpowiedzialność:";
            PrintB91.Text = "";
            PrintB100.Content = "Zaangażowanie:";
            PrintB101.Text = "";
            PrintB110.Content = "Bliskie relacje:";
            PrintB111.Text = "";
            PrintB120.Content = "Innowacyjność:";
            PrintB121.Text = "";
            PrintB130.Text = "Jakie Twoje działania określił(a)byś jako utrudniające uzyskanie dobrych rezultatów?";
            PrintB131.Text = "";
            PrintB140.Text = "Nad czym chcesz pracować (jakie elementy zachowania/umiejetności chcesz rozwijać/jakie sobie stawiasz cele)?";
            PrintB141.Text = "";
            PrintB150.Text = "Określ sposób i czas monitorowania dążenia do tych celów (kiedy i po czym poznasz, że zostały one zrealizowane):";
            PrintB151.Text = "";
            PrintB160.Content = "Uwagi:";
            PrintB161.Text = "";
        }
        private void PrintButtonP_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                ChangePrintP();
                DrukOdpP.Visibility = Visibility.Visible;
                printDialog.PrintVisual(DrukOdpP, "Wydruk z aplikacji WPF");
                DrukOdpP.Visibility = Visibility.Collapsed;
            }
        }
        private void PrintButtonP1_Click()
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                ChangePrintP1();
                DrukOdpP.Visibility = Visibility.Visible;
                printDialog.PrintVisual(DrukOdpP, "Wydruk z aplikacji WPF");
                DrukOdpP.Visibility = Visibility.Collapsed;
            }
        }
        private void PrintButtonPClean_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                ChangePrintP2();
                DrukOdpP.Visibility = Visibility.Visible;
                printDialog.PrintVisual(DrukOdpP, "Wydruk z aplikacji WPF");
                DrukOdpP.Visibility = Visibility.Collapsed;
            }
        }
        private void ChangePrintP()
        {
            using (var context = new AppDbContext())
            {
                var department = context.Department.FirstOrDefault(d => d.DepartmentID == historyEvaluationP.DepartmentID);
                if (department != null)
                {
                    PrintP41.Text = department.DepartmentName;
                }
                var user = context.Users.FirstOrDefault(d => d.UserID == historyEvaluationP.UserID);
                if (user != null)
                {
                    PrintP43.Text = user.FullName;
                }
            }
            PrintP00.Content = DetailPTextBlock.Text;
            PrintP11.Text = historyEvaluationP.Date.ToString("yyyy-MM-dd");
            PrintP20.Content = AnswerP1.Text;
            PrintP21.Text = Question0AnswerP.Text;//imie nazwisko 
            PrintP30.Content = "Stanowisko:";
            PrintP31.Text = historyEvaluationP.Stanowisko; ;//stanowisko 
            PrintP40.Content = "Dział:";
            //PrintB41.Text = //Wykonywane wczesniej
            PrintP42.Content = "Rozmowe przeprowadził:";
            /*PrintP50.Text = AnswerP2.Text;
            PrintP51.Text = Question1AnswerP.Text;//Jakie są rezultaty Twojej pracy (konkretne wyniki)?*/
            PrintP60.Text = AnswerP2.Text;
            PrintP61.Text = Question1AnswerP.Text;//Jakie Twoje działania określił(a)byś jako pozytywne?"
            /*PrintP70.Text = AnswerP4.Text;//Jak oceniasz swoje działania i zachowania w kontekście wartości firmy tu bez odpowiedzi
            PrintP80.Content = AnswerP01.Text;
            PrintP81.Text = Question3AnswerP.Text;//Uczciwość:
            PrintP90.Content = AnswerP02.Text;
            PrintP91.Text = Question4AnswerP.Text;//Odpowiedzialność
            PrintP100.Content = AnswerP03.Text;
            PrintP101.Text = Question5AnswerP.Text;//Zaangażowanie
            PrintP110.Content = AnswerP04.Text;
            PrintP111.Text = Question6AnswerP.Text;//Bliskie relacje
            PrintP120.Content = AnswerP05.Text;
            PrintP121.Text = Question7AnswerP.Text;//Innowacyjność*/
            PrintP130.Text = AnswerP3.Text;
            PrintP131.Text = Question2AnswerP.Text;//Jakie Twoje działania określił(a)byś jako utrudniające uzyskanie dobrych rezultatów?
            PrintP140.Text = AnswerP4.Text;
            PrintP141.Text = Question3AnswerP.Text;//Nad czym chcesz pracować (jakie elementy zachowania/umiejetności chcesz rozwijać/jakie sobie stawiasz cele)?
            PrintP150.Text = AnswerP5.Text;
            PrintP151.Text = Question4AnswerP.Text;//Określ sposób i czas monitorowania dążenia do tych celów (kiedy i po czym poznasz, że zostały one zrealizowane):
            PrintP160.Content = "Uwagi:";
            PrintP161.Text = Question5AnswerP.Text;//uwagi*/
        }
        private void ChangePrintP1()
        {
            using (var context = new AppDbContext())
            {
                var selectedDepartment = AccountsComboBoxP2.SelectedItem as Department;
                var department = context.Department.FirstOrDefault(d => d.DepartmentID == selectedDepartment.DepartmentID);
                if (department != null)
                {
                    PrintP41.Text = department.DepartmentName;
                }
            }
            /*var user = context.Users.FirstOrDefault(d => d.UserID == historyEvaluationP.UserID);
            if (user != null)
            {
                PrintP43.Text = user.FullName;
            }*/
            PrintP00.Content = "A";
            PrintP11.Text = DateTime.Now.ToString();

            PrintP20.Content = "Imie Nazwisko:";
            PrintP21.Text = NameTextBoxP.Text;//imie nazwisko 
            PrintP30.Content = "Stanowisko:";
            PrintP31.Text = StanowiskoTextBoxP.Text; ;//stanowisko 
            PrintP40.Content = "Dział:";
            //PrintB41.Text = //Wykonywane wczesniej
            PrintP42.Content = "Rozmowe przeprowadził:";
            PrintP43.Text = "";
            /*PrintP50.Text = AnswerP2.Text;
            PrintP51.Text = Question1AnswerP.Text;//Jakie są rezultaty Twojej pracy (konkretne wyniki)?*/
            PrintP60.Text = "Jakie twoje działania określił(a)byś jako pozytywne (przynoszące rezultaty w twojej pracy)?";
            PrintP61.Text = Question1TextBoxP.Text;//Jakie Twoje działania określił(a)byś jako pozytywne?"
            PrintP130.Text = "Jakie twoje działania określił(a)byś jako utrudniające uzyskanie dobrych rezultatów?";
            PrintP131.Text = Question2TextBoxP.Text;//Jakie Twoje działania określił(a)byś jako utrudniające uzyskanie dobrych rezultatów?
            PrintP140.Text = "Nad czym chcesz pracować (jakie elementy zachowania/umiejętności chcesz rozwijać/jakie sobie stawiasz cele)?";
            PrintP141.Text = Question3TextBoxP.Text;//Nad czym chcesz pracować (jakie elementy zachowania/umiejetności chcesz rozwijać/jakie sobie stawiasz cele)?
            PrintP150.Text = "Określ sposób i czas monitorowania dążenia do tych celów (kiedy i po czym poznasz, że zostały one zrealizowane)";
            PrintP151.Text = Question4TextBoxP.Text;//Określ sposób i czas monitorowania dążenia do tych celów (kiedy i po czym poznasz, że zostały one zrealizowane):
            PrintP160.Content = "Uwagi:";
            PrintP161.Text = Question5TextBoxP.Text;//uwagi*/
        }
        private void ChangePrintP2()
        {
            PrintP00.Content = "A";
            PrintP11.Text = "";
            PrintP20.Content = "Imie Nazwisko:";
            PrintP21.Text = "";
            PrintP30.Content = "Stanowisko:";
            PrintP31.Text = "";
            PrintP40.Content = "Dział:";
            PrintP41.Text = "";
            PrintP42.Content = "Rozmowe przeprowadził:";
            PrintP43.Text = "";
            PrintP60.Text = "Jakie twoje działania określił(a)byś jako pozytywne (przynoszące rezultaty w twojej pracy)?";
            PrintP61.Text = "";
            PrintP130.Text = "Jakie twoje działania określił(a)byś jako utrudniające uzyskanie dobrych rezultatów?";
            PrintP131.Text = "";
            PrintP140.Text = "Nad czym chcesz pracować (jakie elementy zachowania/umiejętności chcesz rozwijać/jakie sobie stawiasz cele)?";
            PrintP141.Text = "";
            PrintP150.Text = "Określ sposób i czas monitorowania dążenia do tych celów (kiedy i po czym poznasz, że zostały one zrealizowane)";
            PrintP151.Text = "";
            PrintP160.Content = "Uwagi:";
            PrintP161.Text = "";
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void MinimalizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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
                AccountsComboBoxDelete.SelectedItem = null;
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
                AccountsComboBoxRestore.SelectedItem = null;
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
            var selectedUserToChange = AccountsComboBoxToChangeManager.SelectedItem as string;
            var selectedNewManagerName = AccountsComboBoxNewManager.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedUserToChange) && !string.IsNullOrEmpty(selectedNewManagerName))
            {
                using (var context = new AppDbContext())
                {
                    var toChange = context.Users.FirstOrDefault(user => user.FullName == selectedUserToChange);
                    var newManager = context.Users.FirstOrDefault(user => user.FullName == selectedNewManagerName);
                    if (newManager != null)
                    {
                        toChange.ManagerId = newManager.UserID;
                        context.SaveChanges(); // Zapisz zmiany w bazie danych

                        MessageBox.Show("Menedżer został pomyślnie zmieniony.");
                    }
                }
            }
        }

        private void AccountsComboBoxToChangeManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AccountsComboBoxNewManager.SelectedItem = null;
            using (var context = new AppDbContext())
            {
                string selectedFullName = AccountsComboBoxToChangeManager.SelectedItem?.ToString();
                var accounts = context.Users
                                      .Where(user => user.FullName != "Administrator" && user.Enabled != false && user.FullName != selectedFullName)
                                      .OrderBy(user => user.FullName)
                                      .Select(user => user.FullName)
                                      .ToList();
                AccountsComboBoxNewManager.ItemsSource = accounts;
            }
            
        }

        private void OdpowiedzP_Click(object sender, RoutedEventArgs e)
        {
            if (historyAnswerPID > 0 && DetailPTextBlock.Text == "A")// if (OdpowiedzP.Content.ToString() == "Sprawdz odpowiedz")
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
                        DetailPTextBlock.Text = "B";
                    }
                    else
                    {
                        MessageBox.Show("Brak danych odpowiedzi.");
                    }
                }
                ChangeAnswerP();
            }
            else if (historyAnswerPID > 0 && DetailPTextBlock.Text == "B")//else if (OdpowiedzP.Content.ToString() == "Sprawdz pytanie")
            {
                Question1AnswerP.Text = historyEvaluationP.Question1;
                Question2AnswerP.Text = historyEvaluationP.Question2;
                Question3AnswerP.Text = historyEvaluationP.Question3;
                Question4AnswerP.Text = historyEvaluationP.Question4;
                Question5AnswerP.Text = historyEvaluationP.Question5;
                DetailPTextBlock.Text = "A";
                ChangeAnswerP1();
            }
            else
            {
                FormPanelProdukcja.Visibility = Visibility.Visible;
                BackButton2.Visibility = Visibility.Visible;
                EvaluationDetailsGridP.Visibility = Visibility.Collapsed;
                PrintButtonPClean.Visibility = Visibility.Visible;
            } 
        }

        private void OdpowiedzB_Click(object sender, RoutedEventArgs e)
        {
            if (historyAnswerBID > 0 && DetailBTextBlock.Text == "A")
            {
                using (var context = new AppDbContext())
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
                        DetailBTextBlock.Text = "B";
                    }
                    else
                    {
                        MessageBox.Show("Brak danych odpowiedzi.");
                    }
                }
                ChangeAnswerB();
            }
            else if (historyAnswerBID > 0 && DetailBTextBlock.Text == "B")
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
                DetailBTextBlock.Text = "A";
                ChangeAnswerB1();
            }
            else
            {
                FormPanelBiuro.Visibility = Visibility.Visible;
                BackButton3.Visibility = Visibility.Visible;
                EvaluationDetailsGridB.Visibility = Visibility.Collapsed;
                PrintButtonBClean.Visibility = Visibility.Visible;
            }
        }
        private void ChangeAnswerP()
        {
            AnswerP1.Text = "Imię i nazwisko pracownika:";
            AnswerP2.Text = "Jakie działania pracownika określił(a)byś jako pozytywne (przynoszące rezultaty w jego pracy)?";
            AnswerP3.Text = "Jakie działania pracownika określił(a)byś jako utrudniające uzyskanie dobrych rezultatów?";
            AnswerP4.Text = "Nad czym powinien pracować (jakie elementy zachowania/umiejętności powinien rozwijać/jakie sobie stawiać cele)?";
            AnswerP5.Text = "Określenie sposobu i czasu monitorowanie dążenia do tych celów(kiedy i po czym poznamy, że zostały one zrealizowane):";
        }
        private void ChangeAnswerP1()
        {
            AnswerP1.Text = "Imię i nazwisko:";
            AnswerP2.Text = "Jakie twoje działania określił(a)byś jako pozytywne (przynoszące rezultaty w twojej pracy)?";
            AnswerP3.Text = "Jakie twoje działania określił(a)byś jako utrudniające uzyskanie dobrych rezultatów?";
            AnswerP4.Text = "Nad czym chcesz pracować (jakie elementy zachowania/ umiejętności chcesz rozwijać/ jakie sobie stawiasz cele)?";
            AnswerP5.Text = "Określ sposób i czas monitorowania dążenia do tych celów (kiedy i po czym poznasz, że zostały one zrealizowane):";
        }
        private void ChangeAnswerB()
        {
            AnswerB1.Text = "Imię i nazwisko pracownika:";
            AnswerB2.Text = "Jakie są rezultaty pracy pracownika (konkretne wyniki)?";
            AnswerB3.Text = "Jakie działania pracownika określił(a)byś jako pozytywne? ";
            AnswerB4.Text = "Jak oceniasz działania i zachowania pracownika w kontekście wartości firmy? ";
            AnswerB5.Text = "Jakie działania pracownika określił(a)byś jako utrudniające uzyskanie dobrych rezultatów? ";
            AnswerB6.Text = "Nad czym powinien pracować (jakie elementy zachowania/ umiejętności powinien rozwijać/ jakie sobie stawiać cele)?";
            AnswerB7.Text = "Określenie sposobu i czasu monitorowania dążenia do tych celów (kiedy i po czym poznamy, że zostały one zrealizowane): ";
        }
        private void ChangeAnswerB1()
        {
            AnswerB1.Text = "Imię i nazwisko:";
            AnswerB2.Text = "Jakie są rezultaty Twojej pracy (konkretne wyniki)?";
            AnswerB3.Text = "Jakie Twoje działania określił(a)byś jako pozytywne?";
            AnswerB4.Text = "Jak oceniasz swoje działania i zachowania w kontekście wartości firmy?";
            AnswerB5.Text = "Jakie Twoje działania określił(a)byś jako utrudniające uzyskanie dobrych rezultatów?";
            AnswerB6.Text = "Nad czym chcesz pracować (jakie elementy zachowania/umiejetności chcesz rozwijać/jakie sobie stawiasz cele)?";
            AnswerB7.Text = "Określ sposób i czas monitorowania dążenia do tych celów (kiedy i po czym poznasz, że zostały one zrealizowane):";
        }
        private void Back2_Click(object sender, RoutedEventArgs e)
        {
            FormPanelProdukcja.Visibility = Visibility.Collapsed;
            BackButton2.Visibility = Visibility.Collapsed;
            EvaluationDetailsGridP.Visibility = Visibility.Visible;
            PrintButtonPClean.Visibility = Visibility.Collapsed;
        }
        private void Back3_Click(object sender, RoutedEventArgs e)
        {
            FormPanelBiuro.Visibility = Visibility.Collapsed;
            BackButton3.Visibility = Visibility.Collapsed;
            EvaluationDetailsGridB.Visibility = Visibility.Visible;
            PrintButtonBClean.Visibility = Visibility.Collapsed;
        }

        private void DepartmentAddButton_Click(object sender, RoutedEventArgs e)
        {
            string departmentName = DepartmentTextBoxAdd.Text;

            // Sprawdź, czy pole nazwy działu jest puste
            if (string.IsNullOrWhiteSpace(departmentName))
            {
                MessageBox.Show("Nazwa działu nie może być pusta.");
                return;
            }
            // Pobranie nazwy użytkownika z ComboBox
            string selectedUserName = DepartmentComboBoxAdd.SelectedItem?.ToString();
            // Znalezienie wybranego użytkownika z ComboBox
            if (string.IsNullOrEmpty(selectedUserName))
            {
                MessageBox.Show("Nie wybrano użytkownika.");
                return;
            }

            using (var context = new AppDbContext())
            {
                // Sprawdź, czy nazwa działu jest unikalna
                bool doesDepartmentExist = context.Department.Any(d => d.DepartmentName == departmentName);
                if (doesDepartmentExist)
                {
                    MessageBox.Show("Dział o tej nazwie już istnieje.");
                    return;
                }
                var user = context.Users.FirstOrDefault(u => u.FullName == selectedUserName);
                if (user == null)
                {
                    MessageBox.Show("Nie znaleziono użytkownika o takim imieniu i nazwisku.");
                    return;
                }
                // Tworzenie nowego obiektu Department
                var newDepartment = new Department
                {
                    DepartmentName = departmentName,
                    UserID = user.UserID,
                    Enabled = 1
                };

                // Zapisanie nowego działu do bazy danych
                context.Department.Add(newDepartment);
                context.SaveChanges();
            }

            // Dodatkowe akcje po pomyślnym dodaniu działu, np. odświeżenie UI (opcjonalnie)
            MessageBox.Show("Dział został pomyślnie dodany.");
            DepartmentTextBoxAdd.Text = String.Empty;
            DepartmentComboBoxAdd.SelectedItem = null;
        }
        private void DepartmentDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentDeleteComboBox2.SelectedItem is Department selectedDepartment)
            {
                using (var context = new AppDbContext())
                {
                    // Teraz używamy właściwości DepartmentName zamiast ToString()
                    var department = context.Department.FirstOrDefault(d => d.DepartmentName == selectedDepartment.DepartmentName);
                    if (department != null)
                    {
                        department.Enabled = 0; // Ustawienie Enabled na 0
                        context.SaveChanges();
                        MessageBox.Show("Dział został pomyślnie usunięty.");
                        LoadDataForRestore();
                    }
                    else
                    {
                        MessageBox.Show("Nie znaleziono wybranego działu.");
                    }
                }
                DepartmentDeleteComboBox1.SelectedItem = null;
                DepartmentDeleteComboBox2.SelectedItem = null;
            }
            else
            {
                MessageBox.Show("Nie wybrano działu do usunięcia.");
            }
        }
        private void DepartmentRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDepartment = DepartmentComboBoxRestore1.SelectedItem as Department;
            var selectedUser = DepartmentComboBoxRestore2.SelectedItem?.ToString();
            
            if (selectedDepartment == null)
            {
                MessageBox.Show("Nie wybrano działu do przywrócenia.");
                return;
            }

            

            using (var context = new AppDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.FullName == selectedUser);
                if (selectedUser == null)
                {
                    MessageBox.Show("Nie wybrano użytkownika.");
                    return;
                }
                // Znajdź dział w bazie danych
                var departmentToRestore = context.Department.FirstOrDefault(d => d.DepartmentID == selectedDepartment.DepartmentID);
                if (departmentToRestore != null)
                {
                    // Zaktualizuj dane działu
                    departmentToRestore.Enabled = 1;
                    departmentToRestore.UserID = user.UserID;
                    context.SaveChanges();
                    MessageBox.Show("Dział został przywrócony.");
                    DepartmentComboBoxRestore1.SelectedItem = null;
                    DepartmentComboBoxRestore1.SelectedItem = null;
                }
                else
                {
                    MessageBox.Show("Nie znaleziono wybranego działu w bazie danych.");
                }
            }
        }

        private void DepartmentChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDepartment = DepartmentChangeComboBox2.SelectedItem as Department;
            var selectedUser = DepartmentChangeComboBox3.SelectedItem?.ToString();
            if (selectedDepartment == null)
            {
                MessageBox.Show("Nie wybrano działu do przywrócenia.");
                return;
            }
            using (var context = new AppDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.FullName == selectedUser);
                if (selectedUser == null)
                {
                    MessageBox.Show("Nie wybrano użytkownika.");
                    return;
                }
                // Znajdź dział w bazie danych
                var departmentToRestore = context.Department.FirstOrDefault(d => d.DepartmentID == selectedDepartment.DepartmentID);
                if (departmentToRestore != null)
                {
                    // Zaktualizuj dane działu
                    departmentToRestore.UserID = user.UserID;
                    context.SaveChanges();
                    MessageBox.Show("Dział został zmieniony.");
                    DepartmentChangeComboBox1.SelectedItem = null;
                    DepartmentChangeComboBox2.SelectedItem = null;
                    DepartmentChangeComboBox3.SelectedItem = null;
                }
                else
                {
                    MessageBox.Show("Nie znaleziono wybranego działu w bazie danych.");
                }
            }
        }
        private void DepartmentDeleteComboBox1_SelectionChanged(object sender, EventArgs e)
        {
            // Pobierz nazwę użytkownika z ComboBox
            string selectedUserName = DepartmentDeleteComboBox1.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedUserName))
            {
                using (var context = new AppDbContext())
                {
                    // Znajdź użytkownika o podanej nazwie
                    var user = context.Users.FirstOrDefault(u => u.FullName == selectedUserName);
                    if (user != null)
                    {
                        // Pobierz tylko te działy, które są włączone (Enabled = 1)
                        var departments = context.Department
                                                 .Where(d => d.UserID == user.UserID && d.Enabled == 1)
                                                 .ToList();
                        DepartmentDeleteComboBox2.ItemsSource = departments;
                        DepartmentDeleteComboBox2.DisplayMemberPath = "DepartmentName"; // lub inna właściwość, którą chcesz wyświetlić
                    }
                }
            }
        }
        private void DepartmentChangeComboBox1_SelectionChanged(object sender, EventArgs e)
        {
            // Pobierz nazwę użytkownika z ComboBox
            string selectedUserName = DepartmentChangeComboBox1.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedUserName))
            {
                using (var context = new AppDbContext())
                {
                    // Znajdź użytkownika o podanej nazwie
                    var user = context.Users.FirstOrDefault(u => u.FullName == selectedUserName);
                    if (user != null)
                    {
                        // Pobierz tylko te działy, które są włączone (Enabled = 1)
                        var departments = context.Department
                                                 .Where(d => d.UserID == user.UserID && d.Enabled == 1)
                                                 .OrderBy(d => d.DepartmentName)
                                                 .ToList();
                        DepartmentChangeComboBox2.ItemsSource = departments;
                        DepartmentChangeComboBox2.DisplayMemberPath = "DepartmentName";
                    }
                }
            }
        }
        private void LoadDataForRestore()
        {
            using (var context = new AppDbContext())
            {
                var departmentsToRestore = context.Department.Where(d => d.Enabled == 0).ToList();
                DepartmentComboBoxRestore1.ItemsSource = departmentsToRestore;
                DepartmentComboBoxRestore1.DisplayMemberPath = "DepartmentName";
            }
        }

        private void AccountsComboBoxB1_SelectionChanged(object sender, EventArgs e)
        {
            // Pobierz nazwę użytkownika z ComboBox
            string selectedUserName = AccountsComboBoxB1.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedUserName))
            {
                using (var context = new AppDbContext())
                {
                    // Znajdź użytkownika o podanej nazwie
                    var user = context.Users.FirstOrDefault(u => u.FullName == selectedUserName);
                    if (user != null)
                    {
                        // Pobierz tylko te działy, które są włączone (Enabled = 1)
                        var departments = context.Department
                                                 .Where(d => d.UserID == user.UserID && d.Enabled == 1)
                                                 .ToList();
                        AccountsComboBoxB2.ItemsSource = departments;
                        AccountsComboBoxB2.DisplayMemberPath = "DepartmentName"; // lub inna właściwość, którą chcesz wyświetlić
                    }
                }
            }
        }
        private void AccountsComboBoxP1_SelectionChanged(object sender, EventArgs e)
        {
            // Pobierz nazwę użytkownika z ComboBox
            string selectedUserName = AccountsComboBoxP1.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedUserName))
            {
                using (var context = new AppDbContext())
                {
                    // Znajdź użytkownika o podanej nazwie
                    var user = context.Users.FirstOrDefault(u => u.FullName == selectedUserName);
                    if (user != null)
                    {
                        // Pobierz tylko te działy, które są włączone (Enabled = 1)
                        var departments = context.Department
                                                 .Where(d => d.UserID == user.UserID && d.Enabled == 1)
                                                 .ToList();
                        AccountsComboBoxP2.ItemsSource = departments;
                        AccountsComboBoxP2.DisplayMemberPath = "DepartmentName"; // lub inna właściwość, którą chcesz wyświetlić
                    }
                }
            }
        }

        
    }
}
