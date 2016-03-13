using System.Collections.Generic;
using UserProfileDomain;

namespace UserProfileEntry.DataProvides.Interfaces
{
    public interface IUserProfileDataProvider
    {
        IList<UserProfile> GetUserProfiles();

        void DeleteUserProfile(int userProfileId);
        void CreateUserProfile(UserProfile userProfile);
        void UpdateUserProfile(UserProfile userProfile);
        int GetNewUserProfileId();
        UserProfile GetUserProfileById(int toInt32);
    }
}