using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
namespace Maracannes {
public sealed class BackgroundMusicAudit:MonoBehaviour {
 IEnumerator Start(){const string b="Assets/Maracannes/Documentation/";if(!File.Exists(b+"musica-test.request")||File.ReadAllText(b+"musica-test.request").Trim()!="run")yield break;File.WriteAllText(b+"musica-test.request","running");yield return new WaitForSeconds(2);var m=MuseumBackgroundMusic.Instance;var source=m.Source;var original=source;var log=new List<string>();bool ok=source.isPlaying&&source.loop&&source.spatialBlend==0&&Mathf.Abs(source.volume-.035f)<.001f;log.Add((ok?"PASS":"FAIL")+" Reprodução ativa em loop 2D; volume 3,5%; entrada suave.");var t=AcervoTravel.Instance;t.SetPosition(new Vector3(12,.05f,-25.8f),90);t.Refresh();int sample=source.timeSamples;bool entry=t.UseDoor(FindObjectsByType<AcervoDoor>(FindObjectsInactive.Exclude).First(d=>d.enterAcervo));yield return new WaitForSeconds(2);while(t.Busy)yield return null;bool continuity=entry&&t.InAcervo&&source&&MuseumBackgroundMusic.Instance.Source==original&&source.isPlaying&&source.timeSamples>sample&&FindObjectsByType<MuseumBackgroundMusic>(FindObjectsInactive.Exclude).Length==1;ok&=continuity;log.Add((continuity?"PASS":"FAIL")+" Teatro → acervo: mesma reprodução contínua, sem duplicação.");t.SetPosition(new Vector3(17.6f,.05f,-25.8f),-90);t.Refresh();bool exit=t.UseDoor(FindObjectsByType<AcervoDoor>(FindObjectsInactive.Exclude).First(d=>!d.enterAcervo));yield return new WaitForSeconds(2);while(t.Busy)yield return null;bool returned=exit&&!t.InAcervo&&source&&MuseumBackgroundMusic.Instance.Source==original&&source.isPlaying&&FindObjectsByType<MuseumBackgroundMusic>(FindObjectsInactive.Exclude).Length==1;ok&=returned;log.Add((returned?"PASS":"FAIL")+" Acervo → teatro: música preservada, um único emissor.");File.WriteAllLines(b+"musica-runtime.txt",new[]{(ok?"PASS":"FAIL")+" · Play Mode: áudio ambiente e transições"}.Concat(log));File.WriteAllText(b+"musica-test.request","done");
 #if UNITY_EDITOR
 UnityEditor.EditorApplication.delayCall+=()=>UnityEditor.EditorApplication.isPlaying=false;
 #endif
 }
}
}

