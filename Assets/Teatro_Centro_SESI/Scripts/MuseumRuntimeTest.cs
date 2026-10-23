using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeatroSESI {
public sealed class MuseumRuntimeTest : MonoBehaviour {
 const string B="Assets/Teatro_Centro_SESI/";List<string> log=new List<string>();VisitaCentroSESI visitor;MuseumExperience museum;bool failed;float started;
 IEnumerator Start(){
  if(!File.Exists(B+"References/museum-test.request"))yield break;
  string mode=File.ReadAllText(B+"References/museum-test.request").Trim();if(mode!="run"&&mode!="manual"&&mode!="accessmanual")yield break;
  File.WriteAllText(B+"References/museum-test.request","running");visitor=GetComponent<VisitaCentroSESI>();museum=GetComponent<MuseumExperience>();started=Time.realtimeSinceStartup;yield return null;
  Check(transform.position.z<-30,"Início na área externa");
  if(mode=="manual"){yield return Walk(0,-27);yield return Walk(0,-26.2f);yield return Walk(-2.4f,-25.9f);File.WriteAllText(B+"References/museum-test.request","manual-ready");yield break;}
  yield return Walk(0,-27);yield return Walk(0,-22);yield return Walk(0,-16.6f);
  var gate=museum.points.First(p=>p.action==MuseumAction.Gate);Check(!museum.Interact(gate)&&!museum.ticketValidated,"Sem ingresso: interação recusa entrada");Check(museum.AccessDenied&&museum.BlockInput,"Aviso de acesso abre e libera cursor");yield return Shot("Polimento_UI_Sem_Ingresso.png");if(mode=="accessmanual"){File.WriteAllText(B+"References/museum-test.request","manual-ready");yield break;}museum.GuideToTicket();Check(museum.GuidingToTicket&&!museum.BlockInput,"Botão de orientação fecha aviso e devolve controles");
  visitor.autoTarget=new Vector3(0,0,-14.5f);visitor.autoWalking=true;yield return new WaitForSeconds(2);visitor.autoWalking=false;Check(transform.position.z<-15.5f,"Porta fechada bloqueia CharacterController");
  yield return Walk(0,-22);yield return Walk(0,-26.2f);yield return Walk(-2.4f,-25.9f);
  var ticket=museum.points.First(p=>p.action==MuseumAction.Ticket);Check(museum.Interact(ticket),"Bilheteria abre por proximidade");museum.Close();Check(!museum.ticketIssued,"Cancelar não emite ingresso");
  Check(museum.Interact(ticket),"Bilheteria reabre");Check(museum.IssueTicket(),"Retirar ingresso emite ingresso digital");Check(!museum.IssueTicket()&&museum.issuedCount==1,"Emissão duplicada impedida");Check(!museum.GuidingToTicket,"Emissão encerra orientação à bilheteria");yield return Shot("Museu_UI_Ingresso.png");museum.Close();
  yield return Walk(-.4f,-25.9f);yield return Walk(2.35f,-22.8f);yield return Exhibit("01");
  yield return Walk(-4.5f,-22.3f);visitor.autoWalking=true;visitor.autoTarget=new Vector3(-7,0,-22.3f);yield return new WaitForSeconds(1.5f);visitor.autoWalking=false;Check(transform.position.x>-5.4f,"Parede do hall impede atravessamento");
  yield return Walk(0,-22.5f);yield return Walk(2.5f,-24.3f);yield return Walk(5,-24.3f);yield return Walk(5,-22.8f);yield return Exhibit("02");yield return Walk(8,-22.8f);yield return Exhibit("03");var fourth=museum.points.First(p=>p.action==MuseumAction.Exhibit&&p.title.StartsWith("04"));if(fourth.transform.position.z> -24)yield return Walk(fourth.transform.position.x,-23.3f);else yield return Walk(12.05f,-24.95f);yield return Exhibit("04");
  yield return Walk(5,-24.3f);yield return Walk(2.5f,-24.3f);yield return Walk(0,-22);yield return Walk(0,-19);yield return Walk(0,-16.6f);
  Check(museum.Interact(gate)&&museum.ticketValidated,"Ingresso validado na porta");yield return new WaitForSeconds(1.5f);Check(museum.DoorProgress>=.98f&&!museum.gateBlocker.enabled,"Folhas animadas e passagem liberada");
  yield return Walk(0,-14.8f);yield return Walk(0,-12);yield return Walk(0,-8);yield return Walk(0,-4);yield return Shot("Museu_UI_Plateia.png");yield return Walk(0,-2.1f);yield return Walk(0,0);
  Check(transform.position.y>.9f&&transform.position.y<1.3f,"Chegada ao palco sem cair pelo piso");
  museum.Close();yield return null;Finish();
 }
 IEnumerator Walk(float x,float z){if(failed)yield break;visitor.autoTarget=new Vector3(x,0,z);visitor.autoWalking=true;float end=Time.realtimeSinceStartup+18;while(new Vector2(transform.position.x-x,transform.position.z-z).magnitude>.22f&&Time.realtimeSinceStartup<end){if(transform.position.y<-.2f){Check(false,"Queda no percurso");break;}yield return null;}visitor.autoWalking=false;bool reached=new Vector2(transform.position.x-x,transform.position.z-z).magnitude<=.22f;Check(reached,"Percurso até "+x+", "+z+"; posição "+transform.position.ToString("F2"));if(!reached)Finish();}
 IEnumerator Exhibit(string prefix){if(failed)yield break;var point=museum.points.First(p=>p.action==MuseumAction.Exhibit&&p.title.StartsWith(prefix));Check(museum.Interact(point)&&museum.Active==point,"Painel "+prefix+" abre com conteúdo e fonte");yield return Shot("Museu_UI_Painel_"+prefix+".png");museum.Close();Check(!museum.BlockInput,"Fechar painel devolve movimento");}
 IEnumerator Shot(string name){yield return new WaitForEndOfFrame();ScreenCapture.CaptureScreenshot(B+"Lighting/"+name);yield return null;}
 void Check(bool ok,string value){log.Add((ok?"PASS ":"FAIL ")+value);if(!ok)failed=true;}
 void Finish(){if(visitor)visitor.autoWalking=false;File.WriteAllLines(B+"References/museu-runtime.txt",new[]{(failed?"FAIL":"PASS")+" — fluxo do museu com CharacterController, interações e estado real; "+(Time.realtimeSinceStartup-started).ToString("F1")+" s"}.Concat(log));File.WriteAllText(B+"References/museum-test.request","done");
 #if UNITY_EDITOR
 UnityEditor.EditorApplication.delayCall+=()=>UnityEditor.EditorApplication.isPlaying=false;
 #endif
 }
}
}
