using System.Linq;
using UnityEngine;
using UnityEditor;
namespace Maracannes {
public static partial class MaracannesBuilder {
 public static void PolishExperience2024(){
  var room=UnityEngine.Object.FindObjectsByType<MaracannesRoom>(FindObjectsInactive.Exclude).First(r=>r.number==5).transform;
  foreach(var t in room.GetComponentsInChildren<Transform>().Where(t=>t.name=="Junta discreta do piso").ToArray())UnityEngine.Object.DestroyImmediate(t.gameObject);
  foreach(var a in UnityEngine.Object.FindObjectsByType<AcervoSculpture>(FindObjectsInactive.Exclude)){if(a.room==2&&Vector3.Distance(a.transform.position,new Vector3(51,0,-18.5f))<.1f)a.transform.position=new Vector3(46.5f,0,-17.5f);if(a.room==3&&Vector3.Distance(a.transform.position,new Vector3(50.5f,0,-8.5f))<.1f)a.transform.position=new Vector3(49,0,-6.5f);}
  if(!room.Find("Iluminacao_Experience_Rebatida")){var group=Group("Iluminacao_Experience_Rebatida",room);var diffuser=AssetDatabase.LoadAssetAtPath<Material>(B+"Materials/Difusor.mat");foreach(var p in new[]{new Vector3(60,3.45f,-18),new Vector3(69,3.45f,-18),new Vector3(60,3.45f,-7),new Vector3(69,3.45f,-7)}){var t=Group("Preenchimento de galeria",group);t.position=p;var l=t.gameObject.AddComponent<Light>();l.type=LightType.Point;l.color=new Color(1,.95f,.86f);l.intensity=7;l.range=14;l.shadows=LightShadows.None;Box("Difusor de teto",p+Vector3.up*.26f,new Vector3(.65f,.06f,.35f),diffuser,group,false);}}
 }
}
}
