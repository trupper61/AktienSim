-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 26. Mai 2025 um 14:10
-- Server-Version: 10.4.32-MariaDB
-- PHP-Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Datenbank: `aktiensimdb`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `aktiendaten`
--

CREATE TABLE `aktiendaten` (
  `aktienID` int(25) NOT NULL,
  `Firma` varchar(30) NOT NULL,
  `Name` varchar(30) NOT NULL,
  `Wert` int(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `aktiendaten`
--

INSERT INTO `aktiendaten` (`aktienID`, `Firma`, `Name`, `Wert`) VALUES
(1, 'DAX', 'Test', 0),
(2, 'DHL', 'Test', 0),
(3, 'Lufthansa', 'Test', 0);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `aktiendepot`
--

CREATE TABLE `aktiendepot` (
  `aktienID` int(11) NOT NULL,
  `depotID` int(11) NOT NULL,
  `Anteil` float DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `benutzer`
--

CREATE TABLE `benutzer` (
  `BenutzerID` int(10) NOT NULL,
  `ID_Konto` int(10) NOT NULL,
  `ID_Login` int(10) NOT NULL,
  `Name` varchar(30) NOT NULL,
  `Vorname` varchar(30) NOT NULL,
  `Email` varchar(30) NOT NULL,
  `MitgliedSeit` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `benutzer`
--

INSERT INTO `benutzer` (`BenutzerID`, `ID_Konto`, `ID_Login`, `Name`, `Vorname`, `Email`, `MitgliedSeit`) VALUES
(1, 1, 1, 'Bozkurt', 'Denis', 'deniboz11@gmail.com', '2025-05-26'),
(2, 2, 2, 'Herwardt', 'Thomas', 'her@gmail.com', '2025-05-26');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `konto`
--

CREATE TABLE `konto` (
  `KontoID` int(30) NOT NULL,
  `ID_Benutzer` int(30) NOT NULL,
  `Kontostand` int(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `konto`
--

INSERT INTO `konto` (`KontoID`, `ID_Benutzer`, `Kontostand`) VALUES
(1, 1, 1800),
(2, 2, 0);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `logininfo`
--

CREATE TABLE `logininfo` (
  `Email` varchar(30) NOT NULL,
  `ID_Benutzer` int(10) NOT NULL,
  `LoginID` int(10) NOT NULL,
  `passwort` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `logininfo`
--

INSERT INTO `logininfo` (`Email`, `ID_Benutzer`, `LoginID`, `passwort`) VALUES
('deniboz11@gmail.com', 1, 1, '7110eda4d09e062aa5e4a390b0a572ac0d2c0220'),
('her@gmail.com', 2, 2, '7110eda4d09e062aa5e4a390b0a572ac0d2c0220');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `transaktionshistorie`
--

CREATE TABLE `transaktionshistorie` (
  `transaktionID` int(30) NOT NULL,
  `Placeholder1` int(30) NOT NULL,
  `Placeholder2` int(30) NOT NULL,
  `Placeholder3` int(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `aktiendaten`
--
ALTER TABLE `aktiendaten`
  ADD PRIMARY KEY (`aktienID`);

--
-- Indizes für die Tabelle `benutzer`
--
ALTER TABLE `benutzer`
  ADD PRIMARY KEY (`BenutzerID`);

--
-- Indizes für die Tabelle `konto`
--
ALTER TABLE `konto`
  ADD PRIMARY KEY (`KontoID`);

--
-- Indizes für die Tabelle `logininfo`
--
ALTER TABLE `logininfo`
  ADD PRIMARY KEY (`LoginID`,`Email`);

--
-- Indizes für die Tabelle `transaktionshistorie`
--
ALTER TABLE `transaktionshistorie`
  ADD PRIMARY KEY (`transaktionID`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `aktiendaten`
--
ALTER TABLE `aktiendaten`
  MODIFY `aktienID` int(25) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT für Tabelle `benutzer`
--
ALTER TABLE `benutzer`
  MODIFY `BenutzerID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT für Tabelle `konto`
--
ALTER TABLE `konto`
  MODIFY `KontoID` int(30) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT für Tabelle `logininfo`
--
ALTER TABLE `logininfo`
  MODIFY `LoginID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT für Tabelle `transaktionshistorie`
--
ALTER TABLE `transaktionshistorie`
  MODIFY `transaktionID` int(30) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
