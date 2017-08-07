using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data.Sample
{
    public enum GenderType
    {
        Male,
        Female
    }

    public enum CheckLevelType
    {
        Low,
        Normal,
        High
    }

    [DataTable("Te_AreaInfo")]
    public class TeAreaInfo
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private string name;

        /// <summary>
        /// Name
        /// </summary>
        /// <value></value>
        [DataField("Name")]
        public string Name {
            get {
                return this.name;
            }
            set {
                this.name = value;
            }
        }
        private int v1;

        /// <summary>
        /// V1
        /// </summary>
        /// <value></value>
        [DataField("V1")]
        public int V1 {
            get {
                return this.v1;
            }
            set {
                this.v1 = value;
            }
        }
        private int v2;

        /// <summary>
        /// V2
        /// </summary>
        /// <value></value>
        [DataField("V2")]
        public int V2 {
            get {
                return this.v2;
            }
            set {
                this.v2 = value;
            }
        }
        private int v3;

        /// <summary>
        /// V3
        /// </summary>
        /// <value></value>
        [DataField("V3")]
        public int V3 {
            get {
                return this.v3;
            }
            set {
                this.v3 = value;
            }
        }
        #endregion
    }


    [DataTable("Te_Article")]
    public class TeArticle
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private int userId;

        /// <summary>
        /// UserId
        /// </summary>
        /// <value></value>
        [DataField("UserId")]
        public int UserId {
            get {
                return this.userId;
            }
            set {
                this.userId = value;
            }
        }
        private string title;

        /// <summary>
        /// Title
        /// </summary>
        /// <value></value>
        [DataField("Title")]
        public string Title {
            get {
                return this.title;
            }
            set {
                this.title = value;
            }
        }
        private string content;

        /// <summary>
        /// Content
        /// </summary>
        /// <value></value>
        [DataField("Content")]
        public string Content {
            get {
                return this.content;
            }
            set {
                this.content = value;
            }
        }
        private DateTime publishTime;

        /// <summary>
        /// PublishTime
        /// </summary>
        /// <value></value>
        [DataField("PublishTime")]
        public DateTime PublishTime {
            get {
                return this.publishTime;
            }
            set {
                this.publishTime = value;
            }
        }
        private int status;

        /// <summary>
        /// Status
        /// </summary>
        /// <value></value>
        [DataField("Status")]
        public int Status {
            get {
                return this.status;
            }
            set {
                this.status = value;
            }
        }
        private int readNum;

        /// <summary>
        /// ReadNum
        /// </summary>
        /// <value></value>
        [DataField("ReadNum")]
        public int ReadNum {
            get {
                return this.readNum;
            }
            set {
                this.readNum = value;
            }
        }
        private int praiseNum;

        /// <summary>
        /// PraiseNum
        /// </summary>
        /// <value></value>
        [DataField("PraiseNum")]
        public int PraiseNum {
            get {
                return this.praiseNum;
            }
            set {
                this.praiseNum = value;
            }
        }
        private DateTime lastModifyTime;

        /// <summary>
        /// LastModifyTime
        /// </summary>
        /// <value></value>
        [DataField("LastModifyTime")]
        public DateTime LastModifyTime {
            get {
                return this.lastModifyTime;
            }
            set {
                this.lastModifyTime = value;
            }
        }
        private DateTime lastCommentTime;

        /// <summary>
        /// LastCommentTime
        /// </summary>
        /// <value></value>
        [DataField("LastCommentTime")]
        public DateTime LastCommentTime {
            get {
                return this.lastCommentTime;
            }
            set {
                this.lastCommentTime = value;
            }
        }
        private string tags;

        /// <summary>
        /// Tags
        /// </summary>
        /// <value></value>
        [DataField("Tags", IsNullable = true)]
        public string Tags {
            get {
                return this.tags;
            }
            set {
                this.tags = value;
            }
        }
        private string columnId;

        /// <summary>
        /// ColumnId
        /// </summary>
        /// <value></value>
        [DataField("ColumnId", IsNullable = true)]
        public string ColumnId {
            get {
                return this.columnId;
            }
            set {
                this.columnId = value;
            }
        }
        #endregion
    }


    [DataTable("Te_ArticleColumn")]
    public class TeArticleColumn
    {
        #region "Data Property"
        private string columnId;

        /// <summary>
        /// ColumnId
        /// </summary>
        /// <value></value>
        [DataField("ColumnId", IsPrimaryKey = true)]
        public string ColumnId {
            get {
                return this.columnId;
            }
            set {
                this.columnId = value;
            }
        }
        private string columnName;

        /// <summary>
        /// ColumnName
        /// </summary>
        /// <value></value>
        [DataField("ColumnName")]
        public string ColumnName {
            get {
                return this.columnName;
            }
            set {
                this.columnName = value;
            }
        }
        private string parentId;

        /// <summary>
        /// ParentId
        /// </summary>
        /// <value></value>
        [DataField("ParentId")]
        public string ParentId {
            get {
                return this.parentId;
            }
            set {
                this.parentId = value;
            }
        }
        private int status;

        /// <summary>
        /// Status
        /// </summary>
        /// <value></value>
        [DataField("Status")]
        public int Status {
            get {
                return this.status;
            }
            set {
                this.status = value;
            }
        }
        private string remark;

        /// <summary>
        /// Remark
        /// </summary>
        /// <value></value>
        [DataField("Remark", IsNullable = true)]
        public string Remark {
            get {
                return this.remark;
            }
            set {
                this.remark = value;
            }
        }
        #endregion
    }


    [DataTable("Te_ArticleComment")]
    public class TeArticleComment
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private int articleId;

        /// <summary>
        /// ArticleId
        /// </summary>
        /// <value></value>
        [DataField("ArticleId")]
        public int ArticleId {
            get {
                return this.articleId;
            }
            set {
                this.articleId = value;
            }
        }
        private int userId;

        /// <summary>
        /// UserId
        /// </summary>
        /// <value></value>
        [DataField("UserId")]
        public int UserId {
            get {
                return this.userId;
            }
            set {
                this.userId = value;
            }
        }
        private string content;

        /// <summary>
        /// Content
        /// </summary>
        /// <value></value>
        [DataField("Content")]
        public string Content {
            get {
                return this.content;
            }
            set {
                this.content = value;
            }
        }
        private DateTime publishTime;

        /// <summary>
        /// PublishTime
        /// </summary>
        /// <value></value>
        [DataField("PublishTime")]
        public DateTime PublishTime {
            get {
                return this.publishTime;
            }
            set {
                this.publishTime = value;
            }
        }
        private int status;

        /// <summary>
        /// Status
        /// </summary>
        /// <value></value>
        [DataField("Status")]
        public int Status {
            get {
                return this.status;
            }
            set {
                this.status = value;
            }
        }
        #endregion
    }


    [DataTable("Te_CheckValue")]
    public class TeCheckValue
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private int checkId;

        /// <summary>
        /// CheckId
        /// </summary>
        /// <value></value>
        [DataField("CheckId")]
        public int CheckId {
            get {
                return this.checkId;
            }
            set {
                this.checkId = value;
            }
        }
        private double checkRate;

        /// <summary>
        /// CheckRate
        /// </summary>
        /// <value></value>
        [DataField("CheckRate")]
        public double CheckRate {
            get {
                return this.checkRate;
            }
            set {
                this.checkRate = value;
            }
        }
        private DateTime checkTime;

        /// <summary>
        /// CheckTime
        /// </summary>
        /// <value></value>
        [DataField("CheckTime")]
        public DateTime CheckTime {
            get {
                return this.checkTime;
            }
            set {
                this.checkTime = value;
            }
        }
        private DateTime checkDate;

        /// <summary>
        /// CheckDate
        /// </summary>
        /// <value></value>
        [DataField("CheckDate")]
        public DateTime CheckDate {
            get {
                return this.checkDate;
            }
            set {
                this.checkDate = value;
            }
        }
        private string checkData;

        /// <summary>
        /// CheckData
        /// </summary>
        /// <value></value>
        [DataField("CheckData")]
        public string CheckData {
            get {
                return this.checkData;
            }
            set {
                this.checkData = value;
            }
        }
        private int checkLevel;

        /// <summary>
        /// CheckLevel
        /// </summary>
        /// <value></value>
        [DataField("CheckLevel")]
        public int CheckLevel {
            get {
                return this.checkLevel;
            }
            set {
                this.checkLevel = value;
            }
        }
        #endregion
    }


    [DataTable("Te_DataLog")]
    public class TeDataLog
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private int userId;

        /// <summary>
        /// UserId
        /// </summary>
        /// <value></value>
        [DataField("UserId")]
        public int UserId {
            get {
                return this.userId;
            }
            set {
                this.userId = value;
            }
        }
        private int articleId;

        /// <summary>
        /// ArticleId
        /// </summary>
        /// <value></value>
        [DataField("ArticleId")]
        public int ArticleId {
            get {
                return this.articleId;
            }
            set {
                this.articleId = value;
            }
        }
        private DateTime recordTime;

        /// <summary>
        /// RecordTime
        /// </summary>
        /// <value></value>
        [DataField("RecordTime")]
        public DateTime RecordTime {
            get {
                return this.recordTime;
            }
            set {
                this.recordTime = value;
            }
        }
        private int status;

        /// <summary>
        /// Status
        /// </summary>
        /// <value></value>
        [DataField("Status")]
        public int Status {
            get {
                return this.status;
            }
            set {
                this.status = value;
            }
        }
        private int action;

        /// <summary>
        /// Action
        /// </summary>
        /// <value></value>
        [DataField("Action")]
        public int Action {
            get {
                return this.action;
            }
            set {
                this.action = value;
            }
        }
        private string requestUrl;

        /// <summary>
        /// RequestUrl
        /// </summary>
        /// <value></value>
        [DataField("RequestUrl")]
        public string RequestUrl {
            get {
                return this.requestUrl;
            }
            set {
                this.requestUrl = value;
            }
        }
        private int? checkId;

        /// <summary>
        /// CheckId
        /// </summary>
        /// <value></value>
        [DataField("CheckId", IsNullable = true)]
        public int? CheckId {
            get {
                return this.checkId;
            }
            set {
                this.checkId = value;
            }
        }
        private double? checkPoint;

        /// <summary>
        /// CheckPoint
        /// </summary>
        /// <value></value>
        [DataField("CheckPoint", IsNullable = true)]
        public double? CheckPoint {
            get {
                return this.checkPoint;
            }
            set {
                this.checkPoint = value;
            }
        }
        private DateTime? checkTime;

        /// <summary>
        /// CheckTime
        /// </summary>
        /// <value></value>
        [DataField("CheckTime", IsNullable = true)]
        public DateTime? CheckTime {
            get {
                return this.checkTime;
            }
            set {
                this.checkTime = value;
            }
        }
        private string checkData;

        /// <summary>
        /// CheckData
        /// </summary>
        /// <value></value>
        [DataField("CheckData", IsNullable = true)]
        public string CheckData {
            get {
                return this.checkData;
            }
            set {
                this.checkData = value;
            }
        }
        private CheckLevelType? checkLevelTypeInt;

        /// <summary>
        /// #EnumType:CheckLevelType#level
        /// </summary>
        /// <value></value>
        [DataField("Check_LevelTypeInt", IsNullable = true)]
        public CheckLevelType? CheckLevelTypeInt {
            get {
                return this.checkLevelTypeInt;
            }
            set {
                this.checkLevelTypeInt = value;
            }
        }
        #endregion
    }


    [DataTable("Te_DataLogHistory")]
    public class TeDataLogHistory
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id")]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private int userId;

        /// <summary>
        /// UserId
        /// </summary>
        /// <value></value>
        [DataField("UserId")]
        public int UserId {
            get {
                return this.userId;
            }
            set {
                this.userId = value;
            }
        }
        private int articleId;

        /// <summary>
        /// ArticleId
        /// </summary>
        /// <value></value>
        [DataField("ArticleId")]
        public int ArticleId {
            get {
                return this.articleId;
            }
            set {
                this.articleId = value;
            }
        }
        private DateTime recordTime;

        /// <summary>
        /// RecordTime
        /// </summary>
        /// <value></value>
        [DataField("RecordTime")]
        public DateTime RecordTime {
            get {
                return this.recordTime;
            }
            set {
                this.recordTime = value;
            }
        }
        private int status;

        /// <summary>
        /// Status
        /// </summary>
        /// <value></value>
        [DataField("Status")]
        public int Status {
            get {
                return this.status;
            }
            set {
                this.status = value;
            }
        }
        private int action;

        /// <summary>
        /// Action
        /// </summary>
        /// <value></value>
        [DataField("Action")]
        public int Action {
            get {
                return this.action;
            }
            set {
                this.action = value;
            }
        }
        private string requestUrl;

        /// <summary>
        /// RequestUrl
        /// </summary>
        /// <value></value>
        [DataField("RequestUrl")]
        public string RequestUrl {
            get {
                return this.requestUrl;
            }
            set {
                this.requestUrl = value;
            }
        }
        private int? checkId;

        /// <summary>
        /// CheckId
        /// </summary>
        /// <value></value>
        [DataField("CheckId", IsNullable = true)]
        public int? CheckId {
            get {
                return this.checkId;
            }
            set {
                this.checkId = value;
            }
        }
        private double? checkPoint;

        /// <summary>
        /// CheckPoint
        /// </summary>
        /// <value></value>
        [DataField("CheckPoint", IsNullable = true)]
        public double? CheckPoint {
            get {
                return this.checkPoint;
            }
            set {
                this.checkPoint = value;
            }
        }
        private DateTime? checkTime;

        /// <summary>
        /// CheckTime
        /// </summary>
        /// <value></value>
        [DataField("CheckTime", IsNullable = true)]
        public DateTime? CheckTime {
            get {
                return this.checkTime;
            }
            set {
                this.checkTime = value;
            }
        }
        private string checkData;

        /// <summary>
        /// CheckData
        /// </summary>
        /// <value></value>
        [DataField("CheckData", IsNullable = true)]
        public string CheckData {
            get {
                return this.checkData;
            }
            set {
                this.checkData = value;
            }
        }
        private CheckLevelType? checkLevelTypeInt;

        /// <summary>
        /// #EnumType:CheckLevelType#level
        /// </summary>
        /// <value></value>
        [DataField("Check_LevelTypeInt", IsNullable = true)]
        public CheckLevelType? CheckLevelTypeInt {
            get {
                return this.checkLevelTypeInt;
            }
            set {
                this.checkLevelTypeInt = value;
            }
        }
        #endregion
    }


    [DataTable("Te_DataLogHistory2")]
    public class TeDataLogHistory2
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private int userId;

        /// <summary>
        /// UserId
        /// </summary>
        /// <value></value>
        [DataField("UserId")]
        public int UserId {
            get {
                return this.userId;
            }
            set {
                this.userId = value;
            }
        }
        private int articleId;

        /// <summary>
        /// ArticleId
        /// </summary>
        /// <value></value>
        [DataField("ArticleId")]
        public int ArticleId {
            get {
                return this.articleId;
            }
            set {
                this.articleId = value;
            }
        }
        private DateTime recordTime;

        /// <summary>
        /// RecordTime
        /// </summary>
        /// <value></value>
        [DataField("RecordTime")]
        public DateTime RecordTime {
            get {
                return this.recordTime;
            }
            set {
                this.recordTime = value;
            }
        }
        private int status;

        /// <summary>
        /// Status
        /// </summary>
        /// <value></value>
        [DataField("Status")]
        public int Status {
            get {
                return this.status;
            }
            set {
                this.status = value;
            }
        }
        private int action;

        /// <summary>
        /// Action
        /// </summary>
        /// <value></value>
        [DataField("Action")]
        public int Action {
            get {
                return this.action;
            }
            set {
                this.action = value;
            }
        }
        private string requestUrl;

        /// <summary>
        /// RequestUrl
        /// </summary>
        /// <value></value>
        [DataField("RequestUrl")]
        public string RequestUrl {
            get {
                return this.requestUrl;
            }
            set {
                this.requestUrl = value;
            }
        }
        private int? checkId;

        /// <summary>
        /// CheckId
        /// </summary>
        /// <value></value>
        [DataField("CheckId", IsNullable = true)]
        public int? CheckId {
            get {
                return this.checkId;
            }
            set {
                this.checkId = value;
            }
        }
        private double? checkPoint;

        /// <summary>
        /// CheckPoint
        /// </summary>
        /// <value></value>
        [DataField("CheckPoint", IsNullable = true)]
        public double? CheckPoint {
            get {
                return this.checkPoint;
            }
            set {
                this.checkPoint = value;
            }
        }
        private DateTime? checkTime;

        /// <summary>
        /// CheckTime
        /// </summary>
        /// <value></value>
        [DataField("CheckTime", IsNullable = true)]
        public DateTime? CheckTime {
            get {
                return this.checkTime;
            }
            set {
                this.checkTime = value;
            }
        }
        private string checkData;

        /// <summary>
        /// CheckData
        /// </summary>
        /// <value></value>
        [DataField("CheckData", IsNullable = true)]
        public string CheckData {
            get {
                return this.checkData;
            }
            set {
                this.checkData = value;
            }
        }
        private CheckLevelType? checkLevelTypeInt;

        /// <summary>
        /// #EnumType:CheckLevelType#level
        /// </summary>
        /// <value></value>
        [DataField("Check_LevelTypeInt", IsNullable = true)]
        public CheckLevelType? CheckLevelTypeInt {
            get {
                return this.checkLevelTypeInt;
            }
            set {
                this.checkLevelTypeInt = value;
            }
        }
        #endregion
    }


    [DataTable("Te_RelateA")]
    public class TeRelateA
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private int relateBId;

        /// <summary>
        /// RelateBId
        /// </summary>
        /// <value></value>
        [DataField("RelateBId")]
        public int RelateBId {
            get {
                return this.relateBId;
            }
            set {
                this.relateBId = value;
            }
        }
        private int relateCId;

        /// <summary>
        /// RelateCId
        /// </summary>
        /// <value></value>
        [DataField("RelateCId")]
        public int RelateCId {
            get {
                return this.relateCId;
            }
            set {
                this.relateCId = value;
            }
        }
        private int relateDId;

        /// <summary>
        /// RelateDId
        /// </summary>
        /// <value></value>
        [DataField("RelateDId")]
        public int RelateDId {
            get {
                return this.relateDId;
            }
            set {
                this.relateDId = value;
            }
        }
        private int relateEId;

        /// <summary>
        /// RelateEId
        /// </summary>
        /// <value></value>
        [DataField("RelateEId")]
        public int RelateEId {
            get {
                return this.relateEId;
            }
            set {
                this.relateEId = value;
            }
        }
        private int relateFId;

        /// <summary>
        /// RelateFId
        /// </summary>
        /// <value></value>
        [DataField("RelateFId")]
        public int RelateFId {
            get {
                return this.relateFId;
            }
            set {
                this.relateFId = value;
            }
        }
        private string content;

        /// <summary>
        /// Content
        /// </summary>
        /// <value></value>
        [DataField("Content")]
        public string Content {
            get {
                return this.content;
            }
            set {
                this.content = value;
            }
        }
        #endregion
    }


    [DataTable("Te_RelateB")]
    public class TeRelateB
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private int relateAId;

        /// <summary>
        /// RelateAId
        /// </summary>
        /// <value></value>
        [DataField("RelateAId")]
        public int RelateAId {
            get {
                return this.relateAId;
            }
            set {
                this.relateAId = value;
            }
        }
        private int relateCId;

        /// <summary>
        /// RelateCId
        /// </summary>
        /// <value></value>
        [DataField("RelateCId")]
        public int RelateCId {
            get {
                return this.relateCId;
            }
            set {
                this.relateCId = value;
            }
        }
        private int relateDId;

        /// <summary>
        /// RelateDId
        /// </summary>
        /// <value></value>
        [DataField("RelateDId")]
        public int RelateDId {
            get {
                return this.relateDId;
            }
            set {
                this.relateDId = value;
            }
        }
        private int relateEId;

        /// <summary>
        /// RelateEId
        /// </summary>
        /// <value></value>
        [DataField("RelateEId")]
        public int RelateEId {
            get {
                return this.relateEId;
            }
            set {
                this.relateEId = value;
            }
        }
        private int relateFId;

        /// <summary>
        /// RelateFId
        /// </summary>
        /// <value></value>
        [DataField("RelateFId")]
        public int RelateFId {
            get {
                return this.relateFId;
            }
            set {
                this.relateFId = value;
            }
        }
        private string content;

        /// <summary>
        /// Content
        /// </summary>
        /// <value></value>
        [DataField("Content")]
        public string Content {
            get {
                return this.content;
            }
            set {
                this.content = value;
            }
        }
        #endregion
    }


    [DataTable("Te_RelateC")]
    public class TeRelateC
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private int relateAId;

        /// <summary>
        /// RelateAId
        /// </summary>
        /// <value></value>
        [DataField("RelateAId")]
        public int RelateAId {
            get {
                return this.relateAId;
            }
            set {
                this.relateAId = value;
            }
        }
        private int relateBId;

        /// <summary>
        /// RelateBId
        /// </summary>
        /// <value></value>
        [DataField("RelateBId")]
        public int RelateBId {
            get {
                return this.relateBId;
            }
            set {
                this.relateBId = value;
            }
        }
        private int relateDId;

        /// <summary>
        /// RelateDId
        /// </summary>
        /// <value></value>
        [DataField("RelateDId")]
        public int RelateDId {
            get {
                return this.relateDId;
            }
            set {
                this.relateDId = value;
            }
        }
        private int relateEId;

        /// <summary>
        /// RelateEId
        /// </summary>
        /// <value></value>
        [DataField("RelateEId")]
        public int RelateEId {
            get {
                return this.relateEId;
            }
            set {
                this.relateEId = value;
            }
        }
        private int relateFId;

        /// <summary>
        /// RelateFId
        /// </summary>
        /// <value></value>
        [DataField("RelateFId")]
        public int RelateFId {
            get {
                return this.relateFId;
            }
            set {
                this.relateFId = value;
            }
        }
        private string content;

        /// <summary>
        /// Content
        /// </summary>
        /// <value></value>
        [DataField("Content")]
        public string Content {
            get {
                return this.content;
            }
            set {
                this.content = value;
            }
        }
        #endregion
    }


    [DataTable("Te_RelateD")]
    public class TeRelateD
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private int relateAId;

        /// <summary>
        /// RelateAId
        /// </summary>
        /// <value></value>
        [DataField("RelateAId")]
        public int RelateAId {
            get {
                return this.relateAId;
            }
            set {
                this.relateAId = value;
            }
        }
        private int relateBId;

        /// <summary>
        /// RelateBId
        /// </summary>
        /// <value></value>
        [DataField("RelateBId")]
        public int RelateBId {
            get {
                return this.relateBId;
            }
            set {
                this.relateBId = value;
            }
        }
        private int relateCId;

        /// <summary>
        /// RelateCId
        /// </summary>
        /// <value></value>
        [DataField("RelateCId")]
        public int RelateCId {
            get {
                return this.relateCId;
            }
            set {
                this.relateCId = value;
            }
        }
        private int relateEId;

        /// <summary>
        /// RelateEId
        /// </summary>
        /// <value></value>
        [DataField("RelateEId")]
        public int RelateEId {
            get {
                return this.relateEId;
            }
            set {
                this.relateEId = value;
            }
        }
        private int relateFId;

        /// <summary>
        /// RelateFId
        /// </summary>
        /// <value></value>
        [DataField("RelateFId")]
        public int RelateFId {
            get {
                return this.relateFId;
            }
            set {
                this.relateFId = value;
            }
        }
        private string content;

        /// <summary>
        /// Content
        /// </summary>
        /// <value></value>
        [DataField("Content")]
        public string Content {
            get {
                return this.content;
            }
            set {
                this.content = value;
            }
        }
        #endregion
    }


    [DataTable("Te_RelateE")]
    public class TeRelateE
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private int relateAId;

        /// <summary>
        /// RelateAId
        /// </summary>
        /// <value></value>
        [DataField("RelateAId")]
        public int RelateAId {
            get {
                return this.relateAId;
            }
            set {
                this.relateAId = value;
            }
        }
        private int relateBId;

        /// <summary>
        /// RelateBId
        /// </summary>
        /// <value></value>
        [DataField("RelateBId")]
        public int RelateBId {
            get {
                return this.relateBId;
            }
            set {
                this.relateBId = value;
            }
        }
        private int relateCId;

        /// <summary>
        /// RelateCId
        /// </summary>
        /// <value></value>
        [DataField("RelateCId")]
        public int RelateCId {
            get {
                return this.relateCId;
            }
            set {
                this.relateCId = value;
            }
        }
        private int relateDId;

        /// <summary>
        /// RelateDId
        /// </summary>
        /// <value></value>
        [DataField("RelateDId")]
        public int RelateDId {
            get {
                return this.relateDId;
            }
            set {
                this.relateDId = value;
            }
        }
        private int relateFId;

        /// <summary>
        /// RelateFId
        /// </summary>
        /// <value></value>
        [DataField("RelateFId")]
        public int RelateFId {
            get {
                return this.relateFId;
            }
            set {
                this.relateFId = value;
            }
        }
        private string content;

        /// <summary>
        /// Content
        /// </summary>
        /// <value></value>
        [DataField("Content")]
        public string Content {
            get {
                return this.content;
            }
            set {
                this.content = value;
            }
        }
        #endregion
    }


    [DataTable("Te_RelateF")]
    public class TeRelateF
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private int relateAId;

        /// <summary>
        /// RelateAId
        /// </summary>
        /// <value></value>
        [DataField("RelateAId")]
        public int RelateAId {
            get {
                return this.relateAId;
            }
            set {
                this.relateAId = value;
            }
        }
        private int relateBId;

        /// <summary>
        /// RelateBId
        /// </summary>
        /// <value></value>
        [DataField("RelateBId")]
        public int RelateBId {
            get {
                return this.relateBId;
            }
            set {
                this.relateBId = value;
            }
        }
        private int relateCId;

        /// <summary>
        /// RelateCId
        /// </summary>
        /// <value></value>
        [DataField("RelateCId")]
        public int RelateCId {
            get {
                return this.relateCId;
            }
            set {
                this.relateCId = value;
            }
        }
        private int relateDId;

        /// <summary>
        /// RelateDId
        /// </summary>
        /// <value></value>
        [DataField("RelateDId")]
        public int RelateDId {
            get {
                return this.relateDId;
            }
            set {
                this.relateDId = value;
            }
        }
        private int relateEId;

        /// <summary>
        /// RelateEId
        /// </summary>
        /// <value></value>
        [DataField("RelateEId")]
        public int RelateEId {
            get {
                return this.relateEId;
            }
            set {
                this.relateEId = value;
            }
        }
        private string content;

        /// <summary>
        /// Content
        /// </summary>
        /// <value></value>
        [DataField("Content")]
        public string Content {
            get {
                return this.content;
            }
            set {
                this.content = value;
            }
        }
        #endregion
    }


    [DataTable("Te_TagInfo")]
    public class TeTagInfo
    {
        #region "Data Property"
        private string groupCode;

        /// <summary>
        /// GroupCode
        /// </summary>
        /// <value></value>
        [DataField("GroupCode", IsPrimaryKey = true)]
        public string GroupCode {
            get {
                return this.groupCode;
            }
            set {
                this.groupCode = value;
            }
        }
        private string tagCode;

        /// <summary>
        /// TagCode
        /// </summary>
        /// <value></value>
        [DataField("TagCode", IsPrimaryKey = true)]
        public string TagCode {
            get {
                return this.tagCode;
            }
            set {
                this.tagCode = value;
            }
        }
        private string tagName;

        /// <summary>
        /// TagName
        /// </summary>
        /// <value></value>
        [DataField("TagName")]
        public string TagName {
            get {
                return this.tagName;
            }
            set {
                this.tagName = value;
            }
        }
        private string remark;

        /// <summary>
        /// Remark
        /// </summary>
        /// <value></value>
        [DataField("Remark", IsNullable = true)]
        public string Remark {
            get {
                return this.remark;
            }
            set {
                this.remark = value;
            }
        }
        private int status;

        /// <summary>
        /// Status
        /// </summary>
        /// <value></value>
        [DataField("Status")]
        public int Status {
            get {
                return this.status;
            }
            set {
                this.status = value;
            }
        }
        #endregion
    }


    [DataTable("Te_User")]
    public class TeUser
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private string account;

        /// <summary>
        /// Account
        /// </summary>
        /// <value></value>
        [DataField("Account")]
        public string Account {
            get {
                return this.account;
            }
            set {
                this.account = value;
            }
        }
        private string password;

        /// <summary>
        /// Password
        /// </summary>
        /// <value></value>
        [DataField("Password")]
        public string Password {
            get {
                return this.password;
            }
            set {
                this.password = value;
            }
        }
        private string nickName;

        /// <summary>
        /// NickName
        /// </summary>
        /// <value></value>
        [DataField("NickName", IsNullable = true)]
        public string NickName {
            get {
                return this.nickName;
            }
            set {
                this.nickName = value;
            }
        }
        private GenderType gender;

        /// <summary>
        /// #EnumType:GenderType#sexy
        /// </summary>
        /// <value></value>
        [DataField("Gender")]
        public GenderType Gender {
            get {
                return this.gender;
            }
            set {
                this.gender = value;
            }
        }
        private DateTime birthday;

        /// <summary>
        /// Birthday
        /// </summary>
        /// <value></value>
        [DataField("Birthday")]
        public DateTime Birthday {
            get {
                return this.birthday;
            }
            set {
                this.birthday = value;
            }
        }
        private string telephone;

        /// <summary>
        /// Telephone
        /// </summary>
        /// <value></value>
        [DataField("Telephone", IsNullable = true)]
        public string Telephone {
            get {
                return this.telephone;
            }
            set {
                this.telephone = value;
            }
        }
        private string email;

        /// <summary>
        /// Email
        /// </summary>
        /// <value></value>
        [DataField("Email", IsNullable = true)]
        public string Email {
            get {
                return this.email;
            }
            set {
                this.email = value;
            }
        }
        private string address;

        /// <summary>
        /// Address
        /// </summary>
        /// <value></value>
        [DataField("Address", IsNullable = true)]
        public string Address {
            get {
                return this.address;
            }
            set {
                this.address = value;
            }
        }
        private int levelId;

        /// <summary>
        /// LevelId
        /// </summary>
        /// <value></value>
        [DataField("LevelId")]
        public int LevelId {
            get {
                return this.levelId;
            }
            set {
                this.levelId = value;
            }
        }
        private DateTime regTime;

        /// <summary>
        /// RegTime
        /// </summary>
        /// <value></value>
        [DataField("RegTime")]
        public DateTime RegTime {
            get {
                return this.regTime;
            }
            set {
                this.regTime = value;
            }
        }
        private DateTime? lastLoginTime;

        /// <summary>
        /// LastLoginTime
        /// </summary>
        /// <value></value>
        [DataField("LastLoginTime", IsNullable = true)]
        public DateTime? LastLoginTime {
            get {
                return this.lastLoginTime;
            }
            set {
                this.lastLoginTime = value;
            }
        }
        private int status;

        /// <summary>
        /// Status
        /// </summary>
        /// <value></value>
        [DataField("Status")]
        public int Status {
            get {
                return this.status;
            }
            set {
                this.status = value;
            }
        }
        private double hotRate;

        /// <summary>
        /// HotRate
        /// </summary>
        /// <value></value>
        [DataField("HotRate")]
        public double HotRate {
            get {
                return this.hotRate;
            }
            set {
                this.hotRate = value;
            }
        }
        private int? area;

        /// <summary>
        /// Area
        /// </summary>
        /// <value></value>
        [DataField("Area", IsNullable = true)]
        public int? Area {
            get {
                return this.area;
            }
            set {
                this.area = value;
            }
        }
        private bool deleteFlag;

        /// <summary>
        /// DeleteFlag
        /// </summary>
        /// <value></value>
        [DataField("DeleteFlag")]
        public bool DeleteFlag {
            get {
                return this.deleteFlag;
            }
            set {
                this.deleteFlag = value;
            }
        }
        private int? refereeId;

        /// <summary>
        /// RefereeId
        /// </summary>
        /// <value></value>
        [DataField("RefereeId", IsNullable = true)]
        public int? RefereeId {
            get {
                return this.refereeId;
            }
            set {
                this.refereeId = value;
            }
        }
        private double? checkPoint;

        /// <summary>
        /// Check_Point
        /// </summary>
        /// <value></value>
        [DataField("Check_Point", IsNullable = true)]
        public double? CheckPoint {
            get {
                return this.checkPoint;
            }
            set {
                this.checkPoint = value;
            }
        }
        private bool? checkStatus;

        /// <summary>
        /// Check_Status
        /// </summary>
        /// <value></value>
        [DataField("Check_Status", IsNullable = true)]
        public bool? CheckStatus {
            get {
                return this.checkStatus;
            }
            set {
                this.checkStatus = value;
            }
        }
        private CheckLevelType? checkLevelType;

        /// <summary>
        /// #EnumType:CheckLevelType#level
        /// </summary>
        /// <value></value>
        [DataField("Check_LevelType", IsNullable = true)]
        public CheckLevelType? CheckLevelType {
            get {
                return this.checkLevelType;
            }
            set {
                this.checkLevelType = value;
            }
        }
        private int loginTimes;

        /// <summary>
        /// LoginTimes
        /// </summary>
        /// <value></value>
        [DataField("LoginTimes")]
        public int LoginTimes {
            get {
                return this.loginTimes;
            }
            set {
                this.loginTimes = value;
            }
        }
        private int mark;

        /// <summary>
        /// Mark
        /// </summary>
        /// <value></value>
        [DataField("Mark")]
        public int Mark {
            get {
                return this.mark;
            }
            set {
                this.mark = value;
            }
        }
        #endregion

        public override string ToString()
        {
            return string.Format("[TeUser: Id={0}, Account={1}, Password={2}, NickName={3}, Gender={4}, Birthday={5}, Telephone={6}, Email={7}, Address={8}, LevelId={9}, RegTime={10}, LastLoginTime={11}, Status={12}, HotRate={13}, Area={14}, DeleteFlag={15}, RefereeId={16}, CheckPoint={17}, CheckStatus={18}, CheckLevelType={19}, LoginTimes={20}, Mark={21}]", Id, Account, Password, NickName, Gender, Birthday, Telephone, Email, Address, LevelId, RegTime, LastLoginTime, Status, HotRate, Area, DeleteFlag, RefereeId, CheckPoint, CheckStatus, CheckLevelType, LoginTimes, Mark);
        }
    }


    [DataTable("Te_UserExtend")]
    public class TeUserExtend
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private int userId;

        /// <summary>
        /// UserId
        /// </summary>
        /// <value></value>
        [DataField("UserId")]
        public int UserId {
            get {
                return this.userId;
            }
            set {
                this.userId = value;
            }
        }
        private string extend1;

        /// <summary>
        /// Extend1
        /// </summary>
        /// <value></value>
        [DataField("Extend1", IsNullable = true)]
        public string Extend1 {
            get {
                return this.extend1;
            }
            set {
                this.extend1 = value;
            }
        }
        private string extend2;

        /// <summary>
        /// Extend2
        /// </summary>
        /// <value></value>
        [DataField("Extend2", IsNullable = true)]
        public string Extend2 {
            get {
                return this.extend2;
            }
            set {
                this.extend2 = value;
            }
        }
        private string extend3;

        /// <summary>
        /// Extend3
        /// </summary>
        /// <value></value>
        [DataField("Extend3", IsNullable = true)]
        public string Extend3 {
            get {
                return this.extend3;
            }
            set {
                this.extend3 = value;
            }
        }
        private int? extendAreaId;

        /// <summary>
        /// ExtendAreaId
        /// </summary>
        /// <value></value>
        [DataField("ExtendAreaId", IsNullable = true)]
        public int? ExtendAreaId {
            get {
                return this.extendAreaId;
            }
            set {
                this.extendAreaId = value;
            }
        }
        #endregion

        public override string ToString()
        {
            return string.Format("[TeUserExtend: Id={0}, UserId={1}, Extend1={2}, Extend2={3}, Extend3={4}, ExtendAreaId={5}]", Id, UserId, Extend1, Extend2, Extend3, ExtendAreaId);
        }
    }


    [DataTable("Te_UserExtendProfile")]
    public class TeUserExtendProfile
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private int userId;

        /// <summary>
        /// UserId
        /// </summary>
        /// <value></value>
        [DataField("UserId")]
        public int UserId {
            get {
                return this.userId;
            }
            set {
                this.userId = value;
            }
        }
        private string extend1;

        /// <summary>
        /// Extend1
        /// </summary>
        /// <value></value>
        [DataField("Extend1", IsNullable = true)]
        public string Extend1 {
            get {
                return this.extend1;
            }
            set {
                this.extend1 = value;
            }
        }
        private string extend2;

        /// <summary>
        /// Extend2
        /// </summary>
        /// <value></value>
        [DataField("Extend2", IsNullable = true)]
        public string Extend2 {
            get {
                return this.extend2;
            }
            set {
                this.extend2 = value;
            }
        }
        private string extend3;

        /// <summary>
        /// Extend3
        /// </summary>
        /// <value></value>
        [DataField("Extend3", IsNullable = true)]
        public string Extend3 {
            get {
                return this.extend3;
            }
            set {
                this.extend3 = value;
            }
        }
        private int extendProfileId;

        /// <summary>
        /// ExtendProfileId
        /// </summary>
        /// <value></value>
        [DataField("ExtendProfileId")]
        public int ExtendProfileId {
            get {
                return this.extendProfileId;
            }
            set {
                this.extendProfileId = value;
            }
        }
        #endregion
    }


    [DataTable("Te_UserLevel")]
    public class TeUserLevel
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        [DataField("Id", IsPrimaryKey = true)]
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private string levelName;

        /// <summary>
        /// LevelName
        /// </summary>
        /// <value></value>
        [DataField("LevelName")]
        public string LevelName {
            get {
                return this.levelName;
            }
            set {
                this.levelName = value;
            }
        }
        private int status;

        /// <summary>
        /// Status
        /// </summary>
        /// <value></value>
        [DataField("Status")]
        public int Status {
            get {
                return this.status;
            }
            set {
                this.status = value;
            }
        }
        private string remark;

        /// <summary>
        /// Remark
        /// </summary>
        /// <value></value>
        [DataField("Remark", IsNullable = true)]
        public string Remark {
            get {
                return this.remark;
            }
            set {
                this.remark = value;
            }
        }
        #endregion
    }



    public class TeUserLevel1
    {
        #region "Data Property"
        private int id;

        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        private string levelName;

        /// <summary>
        /// LevelName
        /// </summary>
        /// <value></value>
        public string LevelName {
            get {
                return this.levelName;
            }
            set {
                this.levelName = value;
            }
        }
        private int status;

        /// <summary>
        /// Status
        /// </summary>
        /// <value></value>
        public int Status {
            get {
                return this.status;
            }
            set {
                this.status = value;
            }
        }
        private string remark;

        /// <summary>
        /// Remark
        /// </summary>
        /// <value></value>
        public string Remark {
            get {
                return this.remark;
            }
            set {
                this.remark = value;
            }
        }



        public LCollection<TeUser> Users {
            get;
            set;
        }

        #endregion
    }

}
