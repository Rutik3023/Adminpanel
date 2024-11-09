namespace  AdminPanel.Models
{
    public class VMGet_Menu_ByRoleID
    {
        public string? Code { get; set; }
        public string? MenuName { get; set; }

        public bool? IsAssigned { get; set; }
        public bool? IsEditView { get; set; }
    }
}
