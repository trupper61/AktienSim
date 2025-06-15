-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 15. Jun 2025 um 22:16
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
  `Wert` decimal(15,2) NOT NULL,
  `letzterschluss` decimal(15,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `aktiendaten`
--

INSERT INTO `aktiendaten` (`aktienID`, `Firma`, `Name`, `Wert`, `letzterschluss`) VALUES
(1, 'DAX', 'Deutscher Aktienindex', 72.33, 71.69),
(2, 'DHL', 'Deutsche Post DHL Group', 0.22, 0.25),
(3, 'LHA', 'Lufthansa AG', 0.10, 0.14),
(4, 'SAP', 'SAP SE', 19.45, 24.08),
(5, 'BMW', 'Bayerische Motoren Werke AG', 12.40, 12.56);

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
(1, 0, 1, 'Mustermann', 'Max', '0@0.com', '2025-06-15');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `depot`
--

CREATE TABLE `depot` (
  `id` int(11) NOT NULL,
  `benutzer_id` int(11) DEFAULT NULL,
  `name` varchar(100) DEFAULT NULL,
  `erstellt` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `ereignisse`
--

CREATE TABLE `ereignisse` (
  `id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `einfluss_prozent` double DEFAULT NULL,
  `beschreibung` text DEFAULT NULL,
  `typ` enum('global','lokal') DEFAULT NULL,
  `aktiv` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `ereignisse`
--

INSERT INTO `ereignisse` (`id`, `name`, `einfluss_prozent`, `beschreibung`, `typ`, `aktiv`) VALUES
(1, 'Marktkrise', -0.25, 'Weltwirtschaftskrise – Panikverkäufe!', 'global', 1),
(2, 'Zinserhöhung', -0.1, 'Zinsen steigen – Aktien unter Druck.', 'global', 1),
(3, 'Staatshilfe', 0.15, 'Staat stützt Finanzbranche.', 'global', 1),
(4, 'Skandal', -0.12, 'CEO tritt wegen Korruption zurück.', 'lokal', 1),
(5, 'Produktinnovation', 0.08, 'Revolutionäres Produkt vorgestellt.', 'lokal', 1),
(6, 'Verklagt', -0.1, 'Klage wegen Patentverletzung.', 'lokal', 1),
(7, 'Übernahmegerücht', 0.06, 'Mögliche Übernahme durch Großkonzern.', 'lokal', 1),
(8, 'Guter Quartalsbericht', 0.05, 'Umsatzwachstum übertrifft Erwartungen.', 'lokal', 1),
(9, 'Schlechter Quartalsbericht', -0.05, 'Verluste wegen Lieferproblemen.', 'lokal', 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `konto`
--

CREATE TABLE `konto` (
  `KontoID` int(30) NOT NULL,
  `ID_Benutzer` int(30) NOT NULL,
  `Kontostand` int(50) NOT NULL,
  `KreditRating` varchar(11) NOT NULL,
  `KreditScore` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `konto`
--

INSERT INTO `konto` (`KontoID`, `ID_Benutzer`, `Kontostand`, `KreditRating`, `KreditScore`) VALUES
(0, 1, 0, 'C', 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `kredite`
--

CREATE TABLE `kredite` (
  `KreditID` int(30) NOT NULL,
  `ID_Benutzer` int(30) NOT NULL,
  `Betrag` double(50,2) NOT NULL,
  `Zinssatz` int(10) NOT NULL,
  `Restschuld` double(50,2) NOT NULL,
  `Laufzeit` int(10) NOT NULL,
  `Rate` double(30,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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
('0@0.com', 1, 1, '356a192b7913b04c54574d18c28d46e6395428ab');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `transaktion`
--

CREATE TABLE `transaktion` (
  `id` int(11) NOT NULL,
  `aktie_ID` int(11) DEFAULT NULL,
  `typ` enum('Kauf','Verkauf') DEFAULT NULL,
  `anzahl` double DEFAULT NULL,
  `einzelpreis` decimal(10,0) DEFAULT NULL,
  `zeitpunkt` datetime DEFAULT NULL,
  `depot_ID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `ueberweisung`
--

CREATE TABLE `ueberweisung` (
  `id` int(11) NOT NULL,
  `absender_id` int(11) NOT NULL,
  `empfaenger_id` int(11) NOT NULL,
  `betrag` decimal(10,2) NOT NULL,
  `datum` datetime NOT NULL DEFAULT current_timestamp()
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
-- Indizes für die Tabelle `depot`
--
ALTER TABLE `depot`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `ereignisse`
--
ALTER TABLE `ereignisse`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `logininfo`
--
ALTER TABLE `logininfo`
  ADD PRIMARY KEY (`LoginID`,`Email`);

--
-- Indizes für die Tabelle `transaktion`
--
ALTER TABLE `transaktion`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `ueberweisung`
--
ALTER TABLE `ueberweisung`
  ADD PRIMARY KEY (`id`),
  ADD KEY `absender_id` (`absender_id`),
  ADD KEY `empfaenger_id` (`empfaenger_id`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `aktiendaten`
--
ALTER TABLE `aktiendaten`
  MODIFY `aktienID` int(25) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT für Tabelle `benutzer`
--
ALTER TABLE `benutzer`
  MODIFY `BenutzerID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT für Tabelle `depot`
--
ALTER TABLE `depot`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT für Tabelle `ereignisse`
--
ALTER TABLE `ereignisse`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT für Tabelle `logininfo`
--
ALTER TABLE `logininfo`
  MODIFY `LoginID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT für Tabelle `transaktion`
--
ALTER TABLE `transaktion`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `ueberweisung`
--
ALTER TABLE `ueberweisung`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `ueberweisung`
--
ALTER TABLE `ueberweisung`
  ADD CONSTRAINT `ueberweisung_ibfk_1` FOREIGN KEY (`absender_id`) REFERENCES `benutzer` (`BenutzerID`),
  ADD CONSTRAINT `ueberweisung_ibfk_2` FOREIGN KEY (`empfaenger_id`) REFERENCES `benutzer` (`BenutzerID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
