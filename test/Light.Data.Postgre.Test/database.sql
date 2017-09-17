/*
 Navicat Premium Data Transfer

 Source Server         : PostgreTest
 Source Server Type    : PostgreSQL
 Source Server Version : 90405
 Source Host           : 127.0.0.1
 Source Database       : LightData_Test
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 90405
 File Encoding         : utf-8

 Date: 09/17/2017 13:42:14 PM
*/

-- ----------------------------
--  Sequence structure for Te_BaseFieldAggregateField_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_BaseFieldAggregateField_Id_seq";
CREATE SEQUENCE "public"."Te_BaseFieldAggregateField_Id_seq" INCREMENT 1 START 45 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_BaseFieldAggregateField_Id_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_BaseFieldExpression_Extend_ExtendId_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_BaseFieldExpression_Extend_ExtendId_seq";
CREATE SEQUENCE "public"."Te_BaseFieldExpression_Extend_ExtendId_seq" INCREMENT 1 START 10 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_BaseFieldExpression_Extend_ExtendId_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_BaseFieldExpression_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_BaseFieldExpression_Id_seq";
CREATE SEQUENCE "public"."Te_BaseFieldExpression_Id_seq" INCREMENT 1 START 45 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_BaseFieldExpression_Id_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_BaseFieldSelectField_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_BaseFieldSelectField_Id_seq";
CREATE SEQUENCE "public"."Te_BaseFieldSelectField_Id_seq" INCREMENT 1 START 45 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_BaseFieldSelectField_Id_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_BaseField_Alias_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_BaseField_Alias_Id_seq";
CREATE SEQUENCE "public"."Te_BaseField_Alias_Id_seq" INCREMENT 1 START 1 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_BaseField_Alias_Id_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_BaseField_Config_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_BaseField_Config_Id_seq";
CREATE SEQUENCE "public"."Te_BaseField_Config_Id_seq" INCREMENT 1 START 1 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_BaseField_Config_Id_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_BaseField_Config_Replace_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_BaseField_Config_Replace_Id_seq";
CREATE SEQUENCE "public"."Te_BaseField_Config_Replace_Id_seq" INCREMENT 1 START 1 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_BaseField_Config_Replace_Id_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_BaseField_DefaultValue_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_BaseField_DefaultValue_Id_seq";
CREATE SEQUENCE "public"."Te_BaseField_DefaultValue_Id_seq" INCREMENT 1 START 1 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_BaseField_DefaultValue_Id_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_BaseField_Entity_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_BaseField_Entity_Id_seq";
CREATE SEQUENCE "public"."Te_BaseField_Entity_Id_seq" INCREMENT 1 START 33 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_BaseField_Entity_Id_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_BaseField_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_BaseField_Id_seq";
CREATE SEQUENCE "public"."Te_BaseField_Id_seq" INCREMENT 1 START 45 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_BaseField_Id_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_BaseField_NullMiniValue_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_BaseField_NullMiniValue_Id_seq";
CREATE SEQUENCE "public"."Te_BaseField_NullMiniValue_Id_seq" INCREMENT 1 START 10 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_BaseField_NullMiniValue_Id_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_BaseField_SelectInsert_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_BaseField_SelectInsert_Id_seq";
CREATE SEQUENCE "public"."Te_BaseField_SelectInsert_Id_seq" INCREMENT 1 START 28 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_BaseField_SelectInsert_Id_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_MainTable_MainId_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_MainTable_MainId_seq";
CREATE SEQUENCE "public"."Te_MainTable_MainId_seq" INCREMENT 1 START 10 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_MainTable_MainId_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_ObjectField_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_ObjectField_Id_seq";
CREATE SEQUENCE "public"."Te_ObjectField_Id_seq" INCREMENT 1 START 45 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_ObjectField_Id_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_RelateCollection_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_RelateCollection_Id_seq";
CREATE SEQUENCE "public"."Te_RelateCollection_Id_seq" INCREMENT 1 START 120 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_RelateCollection_Id_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_RelateMain_Config_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_RelateMain_Config_Id_seq";
CREATE SEQUENCE "public"."Te_RelateMain_Config_Id_seq" INCREMENT 1 START 10 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_RelateMain_Config_Id_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_RelateSub_Config_Id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_RelateSub_Config_Id_seq";
CREATE SEQUENCE "public"."Te_RelateSub_Config_Id_seq" INCREMENT 1 START 5 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_RelateSub_Config_Id_seq" OWNER TO "root";

-- ----------------------------
--  Sequence structure for Te_SubTable_SubId_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."Te_SubTable_SubId_seq";
CREATE SEQUENCE "public"."Te_SubTable_SubId_seq" INCREMENT 1 START 10 MAXVALUE 9223372036854775807 MINVALUE 1 CACHE 1;
ALTER TABLE "public"."Te_SubTable_SubId_seq" OWNER TO "root";

-- ----------------------------
--  Function structure for public.sptest1()
-- ----------------------------
DROP FUNCTION IF EXISTS "public"."sptest1"();
CREATE FUNCTION "public"."sptest1"() RETURNS SETOF "public"."Te_BaseField" 
	AS $BODY$
select * from "Te_BaseField";
$BODY$
	LANGUAGE sql
	COST 100
	ROWS 1000
	CALLED ON NULL INPUT
	SECURITY INVOKER
	VOLATILE;
ALTER FUNCTION "public"."sptest1"() OWNER TO "root";

-- ----------------------------
--  Function structure for public.sptest2(int4, int4)
-- ----------------------------
DROP FUNCTION IF EXISTS "public"."sptest2"(int4, int4);
CREATE FUNCTION "public"."sptest2"(IN p1 int4, IN p2 int4) RETURNS SETOF "public"."Te_BaseField" 
	AS $BODY$
