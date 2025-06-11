-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 09, 2025 at 12:16 PM
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
(1, 'DAX', 'Deutscher Aktienindex', 22168.76, 22067.43),
(2, 'DHL', 'Deutsche Post DHL Group', 39.69, 38.78),
(3, 'LHA', 'Lufthansa AG', 8.23, 8.43),
(4, 'SAP', 'SAP SE', 159.99, 164.93),
(5, 'BMW', 'Bayerische Motoren Werke AG', 106.36, 103.22);

-- --------------------------------------------------------

--
-- Table structure for table `aktiendepot`
--

CREATE TABLE `aktiendepot` (
  `aktienID` int(11) NOT NULL,
  `depotID` int(11) NOT NULL,
  `Anteil` float DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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
(7, 7, 7, 'Bozkurt', 'Denis', 'deniboz11@gmail.com', '2025-06-07'),
(8, 8, 8, 'Operator', 'Admin', '1', '2025-06-08');

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
  `Kontostand` double(30,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `konto`
--

INSERT INTO `konto` (`KontoID`, `ID_Benutzer`, `Kontostand`) VALUES
(7, 7, 17300.00),
(8, 8, 16632.00);

-- --------------------------------------------------------

--
-- Table structure for table `kredite`
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
-- Dumping data for table `kredite`
--

INSERT INTO `kredite` (`KreditID`, `ID_Benutzer`, `Betrag`, `Zinssatz`, `Restschuld`, `Laufzeit`) VALUES
(22, 8, 100.00, 10, 110.00, 4);

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
('deniboz11@gmail.com', 7, 7, '7110eda4d09e062aa5e4a390b0a572ac0d2c0220'),
('1', 8, 8, '356a192b7913b04c54574d18c28d46e6395428ab');

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
-- Indexes for table `ereignisse`
--
ALTER TABLE `ereignisse`
  ADD PRIMARY KEY (`EreignisID`);

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
  MODIFY `BenutzerID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `ereignisse`
--
ALTER TABLE `ereignisse`
  MODIFY `EreignisID` int(30) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `konto`
--
ALTER TABLE `konto`
  MODIFY `KontoID` int(30) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `kredite`
--
ALTER TABLE `kredite`
  MODIFY `KreditID` int(30) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT for table `logininfo`
--
ALTER TABLE `logininfo`
  MODIFY `LoginID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `transaktionshistorie`
--
ALTER TABLE `transaktionshistorie`
  MODIFY `transaktionID` int(30) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
