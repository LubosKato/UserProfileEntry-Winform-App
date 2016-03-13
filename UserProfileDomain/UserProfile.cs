using System;

namespace UserProfileDomain
{
    public class UserProfile
    {
        public int UserProfileId { get; set; }
        public int UserProfileStatus { get; set; }
        public string UserProfileAccount { get; set; }
        public string UserProfileDomainName { get; set; }
        public string UserProfileName { get; set; }
        public string UserProfileMailAddress { get; set; }
        public bool UserProfileUserLevelToUserAdmin { get; set; }
        public int UserProfileOperatorId { get; set; }
        public DateTime UserProfileTimeStamp { get; set; }
    }
}
