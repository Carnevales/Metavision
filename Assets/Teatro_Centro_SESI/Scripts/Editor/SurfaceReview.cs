using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
namespace TeatroSESI {
[InitializeOnLoad] public static class SurfaceReview {
 const string B="Assets/Teatro_Centro_SESI/";
 static SurfaceReview(){EditorApplication.delayCall+=()=>{if(!File.Exists(B+"References/surface.request"))return;AssetDatabase.DeleteAsset(B+"References/surface.request");Review();};}
 static void Review(){
  var renderers=Object.FindObjectsByType<Renderer>();var boards=renderers.Where(r=>r.name=="Tabua").ToArray();
  File.WriteAllLines(B+"References/superficies-diagnostico.txt",renderers.Where(r=>r.bounds.min.y<1.12f&&r.bounds.max.y>1.045f&&r.bounds.size.x>5).Select(r=>r.name+" "+r.bounds));
  Vector3 p=new Vector3(0,1.65f,-4.2f),target=new Vector3(0,2.1f,1);
  HallPolish.Capture("Diagnostico_Piso_Normal",p,target);
  var lights=Object.FindObjectsByType<Light>();var shadows=lights.Select(l=>l.shadows).ToArray();
  try{foreach(var light in lights)light.shadows=LightShadows.None;HallPolish.Capture("Diagnostico_Piso_Sem_Sombra",p,target);}finally{for(int i=0;i<lights.Length;i++)lights[i].shadows=shadows[i];}
  var materials=boards.Select(r=>r.sharedMaterial).ToArray();var plain=new Material(Shader.Find("Universal Render Pipeline/Lit"));plain.color=Color.gray;
  try{foreach(var board in boards)board.sharedMaterial=plain;HallPolish.Capture("Diagnostico_Piso_Liso",p,target);}finally{for(int i=0;i<boards.Length;i++)boards[i].sharedMaterial=materials[i];Object.DestroyImmediate(plain);}
 }
}
}
