using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace unrealProjectRenamer
{
    class Ue4ProjectRenamer
    {
        private readonly Ue4ProjectController projectController;
        private readonly string newName;
        private string newProjectPath;

        public Ue4ProjectRenamer(Ue4ProjectController projectController, string newName)
        {
            this.projectController = projectController;
            this.newName = newName;
        }

        //ToDo:
        //    Open NewName.uproject in a text editor, and replace all instances of OldName with NewName. Save and close the file.
        //    If you have an OldName.png thumbnail, rename it to NewName.png.
        //    Open the Source folder.
        //    Rename OldName.Target.cs to NewName.Target.cs.
        //    Open NewName.Target.cs.Find all instances of OldName in this file with NewName.There may be some partial word matches.Save and close the file.
        //    Repeat step 7 and 8 for OldNameEditor.Target.cs (renaming it to NewNameEditor.Target.cs.
        //    Rename this OldName folder to NewName as well.
        //    Open the NewName folder.
        //    Rename OldName.Build.cs to NewName.Build.cs.
        //    Open NewName.Build.cs.Find all instances of OldName in this file with NewName.Save and close the file.
        //    Go back to the main project folder, and right-click on the NewName.uproject file. Select Generate Visual Studio Project files. (If you don't have this option, run Engine/Binaries/Win64/UnrealVersionSelector-Win64-Shipping.exe once).
        //    Open NewName.sln.
        //    Open OldName.cpp
        //    Change IMPLEMENT_PRIMARY_GAME_MODULE(FDefaultGameModuleImpl, OldName, "OldName" ); to IMPLEMENT_PRIMARY_GAME_MODULE(FDefaultGameModuleImpl, NewName, "NewName" );
        //    Open the Config folder, and check all the configuration files for OldName references to change to NewName.For example, change GlobalDefaultGameMode =/ Script / OldName.OldNameGameMode to GlobalDefaultGameMode =/ Script / NewName.OldNameGameMode.
        //    Add the following to DefaultEngine.ini (under an existing or new [/ Script / Engine.Engine] header):
        //    +ActiveGameNameRedirects=(OldGameName="/Script/OldName", NewGameName="/Script/NewName")
        //    If you have any OLDNAME_API in your project's header files, change those instances to NEWNAME_API.
        //    Delete Saved and Intermediate folders in your project directory.
        //    Compile, and your project should now open without errors.
        public void Rename()
        {
            DuplicateProject();
            RenameUprojectFile();
            //UpdateNewUprojectFile();
            //RenameThumbnail();
            //RenameTargetCs();
            //UpdateNewTargetCsFile();
            //RenameEditorTargetCs();
            //UpdateNewEditorTargetCsFile();
            //RenameMainModuleFolder();
            //RenameBuildCs():
            //UpdateNewBuildCsFile();
            //GenerateProjectFiles();
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
                Directory.CreateDirectory(dirPath.Replace(sourcePath, newProjectPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, newProjectPath), true);
            }
        }

        private void RenameUprojectFile()
        {
            string oldUproject = Path.Combine(newProjectPath, projectController.GetProjectName() + ".uproject");
            string newUproject = Path.Combine(newProjectPath, newName + ".uproject");
            File.Move(oldUproject, newUproject);
        }
    }
}
