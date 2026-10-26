using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TeatroSESI;
namespace Maracannes {
public sealed class Experience2024Audit:MonoBehaviour {
 const string B="Assets/Maracannes/Documentation/";VisitaCentroSESI visitor;AcervoTravel travel;bool failed;List<string> log=new List<string>();
 IEnumerator Start(){if(!File.Exists(B+"experience2024-test.request")||File.ReadAllText(B+"experience2024-test.request").Trim()!="run")yield break;File.WriteAllText(B+"experience2024-test.request","running");yield return new WaitForSeconds(1);travel=AcervoTravel.Instance;visitor=travel.GetComponent<VisitaCentroSESI>();var rooms=FindObjectsByType<MaracannesRoom>(FindObjectsInactive.Exclude);Check(rooms.Length==5,"Cinco salas, incluindo Experience 2024");var all=FindObjectsByType<MaracannesExhibit>(FindObjectsInactive.Exclude);Check(all.Length==66&&all.All(e=>e.GetComponentsInChildren<MuseumPoint>().Length==0),"66 cartazes preservados sem interação");var added=all.Where(e=>e.room==5).ToArray();Check(added.Length==9,"Nove cartazes recebidos expostos na sala 2024");Check(all.Count(e=>e.room==2)==30&&all.Count(e=>e.room==3)==10&&all.Count(e=>e.room==4)==17,"Acervo anterior preservado: 30 + 10 + 17 cartazes");
 foreach(var e in added){var image=e.GetComponentsInChildren<MeshRenderer>().First(r=>r.name=="Cartaz original integral");var tex=image.sharedMaterial.GetTexture("_BaseMap");Check(tex&&e.GetComponentsInChildren<MuseumPoint>().Length==0&&e.GetComponentsInChildren<TextMesh>().Length==0,e.catalogId+" · material local válido e cartaz sem interação/legenda");}
 Check(Physics.Raycast(new Vector3(70,1,-13),Vector3.up,4)&&Physics.Raycast(new Vector3(70,1,-13),Vector3.right,5),"Cobertura e parede externa fecham a nova sala");
 travel.SetPosition(new Vector3(51,.05f,-22),0);yield return Walk(51,-18);yield return Walk(57,-18);yield return Walk(57,-8);yield return Walk(51,-8);Check(rooms.First(r=>r.number==3).Contains(visitor.transform.position+Vector3.up),"Percurso atravessa Experience 2024 e chega a Brasil em Cena");
 foreach(var shot in new[]{new Vector3(58,.05f,-18),new Vector3(70,.05f,-6),new Vector3(67,.05f,-18)}){travel.SetPosition(shot,0);visitor.enabled=false;visitor.cameraVisita.transform.LookAt(new Vector3(66,1.7f,-12));yield return new WaitForEndOfFrame();ScreenCapture.CaptureScreenshot(B+"Evidencias/Experience2024_"+shot.x+"_"+shot.z+".png");yield return null;visitor.enabled=true;}
 File.WriteAllLines(B+"experience2024-runtime.txt",new[]{(failed?"FAIL":"PASS")+" · Teste real no Play Mode: acervo, cartazes, cobertura e circulação 02 → 2024 → 2025"}.Concat(log));File.WriteAllText(B+"experience2024-test.request","done");
 #if UNITY_EDITOR
 UnityEditor.EditorApplication.delayCall+=()=>UnityEditor.EditorApplication.isPlaying=false;
 #endif
 }
 void Check(bool ok,string s){log.Add((ok?"PASS ":"FAIL ")+s);if(!ok)failed=true;File.WriteAllLines(B+"experience2024-progresso.txt",log);}
 IEnumerator Walk(float x,float z){visitor.autoWalking=true;visitor.autoTarget=new Vector3(x,0,z);float end=Time.realtimeSinceStartup+12;while(Vector2.Distance(new Vector2(visitor.transform.position.x,visitor.transform.position.z),new Vector2(x,z))>.24f&&Time.realtimeSinceStartup<end)yield return null;visitor.autoWalking=false;Check(Vector2.Distance(new Vector2(visitor.transform.position.x,visitor.transform.position.z),new Vector2(x,z))<=.24f,"Caminhada com colisões até "+x+", "+z+" → "+visitor.transform.position);}
}
}

