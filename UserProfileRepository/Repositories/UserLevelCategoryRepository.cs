using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UserProfileDomain;

namespace UserProfileRepository.Repositories
{
    public class UserLevelCategoryRepository : Repository<UserLevelCategory>
    {
        private readonly DbContext _context;

        public UserLevelCategoryRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public List<UserLevelCategory> GetAll()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = "Select * from [assignment].[dbo].[UserLevelCategory]";
                return this.ToList(command).ToList();
            }
        }

        protected override void Map(IDataRecord record, UserLevelCategory system)
        {
            system.UserLevelCategoryId = (int)record["UserLevelCategoryId"];
            system.UserLevelCategoryLocalSystemUd = (int)record["UserLevelCategoryLocalSystemUd"];
            system.UserLevelCategoryName = (record["UserLevelCategoryName"] == DBNull.Value) ? string.Empty : record["UserLevelCategoryName"].ToString();
        }
    }
}