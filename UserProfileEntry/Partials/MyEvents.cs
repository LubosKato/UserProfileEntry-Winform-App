using System;
using System.Windows.Forms;

namespace UserProfileEntry
{
    public partial class UserProfileEntryForm
    {
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PanelsEnabled();
            btnEdit.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            PanelsDisabled();
            btnEdit.Enabled = true;
            RefreshControls();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _userProfileDataProvider.DeleteUserProfile(_currentUserProfile.UserProfileId);
            ClearAndEnableControls();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int userProfileId;
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Check Email Format!");
            }
            if (txtUserProfileId.Text == string.Empty || !Int32.TryParse(txtUserProfileId.Text, out userProfileId))
            {
                MessageBox.Show("User Profile Id must be number and is required field");
            }
            else
            {
                btnEdit.Enabled = true;
                SaveUserProfile();
                foreach (var localSystem in _localSystems)
                {
                    SaveSystems(localSystem.Value, localSystem.Key);
                }
                IsAdmin();
                ResetChangedProperties();
            }
        }

        private void txtUserId_TextChanged(object sender, EventArgs e)
        {
            if (txtUserProfileId.Text != null && _currentUserProfile != null)
            {
                int operatorId;
                if (Int32.TryParse(txtUserProfileId.Text, out operatorId))
                {
                    _currentUserProfile.UserProfileOperatorId = operatorId;
                    GenerateSystems();
                    RefreshControls();
                }
            }
            else
            {
                btnSave.Enabled = true;
            }
        }

        private void BranchesChanged(object sender, EventArgs e)
        {
            try
            {
                //var sytem = _localSystems[_currentUserProfile.UserProfileOperatorId];
                //btnSave.Enabled = true;
                CheckBox checkBox = (CheckBox)sender;
                var name = checkBox.Name;
                string[] values = name.Split(',');
                var column = Convert.ToInt32(values[1]);
                var row = Convert.ToInt32(values[0]);
                _localSystems[_currentUserProfile.UserProfileOperatorId][row - 1].Branches[column - 1].Selected = checkBox.Checked;
                _localSystems[_currentUserProfile.UserProfileOperatorId][row - 1].Branches[column - 1].Changed = true;
                _localSystems[_currentUserProfile.UserProfileOperatorId][row - 1].Changed = true;
                //PopulateDefaultUser();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ComboBoxChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox objLabel = (ComboBox)sender;
                var name = objLabel.Name;
                string[] values = name.Split(',');
                var row = Convert.ToInt32(values[0]);
                foreach (var permission in _localSystems[_currentUserProfile.UserProfileOperatorId][row - 1].Permissions)
                {
                    permission.Selected = false;
                }
                // set just one to selected
                if (objLabel.SelectedIndex >= 0)
                {
                    _localSystems[_currentUserProfile.UserProfileOperatorId][row - 1].Permissions[objLabel.SelectedIndex].Selected = true;
                    _localSystems[_currentUserProfile.UserProfileOperatorId][row - 1].Permissions[objLabel.SelectedIndex].Changed = true;
                    _localSystems[_currentUserProfile.UserProfileOperatorId][row - 1].Changed = true;
                    //PopulateDefaultUser();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}