-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Sty 10, 2024 at 02:45 PM
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
(1, 'test', 1, 1),
(2, 'tests', 1, 1);

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
  `EvaluationAnswerID` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `evaluationbiuro`
--

INSERT INTO `evaluationbiuro` (`EvaluationID`, `UserName`, `UserID`, `Date`, `Question1`, `Question2`, `Question3`, `Question4`, `Question5`, `Question6`, `EvaluatorNameID`, `Question10`, `Question11`, `Question7`, `Question8`, `Question9`, `EvaluationAnswerID`) VALUES
(8, '1', 1, '2023-10-30 10:50:25.128737', '1', '1', '1', '1', '1', '1', 8, '1', '1', '1', '1', '1', 2),
(9, 'test2', 5, '2023-11-03 08:21:01.431253', 'test2', 'test2', 'test2', 'test2', 'test2test2', 'test2', 8, 'test2', 'test2', 'test2', 'test2', 'test2', 0),
(10, 'asdasda', 1, '2023-11-30 13:18:55.400147', 'asdasda', 'asdasda', 'asdasda', 'asdasda', 'asdasda', 'asdasda', 8, 'asdasda', 'asdasda\r\n', 'asdasda', 'asdasda', 'asdasda', 4),
(11, 'Imie', 1, '2024-01-08 09:58:35.024598', 'rezultaty', 'dzialania', 'uczciwosc', 'odpowie', 'zaanga', 'bliskie', 8, 'okres spo', 'uwagi', 'inno', 'utrudnia', 'nad czym', 3);

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

--
-- Dumping data for table `evaluationbiuroanswers`
--

INSERT INTO `evaluationbiuroanswers` (`EvaluationID`, `Question1`, `Question2`, `Question3`, `Question4`, `Question5`, `Question6`, `Question7`, `Question8`, `Question9`, `Question10`, `Question11`) VALUES
(1, 'test', 'test', 'test', 'test', 'test', 'test', 'test', 'test', 'test', 'test', 'test'),
(2, 'test', 'test', 'test', 'test', 'test', 'test', 'test', 'test', 'test', 'test', 'test'),
(3, 'imie2', 'dzialania2', 'uczciw', 'odpowie', 'zaang', 'bliskie', 'innowa', 'utrudnia', 'praco', 'dazenie', 'uwagi'),
(4, 'asdasdas', 'dddas', 'dsasad', 'asdasd', 'asdasd', 'asdas', 'dasdas', 'dasd', 'asdas', 'dasd', 'sad');

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
(4, 'jeden'),
(5, 'dwa'),
(6, 'trzy'),
(7, 'asd'),
(8, 'yes');

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

--
-- Dumping data for table `evaluationprodukcjaanswers`
--

INSERT INTO `evaluationprodukcjaanswers` (`EvaluationID`, `Question1`, `Question2`, `Question3`, `Question4`, `Question5`) VALUES
(1, 'test', 'test', 'test', 'test', ''),
(2, 'test', 'test', 'test', 'test', ''),
(4, 'test', 'test', 'test', 'test', ''),
(5, 'asdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasd', 'asdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasd', 'asdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasd', 'asdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasd', 'asdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasd'),
(6, 'qsdq', 'asdasd', 'asdad', 'asdasd', 'asdasdas'),
(7, 'test', 'test', 'test', 'test', 'test'),
(8, 'test', 'test', 'test', 'test', 'test'),
(9, 'asdasd', 'asdas', 'dasdasd', 'asdasd', 'asdasd');

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
  `Question5` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `evaluationsprodukcja`
--

INSERT INTO `evaluationsprodukcja` (`EvaluationID`, `UserName`, `UserID`, `EvaluatorNameID`, `Date`, `Question1`, `Question2`, `Question3`, `Question4`, `EvaluationAnswerID`, `Question5`) VALUES
(1, '2', 1, 8, '2023-10-30 10:50:38.159158', '2', '2', '2', '2', 8, ''),
(2, 'test2test2', 5, 8, '2023-11-03 08:21:12.639503', 'test2test2', 'test2test2', 'test2test2', 'test2test2', 9, ''),
(3, 'sadasd', 5, 8, '2023-12-11 10:12:49.506658', 'sadasd', 'sadasd', 'sadasd', 'sadasd', 4, ''),
(4, 'test', 1, 8, '2024-01-05 12:32:03.771647', 'test', 'test', 'test', 'test', 6, ''),
(5, 'test2da', 1, 8, '2024-01-05 12:32:11.819484', 'test', 'test', 'test', 'test', 5, 'test2');

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
('yes', 1);

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
(1, 'Testowy Test', '$2a$11$FXeaQASyorttes6gwmvV4eYqjVyJKWJVEHrdUazjndV14yT9VcSXi', 'test', 1, NULL),
(3, 'Ksawery Nowak', 'test2', 'test2', 1, NULL),
(4, 'Sebastian D', 'test4', 'test4', 1, NULL),
(5, 'test2', 'test22', 'test2', 1, 1),
(6, 'tasdasda', 'asdasdasd', 'dtsadasd', 1, NULL),
(7, 'Administrator', '$2a$11$VWSBHGWg8/bifft6h443ROeexQ1ZnsnOjZ4DoOQh8AYV4P8zFj5c.', 'admin', 1, NULL);

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
('20240110125314_DepartmentEnabledUpdate', '7.0.11');

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
  MODIFY `DepartmentID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `evaluationbiuro`
--
ALTER TABLE `evaluationbiuro`
  MODIFY `EvaluationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `evaluationbiuroanswers`
--
ALTER TABLE `evaluationbiuroanswers`
  MODIFY `EvaluationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `evaluationnames`
--
ALTER TABLE `evaluationnames`
  MODIFY `EvaluatorNameID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `evaluationprodukcjaanswers`
--
ALTER TABLE `evaluationprodukcjaanswers`
  MODIFY `EvaluationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `evaluationsprodukcja`
--
ALTER TABLE `evaluationsprodukcja`
  MODIFY `EvaluationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `globalsettings`
--
ALTER TABLE `globalsettings`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `UserID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
