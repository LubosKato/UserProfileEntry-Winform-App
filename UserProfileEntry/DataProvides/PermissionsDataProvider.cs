using System.Collections.Generic;
using UserProfileDomain;
using UserProfileEntry.DataProvides.Interfaces;
using UserProfileRepository;

namespace UserProfileEntry.DataProvides
{
    public class PermissionsDataProvider : IPermissionsDataProvider
    {
        private readonly DbContext _context;
        public PermissionsDataProvider()
        {
            var connectionFactory = ConnectionHelper.GetConnection();
            _context = new DbContext(connectionFactory);
        }

        public List<UserAccess> GetUserAccess(int userProfileId)
        {
            var userRep = new UserProfileRepository.Repositories.UserAccessRepository(_context);
            return userRep.GetAccessByUserProfileId(userProfileId);
        }
        public IList<LocalSystem> GetSystems()
        {
            var userRep = new UserProfileRepository.Repositories.LocalSystemRepository(_context);
            return userRep.GetAll();
        }

        public void UserAccessUpdate(int userAccessStatus, int userAccessUserProfileId, int userAccessLocalSystemId, int userAccessUserLevelCategoryId)
        {
            var userRep = new UserProfileRepository.Repositories.UserAccessRepository(_context);
            userRep.Update(userAccessStatus, userAccessUserProfileId, userAccessLocalSystemId, userAccessUserLevelCategoryId);
        }

        public void UserAccessCreate(int userAccessId, int userAccessStatus, int userAccessUserProfileId, int userAccessLocalSystemId, int userAccessUserLevelCategoryId)
        {
            var userRep = new UserProfileRepository.Repositories.UserAccessRepository(_context);
            userRep.Create(userAccessId, userAccessStatus, userAccessUserProfileId, userAccessLocalSystemId, userAccessUserLevelCategoryId);
        }

        public int GetLatestUserAccessId()
        {
            var userRep = new UserProfileRepository.Repositories.UserAccessRepository(_context);
            return userRep.GetLatestUserAccessId();
        }

        public List<UserLevelCategory> GetCategories()
        {
            var userRep = new UserProfileRepository.Repositories.UserLevelCategoryRepository(_context);
            return userRep.GetAll();
        }
    }
}