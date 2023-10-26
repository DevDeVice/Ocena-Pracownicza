-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Paź 26, 2023 at 12:32 PM
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
  `Question9` longtext NOT NULL
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
  `Question4` longtext NOT NULL
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
('yes', 1);

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
(2, 'Administrator', 'admin', 'admin'),
(3, 'Ksawery Nowak', 'test2', 'test2'),
(4, 'Sebastian D', 'test4', 'test4');

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
('20231026102621_AddProdukcja', '7.0.11');

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
-- AUTO_INCREMENT for table `evaluationnames`
--
ALTER TABLE `evaluationnames`
  MODIFY `EvaluatorNameID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `evaluations`
--
ALTER TABLE `evaluations`
  MODIFY `EvaluationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `evaluationsprodukcja`
--
ALTER TABLE `evaluationsprodukcja`
  MODIFY `EvaluationID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `globalsettings`
--
ALTER TABLE `globalsettings`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `UserID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
