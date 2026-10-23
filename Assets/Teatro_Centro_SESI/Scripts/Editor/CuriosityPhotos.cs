using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
namespace TeatroSESI {
[InitializeOnLoad] public static class CuriosityPhotos {
 const string B="Assets/Teatro_Centro_SESI/";
 static CuriosityPhotos(){EditorApplication.delayCall+=()=>{if(File.Exists(B+"References/photos.request")){AssetDatabase.DeleteAsset(B+"References/photos.request");Apply();}};}
 [MenuItem("Teatro SESI/Museu/Atualizar fotografias das curiosidades")]
 public static void Apply(){
  var scene=SceneManager.GetSceneByPath(B+"Scenes/Teatro_Centro_SESI.unity");if(!scene.isLoaded)throw new System.InvalidOperationException("Abra a cena existente do teatro.");
  foreach(string file in new[]{"IMG_8292 1.png","IMG_8294 1.png"}){var importer=(TextureImporter)AssetImporter.GetAtPath(B+"References/"+file);if(importer.maxTextureSize<4096||importer.filterMode!=FilterMode.Trilinear||importer.npotScale!=TextureImporterNPOTScale.None){importer.npotScale=TextureImporterNPOTScale.None;importer.maxTextureSize=4096;importer.filterMode=FilterMode.Trilinear;importer.anisoLevel=4;importer.textureCompression=TextureImporterCompression.CompressedHQ;importer.SaveAndReimport();}}
  var points=scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<MuseumPoint>(true)).Where(p=>p.action==MuseumAction.Exhibit).ToArray();
  Set(points,"01","IMG_8294 1.png",new Rect(0,0,1,1),"Fachada do teatro · foto fornecida pelo usuário.\nRegistro contemporâneo.");
  Set(points,"02","IMG_8294 1.png",new Rect(.16f,.08f,.72f,.56f),"Entrada e programação cultural · detalhe da foto do usuário.\nRegistro contemporâneo.");
  Set(points,"03","IMG_8292 1.png",new Rect(0,0,1,1),"Palco e plateia · foto fornecida pelo usuário.\nRegistro contemporâneo.");
  Set(points,"04","IMG_8292 1.png",new Rect(.15f,.38f,.70f,.48f),"Cortinas, refletores e equipamentos · detalhe da foto do usuário.\nRegistro contemporâneo.");
  // O painel foi movido manualmente: a interação acompanha sua posição atual.
  var fourth=scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).First(t=>t.name.StartsWith("Painel_04"));points.First(p=>p.title.StartsWith("04")).transform.position=fourth.position;
  EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();File.WriteAllText(B+"References/fotos-curiosidades.txt","Fotografias originais do usuário importadas até 4096 px. Quatro enquadramentos: fachada, entrada/programação, palco/plateia e caixa cênica/equipamentos. Recortes feitos apenas na apresentação da UI, preservando os arquivos originais. Textos e fontes dos painéis preservados. Interação 04 alinhada ao painel movido manualmente.");
 }
 static void Set(MuseumPoint[] points,string id,string file,Rect region,string credit){var p=points.First(p=>p.name.StartsWith("Interacao_"+id)||p.title.StartsWith(id));p.image=AssetDatabase.LoadAssetAtPath<Texture2D>(B+"References/"+file);p.photoRegion=region;p.credit=credit;EditorUtility.SetDirty(p);}
}
}
