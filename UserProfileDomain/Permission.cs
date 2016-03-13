namespace UserProfileDomain
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public bool Selected { get; set; }
        public bool Changed { get; set; }
    }
}