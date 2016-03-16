using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;

namespace UserProfileEntry
{
    public partial class UserProfileEntryForm
    {
        private void IsAdmin()
        {
            btnEdit.Enabled = _currentUserProfile.UserProfileUserLevelToUserAdmin;
            PanelsDisabled();
        }

        private void ClearAndEnableControls()
        {
            _noActiveUser = true;
            txtUserProfileId.Text = string.Empty;
            txtUserProfileId.Text = string.Empty;
            txtDomain.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            chckAdmin.Checked = false;
            pnlControls.Enabled = true;
            ClearGeneratedControls();
        }

        private void ClearGeneratedControls()
        {
            if (_localSystems != null && _localSystems.Count > 0)
            {
                foreach (var localSystem in _localSystems[_currentUserProfile.UserProfileOperatorId])
                {
                    for (int i = 0; i < localSystem.Branches.Count; i++)
                    {
                        CheckBox chk =
                            panel.Controls.Find(localSystem.LocalSystemId + "," + (i + 1), true).FirstOrDefault() as
                                CheckBox;
                        if (chk != null)
                        {
                            chk.Checked = false;
                        }
                    }

                    ComboBox cb =
                        panel.Controls.Find(localSystem.LocalSystemId + "," + (localSystem.Branches.Count + 1), true)
                            .FirstOrDefault() as ComboBox;
                    if (cb != null)
                    {
                        cb.SelectedIndex = 0;
                    }
                }
            }
        }

        private void PanelsDisabled()
        {
            pnlControls.Enabled = false;
            panel.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void PanelsEnabled()
        {
            pnlControls.Enabled = true;
            panel.Enabled = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = true;
        }

        #region helpers
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}