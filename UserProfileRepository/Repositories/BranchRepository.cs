using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UserProfileDomain;

namespace UserProfileRepository.Repositories
{
    public class BranchRepository : Repository<Branch>
    {
        private readonly DbContext _context;

        public BranchRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public List<Branch> GetAll()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = "Select * from [assignment].[dbo].[Branch]";
                return this.ToList(command).ToList();
            }
        }

        protected override void Map(IDataRecord record, Branch system)
        {
            system.BranchCode = record["BranchCode"] == DBNull.Value ? string.Empty : record["BranchCode"].ToString();
            system.BranchName = record["BranchName"] == DBNull.Value ? string.Empty : record["BranchName"].ToString();
        }
    }
}