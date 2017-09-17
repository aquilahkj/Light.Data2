/*
 Navicat Premium Data Transfer

 Source Server         : MysqlTest
 Source Server Type    : MySQL
 Source Server Version : 50625
 Source Host           : localhost
 Source Database       : LightData_Test

 Target Server Type    : MySQL
 Target Server Version : 50625
 File Encoding         : utf-8

 Date: 09/17/2017 14:10:12 PM
*/

SET NAMES utf8;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
--  Table structure for `Te_BaseField`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseField`;
CREATE TABLE `Te_BaseField` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BoolField` bit(1) NOT NULL,
  `BoolFieldNull` bit(1) DEFAULT NULL,
  `SbyteField` tinyint(4) NOT NULL,
  `SbyteFieldNull` tinyint(4) DEFAULT NULL,
  `ByteField` tinyint(3) unsigned NOT NULL,
  `ByteFieldNull` tinyint(3) unsigned DEFAULT NULL,
  `Int16Field` smallint(6) NOT NULL,
  `Int16FieldNull` smallint(6) DEFAULT NULL,
  `UInt16Field` smallint(5) unsigned NOT NULL,
  `UInt16FieldNull` smallint(6) unsigned DEFAULT NULL,
  `Int32Field` int(11) NOT NULL,
  `Int32FieldNull` int(11) DEFAULT NULL,
  `UInt32Field` int(10) unsigned NOT NULL,
  `UInt32FieldNull` int(10) unsigned DEFAULT NULL,
  `Int64Field` bigint(20) NOT NULL,
  `Int64FieldNull` bigint(20) DEFAULT NULL,
  `UInt64Field` bigint(20) unsigned NOT NULL,
  `UInt64FieldNull` bigint(20) unsigned DEFAULT NULL,
  `FloatField` float NOT NULL,
  `FloatFieldNull` float DEFAULT NULL,
  `DoubleField` double NOT NULL,
  `DoubleFieldNull` double DEFAULT NULL,
  `DecimalField` decimal(10,4) NOT NULL,
  `DecimalFieldNull` decimal(10,4) DEFAULT NULL,
  `DateTimeField` datetime NOT NULL,
  `DateTimeFieldNull` datetime DEFAULT NULL,
  `VarcharField` varchar(2000) NOT NULL,
  `VarcharFieldNull` varchar(2000) DEFAULT NULL,
  `TextField` text NOT NULL,
  `TextFieldNull` text,
  `BigDataField` blob NOT NULL,
  `BigDataFieldNull` blob,
  `EnumInt32Field` int(11) NOT NULL,
  `EnumInt32FieldNull` int(11) DEFAULT NULL,
  `EnumInt64Field` bigint(20) NOT NULL,
  `EnumInt64FieldNull` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- ----------------------------
--  Table structure for `Te_BaseFieldAggregateField`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseFieldAggregateField`;
CREATE TABLE `Te_BaseFieldAggregateField` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BoolField` bit(1) NOT NULL,
  `BoolFieldNull` bit(1) DEFAULT NULL,
  `SbyteField` tinyint(4) NOT NULL,
  `SbyteFieldNull` tinyint(4) DEFAULT NULL,
  `ByteField` tinyint(3) unsigned NOT NULL,
  `ByteFieldNull` tinyint(3) unsigned DEFAULT NULL,
  `Int16Field` smallint(6) NOT NULL,
  `Int16FieldNull` smallint(6) DEFAULT NULL,
  `UInt16Field` smallint(5) unsigned NOT NULL,
  `UInt16FieldNull` smallint(6) unsigned DEFAULT NULL,
  `Int32Field` int(11) NOT NULL,
  `Int32FieldNull` int(11) DEFAULT NULL,
  `UInt32Field` int(10) unsigned NOT NULL,
  `UInt32FieldNull` int(10) unsigned DEFAULT NULL,
  `Int64Field` bigint(20) NOT NULL,
  `Int64FieldNull` bigint(20) DEFAULT NULL,
  `UInt64Field` bigint(20) unsigned NOT NULL,
  `UInt64FieldNull` bigint(20) unsigned DEFAULT NULL,
  `FloatField` float NOT NULL,
  `FloatFieldNull` float DEFAULT NULL,
  `DoubleField` double NOT NULL,
  `DoubleFieldNull` double DEFAULT NULL,
  `DecimalField` decimal(10,4) NOT NULL,
  `DecimalFieldNull` decimal(10,4) DEFAULT NULL,
  `DateTimeField` datetime NOT NULL,
  `DateTimeFieldNull` datetime DEFAULT NULL,
  `VarcharField` varchar(2000) NOT NULL,
  `VarcharFieldNull` varchar(2000) DEFAULT NULL,
  `TextField` text NOT NULL,
  `TextFieldNull` text,
  `BigDataField` blob NOT NULL,
  `BigDataFieldNull` blob,
  `EnumInt32Field` int(11) NOT NULL,
  `EnumInt32FieldNull` int(11) DEFAULT NULL,
  `EnumInt64Field` bigint(20) NOT NULL,
  `EnumInt64FieldNull` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=201 DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- ----------------------------
