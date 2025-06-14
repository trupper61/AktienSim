-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 14, 2025 at 08:51 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `aktiensimdb`
--

-- --------------------------------------------------------

--
-- Table structure for table `aktiendaten`
--

CREATE TABLE `aktiendaten` (
  `aktienID` int(25) NOT NULL,
  `Firma` varchar(30) NOT NULL,
  `Name` varchar(30) NOT NULL,
  `Wert` decimal(15,2) NOT NULL,
  `letzterschluss` decimal(15,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `aktiendaten`
--

INSERT INTO `aktiendaten` (`aktienID`, `Firma`, `Name`, `Wert`, `letzterschluss`) VALUES
(1, 'DAX', 'Deutscher Aktienindex', 16348.12, 16435.05),
(2, 'DHL', 'Deutsche Post DHL Group', 38.08, 38.10),
(3, 'LHA', 'Lufthansa AG', 4.64, 4.73),
(4, 'SAP', 'SAP SE', 111.83, 110.95),
(5, 'BMW', 'Bayerische Motoren Werke AG', 102.80, 103.22);

-- --------------------------------------------------------

--
-- Table structure for table `benutzer`
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
-- Dumping data for table `benutzer`
--

INSERT INTO `benutzer` (`BenutzerID`, `ID_Konto`, `ID_Login`, `Name`, `Vorname`, `Email`, `MitgliedSeit`) VALUES
(15, 13, 15, '1', '1', '1', '2025-06-14'),
(16, 14, 16, '2', '2', '2', '2025-06-14');

-- --------------------------------------------------------

--
-- Table structure for table `depot`
--

CREATE TABLE `depot` (
  `id` int(11) NOT NULL,
  `benutzer_id` int(11) DEFAULT NULL,
  `name` varchar(100) DEFAULT NULL,
  `erstellt` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `depot`
--

INSERT INTO `depot` (`id`, `benutzer_id`, `name`, `erstellt`) VALUES
(6, 16, 'Standarddepot', '2025-06-14 20:40:11');

-- --------------------------------------------------------

--
-- Table structure for table `ereignisse`
--

CREATE TABLE `ereignisse` (
  `EreignisID` int(30) NOT NULL,
  `Titel` varchar(30) NOT NULL,
  `Beschreibung` varchar(200) NOT NULL,
  `Einfluss` double(30,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `konto`
--

CREATE TABLE `konto` (
  `KontoID` int(30) NOT NULL,
  `ID_Benutzer` int(30) NOT NULL,
  `Kontostand` int(50) NOT NULL,
  `KreditRating` varchar(11) NOT NULL,
  `KreditScore` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `konto`
--

INSERT INTO `konto` (`KontoID`, `ID_Benutzer`, `Kontostand`, `KreditRating`, `KreditScore`) VALUES
(13, 15, 10080, 'C', 40),
(14, 16, -735, 'C', 30);

-- --------------------------------------------------------

--
-- Table structure for table `kredite`
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
-- Dumping data for table `kredite`
--

INSERT INTO `kredite` (`KreditID`, `ID_Benutzer`, `Betrag`, `Zinssatz`, `Restschuld`, `Laufzeit`, `Rate`) VALUES
(18, 15, 100.00, 3, 91.00, 42, 2.15),
(19, 16, 5000.00, 25, 4862.00, 14, 347.22);

-- --------------------------------------------------------

--
-- Table structure for table `logininfo`
--

CREATE TABLE `logininfo` (
  `Email` varchar(30) NOT NULL,
  `ID_Benutzer` int(10) NOT NULL,
  `LoginID` int(10) NOT NULL,
  `passwort` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `logininfo`
--

INSERT INTO `logininfo` (`Email`, `ID_Benutzer`, `LoginID`, `passwort`) VALUES
('1', 15, 15, '356a192b7913b04c54574d18c28d46e6395428ab'),
('2', 16, 16, 'da4b9237bacccdf19c0760cab7aec4a8359010b0');

-- --------------------------------------------------------

--
-- Table structure for table `transaktion`
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
-- Dumping data for table `transaktion`
--

INSERT INTO `transaktion` (`id`, `aktie_ID`, `typ`, `anzahl`, `einzelpreis`, `zeitpunkt`, `depot_ID`) VALUES
(2, 2, 'Kauf', 0, 43, '2025-06-11 12:38:42', 4),
(20, 3, 'Kauf', 2.12, 6, '2025-06-14 16:51:28', 2),
(31, 3, 'Kauf', 0.8900000000000001, 6, '2025-06-14 16:58:43', 5),
(39, 3, 'Kauf', 3.8099999999999996, 6, '2025-06-14 17:10:40', 5),
(40, 3, 'Kauf', 0.5699999999999998, 5, '2025-06-14 17:10:40', 5),
(42, 3, 'Kauf', 2.63, 5, '2025-06-14 17:10:41', 5),
(43, 3, 'Kauf', 3.47, 5, '2025-06-14 17:11:27', 3),
(44, 3, 'Kauf', 1.22, 5, '2025-06-14 17:11:27', 3),
(45, 3, 'Kauf', 0.67, 5, '2025-06-14 17:11:27', 5);

-- --------------------------------------------------------

--
-- Table structure for table `transaktionshistorie`
--

CREATE TABLE `transaktionshistorie` (
  `transaktionID` int(30) NOT NULL,
  `Sender` varchar(30) NOT NULL,
  `Empfänger` varchar(30) NOT NULL,
  `Geldanzahl` int(30) NOT NULL,
  `Datum` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `aktiendaten`
--
ALTER TABLE `aktiendaten`
  ADD PRIMARY KEY (`aktienID`);

--
-- Indexes for table `benutzer`
--
ALTER TABLE `benutzer`
  ADD PRIMARY KEY (`BenutzerID`);

--
-- Indexes for table `depot`
--
ALTER TABLE `depot`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `konto`
--
ALTER TABLE `konto`
  ADD PRIMARY KEY (`KontoID`);

--
-- Indexes for table `kredite`
--
ALTER TABLE `kredite`
  ADD PRIMARY KEY (`KreditID`);

--
-- Indexes for table `logininfo`
--
ALTER TABLE `logininfo`
  ADD PRIMARY KEY (`LoginID`,`Email`);

--
-- Indexes for table `transaktion`
--
ALTER TABLE `transaktion`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `transaktionshistorie`
--
ALTER TABLE `transaktionshistorie`
  ADD PRIMARY KEY (`transaktionID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `aktiendaten`
--
ALTER TABLE `aktiendaten`
  MODIFY `aktienID` int(25) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `benutzer`
--
ALTER TABLE `benutzer`
  MODIFY `BenutzerID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT for table `depot`
--
ALTER TABLE `depot`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `konto`
--
ALTER TABLE `konto`
  MODIFY `KontoID` int(30) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT for table `kredite`
--
ALTER TABLE `kredite`
  MODIFY `KreditID` int(30) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `logininfo`
--
ALTER TABLE `logininfo`
  MODIFY `LoginID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT for table `transaktion`
--
ALTER TABLE `transaktion`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=82;

--
-- AUTO_INCREMENT for table `transaktionshistorie`
--
ALTER TABLE `transaktionshistorie`
  MODIFY `transaktionID` int(30) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
