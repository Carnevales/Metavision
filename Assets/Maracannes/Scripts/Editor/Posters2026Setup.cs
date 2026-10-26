using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace Maracannes {
public static partial class MaracannesBuilder {
 public static void Add2026Posters(){
  EditorSceneManager.SaveOpenScenes();scene=EditorSceneManager.OpenScene(AcervoTravel.Acervo,OpenSceneMode.Single);root=scene.GetRootGameObjects().First(g=>g.name=="Maracannes_Acervo").transform;
  var room=root.GetComponentsInChildren<MaracannesRoom>().First(r=>r.number==4).transform;
  graphite=AssetDatabase.LoadAssetAtPath<Material>(B+"Materials/Grafite_fosco.mat");
  var catalog=JsonUtility.FromJson<Catalog>(File.ReadAllText(B+"Documentation/cartazes_2026_adicionados.json"));
  var slots=new List<System.Tuple<Vector3,Quaternion>>();foreach(float x in new[]{18f,20,22,26,28,30})slots.Add(System.Tuple.Create(new Vector3(x,1.85f,-17.82f),Quaternion.Euler(0,180,0)));
  foreach(float z in new[]{-16f,-13.5f,-11,-8.5f,-.5f,2,4.5f})slots.Add(System.Tuple.Create(new Vector3(31.82f,1.85f,z),Quaternion.Euler(0,90,0)));
  slots.Add(System.Tuple.Create(new Vector3(24,1.85f,5.82f),Quaternion.identity));
  for(int i=0;i<catalog.posters.Length;i++){var r=catalog.posters[i];var existing=room.GetComponentsInChildren<MaracannesExhibit>().FirstOrDefault(p=>p.catalogId==r.id);if(existing)continue;
   var imp=AssetImporter.GetAtPath(r.path) as TextureImporter;imp.npotScale=TextureImporterNPOTScale.None;imp.mipmapEnabled=true;imp.textureCompression=TextureImporterCompression.Uncompressed;imp.filterMode=FilterMode.Bilinear;imp.SaveAndReimport();
   var p=slots[i].Item1;var rot=slots[i].Item2;float h=1.65f,w=h*r.width/r.height;var t=Group(r.id+" · "+r.sourceName,room);t.position=p;t.rotation=rot;
   Box("Moldura fixada à parede",Vector3.zero,new Vector3(w+.10f,h+.10f,.075f),graphite,t);
   var image=MeshObject("Cartaz original integral",Quad("Quad_"+r.id,w,h),PosterMaterial(r),t);image.transform.localPosition=new Vector3(0,0,-.042f);
   var e=t.gameObject.AddComponent<MaracannesExhibit>();e.catalogId=r.id;e.room=4;e.originalPath=r.original;e.sha256=r.sourceHash;e.inspectionPosition=p-rot*Vector3.forward*1.4f;e.inspectionPosition=new Vector3(e.inspectionPosition.x,.05f,e.inspectionPosition.z);
  }
  if(!root.GetComponent<Posters2026Audit>())root.gameObject.AddComponent<Posters2026Audit>();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();File.WriteAllText(B+"Documentation/cartazes-2026-build.txt","14 cartazes adicionados. Sala 4: "+room.GetComponentsInChildren<MaracannesExhibit>().Length+". Sem legendas externas ou interações.\n");
 }
}
[InitializeOnLoad] public static class Posters2026Setup {static Posters2026Setup(){EditorApplication.delayCall+=()=>{const string p="Assets/Maracannes/Documentation/cartazes-2026.request";if(!EditorApplication.isPlaying&&File.Exists(p)){AssetDatabase.DeleteAsset(p);MaracannesBuilder.Add2026Posters();}};}}
}
