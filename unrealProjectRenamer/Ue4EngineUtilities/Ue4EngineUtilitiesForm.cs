using System;
using System.ComponentModel;
using System.Windows.Forms;
using unrealProjectRenamer.Properties;

namespace unrealProjectRenamer
{
    public partial class Ue4EngineUtilitiesForm : Form
    {
        private readonly Ue4EngineUtilities engineUtilities;

        public Ue4EngineUtilitiesForm()
        {
            InitializeComponent();

            engineUtilities = new Ue4EngineUtilities();

            enginePathTextBox.Text = Settings.Default.EnginePath;

            enginePathTextBox.Validating += EnginePathTextBox_Validating;
        }

        private void EngineBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = Resources.MainForm_enginePathFolderBrowserDescription
            };

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                enginePathTextBox.Text = folderBrowserDialog.SelectedPath;

                EnginePathTextBox_Validating(null, null);
            }
        }

        protected void EnginePathTextBox_Validating(object sender, CancelEventArgs e)
        {
            engineUtilities.InitializeWithEnginePath(enginePathTextBox.Text);
            if (engineUtilities.IsEnginePathValid())
            {
                Settings.Default.EnginePath = enginePathTextBox.Text;
                Settings.Default.Save();
                enginePathErrorProvider.SetError(enginePathTextBox, "");
                continueButton.Enabled = true;
            }
            else
            {
                enginePathErrorProvider.SetError(enginePathTextBox, "Can't find engine files in given path!");
                continueButton.Enabled = false;
            }
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            MainForm form = new MainForm(engineUtilities);
            form.ShowDialog();
            Close();
        }
    }
}
