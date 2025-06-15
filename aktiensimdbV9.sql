-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 15. Jun 2025 um 21:08
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
(1, 'DAX', 'Deutscher Aktienindex', 13845.96, 16355.99),
(2, 'DHL', 'Deutsche Post DHL Group', 41.06, 40.91),
(3, 'LHA', 'Lufthansa AG', 3.62, 3.91),
(4, 'SAP', 'SAP SE', 72.30, 93.06),
(5, 'BMW', 'Bayerische Motoren Werke AG', 65.57, 101.28);

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
(2, 2, 2, 'Herwardt', 'Thomas', 'her@gmail.com', '2025-05-26'),
(3, 3, 3, 'Mustermann', 'Max', '0@0', '2025-05-30'),
(4, 4, 4, 'Müllermann', 'Maxime', '1@1', '2025-06-11');

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

--
-- Daten für Tabelle `depot`
--

INSERT INTO `depot` (`id`, `benutzer_id`, `name`, `erstellt`) VALUES
(1, 1, 'Standarddepot', '2025-06-11 05:50:09'),
(2, 2, 'Standarddepot', '2025-06-11 05:50:09'),
(3, 3, 'tolles Depot', '2025-06-11 05:51:08'),
(4, 4, 'My Depot', '2025-06-11 12:38:35');

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
  `Kontostand` int(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `konto`
--

INSERT INTO `konto` (`KontoID`, `ID_Benutzer`, `Kontostand`) VALUES
(1, 1, 0),
(2, 2, 0),
(3, 3, 7232),
(4, 4, 1001);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `kredite`
--

CREATE TABLE `kredite` (
  `KreditID` int(30) NOT NULL,
  `ID_Benutzer` int(11) NOT NULL,
  `Betrag` double(30,2) NOT NULL,
  `Zinssatz` int(30) NOT NULL,
  `Restschuld` double(30,2) NOT NULL,
  `Laufzeit` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `kredite`
--

INSERT INTO `kredite` (`KreditID`, `ID_Benutzer`, `Betrag`, `Zinssatz`, `Restschuld`, `Laufzeit`) VALUES
(22, 8, 100.00, 10, 110.00, 4),
(0, 3, 201.00, 10, 221.10, 4),
(0, 4, 10000.00, 10, 11000.00, 4),
(0, 3, 10000.00, 10, 11000.00, 4);

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
('her@gmail.com', 2, 2, '7110eda4d09e062aa5e4a390b0a572ac0d2c0220'),
('0@0', 3, 3, '298b5d2a0e8f1ce32457989a54298a0dd9c07682'),
('1@1', 4, 4, '03c22ee7364e9f286d4d4b042f755f0bca38227d');

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

--
-- Daten für Tabelle `transaktion`
--

INSERT INTO `transaktion` (`id`, `aktie_ID`, `typ`, `anzahl`, `einzelpreis`, `zeitpunkt`, `depot_ID`) VALUES
(1, 2, 'Kauf', 2.9499999999999997, 43, '2025-06-11 05:51:18', 3),
(2, 2, 'Kauf', 0, 43, '2025-06-11 12:38:42', 4),
(4, 4, 'Kauf', 0.59, 106, '2025-06-11 12:49:38', 3),
(6, 3, 'Kauf', 1.64, 5, '2025-06-11 12:49:39', 3),
(7, 4, 'Kauf', 0.01, 106, '2025-06-11 12:49:39', 3),
(54, 1, 'Kauf', 0, 15768, '2025-06-15 20:06:53', 2);

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
-- Daten für Tabelle `ueberweisung`
--

INSERT INTO `ueberweisung` (`id`, `absender_id`, `empfaenger_id`, `betrag`, `datum`) VALUES
(1, 3, 4, 1000.00, '2025-06-15 20:45:35');

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
  MODIFY `BenutzerID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

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
-- AUTO_INCREMENT für Tabelle `konto`
--
ALTER TABLE `konto`
  MODIFY `KontoID` int(30) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT für Tabelle `logininfo`
--
ALTER TABLE `logininfo`
  MODIFY `LoginID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT für Tabelle `transaktion`
--
ALTER TABLE `transaktion`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=59;

--
-- AUTO_INCREMENT für Tabelle `ueberweisung`
--
ALTER TABLE `ueberweisung`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

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
