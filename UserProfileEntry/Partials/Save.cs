using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;
using UserProfileDomain;

namespace UserProfileEntry
{
    public partial class UserProfileEntryForm : Form
    {
        private void SaveUserProfile()
        {
            if (_noActiveUser)
            {
                var newUserProfile = new UserProfile
                {
                    UserProfileId = _userProfileDataProvider.GetNewUserProfileId() + 1,
                    UserProfileAccount = new MailAddress(txtEmail.Text).User,
                    UserProfileDomainName = txtDomain.Text,
                    UserProfileMailAddress = txtEmail.Text,
                    UserProfileName = txtUserName.Text,
                    UserProfileTimeStamp = DateTime.Now,
                    UserProfileStatus = 0,
                    UserProfileOperatorId = Convert.ToInt32(txtUserProfileId.Text),
                    UserProfileUserLevelToUserAdmin = chckAdmin.Checked
                };
                _userProfileDataProvider.CreateUserProfile(newUserProfile);
                _currentUserProfile = newUserProfile;
                PopulateDefaultUser();
            }
            else
            {
                _currentUserProfile.UserProfileDomainName = txtDomain.Text;
                _currentUserProfile.UserProfileUserLevelToUserAdmin = chckAdmin.Checked;
                _currentUserProfile.UserProfileName = txtUserName.Text;
                _currentUserProfile.UserProfileMailAddress = txtEmail.Text;
                _currentUserProfile.UserProfileOperatorId = Convert.ToInt32(txtUserProfileId.Text);
                _userProfileDataProvider.UpdateUserProfile(_currentUserProfile);
            }
        }

        private void SaveSystems(List<LocalSystem> systems, int userProfileOperatorId)
        {
            SaveBranches(systems, userProfileOperatorId);
            SavePermissions(systems, userProfileOperatorId);
        }

        private void SaveBranches(List<LocalSystem> systems, int userProfileOperatorId)
        {
            var localSystemBranch = _branchesDataProvider.GetSystemBranches(userProfileOperatorId);
            foreach (var localSystem in systems.Where(c => c.Changed))
            {
                foreach (var branch in localSystem.Branches.Where(c => c.Changed).Select(c => new { c.BranchCode, c.Selected }))
                {
                    //var branch = localSystem.Branches.Where(c => c.Changed).Select(c => new  { c.BranchCode, c.Selected}).FirstOrDefault();
                    if (branch != null)
                    {
                        if (branch.BranchCode ==
                            localSystemBranch.Where(c => c.SystemId == localSystem.LocalSystemId)
                                .Select(c => c.BranchCode)
                                .FirstOrDefault())
                        {
                            if (branch.Selected)
                                _branchesDataProvider.LocalSystemUpdate(0, userProfileOperatorId,
                                    localSystem.LocalSystemId, branch.BranchCode);
                            else
                            {
                                _branchesDataProvider.LocalSystemUpdate(-1, userProfileOperatorId,
                                    localSystem.LocalSystemId, branch.BranchCode);
                            }
                        }
                        else if (branch.Selected)
                        {
                            var newLocalSystemBranchId = _branchesDataProvider.GetLatestLocalSystemBranchId();
                            _branchesDataProvider.LocalSystemCreate(newLocalSystemBranchId + 1, 0, userProfileOperatorId,
                                localSystem.LocalSystemId, branch.BranchCode);
                        }
                    }
                }
            }
        }

        private void SavePermissions(List<LocalSystem> systems, int userProfileId)
        {
            var userAccess = _permissionsDataProvider.GetUserAccess(userProfileId);
            foreach (var localSystem in systems.Where(c => c.Changed))
            {
                var changedPermissionId =
                    localSystem.Permissions.Where(c => c.Selected).Select(c => c.PermissionId).FirstOrDefault();
                if (userAccess.Any(c => c.SystemId == localSystem.LocalSystemId))
                {
                    _permissionsDataProvider.UserAccessUpdate(0, userProfileId, localSystem.LocalSystemId, changedPermissionId);
                }
                else
                {
                    var newUserAccessId = _permissionsDataProvider.GetLatestUserAccessId();
                    _permissionsDataProvider.UserAccessCreate(newUserAccessId + 1, 0, userProfileId, localSystem.LocalSystemId,
                        changedPermissionId);
                }
            }
        }
    }
}