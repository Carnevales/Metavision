using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TeatroSESI;
namespace Maracannes {
public sealed class MapSafetyAudit:MonoBehaviour {
 IEnumerator Start(){const string b="Assets/Maracannes/Documentation/";if(!File.Exists(b+"colisoes-stage.request"))yield break;string stage=File.ReadAllText(b+"colisoes-stage.request").Trim();if(stage!="1"&&stage!="2")yield break;yield return new WaitForSeconds(1);var boundary=GetComponent<MapSafetyBoundary>();var v=FindAnyObjectByType<VisitaCentroSESI>();var cc=v.GetComponent<CharacterController>();var guard=v.GetComponent<VisitorFallGuard>();bool ok=guard!=null;var report=new List<string>();var bounds=boundary.allowedBounds;v.enabled=false;guard.enabled=false;Vector3 original=v.transform.position;var mid=bounds.center;mid.y=.05f;
 var starts=new[]{new Vector3(bounds.min.x+1,.05f,mid.z),new Vector3(bounds.max.x-1,.05f,mid.z),new Vector3(mid.x,.05f,bounds.min.z+1),new Vector3(mid.x,.05f,bounds.max.z-1)};var directions=new[]{Vector3.left,Vector3.right,Vector3.back,Vector3.forward};
 for(int i=0;i<4;i++){cc.enabled=false;v.transform.position=starts[i];cc.enabled=true;var hit=cc.Move(directions[i]*4);var p=v.transform.position;bool pass=p.x>=bounds.min.x&&p.x<=bounds.max.x&&p.z>=bounds.min.z&&p.z<=bounds.max.z&&(hit&CollisionFlags.Sides)!=0;ok&=pass;report.Add((pass?"PASS ":"FAIL ")+"Barreira "+i+" contém CharacterController: "+p);yield return null;}
 cc.enabled=false;v.transform.position=original;cc.enabled=true;guard.enabled=true;v.enabled=true;yield return null;int count=guard.Recoveries;cc.enabled=false;v.transform.position=new Vector3(original.x,-4,original.z);cc.enabled=true;yield return null;yield return null;bool rescued=guard.Recoveries>count&&v.transform.position.y>-.3f;ok&=rescued;report.Add((rescued?"PASS ":"FAIL ")+"Queda simulada abaixo do mapa retorna ao piso seguro: "+v.transform.position);
 var floors=GetComponentsInChildren<BoxCollider>();bool solid=floors.Length==5&&floors.All(c=>c.enabled&&!c.isTrigger);ok&=solid;report.Add((solid?"PASS ":"FAIL ")+"Quatro barreiras sólidas e laje inferior ativas");string id=SceneManager.GetActiveScene().path==AcervoTravel.Acervo?"acervo":"teatro";File.WriteAllLines(b+"colisoes-"+id+"-runtime.txt",new[]{(ok?"PASS":"FAIL")+" · Play Mode: perímetro e recuperação de queda · "+id}.Concat(report));
 #if UNITY_EDITOR
 UnityEditor.EditorApplication.delayCall+=()=>UnityEditor.EditorApplication.isPlaying=false;
 #endif
 }
}
}
