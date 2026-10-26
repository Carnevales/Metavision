using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace Maracannes {
public static partial class MaracannesBuilder {
 [MenuItem("Maracannes/Adicionar sala 2024 Experience")]
 public static void AddExperience2024(){
  EditorSceneManager.SaveOpenScenes();scene=EditorSceneManager.OpenScene(AcervoTravel.Acervo,OpenSceneMode.Single);root=scene.GetRootGameObjects().First(g=>g.name=="Maracannes_Acervo").transform;
  if(root.GetComponentsInChildren<MaracannesRoom>().Any(r=>r.number==5))return;
  AssetDatabase.CopyAsset(AcervoTravel.Acervo,B+"Documentation/Arquivo_Anterior/Acervo_Antes_Experience2024.unity");SetupMaterials();
  var all=root.GetComponentsInChildren<Transform>(true);var structure=all.First(t=>t.name=="Estrutura_Continua_Quatro_Salas");
  // Substituições pontuais: parede leste recebe dois portais; o vão antigo 02-2025 fecha.
  foreach(var r in structure.GetComponentsInChildren<Renderer>()){if(Mathf.Abs(r.transform.position.x-54)<.03f&&r.bounds.size.z>30||r.name=="Rodapé lateral leste"){r.enabled=false;foreach(var c in r.GetComponents<Collider>())c.enabled=false;}}
  var room2=root.GetComponentsInChildren<MaracannesRoom>().First(r=>r.number==2).transform;var brasil=root.GetComponentsInChildren<MaracannesRoom>().First(r=>r.number==3).transform;
  var coating=room2.Find("Revestimento vinho leste");if(coating){coating.GetComponent<Renderer>().enabled=false;}
  var ext=Group("Extensao_Experience2024",root);
  WallX(54,-32,-19.2f,3.8f,wall,ext);WallX(54,-16.8f,-9.2f,3.8f,wall,ext);WallX(54,-6.8f,6,3.8f,wall,ext);WallX(54,-19.2f,-16.8f,.8f,wall,ext,3);WallX(54,-9.2f,-6.8f,.8f,wall,ext,3);
  foreach(var interval in new[]{new Vector2(-31.86f,-19.2f),new Vector2(-16.8f,-12.14f)})Box("Acabamento vinho com vão livre",new Vector3(53.86f,1.9f,(interval.x+interval.y)/2),new Vector3(.02f,3.8f,interval.y-interval.x),red,ext,false);
  Portal(new Vector3(54,0,-18),Quaternion.Euler(0,90,0),ext);Portal(new Vector3(54,0,-8),Quaternion.Euler(0,90,0),ext);
  WallZ(-12,41.8f,44.2f,3.8f,wall,ext);foreach(var portal in structure.GetComponentsInChildren<Transform>().Where(t=>t.name=="Portal_aberto"&&Vector3.Distance(t.position,new Vector3(43,0,-12))<.1f).ToArray())UnityEngine.Object.DestroyImmediate(portal.gameObject);
  var displaced=room2.GetComponentsInChildren<MaracannesExhibit>().Where(e=>e.transform.position.x>53&&Mathf.Abs(e.transform.position.z+18)<1.85f).OrderBy(e=>e.transform.position.z).ToArray();for(int i=0;i<displaced.Length;i++){displaced[i].transform.SetPositionAndRotation(new Vector3(42.1f+i*1.8f,1.75f,-12.18f),Quaternion.identity);displaced[i].inspectionPosition=new Vector3(42.1f+i*1.8f,0,-14);}
  foreach(var e in brasil.GetComponentsInChildren<MaracannesExhibit>().Where(e=>e.transform.position.x>53&&Mathf.Abs(e.transform.position.z+8)<1.8f)){e.transform.SetPositionAndRotation(new Vector3(32.18f,1.75f,1.5f),Quaternion.Euler(0,-90,0));e.inspectionPosition=new Vector3(34,0,1.5f);}
  var r2024=Room(5,"2024 · Maracannes Experience",new Rect(54,-24,20,22));WallZ(-24,54,74,3.8f,wall,r2024);WallZ(-2,54,74,3.8f,wall,r2024);WallX(74,-24,-2,3.8f,wall,r2024);
  var burgundy=Mat("Experience_Veludo_Borgonha",new Color(.19f,.018f,.035f));var carpet=Mat("Experience_Tapete_Vermelho",new Color(.34f,.015f,.025f));var floor=Mat("Experience_Piso_Carvao",new Color(.13f,.12f,.12f),.22f);r2024.Find("Laje piso sala 5").GetComponent<Renderer>().sharedMaterial=floor;
  Box("Parede cenográfica borgonha",new Vector3(73.86f,1.9f,-13),new Vector3(.025f,3.8f,21.7f),burgundy,r2024,false);
  Plaque("Identidade 2024","03  /  MARACANNES EXPERIENCE","2024 · UMA GALERIA DE CINEMA",new Vector3(64,3.22f,-23.8f),Quaternion.Euler(0,180,0),r2024,13,.66f);
  Plaque("Saída para 2025","04  ←  BRASIL EM CENA","MARACANNES 2025",new Vector3(54.18f,3.3f,-8),Quaternion.Euler(0,-90,0),r2024,4.6f,.5f);
  Plaque("Entrada Experience","03  →  EXPERIENCE","MARACANNES 2024",new Vector3(53.80f,3.3f,-18),Quaternion.Euler(0,90,0),room2,4.8f,.55f);
  var oldNext=room2.Find("Próxima sala");if(oldNext){oldNext.GetComponentInChildren<TextMesh>().text="03  →  EXPERIENCE 2024";oldNext.position=new Vector3(49,3.35f,-12.17f);}
  foreach(var tm in brasil.GetComponentsInChildren<TextMesh>()){if(tm.text=="03  /  BRASIL EM CENA")tm.text="04  /  BRASIL EM CENA";if(tm.text.StartsWith("04 ")&&tm.text.Contains("MEMÓRIAS"))tm.text=tm.text.Replace("04","05");}
  var memory=root.GetComponentsInChildren<MaracannesRoom>().First(r=>r.number==4);foreach(var tm in memory.GetComponentsInChildren<TextMesh>())if(tm.text=="04  /  MEMÓRIAS DA ARTE")tm.text="05  /  MEMÓRIAS DA ARTE";
  Box("Tapete de chegada",new Vector3(57,.012f,-13),new Vector3(2.5f,.024f,13),carpet,r2024,false);
  for(int side=-1;side<=1;side+=2){for(int i=0;i<5;i++){var p=new Vector3(57+side*1.65f,0,-16.5f+i*1.8f);Bar("Base do balizador",p+Vector3.up*.03f,p+Vector3.up*.10f,.16f,brass,r2024);Bar("Balizador de latão",p+Vector3.up*.1f,p+Vector3.up*.88f,.025f,brass,r2024);Sphere("Terminal",p+Vector3.up*.9f,Vector3.one*.10f,brass,r2024);if(i<4)Bar("Cordão de veludo",p+Vector3.up*.84f,p+new Vector3(0,.84f,1.8f),.025f,burgundy,r2024);}}
  for(int i=0;i<5;i++){var verts=new List<Vector3>{Vector3.zero};for(int j=0;j<10;j++){float a=j*Mathf.PI/5;float radius=j%2==0?.42f:.19f;verts.Add(new Vector3(Mathf.Sin(a)*radius,0,Mathf.Cos(a)*radius));}var tris=new List<int>();for(int j=0;j<10;j++)tris.AddRange(new[]{0,1+j,1+(j+1)%10});var star=MeshObject("Estrela cenográfica original",Store("Experience_Estrela",new Mesh{vertices=verts.ToArray(),triangles=tris.ToArray()}),brass,r2024);star.transform.position=new Vector3(57,.027f,-18+i*2.5f);}
  var island=Group("Ilha_Producao_Cinematografica",r2024);Pedestal(new Vector3(65,0,-15),new Vector3(4,.35f,3),island);Projector(new Vector3(64,.36f,-15),island);Clapper(new Vector3(65.6f,.36f,-15.6f),island);Reel(new Vector3(65.7f,.8f,-14.5f),.48f,island,brass);
  Pedestal(new Vector3(69,0,-8),new Vector3(1.5f,.8f,1.5f),r2024);CameraModel(new Vector3(69,.81f,-8),r2024);
  Pedestal(new Vector3(65,0,-7),new Vector3(1.4f,.65f,1.4f),r2024);Sculpture("Prêmio cenográfico · criação original",new Vector3(65,.66f,-7),r2024,brass,1.65f);
  for(int i=0;i<2;i++){var c=Group("Cadeira de direção cenográfica",r2024);c.position=new Vector3(62+i*2.2f,0,-20);for(int s=-1;s<=1;s+=2){Bar("Pé cruzado",new Vector3(s*.35f,0,-.3f),new Vector3(-s*.35f,.65f,.3f),.025f,walnut,c);Bar("Pé cruzado",new Vector3(s*.35f,0,.3f),new Vector3(-s*.35f,.65f,-.3f),.025f,walnut,c);Bar("Encosto",new Vector3(s*.35f,.6f,.3f),new Vector3(s*.35f,1.15f,.3f),.025f,walnut,c);}Box("Assento de lona",new Vector3(0,.65f,0),new Vector3(.75f,.06f,.65f),graphite,c);Box("Encosto de lona",new Vector3(0,1,.3f),new Vector3(.72f,.28f,.06f),graphite,c);}
  for(int s=-1;s<=1;s+=2){float x=65+s*4.2f;Bar("Coluna de truss",new Vector3(x,0,-12.8f),new Vector3(x,3.5f,-12.8f),.045f,graphite,r2024);if(s==-1)Bar("Travessa de truss",new Vector3(60.8f,3.5f,-12.8f),new Vector3(69.2f,3.5f,-12.8f),.045f,graphite,r2024);if(s==-1)for(int j=0;j<3;j++){var pos=new Vector3(62+j*3,3.35f,-12.8f);Bar("Corpo do refletor",pos,pos+new Vector3(0,-.2f,.15f),.14f,graphite,r2024);Spot(pos,new Vector3(64+j,1,-15),new Color(1,.84f,.62f),5,8,65,r2024,false);}}
  var catalog=JsonUtility.FromJson<Catalog>(File.ReadAllText(B+"Documentation/experience2024.json"));var slots=new List<System.Tuple<Vector3,Quaternion>>();foreach(float x in new[]{58f,62,66,70})slots.Add(System.Tuple.Create(new Vector3(x,1.9f,-2.18f),Quaternion.identity));foreach(float z in new[]{-20f,-15,-10})slots.Add(System.Tuple.Create(new Vector3(73.80f,1.9f,z),Quaternion.Euler(0,90,0)));foreach(float x in new[]{60f,68})slots.Add(System.Tuple.Create(new Vector3(x,1.9f,-23.82f),Quaternion.Euler(0,180,0)));
  for(int i=0;i<catalog.posters.Length;i++){var r=catalog.posters[i];var imp=AssetImporter.GetAtPath(r.path) as TextureImporter;imp.npotScale=TextureImporterNPOTScale.None;imp.maxTextureSize=4096;imp.textureCompression=TextureImporterCompression.Uncompressed;imp.mipmapEnabled=true;imp.SaveAndReimport();float h=2.05f,w=h*r.width/r.height;var t=Group(r.id+" · "+r.sourceName,r2024);t.SetPositionAndRotation(slots[i].Item1,slots[i].Item2);Box("Moldura do cartaz",Vector3.zero,new Vector3(w+.12f,h+.12f,.075f),brass,t);var image=MeshObject("Cartaz original integral",Quad("Quad_"+r.id,w,h),PosterMaterial(r),t);image.transform.localPosition=new Vector3(0,0,-.042f);var e=t.gameObject.AddComponent<MaracannesExhibit>();e.catalogId=r.id;e.room=5;e.originalPath=r.original;e.sha256=r.sourceHash;e.inspectionPosition=t.position-t.forward*1.7f;}
  PolishExperience2024();root.gameObject.AddComponent<Experience2024Audit>();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();File.WriteAllText(B+"Documentation/experience2024-construcao.txt","Sala Experience 2024 adicionada. Circuito: História → Acervo audiovisual → Experience 2024 → Brasil em Cena → Memórias da Arte. 9 materiais estáticos. Originais preservados. Cenografia original e câmera Poly Haven CC0 reutilizada.\n");
 }
}
[InitializeOnLoad] public static class Experience2024Setup {static Experience2024Setup(){EditorApplication.delayCall+=()=>{const string p="Assets/Maracannes/Documentation/experience2024-build.request";if(!EditorApplication.isPlaying&&File.Exists(p)){AssetDatabase.DeleteAsset(p);MaracannesBuilder.AddExperience2024();}};}}
}


