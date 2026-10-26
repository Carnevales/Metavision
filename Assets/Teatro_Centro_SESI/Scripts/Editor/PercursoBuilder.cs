using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

namespace TeatroSESI {
[InitializeOnLoad] public static class PercursoBuilder {
 const string B="Assets/Teatro_Centro_SESI/";
 static Transform raiz;static Material pedra,escuro,vidro,janela,claro,ouro,piso,calcada,laranja,luz,madeira;
 static PercursoBuilder(){EditorApplication.delayCall+=()=>{string p=B+"References/expand.request";if(File.Exists(p)){AssetDatabase.DeleteAsset(p);Build();}};}
 [MenuItem("Teatro SESI/Ampliar fachada e percurso integrado")]
 public static void Build(){
  var scene=SceneManager.GetSceneByPath(B+"Scenes/Teatro.unity");if(!scene.isLoaded)scene=EditorSceneManager.OpenScene(B+"Scenes/Teatro.unity",OpenSceneMode.Additive);SceneManager.SetActiveScene(scene);
  var teatro=scene.GetRootGameObjects().Select(g=>g.GetComponent<TeatroDimensions>()).FirstOrDefault(d=>d);if(!teatro)throw new InvalidOperationException("Teatro original não encontrado; construção cancelada.");
  var old=scene.GetRootGameObjects().FirstOrDefault(g=>g.name=="Percurso_Centro_SESI");if(old)UnityEngine.Object.DestroyImmediate(old);
  raiz=new GameObject("Percurso_Centro_SESI").transform;
  pedra=Mat("Acesso_Granito_rosado",new Color(.65f,.55f,.47f),.30f);pedra.mainTexture=AssetDatabase.LoadAssetAtPath<Texture2D>(B+"Textures/granite_tile_color_1k.jpg");pedra.mainTextureScale=new Vector2(2,2);
  escuro=Mat("Acesso_Esquadrias",new Color(.045f,.055f,.06f),.58f);claro=Mat("Acesso_Parede_neutra",new Color(.76f,.74f,.69f),.08f);ouro=Mat("Acesso_Letreiro_bronze",new Color(.62f,.47f,.19f),.70f);ouro.SetFloat("_Metallic",.65f);
  janela=Mat("Acesso_Vidro_refletivo",new Color(.72f,.80f,.86f),.6f);janela.mainTexture=Photo("Reflexos_foto_usuario",new Rect(.08f,.69f,.15f,.29f)).mainTexture;janela.mainTextureScale=new Vector2(-1,-1);janela.mainTextureOffset=Vector2.one;
  vidro=Mat("Acesso_Vidro_porta",new Color(.5f,.67f,.72f,.20f),.9f);vidro.SetFloat("_Surface",1);vidro.SetFloat("_SrcBlend",(float)BlendMode.SrcAlpha);vidro.SetFloat("_DstBlend",(float)BlendMode.OneMinusSrcAlpha);vidro.SetFloat("_ZWrite",0);vidro.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");vidro.SetOverrideTag("RenderType","Transparent");vidro.renderQueue=3000;
  piso=Mat("Acesso_Piso_cinza",new Color(.57f,.56f,.53f),.32f);piso.mainTexture=pedra.mainTexture;piso.mainTextureScale=new Vector2(3,3);
  calcada=Mat("Acesso_Pedra_portuguesa",Color.white,.12f);calcada.mainTexture=Pavimento();calcada.mainTextureScale=new Vector2(4,1.4f);
  laranja=Mat("Acesso_Laranja_programacao",new Color(.92f,.31f,.035f));luz=Mat("Acesso_Luminaria",Color.white);luz.EnableKeyword("_EMISSION");luz.SetColor("_EmissionColor",new Color(2,1.8f,1.5f));madeira=AssetDatabase.LoadAssetAtPath<Material>(B+"Materials/Madeira_natural.mat");
  Facade();Hall();Connection(teatro);Cameras(teatro);
  Physics.SyncTransforms();Validate();AssetDatabase.SaveAssets();EditorSceneManager.SaveScene(scene);Render();
  Selection.activeGameObject=raiz.gameObject;
 }
 static Material Mat(string n,Color c,float s=0){var path=B+"Materials/"+n+".mat";var m=AssetDatabase.LoadAssetAtPath<Material>(path);if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(m,path);}m.color=c;m.SetFloat("_Smoothness",s);m.enableInstancing=true;return m;}
 static Transform G(string n,Transform parent=null){var t=new GameObject(n).transform;t.SetParent(parent?parent:raiz,false);return t;}
 static GameObject Box(string n,Vector3 p,Vector3 s,Material m,Transform parent,bool collision=true){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=n;g.transform.SetParent(parent,false);g.transform.localPosition=p;g.transform.localScale=s;g.GetComponent<Renderer>().sharedMaterial=m;if(!collision)UnityEngine.Object.DestroyImmediate(g.GetComponent<Collider>());return g;}
 static void Bar(string n,Vector3 a,Vector3 b,float r,Material m,Transform parent){var g=GameObject.CreatePrimitive(PrimitiveType.Cylinder);g.name=n;g.transform.SetParent(parent,false);g.transform.localPosition=(a+b)/2;g.transform.localScale=new Vector3(r*2,Vector3.Distance(a,b)/2,r*2);g.transform.up=(b-a).normalized;g.GetComponent<Renderer>().sharedMaterial=m;UnityEngine.Object.DestroyImmediate(g.GetComponent<Collider>());}
 static void Label(string text,Vector3 p,float size,Color color,Transform parent,Quaternion? rotation=null){var t=G(text,parent);t.localPosition=p;t.localRotation=rotation??Quaternion.identity;var tm=t.gameObject.AddComponent<TextMesh>();tm.text=text;tm.fontSize=90;tm.characterSize=size;tm.anchor=TextAnchor.MiddleCenter;tm.alignment=TextAlignment.Center;tm.color=color;}
 static Texture2D Pavimento(){string path=B+"Textures/Calcada_portuguesa.png";if(!File.Exists(path)){int n=768;var tex=new Texture2D(n,n,TextureFormat.RGB24,false);var pixels=new Color[n*n];for(int y=0;y<n;y++)for(int x=0;x<n;x++){float u=x/(float)n,v=y/(float)n;float wave=Mathf.Sin(v*Mathf.PI*4)*.09f;float stripe=Mathf.Repeat(u+wave, .5f);bool dark=stripe<.078f;float noise=Mathf.PerlinNoise(x*.55f,y*.55f);float mortar=(x+(int)(Mathf.Sin(y*.025f)*4))%13==0||y%12==0?.68f:1;float c=(dark?.10f:.81f)+(noise-.5f)*.22f;pixels[y*n+x]=new Color(c*mortar,c*mortar,c*mortar*.97f);}tex.SetPixels(pixels);tex.Apply();File.WriteAllBytes(path,tex.EncodeToPNG());UnityEngine.Object.DestroyImmediate(tex);AssetDatabase.ImportAsset(path);}return AssetDatabase.LoadAssetAtPath<Texture2D>(path);}
 static Material Photo(string n,Rect uv){string output=B+"Textures/"+n+".png";if(!File.Exists(output)){var bytes=File.ReadAllBytes(B+"References/IMG_8294 1.png");var tex=new Texture2D(2,2);tex.LoadImage(bytes);int x=(int)(uv.x*tex.width),y=(int)(uv.y*tex.height),w=(int)(uv.width*tex.width),h=(int)(uv.height*tex.height);var crop=new Texture2D(w,h);crop.SetPixels(tex.GetPixels(x,y,w,h));crop.Apply();File.WriteAllBytes(output,crop.EncodeToPNG());UnityEngine.Object.DestroyImmediate(tex);UnityEngine.Object.DestroyImmediate(crop);AssetDatabase.ImportAsset(output);}var m=Mat(n,Color.white);m.mainTexture=AssetDatabase.LoadAssetAtPath<Texture2D>(output);m.mainTextureScale=new Vector2(-1,-1);m.mainTextureOffset=Vector2.one;return m;}
 static void Facade(){var t=G("Fachada_Externa");
  Box("Calçada portuguesa — padrão aproximado",new Vector3(3,-.12f,-31),new Vector3(25,.24f,7),calcada,t);
  Box("Meio-fio",new Vector3(3,-.10f,-34.55f),new Vector3(25,.25f,.22f),pedra,t);
  Box("Faixa da rua",new Vector3(3,-.25f,-37.5f),new Vector3(25,.16f,5.7f),escuro,t);
  for(int i=0;i<6;i++){float x=-6.4f+i*4.2f;Box("Montante vertical de pedra",new Vector3(x,10,-27.8f),new Vector3(.5f,20,.48f),pedra,t);}
  for(int i=0;i<5;i++){float x=-4.3f+i*4.2f;Box("Pano vidro azul",new Vector3(x,12.5f,-27.65f),new Vector3(3.68f,14.8f,.16f),janela,t);
   for(int j=0;j<10;j++)Box("Travessa esquadria",new Vector3(x,5.45f+j*1.48f,-27.79f),new Vector3(3.70f,.035f,.055f),escuro,t,false);
   for(int j=0;j<3;j++)Box("Divisao vertical vidro",new Vector3(x-1.8f+j*1.8f,12.5f,-27.78f),new Vector3(.032f,14.8f,.06f),escuro,t,false);
   if(i==2||i==3)for(int j=0;j<45;j++)Box("Brise metalico",new Vector3(x-1.78f+j*.08f,12.5f,-27.86f),new Vector3(.018f,14.7f,.07f),escuro,t,false);
  }
  Box("Cinta horizontal granito",new Vector3(4.1f,4.72f,-27.88f),new Vector3(21.7f,.8f,.55f),pedra,t);
  Box("Marquise clara",new Vector3(4.1f,3.45f,-28.5f),new Vector3(22.6f,.18f,2.1f),claro,t);
  Box("Borda marquise",new Vector3(4.1f,3.61f,-29.55f),new Vector3(22.7f,.18f,.12f),escuro,t);
  var banner=Photo("Programacao_foto_usuario",new Rect(.238f,.505f,.53f,.12f));Box("Painel laranja — foto fornecida",new Vector3(3.9f,4.05f,-29.61f),new Vector3(22.2f,.92f,.04f),banner,t,false);
  for(int i=0;i<12;i++){float x=-6.5f+i*1.9f;Bar("Haste iluminacao da marquise",new Vector3(x,4.55f,-29.5f),new Vector3(x,5.03f,-28.65f),.018f,escuro,t);Box("Projetor fachada",new Vector3(x,4.58f,-29.52f),new Vector3(.22f,.09f,.13f),escuro,t,false);}
  var e=G("Entrada_Principal");
  Box("Pilar esquerdo entrada",new Vector3(-2.2f,1.62f,-27.8f),new Vector3(.65f,3.24f,.6f),pedra,e);
  Box("Pilar direito entrada",new Vector3(2.15f,1.62f,-27.8f),new Vector3(.6f,3.24f,.6f),pedra,e);
  Box("Letreiro friso",new Vector3(0,3.03f,-27.98f),new Vector3(3.75f,.42f,.25f),pedra,e);
  Label("T E A T R O   S E S I",new Vector3(0,3.04f,-28.13f),.060f,new Color(.66f,.53f,.28f),e);
  Box("Faixa granito esquerda",new Vector3(-4.45f,1.6f,-27.8f),new Vector3(3.8f,3.2f,.4f),pedra,e);
  for(int s=-1;s<=1;s+=2){Box("Bandeira fixa vidro",new Vector3(s*1.57f,1.37f,-27.62f),new Vector3(.48f,2.7f,.05f),vidro,e);Door("Porta entrada aberta",new Vector3(s*1.30f,0,-27.6f),s,1.27f,2.7f,e,vidro);}
  Box("Verga esquadria entrada",new Vector3(0,2.76f,-27.63f),new Vector3(3.65f,.075f,.12f),escuro,e);
  var poster=Photo("Cartaz_foto_usuario",new Rect(.526f,.265f,.239f,.165f));
  Box("Painel programação moldura",new Vector3(5.2f,1.63f,-27.84f),new Vector3(4.6f,2.4f,.24f),pedra,e);
  Box("Painel programação conteúdo",new Vector3(5.2f,1.63f,-28.0f),new Vector3(4.30f,2.12f,.035f),poster,e,false);
  Box("Base painel",new Vector3(5.2f,.2f,-27.85f),new Vector3(5.4f,.4f,.5f),pedra,e);
  for(int s=-1;s<=1;s+=2)Box("Pilar acesso lateral",new Vector3(10.25f+s*1.75f,1.6f,-27.8f),new Vector3(.6f,3.2f,.6f),pedra,e);
  Box("Vidro lateral cafeteria",new Vector3(10.25f,1.55f,-27.7f),new Vector3(2.85f,2.9f,.05f),vidro,e);
  Box("Fecho lado direito",new Vector3(13.65f,1.6f,-27.7f),new Vector3(2.3f,3.2f,.4f),pedra,e);
  for(int i=0;i<7;i++)Box("Junta granito faixa",new Vector3(-6.5f+i*3.1f,4.72f,-28.162f),new Vector3(.015f,.8f,.006f),escuro,t,false);
 }
 static void Door(string n,Vector3 hinge,int side,float width,float height,Transform parent,Material pane){var pivot=G(n,parent);pivot.localPosition=hinge;pivot.localRotation=Quaternion.Euler(0,side*82,0);float cx=-side*width/2;
  Box("Folha",new Vector3(cx,height/2,0),new Vector3(width,height,.035f),pane,pivot);
  for(int s=-1;s<=1;s+=2){Box("Montante",new Vector3(cx+s*width/2,height/2,0),new Vector3(.035f,height,.08f),escuro,pivot);Box("Travessa",new Vector3(cx,height/2+s*(height/2-.025f),0),new Vector3(width,.05f,.08f),escuro,pivot);}
  Bar("Puxador",new Vector3(cx-side*width*.29f,.9f,-.09f),new Vector3(cx-side*width*.29f,1.3f,-.09f),.014f,ouro,pivot);
 }
 static void Hall(){var t=G("Hall_Entrada — volume provisório sem levantamento");
  Box("Piso hall",new Vector3(4,-.10f,-24.5f),new Vector3(19,.2f,6.2f),piso,t);
  Box("Parede lateral esquerda",new Vector3(-5.5f,1.6f,-24.5f),new Vector3(.2f,3.2f,6.2f),claro,t);
  Box("Parede lateral direita",new Vector3(13.5f,1.6f,-24.5f),new Vector3(.2f,3.2f,6.2f),claro,t);
  Box("Teto hall",new Vector3(4,3.25f,-24.5f),new Vector3(19,.15f,6.2f),claro,t);
  // Fundo dividido: passagem central de 2.4 m, sem parede no vão.
  Box("Fundo esquerdo",new Vector3(-3.35f,1.6f,-21.45f),new Vector3(4.3f,3.2f,.2f),claro,t);
  Box("Fundo direito",new Vector3(7.35f,1.6f,-21.45f),new Vector3(12.3f,3.2f,.2f),claro,t);
  Box("Verga acesso corredor",new Vector3(0,2.95f,-21.45f),new Vector3(2.4f,.5f,.2f),claro,t);
  // Existência de café e mini galeria confirmada; mobiliário apenas representativo.
  var cafe=G("Cafe — disposição estimada",t);Box("Balcao simples representativo",new Vector3(10.7f,.52f,-22.65f),new Vector3(3.3f,1.04f,.65f),madeira,cafe);Box("Tampo",new Vector3(10.7f,1.06f,-22.65f),new Vector3(3.42f,.06f,.74f),pedra,cafe);
  for(int i=0;i<2;i++){float x=6.4f+i*2.6f;Bar("Pe mesa",new Vector3(x,.02f,-25),new Vector3(x,.73f,-25),.04f,escuro,cafe);Box("Mesa cafe",new Vector3(x,.75f,-25),new Vector3(.7f,.05f,.7f),madeira,cafe);for(int s=-1;s<=1;s+=2){Box("Assento cafe",new Vector3(x+s*.7f,.45f,-25),new Vector3(.4f,.07f,.4f),madeira,cafe);for(int j=-1;j<=1;j+=2)Bar("Pe cadeira",new Vector3(x+s*.7f+j*.15f,.02f,-25),new Vector3(x+s*.7f+j*.15f,.42f,-25),.019f,escuro,cafe);}}
  for(int i=0;i<4;i++){float z=-26.5f+i*1.3f;Box("Moldura galeria representativa",new Vector3(-5.36f,1.7f,z),new Vector3(.055f,1.0f,.8f),escuro,t,false);Box("Painel vazio para exposição",new Vector3(-5.32f,1.7f,z),new Vector3(.025f,.89f,.69f),claro,t,false);}
  for(int x=0;x<5;x++)for(int z=0;z<2;z++)Downlight(new Vector3(-3+x*3.6f,3.14f,-23-z*2.7f),t,3.5f);
 }
 static void Connection(TeatroDimensions d){var t=G("Corredores_Acesso — traçado estimado");
  Box("Parede corredor esquerda",new Vector3(-1.3f,2.25f,-18.45f),new Vector3(.2f,4.5f,6),claro,t);
  Box("Parede corredor direita",new Vector3(1.3f,2.25f,-18.45f),new Vector3(.2f,4.5f,6),claro,t);
  Box("Teto corredor",new Vector3(0,4.5f,-18.45f),new Vector3(2.8f,.15f,6),claro,t);
  // Rampa funcional provisória: topografia não documentada.
  Ramp("Rampa hall para nível plateia",-21.55f,-16.1f,0,1.32f,2.4f,t,piso);
  Box("Patamar porta teatro",new Vector3(0,1.22f,-15.6f),new Vector3(2.4f,.2f,1.1f),piso,t);
  for(int s=-1;s<=1;s+=2)Bar("Corrimao corredor",new Vector3(s*1.12f,1.05f,-21.4f),new Vector3(s*1.12f,2.37f,-16.05f),.025f,escuro,t);
  for(int i=0;i<3;i++)Downlight(new Vector3(0,4.35f,-20.8f+i*2.1f),t,3);
  var entry=G("Entrada_Teatro");var balcao=d.transform.Find("03_Balcao — estimativa fotografica");
  if(!balcao)throw new InvalidOperationException("Balcão original não localizado.");
  var wall=balcao.Find("Parede traseira");if(wall)wall.gameObject.SetActive(false);
  foreach(Transform child in balcao)if(child.name=="Porta traseira")child.gameObject.SetActive(false);
  for(int s=-1;s<=1;s+=2){Box("Parede original com abertura",new Vector3(s*4.45f,3.4f,-15.5f),new Vector3(6.7f,7,.22f),AssetDatabase.LoadAssetAtPath<Material>(B+"Materials/Estrutura_antracite.mat"),entry);Door("Folha auditório aberta",new Vector3(s*1.08f,1.32f,-15.36f),s,1.04f,2.05f,entry,madeira);}
  Box("Verga auditório",new Vector3(0,5.25f,-15.5f),new Vector3(2.2f,3.6f,.22f),escuro,entry);
  Box("Piso conexão ao corredor central",new Vector3(0,1.24f,-14.65f),new Vector3(1.48f,.16f,1.1f),piso,entry);
  // Degraus de toda a largura permanecem; nenhuma poltrona removida.
 }
 static void Ramp(string name,float z0,float z1,float y0,float y1,float width,Transform parent,Material mat){var g=G(name,parent);var mesh=new Mesh{name="Rampa_Acesso"};mesh.vertices=new[]{new Vector3(-width/2,y0,z0),new Vector3(width/2,y0,z0),new Vector3(-width/2,y1,z1),new Vector3(width/2,y1,z1),new Vector3(-width/2,y0-.25f,z0),new Vector3(width/2,y0-.25f,z0),new Vector3(-width/2,y1-.25f,z1),new Vector3(width/2,y1-.25f,z1)};mesh.triangles=new[]{0,2,1,1,2,3,4,5,6,5,7,6,0,4,2,4,6,2,1,3,5,5,3,7,0,1,4,4,1,5,2,6,3,3,6,7};mesh.uv=new[]{Vector2.zero,Vector2.right,Vector2.up,Vector2.one,Vector2.zero,Vector2.right,Vector2.up,Vector2.one};mesh.RecalculateNormals();mesh.RecalculateBounds();string path=B+"Models/Rampa_Acesso.asset";var old=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(old){EditorUtility.CopySerialized(mesh,old);UnityEngine.Object.DestroyImmediate(mesh);mesh=old;}else AssetDatabase.CreateAsset(mesh,path);g.gameObject.AddComponent<MeshFilter>().sharedMesh=mesh;g.gameObject.AddComponent<MeshRenderer>().sharedMaterial=mat;g.gameObject.AddComponent<MeshCollider>().sharedMesh=mesh;}
 static void Downlight(Vector3 p,Transform parent,float intensity){Box("Luminária",p,new Vector3(.24f,.035f,.24f),luz,parent,false);var t=G("Luz circulação",parent);t.localPosition=p+Vector3.down*.12f;var l=t.gameObject.AddComponent<Light>();t.localRotation=Quaternion.Euler(90,0,0);l.type=LightType.Spot;l.spotAngle=105;l.innerSpotAngle=70;l.intensity=intensity*2;l.range=6;l.color=new Color(1,.91f,.80f);}
 static void Cameras(TeatroDimensions teatro){var t=G("Iluminacao_e_Cameras");
  Cam("Vista_Fachada",new Vector3(1,2.5f,-40),new Vector3(3,5.7f,-27.5f),62,t);
  Cam("Vista_Hall",new Vector3(-2,1.7f,-26),new Vector3(5,1.4f,-23),70,t);
  Cam("Vista_Corredor",new Vector3(0,1.7f,-21),new Vector3(0,2.2f,-15),65,t);
  var v=G("Visitante_Inspecao",t);v.localPosition=new Vector3(0,.05f,-33);var cc=v.gameObject.AddComponent<CharacterController>();cc.height=1.75f;cc.radius=.23f;cc.center=new Vector3(0,.9f,0);cc.stepOffset=.28f;cc.slopeLimit=45;cc.skinWidth=.025f;
  var view=Cam("Camera_Percurso",new Vector3(0,1.65f,0),new Vector3(0,1.65f,1),65,v);view.enabled=true;view.depth=30;var script=v.gameObject.AddComponent<VisitaCentroSESI>();script.cameraVisita=view;script.percurso=new[]{new Vector3(0,0,-30),new Vector3(0,0,-27),new Vector3(0,0,-24.7f),new Vector3(0,0,-21.8f),new Vector3(0,1.32f,-16),new Vector3(0,1.32f,-14.8f),new Vector3(0,1.2f,-12),new Vector3(0,.6f,-8),new Vector3(0,0,-4),new Vector3(0,1.05f,-2.1f),new Vector3(0,1.05f,0)};
  // Camadas padrão preservadas; sombras arquitetônicas separam exterior e interior.
  foreach(var r in raiz.GetComponentsInChildren<Renderer>())r.renderingLayerMask=1;
  var sun=G("Luz externa da fachada",t);sun.localRotation=Quaternion.Euler(48,35,0);var light=sun.gameObject.AddComponent<Light>();light.type=LightType.Directional;light.intensity=1.6f;light.color=new Color(.88f,.94f,1);light.shadows=LightShadows.Soft;light.renderingLayerMask=1;
 }
 static Camera Cam(string n,Vector3 p,Vector3 target,float fov,Transform parent){var t=G(n,parent);t.localPosition=p;t.localRotation=Quaternion.LookRotation(target-p);var c=t.gameObject.AddComponent<Camera>();c.enabled=false;c.fieldOfView=fov;c.nearClipPlane=.08f;c.farClipPlane=100;c.clearFlags=CameraClearFlags.SolidColor;c.backgroundColor=new Color(.52f,.66f,.76f);return c;}
 static void Validate(){int missing=0;foreach(var r in raiz.GetComponentsInChildren<Renderer>())foreach(var m in r.sharedMaterials)if(!m||!m.shader||!m.shader.isSupported)missing++;
  var result=new List<string>{"Ampliação executada no Editor Unity: "+DateTime.Now,"Teatro preservado: construção original reutilizada, somente parede traseira e portas antigas desativadas para abertura.","Materiais/shaders ausentes: "+missing,"Volume hall, corredor e rampa estimados. Não são planta real do prédio.","Validação física completa: ver percurso-runtime.txt após teste Play."};
  foreach(float z in new[]{-30f,-27.5f,-25f,-22f,-20f,-18f,-16f,-15.5f,-14.8f,-12f,-8f,-4f,-2.1f,0}){if(Physics.Raycast(new Vector3(0,10,z),Vector3.down,out var hit,15))result.Add("Eixo central z="+z+": superfície superior "+hit.collider.name+" y="+hit.point.y.ToString("F2"));}
  File.WriteAllLines(B+"References/ampliacao-validation.txt",result);
 }
 [MenuItem("Teatro SESI/Verificar percurso em Play %#t")]
 public static void VerifyPlay(){File.WriteAllText(B+"References/percurso-test.request","run");EditorApplication.isPlaying=true;}
 [MenuItem("Teatro SESI/Renderizar novos ambientes")]
 public static void Render(){if(!raiz){var go=GameObject.Find("Percurso_Centro_SESI");if(!go)return;raiz=go.transform;}foreach(var c in raiz.GetComponentsInChildren<Camera>()){var rt=new RenderTexture(1600,1000,24);var prev=c.targetTexture;var active=RenderTexture.active;try{c.targetTexture=rt;c.Render();RenderTexture.active=rt;var tex=new Texture2D(1600,1000,TextureFormat.RGB24,false);tex.ReadPixels(new Rect(0,0,1600,1000),0,0);tex.Apply();File.WriteAllBytes(B+"Lighting/"+c.name+".png",tex.EncodeToPNG());UnityEngine.Object.DestroyImmediate(tex);}finally{c.targetTexture=prev;RenderTexture.active=active;UnityEngine.Object.DestroyImmediate(rt);}}AssetDatabase.Refresh();}
}
}



