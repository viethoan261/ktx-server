-- MySQL dump 10.13  Distrib 8.0.31, for Win64 (x86_64)
--
-- Host: localhost    Database: ktx
-- ------------------------------------------------------
-- Server version	8.0.31

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
-- Table structure for table `maintenance_tasks`
--

DROP TABLE IF EXISTS `maintenance_tasks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `maintenance_tasks` (
  `id` int NOT NULL AUTO_INCREMENT,
  `taskType` varchar(20) NOT NULL COMMENT 'MAINTENANCE or CLEANING',
  `title` varchar(255) NOT NULL,
  `description` text,
  `location` varchar(100) NOT NULL,
  `scheduledDate` datetime NOT NULL,
  `completedDate` datetime DEFAULT NULL,
  `status` varchar(20) NOT NULL COMMENT 'SCHEDULED, IN_PROGRESS, COMPLETED, CANCELLED',
  `priority` varchar(20) NOT NULL COMMENT 'LOW, MEDIUM, HIGH, URGENT',
  `assignedTo` varchar(255) DEFAULT NULL,
  `notes` text,
  `createdDate` datetime DEFAULT NULL,
  `modifiedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `maintenance_tasks`
--

LOCK TABLES `maintenance_tasks` WRITE;
/*!40000 ALTER TABLE `maintenance_tasks` DISABLE KEYS */;
INSERT INTO `maintenance_tasks` VALUES (1,'CLEANING','Dọn vệ sinh tầng 3','Dọn vệ sinh tầng 3','Tầng 3','2025-04-19 05:05:41','2025-04-19 20:22:56','COMPLETED','MEDIUM','Nguyễn Thị A','Vệ sinh tầng 3 nhé','2025-04-19 19:06:16','2025-04-19 20:22:55'),(2,'MAINTENANCE','s','s','sss','2025-04-19 13:23:39',NULL,'CANCELLED','HIGH','ss','ss','2025-04-19 20:23:53','2025-04-19 20:27:03');
/*!40000 ALTER TABLE `maintenance_tasks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `notification_reads`
--

DROP TABLE IF EXISTS `notification_reads`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notification_reads` (
  `id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `notification_id` int NOT NULL,
  `read_at` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notification_reads`
--

LOCK TABLES `notification_reads` WRITE;
/*!40000 ALTER TABLE `notification_reads` DISABLE KEYS */;
INSERT INTO `notification_reads` VALUES (1,1,1,'2025-04-19 12:54:24'),(2,7,1,'2025-04-19 12:54:37'),(3,1,2,'2025-04-19 13:07:49'),(4,1,3,'2025-04-19 13:10:27'),(5,7,3,'2025-04-19 13:16:15'),(6,7,2,'2025-04-19 13:16:18'),(7,8,2,'2025-04-20 11:05:37');
/*!40000 ALTER TABLE `notification_reads` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `notifications`
--

DROP TABLE IF EXISTS `notifications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notifications` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(255) NOT NULL,
  `content` text NOT NULL,
  `type` varchar(20) NOT NULL COMMENT 'INTERNAL or EMERGENCY',
  `status` varchar(20) NOT NULL DEFAULT 'ACTIVE' COMMENT 'ACTIVE or INACTIVE',
  `publishDate` datetime DEFAULT NULL,
  `expiryDate` datetime DEFAULT NULL,
  `createdBy` int DEFAULT NULL,
  `createdDate` datetime DEFAULT NULL,
  `modifiedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notifications`
--

LOCK TABLES `notifications` WRITE;
/*!40000 ALTER TABLE `notifications` DISABLE KEYS */;
INSERT INTO `notifications` VALUES (1,'Thay đổi giờ đóng cửa KTX ss','từ ngày 20/4, sẽ đóng cửa KTX vào 10h thay vì 10h30 như cũ','internal','active','2025-04-18 14:31:18','2026-05-18 03:00:00',1,'2025-04-19 11:32:23','2025-04-19 12:31:21'),(2,'Test','test','emergency','active','2025-04-06 17:00:00','2025-05-28 17:00:00',1,'2025-04-19 12:58:53','2025-04-19 12:58:53');
/*!40000 ALTER TABLE `notifications` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `order`
--

DROP TABLE IF EXISTS `order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `order` (
  `id` int NOT NULL AUTO_INCREMENT,
  `studentId` int NOT NULL,
  `roomId` int NOT NULL,
  `electricity` decimal(10,2) NOT NULL,
  `water` decimal(10,2) NOT NULL,
  `service` decimal(10,2) NOT NULL,
  `room` decimal(10,2) NOT NULL,
  `total` decimal(10,2) NOT NULL,
  `status` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'Pending',
  `electricNumberPerMonth` decimal(10,2) NOT NULL,
  `waterNumberPerMonth` decimal(10,2) NOT NULL,
  `createdDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `modifiedDate` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order`
--

LOCK TABLES `order` WRITE;
/*!40000 ALTER TABLE `order` DISABLE KEYS */;
INSERT INTO `order` VALUES (9,7,5,200000.00,120000.00,200000.00,200000.00,720000.00,'Paid',20.00,30.00,'2025-04-20 11:23:58','2025-04-20 11:24:52'),(10,8,6,100000.00,60000.00,200000.00,200000.00,560000.00,'Paid',10.00,15.00,'2025-04-20 11:23:58','2025-04-20 11:24:04');
/*!40000 ALTER TABLE `order` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `price`
--

DROP TABLE IF EXISTS `price`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `price` (
  `id` int NOT NULL AUTO_INCREMENT,
  `electricityPrice` decimal(10,2) NOT NULL,
  `waterPrice` decimal(10,2) NOT NULL,
  `servicePrice` decimal(10,2) NOT NULL,
  `roomPrice` decimal(10,2) NOT NULL,
  `createdDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `modifiedDate` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `price`
--

LOCK TABLES `price` WRITE;
/*!40000 ALTER TABLE `price` DISABLE KEYS */;
INSERT INTO `price` VALUES (7,10000.00,4000.00,200000.00,200000.00,'2025-04-19 22:18:29','2025-04-19 22:18:29');
/*!40000 ALTER TABLE `price` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `requests`
--

DROP TABLE IF EXISTS `requests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `requests` (
  `id` int NOT NULL AUTO_INCREMENT,
  `studentId` int NOT NULL,
  `title` varchar(255) NOT NULL,
  `content` text NOT NULL,
  `type` varchar(10) NOT NULL,
  `status` varchar(10) NOT NULL DEFAULT 'PENDING',
  `response` text,
  `createdDate` datetime NOT NULL,
  `modifiedDate` datetime NOT NULL,
  `resolvedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `requests`
--

LOCK TABLES `requests` WRITE;
/*!40000 ALTER TABLE `requests` DISABLE KEYS */;
INSERT INTO `requests` VALUES (3,7,'Sửa lại bóng đèn','Sửa lại bóng đèn nhà vệ sinh phòng 101','REQUEST','APPROVED','ngày 15 bên dịch vụ sẽ xuống kiểm tra','2025-04-13 21:57:38','2025-04-13 22:22:00',NULL),(4,7,'phòng 102 rất ồn','ồn','COMPLAINT','REJECTED','đang sửa nên sẽ ồn','2025-04-13 22:22:50','2025-04-13 22:23:10',NULL),(5,1,'Test nha','aaaa','COMPLAINT','REJECTED','aaaaa','2025-04-19 10:54:18','2025-04-19 10:54:27',NULL);
/*!40000 ALTER TABLE `requests` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `room_student`
--

DROP TABLE IF EXISTS `room_student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `room_student` (
  `id` int NOT NULL AUTO_INCREMENT,
  `roomId` int NOT NULL,
  `studentId` int NOT NULL,
  `createdDate` datetime DEFAULT NULL,
  `modifiedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `room_student`
--

LOCK TABLES `room_student` WRITE;
/*!40000 ALTER TABLE `room_student` DISABLE KEYS */;
INSERT INTO `room_student` VALUES (17,5,7,'2025-04-13 21:56:14','2025-04-13 21:56:14'),(18,6,8,'2025-04-19 23:13:22','2025-04-19 23:13:22');
/*!40000 ALTER TABLE `room_student` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rooms`
--

DROP TABLE IF EXISTS `rooms`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rooms` (
  `id` int NOT NULL AUTO_INCREMENT,
  `floorNumber` varchar(10) NOT NULL,
  `roomNumber` varchar(10) NOT NULL,
  `maxOccupancy` int NOT NULL,
  `status` varchar(10) NOT NULL,
  `currentOccupancy` int DEFAULT '0',
  `createdDate` datetime DEFAULT NULL,
  `modifiedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rooms`
--

LOCK TABLES `rooms` WRITE;
/*!40000 ALTER TABLE `rooms` DISABLE KEYS */;
INSERT INTO `rooms` VALUES (5,'1','101',10,'OCCUPIED',1,'2025-04-13 21:56:14','2025-04-13 21:56:14'),(6,'1','102',5,'OCCUPIED',1,'2025-04-19 23:13:22','2025-04-19 23:13:22');
/*!40000 ALTER TABLE `rooms` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `security_visit`
--

DROP TABLE IF EXISTS `security_visit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `security_visit` (
  `id` int NOT NULL AUTO_INCREMENT,
  `visitorName` varchar(100) NOT NULL,
  `phoneNumber` varchar(20) NOT NULL,
  `studentId` int DEFAULT NULL,
  `entryTime` datetime NOT NULL,
  `exitTime` datetime DEFAULT NULL,
  `status` varchar(20) NOT NULL COMMENT 'CHECKED_IN or CHECKED_OUT',
  `purpose` varchar(255) DEFAULT NULL,
  `notes` text,
  `createdDate` datetime DEFAULT NULL,
  `modifiedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `security_visit`
--

LOCK TABLES `security_visit` WRITE;
/*!40000 ALTER TABLE `security_visit` DISABLE KEYS */;
INSERT INTO `security_visit` VALUES (1,'Nguyễn Văn Cảnh','0966789789',123123,'2025-04-19 17:32:15','2025-04-19 18:03:27','CHECKED_OUT','Thăm bạn','Thăm bạn A','2025-04-19 17:32:15','2025-04-19 18:03:26');
/*!40000 ALTER TABLE `security_visit` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `transactions`
--

DROP TABLE IF EXISTS `transactions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transactions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `OrderId` varchar(50) NOT NULL,
  `UserId` int NOT NULL,
  `Amount` decimal(10,2) NOT NULL,
  `PaymentMethod` varchar(50) NOT NULL,
  `TransactionId` varchar(100) DEFAULT NULL,
  `Status` varchar(20) NOT NULL,
  `OrderDescription` varchar(1000) DEFAULT NULL,
  `CreatedDate` datetime NOT NULL,
  `CompletedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transactions`
--

LOCK TABLES `transactions` WRITE;
/*!40000 ALTER TABLE `transactions` DISABLE KEYS */;
INSERT INTO `transactions` VALUES (30,'9',7,720000.00,'VNPay','14916257','SUCCESS','Thanh toán hóa đơn KTX #9','2025-04-20 11:24:29','2025-04-20 11:24:52');
/*!40000 ALTER TABLE `transactions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `role` varchar(7) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `email` varchar(100) DEFAULT NULL,
  `phone` varchar(15) DEFAULT NULL,
  `fullname` varchar(100) DEFAULT NULL,
  `createdDate` datetime DEFAULT NULL,
  `modifiedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `username` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'admin','$2a$11$VE6XbFkuI3QuhWVnQOhya.nv54zIAyaEyGe5uMHjmo2W9ysE6gwXq','ADMIN','admin@gmail.com','0977867987','Quản trị hệ thống','2025-04-07 19:56:48','2025-04-07 19:56:48'),(7,'tien123','$2a$11$PTP.DEe6rbiQg0fwkE/JM.z/YbtmzvIqA.LStZA8AybIWOqBbuuQu','STUDENT','tien123@gmail.com','0988789789','Nguyễn Văn Tiến','2025-04-13 21:56:02','2025-04-13 21:56:02'),(8,'anhnb','$2a$11$IHs6OXcZAqPKQ3egpG.yAOUnqtsZVnBzajwfZZtwRe4bdl0TgupUS','STUDENT','baoanh@gmail.com','0977867898','Nguyễn Bảo Anh','2025-04-13 22:36:50','2025-04-13 22:36:50');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'ktx'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-20 11:26:05
