using UnityEngine;
using TeatroSESI;
namespace Maracannes {
public sealed class AcervoBootstrap:MonoBehaviour {
 public GameObject visitorPrefab;
 void Start(){if(AcervoTravel.Instance)return;var v=FindAnyObjectByType<VisitaCentroSESI>();if(!v&&visitorPrefab)v=Instantiate(visitorPrefab).GetComponent<VisitaCentroSESI>();if(!v){Debug.LogError("Acervo: visitante compartilhado ausente.");return;}var travel=v.GetComponent<AcervoTravel>();if(!travel)travel=v.gameObject.AddComponent<AcervoTravel>();travel.PrepareMuseum();}
}
}
