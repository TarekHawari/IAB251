namespace IAB251InterportCargoAssignment2Grp21.BusinessLogic.Entities
{
    public class EmployeeLoginResult
    {
        public bool IsSuccessful { get; set; }
        public string EmployeeEmail { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
