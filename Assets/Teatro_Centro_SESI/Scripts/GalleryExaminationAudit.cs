using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maracannes;
namespace TeatroSESI {
public sealed class GalleryExaminationAudit:MonoBehaviour {
 const string B="Assets/Teatro_Centro_SESI/References/";
 IEnumerator Start(){
  if(!File.Exists(B+"gallery-test.request")||File.ReadAllText(B+"gallery-test.request").Trim()!="run")yield break;
  File.WriteAllText(B+"gallery-test.request","running");yield return null;
  var m=GetComponent<MuseumExperience>();var v=GetComponent<VisitaCentroSESI>();var t=GetComponent<AcervoTravel>();var log=new List<string>();
  foreach(var p in m.points.Where(p=>p&&p.title.StartsWith("02"))){t.SetPosition(p.transform.position+new Vector3(0,-p.transform.position.y+.05f,-1.1f),0);v.cameraVisita.transform.LookAt(p.transform);m.RefreshNearby();bool opened=m.Interact(p);m.OpenPhoto();log.Add((opened&&m.ExpandedPhoto&&m.BlockInput?"PASS":"FAIL")+" Foto do foyer abre e amplia na interface existente");yield return Shot("Foyer_Imagem_Ampliada");m.Close();log.Add((!m.BlockInput?"PASS":"FAIL")+" Fechar foto devolve controles");}
  var door=FindObjectsByType<AcervoDoor>(FindObjectsInactive.Exclude).First(d=>d.enterAcervo);t.SetPosition(new Vector3(12,.05f,-25.8f),90);t.UseDoor(door);float end=Time.realtimeSinceStartup+25;while(t.Busy&&Time.realtimeSinceStartup<end)yield return null;
  var posters=FindObjectsByType<MaracannesExhibit>(FindObjectsInactive.Exclude);log.Add((t.InAcervo&&posters.Length>0&&posters.All(e=>{var p=e.GetComponent<MuseumPoint>();var r=e.GetComponentsInChildren<Renderer>().FirstOrDefault(r=>r.name.StartsWith("Cartaz original")&&r.sharedMaterial&&r.sharedMaterial.mainTexture);return p&&p.imageOnly&&p.image&&(!r||p.image==r.sharedMaterial.mainTexture);})?"PASS":"FAIL")+" Todos os "+posters.Length+" cartazes têm imagem original para exame");
  if(posters.Length>0){var e=posters.First(e=>e.catalogId!="S4_002");var p=e.GetComponent<MuseumPoint>();t.SetPosition(e.inspectionPosition,0);v.cameraVisita.transform.LookAt(p.transform);m.RefreshNearby();bool opened=m.Interact(p);log.Add((opened&&m.ExpandedPhoto&&m.BlockInput?"PASS":"FAIL")+" Examinar cartaz abre imagem ampliada e libera cursor");yield return Shot("Cartaz_Examinado");m.Close();log.Add((!m.BlockInput&&!m.Active?"PASS":"FAIL")+" Fechar cartaz restaura exploração");}
  var exit=FindObjectsByType<AcervoDoor>(FindObjectsInactive.Exclude).FirstOrDefault(d=>!d.enterAcervo);if(exit){t.SetPosition(exit.transform.position+new Vector3(1.1f,-exit.transform.position.y+.05f,0),-90);t.UseDoor(exit);end=Time.realtimeSinceStartup+25;while(t.Busy&&Time.realtimeSinceStartup<end)yield return null;log.Add((!t.InAcervo&&!t.Busy?"PASS":"FAIL")+" Retorno ao teatro preservado");}
  File.WriteAllLines(B+"gallery-runtime.txt",log);File.WriteAllText(B+"gallery-test.request","done");
  #if UNITY_EDITOR
  UnityEditor.EditorApplication.delayCall+=()=>UnityEditor.EditorApplication.isPlaying=false;
  #endif
 }
 IEnumerator Shot(string name){yield return new WaitForEndOfFrame();Directory.CreateDirectory("Builds/Gallery_Verification");var tex=ScreenCapture.CaptureScreenshotAsTexture();File.WriteAllBytes("Builds/Gallery_Verification/"+name+".png",tex.EncodeToPNG());Destroy(tex);yield return null;}
}
}
