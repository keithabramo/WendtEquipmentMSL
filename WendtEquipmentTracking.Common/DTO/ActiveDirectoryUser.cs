namespace WendtEquipmentTracking.Common.DTO
{
    public class ActiveDirectoryUser
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public UserRoles Role { get; set; }

        public string DisplayName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
