using System.Collections;
using System.IO;
using UnityEngine;
namespace TeatroSESI {
public sealed class StageScreenVerification:MonoBehaviour {
 IEnumerator Start(){const string b="Assets/Teatro_Centro_SESI/References/";if(!File.Exists(b+"tela-palco-test.request")||File.ReadAllText(b+"tela-palco-test.request").Trim()!="run")yield break;yield return new WaitForSeconds(1);var image=transform.Find("Imagem integral");var texture=image.GetComponent<Renderer>().sharedMaterial.GetTexture("_BaseMap");bool ok=texture&&Mathf.Abs(image.localScale.x/image.localScale.y-(float)texture.width/texture.height)<.001f&&transform.Find("Tela com borda preta").GetComponent<Collider>();var v=FindAnyObjectByType<VisitaCentroSESI>();v.enabled=false;v.GetComponent<Maracannes.AcervoTravel>().SetPosition(new Vector3(0,.6f,-8),0);v.cameraVisita.transform.LookAt(transform.position);yield return new WaitForEndOfFrame();ScreenCapture.CaptureScreenshot(b+"TelaPalco_Verificacao.png");yield return null;File.WriteAllText(b+"tela-palco-runtime.txt",(ok?"PASS":"FAIL")+" · Tela no palco com imagem integral, proporção preservada, material válido e colisor. Captura real em Play Mode.");File.WriteAllText(b+"tela-palco-test.request","done");
 #if UNITY_EDITOR
 UnityEditor.EditorApplication.delayCall+=()=>UnityEditor.EditorApplication.isPlaying=false;
 #endif
 }
}
}
