-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 16. Jun 2025 um 00:23
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
(1, 'DAX', 'Deutscher Aktienindex', 89.86, 71.69),
(2, 'DHL', 'Deutsche Post DHL Group', 0.24, 0.25),
(3, 'LHA', 'Lufthansa AG', 0.13, 0.14),
(4, 'SAP', 'SAP SE', 22.21, 24.08),
(5, 'BMW', 'Bayerische Motoren Werke AG', 15.46, 12.56);

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
(1, 0, 1, 'Mustermann', 'Max', '0@0.com', '2025-06-15'),
(2, 0, 2, 'Mancini', 'Lela', 'LelaTMancini@gmail.com', '2025-06-16'),
(3, 0, 3, 'Hobson', 'Rodney', 'rodney.hobson@gmail.com', '2025-06-16'),
(4, 0, 4, 'Butler', 'Andrew', 'andrew.butler@gmail.com', '2025-06-16'),
(5, 0, 5, 'Tanner', 'Kevin', 'kevin.tanner@gmail.com', '2025-06-16'),
(6, 0, 6, 'Wright', 'Allen', 'allen.wright@gmail.com', '2025-06-16'),
(7, 0, 7, 'Castillo', 'Arthur', 'arthur.castillo@gmail.com', '2025-06-16'),
(8, 0, 8, 'Busch', 'Jörg', 'jörg.busch@gmail.com', '2025-06-16'),
(9, 0, 9, 'Pfeifer', 'Uta', 'uta.pfeifer@gmail.com', '2025-06-16'),
(10, 0, 10, 'Luft', 'Christina', 'christina.luft@gmail.com', '2025-06-16'),
(11, 0, 11, 'Anrooji', 'Quin', 'quin.anroji@gmail.com', '2025-06-16'),
(12, 0, 12, 'Hanekamp', 'Alfredo', 'alfredo.hanekamp@gmail.com', '2025-06-16'),
(13, 0, 13, 'Boekestijn', 'Yoep', 'yoep.boekestijn@gmail.com', '2025-06-16'),
(14, 0, 14, 'Deslauriers', 'Marmion', 'marmion.deslauriers@gmail.com', '2025-06-16'),
(15, 0, 15, 'Frappier', 'Adélaïde ', 'adelaide.frappier@gmail.com', '2025-06-16');

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
(5, 1, 'Standard', '2025-06-15 23:02:36');

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
  `Kontostand` double(50,2) NOT NULL,
  `KreditRating` varchar(11) NOT NULL,
  `KreditScore` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `konto`
--

INSERT INTO `konto` (`KontoID`, `ID_Benutzer`, `Kontostand`, `KreditRating`, `KreditScore`) VALUES
(1, 1, 115.00, 'C', 21),
(2, 2, 0.00, '2', 50),
(3, 3, 0.00, '2', 50),
(4, 4, 0.00, '2', 50),
(5, 5, 0.00, '2', 50),
(6, 6, 0.00, '2', 50),
(7, 7, 0.00, '2', 50),
(8, 8, 0.00, '2', 50),
(9, 9, 0.00, '2', 50),
(10, 10, 0.00, '2', 50),
(11, 11, 0.00, '2', 50),
(12, 12, 0.00, '2', 50),
(13, 13, 0.00, '2', 50),
(14, 14, 0.00, '2', 50),
(15, 15, 0.00, '2', 50);

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

--
-- Daten für Tabelle `kredite`
--

INSERT INTO `kredite` (`KreditID`, `ID_Benutzer`, `Betrag`, `Zinssatz`, `Restschuld`, `Laufzeit`, `Rate`) VALUES
(0, 1, 114.00, 25, 142.50, 4, 35.62);

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
('0@0.com', 1, 1, '356a192b7913b04c54574d18c28d46e6395428ab'),
('LelaTMancini@gmail.com', 2, 2, '19351a83eede2ebe244e0c39cb1e3b9e6e4deac9'),
('rodney.hobson@gmail.com', 3, 3, '6bb4208ab10a85a9c3c56532f84b6d918a92c859'),
('andrew.butler@gmail.com', 4, 4, '960bc0f0a7f0bbd8d04c6b93b8b0d83ba94ac654'),
('kevin.tanner@gmail.com', 5, 5, '864c99a67c6c2d6df59c9cd4460626ee86e68f38'),
('allen.wright@gmail.com', 6, 6, '02278623a43887406af4d60e0fcefa0d6e075954'),
('arthur.castillo@gmail.com', 7, 7, '87aaf72b44f84a845fd6feedfeaf2df99081c4dc'),
('jörg.busch@gmail.com', 8, 8, '4c25a2056f20d67d3aff6ec5acb0d6fc2ab18875'),
('uta.pfeifer@gmail.com', 9, 9, 'a5d7dbe28c08abf1306577b0005bbf2f4da4634c'),
('christina.luft@gmail.com', 10, 10, '5928759af61480c7ab65eedc21fe3923fab77046'),
('quin.anroji@gmail.com', 11, 11, 'fed5ad14737604c9ad27b02dcb533acd9e9818a8'),
('alfredo.hanekamp@gmail.com', 12, 12, 'ff24236570e1d80f635f848df5239fb9f85a4dc5'),
('yoep.boekestijn@gmail.com', 13, 13, 'e4f587c3f1219e466304f37a9ee2e780098408ce'),
('marmion.deslauriers@gmail.com', 14, 14, '31900e5186741731f175e076498196ace3ada9bd'),
('adelaide.frappier@gmail.com', 15, 15, '95433d603f0f301b4e0c8db2720ee055cc1bcec1');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `transaktion`
--

CREATE TABLE `transaktion` (
  `id` int(11) NOT NULL,
  `aktie_ID` int(11) DEFAULT NULL,
  `typ` enum('Kauf','Verkauf') DEFAULT NULL,
  `anzahl` double DEFAULT NULL,
  `einzelpreis` double(50,2) DEFAULT NULL,
  `zeitpunkt` datetime DEFAULT NULL,
  `depot_ID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `transaktion`
--

INSERT INTO `transaktion` (`id`, `aktie_ID`, `typ`, `anzahl`, `einzelpreis`, `zeitpunkt`, `depot_ID`) VALUES
(1, 2, 'Kauf', 0, 0.22, '2025-06-15 23:23:13', 5),
(2, 4, 'Kauf', 2, 19.45, '2025-06-15 23:40:25', 5);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `ueberweisung`
--

CREATE TABLE `ueberweisung` (
  `id` int(11) NOT NULL,
  `absender_id` int(11) NOT NULL,
  `empfaenger_id` int(11) NOT NULL,
  `betrag` double(20,2) NOT NULL,
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
  MODIFY `BenutzerID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT für Tabelle `depot`
--
ALTER TABLE `depot`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT für Tabelle `ereignisse`
--
ALTER TABLE `ereignisse`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT für Tabelle `konto`
--
ALTER TABLE `konto`
  MODIFY `KontoID` int(30) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT für Tabelle `logininfo`
--
ALTER TABLE `logininfo`
  MODIFY `LoginID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT für Tabelle `transaktion`
--
ALTER TABLE `transaktion`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

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
