-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Paź 12, 2023 at 02:45 PM
-- Wersja serwera: 10.4.28-MariaDB
-- Wersja PHP: 8.2.4

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
(6, 'trzy');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `evaluations`
--

CREATE TABLE `evaluations` (
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
  `EvaluatorNameID` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `globalsettings`
--

CREATE TABLE `globalsettings` (
  `CurrentEvaluationName` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `users`
--

CREATE TABLE `users` (
  `UserID` int(11) NOT NULL,
  `FullName` longtext NOT NULL,
  `Password` longtext NOT NULL,
  `Login` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`UserID`, `FullName`, `Password`, `Login`) VALUES
(1, 'Testowy Test', 'test', 'test'),
(2, 'Administrator', 'admin', 'admin');

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
('20231012103124_[NewDB]', '7.0.11');

--
-- Indeksy dla zrzutów tabel
--

--
-- Indeksy dla tabeli `evaluationnames`
--
ALTER TABLE `evaluationnames`
  ADD PRIMARY KEY (`EvaluatorNameID`);

--
-- Indeksy dla tabeli `evaluations`
--
ALTER TABLE `evaluations`
  ADD PRIMARY KEY (`EvaluationID`);

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
-- AUTO_INCREMENT for table `evaluationnames`
--
ALTER TABLE `evaluationnames`
  MODIFY `EvaluatorNameID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `evaluations`
--
ALTER TABLE `evaluations`
  MODIFY `EvaluationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `UserID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
