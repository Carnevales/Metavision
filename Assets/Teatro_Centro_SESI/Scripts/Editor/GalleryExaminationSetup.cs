using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Maracannes;
namespace TeatroSESI {
[InitializeOnLoad] public static class GalleryExaminationSetup {
 const string B="Assets/Teatro_Centro_SESI/";
 static GalleryExaminationSetup(){EditorApplication.update+=Tick;}
 static void Tick(){if(EditorApplication.isPlayingOrWillChangePlaymode||EditorApplication.isCompiling||EditorApplication.isUpdating||!File.Exists(B+"References/gallery-update.request"))return;File.Delete(B+"References/gallery-update.request");Apply();}
 [MenuItem("Teatro SESI/Museu/Atualizar fotos e exame de cartazes")]
 public static void Apply(){
  EditorSceneManager.SaveOpenScenes();var theater=SceneManager.GetSceneByPath(AcervoTravel.Theater);if(!theater.isLoaded)theater=EditorSceneManager.OpenScene(AcervoTravel.Theater,OpenSceneMode.Additive);
  if(!File.Exists(B+"References/Teatro_Antes_Exame_Cartazes.unity"))EditorSceneManager.SaveScene(theater,B+"References/Teatro_Antes_Exame_Cartazes.unity",true);
  foreach(string file in new[]{"Sede_Firjan.jpg","Foyer_Cazuza.jpg","Teatro_Fachada.jpg","Cultura_Ultimo_Ensaio.jpg"}){var ti=AssetImporter.GetAtPath(B+"Textures/Curiosidades/"+file) as TextureImporter;if(ti==null)throw new System.Exception("Foto ausente: "+file);ti.npotScale=TextureImporterNPOTScale.None;ti.maxTextureSize=2048;ti.textureCompression=TextureImporterCompression.CompressedHQ;ti.mipmapEnabled=true;ti.SaveAndReimport();}
  var points=theater.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<MuseumPoint>(true)).ToArray();
  Photo(points,"01","Sede_Firjan.jpg","Fotografia da sede · Divulgação/Firjan · Carta da Indústria");
  Photo(points,"02","Foyer_Cazuza.jpg","Exposição Cazuza 60 anos no foyer · Acervo de projeto BOLD");
  Photo(points,"03","Teatro_Fachada.jpg","Teatro Firjan SESI Centro · Divulgação/Firjan · Time Out");
  var stage=points.First(p=>p.title.StartsWith("04"));stage.photoRegion=new Rect(0,0,1,1);
  var v=theater.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<VisitaCentroSESI>(true)).First();
  if(!v.GetComponent<GalleryExaminationAudit>())v.gameObject.AddComponent<GalleryExaminationAudit>();
  // Reuse the existing foyer poster and its original texture, if present.
  foreach(var r in theater.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Renderer>(true)).Where(r=>r.name.Contains("Cartaz")&&!r.GetComponent<TextMesh>()&&r.sharedMaterial&&r.sharedMaterial.mainTexture is Texture2D)) Add(r.gameObject,r.sharedMaterial.mainTexture as Texture2D);
  v.GetComponent<MuseumExperience>().points=theater.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<MuseumPoint>(true)).ToArray();
  EditorSceneManager.MarkSceneDirty(theater);EditorSceneManager.SaveScene(theater);
  PrefabUtility.SaveAsPrefabAsset(v.gameObject,"Assets/Maracannes/Prefabs/Visitante_Compartilhado.prefab");
  var acervo=SceneManager.GetSceneByPath(AcervoTravel.Acervo);bool opened=!acervo.isLoaded;if(opened)acervo=EditorSceneManager.OpenScene(AcervoTravel.Acervo,OpenSceneMode.Additive);
  if(!File.Exists(B+"References/Acervo_Antes_Exame_Cartazes.unity"))EditorSceneManager.SaveScene(acervo,B+"References/Acervo_Antes_Exame_Cartazes.unity",true);
  int count=0;foreach(var exhibit in acervo.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<MaracannesExhibit>(true))){var r=exhibit.GetComponentsInChildren<Renderer>().FirstOrDefault(r=>r.name.StartsWith("Cartaz original")&&!r.GetComponent<TextMesh>()&&r.sharedMaterial&&r.sharedMaterial.mainTexture is Texture2D);var image=r?r.sharedMaterial.mainTexture as Texture2D:null;if(!image){var mat=AssetDatabase.LoadAssetAtPath<Material>("Assets/Maracannes/Materials/Cartaz_"+exhibit.catalogId+".mat");if(mat)image=mat.mainTexture as Texture2D;}if(!image)throw new System.Exception("Cartaz sem textura: "+exhibit.catalogId);Add(exhibit.gameObject,image);count++;}
  EditorSceneManager.MarkSceneDirty(acervo);EditorSceneManager.SaveScene(acervo);EditorSceneManager.CloseScene(acervo,true);SceneManager.SetActiveScene(theater);AssetDatabase.SaveAssets();
  File.WriteAllText(B+"References/gallery-setup.txt","PASS Fotos novas: sede, exposição do foyer e fachada; foto original de palco integral preservada. Cartazes do acervo com exame: "+count+". Imagens originais e proporções preservadas; controlador e janela existentes reutilizados.");
  File.WriteAllText(B+"References/gallery-test.request","run");EditorApplication.isPlaying=true;
 }
 static void Photo(MuseumPoint[] points,string id,string file,string credit){var p=points.First(p=>p.title.StartsWith(id));p.image=AssetDatabase.LoadAssetAtPath<Texture2D>(B+"Textures/Curiosidades/"+file);p.photoRegion=new Rect(0,0,1,1);p.credit=credit;EditorUtility.SetDirty(p);}
 static void Add(GameObject obj,Texture2D image){var p=obj.GetComponent<MuseumPoint>();if(!p)p=obj.AddComponent<MuseumPoint>();p.action=MuseumAction.Exhibit;p.imageOnly=true;p.image=image;p.title="EXAMINAR CARTAZ";p.body="";p.source="";p.credit="Cartaz original · proporção e conteúdo preservados";p.photoRegion=new Rect(0,0,1,1);p.radius=2.6f;EditorUtility.SetDirty(p);}
}
}
