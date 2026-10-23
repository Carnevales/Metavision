using UnityEngine;
namespace TeatroSESI {
[ExecuteAlways,RequireComponent(typeof(TextMesh))]
public sealed class MuseumTextDepth : MonoBehaviour {
 public Shader depthShader;
 Material material;TextMesh text;
 void OnEnable(){text=GetComponent<TextMesh>();Font.textureRebuilt+=Rebuild;Refresh();}
 void Rebuild(Font f){if(text&&f==text.font)Refresh();}
 void Refresh(){if(!text||!text.font)return;if(!material){var shader=depthShader?depthShader:Shader.Find("TeatroSESI/Texto com profundidade");if(!shader)return;material=new Material(shader){hideFlags=HideFlags.HideAndDontSave};}material.mainTexture=text.font.material.mainTexture;GetComponent<Renderer>().sharedMaterial=material;}
 void OnDisable(){Font.textureRebuilt-=Rebuild;if(material){if(Application.isPlaying)Destroy(material);else DestroyImmediate(material);material=null;}}
}
}
