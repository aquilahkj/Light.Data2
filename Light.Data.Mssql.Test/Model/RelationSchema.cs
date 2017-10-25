using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data.Mssql.Test
{
    public class TeRelateA_B : TeRelateA
    {
        [RelationField("Id", "RelateAId")]
        public TeRelateB RelateB {
            get;
            set;
        }
    }

    public class TeRelateA_BC : TeRelateA
    {
        [RelationField("Id", "RelateAId")]
        public TeRelateB RelateB {
            get;
            set;
        }

        [RelationField("Id", "RelateAId")]
        public TeRelateC RelateC {
            get;
            set;
        }
    }

    public class TeRelateA_LCollection : TeRelateA
    {
        [RelationField("Id", "RelateAId")]
        public LCollection<TeRelateCollection> RelateCollection {
            get;
            set;
        }
    }

    public class TeRelateA_ICollection : TeRelateA
    {
        [RelationField("Id", "RelateAId")]
        public ICollection<TeRelateCollection> RelateCollection {
            get;
            set;
        }
    }

    public class TeRelateA_2Collection : TeRelateA
    {

        [RelationField("Id", "RelateAId")]
        public LCollection<TeRelateCollection> RelateLCollection {
            get;
            set;
        }

        [RelationField("Id", "RelateAId")]
        public ICollection<TeRelateCollection> RelateICollection {
            get;
            set;
        }
    }

    public class TeRelateA_B_Collection : TeRelateA
    {
        [RelationField("Id", "RelateAId")]
        public TeRelateB RelateB {
            get;
            set;
        }

        [RelationField("Id", "RelateAId")]
        public ICollection<TeRelateCollection> RelateCollection {
            get;
            set;
        }
    }

    public class TeRelateA_BX : TeRelateA
    {
        [RelationField("Id", "RelateAId")]
        public TeRelateB_CD RelateB {
            get;
            set;
        }

        [RelationField("Id", "RelateAId")]
        public TeRelateE_A RelateE {
            get;
            set;
        }
    }

    public class TeRelateB_CD : TeRelateB
    {
        [RelationField("RelateAId", "RelateAId")]
        public TeRelateC_A RelateC {
            get;
            set;
        }

        [RelationField("Id", "RelateBId")]
        public TeRelateD RelateD {
            get;
            set;
        }

        [RelationField("RelateAId", "RelateAId")]
        public TeRelateE_A RelateE {
            get;
            set;
        }
    }

    public class TeRelateC_A : TeRelateC
    {
        [RelationField("RelateAId", "Id")]
        public TeRelateA_BX RelateA {
            get;
            set;
        }
    }
    
    public class TeRelateE_A : TeRelateE
    {
        [RelationField("RelateAId", "Id")]
        public TeRelateA_BX RelateA {
            get;
            set;
        }
    }


    public class TeRelateA_B_A : TeRelateA
    {
        [RelationField("Id", "RelateAId")]
        public TeRelateB_A_B RelateB {
            get;
            set;
        }
    }

    public class TeRelateB_A_B : TeRelateB
    {
        [RelationField("RelateAId", "Id")]
        public TeRelateA_B_A RelateA {
            get;
            set;
        }
    }
}
