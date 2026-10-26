using UnityEngine;
namespace Maracannes {
public sealed class AcervoDoor:MonoBehaviour {
 public bool enterAcervo;
 public float radius=2.1f;
 public string Prompt=>enterAcervo?"Pressione E para entrar no Acervo Maracannes.":"Pressione E para voltar ao teatro.";
}
}
