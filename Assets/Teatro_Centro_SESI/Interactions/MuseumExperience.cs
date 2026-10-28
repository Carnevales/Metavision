using UnityEngine;
using UnityEngine.InputSystem;
namespace TeatroSESI {
public sealed class MuseumExperience : MonoBehaviour {
 public VisitaCentroSESI visitor;
 public MuseumPoint[] points;
 public Transform[] doorLeaves;
 public BoxCollider gateBlocker;
 public bool ticketIssued, ticketValidated;
 public int issuedCount;
 public float doorSeconds=1.2f;
 public bool ExpandedPhoto {get;private set;}
 Vector2 photoScroll;float photoZoom=1;
 public void OpenPhoto(){if(Active&&Active.image){ExpandedPhoto=true;photoZoom=1;photoScroll=Vector2.zero;}}
 public void ClosePhoto(){ExpandedPhoto=false;}
 public bool Paused {get; private set;}
 public MuseumPoint Active {get; private set;}
 public MuseumPoint Nearby {get; private set;}
 public bool AccessDenied {get;private set;}
 public bool GuidingToTicket {get;private set;}
 public bool BlockInput => Paused||Active||AccessDenied;
 public float DoorProgress {get; private set;}
 public string Message {get; private set;}
 float messageUntil;Vector2 exhibitScroll;
 int lastKeyFrame=-1;
 GUIStyle heading,body,small,button,badge,centerHeading,centerBody,centerSmall,centerBadge;
 Texture2D background,accent,card;
 void Start(){visitor=GetComponent<VisitaCentroSESI>();Close();}
 void Update(){
  var acervo=GetComponent<Maracannes.AcervoTravel>();if(acervo&&acervo.InAcervo){RefreshNearby();return;}
  var k=Keyboard.current;
  if(k!=null&&k.escapeKey.wasPressedThisFrame)HandleKey(KeyCode.Escape);
  RefreshNearby();
  if(k!=null&&k.eKey.wasPressedThisFrame)HandleKey(KeyCode.E);
  if(ticketValidated&&DoorProgress<1){DoorProgress=Mathf.Min(1,DoorProgress+Time.deltaTime/doorSeconds);float eased=Mathf.SmoothStep(0,1,DoorProgress);for(int i=0;i<doorLeaves.Length;i++)if(doorLeaves[i])doorLeaves[i].localRotation=Quaternion.Euler(0,(i==0?-82:82)*eased,0);if(DoorProgress>=.98f)gateBlocker.enabled=false;}
 }
 void HandleKey(KeyCode key){if(lastKeyFrame==Time.frameCount)return;lastKeyFrame=Time.frameCount;if(key==KeyCode.Escape){if(ExpandedPhoto){if(Active&&Active.imageOnly)Close();else ClosePhoto();}else if(Active||AccessDenied)Close();else{Paused=!Paused;SetCursor();}}else if(key==KeyCode.E&&!BlockInput){RefreshNearby();if(Nearby)Interact(Nearby);}}
 public void RefreshNearby(){Nearby=null;float best=float.MaxValue;if(points==null)return;foreach(var p in points){if(!p||!p.gameObject.activeInHierarchy)continue;var delta=p.transform.position-transform.position;delta.y=0;float d=delta.magnitude;if(p.imageOnly&&visitor&&visitor.cameraVisita){var aim=p.transform.position-visitor.cameraVisita.transform.position;if(Vector3.Dot(visitor.cameraVisita.transform.forward,aim.normalized)<.78f)continue;}if(d<p.radius&&d<best){Nearby=p;best=d;}}}
 public bool Interact(MuseumPoint point){
  RefreshNearby();if(BlockInput||point!=Nearby)return false;
  if(point.action==MuseumAction.Gate){if(!ticketIssued){AccessDenied=true;SetCursor();return false;}ticketValidated=true;Toast("Ingresso validado. Bem-vindo ao teatro!");return true;}
  exhibitScroll=Vector2.zero;Active=point;if(point.imageOnly)OpenPhoto();SetCursor();return true;
 }
 public bool IssueTicket(){if(!Active||Active.action!=MuseumAction.Ticket||ticketIssued)return false;ticketIssued=true;GuidingToTicket=false;issuedCount++;Toast("Ingresso emitido com sucesso!");return true;}
 public void Close(){ExpandedPhoto=false;Active=null;AccessDenied=false;Paused=false;SetCursor();}
 public void GuideToTicket(){Close();GuidingToTicket=true;}
 public void Resume(){Paused=false;SetCursor();}
 void SetCursor(){Cursor.lockState=BlockInput?CursorLockMode.None:CursorLockMode.Locked;Cursor.visible=BlockInput;}
 void Toast(string text){Message=text;messageUntil=Time.unscaledTime+4;}
 void Styles(){if(heading!=null)return;background=Solid(new Color(.035f,.05f,.065f,.96f));accent=Solid(new Color(.97f,.38f,.09f));card=Solid(new Color(.96f,.95f,.91f));heading=new GUIStyle(GUI.skin.label){fontSize=34,fontStyle=FontStyle.Bold,wordWrap=true,normal={textColor=new Color(.1f,.15f,.19f)}};body=new GUIStyle(GUI.skin.label){fontSize=23,wordWrap=true,normal={textColor=new Color(.17f,.22f,.26f)}};small=new GUIStyle(body){fontSize=17};button=new GUIStyle(GUI.skin.button){fontSize=23,padding=new RectOffset(15,15,10,10)};badge=new GUIStyle(GUI.skin.label){fontSize=20,wordWrap=true,normal={textColor=Color.white}};foreach(var style in new[]{heading,body,small,badge}){style.hover.textColor=style.normal.textColor;style.active.textColor=style.normal.textColor;style.focused.textColor=style.normal.textColor;}button.normal.background=background;button.hover.background=accent;button.active.background=accent;button.focused.background=background;button.normal.textColor=button.hover.textColor=button.active.textColor=button.focused.textColor=Color.white;}
 static Texture2D Solid(Color c){var t=new Texture2D(1,1);t.SetPixel(0,0,c);t.Apply();return t;}
 GUIStyle Center(GUIStyle source)=>new GUIStyle(source){alignment=TextAnchor.MiddleCenter};
 void Sheet(string title){GUI.DrawTexture(new Rect(230,175,1140,690),card);GUI.DrawTexture(new Rect(230,175,1140,7),accent);GUI.Label(new Rect(285,215,1030,78),title,centerHeading);}
 void DrawTicket(){
  Sheet("BILHETERIA");GUI.Label(new Rect(285,292,1030,48),"Teatro Firjan SESI Centro · Museu virtual",centerSmall);
  GUI.DrawTexture(new Rect(355,370,890,196),background);
  GUI.Label(new Rect(395,394,810,48),"INGRESSO DIGITAL · VISITA AO MUSEU",centerBadge);
  GUI.Label(new Rect(395,458,810,76),"ADMISSÃO GRATUITA\nFictício · Sem valor comercial",centerBadge);
  GUI.Label(new Rect(315,595,970,90),ticketIssued?"Ingresso emitido com sucesso!\nSiga para a porta do auditório.":"Retire seu ingresso para continuar a visita.\nNenhum dado pessoal é solicitado.",centerBody);
  if(ticketIssued){if(GUI.Button(new Rect(570,730,460,62),"Concluir",button))Close();}
  else {if(GUI.Button(new Rect(355,730,425,62),"Retirar ingresso",button))IssueTicket();if(GUI.Button(new Rect(820,730,425,62),"Cancelar",button))Close();}
 }
 void DrawAccess(){
  Sheet("ACESSO AO TEATRO");GUI.Label(new Rect(325,300,950,72),"Para entrar no auditório, retire seu ingresso na bilheteria.",centerBody);
  // Ícone vetorial simples: ingresso com duas abas; não depende de fonte de ícones.
  GUI.DrawTexture(new Rect(740,407,120,72),background);GUI.DrawTexture(new Rect(748,416,104,54),card);GUI.DrawTexture(new Rect(818,420,3,46),accent);
  GUI.Label(new Rect(325,515,950,80),"Você ainda não possui um ingresso válido.",centerBody);
  GUI.Label(new Rect(325,594,950,66),"Volte ao hall. O balcão fica à esquerda da entrada principal.",centerSmall);
  if(GUI.Button(new Rect(400,706,800,64),"IR PARA A BILHETERIA",button))GuideToTicket();
  if(GUI.Button(new Rect(610,792,380,42),"Fechar",button))Close();
 }
 void DrawExhibit(){Sheet(Active.title);bool pic=Active.image;float textX=pic?825:315;float textWidth=pic?490:970;
  if(pic){DrawPhoto(new Rect(285,330,500,350));if(GUI.Button(new Rect(285,685,500,35),"AMPLIAR IMAGEM",button))OpenPhoto();GUI.Label(new Rect(285,730,500,48),Active.credit,small);}
  var exhibitBody=new GUIStyle(body){fontSize=21};if(!pic){float contentHeight=Mathf.Max(365,exhibitBody.CalcHeight(new GUIContent(Active.body),930));exhibitScroll=GUI.BeginScrollView(new Rect(315,320,970,390),exhibitScroll,new Rect(0,0,940,contentHeight));GUI.Label(new Rect(0,0,930,contentHeight),Active.body,exhibitBody);GUI.EndScrollView();GUI.Label(new Rect(315,730,970,45),"Fonte: "+Active.source,small);}else{GUI.Label(new Rect(textX,330,textWidth,300),Active.body,exhibitBody);GUI.Label(new Rect(textX,640,textWidth,130),"Fonte: "+Active.source,small);}
  if(GUI.Button(new Rect(620,790,360,55),"Fechar",button))Close();
 }
 void DrawPhoto(Rect area){
  GUI.DrawTexture(area,background);Rect uv=Active.photoRegion;if(uv.width<=0||uv.height<=0)uv=new Rect(0,0,1,1);
  uv.x=Mathf.Clamp01(uv.x);uv.y=Mathf.Clamp01(uv.y);uv.width=Mathf.Min(uv.width,1-uv.x);uv.height=Mathf.Min(uv.height,1-uv.y);
  float aspect=Active.image.width*uv.width/(Active.image.height*Mathf.Max(.001f,uv.height));float w=area.width-16,h=area.height-16;
  if(w/h>aspect)w=h*aspect;else h=w/aspect;
  GUI.DrawTextureWithTexCoords(new Rect(area.center.x-w*.5f,area.center.y-h*.5f,w,h),Active.image,uv);if(GUI.Button(area,GUIContent.none,GUIStyle.none))OpenPhoto();
 }
 void DrawExpandedPhoto(){
  GUI.DrawTexture(new Rect(40,20,1520,960),background);
  GUI.Label(new Rect(100,35,1400,55),Active.title,centerBadge);
  var area=new Rect(100,105,1400,730);
  Rect uv=Active.photoRegion;if(uv.width<=0||uv.height<=0)uv=new Rect(0,0,1,1);
  float aspect=Active.image.width*uv.width/(Active.image.height*uv.height);
  float fitHeight=Mathf.Min(700,1360/aspect),h=fitHeight*photoZoom,w=h*aspect;
  var content=new Rect(0,0,Mathf.Max(1370,w+24),Mathf.Max(700,h+24));
  photoScroll=GUI.BeginScrollView(area,photoScroll,content);GUI.DrawTextureWithTexCoords(new Rect((content.width-w)/2,(content.height-h)/2,w,h),Active.image,uv);GUI.EndScrollView();
  GUI.Label(new Rect(100,845,1400,45),Active.credit,centerBadge);
  if(GUI.Button(new Rect(230,905,110,50),"−",button))photoZoom=Mathf.Max(1,photoZoom-.5f);
  GUI.Label(new Rect(350,910,130,40),photoZoom.ToString("F1")+"×",centerBadge);
  if(GUI.Button(new Rect(490,905,110,50),"+",button))photoZoom=Mathf.Min(6,photoZoom+.5f);
  if(GUI.Button(new Rect(640,905,220,50),"Ajustar à tela",button)){photoZoom=1;photoScroll=Vector2.zero;}
  if(GUI.Button(new Rect(900,905,470,50),Active.imageOnly?"Fechar · Esc":"Voltar às informações",button)){if(Active.imageOnly)Close();else ClosePhoto();}
 }
 void DrawGuidance(){
  MuseumPoint ticket=null;foreach(var p in points)if(p&&p.action==MuseumAction.Ticket){ticket=p;break;}if(!ticket)return;
  Vector3 delta=ticket.transform.position-transform.position;delta.y=0;float angle=Vector3.SignedAngle(transform.forward,delta,Vector3.up);
  string direction=Mathf.Abs(angle)>125?"atrás de você":angle>25?"à direita":angle< -25?"à esquerda":"em frente";
  GUI.DrawTexture(new Rect(380,755,840,66),background);GUI.Label(new Rect(400,767,800,44),"BILHETERIA · "+direction+" · "+delta.magnitude.ToString("F0")+" m",centerBadge);
 }
 void OnGUI(){
  var sceneDoor=GetComponent<Maracannes.AcervoTravel>();if(sceneDoor&&sceneDoor.Nearby&&!BlockInput)return;if(sceneDoor&&sceneDoor.InAcervo&&!Active)return;
  if(!(sceneDoor&&sceneDoor.InAcervo)&&Event.current.type==EventType.KeyDown&&(Event.current.keyCode==KeyCode.E||Event.current.keyCode==KeyCode.Escape)){HandleKey(Event.current.keyCode);Event.current.Use();}
  Styles();if(centerHeading==null){centerHeading=Center(heading);centerBody=Center(body);centerSmall=Center(small);centerBadge=Center(badge);}
  // Escala uniforme e área segura: textos mantêm proporção em 16:9, 16:10 e 4:3.
  float scale=Mathf.Min(Screen.width/1600f,Screen.height/1000f);var old=GUI.matrix;GUI.matrix=Matrix4x4.TRS(new Vector3((Screen.width-1600*scale)*.5f,(Screen.height-1000*scale)*.5f,0),Quaternion.identity,Vector3.one*scale);
  if(sceneDoor&&sceneDoor.InAcervo){if(ExpandedPhoto)DrawExpandedPhoto();else DrawExhibit();GUI.matrix=old;return;}
  GUI.DrawTexture(new Rect(0,0,1600,72),background);GUI.Label(new Rect(28,16,950,42),"FIRJAN SESI CENTRO  /  MUSEU VIRTUAL",badge);GUI.Label(new Rect(1095,16,475,42),ticketValidated?"INGRESSO VALIDADO":ticketIssued?"INGRESSO DIGITAL EMITIDO":"VISITANTE · SEM INGRESSO",centerBadge);
  if(!BlockInput){GUI.Label(new Rect(785,480,30,40),"+",centerBadge);GUI.DrawTexture(new Rect(350,930,900,50),background);GUI.Label(new Rect(365,937,870,35),"WASD · caminhar    Mouse · olhar    E · interagir    Esc · pausa",centerBadge);if(GuidingToTicket)DrawGuidance();if(Nearby){GUI.DrawTexture(new Rect(470,840,660,62),background);GUI.Label(new Rect(495,849,610,42),Nearby.Prompt,centerBadge);}}
  if(Time.unscaledTime<messageUntil&&!BlockInput){GUI.DrawTexture(new Rect(360,95,880,65),background);GUI.Label(new Rect(380,105,840,45),Message,centerBadge);}
  if(Paused){Sheet("VISITA EM PAUSA");GUI.Label(new Rect(355,370,890,150),"Explore a bilheteria e os painéis.\nO ingresso é fictício, gratuito e válido apenas nesta visita.",centerBody);if(GUI.Button(new Rect(570,660,460,64),"Continuar visita",button))Resume();}
  else if(AccessDenied)DrawAccess();else if(Active){if(Active.action==MuseumAction.Ticket)DrawTicket();else if(ExpandedPhoto)DrawExpandedPhoto();else DrawExhibit();}
  GUI.matrix=old;
 }
 void OnDestroy(){Cursor.lockState=CursorLockMode.None;Cursor.visible=true;foreach(var tex in new[]{background,accent,card})if(tex)Destroy(tex);}
}
}

