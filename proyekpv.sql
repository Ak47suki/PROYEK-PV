/*
SQLyog Community v13.3.0 (64 bit)
MySQL - 10.4.32-MariaDB : Database - proyekpv
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`proyekpv` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;

USE `proyekpv`;

/*Table structure for table `barang` */

DROP TABLE IF EXISTS `barang`;

CREATE TABLE `barang` (
  `barang_id` varchar(8) NOT NULL,
  `nama_barang` varchar(255) NOT NULL,
  `jumlah_barang` int(255) NOT NULL,
  `harga_barang` int(255) NOT NULL,
  `kategori_barang` varchar(255) NOT NULL,
  PRIMARY KEY (`barang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `barang` */

insert  into `barang`(`barang_id`,`nama_barang`,`jumlah_barang`,`harga_barang`,`kategori_barang`) values 
('BRG001','RTX 3060ti MSI',20,3600000,'VGA'),
('BRG002','Z390 MSI Tomahawk',66,3000000,'Motherboard'),
('BRG004','RTX 4070 ROG',17,14000000,'VGA'),
('BRG005','RTX 4090ti ZOTAC ',7,20000000,'VGA'),
('BRG006','INTEL i3 10100f',31,1100000,'Processor'),
('BRG007','INTEL i5 11400f',33,2100000,'Processor'),
('BRG008','INTEL I7 12700',11,4000000,'Processor'),
('BRG009','AMD Ryzen 5 5600G',20,2100000,'Processor'),
('BRG010','MSI A650GF',22,1750000,'PSU'),
('BRG011','CORSAIR 1200HX',7,4000000,'PSU'),
('BRG012','MSI MAG100R',11,700000,'Casing'),
('BRG013','H110m MSI A pro',88,1100000,'Motherboard'),
('BRG014','GSKILL TRIDENT 8GB x 2 2666mhz',65,1100000,'RAM'),
('BRG015','GSKILL ROYALE 32GB x 2 4000mhz',12,3100000,'RAM'),
('BRG016','SSD SAMSUNG 1 TB 970 EVO',65,2400000,'SSD'),
('BRG017','SSD XPG spectrix 512GB',99,890000,'SSD'),
('BRG018','WD BLUE 1 TB 7200rpm',200,780000,'HDD'),
('BRG019','WD BLUE 4 TB 5400rpm',100,1800000,'HDD'),
('BRG020','Seagate 1 TB 7200rpm',170,790000,'HDD'),
('BRG021','CUBEGAMING 300i',110,550000,'Casing'),
('BRG022','A320 a pro msi',88,980000,'Motherboard'),
('BRG023','Asrock X570 Steel Legend',21,3200000,'Motherboard'),
('BRG024','Amd Ryzen 3 3600g',77,1100000,'Processor'),
('BRG025','intel I3 12100f',88,1300000,'Processor'),
('BRG026','AMD Ryzen 7 7800G',77,4200000,'Processor');

/*Table structure for table `htrans` */

DROP TABLE IF EXISTS `htrans`;

CREATE TABLE `htrans` (
  `htrans_id` varchar(8) NOT NULL,
  `pembeli_id` varchar(8) DEFAULT NULL,
  `alamat_pembeli` varchar(255) DEFAULT NULL,
  `invoice` varchar(255) DEFAULT NULL,
  `tanggal` datetime DEFAULT NULL,
  `barang_id` varchar(8) DEFAULT NULL,
  `jumlah` bigint(255) DEFAULT NULL,
  `grand_total` bigint(255) DEFAULT NULL,
  `status` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`htrans_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `htrans` */

/*Table structure for table `user` */

DROP TABLE IF EXISTS `user`;

CREATE TABLE `user` (
  `user_id` varchar(100) NOT NULL,
  `username` varchar(100) NOT NULL,
  `password` varchar(100) NOT NULL,
  `authority` varchar(100) NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `user` */

insert  into `user`(`user_id`,`username`,`password`,`authority`) values 
('USR001','keset','12345','owner');

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
