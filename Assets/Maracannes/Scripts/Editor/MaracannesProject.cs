using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace Maracannes {
[InitializeOnLoad] public static class MaracannesProject {
 static MaracannesProject(){EditorApplication.delayCall+=()=>{const string marker="Assets/Maracannes/Documentation/boot.request";if(!EditorApplication.isPlaying&&File.Exists(marker)){AssetDatabase.DeleteAsset(marker);Open();}};}
 [MenuItem("Maracannes/Abrir visita integrada")]
 public static void Open(){if(EditorApplication.isPlaying)return;EditorSceneManager.SaveOpenScenes();EditorSceneManager.OpenScene("Assets/Teatro_Centro_SESI/Scenes/Teatro.unity",OpenSceneMode.Single);}
}
}
