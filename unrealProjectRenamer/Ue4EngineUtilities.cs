using System.Diagnostics;
using System.IO;

namespace unrealProjectRenamer
{
    class Ue4EngineUtilities
    {
        private string enginePath;

        public void InitializeWithEnginePath(string enginePath)
        {
            this.enginePath = File.Exists(Path.Combine(enginePath, "Engine/Binaries/DotNET/UnrealBuildTool.exe")) ? enginePath : "";
        }

        public bool IsEnginePathValid()
        {
            return enginePath != "";
        }

        public void GenerateProjectFiles(string uprojectFilePath)
        {
            string buildBatPath = Path.Combine(enginePath, "Engine/Binaries/DotNET/UnrealBuildTool.exe");
            
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.FileName = buildBatPath;
            processStartInfo.Arguments = "-projectfiles -project=\"" + uprojectFilePath + "\" -game -rocket -progress";
            Process process = Process.Start(processStartInfo);
            process?.WaitForExit();
        }
    }
}
