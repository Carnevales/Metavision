using UnityEngine;
using UnityEngine.Rendering;
namespace Maracannes {
[ExecuteAlways] public sealed class AcervoAmbient:MonoBehaviour {
 public Color indirectLight=new Color(.48f,.47f,.43f);
 void Start(){Apply();}
 public void Apply(){var sh=new SphericalHarmonicsL2();sh.AddAmbientLight(indirectLight);RenderSettings.ambientProbe=sh;}
}
}
