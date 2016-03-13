using System.Collections.Generic;
using UserProfileDomain;

namespace UserProfileEntry.DataProvides.Interfaces
{
    public interface IPermissionsDataProvider
    {
        List<UserAccess> GetUserAccess(int userProfileId);
        List<UserLevelCategory> GetCategories();
        IList<LocalSystem> GetSystems();
        void UserAccessUpdate(int i, int userProfileId, int localSystemId, int changedPermissionId);
        void UserAccessCreate(int i, int i1, int userProfileId, int localSystemId, int changedPermissionId);
        int GetLatestUserAccessId();
    }
}