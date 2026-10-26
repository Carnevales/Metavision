using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace Maracannes {
[InitializeOnLoad] public static class Experience2024TestRunner {
 static Experience2024TestRunner(){EditorApplication.update+=Run;}
 static void Run(){const string p="Assets/Maracannes/Documentation/experience2024-play.request";if(EditorApplication.isCompiling||EditorApplication.isUpdating||EditorApplication.isPlaying||!File.Exists(p))return;AssetDatabase.DeleteAsset(p);var scene=EditorSceneManager.OpenScene(AcervoTravel.Acervo,OpenSceneMode.Single);foreach(var root in scene.GetRootGameObjects()){foreach(var tm in root.GetComponentsInChildren<TextMesh>(true))if(tm.text.StartsWith("04 ")&&tm.text.Contains("MEMÓRIAS"))tm.text=tm.text.Replace("04","05");foreach(var t in root.GetComponentsInChildren<Transform>(true).Where(t=>t.name=="Portal_aberto"&&Vector3.Distance(t.position,new Vector3(43,0,-12))<.1f).ToArray())Object.DestroyImmediate(t.gameObject);}MaracannesBuilder.PolishExperience2024();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);MaracannesBuilder.Capture("Experience2024_Editor",new Vector3(58,1.7f,-18),new Vector3(66,1.5f,-12));EditorApplication.isPlaying=true;}
}
}

