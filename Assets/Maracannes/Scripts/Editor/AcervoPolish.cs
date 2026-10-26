using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace Maracannes {
public static partial class MaracannesBuilder {
 [MenuItem("Maracannes/Finalizar iluminação do acervo")]
 public static void PolishAcervo(){
  if(EditorApplication.isPlaying)return;EditorSceneManager.SaveOpenScenes();scene=EditorSceneManager.OpenScene(A,OpenSceneMode.Single);root=scene.GetRootGameObjects().First(g=>g.name=="Maracannes_Acervo").transform;SetupMaterials();brass.SetFloat("_Metallic",.45f);var ambient=root.GetComponent<AcervoAmbient>();if(!ambient)ambient=root.gameObject.AddComponent<AcervoAmbient>();ambient.Apply();
  var old=root.Find("Luz_Indireta_Galerias");if(old)UnityEngine.Object.DestroyImmediate(old.gameObject);var lighting=Group("Luz_Indireta_Galerias",root);var fill=Group("Luz geral rebatida",lighting);fill.rotation=Quaternion.Euler(65,20,0);var broad=fill.gameObject.AddComponent<Light>();broad.type=LightType.Directional;broad.color=new Color(1,.97f,.91f);broad.intensity=.65f;broad.shadows=LightShadows.None;
  foreach(var p in new[]{new Vector3(20,3.45f,-25),new Vector3(28,3.45f,-25),new Vector3(37,3.45f,-22),new Vector3(49,3.45f,-22),new Vector3(37,3.45f,-4),new Vector3(49,3.45f,-4),new Vector3(20,3.45f,-6),new Vector3(28,3.45f,-6)}){var t=Group("Preenchimento difuso",lighting);t.position=p;var l=t.gameObject.AddComponent<Light>();l.type=LightType.Point;l.color=new Color(1,.95f,.86f);l.intensity=7;l.range=14;l.shadows=LightShadows.None;Box("Difusor da luminária",p+Vector3.up*.26f,new Vector3(.65f,.06f,.35f),light,lighting,false);}
  foreach(var t in root.GetComponentsInChildren<Transform>(true).Where(t=>t.name=="Junta discreta do piso").ToArray())UnityEngine.Object.DestroyImmediate(t.gameObject);
  foreach(var mobile in root.GetComponentsInChildren<AcervoSculpture>().Where(s=>s.technique.StartsWith("Móbile"))){if(!mobile.transform.Find("Ancoragem no teto"))for(int anchor=-1;anchor<=1;anchor+=2)Bar("Ancoragem no teto",new Vector3(anchor*1.2f,0,0),new Vector3(anchor*1.2f,.15f,0),.018f,graphite,mobile.transform);}
  FixDoorSign(root);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene,A);
  Capture("Acervo_Sala1",new Vector3(18.4f,1.7f,-25.8f),new Vector3(26,1.5f,-28));Capture("Acervo_Sala2",new Vector3(35,1.7f,-25.8f),new Vector3(44,1.6f,-24));Capture("Acervo_Sala3",new Vector3(43,1.7f,-9.3f),new Vector3(43,1.7f,0));Capture("Acervo_Sala4",new Vector3(29,1.7f,-13),new Vector3(23.5f,1.7f,-2));
  scene=EditorSceneManager.OpenScene(T,OpenSceneMode.Single);var roots=scene.GetRootGameObjects();foreach(var r in roots)FixDoorSign(r.transform);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
  string archive=B+"Documentation/Arquivo_Anterior";if(!AssetDatabase.IsValidFolder(archive))AssetDatabase.CreateFolder(B+"Documentation","Arquivo_Anterior");if(File.Exists(S))AssetDatabase.MoveAsset(S,archive+"/Maracannes_Exposicao_Anterior.unity");AssetDatabase.SaveAssets();AssetDatabase.Refresh();
 }
 static void FixDoorSign(Transform r){foreach(var d in r.GetComponentsInChildren<AcervoDoor>(true)){var sign=d.transform.Find("Placa da porta");if(!sign)continue;sign.localPosition=new Vector3(0,2.98f,0);foreach(var child in sign.GetComponentsInChildren<Transform>()){if(child.name=="Suporte fixado"){var size=child.localScale;size.y=.32f;child.localScale=size;}if(child.name=="Título")child.localPosition=new Vector3(0,.07f,-.04f);if(child.name=="Legenda"){var text=child.GetComponent<TextMesh>();if(text)UnityEngine.Object.DestroyImmediate(text.gameObject);}}}}
}
[InitializeOnLoad] public static class AcervoPolishTrigger {static AcervoPolishTrigger(){EditorApplication.delayCall+=()=>{const string marker="Assets/Maracannes/Documentation/acervo-polish.request";if(File.Exists(marker)&&!EditorApplication.isPlaying){AssetDatabase.DeleteAsset(marker);MaracannesBuilder.PolishAcervo();}};}}
}
