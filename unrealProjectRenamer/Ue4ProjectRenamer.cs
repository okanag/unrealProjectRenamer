using System.IO;
using System.Windows.Forms;

namespace unrealProjectRenamer
{
    class Ue4ProjectRenamer
    {
        private readonly Ue4EngineUtilities engineUtilities;
        private readonly Ue4ProjectController projectController;
        private readonly string newName;
        private string newProjectPath;

        public Ue4ProjectRenamer(Ue4EngineUtilities engineUtilities, Ue4ProjectController projectController, string newName)
        {
            this.engineUtilities = engineUtilities;
            this.projectController = projectController;
            this.newName = newName;
        }

        //ToDo:
        //    Open the Source folder.
        //    Open OldName.cpp
        //    Change IMPLEMENT_PRIMARY_GAME_MODULE(FDefaultGameModuleImpl, OldName, "OldName" ); to IMPLEMENT_PRIMARY_GAME_MODULE(FDefaultGameModuleImpl, NewName, "NewName" );
        //    Open the Config folder, and check all the configuration files for OldName references to change to NewName.For example, change GlobalDefaultGameMode =/ Script / OldName.OldNameGameMode to GlobalDefaultGameMode =/ Script / NewName.OldNameGameMode.
        //    Add the following to DefaultEngine.ini (under an existing or new [/ Script / Engine.Engine] header):
        //    +ActiveGameNameRedirects=(OldGameName="/Script/OldName", NewGameName="/Script/NewName")
        //    If you have any OLDNAME_API in your project's header files, change those instances to NEWNAME_API.
        //    Compile, and your project should now open without errors.
        public void Rename()
        {
            DuplicateProject();
            RenameUprojectFile();
            UpdateNewUprojectFile();
            RenameThumbnail();
            RenameTargetCs();
            UpdateNewTargetCsFile();
            RenameEditorTargetCs();
            UpdateNewEditorTargetCsFile();
            RenameMainModuleFolder();
            RenameBuildCs();
            UpdateNewBuildCsFile();
            GenerateProjectFiles();
            //UpdateMainModuleCpp();
            //UpdateConfigFiles();
            //AddGameRedirectsToEngineIni();
            //UpdateApiInSource();
            Application.Exit();
        }

        private void DuplicateProject()
        {
            string sourcePath = projectController.GetProjectPath();
            newProjectPath = projectController.GetProjectPath();
            //Todo: Can this be safer?
            newProjectPath = newProjectPath.Replace(projectController.GetProjectName(), newName);

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                if (!dirPath.Contains("\\Intermediate") && 
                    !dirPath.Contains("\\Saved") && 
                    !dirPath.Contains("\\.vs"))
                {
                    Directory.CreateDirectory(dirPath.Replace(sourcePath, newProjectPath));
                }
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                if (!newPath.Contains("\\Intermediate\\") && 
                    !newPath.Contains("\\Saved\\") && 
                    !newPath.Contains("\\.vs\\") && 
                    !newPath.Contains(projectController.GetProjectName() + ".sln"))
                {
                    File.Copy(newPath, newPath.Replace(sourcePath, newProjectPath), true);
                }
            }
        }

        private void RenameUprojectFile()
        {
            RenameProjectFile("", ".uproject");
        }

        private void UpdateNewUprojectFile()
        {
            UpdateProjectFile("", ".uproject");
        }

        private void RenameThumbnail()
        {
            RenameProjectFile("", ".png");
        }

        private void RenameTargetCs()
        {
            RenameProjectFile("Source", ".Target.cs");
        }

        private void UpdateNewTargetCsFile()
        {
            UpdateProjectFile("Source", ".Target.cs");
        }

        private void RenameEditorTargetCs()
        {
            RenameProjectFile("Source", "Editor.Target.cs");
        }

        private void UpdateNewEditorTargetCsFile()
        {
            UpdateProjectFile("Source", "Editor.Target.cs");
        }

        private void RenameMainModuleFolder()
        {
            string sourcePath = Path.Combine(newProjectPath, "Source");
            string sourceModulePath = Path.Combine(sourcePath, projectController.GetProjectName());
            string destinationModulePath = Path.Combine(sourcePath, newName);

            if (Directory.Exists(destinationModulePath))
            {
                //ToDo: Error there is already a game module with the new name
            }

            Directory.CreateDirectory(destinationModulePath);
            //Copy all the files & Replaces any files with the same name
            foreach (string file in Directory.GetFiles(sourceModulePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(file, file.Replace(sourceModulePath, destinationModulePath), true);
            }

            Directory.Delete(sourceModulePath, true);
        }

        private void RenameBuildCs()
        {
            RenameProjectFile(Path.Combine("Source", newName), ".Build.cs");
        }

        private void UpdateNewBuildCsFile()
        {
            UpdateProjectFile(Path.Combine("Source", newName), ".Build.cs");
        }

        private void GenerateProjectFiles()
        {
            engineUtilities.GenerateProjectFiles(Path.Combine(newProjectPath, newName + ".uproject"));
        }

        private void UpdateProjectFile(string path, string extention)
        {
            string filePath = Path.Combine(newProjectPath, path, newName + extention);
            string fileContent = File.ReadAllText(filePath);

            fileContent = fileContent.Replace(projectController.GetProjectName(), newName);
            File.WriteAllText(filePath, fileContent);
        }

        private void RenameProjectFile(string path, string extention)
        {
            string oldFile = Path.Combine(newProjectPath, path, projectController.GetProjectName() + extention);
            if (File.Exists(oldFile))
            {
                string newFile = Path.Combine(newProjectPath, path, newName + extention);
                File.Move(oldFile, newFile);
            }
        }
    }
}
