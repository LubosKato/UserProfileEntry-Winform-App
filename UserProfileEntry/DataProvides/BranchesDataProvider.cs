using System.Collections.Generic;
using UserProfileDomain;
using UserProfileEntry.DataProvides.Interfaces;
using UserProfileRepository;

namespace UserProfileEntry.DataProvides
{
    public class BranchesDataProvider : IBranchesDataProvider
    {
        private readonly DbContext _context;
        public BranchesDataProvider()
        {
            var connectionFactory = ConnectionHelper.GetConnection();
            _context = new DbContext(connectionFactory);
        }

        public List<Branch> GetBranches()
        {
            var userRep = new UserProfileRepository.Repositories.BranchRepository(_context);
            return userRep.GetAll();
        }

        public int GetLatestLocalSystemBranchId()
        {
            var userRep = new UserProfileRepository.Repositories.LocalSystemBranchRepository(_context);
            return userRep.GetLatestLocalSystemBranchId();
        }

        public List<LocalSystemBranch> GetSystemBranches(int userProfileId)
        {
            var userRep = new UserProfileRepository.Repositories.LocalSystemBranchRepository(_context);
            return userRep.GetByUserProfileId(userProfileId);
        }

        public void LocalSystemUpdate(int branchStatus, int userProfileId, int localSystemId, string branchName)
        {
            var userRep = new UserProfileRepository.Repositories.LocalSystemBranchRepository(_context);
            userRep.Update(branchStatus, userProfileId, localSystemId, branchName);
        }

        public void LocalSystemCreate(int localSystemBranchId, int branchStatus, int userProfileId, int localSystemId, string branchName)
        {
            var userRep = new UserProfileRepository.Repositories.LocalSystemBranchRepository(_context);
            userRep.Create(localSystemBranchId, branchStatus, userProfileId, localSystemId, branchName);
        }
    }
}