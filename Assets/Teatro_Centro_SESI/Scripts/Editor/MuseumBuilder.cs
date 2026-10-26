using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
namespace TeatroSESI {
[InitializeOnLoad] public static class MuseumBuilder {
 const string B="Assets/Teatro_Centro_SESI/";
 static Transform root;static Material stone,dark,white,orange,wood,light;
 static MuseumBuilder(){EditorApplication.delayCall+=()=>{if(File.Exists(B+"References/museum.request")){AssetDatabase.DeleteAsset(B+"References/museum.request");Build();}};}
 static Transform Group(string n,Transform parent=null){var t=new GameObject(n).transform;t.SetParent(parent?parent:root,false);return t;}
 static GameObject Box(string n,Vector3 p,Vector3 size,Material m,Transform parent,bool collision=true){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=n;g.transform.SetParent(parent,false);g.transform.localPosition=p;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=m;if(!collision)UnityEngine.Object.DestroyImmediate(g.GetComponent<Collider>());return g;}
 static void Text(string value,Vector3 p,float size,Transform parent,Quaternion? rotation=null,float maxWidth=1.25f){var t=Group(value,parent);t.localPosition=p;t.localRotation=rotation??Quaternion.identity;var tm=t.gameObject.AddComponent<TextMesh>();tm.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");tm.text=value;tm.fontSize=80;tm.characterSize=size;tm.color=new Color(.95f,.94f,.89f);tm.anchor=TextAnchor.MiddleCenter;tm.alignment=TextAlignment.Center;t.gameObject.AddComponent<MuseumTextDepth>().depthShader=Shader.Find("TeatroSESI/Texto com profundidade");var bounds=tm.GetComponent<Renderer>().localBounds;if(bounds.size.x>maxWidth)t.localScale=Vector3.one*(maxWidth/bounds.size.x);}
 static Material Mat(string name,Color color,float smooth=0){string p=B+"Materials/"+name+".mat";var m=AssetDatabase.LoadAssetAtPath<Material>(p);if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(m,p);}m.color=color;m.SetFloat("_Smoothness",smooth);m.enableInstancing=true;return m;}
 [MenuItem("Teatro SESI/Museu/Construir ou atualizar museu")]
 public static void Build(){
  var scene=SceneManager.GetSceneByPath(B+"Scenes/Teatro.unity");if(!scene.isLoaded)scene=EditorSceneManager.OpenScene(B+"Scenes/Teatro.unity",OpenSceneMode.Additive);SceneManager.SetActiveScene(scene);
  if(!File.Exists(B+"Scenes/Teatro/Antes_Museu_Editor.unity"))EditorSceneManager.SaveScene(scene,B+"Scenes/Teatro/Antes_Museu_Editor.unity",true);
  var previous=scene.GetRootGameObjects().FirstOrDefault(g=>g.name=="Museu_Interativo_SESI");if(previous)UnityEngine.Object.DestroyImmediate(previous);root=new GameObject("Museu_Interativo_SESI").transform;
  var visitor=UnityEngine.Object.FindAnyObjectByType<VisitaCentroSESI>();if(!visitor)throw new InvalidOperationException("Controlador anterior ausente; não será duplicado.");
  stone=AssetDatabase.LoadAssetAtPath<Material>(B+"Materials/Acesso_Granito_rosado.mat");dark=Mat("Museu_Grafite",new Color(.075f,.10f,.12f),.15f);white=Mat("Museu_Parede",new Color(.72f,.72f,.68f));orange=Mat("Museu_Laranja",new Color(.88f,.25f,.035f),.12f);wood=AssetDatabase.LoadAssetAtPath<Material>(B+"Materials/Madeira_natural.mat");light=Mat("Museu_Tela",new Color(.12f,.28f,.33f),.1f);light.EnableKeyword("_EMISSION");light.SetColor("_EmissionColor",new Color(.10f,.20f,.22f));
  Shell();TicketOffice();Foyer(); int photoIndex=0; foreach(var frame in UnityEngine.Object.FindObjectsByType<Renderer>().Where(r=>r.name=="Painel vazio para exposição")){string file=photoIndex++%2==0?"IMG_8294 1.png":"IMG_8292 1.png";var photo=Mat("Museu_Foto_"+(photoIndex%2),Color.white,.05f);photo.mainTexture=AssetDatabase.LoadAssetAtPath<Texture2D>(B+"References/"+file);frame.sharedMaterial=photo;}
  var experience=visitor.GetComponent<MuseumExperience>();if(!experience)experience=visitor.gameObject.AddComponent<MuseumExperience>();experience.visitor=visitor;experience.ticketIssued=false;experience.ticketValidated=false;experience.issuedCount=0;visitor.velocidade=2.2f;visitor.visitaGuiada=false;visitor.autoWalking=false;
  var gate=Group("Entrada_Auditorio");
  var oldEntry=GameObject.Find("Entrada_Teatro");if(!oldEntry)throw new InvalidOperationException("Passagem de auditório não encontrada.");
  experience.doorLeaves=oldEntry.GetComponentsInChildren<Transform>().Where(t=>t.name=="Folha auditório aberta").OrderBy(t=>t.position.x).ToArray();
  if(experience.doorLeaves.Length!=2)throw new InvalidOperationException("Esperadas duas folhas existentes para reutilização.");foreach(var leaf in experience.doorLeaves)leaf.localRotation=Quaternion.identity;
  var blocker=Group("Bloqueio_ingresso",gate);blocker.position=new Vector3(0,2.34f,-15.4f);experience.gateBlocker=blocker.gameObject.AddComponent<BoxCollider>();experience.gateBlocker.size=new Vector3(2.12f,2.04f,.14f);
  var points=new List<MuseumPoint>();points.Add(Point("Bilheteria",new Vector3(-2.5f,1.1f,-25.0f),MuseumAction.Ticket,"", "",null,"",3.0f));points.Add(Point("Acesso ao auditório",new Vector3(0,2.3f,-16.0f),MuseumAction.Gate,"","",null,"",2.5f));
  string sede="Firjan · Carta da Indústria, 'Trinta anos da Firjan Sede' (2024, atualização 2025)\ncartadaindustria.firjan.com.br/publicacoes/trinta-anos-da-firjan-sede";
  Panel("01 · A sede e a cidade",new Vector3(-4.9f,1.65f,-23.5f),Quaternion.Euler(0,-90,0),"O Centro Empresarial Arthur João Donato foi inaugurado em 12 de setembro de 1994. A nova sede ampliou o espaço das entidades da Firjan e sua integração. Esta maquete representa o percurso público do teatro; as áreas sem fotografias foram interpretadas.",sede,"IMG_8294 1.png",points);
  Panel("02 · SESI: educação e cultura",new Vector3(4.5f,1.65f,-21.62f),Quaternion.identity,"Criado em 1946, o Serviço Social da Indústria desenvolve ações de educação, saúde e cultura para trabalhadores da indústria e seus dependentes. O museu apresenta um recorte dessa atuação por meio do teatro e de seus espaços de encontro.","Firjan · Memória da Indústria, 'A criação do SESI'\nmemoriadaindustria.firjan.com.br/linha-do-tempo/industrias-base/criacao-do-sesi-nova-instituicao-de-apoio-ao-trabalhador",null,points);
  Panel("03 · Um teatro no Centro",new Vector3(8.0f,1.65f,-21.62f),Quaternion.identity,"O Teatro Firjan SESI Centro foi inaugurado em 1994. A programação reúne teatro e música. O rider informa 338 lugares: 246 na plateia e 92 no balcão. A disposição das poltronas nesta maquete é aproximada, não o mapa oficial de lugares.","Time Out, 'Teatro Firjan Sesi Centro' (19/10/2023) e rider Firjan Centro 2025\nwww.timeout.com/pt/rio-de-janeiro/teatro/teatro-firjan-sesi-centro","IMG_8292 1.png",points);
  Panel("04 · Dentro da caixa cênica",new Vector3(1.16f,2.45f,-18.6f),Quaternion.Euler(0,90,0),"A boca de cena é a abertura entre plateia e palco. No rider do Centro, ela mede 9,70 m de largura por 4,48 m de altura. O documento lista varas de iluminação, refletores e equipamentos de áudio. As cortinas e pernas delimitam a cena e escondem as áreas laterais.","Firjan · Rider Teatro Firjan SESI Centro 2025, página 1\nRider_Centro_2025.pdf (References)","IMG_8292 1.png",points,.72f);
  experience.points=points.ToArray();
  var test=visitor.GetComponent<MuseumRuntimeTest>();if(!test)test=visitor.gameObject.AddComponent<MuseumRuntimeTest>();
  visitor.transform.position=new Vector3(0,.05f,-33);visitor.transform.rotation=Quaternion.identity;visitor.cameraVisita.transform.localRotation=Quaternion.identity;
  var camera=Group("Vista_Bilheteria").gameObject.AddComponent<Camera>();camera.transform.position=new Vector3(.8f,1.7f,-26.3f);camera.transform.LookAt(new Vector3(-2.7f,1.4f,-24.5f));camera.enabled=false;camera.fieldOfView=65;camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=Color.gray;
  // Reutiliza as luzes existentes; novos pontos apenas na bilheteria.
  var lp=Group("Luz_Bilheteria");lp.position=new Vector3(-2.5f,2.95f,-24.6f);lp.rotation=Quaternion.Euler(90,0,0);var l=lp.gameObject.AddComponent<Light>();l.type=LightType.Spot;l.spotAngle=105;l.intensity=10;l.range=5;l.color=new Color(1,.89f,.76f);
  Physics.SyncTransforms();ValidateShell();AssetDatabase.SaveAssets();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);Capture(camera,B+"Lighting/Museu_Bilheteria.png");Selection.activeGameObject=root.gameObject;
 }
 static void Shell(){var t=Group("Paredes_Externas_e_Cobertura");
  Box("Lateral esquerda fechada",new Vector3(-8.12f,10,-11),new Vector3(.32f,20,33.6f),stone,t);Box("Lateral direita fechada",new Vector3(14.84f,10,-11),new Vector3(.32f,20,33.6f),stone,t);Box("Parede traseira fechada",new Vector3(3.36f,10,5.65f),new Vector3(23.3f,20,.32f),stone,t);
  Box("Cobertura integral",new Vector3(3.36f,20.05f,-11),new Vector3(23.3f,.3f,33.6f),stone,t);Box("Laje térreo integral",new Vector3(3.36f,-.19f,-11),new Vector3(23.3f,.30f,33.6f),stone,t);
  Box("Fecho superior fachada",new Vector3(3.36f,4.22f,-27.5f),new Vector3(23.3f,1.95f,.30f),stone,t);
  Box("Canto esquerdo fachada",new Vector3(-7.3f,1.63f,-27.5f),new Vector3(1.7f,3.26f,.3f),stone,t);Box("Canto direito fachada",new Vector3(14.62f,1.63f,-27.5f),new Vector3(.4f,3.26f,.3f),stone,t);
  // Completa o volume superior por trás dos panos de vidro da fachada.
  Box("Retorno superior extremo direito",new Vector3(14.6f,12.55f,-27.5f),new Vector3(.8f,14.8f,.3f),stone,t);
  Box("Retorno superior extremo esquerdo",new Vector3(-7.4f,12.55f,-27.5f),new Vector3(1.6f,14.8f,.3f),stone,t);
  Box("Piso exterior laterais",new Vector3(3.36f,-.30f,-10),new Vector3(32,.2f,48),stone,t);
 }
 static void TicketOffice(){var t=Group("Bilheteria — instalação fictícia do museu");
  Box("Balcao corpo",new Vector3(-2.65f,.5f,-24.7f),new Vector3(2.7f,1,.78f),wood,t);Box("Tampo de pedra",new Vector3(-2.65f,1.04f,-24.7f),new Vector3(2.83f,.08f,.86f),stone,t);Box("Friso laranja",new Vector3(-2.65f,.84f,-25.1f),new Vector3(2.7f,.10f,.025f),orange,t,false);
  Box("Monitor",new Vector3(-2.85f,1.44f,-24.63f),new Vector3(.65f,.42f,.065f),dark,t);Box("Tela monitor",new Vector3(-2.85f,1.44f,-24.67f),new Vector3(.59f,.35f,.012f),light,t,false);Box("Suporte monitor",new Vector3(-2.85f,1.20f,-24.63f),new Vector3(.06f,.23f,.06f),dark,t);Box("Base monitor",new Vector3(-2.85f,1.105f,-24.63f),new Vector3(.35f,.025f,.2f),dark,t);Box("Teclado",new Vector3(-2.85f,1.10f,-24.96f),new Vector3(.5f,.025f,.14f),dark,t);
  Box("Placa bilheteria",new Vector3(-2.65f,2.35f,-24.52f),new Vector3(2.8f,.63f,.08f),dark,t,false);Text("BILHETERIA",new Vector3(-2.65f,2.42f,-24.58f),.065f,t,maxWidth:2.4f);Text("MUSEU VIRTUAL · INGRESSO GRATUITO",new Vector3(-2.65f,2.18f,-24.58f),.023f,t,maxWidth:2.5f);
  for(int i=0;i<2;i++){float x=-4.6f+i*.55f;Box("Banco de espera",new Vector3(x,.43f,-26.65f),new Vector3(.5f,.12f,.45f),wood,t);Box("Encosto espera",new Vector3(x,.75f,-26.82f),new Vector3(.5f,.58f,.06f),dark,t);Box("Base banco",new Vector3(x,.22f,-26.65f),new Vector3(.14f,.43f,.33f),dark,t);}
  Box("Painel programação museu",new Vector3(-.95f,1.75f,-23.8f),new Vector3(.75f,1.15f,.08f),dark,t);Text("EM CARTAZ\n\nLUZ EM CENA\n\nEspetáculo fictício\nVisita educativa",new Vector3(-.95f,1.75f,-23.86f),.032f,t,maxWidth:.66f);
  Text("ENTRADA  →",new Vector3(0,2.6f,-21.59f),.04f,t);
 }
 static void Foyer(){var t=Group("Foyer_e_Exposicao");
  Box("Divisoria foyer lado entrada",new Vector3(3.7f,1.6f,-26.45f),new Vector3(.18f,3.2f,1.9f),white,t);Box("Divisoria foyer lado fundo",new Vector3(3.7f,1.6f,-22.3f),new Vector3(.18f,3.2f,1.7f),white,t);Box("Verga foyer",new Vector3(3.7f,2.95f,-24.4f),new Vector3(.18f,.5f,2.3f),white,t);
  Text("FOYER · EXPOSIÇÃO",new Vector3(5.25f,2.7f,-21.60f),.043f,t);
 }
 static MuseumPoint Point(string title,Vector3 p,MuseumAction action,string body,string source,Texture2D image,string credit,float radius){var t=Group("Interacao_"+title);t.position=p;var point=t.gameObject.AddComponent<MuseumPoint>();point.title=title;point.action=action;point.body=body;point.source=source;point.image=image;point.credit=credit;point.radius=radius;return point;}
 static void Panel(string title,Vector3 p,Quaternion rotation,string body,string source,string image,List<MuseumPoint> points,float width=1.5f){var t=Group("Painel_"+title);t.position=p;t.rotation=rotation;Box("Moldura",Vector3.zero,new Vector3(width,1.13f,.07f),dark,t);Box("Friso",new Vector3(0,.48f,-.041f),new Vector3(width,.08f,.01f),orange,t,false);Text(title.Replace(" · ","\n"),new Vector3(0,.16f,-.05f),width*.033f,t,maxWidth:width*.9f);Text("[E] CONHECER",new Vector3(0,-.32f,-.05f),width*.025f,t,maxWidth:width*.85f);var point=Point(title,p,MuseumAction.Exhibit,body,source,image==null?null:AssetDatabase.LoadAssetAtPath<Texture2D>(B+"References/"+image),"Fotografia fornecida pelo usuário.\nRegistro contemporâneo, não histórico.",1.9f);points.Add(point);}
 static void ValidateShell(){var rows=new List<string>();bool ok=true;foreach(var pair in new[]{(new Vector3(-7.8f,10,-10),Vector3.left),(new Vector3(14.4f,10,-10),Vector3.right),(new Vector3(0,10,5),Vector3.forward),(new Vector3(0,19,-10),Vector3.up)}){bool hit=Physics.Raycast(pair.Item1,pair.Item2,out var result,2);ok&=hit;rows.Add((hit?"PASS ":"FAIL ")+(hit?result.collider.name:"fecho ausente"));}rows.Insert(0,"Fechamento lateral, traseiro e cobertura: "+(ok?"PASS":"FAIL"));File.WriteAllLines(B+"References/museu-envelope.txt",rows);}
 [MenuItem("Teatro SESI/Museu/Testar fluxo completo %#m")]
 public static void Test(){File.WriteAllText(B+"References/museum-test.request","run");EditorApplication.isPlaying=true;}
 static void Capture(Camera c,string p){var rt=new RenderTexture(1600,1000,24);var active=RenderTexture.active;c.targetTexture=rt;try{c.Render();RenderTexture.active=rt;var tex=new Texture2D(1600,1000,TextureFormat.RGB24,false);tex.ReadPixels(new Rect(0,0,1600,1000),0,0);tex.Apply();File.WriteAllBytes(p,tex.EncodeToPNG());UnityEngine.Object.DestroyImmediate(tex);}finally{c.targetTexture=null;RenderTexture.active=active;UnityEngine.Object.DestroyImmediate(rt);}AssetDatabase.Refresh();}
}
}




