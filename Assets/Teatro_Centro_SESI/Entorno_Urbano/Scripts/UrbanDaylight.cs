using UnityEngine;
using UnityEngine.Rendering;
using TeatroSESI;
namespace TeatroSESI.Urban {
[ExecuteAlways] public sealed class UrbanDaylight:MonoBehaviour {
 public Material skybox;public Light sun;
 void OnEnable(){Apply();}void Start(){Apply();}
 public void Apply(){if(!skybox)return;RenderSettings.skybox=skybox;RenderSettings.sun=sun;RenderSettings.ambientMode=AmbientMode.Trilight;RenderSettings.ambientSkyColor=new Color(.62f,.72f,.84f);RenderSettings.ambientEquatorColor=new Color(.42f,.46f,.49f);RenderSettings.ambientGroundColor=new Color(.22f,.21f,.19f);RenderSettings.ambientIntensity=1;RenderSettings.reflectionIntensity=.6f;RenderSettings.fog=false;}
 void LateUpdate(){foreach(var c in Camera.allCameras){if(c.gameObject.scene.path==gameObject.scene.path||c.GetComponentInParent<VisitaCentroSESI>()){c.clearFlags=CameraClearFlags.Skybox;c.farClipPlane=Mathf.Max(c.farClipPlane,650);}}}
}

}

