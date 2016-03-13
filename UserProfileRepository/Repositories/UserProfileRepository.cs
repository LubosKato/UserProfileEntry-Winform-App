using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UserProfileDomain;
using UserProfileRepository.Extensions;

namespace UserProfileRepository.Repositories
{
    public class UserProfileRepository : Repository<UserProfile>
    {
        private readonly DbContext _context;
        public UserProfileRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public IList<UserProfile> GetAll()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = "Select * from [assignment].[dbo].[UserProfile] where UserProfileStatus = 0 order by UserProfileId desc";
                return this.ToList(command).ToList();
            }
        }

        public UserProfile GetUserProfileById(int userProfileId)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = "Select top 1 * from [assignment].[dbo].[UserProfile] where UserProfileStatus = 0 AND UserProfileId =@userProfileId";
                command.AddParameter("userProfileId", userProfileId);
                return (UserProfile)command.ExecuteScalar();
            }
        }

        public int GetNewUserProfileId()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = "Select MAX(UserProfileId) from [assignment].[dbo].[UserProfile]";
                return (int)command.ExecuteScalar();
            }
        }

        public void Create(UserProfile userProfile)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = @"INSERT INTO[assignment].[dbo].[UserProfile] 
                (UserProfileId, UserProfileStatus, UserProfileAccount, UserProfileName, UserProfileDomainName, UserProfileMailAddress, UserProfileUserLevelToUserAdmin, UserProfileOperatorId, UserProfileTimeStamp) VALUES                                         
                (@userProfileId, @userProfileStatus, @userProfileAccount, @userProfileName, @userProfileDomainName, @userProfileMailAddress, @UserProfileUserLevelToUserAdmin, @userProfileOperatorId, @userProfileTimeStamp)";
                command.AddParameter("userProfileId", userProfile.UserProfileId);
                command.AddParameter("userProfileAccount", userProfile.UserProfileAccount);
                command.AddParameter("userProfileStatus", userProfile.UserProfileStatus);
                command.AddParameter("userProfileName", userProfile.UserProfileName);
                command.AddParameter("userProfileDomainName", userProfile.UserProfileDomainName);
                command.AddParameter("userProfileMailAddress", userProfile.UserProfileMailAddress);
                command.AddParameter("UserProfileUserLevelToUserAdmin", userProfile.UserProfileUserLevelToUserAdmin ? "Y" : "N");
                command.AddParameter("UserProfileOperatorId", userProfile.UserProfileOperatorId);
                command.AddParameter("userProfileTimeStamp", DateTime.Now);
                command.ExecuteNonQuery();
            }
        }

        public void Update(UserProfile userProfile)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = @"UPDATE UserProfile SET 
                                        UserProfileOperatorId = @userProfileOperatorId, 
                                        UserProfileName = @userProfileName, 
                                        UserProfileDomainName = @userProfileDomainName,
                                        UserProfileMailAddress = @userProfileMailAddress,
                                        UserProfileUserLevelToUserAdmin = @UserProfileUserLevelToUserAdmin,
                                        UserProfileTimeStamp = @userProfileTimeStamp
                                        WHERE UserProfileId = @userProfileId";
                command.AddParameter("userProfileOperatorId", userProfile.UserProfileOperatorId);
                command.AddParameter("userProfileId", userProfile.UserProfileId);
                command.AddParameter("userProfileName", userProfile.UserProfileName);
                command.AddParameter("userProfileDomainName", userProfile.UserProfileDomainName);
                command.AddParameter("userProfileMailAddress", userProfile.UserProfileMailAddress);
                command.AddParameter("UserProfileUserLevelToUserAdmin", userProfile.UserProfileUserLevelToUserAdmin ? "Y" : "N");
                command.AddParameter("userProfileTimeStamp", DateTime.Now);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int userProfileId)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = @"update [assignment].[dbo].[UserProfile] set UserProfileStatus = -1 WHERE UserProfileId = @userId";
                command.AddParameter("userId", userProfileId);
                command.ExecuteNonQuery();
            }
        }

        protected override void Map(IDataRecord record, UserProfile user)
        {
            user.UserProfileAccount = (string)record["UserProfileAccount"];
            user.UserProfileDomainName = (string)record["UserProfileDomainName"];
            user.UserProfileMailAddress = (string)record["UserProfileMailAddress"];
            user.UserProfileName = (string)record["UserProfileName"];
            user.UserProfileOperatorId = (int)record["UserProfileOperatorId"];
            user.UserProfileStatus = (int)record["UserProfileStatus"];
            user.UserProfileTimeStamp = (DateTime)record["UserProfileTimeStamp"];
            user.UserProfileUserLevelToUserAdmin = (string)record["UserProfileUserLevelToUserAdmin"] == "Y";
            user.UserProfileId = (int)record["UserProfileId"];
        }
    }
}
