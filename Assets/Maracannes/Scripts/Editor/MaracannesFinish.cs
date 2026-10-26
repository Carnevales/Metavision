using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Maracannes {
[InitializeOnLoad] public static class MaracannesFinish {
 const string B="Assets/Maracannes/";
 static MaracannesFinish(){EditorApplication.delayCall+=()=>{if(EditorApplication.isPlaying||!File.Exists(B+"Documentation/finish.request"))return;AssetDatabase.DeleteAsset(B+"Documentation/finish.request");
 var theater=SceneManager.GetSceneByPath("Assets/Teatro_Centro_SESI/Scenes/Teatro.unity");if(!theater.isLoaded)EditorSceneManager.OpenScene("Assets/Teatro_Centro_SESI/Scenes/Teatro.unity",OpenSceneMode.Additive);
 var scene=SceneManager.GetSceneByPath(B+"Scenes/Maracannes_Exposicao.unity");if(!scene.isLoaded)scene=EditorSceneManager.OpenScene(B+"Scenes/Maracannes_Exposicao.unity",OpenSceneMode.Additive);SceneManager.SetActiveScene(scene);
 var all=scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).ToArray();all.First(t=>t.name=="Laje piso sala 4").GetComponent<Renderer>().sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>(B+"Materials/Pedra_museu.mat");EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
 int missing=all.Sum(t=>GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject));int bad=all.Where(t=>t.GetComponent<Renderer>()).Sum(t=>t.GetComponent<Renderer>().sharedMaterials.Count(m=>!m||!m.shader||m.shader.name.Contains("Error")));
 File.WriteAllText(B+"Documentation/verificacao-final.txt",(missing==0&&bad==0?"PASS":"FAIL")+" Verificação final da cena salva\nScripts ausentes: "+missing+"\nMateriais/shaders ausentes: "+bad+"\nCâmeras na ala nova: "+all.Count(t=>t.GetComponent<Camera>())+"\nSalas: "+all.Count(t=>t.GetComponent<MaracannesRoom>())+"\nMateriais expostos: "+all.Count(t=>t.GetComponent<MaracannesExhibit>())+"\nBuild Settings: "+string.Join(", ",EditorBuildSettings.scenes.Where(s=>s.enabled).Select(s=>s.path))+"\nAjuste final apenas do material do piso da sala 4; colisores e interações preservados.");
 MaracannesBuilder.Capture("Sala_4_Memorias",new Vector3(29,1.7f,-13),new Vector3(23.5f,1.9f,-2));AssetDatabase.Refresh();};}
}
}
