-- MySQL dump 10.13  Distrib 8.0.16, for Win64 (x86_64)
--
-- Host: localhost    Database: data
-- ------------------------------------------------------
-- Server version	8.0.16

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `accounts`
--

DROP TABLE IF EXISTS `accounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `accounts` (
  `id` varchar(36) NOT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `create_datetime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `update_datetime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `creator_id` varchar(36) DEFAULT NULL,
  `updater_id` varchar(36) DEFAULT NULL,
  `username` varchar(30) NOT NULL,
  `first_name` varchar(250) DEFAULT NULL,
  `middle_name` varchar(250) DEFAULT NULL,
  `last_name` varchar(250) DEFAULT NULL,
  `date_of_birth` datetime DEFAULT NULL,
  `gender` int(11) DEFAULT NULL,
  `code` varchar(100) NOT NULL,
  `facebook_user_id` text,
  `google_user_id` text,
  `phone_number` varchar(13) DEFAULT NULL,
  `phone_number_verified` tinyint(1) NOT NULL,
  `force_change_password` tinyint(1) NOT NULL,
  `hashed_password` varchar(500) NOT NULL,
  `password_salt` varchar(100) NOT NULL,
  `nation_id` varchar(36) DEFAULT NULL,
  `province_id` varchar(36) DEFAULT NULL,
  `district_id` varchar(36) DEFAULT NULL,
  `ward_id` varchar(36) DEFAULT NULL,
  `address` text,
  `cover_id` varchar(36) DEFAULT NULL,
  `email` text,
  `active` tinyint(1) NOT NULL DEFAULT '0',
  `account_type` int(11) NOT NULL DEFAULT '0',
  `second_phone_number` varchar(13) DEFAULT NULL,
  `full_name_search` varchar(250) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `ix_accounts_code` (`code`),
  UNIQUE KEY `ix_accounts_username` (`username`),
  KEY `ix_accounts_create_datetime` (`create_datetime`),
  KEY `ix_accounts_creator_id` (`creator_id`),
  KEY `ix_accounts_district_id` (`district_id`),
  KEY `ix_accounts_is_deleted` (`is_deleted`),
  KEY `ix_accounts_nation_id` (`nation_id`),
  KEY `ix_accounts_province_id` (`province_id`),
  KEY `ix_accounts_update_datetime` (`update_datetime`),
  KEY `ix_accounts_updater_id` (`updater_id`),
  KEY `ix_accounts_ward_id` (`ward_id`),
  CONSTRAINT `fk_accounts_districts_district_id` FOREIGN KEY (`district_id`) REFERENCES `districts` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_accounts_nations_nation_id` FOREIGN KEY (`nation_id`) REFERENCES `nations` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_accounts_provinces_province_id` FOREIGN KEY (`province_id`) REFERENCES `provinces` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_accounts_wards_ward_id` FOREIGN KEY (`ward_id`) REFERENCES `wards` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-12-08 15:49:34
