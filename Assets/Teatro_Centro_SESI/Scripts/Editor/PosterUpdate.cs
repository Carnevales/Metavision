using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
namespace TeatroSESI {
[InitializeOnLoad] public static class PosterUpdate {
 const string B="Assets/Teatro_Centro_SESI/";
 static PosterUpdate(){EditorApplication.delayCall+=()=>{if(File.Exists(B+"References/poster.request")){AssetDatabase.DeleteAsset(B+"References/poster.request");Apply();}};}
 [MenuItem("Teatro SESI/Museu/Atualizar cartaz Maracannes")]
 public static void Apply(){Configure();var scene=SceneManager.GetSceneByPath(B+"Scenes/Teatro_Centro_SESI.unity");EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();HallPolish.Capture("Maracannes_Foyer",new Vector3(9.8f,1.65f,-24.5f),new Vector3(13.35f,1.6f,-24.3f));HallPolish.Capture("Maracannes_Fachada",new Vector3(3.9f,3.7f,-41),new Vector3(3.9f,3.8f,-27.8f));AssetDatabase.Refresh();File.WriteAllText(B+"References/cartaz-atualizado.txt","PASS Cartaz existente reutilizado no foyer, moldura 1,50 × 1,13 m, igual aos painéis 02/03. Título Maracannes Memorias da Arte. Mesma programação aplicada ao letreiro existente da fachada. Texto permanece editável em TextMesh.");}
 public static void Configure(){
  var scene=SceneManager.GetSceneByPath(B+"Scenes/Teatro_Centro_SESI.unity");if(!scene.isLoaded)throw new System.InvalidOperationException("Abra a cena do teatro.");
  var all=scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).ToArray();var museum=all.First(t=>t.name=="Museu_Interativo_SESI");
  var frame=all.First(t=>t.name=="Painel programação museu");var text=all.Select(t=>t.GetComponent<TextMesh>()).First(t=>t&&(t.name=="Cartaz_Texto"||t.name.StartsWith("EM CARTAZ")));
  var foyer=all.First(t=>t.name=="Foyer_e_Exposicao");var mount=foyer.Find("Cartaz_Programacao_Maracannes");if(!mount){mount=new GameObject("Cartaz_Programacao_Maracannes").transform;mount.SetParent(foyer,false);}
  mount.position=new Vector3(13.355f,1.6f,-23.2f);mount.rotation=Quaternion.Euler(0,90,0);
  frame.SetParent(mount,false);frame.localPosition=Vector3.zero;frame.localRotation=Quaternion.identity;frame.localScale=new Vector3(1.5f,1.13f,.07f);
  text.transform.SetParent(mount,false);text.name="Cartaz_Texto";SetText(text,"EM CARTAZ\n\nMARACANNES\nMEMORIAS DA ARTE",new Vector3(0,0,-.043f),1.27f,.80f);
  var trim=mount.Find("Friso cartaz");if(!trim){var cube=GameObject.CreatePrimitive(PrimitiveType.Cube);cube.name="Friso cartaz";trim=cube.transform;trim.SetParent(mount,false);Object.DestroyImmediate(cube.GetComponent<Collider>());}trim.localPosition=new Vector3(0,.48f,-.041f);trim.localScale=new Vector3(1.5f,.08f,.01f);trim.GetComponent<Renderer>().sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>(B+"Materials/Museu_Laranja.mat");
  var scenic=all.First(t=>t.name.StartsWith("Painel_04"));scenic.position=new Vector3(13.355f,1.6f,-25.5f);scenic.rotation=Quaternion.Euler(0,90,0);scenic.Find("Moldura").localScale=new Vector3(1.5f,1.13f,.07f);scenic.Find("Friso").localScale=new Vector3(1.5f,.08f,.01f);
  foreach(var tm in scenic.GetComponentsInChildren<TextMesh>()){bool prompt=tm.text.StartsWith("[E]");SetText(tm,tm.text,new Vector3(0,prompt?-.33f:.13f,-.052f),prompt?1.275f:1.26f,prompt?.10f:.40f);}
  all.Select(t=>t.GetComponent<MuseumPoint>()).First(p=>p&&p.title.StartsWith("04")).transform.position=scenic.position;
  var banner=all.First(t=>t.name.StartsWith("Painel laranja"));banner.GetComponent<Renderer>().sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>(B+"Materials/Museu_Grafite.mat");
  var bannerRoot=banner.parent.Find("Letreiro_Programacao_Maracannes");if(!bannerRoot){bannerRoot=new GameObject("Letreiro_Programacao_Maracannes").transform;bannerRoot.SetParent(banner.parent,false);}bannerRoot.position=new Vector3(3.9f,4.05f,-29.64f);bannerRoot.rotation=Quaternion.identity;
  MakeText(bannerRoot,"Letreiro_EmCartaz","EM CARTAZ",new Vector3(-8.25f,0,-.006f),3.1f,.25f);
  MakeText(bannerRoot,"Letreiro_Titulo","MARACANNES · MEMORIAS DA ARTE",new Vector3(1.8f,0,-.006f),15.3f,.43f);
 }
 static void MakeText(Transform parent,string name,string value,Vector3 p,float w,float h){var t=parent.Find(name);if(!t){t=new GameObject(name).transform;t.SetParent(parent,false);t.gameObject.AddComponent<TextMesh>();}SetText(t.GetComponent<TextMesh>(),value,p,w,h);}
 static void SetText(TextMesh tm,string value,Vector3 p,float w,float h){tm.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");tm.fontSize=80;tm.characterSize=.04f;tm.text=value;tm.anchor=TextAnchor.MiddleCenter;tm.alignment=TextAlignment.Center;tm.color=new Color(.95f,.94f,.89f);tm.transform.localPosition=p;tm.transform.localRotation=Quaternion.identity;tm.transform.localScale=Vector3.one;var bounds=tm.GetComponent<Renderer>().localBounds.size;tm.transform.localScale=Vector3.one*Mathf.Min(w/bounds.x,h/bounds.y);var depth=tm.GetComponent<MuseumTextDepth>();if(!depth)depth=tm.gameObject.AddComponent<MuseumTextDepth>();depth.depthShader=Shader.Find("TeatroSESI/Texto com profundidade");}
}
}
