-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Sty 12, 2024 at 09:37 AM
-- Wersja serwera: 10.4.28-MariaDB
-- Wersja PHP: 8.0.28

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ocena_pracownicza`
--

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `department`
--

CREATE TABLE `department` (
  `DepartmentID` int(11) NOT NULL,
  `DepartmentName` longtext NOT NULL,
  `UserID` int(11) NOT NULL,
  `Enabled` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `department`
--

INSERT INTO `department` (`DepartmentID`, `DepartmentName`, `UserID`, `Enabled`) VALUES
(6, 'HR', 8, 1),
(7, 'dział komunikacji i PR', 10, 1),
(8, 'Apilandia', 10, 1),
(9, 'BOK', 11, 1),
(10, 'dział operacyjny', 11, 1),
(11, 'sklepy', 11, 1),
(12, 'dział IT', 12, 1),
(13, 'dział wysyłki', 13, 1),
(14, 'dział marketingu', 14, 1),
(15, 'dział eksportu', 15, 1),
(16, 'dział analiz', 16, 1),
(17, 'dział ksiegowości, dział kadr i płac', 17, 1),
(18, 'dział księgowości', 18, 1),
(19, 'dział R&D', 22, 1),
(20, 'dział zakupów i gospodarki magazynowej - biuro', 23, 1),
(21, 'dział zakupów i gospodarki magazynowej - magazyn', 23, 1),
(22, 'dział logistyki - biuro', 24, 1),
(23, 'dział logistyki - magazyny', 25, 1),
(24, 'dział optymalizacji procesów', 24, 1),
(25, 'dział produkcji uli - produkcja', 21, 1),
(26, 'dział produkcji urządzeń - biuro', 20, 1),
(27, 'dział kontroli jakości i serwis', 26, 1);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `evaluationbiuro`
--

CREATE TABLE `evaluationbiuro` (
  `EvaluationID` int(11) NOT NULL,
  `UserName` longtext NOT NULL,
  `UserID` int(11) NOT NULL,
  `Date` datetime(6) NOT NULL,
  `Question1` longtext NOT NULL,
  `Question2` longtext NOT NULL,
  `Question3` longtext NOT NULL,
  `Question4` longtext NOT NULL,
  `Question5` longtext NOT NULL,
  `Question6` longtext NOT NULL,
  `EvaluatorNameID` int(11) NOT NULL DEFAULT 0,
  `Question10` longtext NOT NULL,
  `Question11` longtext NOT NULL,
  `Question7` longtext NOT NULL,
  `Question8` longtext NOT NULL,
  `Question9` longtext NOT NULL,
  `EvaluationAnswerID` int(11) NOT NULL DEFAULT 0,
  `DepartmentID` int(11) NOT NULL DEFAULT 0,
  `Stanowisko` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `evaluationbiuroanswers`
--

CREATE TABLE `evaluationbiuroanswers` (
  `EvaluationID` int(11) NOT NULL,
  `Question1` longtext NOT NULL,
  `Question2` longtext NOT NULL,
  `Question3` longtext NOT NULL,
  `Question4` longtext NOT NULL,
  `Question5` longtext NOT NULL,
  `Question6` longtext NOT NULL,
  `Question7` longtext NOT NULL,
  `Question8` longtext NOT NULL,
  `Question9` longtext NOT NULL,
  `Question10` longtext NOT NULL,
  `Question11` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `evaluationnames`
--

CREATE TABLE `evaluationnames` (
  `EvaluatorNameID` int(11) NOT NULL,
  `EvaluatorName` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `evaluationnames`
--

INSERT INTO `evaluationnames` (`EvaluatorNameID`, `EvaluatorName`) VALUES
(9, '2024');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `evaluationprodukcjaanswers`
--

CREATE TABLE `evaluationprodukcjaanswers` (
  `EvaluationID` int(11) NOT NULL,
  `Question1` longtext NOT NULL,
  `Question2` longtext NOT NULL,
  `Question3` longtext NOT NULL,
  `Question4` longtext NOT NULL,
  `Question5` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `evaluationsprodukcja`
--

CREATE TABLE `evaluationsprodukcja` (
  `EvaluationID` int(11) NOT NULL,
  `UserName` longtext NOT NULL,
  `UserID` int(11) NOT NULL,
  `EvaluatorNameID` int(11) NOT NULL,
  `Date` datetime(6) NOT NULL,
  `Question1` longtext NOT NULL,
  `Question2` longtext NOT NULL,
  `Question3` longtext NOT NULL,
  `Question4` longtext NOT NULL,
  `EvaluationAnswerID` int(11) NOT NULL DEFAULT 0,
  `Question5` longtext NOT NULL,
  `DepartmentID` int(11) NOT NULL DEFAULT 0,
  `Stanowisko` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `globalsettings`
--

CREATE TABLE `globalsettings` (
  `CurrentEvaluationName` longtext NOT NULL,
  `Id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `globalsettings`
