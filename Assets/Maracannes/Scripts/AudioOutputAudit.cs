using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
namespace Maracannes {
public sealed class AudioOutputAudit:MonoBehaviour {
 IEnumerator Start(){const string b="Assets/Maracannes/Documentation/";if(!File.Exists(b+"audio-output-test.request")||File.ReadAllText(b+"audio-output-test.request").Trim()!="run")yield break;File.WriteAllText(b+"audio-output-test.request","running");yield return new WaitForSeconds(2);var music=MuseumBackgroundMusic.Instance;var listeners=FindObjectsByType<AudioListener>(FindObjectsInactive.Exclude).Count(l=>l.isActiveAndEnabled);float peak=0;var buffer=new float[1024];for(int n=0;n<90;n++){AudioListener.GetOutputData(buffer,0);foreach(float f in buffer)peak=Mathf.Max(peak,Mathf.Abs(f));yield return null;}bool ok=listeners==1&&music.Source.isPlaying&&!music.Source.mute&&AudioListener.volume>0&&!AudioListener.pause&&peak>.000001f;File.WriteAllText(b+"audio-output-runtime.txt",(ok?"PASS":"FAIL")+" · Reprodução e sinal real de saída\nAudioListeners ativos: "+listeners+"\nMúsica ativa: "+music.Source.isPlaying+"\nVolume da fonte: "+music.Source.volume+"\nVolume global: "+AudioListener.volume+"\nPico na saída do AudioListener: "+peak+"\nMute Editor: "+
 #if UNITY_EDITOR
 UnityEditor.EditorUtility.audioMasterMute
 #else
 false
 #endif
 );File.WriteAllText(b+"audio-output-test.request","done");
 #if UNITY_EDITOR
 UnityEditor.EditorApplication.delayCall+=()=>UnityEditor.EditorApplication.isPlaying=false;
 #endif
 }
}
}
