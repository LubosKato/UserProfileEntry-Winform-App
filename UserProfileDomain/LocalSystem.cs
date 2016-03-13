using System.Collections.Generic;
using System.Linq;

namespace UserProfileDomain
{
    public class LocalSystem
    {
        public int LocalSystemId { get; set; }
        public string LocalSystemName { get; set; }
        public List<BranchSelected> Branches { get; set; }
        public List<Permission> Permissions { get; set; }
        public bool Changed { get; set; }

        public LocalSystem()
        {
            Permissions = new List<Permission>();
            Branches = new List<BranchSelected>();
        }

        public void PopulatePermissions(List<UserAccess> userAccesses, List<UserLevelCategory> categories)
        {
            var categoryId = userAccesses.Where(c => c.SystemId == LocalSystemId).Select(c => c.CategoryId).FirstOrDefault();
            foreach (var permission in categories.Where(c => c.UserLevelCategoryLocalSystemUd == LocalSystemId)
                        .Select(c => new Permission { PermissionId = c.UserLevelCategoryId, PermissionName = c.UserLevelCategoryName }))
            {
                if (permission.PermissionId == categoryId)
                { 
                    permission.Selected = true;
                }

                this.Permissions.Add(permission);
            }
        }

        public void PopulateBranches(List<Branch> branches, List<LocalSystemBranch> localSystemBranches)
        {
            var selectedBranchCode = localSystemBranches.Where(c => c.SystemId == LocalSystemId).Select(c => c.BranchCode).FirstOrDefault();
            foreach (var branch in branches)
            {
                if (selectedBranchCode == branch.BranchCode)
                {
                    this.Branches.Add(new BranchSelected { BranchCode = branch.BranchCode, Selected = true });
                    continue;
                }
                this.Branches.Add(new BranchSelected {BranchCode = branch.BranchCode, Selected = false });
            }
        }
    }
}