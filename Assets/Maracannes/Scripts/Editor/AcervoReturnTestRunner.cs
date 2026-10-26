using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using TeatroSESI;
namespace Maracannes {
[InitializeOnLoad] public static class AcervoReturnTestRunner {
 const string B="Assets/Maracannes/Documentation/";
 static AcervoReturnTestRunner(){EditorApplication.update+=Run;EditorApplication.playModeStateChanged+=State;}
 static void Run(){if(EditorApplication.isPlaying||EditorApplication.isCompiling||EditorApplication.isUpdating||!File.Exists(B+"retorno-start.request"))return;AssetDatabase.DeleteAsset(B+"retorno-start.request");EditorSceneManager.SaveOpenScenes();EditorSceneManager.OpenScene(AcervoTravel.Theater,OpenSceneMode.Single);File.WriteAllText(B+"retorno-test.request","run");EditorApplication.isPlaying=true;}
 static void State(PlayModeStateChange state){if(state!=PlayModeStateChange.EnteredPlayMode||!File.Exists(B+"retorno-test.request")||File.ReadAllText(B+"retorno-test.request").Trim()!="run")return;var v=Object.FindAnyObjectByType<VisitaCentroSESI>();v.gameObject.AddComponent<AcervoReturnAudit>();}
}
}
