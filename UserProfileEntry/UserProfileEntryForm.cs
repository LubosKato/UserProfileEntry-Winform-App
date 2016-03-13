using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;
using UserProfileDomain;
using UserProfileEntry.DataProvides.Interfaces;

namespace UserProfileEntry
{
    public partial class UserProfileEntryForm : Form
    {
        private readonly IUserProfileDataProvider _userProfileDataProvider;
        private readonly IBranchesDataProvider _branchesDataProvider;
        private readonly IPermissionsDataProvider _permissionsDataProvider;
        private IList<UserProfile> _users;
        private UserProfile _currentUserProfile;
        private bool _noActiveUser;
        private readonly Dictionary<int, List<LocalSystem>> _localSystems;

        public UserProfileEntryForm(IUserProfileDataProvider userProfileDataProvider, IBranchesDataProvider branchesDataProvider, IPermissionsDataProvider permissionsDataProvider)
        {
            _userProfileDataProvider = userProfileDataProvider;
            _branchesDataProvider = branchesDataProvider;
            _permissionsDataProvider = permissionsDataProvider;
            _localSystems = new Dictionary<int, List<LocalSystem>>();
            InitializeComponent();
            PanelsDisabled();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateDefaultUser();
        }

        #region Populate Default User

        private void PopulateDefaultUser()
        {
            _users = _userProfileDataProvider.GetUserProfiles();
            
            try
            {
                if (_users.Count != 0)
                {
                    PopulateUserById(_users.Select(c => c.UserProfileId).First());
                    GenerateSystems();
                    _noActiveUser = false;
                    IsAdmin();
                }
                else
                {
                    ClearAndEnableControls(); 
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void IsAdmin()
        {
            btnEdit.Enabled = _currentUserProfile.UserProfileUserLevelToUserAdmin;
            PanelsDisabled();
        }

        private void ClearAndEnableControls()
        {
            _noActiveUser = true;
            txtUserId.Text = string.Empty;
            txtUserId.Text = string.Empty;
            txtDomain.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            chckAdmin.Checked = false;
            pnlControls.Enabled = true;
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

        private void PopulateUserById(int userProfileId)
        {
            _currentUserProfile = _users.FirstOrDefault(c => c.UserProfileId == userProfileId);
            if (_currentUserProfile != null)
            {
                txtUserId.Text = _currentUserProfile.UserProfileOperatorId.ToString();
                txtDomain.Text = _currentUserProfile.UserProfileDomainName;
                txtUserName.Text = _currentUserProfile.UserProfileName;
                txtEmail.Text = _currentUserProfile.UserProfileMailAddress;
                chckAdmin.Checked = _currentUserProfile.UserProfileUserLevelToUserAdmin;
            }
        }

        #endregion

        #region Button actions
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
            PopulateDefaultUser();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _userProfileDataProvider.DeleteUserProfile(_currentUserProfile.UserProfileId);
            PopulateDefaultUser();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Check Email Format!");
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
                    UserProfileOperatorId = Convert.ToInt32(txtUserId.Text),
                    UserProfileUserLevelToUserAdmin = chckAdmin.Checked
                };
                _userProfileDataProvider.CreateUserProfile(newUserProfile);
            }
            else
            {
                _currentUserProfile.UserProfileDomainName = txtDomain.Text;
                _currentUserProfile.UserProfileUserLevelToUserAdmin = chckAdmin.Checked;
                _currentUserProfile.UserProfileName = txtUserName.Text;
                _currentUserProfile.UserProfileMailAddress = txtEmail.Text;
                _currentUserProfile.UserProfileOperatorId = Convert.ToInt32(txtUserId.Text);
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
                var branch = localSystem.Branches.Where(c => c.Changed).Select(c => new  { c.BranchCode, c.Selected}).FirstOrDefault();
                if (branch != null)
                {
                    if (branch.BranchCode == localSystemBranch.Where(c => c.SystemId == localSystem.LocalSystemId).Select(c => c.BranchCode).FirstOrDefault())
                    {
                        if (branch.Selected)
                            _branchesDataProvider.LocalSystemUpdate(0, userProfileOperatorId, localSystem.LocalSystemId, branch.BranchCode);
                        else
                        {
                            _branchesDataProvider.LocalSystemUpdate(-1, userProfileOperatorId, localSystem.LocalSystemId, branch.BranchCode);
                        }
                    }
                    else
                    {
                        var newLocalSystemBranchId = _branchesDataProvider.GetLatestLocalSystemBranchId();
                        _branchesDataProvider.LocalSystemCreate(newLocalSystemBranchId + 1, 0, userProfileOperatorId, localSystem.LocalSystemId, branch.BranchCode);
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

        private void ResetChangedProperties()
        {
            foreach (var localSystem in _localSystems[_currentUserProfile.UserProfileOperatorId])
            {
                localSystem.Changed = false;
                foreach (var branchSelected in localSystem.Branches)
                {
                    branchSelected.Changed = false;
                }
                foreach (var permission in localSystem.Permissions)
                {
                    permission.Changed = false;
                }
            }
        }

        #endregion

        #region Populate dynamic controls

        private void GenerateSystems()
        {
            try
            {
                if (!_localSystems.ContainsKey(_currentUserProfile.UserProfileOperatorId))
                {
                     var system = LoadLocalSystemsFromDB();
                    _localSystems.Add(_currentUserProfile.UserProfileOperatorId, system);
                }

                GeneratePanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GeneratePanel()
        {
            var system = _localSystems[_currentUserProfile.UserProfileOperatorId];
            //get counts
            var columnCount = system.First().Branches.Count + 2;
            var rowCount = system.Count;
            //Clear out the existing controls, we are generating a new panel layout
            panel.Controls.Clear();

            //Clear out the existing row and column styles
            panel.ColumnStyles.Clear();
            panel.RowStyles.Clear();

            //Now we will generate the panel, setting up the row and column counts first
            panel.ColumnCount = columnCount;
            panel.RowCount = rowCount;
            
            for (int column = 0; column < columnCount; column++)
            {
                if (column == 0)
                    panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
                else if (column == system.First().Branches.Count + 1)
                    panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
                else
                    panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
                //Populate header
                CreateTableColumnTitles(column, system);
                // populate table
                for (int row = 1; row < rowCount; row++)
                {
                    // populate lables
                    if (column == 0)
                    {
                        //panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                        Label lbl = new Label {Text = system[row-1].LocalSystemName};
                        panel.Controls.Add(lbl, column, row);
                    }
                    //populate checkboxes
                    if (column > 0 && column <= system.First().Branches.Count)
                    {
                        CheckBox chck = new CheckBox
                        {
                            Name = row.ToString() + "," + column.ToString(),
                            Checked = system[row-1].Branches[column - 1].Selected
                        };
                        chck.CheckedChanged += this.BranchesChanged;
                        panel.Controls.Add(chck, column, row);
                    }
                    //populate dropdowns
                    if (column == system.First().Branches.Count + 1)
                    {
                        ComboBox comboBox = new ComboBox();
                        var selectedItem = system[row-1].Permissions.Where(c => c.Selected)
                                .Select(c => c.PermissionName)
                                .FirstOrDefault();
                        comboBox.Name = row.ToString() + "," + column.ToString();
                        comboBox.DataSource = system[row-1].Permissions;
                        comboBox.DisplayMember = "PermissionName";
                        comboBox.ValueMember = "PermissionId";
                        comboBox.Width = 200;
                        comboBox.SelectedValueChanged += this.ComboBoxChanged;
                        panel.Controls.Add(comboBox, column, row);
                        comboBox.SelectedIndex = comboBox.FindStringExact(selectedItem);
                    }
                }
            }
        }

        private void CreateTableColumnTitles(int column, List<LocalSystem> system)
        {
            if (column == 0)
            {
                panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                Label lbl = new Label {Text = "User Access:"};
                panel.Controls.Add(lbl, column, 0);
            }
            //populate lable for checkboxes
            if (column > 0 && column <= system.First().Branches.Count)
            {
                Label lbl = new Label {Text = system[0].Branches[column - 1].BranchCode };
                panel.Controls.Add(lbl, column, 0);

            }
            //populate label for dropdowns
            if (column == system.First().Branches.Count + 1)
            {
                Label lbl = new Label {Text = "Permissions"};
                panel.Controls.Add(lbl, column, 0);
            }
        }

        private List<LocalSystem> LoadLocalSystemsFromDB()
        {
            var systems = _permissionsDataProvider.GetSystems().ToList();
            var branches = _branchesDataProvider.GetBranches().ToList();
            var categories = _permissionsDataProvider.GetCategories().ToList();
            var userAccess = _permissionsDataProvider.GetUserAccess(_currentUserProfile.UserProfileOperatorId).ToList();
            var localSystemBranches = _branchesDataProvider.GetSystemBranches(_currentUserProfile.UserProfileOperatorId).ToList();
            systems.RemoveAll(x => x.LocalSystemName == string.Empty);
            branches.RemoveAll(x => x.BranchCode == string.Empty);

            foreach (var localSystem in systems)
            {
                localSystem.PopulateBranches(branches, localSystemBranches);
                localSystem.PopulatePermissions(userAccess, categories);
            }
            return systems;
        }

        #endregion

        #region events
        private void BranchesChanged(object sender, EventArgs e)
        {
            try
            {
                var sytem = _localSystems[_currentUserProfile.UserProfileOperatorId];
                //btnSave.Enabled = true;
                CheckBox objLabel = (CheckBox)sender;
                var name = objLabel.Name;
                string[] values = name.Split(',');
                var column = Convert.ToInt32(values[1]);
                var row = Convert.ToInt32(values[0]);
                sytem[row-1].Branches[column - 1].Selected = objLabel.Checked;
                sytem[row-1].Branches[column - 1].Changed = true;
                sytem[row-1].Changed = true;
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
                var sytem = _localSystems[_currentUserProfile.UserProfileOperatorId];
                //btnSave.Enabled = true;
                ComboBox objLabel = (ComboBox)sender;
                var name = objLabel.Name;
                string[] values = name.Split(',');
                var row = Convert.ToInt32(values[0]);
                foreach (var permission in sytem[row-1].Permissions)
                {
                    permission.Selected = false;
                }
                // set just one to selected
                if (objLabel.SelectedIndex >= 0)
                {
                    sytem[row-1].Permissions[objLabel.SelectedIndex].Selected = true;
                    sytem[row-1].Permissions[objLabel.SelectedIndex].Changed = true;
                    sytem[row-1].Changed = true;
                    //PopulateDefaultUser();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtUserId_TextChanged(object sender, EventArgs e)
        {
            if (txtUserId.Text != null && _currentUserProfile != null)
            {
                int operatorId;
                if (Int32.TryParse(txtUserId.Text, out operatorId))
                {
                    _currentUserProfile.UserProfileOperatorId = operatorId;
                    GenerateSystems();
                }
            }
            else
            {
                btnSave.Enabled = true;
            }
        }

        #endregion

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
