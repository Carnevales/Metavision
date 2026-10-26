using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace Maracannes {
[InitializeOnLoad]public static class BackgroundMusicSetup {
 const string B="Assets/Maracannes/";
 static BackgroundMusicSetup(){EditorApplication.update+=Run;}
 static void Run(){const string marker=B+"Documentation/musica-build.request";if(EditorApplication.isPlaying||EditorApplication.isCompiling||EditorApplication.isUpdating||!File.Exists(marker))return;AssetDatabase.DeleteAsset(marker);EditorSceneManager.SaveOpenScenes();string path=B+"Audio/Ambiente_Lofi.mp3";var imp=AssetImporter.GetAtPath(path) as AudioImporter;var settings=imp.defaultSampleSettings;settings.loadType=AudioClipLoadType.Streaming;settings.compressionFormat=AudioCompressionFormat.Vorbis;settings.quality=.65f;imp.defaultSampleSettings=settings;imp.forceToMono=false;imp.SaveAndReimport();var clip=AssetDatabase.LoadAssetAtPath<AudioClip>(path);
 foreach(var target in new[]{AcervoTravel.Acervo,AcervoTravel.Theater}){var scene=EditorSceneManager.OpenScene(target,OpenSceneMode.Single);var go=scene.GetRootGameObjects().FirstOrDefault(g=>g.name=="Musica_Ambiente_Lofi");if(!go)go=new GameObject("Musica_Ambiente_Lofi");var source=go.GetComponent<AudioSource>();if(!source)source=go.AddComponent<AudioSource>();source.clip=clip;source.loop=true;source.playOnAwake=false;source.spatialBlend=0;source.volume=.035f;source.priority=192;var music=go.GetComponent<MuseumBackgroundMusic>();if(!music)music=go.AddComponent<MuseumBackgroundMusic>();music.volume=.035f;if(target==AcervoTravel.Theater&&!go.GetComponent<BackgroundMusicAudit>())go.AddComponent<BackgroundMusicAudit>();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);}AssetDatabase.SaveAssets();File.WriteAllText(B+"Documentation/musica-test.request","run");EditorApplication.isPlaying=true;
 }
}
}
