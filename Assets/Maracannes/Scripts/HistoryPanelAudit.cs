using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using TeatroSESI;
namespace Maracannes {
public sealed class HistoryPanelAudit:MonoBehaviour {
 const string B="Assets/Maracannes/Documentation/";
 IEnumerator Start(){if(!File.Exists(B+"history-test.request"))yield break;string mode=File.ReadAllText(B+"history-test.request").Trim();if(mode!="run"&&mode!="manual")yield break;yield return new WaitForSeconds(1);var t=AcervoTravel.Instance;var m=t.GetComponent<MuseumExperience>();var panels=m.points.OrderBy(p=>p.name).ToArray();if(mode=="manual"){t.SetPosition(new Vector3(20,.05f,-30.6f),180);File.WriteAllText(B+"history-test.request","manual-ready");yield break;}
 bool ok=panels.Length==3&&m.enabled;string report="";foreach(var p in panels){t.SetPosition(new Vector3(p.transform.position.x,.05f,-30.8f),180);yield return null;bool opened=m.Interact(p);bool pass=opened&&m.Active==p&&m.BlockInput&&p.body.Length>400;ok&=pass;report+=(pass?"PASS ":"FAIL ")+p.title+" · "+p.body.Length+" caracteres\n";yield return new WaitForEndOfFrame();ScreenCapture.CaptureScreenshot(B+"Evidencias/Historia_"+p.name.Substring(p.name.Length-1)+".png");m.Close();ok&=!m.BlockInput;yield return null;}
 var posters=FindObjectsByType<MaracannesExhibit>(FindObjectsInactive.Exclude);bool unchanged=posters.Length==43&&posters.All(p=>p.GetComponentsInChildren<MuseumPoint>(true).Length==0);ok&=unchanged;report+=(unchanged?"PASS":"FAIL")+" 43 cartazes preservados sem interação\n";File.WriteAllText(B+"history-runtime.txt",(ok?"PASS":"FAIL")+" · Teste no Play Mode: três textos completos, abertura, bloqueio temporário e fechamento\n"+report);File.WriteAllText(B+"history-test.request","done");
 #if UNITY_EDITOR
 UnityEditor.EditorApplication.delayCall+=()=>UnityEditor.EditorApplication.isPlaying=false;
 #endif
 }
}
}