--

INSERT INTO `globalsettings` (`CurrentEvaluationName`, `Id`) VALUES
('2024', 1);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `users`
--

CREATE TABLE `users` (
  `UserID` int(11) NOT NULL,
  `FullName` longtext NOT NULL,
  `Password` longtext NOT NULL,
  `Login` longtext NOT NULL,
  `Enabled` tinyint(1) NOT NULL DEFAULT 0,
  `ManagerId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`UserID`, `FullName`, `Password`, `Login`, `Enabled`, `ManagerId`) VALUES
(7, 'Administrator', '$2a$11$VWSBHGWg8/bifft6h443ROeexQ1ZnsnOjZ4DoOQh8AYV4P8zFj5c.', 'admin', 1, NULL),
(8, 'Maria Obrzud', '$2a$11$p.hIGcALnDr4volFfHietutLVNya/QCdhphEeZoAmah1mBGJUAG/C', 'mobrzud', 1, NULL),
(10, 'Barbara Łysoń', '$2a$11$ayTpaKMqSjgU4Ce8qCkvy.1MDX1x.fFy1SLsfABtGsi3oO.hBKpmi', 'blyson', 1, NULL),
(11, 'Sylwia Gibas', '$2a$11$HViaIKZEGbtT0bbfIUFAD.wLhDsp/GKHcvEgxnKJ6snmkRavRyLiW', 'sgibas', 1, NULL),
(12, 'Jacek Spisak', '$2a$11$ZG72HkMomYB7DsfH9NeZyu9l.A0P2qT4We5Oco78ZnO4WV82if7le', 'jspisak', 1, 11),
(13, 'Joanna Biczak', '$2a$11$mCjMSN.x/.vsQjm0/ScgouqU2Xo7abz8UB0CBkfCJ0i18wlpI8jxi', 'jbiczak', 1, 11),
(14, 'Katarzyna Kudzia', '$2a$11$IOXHjTehEyRU9tV9dlpccOiCDNBb3S9HPUHuJJKgu9Hg1K/lG3usO', 'kkudzia', 1, 11),
(15, 'Tarkan Ince', '$2a$11$RnUb9T5AYx.FZSf1gvOozOESIk9PundgPEKqizi2Q8WPHf9r8oEUu', 'tince', 1, NULL),
(16, 'Anna Rusinek', '$2a$11$ylULzduq0DuyyiHhUgJgGeZAC3pSrJihxfCZRLs9kjsjTltRJX0Iu', 'arusinek', 1, NULL),
(17, 'Marta Leśniewska', '$2a$11$f84HHRA4Ua5bPnavp6RjGuitvk0M0HbzebYqM6sxJ1HH4fWweNLmu', 'mlesniewska', 1, NULL),
(18, 'Katarzyna Bałys', '$2a$11$yMGZ2L9bLl.Bn75O1B9TuuXwZj8RME6N7aWtoaoBiUcNzghIzc//y', 'kbalys', 1, NULL),
(19, 'Seweryn Biczak', '$2a$11$BIfkjXP9YEBRWbAitulTCeozcEFrxR.CLziQFyhGjFz7FZvJESi4W', 'sbiczak', 1, NULL),
(20, 'Tomasz Zajda', '$2a$11$qrR9uczxBdVCLsmt/RTbiOSYvYapPt4vMwr0XBgndW3SM.GE2Wu0O', 'tzajda', 1, 19),
(21, 'Szymon Wróbel', '$2a$11$Xg8zGMMGzSBwx3l561PtouUtQvnu3ciNkDvzwYpZlLzgqihRutvLK', 'swrobel', 1, 19),
(22, 'Rafał Smolec', '$2a$11$YKA5/1EWNAd5/j6Teipyl.ZkGs5AICNNXUsmYtfLvfBS3OVDOatcu', 'rsmolec', 1, 19),
(23, 'Łukasz Gołba', '$2a$11$GckkoRVXI1zq69VYwEGnn.L.dagkaLbvcKu1/iI35A9hwW/0SjtVG', 'lgolba', 1, 19),
(24, 'Maciej Malaga', '$2a$11$v/wdADMTzztkrm4iGZbfsOU7Avmv9mRLhM.O.jAWnpbJ2w1kWJRYy', 'mmalaga', 1, 19),
(25, 'Marcin Wajdzik', '$2a$11$T2sLB/ixDlYWZZDUNn2wWOY1Uag5IWajy3Cb9nS5y58hmcNhtMZTG', 'mwajdzik', 1, 19),
(26, 'Przemysław Przytuła', '$2a$11$.aNYeHknedLnIjjKPcg/1.mOOOHtu9fKc00xX/DpmSevy1fkZnydu', 'pprzytula', 1, NULL),
(27, 'Tomasz Łysoń', '$2a$11$cPV6OYpmni3xYISHXlqpF.gil.g69hFnypRdGiRuER2.YbeE4cU2.', 'tlyson', 1, NULL),
(28, 'Rafał Krawczyk', '$2a$11$5J0oWanmFToopeWQH9ejGOXCnqv1Mol.T1ZDYMc9lyy/Rq5cIQ/by', 'rkrawczyk', 1, NULL);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20230929082422_InitialCreate', '7.0.11'),
('20231010092553_[UserUpdateLogin]', '7.0.11'),
('20231010112107_[UserNameToString]', '7.0.11'),
('20231010113407_[PrimaryKey]', '7.0.11'),
('20231010115206_[DeleteEva]', '7.0.11'),
('20231010120504_[ChangeEvaluationToINT2]', '7.0.11'),
('20231012102702_[GlobalSettings]', '7.0.11'),
('20231012103124_[NewDB]', '7.0.11'),
('20231012130432_[GlobalSettingsChange]', '7.0.11'),
('20231026101803_NewDatabase', '7.0.11'),
('20231026102621_AddProdukcja', '7.0.11'),
('20231030095308_UserEnableBool', '7.0.11'),
('20231102133421_User-ManagerId', '7.0.11'),
('20231130114833_answer', '7.0.11'),
('20231217220512_RenameEvaluationsToEvaluationBiuro', '7.0.11'),
('20240105112358_produkcjaPlusquestion', '7.0.11'),
('20240105113002_produkcjaPlusquestion1', '7.0.11'),
('20240105120127_addSaltToUser', '7.0.11'),
('20240105121117_reverseSalt', '7.0.11'),
('20240110082434_department', '7.0.11'),
('20240110082503_departments', '7.0.11'),
('20240110125314_DepartmentEnabledUpdate', '7.0.11'),
('20240111065633_userAddDepartmentID', '7.0.11'),
('20240111070210_userRemoveDepartmentID', '7.0.11'),
('20240111070401_evaluationAddDepartmentID', '7.0.11'),
('20240111193746_addingStanowisko', '7.0.11');

--
-- Indeksy dla zrzutów tabel
--

--
-- Indeksy dla tabeli `department`
--
ALTER TABLE `department`
  ADD PRIMARY KEY (`DepartmentID`);

--
-- Indeksy dla tabeli `evaluationbiuro`
--
ALTER TABLE `evaluationbiuro`
  ADD PRIMARY KEY (`EvaluationID`);

--
-- Indeksy dla tabeli `evaluationbiuroanswers`
--
ALTER TABLE `evaluationbiuroanswers`
  ADD PRIMARY KEY (`EvaluationID`);

--
-- Indeksy dla tabeli `evaluationnames`
--
ALTER TABLE `evaluationnames`
  ADD PRIMARY KEY (`EvaluatorNameID`);

--
-- Indeksy dla tabeli `evaluationprodukcjaanswers`
--
ALTER TABLE `evaluationprodukcjaanswers`
  ADD PRIMARY KEY (`EvaluationID`);

--
-- Indeksy dla tabeli `evaluationsprodukcja`
--
ALTER TABLE `evaluationsprodukcja`
  ADD PRIMARY KEY (`EvaluationID`);

--
-- Indeksy dla tabeli `globalsettings`
--
ALTER TABLE `globalsettings`
  ADD PRIMARY KEY (`Id`);

--
-- Indeksy dla tabeli `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`UserID`);

--
-- Indeksy dla tabeli `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `department`
--
ALTER TABLE `department`
  MODIFY `DepartmentID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;

--
-- AUTO_INCREMENT for table `evaluationbiuro`
--
ALTER TABLE `evaluationbiuro`
  MODIFY `EvaluationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `evaluationbiuroanswers`
--
ALTER TABLE `evaluationbiuroanswers`
  MODIFY `EvaluationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `evaluationnames`
--
ALTER TABLE `evaluationnames`
  MODIFY `EvaluatorNameID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `evaluationprodukcjaanswers`
--
ALTER TABLE `evaluationprodukcjaanswers`
  MODIFY `EvaluationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `evaluationsprodukcja`
--
ALTER TABLE `evaluationsprodukcja`
  MODIFY `EvaluationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `globalsettings`
--
ALTER TABLE `globalsettings`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `UserID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
