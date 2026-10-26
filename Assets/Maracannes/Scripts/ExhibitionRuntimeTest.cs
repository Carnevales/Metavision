using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeatroSESI;
namespace Maracannes {
public sealed class ExhibitionRuntimeTest:MonoBehaviour {
 const string B="Assets/Maracannes/Documentation/";readonly List<string> log=new List<string>();VisitaCentroSESI v;MuseumExperience m;bool failed,running;float started;readonly List<float> frames=new List<float>();
 void Update(){if(running&&Time.realtimeSinceStartup-started>5)frames.Add(Time.unscaledDeltaTime);}
 IEnumerator Start(){
  if(!File.Exists(B+"test.request"))yield break;string mode=File.ReadAllText(B+"test.request").Trim();if(mode!="run"&&mode!="catalog"&&mode!="manual")yield break;
  File.WriteAllText(B+"test.request","running");started=Time.realtimeSinceStartup;running=true;
  for(int i=0;i<300;i++){v=FindAnyObjectByType<VisitaCentroSESI>();if(v){m=v.GetComponent<MuseumExperience>();if(m&&m.points!=null&&m.points.Count(p=>p&&p.GetComponentInParent<MaracannesExhibit>())==43)break;}yield return null;}
  Check(v&&m,"Controlador e interface existentes reutilizados");if(failed){Finish();yield break;}
  Check(v.transform.position.z<-30,"Visitante começa no exterior");
  Check(UnityEngine.SceneManagement.SceneManager.sceneCount==3,"Inicialização com três cenas carregadas e sem duplicação");
  Check(Resources.FindObjectsOfTypeAll<VisitaCentroSESI>().Count(x=>x.gameObject.scene.isLoaded)==1,"Um único controlador de visitante");
  Check(m.points.Count(p=>p&&p.GetComponentInParent<MaracannesExhibit>())==43,"43 pontos do acervo registrados na interface existente");
  if(mode=="manual"){var e=GetComponentsInChildren<MaracannesExhibit>().First(x=>x.catalogId=="S2_001");var motor=v.GetComponent<CharacterController>();motor.enabled=false;v.transform.position=e.inspectionPosition+Vector3.up*.05f;v.transform.rotation=e.transform.rotation;v.cameraVisita.transform.localRotation=Quaternion.identity;motor.enabled=true;running=false;File.WriteAllText(B+"test.request","manual-ready");yield break;}
  if(mode=="run"){
  yield return Walk(0,-27);yield return Walk(0,-22);yield return Walk(0,-16.6f);
  var closedGate=m.points.First(p=>p&&p.action==MuseumAction.Gate);Check(!m.Interact(closedGate)&&m.AccessDenied&&m.BlockInput,"Sem ingresso: aviso de acesso e bloqueio preservados");yield return Shot("UI_Acesso_Sem_Ingresso.png");m.Close();yield return Walk(0,-22);yield return Walk(0,-26.2f);yield return Walk(-2.4f,-25.9f);
  var ticket=m.points.First(p=>p&&p.action==MuseumAction.Ticket);Check(m.Interact(ticket),"Bilheteria existente abre");Check(m.IssueTicket()&&!m.IssueTicket()&&m.issuedCount==1,"Ingresso único emitido e duplicação impedida");m.Close();
  yield return Walk(0,-26.2f);yield return Walk(2.5f,-24.3f);yield return Walk(5,-24.3f);yield return Walk(5,-23.8f);yield return Walk(12,-23.8f);yield return Walk(12,-25.8f);yield return Walk(18,-25.8f);
  yield return Shot("Percurso_Sala1.png");yield return Walk(18,-30);v.autoTarget=new Vector3(18,0,-34);v.autoWalking=true;yield return new WaitForSeconds(2);v.autoWalking=false;Check(v.transform.position.z> -31.7f,"Parede da exposição impede atravessamento");yield return Walk(18,-25.8f);yield return Walk(28,-25.8f);yield return Walk(35,-25.8f);yield return Shot("Percurso_Sala2.png");
  yield return Walk(36,-17);yield return Walk(43,-15);yield return Walk(43,-9.5f);yield return Shot("Percurso_Sala3.png");yield return Walk(43,-7);yield return Walk(35,-7);yield return Walk(35,-4);yield return Walk(29,-4);yield return Shot("Percurso_Sala4.png");
  yield return Walk(29,-15);yield return Walk(24,-15);yield return Walk(24,-20);yield return Walk(18,-20);yield return Walk(18,-25.8f);yield return Walk(12,-25.8f);yield return Walk(12,-23.8f);yield return Walk(5,-23.8f);yield return Walk(5,-24.3f);yield return Walk(2.5f,-24.3f);yield return Walk(0,-22);yield return Walk(0,-16.6f);
  var gate=m.points.First(p=>p&&p.action==MuseumAction.Gate);Check(m.Interact(gate)&&m.ticketValidated,"Ingresso validado na entrada original");yield return new WaitForSeconds(1.5f);Check(!m.gateBlocker.enabled,"Porta original abre");yield return Walk(0,-8);yield return Walk(0,-4);yield return Walk(0,0);Check(v.transform.position.y>.9f,"Palco original acessível");
  }
  if(!failed){
   var cc=v.GetComponent<CharacterController>();
   foreach(var p in GetComponentsInChildren<MuseumPoint>().Where(p=>!p.GetComponentInParent<MaracannesExhibit>())){cc.enabled=false;v.transform.position=p.transform.position+Vector3.forward*(p.transform.position.z < -30?1.5f:-1.5f);v.transform.position=new Vector3(v.transform.position.x,.05f,v.transform.position.z);cc.enabled=true;yield return null;Check(m.Interact(p),"Painel histórico abre: "+p.title);m.Close();}
   var exhibits=GetComponentsInChildren<MaracannesExhibit>();Check(exhibits.Length==43&&GetComponentsInChildren<MaracannesRoom>().Length==4,"43 materiais em exatamente quatro salas");
   foreach(var e in exhibits){cc.enabled=false;v.transform.position=e.inspectionPosition+Vector3.up*.05f;cc.enabled=true;yield return null;var p=e.GetComponentInChildren<MuseumPoint>();Check(p&&p.image&&m.Interact(p),"Abre cartaz por proximidade: "+e.catalogId);if(m.Active){m.OpenPhoto();Check(m.ExpandedPhoto,"Ampliação integral: "+e.catalogId);if(e.catalogId=="S4_002")yield return Shot("UI_Panorama.png");m.ClosePhoto();m.Close();Check(!m.BlockInput,"Fechar devolve controles: "+e.catalogId);}}
  }
  Finish();
 }
 IEnumerator Walk(float x,float z){if(failed)yield break;v.autoWalking=true;v.autoTarget=new Vector3(x,0,z);float deadline=Time.realtimeSinceStartup+24;while(Vector2.Distance(new Vector2(v.transform.position.x,v.transform.position.z),new Vector2(x,z))>.24f&&Time.realtimeSinceStartup<deadline){yield return null;}v.autoWalking=false;Check(Vector2.Distance(new Vector2(v.transform.position.x,v.transform.position.z),new Vector2(x,z))<=.24f&&v.transform.position.y>-.2f,"Caminhada com colisões até "+x+", "+z+" → "+v.transform.position.ToString("F2"));}
 IEnumerator Shot(string name){v.cameraVisita.transform.localRotation=Quaternion.identity;yield return new WaitForEndOfFrame();ScreenCapture.CaptureScreenshot(B+"Evidencias/"+name);yield return null;}
 void Check(bool ok,string message){log.Add((ok?"PASS ":"FAIL ")+message);if(!ok)failed=true;File.WriteAllLines(B+"runtime-progress.txt",log);}
 void Finish(){running=false;if(frames.Count>0){var sorted=frames.OrderBy(f=>f).ToArray();log.Add("MEDIÇÃO Editor: "+(1/frames.Average()).ToString("F1")+" FPS médios; p95 "+(sorted[(int)((sorted.Length-1)*.95f)]*1000).ToString("F1")+" ms; "+frames.Count+" quadros. Não é benchmark de build.");}if(v)v.autoWalking=false;File.WriteAllLines(B+"runtime.txt",new[]{(failed?"FAIL":"PASS")+" Teste real no Editor · "+(Time.realtimeSinceStartup-started).ToString("F1")+" segundos. Percurso contínuo; inspeção individual dos cartazes com reposicionamento explícito de teste."}.Concat(log));File.WriteAllText(B+"test.request","done");
 #if UNITY_EDITOR
 UnityEditor.EditorApplication.delayCall+=()=>UnityEditor.EditorApplication.isPlaying=false;
 #endif
 }
}
}

