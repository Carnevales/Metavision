using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TeatroSESI;
namespace Maracannes {
public sealed class AcervoReturnAudit:MonoBehaviour {
 IEnumerator Start(){const string b="Assets/Maracannes/Documentation/";File.WriteAllText(b+"retorno-test.request","running");yield return new WaitForSecondsRealtime(1);var travel=GetComponent<AcervoTravel>();var museum=GetComponent<MuseumExperience>();var log=new List<string>();bool ok=Application.CanStreamedLevelBeLoaded(AcervoTravel.Theater);log.Add((ok?"PASS":"FAIL")+" Destino Teatro.unity disponível no build");var ticket=museum.points.First(p=>p&&p.action==MuseumAction.Ticket);travel.SetPosition(new Vector3(ticket.transform.position.x,.05f,ticket.transform.position.z-.8f),0);bool issued=museum.Interact(ticket)&&museum.IssueTicket();museum.Close();ok&=issued;log.Add((issued?"PASS":"FAIL")+" Ingresso emitido antes da visita ao acervo");
 foreach(bool entering in new[]{true,false}){var door=FindObjectsByType<AcervoDoor>(FindObjectsInactive.Exclude).First(d=>d.enterAcervo==entering);var position=door.transform.position-door.transform.forward*1.4f;position.y=.05f;travel.SetPosition(position,entering?90:-90);bool used=travel.UseDoor(door);float timeout=Time.realtimeSinceStartup+25;yield return null;while(travel.Busy&&Time.realtimeSinceStartup<timeout)yield return null;bool passed=used&&!travel.Busy&&travel.InAcervo==entering;ok&=passed;log.Add((passed?"PASS":"FAIL")+(entering?" Porta de entrada carrega o acervo":" Porta de saída retorna à cena Teatro"));yield return null;}
 bool state=museum.ticketIssued&&museum.issuedCount==1&&museum.enabled&&!museum.BlockInput&&FindObjectsByType<VisitaCentroSESI>(FindObjectsInactive.Exclude).Length==1&&GetComponent<CharacterController>().enabled;ok&=state;log.Add((state?"PASS":"FAIL")+" Um visitante, ingresso preservado, controles ativos e interface fechada");bool safe=Vector3.Distance(transform.position,new Vector3(12,.05f,-25.8f))<.4f;ok&=safe;log.Add((safe?"PASS":"FAIL")+" Retorno em posição segura no saguão: "+transform.position);File.WriteAllLines(b+"retorno-runtime.txt",new[]{(ok?"PASS":"FAIL")+" · Teste real no Play Mode: teatro → acervo → teatro"}.Concat(log));File.WriteAllText(b+"retorno-test.request","done");
 #if UNITY_EDITOR
 UnityEditor.EditorApplication.delayCall+=()=>UnityEditor.EditorApplication.isPlaying=false;
 #endif
 }
}
}
