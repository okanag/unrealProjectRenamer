using System;
using System.ComponentModel;
using System.Windows.Forms;
using unrealProjectRenamer.Properties;

namespace unrealProjectRenamer
{
    public partial class MainForm : Form
    {
        private readonly Ue4ProjectController projectController;
        private readonly Ue4EngineUtilities engineUtilities;

        public MainForm()
        {
            InitializeComponent();

            projectPathTextBox.Validating += new CancelEventHandler(ProjectPathTextBox_Validating);
            EnginePathTextBox.Validating += new CancelEventHandler(EnginePathTextBox_Validating);

            projectController = new Ue4ProjectController();
            engineUtilities = new Ue4EngineUtilities();
        }

        private void BrowseProjectButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = Resources.MainForm_projectPathFolderBrowserDescription
            };

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                projectPathTextBox.Text = folderBrowserDialog.SelectedPath;
                
                ProjectPathTextBox_Validating(null, null);
            }
        }

        protected void ProjectPathTextBox_Validating(object sender, CancelEventArgs e)
        {
            projectController.InitializeWithProjectPath(projectPathTextBox.Text);
            if (projectController.IsProjectPathValid())
            {
                projectPathErrorProvider.SetError(projectPathTextBox, "");
                CurrentProjectNameLabel.Text = projectController.GetProjectName();
            }
            else
            {
                projectPathErrorProvider.SetError(projectPathTextBox, "Can't find .uproject file in given path!");
                CurrentProjectNameLabel.Text = "";
            }
        }

        private void BrowseEngineButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = Resources.MainForm_enginePathFolderBrowserDescription
            };

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                EnginePathTextBox.Text = folderBrowserDialog.SelectedPath;

                EnginePathTextBox_Validating(null, null);
            }
        }

        protected void EnginePathTextBox_Validating(object sender, CancelEventArgs e)
        {
            engineUtilities.InitializeWithEnginePath(EnginePathTextBox.Text);
            if (engineUtilities.IsEnginePathValid())
            {
                projectPathErrorProvider.SetError(projectPathTextBox, "");
            }
            else
            {
                projectPathErrorProvider.SetError(EnginePathTextBox, "Can't find engine files in given path!");
            }
        }

        private void RenameButton_Click(object sender, EventArgs e)
        {
            if (newProjectNameBox.Text.Equals(""))
            {
                projectPathErrorProvider.SetError(newProjectNameBox, "Module name can't be empty!");
            }
            else
            {
                projectPathErrorProvider.SetError(newProjectNameBox, "");

                Ue4ProjectRenamer Renamer = new Ue4ProjectRenamer(engineUtilities, projectController, newProjectNameBox.Text);
                Renamer.Rename();
            }
        }
    }
}
