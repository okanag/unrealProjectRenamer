using System.Collections.Generic;
using System.IO;

namespace unrealProjectRenamer
{
    class Ue4ProjectController
    {
        private string ue4ProjectPath;
        private string ue4ProjectName;

        private string projectPluginsFolder;

        public bool InitializeWithProjectPath(string text)
        {
            if (!Directory.Exists(text))
            {
                ue4ProjectPath = "";
                return false;
            }
          
            string[] files = Directory.GetFiles(text);
            foreach (string file in files)
            {
                if (file.Contains(".uproject"))
                {
                    ue4ProjectPath = text;
                    ue4ProjectName = Path.GetFileNameWithoutExtension(ue4ProjectPath);
                    FindPluginsFolder();
                    return true;
                }
            }

            ue4ProjectPath = "";
            return false;
        }

        private void FindPluginsFolder()
        {
            string pluginsFolder = Path.Combine(ue4ProjectPath, "Plugins");
            projectPluginsFolder = Directory.Exists(pluginsFolder) ? pluginsFolder : "";
        }

        public List<string> GetPossibleModuleLocationList()
        {
            List<string> modueLocations = GetValidatedPluginList();
            modueLocations.Insert(0, ue4ProjectName);
            return modueLocations;
        }

        private List<string> GetValidatedPluginList()
        {
            //If there are no plugins, plugins folder does not exist
            if (projectPluginsFolder.Equals(""))
            {
                return new List<string>();
            }

            string[] pluginFolderList = Directory.GetDirectories(projectPluginsFolder);
            List<string> validatedPluginList = new List<string>();

            foreach (string pluginFolder in pluginFolderList)
            {
                string pluginName = Path.GetFileName(pluginFolder);
                string pluginFile = Path.Combine(pluginFolder, pluginName + ".uplugin");
                if (File.Exists(pluginFile))
                {
                    validatedPluginList.Add(pluginName);
                }
            }

            return validatedPluginList;
        }

        public bool IsProjectPathValid()
        {
            return !ue4ProjectPath.Equals("");
        }

        public string GetPathForModuleLocation(string selectedItem)
        {
            if (IsMainGameModuleSelected(selectedItem))
            {
                return ue4ProjectPath;
            }

            return Path.Combine(projectPluginsFolder, selectedItem);
        }

        public bool IsMainGameModuleSelected(string selectedItem)
        {
            return selectedItem.Equals(ue4ProjectName);
        }

        public string GetProjectName()
        {
            return ue4ProjectName;
        }

        public string GetProjectPath()
        {
            return ue4ProjectPath;
        }
    }
}
