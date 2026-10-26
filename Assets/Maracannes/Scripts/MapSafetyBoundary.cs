using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TeatroSESI;
namespace Maracannes {
public sealed class MapSafetyBoundary:MonoBehaviour {
 public Bounds allowedBounds;public float rescueBelowY=-.5f;
 IEnumerator Start(){for(int i=0;i<120;i++){var visitors=FindObjectsByType<VisitaCentroSESI>(FindObjectsInactive.Exclude);foreach(var v in visitors)if(!v.GetComponent<VisitorFallGuard>())v.gameObject.AddComponent<VisitorFallGuard>();if(visitors.Length>0)yield break;yield return null;}}
 void OnDrawGizmosSelected(){Gizmos.color=Color.cyan;Gizmos.DrawWireCube(allowedBounds.center,allowedBounds.size);}
}
public sealed class VisitorFallGuard:MonoBehaviour {
 public int Recoveries{get;private set;} public Vector3 LastSafePosition{get;private set;}
 MapSafetyBoundary boundary;CharacterController motor;VisitaCentroSESI visitor;ulong sceneHandle=ulong.MaxValue;float safeYaw;
 void Awake(){motor=GetComponent<CharacterController>();visitor=GetComponent<VisitaCentroSESI>();}
 void LateUpdate(){var scene=SceneManager.GetActiveScene();if(sceneHandle!=scene.handle.GetRawData()||!boundary){sceneHandle=scene.handle.GetRawData();boundary=FindObjectsByType<MapSafetyBoundary>(FindObjectsInactive.Exclude).Length>0?FindObjectsByType<MapSafetyBoundary>(FindObjectsInactive.Exclude)[0]:null;LastSafePosition=scene.path==AcervoTravel.Acervo?new Vector3(18.4f,.05f,-25.8f):new Vector3(0,.05f,-33);safeYaw=scene.path==AcervoTravel.Acervo?90:0;}if(!boundary)return;var travel=GetComponent<AcervoTravel>();if(travel&&travel.Busy)return;
 var p=transform.position;var b=boundary.allowedBounds;if(p.y<boundary.rescueBelowY||p.x<b.min.x-1||p.x>b.max.x+1||p.z<b.min.z-1||p.z>b.max.z+1){bool active=motor.enabled;motor.enabled=false;transform.SetPositionAndRotation(LastSafePosition,Quaternion.Euler(0,safeYaw,0));visitor.ResetFallVelocity();motor.enabled=active;Recoveries++;return;}
 if(motor.enabled&&motor.isGrounded&&p.y>-.3f&&p.x>b.min.x+.45f&&p.x<b.max.x-.45f&&p.z>b.min.z+.45f&&p.z<b.max.z-.45f){LastSafePosition=p+Vector3.up*.04f;safeYaw=transform.eulerAngles.y;}}
}
}


