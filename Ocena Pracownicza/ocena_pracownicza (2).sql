-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 11, 2023 at 01:48 PM
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

DELIMITER $$
--
-- Procedury
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY` (IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255), IN `COLUMN_NAME_ARGUMENT` VARCHAR(255))   BEGIN
	DECLARE HAS_AUTO_INCREMENT_ID INT(11);
	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
	DECLARE SQL_EXP VARCHAR(1000);
	SELECT COUNT(*)
		INTO HAS_AUTO_INCREMENT_ID
		FROM `information_schema`.`COLUMNS`
		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
			AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
			AND `COLUMN_TYPE` LIKE '%int%'
			AND `COLUMN_KEY` = 'PRI';
	IF HAS_AUTO_INCREMENT_ID THEN
		SELECT `COLUMN_TYPE`
			INTO PRIMARY_KEY_TYPE
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
				AND `COLUMN_TYPE` LIKE '%int%'
				AND `COLUMN_KEY` = 'PRI';
		SELECT `COLUMN_NAME`
			INTO PRIMARY_KEY_COLUMN_NAME
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
				AND `COLUMN_TYPE` LIKE '%int%'
				AND `COLUMN_KEY` = 'PRI';
		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL AUTO_INCREMENT;');
		SET @SQL_EXP = SQL_EXP;
		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
		EXECUTE SQL_EXP_EXECUTE;
		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
	END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY` (IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255))   BEGIN
	DECLARE HAS_AUTO_INCREMENT_ID TINYINT(1);
	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
	DECLARE SQL_EXP VARCHAR(1000);
	SELECT COUNT(*)
		INTO HAS_AUTO_INCREMENT_ID
		FROM `information_schema`.`COLUMNS`
		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
			AND `Extra` = 'auto_increment'
			AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
	IF HAS_AUTO_INCREMENT_ID THEN
		SELECT `COLUMN_TYPE`
			INTO PRIMARY_KEY_TYPE
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
		SELECT `COLUMN_NAME`
			INTO PRIMARY_KEY_COLUMN_NAME
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL;');
		SET @SQL_EXP = SQL_EXP;
		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
		EXECUTE SQL_EXP_EXECUTE;
		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
	END IF;
END$$

DELIMITER ;

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
  `Question4` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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
  `EvaluatorNameID` int(11) NOT NULL DEFAULT 0,
  `Question10` longtext NOT NULL,
  `Question11` longtext NOT NULL,
  `Question7` longtext NOT NULL,
  `Question8` longtext NOT NULL,
  `Question9` longtext NOT NULL,
  `EvaluationAnswerID` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `evaluations`
--

INSERT INTO `evaluations` (`EvaluationID`, `UserName`, `UserID`, `Date`, `Question1`, `Question2`, `Question3`, `Question4`, `Question5`, `Question6`, `EvaluatorNameID`, `Question10`, `Question11`, `Question7`, `Question8`, `Question9`, `EvaluationAnswerID`) VALUES
(8, '1', 1, '2023-10-30 10:50:25.128737', '1', '1', '1', '1', '1', '1', 8, '1', '1', '1', '1', '1', 0),
(9, 'test2', 5, '2023-11-03 08:21:01.431253', 'test2', 'test2', 'test2', 'test2', 'test2test2', 'test2', 8, 'test2', 'test2', 'test2', 'test2', 'test2', 0),
(10, 'asdasda', 1, '2023-11-30 13:18:55.400147', 'asdasda', 'asdasda', 'asdasda', 'asdasda', 'asdasda', 'asdasda', 8, 'asdasda', 'asdasda\r\n', 'asdasda', 'asdasda', 'asdasda', 0);

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
  `EvaluationAnswerID` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `evaluationsprodukcja`
--

INSERT INTO `evaluationsprodukcja` (`EvaluationID`, `UserName`, `UserID`, `EvaluatorNameID`, `Date`, `Question1`, `Question2`, `Question3`, `Question4`, `EvaluationAnswerID`) VALUES
(1, '2', 1, 8, '2023-10-30 10:50:38.159158', '2', '2', '2', '2', 0),
(2, 'test2test2', 5, 8, '2023-11-03 08:21:12.639503', 'test2test2', 'test2test2', 'test2test2', 'test2test2', 0),
(3, 'sadasd', 5, 8, '2023-12-11 10:12:49.506658', 'sadasd', 'sadasd', 'sadasd', 'sadasd', 0);

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
(1, 'Testowy Test', 'test2', 'test', 1, NULL),
(2, 'Administrator', 'admin', 'admin', 1, NULL),
(3, 'Ksawery Nowak', 'test2', 'test2', 1, NULL),
(4, 'Sebastian D', 'test4', 'test4', 1, NULL),
(5, 'test2', 'test22', 'test2', 1, 1);

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
('20231130114833_answer', '7.0.11');

--
-- Indeksy dla zrzutów tabel
--

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
-- Indeksy dla tabeli `evaluations`
--
ALTER TABLE `evaluations`
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
-- AUTO_INCREMENT for table `evaluationbiuroanswers`
--
ALTER TABLE `evaluationbiuroanswers`
  MODIFY `EvaluationID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `evaluationnames`
--
ALTER TABLE `evaluationnames`
  MODIFY `EvaluatorNameID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `evaluationprodukcjaanswers`
--
ALTER TABLE `evaluationprodukcjaanswers`
  MODIFY `EvaluationID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `evaluations`
--
ALTER TABLE `evaluations`
  MODIFY `EvaluationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `evaluationsprodukcja`
--
ALTER TABLE `evaluationsprodukcja`
  MODIFY `EvaluationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `globalsettings`
--
ALTER TABLE `globalsettings`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `UserID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
