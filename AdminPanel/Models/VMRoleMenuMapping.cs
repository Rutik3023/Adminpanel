namespace  AdminPanel.Models
{
    public class VMRoleMenuMapping
    {
        public string DropdownValue { get; set; }
        public List<GridData> GridData { get; set; }
    }

    public class GridData
    {
        public string Code { get; set; }
        public string MenuName { get; set; }
        public bool IsAssigned { get; set; }
        public bool IsEditView { get; set; }
    }
}
