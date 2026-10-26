using System.Collections;
using System.Linq;
using UnityEngine;
using TeatroSESI;
namespace Maracannes {
public sealed class ExhibitionLink : MonoBehaviour {
 MuseumExperience experience;MuseumPoint[] ownPoints;
 IEnumerator Start(){
  ownPoints=GetComponentsInChildren<MuseumPoint>(true);
  for(int i=0;i<240;i++){experience=Object.FindAnyObjectByType<MuseumExperience>();if(experience)break;yield return null;}
  if(!experience){Debug.LogError("Maracannes: controlador do teatro não encontrado.");yield break;}
  experience.points=(experience.points??new MuseumPoint[0]).Concat(ownPoints).Where(p=>p).Distinct().ToArray();
 }
 void OnDestroy(){if(experience&&ownPoints!=null)experience.points=experience.points.Where(p=>p&&!ownPoints.Contains(p)).ToArray();}
}
}
