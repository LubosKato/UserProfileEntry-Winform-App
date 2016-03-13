using System.Collections.Generic;

namespace UserProfileDomain
{
    public class UserAccess
    {
        public int UserAccessId { get; set; }
        public int UserProfileOperatorId { get; set; }
        public int SystemId { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
    }
}