using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UserProfileDomain;
using UserProfileRepository.Extensions;

namespace UserProfileRepository.Repositories
{
    public class LocalSystemBranchRepository : Repository<LocalSystemBranch>
    {
        private readonly DbContext _context;

        public LocalSystemBranchRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public int GetLatestLocalSystemBranchId()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = "Select top 1 LocalSystemBranchId from [assignment].[dbo].[LocalSystemBranch] order by LocalSystemBranchId desc";
                return (int)command.ExecuteScalar();
            }
        }

        public List<LocalSystemBranch> GetByUserProfileId(int userProfileId)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = "Select * from [assignment].[dbo].[LocalSystemBranch] where LocalSystemBranchStatus = 0 AND LocalSystemBranchUserProfileId = @userProfileId";
                command.AddParameter("userProfileId", userProfileId);
                return this.ToList(command).ToList();
            }
        }

        public void Create(int localSystemBranchId, int branchStatus, int userProfileId, int localSystemId, string branchCode)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = @"INSERT INTO [assignment].[dbo].[LocalSystemBranch]
                (LocalSystemBranchId, LocalSystemBranchStatus, LocalSystemBranchUserProfileId, LocalSystemBranchLocalSystemId, LocalSystemBranchCode) VALUES                                         
                (@LocalSystemBranchId, @localSystemBranchStatus, @localSystemBranchUserProfileId, @localSystemBranchLocalSystemId, @localSystemBranchCode)";
                command.AddParameter("LocalSystemBranchId", localSystemBranchId);
                command.AddParameter("localSystemBranchStatus", branchStatus);
                command.AddParameter("localSystemBranchUserProfileId", userProfileId);
                command.AddParameter("localSystemBranchLocalSystemId", localSystemId);
                command.AddParameter("localSystemBranchCode", branchCode);
                command.ExecuteNonQuery();
            }
        }

        public void Update(int branchStatus, int userProfileId, int localSystemId, string branchCode)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = @"UPDATE [assignment].[dbo].[LocalSystemBranch] SET 
                                        LocalSystemBranchStatus = @localSystemBranchStatus
                                        WHERE LocalSystemBranchUserProfileId = @localSystemBranchUserProfileId 
                                        AND LocalSystemBranchLocalSystemId = @localSystemBranchLocalSystemId 
                                        AND LocalSystemBranchCode = @localSystemBranchCode";
                command.AddParameter("localSystemBranchStatus", branchStatus);
                command.AddParameter("localSystemBranchUserProfileId", userProfileId);
                command.AddParameter("localSystemBranchLocalSystemId", localSystemId);
                command.AddParameter("localSystemBranchCode", branchCode);
                command.ExecuteNonQuery();
            }
        }

        protected override void Map(IDataRecord record, LocalSystemBranch system)
        {
            system.LocalSystemBranchId = (int) record["LocalSystemBranchId"];
            system.BranchUserProfileId = (int)record["LocalSystemBranchUserProfileId"];
            system.Status = (int)record["LocalSystemBranchStatus"];
            system.SystemId = (int)record["LocalSystemBranchLocalSystemId"];
            system.BranchCode = (string)record["LocalSystemBranchCode"];
        }
    }
}