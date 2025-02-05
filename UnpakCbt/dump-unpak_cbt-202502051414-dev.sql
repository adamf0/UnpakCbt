-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: localhost    Database: unpak_cbt
-- ------------------------------------------------------
-- Server version	5.5.5-10.4.32-MariaDB-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `bank_soal`
--

DROP TABLE IF EXISTS `bank_soal`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bank_soal` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `uuid` varchar(36) DEFAULT NULL,
  `judul` text NOT NULL,
  `rule` text DEFAULT NULL COMMENT '[tgl, gel, part,...]',
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bank_soal`
--

LOCK TABLES `bank_soal` WRITE;
/*!40000 ALTER TABLE `bank_soal` DISABLE KEYS */;
INSERT INTO `bank_soal` VALUES (1,'adce6c55-eedb-462f-9a83-c88a6a3f2430','beta test','[]',NULL,NULL);
/*!40000 ALTER TABLE `bank_soal` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `jadwal_ujian`
--

DROP TABLE IF EXISTS `jadwal_ujian`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `jadwal_ujian` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `uuid` varchar(36) DEFAULT NULL,
  `deskripsi` text DEFAULT NULL,
  `kuota` int(11) DEFAULT NULL COMMENT '-1,0...n',
  `tanggal` varchar(100) NOT NULL,
  `jam_mulai_ujian` varchar(100) NOT NULL,
  `jam_akhir_ujian` varchar(100) NOT NULL,
  `id_bank_soal` int(11) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `jadwal_ujian`
--

LOCK TABLES `jadwal_ujian` WRITE;
/*!40000 ALTER TABLE `jadwal_ujian` DISABLE KEYS */;
INSERT INTO `jadwal_ujian` VALUES (4,'fcd521e7-d99f-4d83-ba99-7eb73e3dbc31','beta test',2,'2025-02-10','07:00','20:00',1,NULL,NULL),(6,'98d7e927-65c8-4892-a085-3d53605b2f45','gamma test',1,'2025-02-06','07:00','20:00',1,NULL,NULL);
/*!40000 ALTER TABLE `jadwal_ujian` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `template_pilihan`
--

DROP TABLE IF EXISTS `template_pilihan`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `template_pilihan` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `uuid` varchar(36) DEFAULT NULL,
  `id_template_soal` int(11) DEFAULT NULL,
  `jawaban_text` text DEFAULT NULL,
  `jawaban_img` text DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `template_pilihan`
--

LOCK TABLES `template_pilihan` WRITE;
/*!40000 ALTER TABLE `template_pilihan` DISABLE KEYS */;
INSERT INTO `template_pilihan` VALUES (1,'4c71814d-b822-4bab-a85f-127139869ade',1,'1','uploads/30bqw5yv.azf.png',NULL,NULL),(2,'91b5279d-0014-4fe7-b7ba-513501c6b189',1,'2','',NULL,NULL);
/*!40000 ALTER TABLE `template_pilihan` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `template_soal`
--

DROP TABLE IF EXISTS `template_soal`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `template_soal` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `uuid` varchar(36) DEFAULT NULL,
  `id_bank_soal` int(11) DEFAULT NULL,
  `tipe` varchar(100) NOT NULL COMMENT 'TPA, Inggris, MTK',
  `pertanyaan_text` text DEFAULT NULL,
  `pertanyaan_img` text DEFAULT NULL,
  `jawaban_benar` int(11) DEFAULT NULL,
  `bobot` int(11) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  `state` varchar(100) DEFAULT NULL COMMENT 'init/null',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `template_soal`
--

LOCK TABLES `template_soal` WRITE;
/*!40000 ALTER TABLE `template_soal` DISABLE KEYS */;
INSERT INTO `template_soal` VALUES (1,'0b3c007c-8475-4253-b924-3c39e84f28d6',1,'mtk','1+1','',1,4,NULL,NULL,'');
/*!40000 ALTER TABLE `template_soal` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ujian`
--

DROP TABLE IF EXISTS `ujian`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ujian` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `uuid` varchar(36) DEFAULT NULL,
  `no_reg` varchar(100) NOT NULL,
  `id_jadwal_ujian` int(11) NOT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  `status` varchar(100) DEFAULT 'active',
  PRIMARY KEY (`id`),
  UNIQUE KEY `ujian_no_reg_IDX` (`no_reg`,`id_jadwal_ujian`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ujian`
--

LOCK TABLES `ujian` WRITE;
/*!40000 ALTER TABLE `ujian` DISABLE KEYS */;
/*!40000 ALTER TABLE `ujian` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ujian_cbt`
--

DROP TABLE IF EXISTS `ujian_cbt`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ujian_cbt` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `uuid` varchar(36) DEFAULT NULL,
  `id_ujian` int(11) NOT NULL,
  `id_template_soal` int(11) DEFAULT NULL,
  `jawaban_benar` int(11) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ujian_cbt`
--

LOCK TABLES `ujian_cbt` WRITE;
/*!40000 ALTER TABLE `ujian_cbt` DISABLE KEYS */;
/*!40000 ALTER TABLE `ujian_cbt` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'unpak_cbt'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-02-05 14:14:09
