using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Maracannes {
// Compatibilidade com a SampleScene existente: somente o teatro é pré-carregado.
public sealed class MaracannesLoader:MonoBehaviour {
 IEnumerator Start(){if(!SceneManager.GetSceneByPath(AcervoTravel.Theater).isLoaded){yield return SceneManager.LoadSceneAsync(AcervoTravel.Theater,LoadSceneMode.Additive);}}
}
}
