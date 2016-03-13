using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using UserProfileDomain;
using UserProfileRepository.Extensions;

namespace UserProfileRepository.Repositories
{
    public class UserAccessRepository : Repository<UserAccess>
    {
        private readonly DbContext _context;

        public UserAccessRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public int GetLatestUserAccessId()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = "Select top 1 UserAccessId from [assignment].[dbo].[UserAccess] order by UserAccessId desc";
                return (int)command.ExecuteScalar();
            }
        }

        public List<UserAccess> GetAccessByUserProfileId(int userProfileId)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = "Select * from [assignment].[dbo].[UserAccess] where UserAccessStatus = 0 AND [UserAccessUserProfileId] = @userProfileId";
                command.AddParameter("userProfileId", userProfileId);
                return this.ToList(command).ToList();
            }
        }

        public void Create(int userAccessId, int userAccessStatus, int userAccessUserProfileId, int userAccessLocalSystemId, int userAccessUserLevelCategoryId)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = @"INSERT INTO [assignment].[dbo].[UserAccess]
                (UserAccessId, UserAccessStatus, UserAccessUserProfileId, UserAccessLocalSystemId, UserAccessUserLevelCategoryId) VALUES                                         
                (@userAccessId, @userAccessStatus, @userAccessUserProfileId, @userAccessLocalSystemId, @userAccessUserLevelCategoryId)";
                command.AddParameter("userAccessId", userAccessId);
                command.AddParameter("userAccessStatus", userAccessStatus);
                command.AddParameter("userAccessUserProfileId", userAccessUserProfileId);
                command.AddParameter("userAccessLocalSystemId", userAccessLocalSystemId);
                command.AddParameter("userAccessUserLevelCategoryId", userAccessUserLevelCategoryId);
                command.ExecuteNonQuery();
            }   
        }

        public void Update(int userAccessStatus, int userAccessUserProfileId, int userAccessLocalSystemId, int userAccessUserLevelCategoryId)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = @"UPDATE [assignment].[dbo].[UserAccess] SET 
                                        userAccessStatus = @userAccessStatus, 
                                        userAccessUserLevelCategoryId = @userAccessUserLevelCategoryId
                                        WHERE UserAccessUserProfileId = @userAccessUserProfileId 
                                        AND UserAccessLocalSystemId = @userAccessLocalSystemId";
                command.AddParameter("userAccessStatus", userAccessStatus);
                command.AddParameter("userAccessUserProfileId", userAccessUserProfileId);
                command.AddParameter("userAccessLocalSystemId", userAccessLocalSystemId);
                command.AddParameter("userAccessUserLevelCategoryId", userAccessUserLevelCategoryId);
                command.ExecuteNonQuery();
            }
        }

        protected override void Map(IDataRecord record, UserAccess system)
        {
            system.UserAccessId = (int)record["UserAccessId"];
            system.SystemId = (int)record["UserAccessLocalSystemId"];
            system.Status = (int)record["UserAccessStatus"];
            system.UserProfileOperatorId = (int)record["UserAccessUserProfileId"];
            system.CategoryId = (int)record["UserAccessUserLevelCategoryId"];
        }
    }
}