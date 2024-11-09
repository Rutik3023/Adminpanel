namespace  AdminPanel.Models
{
    public class MenuItem
    {
        public int Code { get; set; }
        public string MenuName { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int? ParentId { get; set; }
        public string MenuUrl { get; set; }
        public int MenuSrNo { get; set; }
        public string MenuDesc { get; set; }
        public string MenuIcon { get; set; }
        public List<MenuItem> Children { get; set; } = new List<MenuItem>();
    }
}
