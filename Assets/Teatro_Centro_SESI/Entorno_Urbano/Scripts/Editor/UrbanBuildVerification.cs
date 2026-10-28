using System.IO;using System.Linq;using UnityEditor;using UnityEditor.Build.Reporting;
namespace TeatroSESI.Urban {
[InitializeOnLoad]public static class UrbanBuildVerification {
 static UrbanBuildVerification(){EditorApplication.update+=Run;}
 static void Run(){const string b=UrbanBuilder.B+"References/";if(EditorApplication.isPlaying||EditorApplication.isCompiling||EditorApplication.isUpdating||!File.Exists(b+"urban-player.request")||!File.Exists(b+"urban-runtime.txt"))return;AssetDatabase.DeleteAsset(b+"urban-player.request");Directory.CreateDirectory("Builds/Metavision_Entorno");var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=EditorBuildSettings.scenes.Where(s=>s.enabled).Select(s=>s.path).ToArray(),locationPathName="Builds/Metavision_Entorno/Metavision.exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.Development});File.WriteAllText(b+"urban-player-build.txt",report.summary.result+" · erros "+report.summary.totalErrors+" · tamanho "+report.summary.totalSize+" · "+report.summary.totalTime);}
}
}