select * from "Te_BaseField" where "Id">P1 and "Id"<=P2
$BODY$
	LANGUAGE sql
	COST 100
	ROWS 1000
	CALLED ON NULL INPUT
	SECURITY INVOKER
	VOLATILE;
ALTER FUNCTION "public"."sptest2"(IN p1 int4, IN p2 int4) OWNER TO "root";

-- ----------------------------
--  Function structure for public.sptest3()
-- ----------------------------
DROP FUNCTION IF EXISTS "public"."sptest3"();
CREATE FUNCTION "public"."sptest3"() RETURNS "void" 
	AS $BODY$
update "Te_BaseField" set "VarcharField"='abc' where "Id"=1
$BODY$
	LANGUAGE sql
	COST 100
	CALLED ON NULL INPUT
	SECURITY INVOKER
	VOLATILE;
ALTER FUNCTION "public"."sptest3"() OWNER TO "root";

-- ----------------------------
--  Function structure for public.sptest4(int4, varchar)
-- ----------------------------
DROP FUNCTION IF EXISTS "public"."sptest4"(int4, varchar);
CREATE FUNCTION "public"."sptest4"(IN p1 int4, IN p2 varchar) RETURNS "void" 
	AS $BODY$
update "Te_BaseField" set "VarcharField"=P2 where "Id"=P1
$BODY$
	LANGUAGE sql
	COST 100
	CALLED ON NULL INPUT
	SECURITY INVOKER
	VOLATILE;
ALTER FUNCTION "public"."sptest4"(IN p1 int4, IN p2 varchar) OWNER TO "root";

-- ----------------------------
--  Function structure for public.sptest5()
-- ----------------------------
DROP FUNCTION IF EXISTS "public"."sptest5"();
CREATE FUNCTION "public"."sptest5"() RETURNS "int4" 
	AS $BODY$
SELECT count(1)::INTEGER from "Te_BaseField" 
$BODY$
	LANGUAGE sql
	COST 100
	CALLED ON NULL INPUT
	SECURITY INVOKER
	VOLATILE;
ALTER FUNCTION "public"."sptest5"() OWNER TO "root";

-- ----------------------------
--  Function structure for public.sptest6(int4)
-- ----------------------------
DROP FUNCTION IF EXISTS "public"."sptest6"(int4);
CREATE FUNCTION "public"."sptest6"(IN p1 int4) RETURNS "int4" 
	AS $BODY$
SELECT count(1)::INTEGER from "Te_BaseField" where "Id"<=P1
$BODY$
	LANGUAGE sql
	COST 100
	CALLED ON NULL INPUT
	SECURITY INVOKER
	VOLATILE;
ALTER FUNCTION "public"."sptest6"(IN p1 int4) OWNER TO "root";

-- ----------------------------
--  Function structure for public.sptest7(int4)
-- ----------------------------
DROP FUNCTION IF EXISTS "public"."sptest7"(int4);
CREATE FUNCTION "public"."sptest7"(IN p1 int4, OUT p2 int4) RETURNS "int4" 
	AS $BODY$
begin
SELECT count(1) into P2 from "Te_BaseField" where "Id"<=P1;
end
$BODY$
	LANGUAGE plpgsql
	COST 100
	CALLED ON NULL INPUT
	SECURITY INVOKER
	VOLATILE;
