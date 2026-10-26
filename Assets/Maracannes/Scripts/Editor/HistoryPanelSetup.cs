using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using TeatroSESI;
namespace Maracannes {
public static partial class MaracannesBuilder {
 public static void ConfigureHistory(Scene target){
  var text=File.ReadAllText(B+"Documentation/HISTORIA_FORNECIDA.txt").Replace("\r","").Replace("**","");var parts=text.Split(new[]{"\n## "},System.StringSplitOptions.None);
  var panels=target.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).Where(t=>t.name.StartsWith("Marco histórico ")).OrderBy(t=>t.name).ToArray();
  for(int i=0;i<panels.Length;i++){var t=panels[i];var p=t.GetComponent<MuseumPoint>();if(!p)p=t.gameObject.AddComponent<MuseumPoint>();string section=parts[i];int end=section.IndexOf('\n');p.title=section.Substring(0,end).TrimStart('#',' ');p.body=section.Substring(end).Trim();if(i==0&&parts.Length>3)p.body+="\n\n"+parts[3].Trim();p.source="Texto histórico fornecido pelo responsável pelo projeto.";p.action=MuseumAction.Exhibit;p.image=null;p.radius=2.3f;
   if(!t.Find("Instrução leitura"))Text("Instrução leitura","[E] LER O TEXTO COMPLETO",new Vector3(0,-.59f,-.05f),2.5f,.12f,t);
  }
  EditorSceneManager.MarkSceneDirty(target);EditorSceneManager.SaveScene(target);
 }
}
[InitializeOnLoad] public static class HistoryPanelSetup {
 static HistoryPanelSetup(){EditorApplication.delayCall+=()=>{const string b="Assets/Maracannes/Documentation/";if(!File.Exists(b+"history-build.request")||EditorApplication.isPlaying)return;AssetDatabase.DeleteAsset(b+"history-build.request");EditorSceneManager.SaveOpenScenes();var s=EditorSceneManager.OpenScene(AcervoTravel.Acervo,OpenSceneMode.Single);MaracannesBuilder.ConfigureHistory(s);if(!s.GetRootGameObjects().Any(g=>g.GetComponent<HistoryPanelAudit>())){var g=new GameObject("Verificacao_Paineis_Historicos");g.AddComponent<HistoryPanelAudit>();EditorSceneManager.SaveScene(s);}AssetDatabase.SaveAssets();};}
}
}