--  Table structure for `Te_BaseFieldAggregateField_GroupBy`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseFieldAggregateField_GroupBy`;
CREATE TABLE `Te_BaseFieldAggregateField_GroupBy` (
  `KeyData` int(50) NOT NULL,
  `MonthData` int(11) NOT NULL,
  `CountData` int(11) NOT NULL,
  `CountFieldData` int(11) NOT NULL,
  `CountConditionData` int(11) NOT NULL,
  `SumData` int(11) NOT NULL,
  `AvgData` double NOT NULL,
  `MaxData` datetime NOT NULL,
  `MinData` datetime NOT NULL,
  PRIMARY KEY (`KeyData`,`MonthData`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_BaseFieldExpression`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseFieldExpression`;
CREATE TABLE `Te_BaseFieldExpression` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BoolField` bit(1) NOT NULL,
  `BoolFieldNull` bit(1) DEFAULT NULL,
  `SbyteField` tinyint(4) NOT NULL,
  `SbyteFieldNull` tinyint(4) DEFAULT NULL,
  `ByteField` tinyint(3) unsigned NOT NULL,
  `ByteFieldNull` tinyint(3) unsigned DEFAULT NULL,
  `Int16Field` smallint(6) NOT NULL,
  `Int16FieldNull` smallint(6) DEFAULT NULL,
  `UInt16Field` smallint(5) unsigned NOT NULL,
  `UInt16FieldNull` smallint(6) unsigned DEFAULT NULL,
  `Int32Field` int(11) NOT NULL,
  `Int32FieldNull` int(11) DEFAULT NULL,
  `UInt32Field` int(10) unsigned NOT NULL,
  `UInt32FieldNull` int(10) unsigned DEFAULT NULL,
  `Int64Field` bigint(20) NOT NULL,
  `Int64FieldNull` bigint(20) DEFAULT NULL,
  `UInt64Field` bigint(20) unsigned NOT NULL,
  `UInt64FieldNull` bigint(20) unsigned DEFAULT NULL,
  `FloatField` float NOT NULL,
  `FloatFieldNull` float DEFAULT NULL,
  `DoubleField` double NOT NULL,
  `DoubleFieldNull` double DEFAULT NULL,
  `DecimalField` decimal(10,4) NOT NULL,
  `DecimalFieldNull` decimal(10,4) DEFAULT NULL,
  `DateTimeField` datetime NOT NULL,
  `DateTimeFieldNull` datetime DEFAULT NULL,
  `VarcharField` varchar(2000) NOT NULL,
  `VarcharFieldNull` varchar(2000) DEFAULT NULL,
  `EnumInt32Field` int(11) NOT NULL,
  `EnumInt32FieldNull` int(11) DEFAULT NULL,
  `EnumInt64Field` bigint(20) NOT NULL,
  `EnumInt64FieldNull` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- ----------------------------
