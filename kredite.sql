-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 12, 2025 at 04:11 PM
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
-- Table structure for table `kredite`
--

CREATE TABLE `kredite` (
  `KreditID` int(30) NOT NULL,
  `ID_Benutzer` int(30) NOT NULL,
  `Betrag` double(50,2) NOT NULL,
  `Zinssatz` int(10) NOT NULL,
  `Restschuld` double(50,2) NOT NULL,
  `Laufzeit` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `kredite`
--

INSERT INTO `kredite` (`KreditID`, `ID_Benutzer`, `Betrag`, `Zinssatz`, `Restschuld`, `Laufzeit`) VALUES
(1, 1, 100.00, 10, 110.00, 4),
(2, 1, 1000.00, 10, 1100.00, 6),
(4, 5, 100.00, 10, 110.00, 4);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `kredite`
--
ALTER TABLE `kredite`
  ADD PRIMARY KEY (`KreditID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `kredite`
--
ALTER TABLE `kredite`
  MODIFY `KreditID` int(30) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
