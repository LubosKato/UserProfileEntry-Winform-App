using System.Collections.Generic;
using UserProfileDomain;

namespace UserProfileEntry.DataProvides.Interfaces
{
    public interface IBranchesDataProvider
    {
        List<Branch> GetBranches();
        int GetLatestLocalSystemBranchId();
        List<LocalSystemBranch> GetSystemBranches(int userProfileId);
        void LocalSystemUpdate(int branchStatus, int userProfileId, int localSystemId, string branchCode);
        void LocalSystemCreate(int localSystemBranchId, int branchStatus, int userProfileId, int localSystemId, string branchCode);
    }
}