using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UserProfileDomain;

namespace UserProfileEntry
{
    public partial class UserProfileEntryForm
    {
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
                        Label lbl = new Label { Text = system[row - 1].LocalSystemName };
                        panel.Controls.Add(lbl, column, row);
                    }
                    //populate checkboxes
                    if (column > 0 && column <= system.First().Branches.Count)
                    {
                        CheckBox chck = new CheckBox
                        {
                            Name = row.ToString() + "," + column.ToString(),
                            Checked = system[row - 1].Branches[column - 1].Selected
                        };
                        chck.CheckedChanged += this.BranchesChanged;
                        panel.Controls.Add(chck, column, row);
                    }
                    //populate dropdowns
                    if (column == system.First().Branches.Count + 1)
                    {
                        ComboBox comboBox = new ComboBox();
                        var selectedItem = system[row - 1].Permissions.Where(c => c.Selected)
                                .Select(c => c.PermissionName)
                                .FirstOrDefault();
                        comboBox.Name = row.ToString() + "," + column.ToString();
                        comboBox.DataSource = system[row - 1].Permissions;
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
                Label lbl = new Label { Text = "User Access:" };
                panel.Controls.Add(lbl, column, 0);
            }
            //populate lable for checkboxes
            if (column > 0 && column <= system.First().Branches.Count)
            {
                Label lbl = new Label { Text = system[0].Branches[column - 1].BranchCode };
                panel.Controls.Add(lbl, column, 0);

            }
            //populate label for dropdowns
            if (column == system.First().Branches.Count + 1)
            {
                Label lbl = new Label { Text = "Permissions" };
                panel.Controls.Add(lbl, column, 0);
            }
        }
    }
}