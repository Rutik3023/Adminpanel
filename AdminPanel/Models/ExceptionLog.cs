namespace  AdminPanel.Models
{
    public class ExceptionLog
    {
        public string Code { get; set; }
        public string OrgCode { get; set; }
        public string UserCode { get; set; }
        public string MachineName { get; set; }
        public string Url { get; set; }
        public string FormName { get; set; }
        public string MethodName { get; set; }
        public string ExceptionDetails { get; set; }
        public string LogDate { get; set; }
        public string IsResolved { get; set; }
        public string ResolvedBy { get; set; }
        public string ResolvedDate { get; set; }
        public string ResolutionComment { get; set; }
    }
}
