using System;
using System.Collections.Generic;
using UserProfileDomain;
using UserProfileEntry.DataProvides.Interfaces;
using UserProfileRepository;

namespace UserProfileEntry.DataProvides
{
    public class UserProfileUserProfileDataProvider : IUserProfileDataProvider
    {
        private readonly DbContext _context;
        public UserProfileUserProfileDataProvider()
        {
            var connectionFactory = ConnectionHelper.GetConnection();
            _context = new DbContext(connectionFactory);
        }

        public IList<UserProfile> GetUserProfiles()
        {
            var userRep = new UserProfileRepository.Repositories.UserProfileRepository(_context);
            return userRep.GetAll();
        }

        public void CreateUserProfile(UserProfile userProfile)
        {
            var userRep = new UserProfileRepository.Repositories.UserProfileRepository(_context);
            userRep.Create(userProfile);
        }

        public void UpdateUserProfile(UserProfile userProfile)
        {
            var userRep = new UserProfileRepository.Repositories.UserProfileRepository(_context);
            userRep.Update(userProfile);
        }

        public int GetNewUserProfileId()
        {
            var userRep = new UserProfileRepository.Repositories.UserProfileRepository(_context);
            return userRep.GetNewUserProfileId();
        }

        public UserProfile GetUserProfileById(int userProfileId)
        {
            var userRep = new UserProfileRepository.Repositories.UserProfileRepository(_context);
            return userRep.GetUserProfileById(userProfileId);
        }

        public void DeleteUserProfile(int userProfileId)
        {
            var userRep = new UserProfileRepository.Repositories.UserProfileRepository(_context);
            userRep.Delete(userProfileId);
        }
    }
}