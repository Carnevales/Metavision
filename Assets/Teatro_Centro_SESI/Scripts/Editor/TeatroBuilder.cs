using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
namespace TeatroSESI {
[InitializeOnLoad] public static class TeatroBuilder {
 const string B="Assets/Teatro_Centro_SESI/";
 static Transform root; static Material black,cloth,stone,wood,metal,seat,carpet,red,glow;
 static int meshId;
 static TeatroBuilder(){ EditorApplication.delayCall += ()=> { if(File.Exists(B+"References/build.request")) { AssetDatabase.DeleteAsset(B+"References/build.request"); CreateScene(); } }; }
 [MenuItem("Teatro SESI/Criar ou abrir cena do teatro")]
 public static void CreateScene(){
  string path=B+"Scenes/Teatro.unity";
  if(File.Exists(path)) { var existing=SceneManager.GetSceneByPath(path); if(!existing.isLoaded)existing=EditorSceneManager.OpenScene(path,OpenSceneMode.Additive); SceneManager.SetActiveScene(existing); foreach(var g in existing.GetRootGameObjects()){var settings=g.GetComponent<TeatroDimensions>();if(settings){Build(settings);EditorSceneManager.SaveScene(existing);RenderViews();return;}} }
  var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive); SceneManager.SetActiveScene(scene);
  var go=new GameObject("Teatro_Centro_SESI"); var d=go.AddComponent<TeatroDimensions>(); Build(d);
  EditorSceneManager.SaveScene(scene,path); RenderViews();
 }
 [MenuItem("Teatro SESI/Atualizar teatro selecionado")]
 public static void Rebuild(){var d=Selection.activeGameObject?Selection.activeGameObject.GetComponentInParent<TeatroDimensions>():null; if(!d) { Debug.LogWarning("Selecione Teatro_Centro_SESI."); return; } Build(d); EditorSceneManager.MarkSceneDirty(d.gameObject.scene);}
 static Material Mat(string name,Color c,float smooth=0){
  var path=B+"Materials/"+name+".mat"; var m=AssetDatabase.LoadAssetAtPath<Material>(path);
  if(!m){ m=new Material(Shader.Find("Universal Render Pipeline/Lit")); AssetDatabase.CreateAsset(m,path); }
  m.color=c; m.SetFloat("_Smoothness",smooth); m.enableInstancing=true; return m;
 }
 static GameObject Group(string n,Transform parent=null){var g=new GameObject(n);g.transform.SetParent(parent?parent:root,false);return g;}
 static GameObject Box(string n,Vector3 p,Vector3 s,Material m,Transform parent=null){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=n;g.transform.SetParent(parent?parent:root,false);g.transform.localPosition=p;g.transform.localScale=s;g.GetComponent<Renderer>().sharedMaterial=m;return g;}
 static GameObject Tube(string n,Vector3 a,Vector3 b,float radius,Material m,Transform parent=null){var g=GameObject.CreatePrimitive(PrimitiveType.Cylinder);g.name=n;g.transform.SetParent(parent?parent:root,false);g.transform.localPosition=(a+b)*.5f;g.transform.localScale=new Vector3(radius*2,Vector3.Distance(a,b)*.5f,radius*2);g.transform.up=(b-a).normalized;g.GetComponent<Renderer>().sharedMaterial=m;UnityEngine.Object.DestroyImmediate(g.GetComponent<Collider>());return g;}
 static void Build(TeatroDimensions d){
  root=d.transform; for(int i=root.childCount-1;i>=0;i--)UnityEngine.Object.DestroyImmediate(root.GetChild(i).gameObject);meshId=0;
  black=Mat("Estrutura_antracite",new Color(.025f,.028f,.033f)); cloth=Mat("Veludo_preto",new Color(.012f,.014f,.019f)); stone=Mat("Granito_bege",new Color(.43f,.40f,.34f)); wood=Mat("Madeira_natural",new Color(.65f,.39f,.19f),.27f);
  wood.mainTexture=AssetDatabase.LoadAssetAtPath<Texture2D>(B+"Textures/fine_grained_wood_diff_1k.jpg"); wood.color=new Color(1,.85f,.65f); metal=Mat("Aco_grafite",new Color(.10f,.11f,.12f),.45f);seat=Mat("Estofado_azul_escuro",new Color(.028f,.041f,.065f));carpet=Mat("Carpete_cinza",new Color(.32f,.33f,.32f));red=Mat("Veludo_vinho",new Color(.25f,.018f,.026f));glow=Mat("Balizamento_azul",new Color(.04f,.12f,.8f));glow.EnableKeyword("_EMISSION");glow.SetColor("_EmissionColor",new Color(.04f,.15f,2));
  float w=d.larguraSala,h=d.alturaPalco,front=-2.32f,back=front+d.profundidade;
  var arch=Group("01_Arquitetura").transform;
  Box("Piso plateia",new Vector3(0,-.15f,-8),new Vector3(w,.3f,14),carpet,arch);
  Box("Palco — profundidade 7.30 m",new Vector3(0,h*.5f,(front+back)/2),new Vector3(12,h,d.profundidade),stone,arch);
  var floor=Group("Assoalho individual",arch).transform;
  for(int x=0;x<58;x++) for(int z=0;z<5;z++)Box("Tabua",new Vector3(-5.94f+x*.207f,h+.015f,front+.73f+z*1.46f),new Vector3(.202f,.03f,1.455f),wood,floor);
  for(int s=-1;s<=1;s+=2){
   Box("Parede lateral",new Vector3(s*w/2,3.3f,-5.2f),new Vector3(.24f,8,22),black,arch);
   for(int i=0;i<53;i++)Box("Lamelas de madeira",new Vector3(s*(w/2-.14f),3,-14+i*.34f),new Vector3(.045f,6,.326f),wood,arch);
   Box("Ombreira mineral",new Vector3(s*(d.boca/2+.23f),h+d.alturaBoca/2,front+.2f),new Vector3(.46f,d.alturaBoca,.45f),stone,arch);
   Curtain("Faixa vinho lateral",s*(d.boca/2+.5f),h,front+.25f,.30f,d.alturaBoca,red);
   for(int k=0;k<5;k++) {var sp=Box("PA suspenso modulo",new Vector3(s*6.6f,4.6f-k*.22f,-2.1f),new Vector3(.62f,.20f,.43f),black);Box("Grade PA",sp.transform.localPosition+new Vector3(0,0,-.223f),new Vector3(.56f,.15f,.015f),metal);}
  }
  Box("Teto preto",new Vector3(0,7.05f,-5),new Vector3(w,.18f,22),black,arch);
  Box("Parede fundo palco",new Vector3(0,3.4f,back+.3f),new Vector3(w,6.8f,.3f),black,arch);
  Box("Verga boca",new Vector3(0,h+d.alturaBoca+.38f,front+.15f),new Vector3(10.65f,.76f,.50f),black,arch);
  Curtain("Fundo preto pregueado",0,h,back-.18f,10,d.alturaBoca,cloth);
  for(int s=-1;s<=1;s+=2)for(int j=0;j<3;j++)Curtain("Perna preta coxia",s*4.65f,h,front+.8f+j*1.9f,.95f,d.alturaBoca,cloth);
  for(int i=0;i<5;i++)Box("Escada central degrau",new Vector3(0,(i+1)*h/10,front-1.25f+i*.25f),new Vector3(1.42f,(i+1)*h/5,.25f),carpet,arch);
  var chairs=Group("02_Plateia estimada").transform;
  var prototype=Chair(); var prefab=PrefabUtility.SaveAsPrefabAsset(prototype,B+"Prefabs/Poltrona.prefab"); UnityEngine.Object.DestroyImmediate(prototype);
  for(int r=0;r<d.fileiras;r++){
   float z=-4.05f-r*d.passoFileira,y=r*.12f;
   Box("Patamar fileira "+(r+1),new Vector3(0,y-.08f,z-.25f),new Vector3(w-.4f,.16f,d.passoFileira),carpet,chairs);
   for(int s=-1;s<=1;s+=2)for(int c=0;c<10;c++) {var g=(GameObject)PrefabUtility.InstantiatePrefab(prefab,chairs);g.name="Poltrona "+(r+1)+"-"+s+"-"+c;g.transform.localPosition=new Vector3(s*(1.03f+c*.61f),y,z);g.transform.localRotation=Quaternion.Euler(0,-s*c*.65f,0);}
   for(int i=0;i<2;i++)Box("LED corredor",new Vector3(i==0?-.73f:.73f,y+.012f,z),new Vector3(.025f,.018f,.085f),glow,chairs);
  }
  Balcony(w);
  Equipment(h,front);
  Lighting(w,h);
  CameraView("Camera_Inspecao_Frente",new Vector3(0,3.05f,-12.6f),new Vector3(0,2.5f,.8f),57,true);
  CameraView("Camera_Inspecao_Palco",new Vector3(0,2.8f,2.8f),new Vector3(0,2.8f,-12),64,false);
  CameraView("Camera_Inspecao_Balcao",new Vector3(5.4f,6.4f,-11.0f),new Vector3(0,2.3f,-.5f),65,false);
  RenderSettings.skybox=null;RenderSettings.ambientMode=AmbientMode.Flat;RenderSettings.ambientLight=new Color(.48f,.49f,.53f);
  DynamicGI.UpdateEnvironment();AssetDatabase.SaveAssets();Selection.activeGameObject=d.gameObject;
  if(SceneView.lastActiveSceneView)SceneView.lastActiveSceneView.LookAt(new Vector3(0,2,-2),Quaternion.Euler(12,0,0),16);
  int missing=0;foreach(var renderer in root.GetComponentsInChildren<Renderer>())foreach(var material in renderer.sharedMaterials)if(!material||!material.shader||!material.shader.isSupported)missing++;
  File.WriteAllText(B+"References/validation.txt","Construção executada no Editor: "+DateTime.Now+"\nRenderers: "+root.GetComponentsInChildren<Renderer>().Length+"\nMateriais ausentes ou shaders incompatíveis: "+missing+"\nBoca: "+d.boca+" x "+d.alturaBoca+"; profundidade: "+d.profundidade+"\nSem assets externos de modelos. Madeira Poly Haven CC0.\nDistribuição da plateia aproximada, não mapa de lugares oficial.");
 }
 static GameObject Chair(){var g=Group("Poltrona"); var t=g.transform;
  Box("Base madeira",new Vector3(0,.36f,0),new Vector3(.48f,.13f,.48f),wood,t);
  var cushion=Rounded("Assento estofado",new Vector3(0,.46f,-.015f),new Vector3(.47f,.12f,.43f),seat,t);
  var back=Rounded("Encosto curvo",new Vector3(0,.78f,-.22f),new Vector3(.49f,.64f,.10f),black,t);back.transform.localRotation=Quaternion.Euler(-6,0,0);
  Rounded("Almofada encosto",new Vector3(0,.79f,-.155f),new Vector3(.435f,.51f,.075f),seat,t);
  for(int s=-1;s<=1;s+=2){Box("Lateral madeira",new Vector3(s*.265f,.38f,-.02f),new Vector3(.045f,.59f,.40f),wood,t);Box("Braco",new Vector3(s*.27f,.68f,0),new Vector3(.075f,.065f,.45f),metal,t);Tube("Suporte",new Vector3(s*.19f,.03f,0),new Vector3(s*.19f,.34f,0),.025f,metal,t);}
  return g;
 }
 static GameObject Rounded(string name,Vector3 p,Vector3 size,Material mat,Transform parent){
  var verts=new List<Vector3>();var tris=new List<int>();float r=Mathf.Min(size.x,size.y)*.16f;
  for(int side=0;side<2;side++)for(int corner=0;corner<4;corner++)for(int j=0;j<5;j++){float a=(corner*90+j*22.5f)*Mathf.Deg2Rad;float cx=corner==0||corner==3?size.x/2-r:-size.x/2+r;float cy=corner<2?size.y/2-r:-size.y/2+r;verts.Add(new Vector3(cx+Mathf.Cos(a)*r,cy+Mathf.Sin(a)*r,(side==0?-.5f:.5f)*size.z));}
  for(int i=1;i<19;i++){tris.AddRange(new[]{0,i+1,i,20,20+i,21+i});}for(int i=0;i<20;i++){int j=(i+1)%20;tris.AddRange(new[]{i,j,20+i,j,20+j,20+i});}
  var mesh=new Mesh{name=name};mesh.SetVertices(verts);mesh.SetTriangles(tris,0);mesh.RecalculateNormals();mesh.RecalculateBounds();string path=B+"Models/Poltrona_"+name.Replace(" ","_")+".asset";mesh.name=Path.GetFileNameWithoutExtension(path);var old=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(old){EditorUtility.CopySerialized(mesh,old);UnityEngine.Object.DestroyImmediate(mesh);mesh=old;}else AssetDatabase.CreateAsset(mesh,path);
  var g=Group(name,parent);g.transform.localPosition=p;g.AddComponent<MeshFilter>().sharedMesh=mesh;g.AddComponent<MeshRenderer>().sharedMaterial=mat;return g;
 }
 static void Curtain(string name,float x,float y,float z,float width,float height,Material mat){
  int n=Mathf.CeilToInt(width*32);var v=new Vector3[(n+1)*2];var uv=new Vector2[v.Length];var tris=new int[n*6];
  for(int i=0;i<=n;i++){float u=(float)i/n;float zz=Mathf.Sin(u*width*24)*.065f+Mathf.Sin(u*width*48)*.012f;v[i*2]=new Vector3((u-.5f)*width,0,zz);v[i*2+1]=new Vector3((u-.5f)*width,height,zz);uv[i*2]=new Vector2(u,0);uv[i*2+1]=new Vector2(u,1);if(i<n){int a=i*2,b=i*6;tris[b]=a;tris[b+1]=a+1;tris[b+2]=a+2;tris[b+3]=a+2;tris[b+4]=a+1;tris[b+5]=a+3;}}
  var mesh=new Mesh{name=name};mesh.vertices=v;mesh.uv=uv;mesh.triangles=tris;mesh.RecalculateNormals();mesh.RecalculateBounds();string path=B+"Models/Cortina_"+(meshId++)+".asset";mesh.name=Path.GetFileNameWithoutExtension(path);var old=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(old){EditorUtility.CopySerialized(mesh,old);UnityEngine.Object.DestroyImmediate(mesh);mesh=old;}else AssetDatabase.CreateAsset(mesh,path);
  var go=Group(name);go.transform.localPosition=new Vector3(x,y,z);go.AddComponent<MeshFilter>().sharedMesh=mesh;go.AddComponent<MeshRenderer>().sharedMaterial=mat;
 }
 static void Balcony(float w){var t=Group("03_Balcao — estimativa fotografica").transform;
  Box("Laje balcao",new Vector3(0,3.65f,-13),new Vector3(w,.3f,4.4f),black,t);Box("Testeira",new Vector3(0,3.92f,-10.8f),new Vector3(w,.7f,.18f),black,t);
  Tube("Corrimao",new Vector3(-w/2,4.55f,-10.8f),new Vector3(w/2,4.55f,-10.8f),.028f,metal,t);
  var prefab=AssetDatabase.LoadAssetAtPath<GameObject>(B+"Prefabs/Poltrona.prefab");for(int r=0;r<4;r++)for(int s=-1;s<=1;s+=2)for(int c=0;c<(r==3?10:12);c++){var g=(GameObject)PrefabUtility.InstantiatePrefab(prefab,t);g.transform.localPosition=new Vector3(s*(.85f+c*.55f),3.85f+r*.20f,-11.45f-r*.83f);}
  Box("Parede traseira",new Vector3(0,3.4f,-15.5f),new Vector3(w,7,.22f),black,t);
  for(int s=-1;s<=1;s+=2){Box("Porta traseira",new Vector3(s*1.5f,1.1f,-15.34f),new Vector3(1.3f,2.2f,.06f),red,t);for(int j=0;j<3;j++)Curtain("Painel vinho traseiro",s*(3+j*1.7f),0,-15.32f,1.3f,3.5f,red);}
  Box("Cabine tecnica",new Vector3(0,5.65f,-15.15f),new Vector3(2.5f,1.4f,.18f),wood,t);Box("Vidro cabine",new Vector3(0,5.8f,-15.03f),new Vector3(2.2f,.92f,.035f),black,t);
 }
 static void Equipment(float h,float f){var t=Group("04_Equipamentos").transform;
  var piano=Group("Piano de cauda — aproximacao foto IMG_8292",t).transform;piano.localPosition=new Vector3(-3.8f,h,.8f);
  Rounded("Corpo piano",new Vector3(0,.75f,.3f),new Vector3(1.4f,.32f,1.8f),black,piano);
  Box("Teclado",new Vector3(0,.79f,-.62f),new Vector3(1.24f,.045f,.26f),stone,piano);
  for(int i=0;i<35;i++)Box("Tecla branca",new Vector3(-.6f+i*.035f,.82f,-.63f),new Vector3(.033f,.025f,.22f),Mat("Marfim",new Color(.85f,.84f,.77f),.4f),piano);
  for(int i=0;i<34;i++)if(i%7!=2&&i%7!=6)Box("Tecla preta",new Vector3(-.583f+i*.035f,.845f,-.58f),new Vector3(.018f,.03f,.12f),black,piano);
  for(int i=0;i<3;i++)Tube("Perna piano",new Vector3(i==0?-.5f:.5f,.05f,i==2?1:-.35f),new Vector3(i==0?-.5f:.5f,.64f,i==2?1:-.35f),.045f,black,piano);
  var lid=Box("Tampa piano aberta",new Vector3(-.1f,1.25f,.35f),new Vector3(1.35f,.045f,1.8f),wood,piano);lid.transform.localRotation=Quaternion.Euler(0,0,32);
  Box("Banco piano",new Vector3(0,.42f,-1.1f),new Vector3(.65f,.13f,.36f),black,piano);
  for(int i=0;i<4;i++)Tube("Pe banco",new Vector3(i%2==0?-.25f:.25f,.04f,i<2?-1.23f:-.97f),new Vector3(i%2==0?-.25f:.25f,.36f,i<2?-1.23f:-.97f),.025f,black,piano);
  for(int j=0;j<4;j++){float z=f+.5f+j*1.7f;Tube("Vara iluminacao",new Vector3(-4.8f,5.2f,z),new Vector3(4.8f,5.2f,z),.035f,metal,t);for(int k=0;k<7;k++){float x=-4.1f+k*1.36f;Tube("Refletor",new Vector3(x,4.96f,z),new Vector3(x,4.68f,z-.16f),.105f,black,t);Box("Suporte refletor",new Vector3(x,5.06f,z),new Vector3(.26f,.15f,.03f),metal,t);}}
  for(int s=-1;s<=1;s+=2){var m=Box("Monitor de palco",new Vector3(s*2.8f,h+.2f,f+.35f),new Vector3(.65f,.38f,.45f),black,t);m.transform.localRotation=Quaternion.Euler(-18,0,0);}
  for(int i=0;i<7;i++){float x=-2.5f+i*.85f;Tube("Pedestal microfone",new Vector3(x,h,.1f),new Vector3(x,h+1.45f,.1f),.012f,metal,t);Tube("Microfone",new Vector3(x,h+1.45f,.1f),new Vector3(x,h+1.58f,.14f),.026f,metal,t);for(int j=0;j<3;j++){float a=j*Mathf.PI*2/3;Tube("Pe tripé",new Vector3(x,h+.15f,.1f),new Vector3(x+Mathf.Cos(a)*.24f,h+.025f,.1f+Mathf.Sin(a)*.24f),.012f,metal,t);}}
 }
 static void Lighting(float w,float h){var t=Group("05_Iluminacao").transform;
  var fill=Group("Preenchimento indireto aproximado",t);fill.transform.localRotation=Quaternion.Euler(65,-25,0);var fl=fill.AddComponent<Light>();fl.type=LightType.Directional;fl.intensity=.8f;fl.color=new Color(1,.92f,.82f);fl.shadows=LightShadows.None;
  for(int s=-1;s<=1;s+=2)for(int i=0;i<3;i++){var g=Group("Luz sob balcao",t);g.transform.localPosition=new Vector3(s*3,3.4f,-11-i*1.4f);var l=g.AddComponent<Light>();l.type=LightType.Point;l.intensity=4;l.range=5;l.color=new Color(1,.9f,.78f);}
  for(int z=0;z<5;z++)for(int s=-1;s<=1;s+=2){var p=new Vector3(s*4.2f,6.8f,-4-z*2.1f);Tube("Luminaria teto",p,p+Vector3.down*.06f,.16f,stone,t);var g=Group("Luz geral quente",t);g.transform.localPosition=p+Vector3.down*.12f;var l=g.AddComponent<Light>();l.type=LightType.Point;l.color=new Color(1,.85f,.66f);l.intensity=2.8f;l.range=7;}
  for(int i=0;i<6;i++){var g=Group("Geral palco",t);g.transform.localPosition=new Vector3(-4+i*1.6f,5.15f,-2);g.transform.LookAt(new Vector3(-3+i*1.2f,h,1.5f));var l=g.AddComponent<Light>();l.type=LightType.Spot;l.spotAngle=65;l.intensity=28;l.range=12;l.color=new Color(1,.87f,.71f);l.shadows=i%2==0?LightShadows.Soft:LightShadows.None;}
 }
 static void CameraView(string name,Vector3 p,Vector3 target,float fov,bool active){var g=Group(name);g.transform.localPosition=p;g.transform.LookAt(target);var c=g.AddComponent<Camera>();c.fieldOfView=fov;c.nearClipPlane=.1f;c.farClipPlane=80;c.clearFlags=CameraClearFlags.SolidColor;c.backgroundColor=Color.black;c.enabled=active;c.depth=active?10:0;}
 [MenuItem("Teatro SESI/Renderizar vistas de verificacao")]
 public static void RenderViews(){if(!root){var d=UnityEngine.Object.FindAnyObjectByType<TeatroDimensions>();if(!d)return;root=d.transform;} foreach(var c in root.GetComponentsInChildren<Camera>()){var rt=new RenderTexture(1600,1000,24);c.targetTexture=rt;c.Render();RenderTexture.active=rt;var tex=new Texture2D(1600,1000,TextureFormat.RGB24,false);tex.ReadPixels(new Rect(0,0,1600,1000),0,0);tex.Apply();File.WriteAllBytes(B+"Lighting/"+c.name+".png",tex.EncodeToPNG());c.targetTexture=null;RenderTexture.active=null;UnityEngine.Object.DestroyImmediate(tex);UnityEngine.Object.DestroyImmediate(rt);}AssetDatabase.Refresh();}
}
}