ALTER FUNCTION "public"."sptest7"(IN p1 int4, OUT p2 int4) OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseField_SelectInsert
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseField_SelectInsert";
CREATE TABLE "public"."Te_BaseField_SelectInsert" (
	"Id" int4 NOT NULL DEFAULT nextval('"Te_BaseField_SelectInsert_Id_seq"'::regclass),
	"BoolField" bool NOT NULL,
	"BoolFieldNull" bool,
	"SbyteField" int2 NOT NULL,
	"SbyteFieldNull" int2,
	"ByteField" int2 NOT NULL,
	"ByteFieldNull" int2,
	"Int16Field" int2 NOT NULL,
	"Int16FieldNull" int2,
	"UInt16Field" int4 NOT NULL,
	"UInt16FieldNull" int4,
	"Int32Field" int4 NOT NULL,
	"Int32FieldNull" int4,
	"UInt32Field" int8 NOT NULL,
	"UInt32FieldNull" int8,
	"Int64Field" int8 NOT NULL,
	"Int64FieldNull" int8,
	"UInt64Field" numeric(20,0) NOT NULL,
	"UInt64FieldNull" numeric(20,0),
	"FloatField" float4 NOT NULL,
	"FloatFieldNull" float4,
	"DoubleField" float8 NOT NULL,
	"DoubleFieldNull" float8,
	"DecimalField" numeric(10,4) NOT NULL,
	"DecimalFieldNull" numeric(10,4),
	"DateTimeField" timestamp(6) NOT NULL,
	"DateTimeFieldNull" timestamp(6) NULL,
	"VarcharField" varchar(2000) NOT NULL COLLATE "default",
	"VarcharFieldNull" varchar(2000) COLLATE "default",
	"TextField" text NOT NULL COLLATE "default",
	"TextFieldNull" text COLLATE "default",
	"BigDataField" bytea NOT NULL,
	"BigDataFieldNull" bytea,
	"EnumInt32Field" int4 NOT NULL,
	"EnumInt32FieldNull" int4,
	"EnumInt64Field" int8 NOT NULL,
	"EnumInt64FieldNull" int8
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseField_SelectInsert" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseFieldExtend
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseFieldExtend";
CREATE TABLE "public"."Te_BaseFieldExtend" (
	"Id" int4 NOT NULL,
	"LevelName" varchar(100) NOT NULL COLLATE "default",
	"Status" int4 NOT NULL,
	"Remark" varchar(2000) COLLATE "default"
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseFieldExtend" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_JoinTable_SelectInsert
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_JoinTable_SelectInsert";
CREATE TABLE "public"."Te_JoinTable_SelectInsert" (
	"MainId" int4 NOT NULL,
	"Int32Field" int4 NOT NULL,
	"Int32FieldNull" int4,
	"SubId" int4 NOT NULL,
	"SubInt32Field" int4 NOT NULL,
	"SubInt32FieldNull" int4
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_JoinTable_SelectInsert" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_RelateF
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_RelateF";
CREATE TABLE "public"."Te_RelateF" (
	"Id" int4 NOT NULL,
	"RelateAId" int4 NOT NULL,
	"RelateBId" int4 NOT NULL,
	"RelateCId" int4 NOT NULL,
	"RelateDId" int4 NOT NULL,
	"RelateEId" int4 NOT NULL,
	"VarcharField" varchar(255) NOT NULL COLLATE "default"
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_RelateF" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseFieldNoIdentity
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseFieldNoIdentity";
CREATE TABLE "public"."Te_BaseFieldNoIdentity" (
	"Id" int4 NOT NULL,
	"Int32Field" int4 NOT NULL,
	"DoubleField" float8 NOT NULL,
	"VarcharField" varchar(500) NOT NULL COLLATE "default",
	"DateTimeField" timestamp(6) NOT NULL,
	"EnumInt32Field" int4 NOT NULL
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseFieldNoIdentity" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_RelateC
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_RelateC";
CREATE TABLE "public"."Te_RelateC" (
	"Id" int4 NOT NULL,
	"RelateAId" int4 NOT NULL,
	"RelateBId" int4 NOT NULL,
	"RelateDId" int4 NOT NULL,
	"RelateEId" int4 NOT NULL,
	"RelateFId" int4 NOT NULL,
	"VarcharField" varchar(255) NOT NULL COLLATE "default"
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_RelateC" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_RelateD
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_RelateD";
CREATE TABLE "public"."Te_RelateD" (
	"Id" int4 NOT NULL,
	"RelateAId" int4 NOT NULL,
	"RelateBId" int4 NOT NULL,
	"RelateCId" int4 NOT NULL,
	"RelateEId" int4 NOT NULL,
	"RelateFId" int4 NOT NULL,
	"VarcharField" varchar(255) NOT NULL COLLATE "default"
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_RelateD" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_RelateB
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_RelateB";
CREATE TABLE "public"."Te_RelateB" (
	"Id" int4 NOT NULL,
	"RelateAId" int4 NOT NULL,
	"RelateCId" int4 NOT NULL,
	"RelateDId" int4 NOT NULL,
	"RelateEId" int4 NOT NULL,
	"RelateFId" int4 NOT NULL,
	"DecimalField" numeric(10,4) NOT NULL,
	"DateTimeField" timestamp(6) NOT NULL,
	"VarcharField" varchar(255) NOT NULL COLLATE "default"
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_RelateB" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_RelateCollection
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_RelateCollection";
CREATE TABLE "public"."Te_RelateCollection" (
	"Id" int4 NOT NULL DEFAULT nextval('"Te_RelateCollection_Id_seq"'::regclass),
	"RelateAId" int4 NOT NULL,
	"VarcharField" varchar(255) NOT NULL COLLATE "default"
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_RelateCollection" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseFieldExpression
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseFieldExpression";
CREATE TABLE "public"."Te_BaseFieldExpression" (
	"Id" int4 NOT NULL DEFAULT nextval('"Te_BaseFieldExpression_Id_seq"'::regclass),
	"BoolField" bool NOT NULL,
	"BoolFieldNull" bool,
	"SbyteField" int2 NOT NULL,
	"SbyteFieldNull" int2,
	"ByteField" int2 NOT NULL,
	"ByteFieldNull" int2,
	"Int16Field" int2 NOT NULL,
	"Int16FieldNull" int2,
	"UInt16Field" int4 NOT NULL,
	"UInt16FieldNull" int4,
	"Int32Field" int4 NOT NULL,
	"Int32FieldNull" int4,
	"UInt32Field" int8 NOT NULL,
	"UInt32FieldNull" int8,
	"Int64Field" int8 NOT NULL,
	"Int64FieldNull" int8,
	"UInt64Field" numeric(20,0) NOT NULL,
	"UInt64FieldNull" numeric(20,0),
	"FloatField" float4 NOT NULL,
	"FloatFieldNull" float4,
	"DoubleField" float8 NOT NULL,
	"DoubleFieldNull" float8,
	"DecimalField" numeric(10,4) NOT NULL,
	"DecimalFieldNull" numeric(10,4),
	"DateTimeField" timestamp(6) NOT NULL,
	"DateTimeFieldNull" timestamp(6) NULL,
	"VarcharField" varchar(2000) NOT NULL COLLATE "default",
	"VarcharFieldNull" varchar(2000) COLLATE "default",
	"EnumInt32Field" int4 NOT NULL,
	"EnumInt32FieldNull" int4,
	"EnumInt64Field" int8 NOT NULL,
	"EnumInt64FieldNull" int8
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseFieldExpression" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_RelateMain_Config
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_RelateMain_Config";
CREATE TABLE "public"."Te_RelateMain_Config" (
	"Id" int4 NOT NULL DEFAULT nextval('"Te_RelateMain_Config_Id_seq"'::regclass),
	"DecimalField" numeric(10,4) NOT NULL,
	"DateTimeField" timestamp(6) NOT NULL,
	"VarcharField" varchar(255) NOT NULL COLLATE "default"
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_RelateMain_Config" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseField_DefaultValue
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseField_DefaultValue";
CREATE TABLE "public"."Te_BaseField_DefaultValue" (
	"Id" int4 NOT NULL DEFAULT nextval('"Te_BaseField_DefaultValue_Id_seq"'::regclass),
	"BoolFieldNull" bool,
	"SbyteFieldNull" int2,
	"ByteFieldNull" int2,
	"Int16FieldNull" int2,
	"UInt16FieldNull" int4,
	"Int32FieldNull" int4,
	"UInt32FieldNull" int8,
	"Int64FieldNull" int8,
	"UInt64FieldNull" numeric(20,0),
	"FloatFieldNull" float4,
	"DoubleFieldNull" float8,
	"DecimalFieldNull" numeric(10,4),
	"DateTimeFieldNull" timestamp(6) NULL,
	"NowFieldNull" timestamp(6) NULL,
	"TodayFieldNull" timestamp(6) NULL,
	"VarcharFieldNull" varchar(2000) COLLATE "default",
	"TextFieldNull" text COLLATE "default",
	"EnumInt32FieldNull" int4,
	"EnumInt64FieldNull" int8,
	"DateTimeField" timestamp(6) NOT NULL,
	"NowField" timestamp(6) NOT NULL,
	"TodayField" timestamp(6) NOT NULL
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseField_DefaultValue" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_ObjectField
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_ObjectField";
CREATE TABLE "public"."Te_ObjectField" (
	"Id" int4 NOT NULL DEFAULT nextval('"Te_ObjectField_Id_seq"'::regclass),
	"VarcharField" varchar(50) NOT NULL COLLATE "default",
	"ObjectField" varchar(1000) NOT NULL COLLATE "default",
	"ObjectFieldNull" varchar(1000) COLLATE "default"
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_ObjectField" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseFieldExpression_Extend
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseFieldExpression_Extend";
CREATE TABLE "public"."Te_BaseFieldExpression_Extend" (
	"ExtendId" int4 NOT NULL DEFAULT nextval('"Te_BaseFieldExpression_Extend_ExtendId_seq"'::regclass),
	"Int32Field" int4 NOT NULL,
	"Int32FieldNull" int4,
	"DecimalField" numeric(10,4) NOT NULL,
	"DecimalFieldNull" numeric(10,4),
	"DateTimeField" timestamp(6) NOT NULL,
	"DateTimeFieldNull" timestamp(6) NULL,
	"VarcharField" varchar(2000) NOT NULL COLLATE "default",
	"VarcharFieldNull" varchar(2000) COLLATE "default"
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseFieldExpression_Extend" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseFieldSelectField
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseFieldSelectField";
CREATE TABLE "public"."Te_BaseFieldSelectField" (
	"Id" int4 NOT NULL DEFAULT nextval('"Te_BaseFieldSelectField_Id_seq"'::regclass),
	"BoolField" bool NOT NULL,
	"BoolFieldNull" bool,
	"SbyteField" int2 NOT NULL,
	"SbyteFieldNull" int2,
	"ByteField" int2 NOT NULL,
	"ByteFieldNull" int2,
	"Int16Field" int2 NOT NULL,
	"Int16FieldNull" int2,
	"UInt16Field" int4 NOT NULL,
	"UInt16FieldNull" int4,
	"Int32Field" int4 NOT NULL,
	"Int32FieldNull" int4,
	"UInt32Field" int8 NOT NULL,
	"UInt32FieldNull" int8,
	"Int64Field" int8 NOT NULL,
	"Int64FieldNull" int8,
	"UInt64Field" numeric(20,0) NOT NULL,
	"UInt64FieldNull" numeric(20,0),
	"FloatField" float4 NOT NULL,
	"FloatFieldNull" float4,
	"DoubleField" float8 NOT NULL,
	"DoubleFieldNull" float8,
	"DecimalField" numeric(10,4) NOT NULL,
	"DecimalFieldNull" numeric(10,4),
	"DateTimeField" timestamp(6) NOT NULL,
	"DateTimeFieldNull" timestamp(6) NULL,
	"VarcharField" varchar(2000) NOT NULL COLLATE "default",
	"VarcharFieldNull" varchar(2000) COLLATE "default",
	"TextField" text NOT NULL COLLATE "default",
	"TextFieldNull" text COLLATE "default",
	"BigDataField" bytea NOT NULL,
	"BigDataFieldNull" bytea,
	"EnumInt32Field" int4 NOT NULL,
	"EnumInt32FieldNull" int4,
	"EnumInt64Field" int8 NOT NULL,
	"EnumInt64FieldNull" int8
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseFieldSelectField" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseFieldAggregateField_GroupBy
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseFieldAggregateField_GroupBy";
CREATE TABLE "public"."Te_BaseFieldAggregateField_GroupBy" (
	"KeyData" int4 NOT NULL,
	"MonthData" int4 NOT NULL,
	"CountData" int4 NOT NULL,
	"CountFieldData" int4 NOT NULL,
	"CountConditionData" int4 NOT NULL,
	"SumData" int4 NOT NULL,
	"AvgData" float8 NOT NULL,
	"MaxData" timestamp(6) NOT NULL,
	"MinData" timestamp(6) NOT NULL
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseFieldAggregateField_GroupBy" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseFieldAggregateField
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseFieldAggregateField";
CREATE TABLE "public"."Te_BaseFieldAggregateField" (
	"Id" int4 NOT NULL DEFAULT nextval('"Te_BaseFieldAggregateField_Id_seq"'::regclass),
	"BoolField" bool NOT NULL,
	"BoolFieldNull" bool,
	"SbyteField" int2 NOT NULL,
	"SbyteFieldNull" int2,
	"ByteField" int2 NOT NULL,
	"ByteFieldNull" int2,
	"Int16Field" int2 NOT NULL,
	"Int16FieldNull" int2,
	"UInt16Field" int4 NOT NULL,
	"UInt16FieldNull" int4,
	"Int32Field" int4 NOT NULL,
	"Int32FieldNull" int4,
	"UInt32Field" int8 NOT NULL,
	"UInt32FieldNull" int8,
	"Int64Field" int8 NOT NULL,
	"Int64FieldNull" int8,
	"UInt64Field" numeric(20,0) NOT NULL,
	"UInt64FieldNull" numeric(20,0),
	"FloatField" float4 NOT NULL,
	"FloatFieldNull" float4,
	"DoubleField" float8 NOT NULL,
	"DoubleFieldNull" float8,
	"DecimalField" numeric(10,4) NOT NULL,
	"DecimalFieldNull" numeric(10,4),
	"DateTimeField" timestamp(6) NOT NULL,
	"DateTimeFieldNull" timestamp(6) NULL,
	"VarcharField" varchar(2000) NOT NULL COLLATE "default",
	"VarcharFieldNull" varchar(2000) COLLATE "default",
	"TextField" text NOT NULL COLLATE "default",
	"TextFieldNull" text COLLATE "default",
	"BigDataField" bytea NOT NULL,
	"BigDataFieldNull" bytea,
	"EnumInt32Field" int4 NOT NULL,
	"EnumInt32FieldNull" int4,
	"EnumInt64Field" int8 NOT NULL,
	"EnumInt64FieldNull" int8
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseFieldAggregateField" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseField
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseField";
CREATE TABLE "public"."Te_BaseField" (
	"Id" int4 NOT NULL DEFAULT nextval('"Te_BaseField_Id_seq"'::regclass),
	"BoolField" bool NOT NULL,
	"BoolFieldNull" bool,
	"SbyteField" int2 NOT NULL,
	"SbyteFieldNull" int2,
	"ByteField" int2 NOT NULL,
	"ByteFieldNull" int2,
	"Int16Field" int2 NOT NULL,
	"Int16FieldNull" int2,
	"UInt16Field" int4 NOT NULL,
	"UInt16FieldNull" int4,
	"Int32Field" int4 NOT NULL,
	"Int32FieldNull" int4,
	"UInt32Field" int8 NOT NULL,
	"UInt32FieldNull" int8,
	"Int64Field" int8 NOT NULL,
	"Int64FieldNull" int8,
	"UInt64Field" numeric(20,0) NOT NULL,
	"UInt64FieldNull" numeric(20,0),
	"FloatField" float4 NOT NULL,
	"FloatFieldNull" float4,
	"DoubleField" float8 NOT NULL,
	"DoubleFieldNull" float8,
	"DecimalField" numeric(10,4) NOT NULL,
	"DecimalFieldNull" numeric(10,4),
	"DateTimeField" timestamp(6) NOT NULL,
	"DateTimeFieldNull" timestamp(6) NULL,
	"VarcharField" varchar(2000) NOT NULL COLLATE "default",
	"VarcharFieldNull" varchar(2000) COLLATE "default",
	"TextField" text NOT NULL COLLATE "default",
	"TextFieldNull" text COLLATE "default",
	"BigDataField" bytea NOT NULL,
	"BigDataFieldNull" bytea,
	"EnumInt32Field" int4 NOT NULL,
	"EnumInt32FieldNull" int4,
	"EnumInt64Field" int8 NOT NULL,
	"EnumInt64FieldNull" int8
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseField" OWNER TO "root";

COMMENT ON TABLE "public"."Te_BaseField" IS '基础测试表';

-- ----------------------------
--  Table structure for Te_BaseField_NullMiniValue
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseField_NullMiniValue";
CREATE TABLE "public"."Te_BaseField_NullMiniValue" (
	"Id" int4 NOT NULL DEFAULT nextval('"Te_BaseField_NullMiniValue_Id_seq"'::regclass),
	"BoolFieldNull" bool,
	"SbyteFieldNull" int2,
	"ByteFieldNull" int2,
	"Int16FieldNull" int2,
	"UInt16FieldNull" int4,
	"Int32FieldNull" int4,
	"UInt32FieldNull" int8,
	"Int64FieldNull" int8,
	"UInt64FieldNull" numeric(20,0),
	"FloatFieldNull" float4,
	"DoubleFieldNull" float8,
	"DecimalFieldNull" numeric(10,4),
	"DateTimeFieldNull" timestamp(6) NULL,
	"VarcharFieldNull" varchar(2000) COLLATE "default",
	"TextFieldNull" text COLLATE "default",
	"BigDataFieldNull" bytea,
	"EnumInt32FieldNull" int4,
	"EnumInt64FieldNull" int8
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseField_NullMiniValue" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_RelateE
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_RelateE";
CREATE TABLE "public"."Te_RelateE" (
	"Id" int4 NOT NULL,
	"RelateAId" int4 NOT NULL,
	"RelateBId" int4 NOT NULL,
	"RelateCId" int4 NOT NULL,
	"RelateDId" int4 NOT NULL,
	"RelateFId" int4 NOT NULL,
	"VarcharField" varchar(255) NOT NULL COLLATE "default"
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_RelateE" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_RelateA
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_RelateA";
CREATE TABLE "public"."Te_RelateA" (
	"Id" int4 NOT NULL,
	"RelateBId" int4 NOT NULL,
	"RelateCId" int4 NOT NULL,
	"RelateDId" int4 NOT NULL,
	"RelateEId" int4 NOT NULL,
	"RelateFId" int4 NOT NULL,
	"DecimalField" numeric(10,4) NOT NULL,
	"DateTimeField" timestamp(6) NOT NULL,
	"VarcharField" varchar(255) NOT NULL COLLATE "default"
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_RelateA" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseField_SelectInsert_NoIdentity
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseField_SelectInsert_NoIdentity";
CREATE TABLE "public"."Te_BaseField_SelectInsert_NoIdentity" (
	"Id" int4 NOT NULL,
	"BoolField" bool NOT NULL,
	"BoolFieldNull" bool,
	"SbyteField" int2 NOT NULL,
	"SbyteFieldNull" int2,
	"ByteField" int2 NOT NULL,
	"ByteFieldNull" int2,
	"Int16Field" int2 NOT NULL,
	"Int16FieldNull" int2,
	"UInt16Field" int4 NOT NULL,
	"UInt16FieldNull" int4,
	"Int32Field" int4 NOT NULL,
	"Int32FieldNull" int4,
	"UInt32Field" int8 NOT NULL,
	"UInt32FieldNull" int8,
	"Int64Field" int8 NOT NULL,
	"Int64FieldNull" int8,
	"UInt64Field" numeric(20,0) NOT NULL,
	"UInt64FieldNull" numeric(20,0),
	"FloatField" float4 NOT NULL,
	"FloatFieldNull" float4,
	"DoubleField" float8 NOT NULL,
	"DoubleFieldNull" float8,
	"DecimalField" numeric(10,4) NOT NULL,
	"DecimalFieldNull" numeric(10,4),
	"DateTimeField" timestamp(6) NOT NULL,
	"DateTimeFieldNull" timestamp(6) NULL,
	"VarcharField" varchar(2000) NOT NULL COLLATE "default",
	"VarcharFieldNull" varchar(2000) COLLATE "default",
	"TextField" text NOT NULL COLLATE "default",
	"TextFieldNull" text COLLATE "default",
	"BigDataField" bytea NOT NULL,
	"BigDataFieldNull" bytea,
	"EnumInt32Field" int4 NOT NULL,
	"EnumInt32FieldNull" int4,
	"EnumInt64Field" int8 NOT NULL,
	"EnumInt64FieldNull" int8
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseField_SelectInsert_NoIdentity" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseField_Config
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseField_Config";
CREATE TABLE "public"."Te_BaseField_Config" (
	"Id" int4 NOT NULL DEFAULT nextval('"Te_BaseField_Config_Id_seq"'::regclass),
	"Int32_Field" int4 NOT NULL,
	"Int32_FieldNull" int4,
	"Decimal_Field" numeric(10,4) NOT NULL,
	"Decimal_FieldNull" numeric(10,4),
	"DateTime_Field" timestamp(6) NOT NULL,
	"DateTime_FieldNull" timestamp(6) NULL,
	"Varchar_Field" varchar(2000) NOT NULL COLLATE "default",
	"Varchar_FieldNull" varchar(2000) COLLATE "default",
	"Now_Field" timestamp(6) NOT NULL,
	"Now_FieldNull" timestamp(6) NULL,
	"Today_Field" timestamp(6) NOT NULL,
	"Today_FieldNull" timestamp(6) NULL,
	"EnumInt32_Field" int4 NOT NULL,
	"EnumInt32_FieldNull" int4
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseField_Config" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_RelateSub_Config
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_RelateSub_Config";
CREATE TABLE "public"."Te_RelateSub_Config" (
	"Id" int4 NOT NULL DEFAULT nextval('"Te_RelateSub_Config_Id_seq"'::regclass),
	"MainId" int4 NOT NULL,
	"DecimalField" numeric(10,4) NOT NULL,
	"DateTimeField" timestamp(6) NOT NULL,
	"VarcharField" varchar(255) NOT NULL COLLATE "default"
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_RelateSub_Config" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseField_Config_Replace
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseField_Config_Replace";
CREATE TABLE "public"."Te_BaseField_Config_Replace" (
	"Id" int4 NOT NULL DEFAULT nextval('"Te_BaseField_Config_Replace_Id_seq"'::regclass),
	"Int32_Field" int4 NOT NULL,
	"Int32_FieldNull" int4,
	"Decimal_Field" numeric(10,4) NOT NULL,
	"Decimal_FieldNull" numeric(10,4),
	"DateTime_Field" timestamp(6) NOT NULL,
	"DateTime_FieldNull" timestamp(6) NULL,
	"Varchar_Field" varchar(2000) NOT NULL COLLATE "default",
	"Varchar_FieldNull" varchar(2000) COLLATE "default",
	"Now_Field" timestamp(6) NOT NULL,
	"Now_FieldNull" timestamp(6) NULL,
	"Today_Field" timestamp(6) NOT NULL,
	"Today_FieldNull" timestamp(6) NULL,
	"EnumInt32_Field" int4 NOT NULL,
	"EnumInt32_FieldNull" int4
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseField_Config_Replace" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseField_Entity
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseField_Entity";
CREATE TABLE "public"."Te_BaseField_Entity" (
	"Id" int4 NOT NULL DEFAULT nextval('"Te_BaseField_Entity_Id_seq"'::regclass),
	"BoolField" bool NOT NULL,
	"BoolFieldNull" bool,
	"SbyteField" int2 NOT NULL,
	"SbyteFieldNull" int2,
	"ByteField" int2 NOT NULL,
	"ByteFieldNull" int2,
	"Int16Field" int2 NOT NULL,
	"Int16FieldNull" int2,
	"UInt16Field" int4 NOT NULL,
	"UInt16FieldNull" int4,
	"Int32Field" int4 NOT NULL,
	"Int32FieldNull" int4,
	"UInt32Field" int8 NOT NULL,
	"UInt32FieldNull" int8,
	"Int64Field" int8 NOT NULL,
	"Int64FieldNull" int8,
	"UInt64Field" numeric(20,0) NOT NULL,
	"UInt64FieldNull" numeric(20,0),
	"FloatField" float4 NOT NULL,
	"FloatFieldNull" float4,
	"DoubleField" float8 NOT NULL,
	"DoubleFieldNull" float8,
	"DecimalField" numeric(10,4) NOT NULL,
	"DecimalFieldNull" numeric(10,4),
	"DateTimeField" timestamp(6) NOT NULL,
	"DateTimeFieldNull" timestamp(6) NULL,
	"VarcharField" varchar(2000) NOT NULL COLLATE "default",
	"VarcharFieldNull" varchar(2000) COLLATE "default",
	"TextField" text NOT NULL COLLATE "default",
	"TextFieldNull" text COLLATE "default",
	"BigDataField" bytea NOT NULL,
	"BigDataFieldNull" bytea,
	"EnumInt32Field" int4 NOT NULL,
	"EnumInt32FieldNull" int4,
	"EnumInt64Field" int8 NOT NULL,
	"EnumInt64FieldNull" int8
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseField_Entity" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseFieldNoIdentity_Entity
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseFieldNoIdentity_Entity";
CREATE TABLE "public"."Te_BaseFieldNoIdentity_Entity" (
	"Id" int4 NOT NULL,
	"Int32Field" int4 NOT NULL,
	"DoubleField" float8 NOT NULL,
	"VarcharField" varchar(2000) NOT NULL COLLATE "default",
	"DateTimeField" timestamp(6) NOT NULL,
	"EnumInt32Field" int4 NOT NULL
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseFieldNoIdentity_Entity" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_BaseField_Alias
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_BaseField_Alias";
CREATE TABLE "public"."Te_BaseField_Alias" (
	"Id" int4 NOT NULL DEFAULT nextval('"Te_BaseField_Alias_Id_seq"'::regclass),
	"BoolField" bool NOT NULL,
	"BoolFieldNull" bool,
	"SbyteField" int2 NOT NULL,
	"SbyteFieldNull" int2,
	"ByteField" int2 NOT NULL,
	"ByteFieldNull" int2,
	"Int16Field" int2 NOT NULL,
	"Int16FieldNull" int2,
	"UInt16Field" int4 NOT NULL,
	"UInt16FieldNull" int4,
	"Int32Field" int4 NOT NULL,
	"Int32FieldNull" int4,
	"UInt32Field" int8 NOT NULL,
	"UInt32FieldNull" int8,
	"Int64Field" int8 NOT NULL,
	"Int64FieldNull" int8,
	"UInt64Field" numeric(20,0) NOT NULL,
	"UInt64FieldNull" numeric(20,0),
	"FloatField" float4 NOT NULL,
	"FloatFieldNull" float4,
	"DoubleField" float8 NOT NULL,
	"DoubleFieldNull" float8,
	"DecimalField" numeric(10,4) NOT NULL,
	"DecimalFieldNull" numeric(10,4),
	"DateTimeField" timestamp(6) NOT NULL,
	"DateTimeFieldNull" timestamp(6) NULL,
	"VarcharField" varchar(2000) NOT NULL COLLATE "default",
	"VarcharFieldNull" varchar(2000) COLLATE "default",
	"TextField" text NOT NULL COLLATE "default",
	"TextFieldNull" text COLLATE "default",
	"BigDataField" bytea NOT NULL,
	"BigDataFieldNull" bytea,
	"EnumInt32Field" int4 NOT NULL,
	"EnumInt32FieldNull" int4,
	"EnumInt64Field" int8 NOT NULL,
	"EnumInt64FieldNull" int8
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_BaseField_Alias" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_MainTable
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_MainTable";
CREATE TABLE "public"."Te_MainTable" (
	"MainId" int4 NOT NULL DEFAULT nextval('"Te_MainTable_MainId_seq"'::regclass),
	"Int32Field" int4 NOT NULL,
	"Int32FieldNull" int4,
	"DecimalField" numeric(10,4) NOT NULL,
	"DecimalFieldNull" numeric(10,4),
	"DateTimeField" timestamp(6) NOT NULL,
	"DateTimeFieldNull" timestamp(6) NULL,
	"VarcharField" varchar(2000) NOT NULL COLLATE "default",
	"VarcharFieldNull" varchar(2000) COLLATE "default",
	"SubId" int4 NOT NULL
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_MainTable" OWNER TO "root";

-- ----------------------------
--  Table structure for Te_SubTable
-- ----------------------------
DROP TABLE IF EXISTS "public"."Te_SubTable";
CREATE TABLE "public"."Te_SubTable" (
	"SubId" int4 NOT NULL DEFAULT nextval('"Te_SubTable_SubId_seq"'::regclass),
	"Int32Field" int4 NOT NULL,
	"Int32FieldNull" int4,
	"DecimalField" numeric(10,4) NOT NULL,
	"DecimalFieldNull" numeric(10,4),
	"DateTimeField" timestamp(6) NOT NULL,
	"DateTimeFieldNull" timestamp(6) NULL,
	"VarcharField" varchar(2000) NOT NULL COLLATE "default",
	"VarcharFieldNull" varchar(2000) COLLATE "default"
)
WITH (OIDS=FALSE);
ALTER TABLE "public"."Te_SubTable" OWNER TO "root";


-- ----------------------------
--  Alter sequences owned by
-- ----------------------------
ALTER SEQUENCE "public"."Te_BaseFieldAggregateField_Id_seq" RESTART 46 OWNED BY "Te_BaseFieldAggregateField"."Id";
ALTER SEQUENCE "public"."Te_BaseFieldExpression_Extend_ExtendId_seq" RESTART 11 OWNED BY "Te_BaseFieldExpression_Extend"."ExtendId";
ALTER SEQUENCE "public"."Te_BaseFieldExpression_Id_seq" RESTART 46 OWNED BY "Te_BaseFieldExpression"."Id";
ALTER SEQUENCE "public"."Te_BaseFieldSelectField_Id_seq" RESTART 46 OWNED BY "Te_BaseFieldSelectField"."Id";
ALTER SEQUENCE "public"."Te_BaseField_Alias_Id_seq" RESTART 2 OWNED BY "Te_BaseField_Alias"."Id";
ALTER SEQUENCE "public"."Te_BaseField_Config_Id_seq" RESTART 2 OWNED BY "Te_BaseField_Config"."Id";
ALTER SEQUENCE "public"."Te_BaseField_Config_Replace_Id_seq" RESTART 2 OWNED BY "Te_BaseField_Config_Replace"."Id";
ALTER SEQUENCE "public"."Te_BaseField_DefaultValue_Id_seq" RESTART 2 OWNED BY "Te_BaseField_DefaultValue"."Id";
ALTER SEQUENCE "public"."Te_BaseField_Entity_Id_seq" RESTART 34 OWNED BY "Te_BaseField_Entity"."Id";
ALTER SEQUENCE "public"."Te_BaseField_Id_seq" RESTART 46 OWNED BY "Te_BaseField"."Id";
ALTER SEQUENCE "public"."Te_BaseField_NullMiniValue_Id_seq" RESTART 11 OWNED BY "Te_BaseField_NullMiniValue"."Id";
ALTER SEQUENCE "public"."Te_BaseField_SelectInsert_Id_seq" RESTART 29 OWNED BY "Te_BaseField_SelectInsert"."Id";
ALTER SEQUENCE "public"."Te_MainTable_MainId_seq" RESTART 11 OWNED BY "Te_MainTable"."MainId";
ALTER SEQUENCE "public"."Te_ObjectField_Id_seq" RESTART 46 OWNED BY "Te_ObjectField"."Id";
ALTER SEQUENCE "public"."Te_RelateCollection_Id_seq" RESTART 121 OWNED BY "Te_RelateCollection"."Id";
ALTER SEQUENCE "public"."Te_RelateMain_Config_Id_seq" RESTART 11 OWNED BY "Te_RelateMain_Config"."Id";
ALTER SEQUENCE "public"."Te_RelateSub_Config_Id_seq" RESTART 6 OWNED BY "Te_RelateSub_Config"."Id";
ALTER SEQUENCE "public"."Te_SubTable_SubId_seq" RESTART 11 OWNED BY "Te_SubTable"."SubId";
-- ----------------------------
--  Primary key structure for table Te_BaseField_SelectInsert
-- ----------------------------
ALTER TABLE "public"."Te_BaseField_SelectInsert" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseFieldExtend
-- ----------------------------
ALTER TABLE "public"."Te_BaseFieldExtend" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_JoinTable_SelectInsert
-- ----------------------------
ALTER TABLE "public"."Te_JoinTable_SelectInsert" ADD PRIMARY KEY ("MainId") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_RelateF
-- ----------------------------
ALTER TABLE "public"."Te_RelateF" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseFieldNoIdentity
-- ----------------------------
ALTER TABLE "public"."Te_BaseFieldNoIdentity" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_RelateC
-- ----------------------------
ALTER TABLE "public"."Te_RelateC" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_RelateD
-- ----------------------------
ALTER TABLE "public"."Te_RelateD" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_RelateB
-- ----------------------------
ALTER TABLE "public"."Te_RelateB" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_RelateCollection
-- ----------------------------
ALTER TABLE "public"."Te_RelateCollection" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseFieldExpression
-- ----------------------------
ALTER TABLE "public"."Te_BaseFieldExpression" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_RelateMain_Config
-- ----------------------------
ALTER TABLE "public"."Te_RelateMain_Config" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseField_DefaultValue
-- ----------------------------
ALTER TABLE "public"."Te_BaseField_DefaultValue" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_ObjectField
-- ----------------------------
ALTER TABLE "public"."Te_ObjectField" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseFieldExpression_Extend
-- ----------------------------
ALTER TABLE "public"."Te_BaseFieldExpression_Extend" ADD PRIMARY KEY ("ExtendId") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseFieldSelectField
-- ----------------------------
ALTER TABLE "public"."Te_BaseFieldSelectField" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseFieldAggregateField_GroupBy
-- ----------------------------
ALTER TABLE "public"."Te_BaseFieldAggregateField_GroupBy" ADD PRIMARY KEY ("KeyData", "MonthData") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseFieldAggregateField
-- ----------------------------
ALTER TABLE "public"."Te_BaseFieldAggregateField" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseField
-- ----------------------------
ALTER TABLE "public"."Te_BaseField" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseField_NullMiniValue
-- ----------------------------
ALTER TABLE "public"."Te_BaseField_NullMiniValue" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_RelateE
-- ----------------------------
ALTER TABLE "public"."Te_RelateE" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_RelateA
-- ----------------------------
ALTER TABLE "public"."Te_RelateA" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseField_SelectInsert_NoIdentity
-- ----------------------------
ALTER TABLE "public"."Te_BaseField_SelectInsert_NoIdentity" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseField_Config
-- ----------------------------
ALTER TABLE "public"."Te_BaseField_Config" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_RelateSub_Config
-- ----------------------------
ALTER TABLE "public"."Te_RelateSub_Config" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseField_Config_Replace
-- ----------------------------
ALTER TABLE "public"."Te_BaseField_Config_Replace" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseField_Entity
-- ----------------------------
ALTER TABLE "public"."Te_BaseField_Entity" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseFieldNoIdentity_Entity
-- ----------------------------
ALTER TABLE "public"."Te_BaseFieldNoIdentity_Entity" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_BaseField_Alias
-- ----------------------------
ALTER TABLE "public"."Te_BaseField_Alias" ADD PRIMARY KEY ("Id") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_MainTable
-- ----------------------------
ALTER TABLE "public"."Te_MainTable" ADD PRIMARY KEY ("MainId") NOT DEFERRABLE INITIALLY IMMEDIATE;

-- ----------------------------
--  Primary key structure for table Te_SubTable
-- ----------------------------
ALTER TABLE "public"."Te_SubTable" ADD PRIMARY KEY ("SubId") NOT DEFERRABLE INITIALLY IMMEDIATE;

