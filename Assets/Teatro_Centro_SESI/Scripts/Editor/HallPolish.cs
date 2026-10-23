using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace TeatroSESI {
[InitializeOnLoad] public static class HallPolish {
 const string B="Assets/Teatro_Centro_SESI/";
 static Transform finish; static Material graphite,stone,ivory,trim;
 static Scene scene;
 static HallPolish(){EditorApplication.delayCall+=()=>{if(File.Exists(B+"References/polish.request")){AssetDatabase.DeleteAsset(B+"References/polish.request");Apply();}};}
 static Transform[] All()=>scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).ToArray();
 static Transform Find(string n)=>All().First(t=>t.name==n);
 static Material Mat(string name,Color c,float smooth){string path=B+"Materials/"+name+".mat";var m=AssetDatabase.LoadAssetAtPath<Material>(path);if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(m,path);}m.color=c;m.SetFloat("_Smoothness",smooth);return m;}
 static Transform Box(string name,Vector3 p,Vector3 size,Material m,Transform parent=null){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(parent?parent:finish,false);g.transform.localPosition=p;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=m;UnityEngine.Object.DestroyImmediate(g.GetComponent<Collider>());return g.transform;}
 static void Fit(TextMesh tm,string text,Vector3 position,float width,float height){tm.text=text;tm.transform.position=position;tm.transform.rotation=Quaternion.identity;tm.transform.localScale=Vector3.one;tm.anchor=TextAnchor.MiddleCenter;tm.alignment=TextAlignment.Center;tm.fontSize=80;tm.characterSize=.04f;tm.color=new Color(.95f,.94f,.89f);var size=tm.GetComponent<Renderer>().localBounds.size;tm.transform.localScale=Vector3.one*Mathf.Min(width/Mathf.Max(size.x,.001f),height/Mathf.Max(size.y,.001f));}
 static Transform Label(string name,string text,Vector3 p,float w,float h,Transform parent=null){var t=new GameObject(name).transform;t.SetParent(parent?parent:finish,false);var tm=t.gameObject.AddComponent<TextMesh>();tm.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");t.gameObject.AddComponent<MuseumTextDepth>().depthShader=Shader.Find("TeatroSESI/Texto com profundidade");Fit(tm,text,p,w,h);return t;}
 [MenuItem("Teatro SESI/Museu/Polir hall e sinalização")]
 public static void Apply(){
  scene=SceneManager.GetSceneByPath(B+"Scenes/Teatro_Centro_SESI.unity");if(!scene.isLoaded)throw new InvalidOperationException("Abra a cena do teatro antes de polir.");
  SceneManager.SetActiveScene(scene);if(!File.Exists(B+"Scenes/Teatro/Antes_Polimento.unity"))EditorSceneManager.SaveScene(scene,B+"Scenes/Teatro/Antes_Polimento.unity",true);
  var museum=Find("Museu_Interativo_SESI");var old=museum.Find("Acabamento_Hall");if(old)UnityEngine.Object.DestroyImmediate(old.gameObject);
  var obj=new GameObject("Acabamento_Hall");SceneManager.MoveGameObjectToScene(obj,scene);finish=obj.transform;finish.SetParent(museum,false);
  graphite=AssetDatabase.LoadAssetAtPath<Material>(B+"Materials/Museu_Grafite.mat");stone=AssetDatabase.LoadAssetAtPath<Material>(B+"Materials/Acesso_Granito_rosado.mat");ivory=Mat("Hall_Marfim_fosco",new Color(.70f,.68f,.63f),.12f);trim=Mat("Hall_Juntas",new Color(.32f,.31f,.29f),.08f);
  if(!File.Exists(B+"Lighting/Polimento_Antes_Hall.png"))Capture("Polimento_Antes_Hall",new Vector3(.3f,1.65f,-26.6f),new Vector3(2,1.55f,-22));
  // Orientação do visitante: entrada em -Z, deslocamento para +Z; direita = +X.
  MovePanel("01",new Vector3(2.35f,1.6f,-21.595f));MovePanel("02",new Vector3(5.25f,1.6f,-21.595f));MovePanel("03",new Vector3(8.05f,1.6f,-21.595f));
  foreach(var panel in All().Where(t=>t.name.StartsWith("Painel_0"))){
   foreach(var tm in panel.GetComponentsInChildren<TextMesh>()){
    bool title=!tm.text.StartsWith("[E]");tm.fontSize=80;tm.characterSize=.04f;tm.transform.localScale=Vector3.one;tm.anchor=TextAnchor.MiddleCenter;tm.alignment=TextAlignment.Center;
    float w=panel.Find("Moldura").localScale.x;var bounds=tm.GetComponent<Renderer>().localBounds.size;tm.transform.localScale=Vector3.one*Mathf.Min((w*.84f)/bounds.x,(title?.40f:.10f)/bounds.y);tm.transform.localPosition=new Vector3(0,title?.13f:-.33f,-.052f);
   }
  }
  var desk=Find("Balcao corpo");var office=desk.parent;Box("Sóculo bilheteria",new Vector3(-2.65f,.045f,-24.7f),new Vector3(2.72f,.09f,.8f),graphite);var accentMaterial=AssetDatabase.LoadAssetAtPath<Material>(B+"Materials/Museu_Laranja.mat");accentMaterial.color=new Color(.73f,.18f,.035f);foreach(var leaf in All().Where(t=>t.name=="Folha auditório aberta"))leaf.localScale=new Vector3(1.075f/1.04f,1,1);
  var sign=Find("Placa bilheteria");sign.position=new Vector3(-2.65f,2.22f,-24.52f);sign.localScale=new Vector3(2.70f,.54f,.08f);
  Fit(Find("BILHETERIA").GetComponent<TextMesh>(),"BILHETERIA",new Vector3(-2.65f,2.29f,-24.566f),2.25f,.23f);
  Fit(Find("MUSEU VIRTUAL · INGRESSO GRATUITO").GetComponent<TextMesh>(),"INGRESSO GRATUITO · MUSEU VIRTUAL",new Vector3(-2.65f,2.07f,-24.566f),2.25f,.09f);
  // Placa fixa ao balcão por montantes; não fica suspensa sem ligação física.
  for(int s=-1;s<=1;s+=2){Box("Montante placa bilheteria",new Vector3(-2.65f+s*1.17f,1.52f,-24.49f),new Vector3(.035f,.88f,.035f),graphite);Box("Sapata placa",new Vector3(-2.65f+s*1.17f,1.09f,-24.49f),new Vector3(.12f,.02f,.12f),graphite);}
  string[] monitorNames={"Monitor","Tela monitor","Suporte monitor","Base monitor","Teclado"};float[] monitorHeights={1.4275f,1.4275f,1.1875f,1.0925f,1.0875f};for(int i=0;i<monitorNames.Length;i++){var t=Find(monitorNames[i]);t.position=new Vector3(t.position.x,monitorHeights[i],t.position.z);}
  var poster=Find("Painel programação museu");poster.position=new Vector3(-2.65f,1.65f,-21.595f);poster.localScale=new Vector3(1.0f,1.32f,.07f);
  var posterText=All().Select(t=>t.GetComponent<TextMesh>()).First(t=>t&&(t.name=="Cartaz_Texto"||t.name.StartsWith("EM CARTAZ")));Fit(posterText,"EM CARTAZ\n\nMARACANNES\nMEMORIAS DA ARTE\n\nEspetáculo fictício\nVisita educativa",new Vector3(-2.65f,1.65f,-21.636f),.78f,.95f);
  var entranceText=office.GetComponentsInChildren<TextMesh>().First(t=>t.text.StartsWith("ENTRADA")||t.text.StartsWith("AUDITÓRIO"));Fit(entranceText,"AUDITÓRIO  ↑",new Vector3(0,2.95f,-21.585f),1.85f,.16f);Box("Suporte orientação auditório",new Vector3(0,2.95f,-21.57f),new Vector3(2.16f,.34f,.025f),graphite);
  var foyer=All().First(t=>t.name=="FOYER · EXPOSIÇÃO").GetComponent<TextMesh>();Fit(foyer,"FOYER  /  MEMÓRIA E CULTURA",new Vector3(6.65f,2.58f,-21.592f),4.0f,.17f);Box("Suporte identificação foyer",new Vector3(6.65f,2.58f,-21.57f),new Vector3(4.5f,.34f,.025f),graphite);
  // Uma orientação compacta, fixada na divisória à direita da entrada.
  var way=Box("Orientação hall suporte",new Vector3(3.565f,1.6f,-26.25f),new Vector3(1.18f,.75f,.07f),graphite);way.rotation=Quaternion.Euler(0,90,0);
  var label=Label("Orientação hall texto","BEM-VINDO\n\nBilheteria à esquerda\nExposição e teatro ao fundo",Vector3.zero,.99f,.52f,way);
  // Parent com escala não uniforme: restaurar tamanho em espaço mundial.
  label.SetParent(finish,true);Fit(label.GetComponent<TextMesh>(),"BEM-VINDO\n\nBilheteria à esquerda\nExposição e teatro ao fundo",new Vector3(3.524f,1.6f,-26.25f),.99f,.52f);label.rotation=Quaternion.Euler(0,90,0);
  // Rodapés e juntas discretas; volumes e passagens existentes são preservados.
  Box("Rodapé esquerdo",new Vector3(-5.385f,.065f,-24.5f),new Vector3(.028f,.13f,6.05f),stone);Box("Rodapé direito",new Vector3(13.385f,.065f,-24.5f),new Vector3(.028f,.13f,6.05f),stone);
  Box("Rodapé fundo esquerdo",new Vector3(-3.35f,.065f,-21.562f),new Vector3(4.28f,.13f,.025f),stone);Box("Rodapé fundo direito",new Vector3(7.35f,.065f,-21.562f),new Vector3(12.28f,.13f,.025f),stone);
  foreach(var t in All().Where(t=>t.name=="Divisoria foyer lado entrada"||t.name=="Divisoria foyer lado fundo")){var sc=t.localScale;Box("Rodapé divisória",new Vector3(t.position.x,.065f,t.position.z),new Vector3(sc.x+.03f,.13f,sc.z),stone);t.GetComponent<Renderer>().sharedMaterial=ivory;}
  foreach(var t in All().Where(t=>t.name=="Parede lateral esquerda"||t.name=="Parede lateral direita"||t.name=="Fundo esquerdo"||t.name=="Fundo direito"))t.GetComponent<Renderer>().sharedMaterial=ivory;
  var floor=Find("Piso hall");var floorMat=Mat("Hall_Pedra_acetinada",new Color(.63f,.61f,.56f),.22f);floorMat.mainTexture=AssetDatabase.LoadAssetAtPath<Texture2D>(B+"Textures/granite_tile_color_1k.jpg");floorMat.mainTextureScale=new Vector2(19,6.2f);floor.GetComponent<Renderer>().sharedMaterial=floorMat;
  for(float x=-5.4f;x<13.4f;x+=1)Box("Junta piso longitudinal",new Vector3(x,.001f,-24.525f),new Vector3(.004f,.001f,5.95f),trim);
  for(float z=-27.5f;z<-21.55f;z+=1)Box("Junta piso transversal",new Vector3(4,.001f,z),new Vector3(18.8f,.001f,.004f),trim);
  foreach(var t in All().Where(t=>t.name=="Pe mesa"||t.name=="Pe cadeira")){var r=t.GetComponent<Renderer>();if(r&&r.bounds.min.y>0&&r.bounds.min.y<.04f){float top=r.bounds.max.y;var scale=t.localScale;scale.y+=r.bounds.min.y*.5f;t.localScale=scale;var pos=t.position;pos.y=top*.5f;t.position=pos;}}
  foreach(var l in Find("Hall_Entrada — volume provisório sem levantamento").GetComponentsInChildren<Light>()){l.color=new Color(1,.94f,.84f);l.intensity=4.5f;l.shadows=LightShadows.None;}
  var boothLight=Find("Luz_Bilheteria").GetComponent<Light>();boothLight.intensity=5;boothLight.shadows=LightShadows.Soft;boothLight.shadowStrength=.65f;
  for(int i=0;i<3;i++){var g=new GameObject("Luz de leitura painel "+(i+1));g.transform.SetParent(finish,false);g.transform.position=new Vector3(i==0?2.35f:i==1?5.25f:8.05f,3.14f,-22.25f);g.transform.LookAt(new Vector3(g.transform.position.x,1.65f,-21.55f));var l=g.AddComponent<Light>();l.type=LightType.Spot;l.color=new Color(1,.94f,.84f);l.intensity=2.5f;l.range=3;l.spotAngle=70;Box("Acabamento luminária painel",g.transform.position,new Vector3(.16f,.065f,.13f),graphite);}
  CurateGallery();foreach(var board in All().Where(t=>t.name=="Tabua"))board.localScale=new Vector3(.207f,.03f,1.46f);foreach(var tm in All().Select(t=>t.GetComponent<TextMesh>()).Where(t=>t)){if(!tm.GetComponent<MuseumTextDepth>())tm.gameObject.AddComponent<MuseumTextDepth>().depthShader=Shader.Find("TeatroSESI/Texto com profundidade");}PosterUpdate.Configure();Physics.SyncTransforms();Audit();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
  Capture("Polimento_Hall_Entrada",new Vector3(.2f,1.65f,-26.7f),new Vector3(2.15f,1.65f,-21.6f));Capture("Polimento_Bilheteria",new Vector3(-.1f,1.65f,-26.8f),new Vector3(-2.65f,1.6f,-24.55f));Capture("Polimento_Foyer",new Vector3(5.65f,1.65f,-26.2f),new Vector3(6.65f,1.65f,-21.55f));Capture("Polimento_Hall_Reverso",new Vector3(.1f,1.65f,-22.1f),new Vector3(-2,1.45f,-26));Capture("Polimento_Palco",new Vector3(0,1.65f,-4.2f),new Vector3(0,2.1f,1));AssetDatabase.Refresh();
 }
 static void CurateGallery(){
  var frames=All().Where(t=>t.name=="Moldura galeria representativa").OrderBy(t=>t.position.z).ToArray();
  // Mantém duas fotografias distintas; remove somente as duas cópias repetidas.
  for(int i=1;i<frames.Length-1;i++){var image=All().FirstOrDefault(t=>t.name=="Painel vazio para exposição"&&Mathf.Abs(t.position.z-frames[i].position.z)<.01f);if(image)UnityEngine.Object.DestroyImmediate(image.gameObject);UnityEngine.Object.DestroyImmediate(frames[i].gameObject);}
  frames=All().Where(t=>t.name=="Moldura galeria representativa").OrderBy(t=>t.position.z).ToArray();
  for(int i=0;i<frames.Length;i++){var frame=frames[i];var image=All().First(t=>t.name=="Painel vazio para exposição"&&Mathf.Abs(t.position.z-frame.position.z)<.01f);float z=i==0?-26.1f:-23.6f;frame.position=new Vector3(-5.36f,1.65f,z);frame.localScale=new Vector3(.055f,1.30f,1.72f);image.position=new Vector3(-5.318f,1.65f,z);image.localScale=new Vector3(.025f,1.18f,1.60f);var m=Mat(i==0?"Hall_Foto_Palco":"Hall_Foto_Fachada",Color.white,.05f);m.mainTexture=AssetDatabase.LoadAssetAtPath<Texture2D>(B+"References/"+(i==0?"IMG_8292 1.png":"IMG_8294 1.png"));image.GetComponent<Renderer>().sharedMaterial=m;}
 }
 static void MovePanel(string prefix,Vector3 p){var panel=All().First(t=>t.name.StartsWith("Painel_"+prefix));panel.position=p;panel.rotation=Quaternion.identity;var point=All().Select(t=>t.GetComponent<MuseumPoint>()).First(t=>t&&t.title.StartsWith(prefix));point.transform.position=p;}
 static void Audit(){var lines=new List<string>{"Direita da entrada = +X. Placas e painéis existentes reposicionados; não foram gerados novos pontos de interação."};foreach(var t in All().Where(t=>t.name.StartsWith("Painel_0"))){var r=t.Find("Moldura").GetComponent<Renderer>();float gap=t.name.StartsWith("Painel_04")?13.4f-r.bounds.max.x:-21.55f-r.bounds.max.z; // distância positiva em direção à sala
    
    lines.Add((gap>=-.002f&&gap<.025f?"PASS ":"FAIL ")+t.name+" afastamento parede "+gap.ToString("F3")+" m");}
   lines.Add("Controladores de visita: "+All().Count(t=>t.GetComponent<VisitaCentroSESI>())+"; sistemas de ingresso: "+All().Count(t=>t.GetComponent<MuseumExperience>()));lines.Add("Placa BILHETERIA: dois montantes conectados ao tampo; cartaz montado na parede de fundo.");File.WriteAllLines(B+"References/polimento-espacial.txt",lines);}
 public static void Capture(string n,Vector3 p,Vector3 target){var g=new GameObject("Captura temporária");var c=g.AddComponent<Camera>();c.transform.position=p;c.transform.LookAt(target);c.fieldOfView=68;c.nearClipPlane=.05f;c.clearFlags=CameraClearFlags.SolidColor;c.backgroundColor=Color.gray;var rt=new RenderTexture(1600,1000,24);var previous=RenderTexture.active;try{c.targetTexture=rt;c.Render();RenderTexture.active=rt;var texture=new Texture2D(1600,1000,TextureFormat.RGB24,false);texture.ReadPixels(new Rect(0,0,1600,1000),0,0);texture.Apply();File.WriteAllBytes(B+"Lighting/"+n+".png",texture.EncodeToPNG());UnityEngine.Object.DestroyImmediate(texture);}finally{RenderTexture.active=previous;UnityEngine.Object.DestroyImmediate(g);UnityEngine.Object.DestroyImmediate(rt);}}
}
}