--  Table structure for `Te_BaseFieldExpression_Extend`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseFieldExpression_Extend`;
CREATE TABLE `Te_BaseFieldExpression_Extend` (
  `ExtendId` int(11) NOT NULL AUTO_INCREMENT,
  `Int32Field` int(11) NOT NULL,
  `Int32FieldNull` int(11) DEFAULT NULL,
  `DecimalField` decimal(10,4) NOT NULL,
  `DecimalFieldNull` decimal(10,4) DEFAULT NULL,
  `DateTimeField` datetime NOT NULL,
  `DateTimeFieldNull` datetime DEFAULT NULL,
  `VarcharField` varchar(2000) NOT NULL,
  `VarcharFieldNull` varchar(2000) DEFAULT NULL,
  PRIMARY KEY (`ExtendId`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_BaseFieldExtend`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseFieldExtend`;
CREATE TABLE `Te_BaseFieldExtend` (
  `Id` int(11) NOT NULL,
  `LevelName` varchar(100) NOT NULL,
  `Status` int(11) NOT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- ----------------------------
--  Table structure for `Te_BaseFieldNoIdentity`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseFieldNoIdentity`;
CREATE TABLE `Te_BaseFieldNoIdentity` (
  `Id` int(11) NOT NULL,
  `Int32Field` int(11) NOT NULL,
  `DoubleField` double NOT NULL,
  `VarcharField` varchar(500) NOT NULL,
  `DateTimeField` datetime NOT NULL,
  `EnumInt32Field` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- ----------------------------
--  Table structure for `Te_BaseFieldNoIdentity_Entity`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseFieldNoIdentity_Entity`;
CREATE TABLE `Te_BaseFieldNoIdentity_Entity` (
  `Id` int(11) NOT NULL,
  `Int32Field` int(11) NOT NULL,
  `DoubleField` double NOT NULL,
  `VarcharField` varchar(2000) NOT NULL,
  `DateTimeField` datetime NOT NULL,
  `EnumInt32Field` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- ----------------------------
--  Table structure for `Te_BaseFieldSelectField`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseFieldSelectField`;
CREATE TABLE `Te_BaseFieldSelectField` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BoolField` bit(1) NOT NULL,
  `BoolFieldNull` bit(1) DEFAULT NULL,
  `SbyteField` tinyint(4) NOT NULL,
  `SbyteFieldNull` tinyint(4) DEFAULT NULL,
  `ByteField` tinyint(3) unsigned NOT NULL,
  `ByteFieldNull` tinyint(3) unsigned DEFAULT NULL,
  `Int16Field` smallint(6) NOT NULL,
  `Int16FieldNull` smallint(6) DEFAULT NULL,
  `UInt16Field` smallint(5) unsigned NOT NULL,
  `UInt16FieldNull` smallint(6) unsigned DEFAULT NULL,
  `Int32Field` int(11) NOT NULL,
  `Int32FieldNull` int(11) DEFAULT NULL,
  `UInt32Field` int(10) unsigned NOT NULL,
  `UInt32FieldNull` int(10) unsigned DEFAULT NULL,
  `Int64Field` bigint(20) NOT NULL,
  `Int64FieldNull` bigint(20) DEFAULT NULL,
  `UInt64Field` bigint(20) unsigned NOT NULL,
  `UInt64FieldNull` bigint(20) unsigned DEFAULT NULL,
  `FloatField` float NOT NULL,
  `FloatFieldNull` float DEFAULT NULL,
  `DoubleField` double NOT NULL,
  `DoubleFieldNull` double DEFAULT NULL,
  `DecimalField` decimal(10,4) NOT NULL,
  `DecimalFieldNull` decimal(10,4) DEFAULT NULL,
  `DateTimeField` datetime NOT NULL,
  `DateTimeFieldNull` datetime DEFAULT NULL,
  `VarcharField` varchar(2000) NOT NULL,
  `VarcharFieldNull` varchar(2000) DEFAULT NULL,
  `TextField` text NOT NULL,
  `TextFieldNull` text,
  `BigDataField` blob NOT NULL,
  `BigDataFieldNull` blob,
  `EnumInt32Field` int(11) NOT NULL,
  `EnumInt32FieldNull` int(11) DEFAULT NULL,
  `EnumInt64Field` bigint(20) NOT NULL,
  `EnumInt64FieldNull` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- ----------------------------
--  Table structure for `Te_BaseField_Alias`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseField_Alias`;
CREATE TABLE `Te_BaseField_Alias` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BoolField` bit(1) NOT NULL,
  `BoolFieldNull` bit(1) DEFAULT NULL,
  `SbyteField` tinyint(4) NOT NULL,
  `SbyteFieldNull` tinyint(4) DEFAULT NULL,
  `ByteField` tinyint(3) unsigned NOT NULL,
  `ByteFieldNull` tinyint(3) unsigned DEFAULT NULL,
  `Int16Field` smallint(6) NOT NULL,
  `Int16FieldNull` smallint(6) DEFAULT NULL,
  `UInt16Field` smallint(5) unsigned NOT NULL,
  `UInt16FieldNull` smallint(6) unsigned DEFAULT NULL,
  `Int32Field` int(11) NOT NULL,
  `Int32FieldNull` int(11) DEFAULT NULL,
  `UInt32Field` int(10) unsigned NOT NULL,
  `UInt32FieldNull` int(10) unsigned DEFAULT NULL,
  `Int64Field` bigint(20) NOT NULL,
  `Int64FieldNull` bigint(20) DEFAULT NULL,
  `UInt64Field` bigint(20) unsigned NOT NULL,
  `UInt64FieldNull` bigint(20) unsigned DEFAULT NULL,
  `FloatField` float NOT NULL,
  `FloatFieldNull` float DEFAULT NULL,
  `DoubleField` double NOT NULL,
  `DoubleFieldNull` double DEFAULT NULL,
  `DecimalField` decimal(10,4) NOT NULL,
  `DecimalFieldNull` decimal(10,4) DEFAULT NULL,
  `DateTimeField` datetime NOT NULL,
  `DateTimeFieldNull` datetime DEFAULT NULL,
  `VarcharField` varchar(2000) NOT NULL,
  `VarcharFieldNull` varchar(2000) DEFAULT NULL,
  `TextField` text NOT NULL,
  `TextFieldNull` text,
  `BigDataField` blob NOT NULL,
  `BigDataFieldNull` blob,
  `EnumInt32Field` int(11) NOT NULL,
  `EnumInt32FieldNull` int(11) DEFAULT NULL,
  `EnumInt64Field` bigint(20) NOT NULL,
  `EnumInt64FieldNull` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- ----------------------------
--  Table structure for `Te_BaseField_Config`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseField_Config`;
CREATE TABLE `Te_BaseField_Config` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Int32_Field` int(11) NOT NULL,
  `Int32_FieldNull` int(11) DEFAULT NULL,
  `Decimal_Field` decimal(10,4) NOT NULL,
  `Decimal_FieldNull` decimal(10,4) DEFAULT NULL,
  `DateTime_Field` datetime NOT NULL,
  `DateTime_FieldNull` datetime DEFAULT NULL,
  `Varchar_Field` varchar(2000) NOT NULL,
  `Varchar_FieldNull` varchar(2000) DEFAULT NULL,
  `Now_Field` datetime NOT NULL,
  `Now_FieldNull` datetime(6) DEFAULT NULL,
  `Today_Field` datetime NOT NULL,
  `Today_FieldNull` datetime(6) DEFAULT NULL,
  `EnumInt32_Field` int(11) NOT NULL,
  `EnumInt32_FieldNull` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_BaseField_Config_Replace`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseField_Config_Replace`;
CREATE TABLE `Te_BaseField_Config_Replace` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Int32_Field` int(11) NOT NULL,
  `Int32_FieldNull` int(11) DEFAULT NULL,
  `Decimal_Field` decimal(10,4) NOT NULL,
  `Decimal_FieldNull` decimal(10,4) DEFAULT NULL,
  `DateTime_Field` datetime NOT NULL,
  `DateTime_FieldNull` datetime DEFAULT NULL,
  `Varchar_Field` varchar(2000) NOT NULL,
  `Varchar_FieldNull` varchar(2000) DEFAULT NULL,
  `Now_Field` datetime NOT NULL,
  `Now_FieldNull` datetime(6) DEFAULT NULL,
  `Today_Field` datetime NOT NULL,
  `Today_FieldNull` datetime(6) DEFAULT NULL,
  `EnumInt32_Field` int(11) NOT NULL,
  `EnumInt32_FieldNull` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_BaseField_DefaultValue`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseField_DefaultValue`;
CREATE TABLE `Te_BaseField_DefaultValue` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BoolFieldNull` bit(1) DEFAULT NULL,
  `SbyteFieldNull` tinyint(4) DEFAULT NULL,
  `ByteFieldNull` tinyint(3) unsigned DEFAULT NULL,
  `Int16FieldNull` smallint(6) DEFAULT NULL,
  `UInt16FieldNull` smallint(6) unsigned DEFAULT NULL,
  `Int32FieldNull` int(11) DEFAULT NULL,
  `UInt32FieldNull` int(10) unsigned DEFAULT NULL,
  `Int64FieldNull` bigint(20) DEFAULT NULL,
  `UInt64FieldNull` bigint(20) unsigned DEFAULT NULL,
  `FloatFieldNull` float DEFAULT NULL,
  `DoubleFieldNull` double DEFAULT NULL,
  `DecimalFieldNull` decimal(10,4) DEFAULT NULL,
  `DateTimeFieldNull` datetime DEFAULT NULL,
  `NowFieldNull` datetime DEFAULT NULL,
  `TodayFieldNull` datetime DEFAULT NULL,
  `VarcharFieldNull` varchar(2000) DEFAULT NULL,
  `TextFieldNull` text,
  `EnumInt32FieldNull` int(11) DEFAULT NULL,
  `EnumInt64FieldNull` bigint(20) DEFAULT NULL,
  `DateTimeField` datetime NOT NULL,
  `NowField` datetime NOT NULL,
  `TodayField` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- ----------------------------
--  Table structure for `Te_BaseField_Entity`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseField_Entity`;
CREATE TABLE `Te_BaseField_Entity` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BoolField` bit(1) NOT NULL,
  `BoolFieldNull` bit(1) DEFAULT NULL,
  `SbyteField` tinyint(4) NOT NULL,
  `SbyteFieldNull` tinyint(4) DEFAULT NULL,
  `ByteField` tinyint(3) unsigned NOT NULL,
  `ByteFieldNull` tinyint(3) unsigned DEFAULT NULL,
  `Int16Field` smallint(6) NOT NULL,
  `Int16FieldNull` smallint(6) DEFAULT NULL,
  `UInt16Field` smallint(5) unsigned NOT NULL,
  `UInt16FieldNull` smallint(6) unsigned DEFAULT NULL,
  `Int32Field` int(11) NOT NULL,
  `Int32FieldNull` int(11) DEFAULT NULL,
  `UInt32Field` int(10) unsigned NOT NULL,
  `UInt32FieldNull` int(10) unsigned DEFAULT NULL,
  `Int64Field` bigint(20) NOT NULL,
  `Int64FieldNull` bigint(20) DEFAULT NULL,
  `UInt64Field` bigint(20) unsigned NOT NULL,
  `UInt64FieldNull` bigint(20) unsigned DEFAULT NULL,
  `FloatField` float NOT NULL,
  `FloatFieldNull` float DEFAULT NULL,
  `DoubleField` double NOT NULL,
  `DoubleFieldNull` double DEFAULT NULL,
  `DecimalField` decimal(10,4) NOT NULL,
  `DecimalFieldNull` decimal(10,4) DEFAULT NULL,
  `DateTimeField` datetime NOT NULL,
  `DateTimeFieldNull` datetime DEFAULT NULL,
  `VarcharField` varchar(2000) NOT NULL,
  `VarcharFieldNull` varchar(2000) DEFAULT NULL,
  `TextField` text NOT NULL,
  `TextFieldNull` text,
  `BigDataField` blob NOT NULL,
  `BigDataFieldNull` blob,
  `EnumInt32Field` int(11) NOT NULL,
  `EnumInt32FieldNull` int(11) DEFAULT NULL,
  `EnumInt64Field` bigint(20) NOT NULL,
  `EnumInt64FieldNull` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- ----------------------------
--  Table structure for `Te_BaseField_NullMiniValue`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseField_NullMiniValue`;
CREATE TABLE `Te_BaseField_NullMiniValue` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BoolFieldNull` bit(1) DEFAULT NULL,
  `SbyteFieldNull` tinyint(4) DEFAULT NULL,
  `ByteFieldNull` tinyint(3) unsigned DEFAULT NULL,
  `Int16FieldNull` smallint(6) DEFAULT NULL,
  `UInt16FieldNull` smallint(6) unsigned DEFAULT NULL,
  `Int32FieldNull` int(11) DEFAULT NULL,
  `UInt32FieldNull` int(10) unsigned DEFAULT NULL,
  `Int64FieldNull` bigint(20) DEFAULT NULL,
  `UInt64FieldNull` bigint(20) unsigned DEFAULT NULL,
  `FloatFieldNull` float DEFAULT NULL,
  `DoubleFieldNull` double DEFAULT NULL,
  `DecimalFieldNull` decimal(10,4) DEFAULT NULL,
  `DateTimeFieldNull` datetime DEFAULT NULL,
  `VarcharFieldNull` varchar(2000) DEFAULT NULL,
  `TextFieldNull` text,
  `BigDataFieldNull` blob,
  `EnumInt32FieldNull` int(11) DEFAULT NULL,
  `EnumInt64FieldNull` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- ----------------------------
--  Table structure for `Te_BaseField_SelectInsert`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseField_SelectInsert`;
CREATE TABLE `Te_BaseField_SelectInsert` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BoolField` bit(1) NOT NULL,
  `BoolFieldNull` bit(1) DEFAULT NULL,
  `SbyteField` tinyint(4) NOT NULL,
  `SbyteFieldNull` tinyint(4) DEFAULT NULL,
  `ByteField` tinyint(3) unsigned NOT NULL,
  `ByteFieldNull` tinyint(3) unsigned DEFAULT NULL,
  `Int16Field` smallint(6) NOT NULL,
  `Int16FieldNull` smallint(6) DEFAULT NULL,
  `UInt16Field` smallint(5) unsigned NOT NULL,
  `UInt16FieldNull` smallint(6) unsigned DEFAULT NULL,
  `Int32Field` int(11) NOT NULL,
  `Int32FieldNull` int(11) DEFAULT NULL,
  `UInt32Field` int(10) unsigned NOT NULL,
  `UInt32FieldNull` int(10) unsigned DEFAULT NULL,
  `Int64Field` bigint(20) NOT NULL,
  `Int64FieldNull` bigint(20) DEFAULT NULL,
  `UInt64Field` bigint(20) unsigned NOT NULL,
  `UInt64FieldNull` bigint(20) unsigned DEFAULT NULL,
  `FloatField` float NOT NULL,
  `FloatFieldNull` float DEFAULT NULL,
  `DoubleField` double NOT NULL,
  `DoubleFieldNull` double DEFAULT NULL,
  `DecimalField` decimal(10,4) NOT NULL,
  `DecimalFieldNull` decimal(10,4) DEFAULT NULL,
  `DateTimeField` datetime NOT NULL,
  `DateTimeFieldNull` datetime DEFAULT NULL,
  `VarcharField` varchar(2000) NOT NULL,
  `VarcharFieldNull` varchar(2000) DEFAULT NULL,
  `TextField` text NOT NULL,
  `TextFieldNull` text,
  `BigDataField` blob NOT NULL,
  `BigDataFieldNull` blob,
  `EnumInt32Field` int(11) NOT NULL,
  `EnumInt32FieldNull` int(11) DEFAULT NULL,
  `EnumInt64Field` bigint(20) NOT NULL,
  `EnumInt64FieldNull` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- ----------------------------
--  Table structure for `Te_BaseField_SelectInsert_NoIdentity`
-- ----------------------------
DROP TABLE IF EXISTS `Te_BaseField_SelectInsert_NoIdentity`;
CREATE TABLE `Te_BaseField_SelectInsert_NoIdentity` (
  `Id` int(11) NOT NULL,
  `BoolField` bit(1) NOT NULL,
  `BoolFieldNull` bit(1) DEFAULT NULL,
  `SbyteField` tinyint(4) NOT NULL,
  `SbyteFieldNull` tinyint(4) DEFAULT NULL,
  `ByteField` tinyint(3) unsigned NOT NULL,
  `ByteFieldNull` tinyint(3) unsigned DEFAULT NULL,
  `Int16Field` smallint(6) NOT NULL,
  `Int16FieldNull` smallint(6) DEFAULT NULL,
  `UInt16Field` smallint(5) unsigned NOT NULL,
  `UInt16FieldNull` smallint(6) unsigned DEFAULT NULL,
  `Int32Field` int(11) NOT NULL,
  `Int32FieldNull` int(11) DEFAULT NULL,
  `UInt32Field` int(10) unsigned NOT NULL,
  `UInt32FieldNull` int(10) unsigned DEFAULT NULL,
  `Int64Field` bigint(20) NOT NULL,
  `Int64FieldNull` bigint(20) DEFAULT NULL,
  `UInt64Field` bigint(20) unsigned NOT NULL,
  `UInt64FieldNull` bigint(20) unsigned DEFAULT NULL,
  `FloatField` float NOT NULL,
  `FloatFieldNull` float DEFAULT NULL,
  `DoubleField` double NOT NULL,
  `DoubleFieldNull` double DEFAULT NULL,
  `DecimalField` decimal(10,4) NOT NULL,
  `DecimalFieldNull` decimal(10,4) DEFAULT NULL,
  `DateTimeField` datetime NOT NULL,
  `DateTimeFieldNull` datetime DEFAULT NULL,
  `VarcharField` varchar(2000) NOT NULL,
  `VarcharFieldNull` varchar(2000) DEFAULT NULL,
  `TextField` text NOT NULL,
  `TextFieldNull` text,
  `BigDataField` blob NOT NULL,
  `BigDataFieldNull` blob,
  `EnumInt32Field` int(11) NOT NULL,
  `EnumInt32FieldNull` int(11) DEFAULT NULL,
  `EnumInt64Field` bigint(20) NOT NULL,
  `EnumInt64FieldNull` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- ----------------------------
--  Table structure for `Te_JoinTable_SelectInsert`
-- ----------------------------
DROP TABLE IF EXISTS `Te_JoinTable_SelectInsert`;
CREATE TABLE `Te_JoinTable_SelectInsert` (
  `MainId` int(11) NOT NULL,
  `Int32Field` int(11) NOT NULL,
  `Int32FieldNull` int(11) DEFAULT NULL,
  `SubId` int(11) NOT NULL,
  `SubInt32Field` int(11) NOT NULL,
  `SubInt32FieldNull` int(11) DEFAULT NULL,
  PRIMARY KEY (`MainId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_MainTable`
-- ----------------------------
DROP TABLE IF EXISTS `Te_MainTable`;
CREATE TABLE `Te_MainTable` (
  `MainId` int(11) NOT NULL AUTO_INCREMENT,
  `Int32Field` int(11) NOT NULL,
  `Int32FieldNull` int(11) DEFAULT NULL,
  `DecimalField` decimal(10,4) NOT NULL,
  `DecimalFieldNull` decimal(10,4) DEFAULT NULL,
  `DateTimeField` datetime NOT NULL,
  `DateTimeFieldNull` datetime DEFAULT NULL,
  `VarcharField` varchar(2000) NOT NULL,
  `VarcharFieldNull` varchar(2000) DEFAULT NULL,
  `SubId` int(11) NOT NULL,
  PRIMARY KEY (`MainId`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_ObjectField`
-- ----------------------------
DROP TABLE IF EXISTS `Te_ObjectField`;
CREATE TABLE `Te_ObjectField` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `VarcharField` varchar(50) NOT NULL,
  `ObjectField` varchar(1000) NOT NULL,
  `ObjectFieldNull` varchar(1000) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_RelateA`
-- ----------------------------
DROP TABLE IF EXISTS `Te_RelateA`;
CREATE TABLE `Te_RelateA` (
  `Id` int(11) NOT NULL,
  `RelateBId` int(11) NOT NULL,
  `RelateCId` int(11) NOT NULL,
  `RelateDId` int(11) NOT NULL,
  `RelateEId` int(11) NOT NULL,
  `RelateFId` int(11) NOT NULL,
  `DecimalField` decimal(10,4) NOT NULL,
  `DateTimeField` datetime NOT NULL,
  `VarcharField` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_RelateB`
-- ----------------------------
DROP TABLE IF EXISTS `Te_RelateB`;
CREATE TABLE `Te_RelateB` (
  `Id` int(11) NOT NULL,
  `RelateAId` int(11) NOT NULL,
  `RelateCId` int(11) NOT NULL,
  `RelateDId` int(11) NOT NULL,
  `RelateEId` int(11) NOT NULL,
  `RelateFId` int(11) NOT NULL,
  `DecimalField` decimal(10,4) NOT NULL,
  `DateTimeField` datetime NOT NULL,
  `VarcharField` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_RelateC`
-- ----------------------------
DROP TABLE IF EXISTS `Te_RelateC`;
CREATE TABLE `Te_RelateC` (
  `Id` int(11) NOT NULL,
  `RelateAId` int(11) NOT NULL,
  `RelateBId` int(11) NOT NULL,
  `RelateDId` int(11) NOT NULL,
  `RelateEId` int(11) NOT NULL,
  `RelateFId` int(11) NOT NULL,
  `VarcharField` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_RelateCollection`
-- ----------------------------
DROP TABLE IF EXISTS `Te_RelateCollection`;
CREATE TABLE `Te_RelateCollection` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RelateAId` int(11) NOT NULL,
  `VarcharField` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=121 DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_RelateD`
-- ----------------------------
DROP TABLE IF EXISTS `Te_RelateD`;
CREATE TABLE `Te_RelateD` (
  `Id` int(11) NOT NULL,
  `RelateAId` int(11) NOT NULL,
  `RelateBId` int(11) NOT NULL,
  `RelateCId` int(11) NOT NULL,
  `RelateEId` int(11) NOT NULL,
  `RelateFId` int(11) NOT NULL,
  `VarcharField` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_RelateE`
-- ----------------------------
DROP TABLE IF EXISTS `Te_RelateE`;
CREATE TABLE `Te_RelateE` (
  `Id` int(11) NOT NULL,
  `RelateAId` int(11) NOT NULL,
  `RelateBId` int(11) NOT NULL,
  `RelateCId` int(11) NOT NULL,
  `RelateDId` int(11) NOT NULL,
  `RelateFId` int(11) NOT NULL,
  `VarcharField` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_RelateF`
-- ----------------------------
DROP TABLE IF EXISTS `Te_RelateF`;
CREATE TABLE `Te_RelateF` (
  `Id` int(11) NOT NULL,
  `RelateAId` int(11) NOT NULL,
  `RelateBId` int(11) NOT NULL,
  `RelateCId` int(11) NOT NULL,
  `RelateDId` int(11) NOT NULL,
  `RelateEId` int(11) NOT NULL,
  `VarcharField` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_RelateMain_Config`
-- ----------------------------
DROP TABLE IF EXISTS `Te_RelateMain_Config`;
CREATE TABLE `Te_RelateMain_Config` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DecimalField` decimal(10,4) NOT NULL,
  `DateTimeField` datetime NOT NULL,
  `VarcharField` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_RelateSub_Config`
-- ----------------------------
DROP TABLE IF EXISTS `Te_RelateSub_Config`;
CREATE TABLE `Te_RelateSub_Config` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MainId` int(11) NOT NULL,
  `DecimalField` decimal(10,4) NOT NULL,
  `DateTimeField` datetime NOT NULL,
  `VarcharField` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
--  Table structure for `Te_SubTable`
-- ----------------------------
DROP TABLE IF EXISTS `Te_SubTable`;
CREATE TABLE `Te_SubTable` (
  `SubId` int(11) NOT NULL AUTO_INCREMENT,
  `Int32Field` int(11) NOT NULL,
  `Int32FieldNull` int(11) DEFAULT NULL,
  `DecimalField` decimal(10,4) NOT NULL,
  `DecimalFieldNull` decimal(10,4) DEFAULT NULL,
  `DateTimeField` datetime NOT NULL,
  `DateTimeFieldNull` datetime DEFAULT NULL,
  `VarcharField` varchar(2000) NOT NULL,
  `VarcharFieldNull` varchar(2000) DEFAULT NULL,
  PRIMARY KEY (`SubId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
--  Procedure structure for `sptest1`
-- ----------------------------
DROP PROCEDURE IF EXISTS `sptest1`;
delimiter ;;
CREATE DEFINER=`root`@`192.168.210.%` PROCEDURE `sptest1`()
select * from Te_BaseField
 ;;
delimiter ;

-- ----------------------------
--  Procedure structure for `sptest2`
-- ----------------------------
DROP PROCEDURE IF EXISTS `sptest2`;
delimiter ;;
CREATE DEFINER=`root`@`192.168.210.%` PROCEDURE `sptest2`(IN P1 int, IN P2 int)
    DETERMINISTIC
select * from Te_BaseField where Id>P1 and Id<=P2
 ;;
delimiter ;

-- ----------------------------
--  Procedure structure for `sptest3`
-- ----------------------------
DROP PROCEDURE IF EXISTS `sptest3`;
delimiter ;;
CREATE DEFINER=`root`@`192.168.210.%` PROCEDURE `sptest3`()
update Te_BaseField set VarcharField='abc' where Id=1
 ;;
delimiter ;

-- ----------------------------
--  Procedure structure for `sptest4`
-- ----------------------------
DROP PROCEDURE IF EXISTS `sptest4`;
delimiter ;;
CREATE DEFINER=`root`@`192.168.210.%` PROCEDURE `sptest4`(IN P1 int, IN P2 varchar(200))
    DETERMINISTIC
update Te_BaseField set VarcharField=P2 where Id=P1
 ;;
delimiter ;

-- ----------------------------
--  Procedure structure for `sptest5`
-- ----------------------------
DROP PROCEDURE IF EXISTS `sptest5`;
delimiter ;;
CREATE DEFINER=`root`@`192.168.210.%` PROCEDURE `sptest5`()
select count(1) from Te_BaseField
 ;;
delimiter ;

-- ----------------------------
--  Procedure structure for `sptest6`
-- ----------------------------
DROP PROCEDURE IF EXISTS `sptest6`;
delimiter ;;
CREATE DEFINER=`root`@`192.168.210.%` PROCEDURE `sptest6`(IN P1 integer)
select count(1) from Te_BaseField where Id<=P1
 ;;
delimiter ;

-- ----------------------------
--  Procedure structure for `sptest7`
-- ----------------------------
DROP PROCEDURE IF EXISTS `sptest7`;
delimiter ;;
CREATE DEFINER=`root`@`192.168.210.%` PROCEDURE `sptest7`(IN P1 integer, OUT P2 int)
select count(1) into P2 from Te_BaseField where Id<=P1
 ;;
delimiter ;

SET FOREIGN_KEY_CHECKS = 1;
