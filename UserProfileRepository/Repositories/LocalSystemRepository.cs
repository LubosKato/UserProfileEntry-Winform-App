using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UserProfileDomain;

namespace UserProfileRepository.Repositories
{
    public class LocalSystemRepository : Repository<LocalSystem>
    {
        private readonly DbContext _context;

        public LocalSystemRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public List<LocalSystem> GetAll()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = "Select * from [assignment].[dbo].[LocalSystem]";
                return this.ToList(command).ToList();
            }
        } 

        protected override void Map(IDataRecord record, LocalSystem system)
        {
            system.LocalSystemId = (int)record["LocalSystemId"];
            system.LocalSystemName = record["LocalSystemName"] == DBNull.Value ? string.Empty : record["LocalSystemName"].ToString();
        }
    }
}