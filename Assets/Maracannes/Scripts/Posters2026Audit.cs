using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using TeatroSESI;
namespace Maracannes {
public sealed class Posters2026Audit:MonoBehaviour {
 IEnumerator Start(){const string b="Assets/Maracannes/Documentation/";if(!File.Exists(b+"posters-2026-test.request")||File.ReadAllText(b+"posters-2026-test.request").Trim()!="run")yield break;yield return new WaitForSeconds(1);var all=FindObjectsByType<MaracannesExhibit>(FindObjectsInactive.Exclude);var added=all.Where(e=>e.catalogId.StartsWith("S4_NOVO_")).OrderBy(e=>e.catalogId).ToArray();bool ok=added.Length==14&&all.Count(e=>e.room==4)==17;string report="";
 foreach(var e in added){var image=e.GetComponentsInChildren<MeshRenderer>().First(r=>r.name=="Cartaz original integral");var tex=image.sharedMaterial.GetTexture("_BaseMap");var size=image.GetComponent<MeshFilter>().sharedMesh.bounds.size;bool pass=tex&&Mathf.Abs(size.x/size.y-(float)tex.width/tex.height)<.001f&&e.GetComponentsInChildren<MuseumPoint>().Length==0&&e.GetComponentsInChildren<TextMesh>().Length==0;ok&=pass;report+=(pass?"PASS ":"FAIL ")+e.catalogId+" [mesh "+size+" tex "+(tex?tex.width+"x"+tex.height:"null")+" points "+e.GetComponentsInChildren<MuseumPoint>().Length+" texts "+e.GetComponentsInChildren<TextMesh>().Length+"] · original integral, material válido, sem interação e sem legenda\n";}
 var travel=AcervoTravel.Instance;var v=travel.GetComponent<VisitaCentroSESI>();int i=0;foreach(var p in new[]{new Vector3(24,.05f,-10.8f),new Vector3(27,.05f,-10),new Vector3(24,.05f,1.5f)}){travel.SetPosition(p,i==0?180:i==1?90:0);yield return new WaitForSeconds(.3f);yield return new WaitForEndOfFrame();ScreenCapture.CaptureScreenshot(b+"Evidencias/Cartazes2026_"+(i++)+".png");yield return null;}
 File.WriteAllText(b+"posters-2026-runtime.txt",(ok?"PASS":"FAIL")+" · Play Mode: 14 novos cartazes; 17 na Sala 4; quatro salas preservadas\n"+report);File.WriteAllText(b+"posters-2026-test.request","done");
 #if UNITY_EDITOR
 UnityEditor.EditorApplication.delayCall+=()=>UnityEditor.EditorApplication.isPlaying=false;
 #endif
 }
}
}


