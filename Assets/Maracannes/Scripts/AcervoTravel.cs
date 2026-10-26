using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TeatroSESI;
namespace Maracannes {
[DefaultExecutionOrder(-150)] public sealed class AcervoTravel:MonoBehaviour {
 public const string Theater="Assets/Teatro_Centro_SESI/Scenes/Teatro.unity",Acervo="Assets/Maracannes/Scenes/Maracannes_Acervo.unity";
 public static AcervoTravel Instance {get;private set;}
 public bool Busy {get;private set;} public AcervoDoor Nearby {get;private set;}
 public bool InAcervo=>SceneManager.GetSceneByPath(Acervo).isLoaded;
 VisitaCentroSESI visitor;MuseumExperience museum;float fade;bool browsingPause;int lastEscapeFrame=-1;
 void Awake(){if(Instance&&Instance!=this){enabled=false;return;}Instance=this;visitor=GetComponent<VisitaCentroSESI>();museum=GetComponent<MuseumExperience>();}
 void Update(){if(Instance!=this||Busy)return;Refresh();var k=Keyboard.current;if(InAcervo&&k!=null&&k.escapeKey.wasPressedThisFrame){TogglePause();}
 if(k!=null&&k.eKey.wasPressedThisFrame&&!browsingPause&&!museum.BlockInput){if(Nearby)UseDoor(Nearby);else if(InAcervo&&museum.Nearby)museum.Interact(museum.Nearby);}
 }
 void TogglePause(){if(lastEscapeFrame==Time.frameCount)return;lastEscapeFrame=Time.frameCount;if(museum.Active){museum.Close();return;}browsingPause=!browsingPause;visitor.enabled=!browsingPause;Cursor.lockState=browsingPause?CursorLockMode.None:CursorLockMode.Locked;Cursor.visible=browsingPause;}
 public void Refresh(){Nearby=null;float best=float.MaxValue;foreach(var d in Object.FindObjectsByType<AcervoDoor>(FindObjectsInactive.Exclude)){float distance=Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z),new Vector3(d.transform.position.x,0,d.transform.position.z));if(distance<d.radius&&distance<best){best=distance;Nearby=d;}}}
 public bool UseDoor(AcervoDoor door){Refresh();if(Busy||door!=Nearby)return false;StartCoroutine(Travel(door.enterAcervo));return true;}
 IEnumerator Travel(bool entering){string path=entering?Acervo:Theater;if(!Application.CanStreamedLevelBeLoaded(path)){Debug.LogError("Cena indisponível nas configurações de build: "+path);yield break;}Busy=true;Nearby=null;visitor.autoWalking=false;museum.Close();museum.enabled=false;visitor.enabled=false;transform.SetParent(null);DontDestroyOnLoad(gameObject);for(float t=0;t<.35f;t+=Time.unscaledDeltaTime){fade=Mathf.Clamp01(t/.35f);yield return null;}fade=1;
 var op=SceneManager.LoadSceneAsync(path,LoadSceneMode.Single);if(op==null){Debug.LogError("Cena indisponível: "+path);museum.enabled=true;visitor.enabled=true;fade=0;Busy=false;yield break;}yield return op;yield return null;
 if(!entering){var fresh=Object.FindObjectsByType<VisitaCentroSESI>(FindObjectsInactive.Exclude).FirstOrDefault(v=>v!=visitor);if(fresh){var freshMuseum=fresh.GetComponent<MuseumExperience>();museum.points=freshMuseum.points;museum.doorLeaves=freshMuseum.doorLeaves;museum.gateBlocker=freshMuseum.gateBlocker;Destroy(fresh.gameObject);}museum.enabled=true;if(museum.ticketValidated){for(int i=0;i<museum.doorLeaves.Length;i++)if(museum.doorLeaves[i])museum.doorLeaves[i].localRotation=Quaternion.Euler(0,i==0?-82:82,0);if(museum.gateBlocker)museum.gateBlocker.enabled=false;}}
 else PrepareHistory();
 SetPosition(entering?new Vector3(18.4f,.05f,-25.8f):new Vector3(12,.05f,-25.8f),entering?90:-90);browsingPause=false;visitor.enabled=true;Cursor.lockState=CursorLockMode.Locked;Cursor.visible=false;
 for(float t=0;t<.35f;t+=Time.unscaledDeltaTime){fade=1-Mathf.Clamp01(t/.35f);yield return null;}fade=0;Busy=false;Cursor.lockState=CursorLockMode.Locked;Cursor.visible=false;
 }
 public void SetPosition(Vector3 p,float yaw){var cc=GetComponent<CharacterController>();cc.enabled=false;transform.SetPositionAndRotation(p,Quaternion.Euler(0,yaw,0));visitor.cameraVisita.transform.localRotation=Quaternion.identity;cc.enabled=true;}
 public void PrepareHistory(){museum.points=Object.FindObjectsByType<MuseumPoint>(FindObjectsInactive.Exclude).Where(p=>p.gameObject.scene.path==Acervo&&p.name.StartsWith("Marco histórico ")).ToArray();museum.Close();museum.enabled=true;}
 public void PrepareMuseum(){PrepareHistory();SetPosition(new Vector3(18.4f,.05f,-25.8f),90);}
 void OnGUI(){if(Instance!=this)return;
 if(Event.current.type==EventType.KeyDown){if(Event.current.keyCode==KeyCode.E&&Nearby&&!Busy&&!browsingPause){UseDoor(Nearby);Event.current.Use();}else if(Event.current.keyCode==KeyCode.E&&InAcervo&&museum.Nearby&&!Busy&&!browsingPause&&!museum.BlockInput){museum.Interact(museum.Nearby);Event.current.Use();}else if(Event.current.keyCode==KeyCode.Escape&&InAcervo&&!Busy){TogglePause();Event.current.Use();}}
 int old=GUI.depth;GUI.depth=-10000;
 if(InAcervo&&museum.Nearby&&!Nearby&&!Busy&&!browsingPause&&!museum.BlockInput){var hint=new GUIStyle(GUI.skin.box){fontSize=Mathf.Max(16,Screen.height/42),alignment=TextAnchor.MiddleCenter};GUI.Box(new Rect(Screen.width*.18f,Screen.height*.84f,Screen.width*.64f,Screen.height*.07f),"[E] LER O TEXTO COMPLETO",hint);}
 if(Nearby&&!Busy&&!browsingPause&&!museum.BlockInput){var style=new GUIStyle(GUI.skin.box){fontSize=Mathf.Max(16,Screen.height/42),alignment=TextAnchor.MiddleCenter};GUI.Box(new Rect(Screen.width*.18f,Screen.height*.84f,Screen.width*.64f,Screen.height*.07f),Nearby.Prompt,style);}
 if(InAcervo){var style=new GUIStyle(GUI.skin.box){fontSize=Mathf.Max(14,Screen.height/55),alignment=TextAnchor.MiddleCenter};GUI.Box(new Rect(Screen.width*.22f,Screen.height*.94f,Screen.width*.56f,Screen.height*.045f),museum.Active?"Esc · fechar texto":browsingPause?"VISITA EM PAUSA · Esc para continuar":"WASD · caminhar    Mouse · olhar    Esc · pausa",style);}
 if(fade>0){var c=GUI.color;GUI.color=new Color(0,0,0,fade);GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),Texture2D.whiteTexture);GUI.color=c;}GUI.depth=old;
 }
 void OnDestroy(){if(Instance==this)Instance=null;}
}
}
