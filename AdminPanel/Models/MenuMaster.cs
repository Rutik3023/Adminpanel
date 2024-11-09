namespace  AdminPanel.Models
{
    public class MenuMaster
    {
        //M
        public string Code { get; set; }
        public string[] OrgCode { get; set; }


        public string OrgCodeAsString { get; set; }
        public string MenuName { get; set; }
        public string MenuDesc { get; set; }
        public string? ParentId { get; set; }
        public string MenuUrl { get; set; }
        public string? MenuIcon { get; set; }
        public string? MenuSrNo { get; set; }
        public string Org_Code { get; set; }
        public string Organization_Name { get; set; }
        public string ORG_Menu_MapCode { get; set; }

        public bool Assigned { get; set; }
        public string Type { get; set; }
        public string status { get; set; }
        public string CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public string? LastModificationDate { get; set; }
        public int? LastModifiedBy { get; set; }

        public List<MenuMaster> Orglist { get; set; }


        public MenuMaster()
        {

            Orglist = new List<MenuMaster>();
        }


    }

}
