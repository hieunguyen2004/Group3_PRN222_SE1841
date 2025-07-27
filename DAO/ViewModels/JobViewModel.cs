namespace TopCVWeb.ViewModels
{
    public class JobViewModel
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string LogoCompany { get; set; }
        public string Salary { get; set; }
        public string Location { get; set; }
        public int NumberOfSeeker { get; set; }
        public bool IsSaved { get; set; }
    }
}
