using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using TeatroSESI;
namespace Maracannes {
[InitializeOnLoad] public static class BackgroundAudioFix {
 static BackgroundAudioFix(){EditorApplication.update+=Run;}
 static void Run(){const string b="Assets/Maracannes/Documentation/";if(EditorApplication.isCompiling||EditorApplication.isUpdating||EditorApplication.isPlaying||!File.Exists(b+"audio-fix.request"))return;AssetDatabase.DeleteAsset(b+"audio-fix.request");EditorSceneManager.SaveOpenScenes();string prefab="Assets/Maracannes/Prefabs/Visitante_Compartilhado.prefab";var contents=PrefabUtility.LoadPrefabContents(prefab);try{var v=contents.GetComponent<VisitaCentroSESI>();if(!v.cameraVisita.GetComponent<AudioListener>())v.cameraVisita.gameObject.AddComponent<AudioListener>();PrefabUtility.SaveAsPrefabAsset(contents,prefab);}finally{PrefabUtility.UnloadPrefabContents(contents);}
 foreach(string path in new[]{AcervoTravel.Acervo,AcervoTravel.Theater}){var scene=EditorSceneManager.OpenScene(path,OpenSceneMode.Single);foreach(var v in scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<VisitaCentroSESI>(true)))if(v.cameraVisita&&!v.cameraVisita.GetComponent<AudioListener>())v.cameraVisita.gameObject.AddComponent<AudioListener>();var music=scene.GetRootGameObjects().First(g=>g.GetComponent<MuseumBackgroundMusic>());if(!music.GetComponent<AudioOutputAudit>())music.AddComponent<AudioOutputAudit>();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);}EditorUtility.audioMasterMute=false;AssetDatabase.SaveAssets();File.WriteAllText(b+"audio-output-test.request","run");EditorApplication.isPlaying=true;
 }
}
}
