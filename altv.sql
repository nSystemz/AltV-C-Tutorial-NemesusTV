-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 13. Jan 2022 um 14:24
-- Server-Version: 10.4.21-MariaDB
-- PHP-Version: 8.0.11

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Datenbank: `altv`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `accounts`
--

CREATE TABLE `accounts` (
  `id` int(11) NOT NULL,
  `name` varchar(35) NOT NULL,
  `password` varchar(128) NOT NULL,
  `geld` int(11) NOT NULL DEFAULT 5000,
  `adminlevel` int(1) NOT NULL DEFAULT 0,
  `fraktion` int(2) NOT NULL DEFAULT 0,
  `rang` int(2) NOT NULL DEFAULT 0,
  `payday` int(2) NOT NULL DEFAULT 60,
  `posx` float NOT NULL,
  `posy` float NOT NULL,
  `posz` float NOT NULL,
  `posa` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Daten für Tabelle `accounts`
--

INSERT INTO `accounts` (`id`, `name`, `password`, `geld`, `adminlevel`, `fraktion`, `rang`, `payday`, `posx`, `posy`, `posz`, `posa`) VALUES
(2, 'Nemesus', '$2a$10$GanlwtL5ZdTDn/I2F2GiFeIWuVLAOc7ONQENCpmeAawQ6IKU7lwDu', 5000, 3, 0, 0, 60, -427.002, 1115, 326.763, 0);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `fahrzeuge`
--

CREATE TABLE `fahrzeuge` (
  `id` int(11) NOT NULL,
  `owner` int(11) NOT NULL,
  `vehicleName` varchar(50) NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `posA` float NOT NULL,
  `vehicleLock` int(1) NOT NULL,
  `fuel` float NOT NULL,
  `engine` int(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Daten für Tabelle `fahrzeuge`
--

INSERT INTO `fahrzeuge` (`id`, `owner`, `vehicleName`, `posX`, `posY`, `posZ`, `posA`, `vehicleLock`, `fuel`, `engine`) VALUES
(1, 2, 'Sultan', -376.747, 1180, 325.112, -1.95458, 1, 0, 0),
(2, 2, 'Infernus', -409.793, 1242.67, 325.213, -0.242977, 1, 17, 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `whitelist`
--

CREATE TABLE `whitelist` (
  `id` int(11) NOT NULL,
  `socialclubid` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Daten für Tabelle `whitelist`
--

INSERT INTO `whitelist` (`id`, `socialclubid`, `timestamp`) VALUES
(3, 268709457, '2022-01-13 12:33:56');

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `fahrzeuge`
--
ALTER TABLE `fahrzeuge`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `whitelist`
--
ALTER TABLE `whitelist`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `fahrzeuge`
--
ALTER TABLE `fahrzeuge`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT für Tabelle `whitelist`
--
ALTER TABLE `whitelist`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
