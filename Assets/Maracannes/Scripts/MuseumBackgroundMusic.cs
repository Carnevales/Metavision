using UnityEngine;
using TeatroSESI;
namespace Maracannes {
[DisallowMultipleComponent,RequireComponent(typeof(AudioSource))]
public sealed class MuseumBackgroundMusic:MonoBehaviour {
 public static MuseumBackgroundMusic Instance{get;private set;}
 [Range(0,.2f)] public float volume=.035f;
 public AudioSource Source{get;private set;}
 void Awake(){if(Instance&&Instance!=this){Destroy(gameObject);return;}Instance=this;DontDestroyOnLoad(gameObject);Source=GetComponent<AudioSource>();Source.playOnAwake=false;Source.loop=true;Source.spatialBlend=0;Source.dopplerLevel=0;Source.volume=0;}
 void Start(){if(Instance==this&&Source.clip)Source.Play();}
 AudioListener listener;
 void EnsureListener(){if(listener&&listener.isActiveAndEnabled)return;var visitor=Object.FindAnyObjectByType<VisitaCentroSESI>();if(!visitor||!visitor.cameraVisita)return;foreach(var l in Object.FindObjectsByType<AudioListener>(FindObjectsInactive.Exclude))if(l.isActiveAndEnabled){listener=l;return;}listener=visitor.cameraVisita.GetComponent<AudioListener>();if(!listener)listener=visitor.cameraVisita.gameObject.AddComponent<AudioListener>();listener.enabled=true;}
 void Update(){if(Instance==this){EnsureListener();Source.volume=Mathf.MoveTowards(Source.volume,volume,Time.unscaledDeltaTime*.044f);}}
 void OnDestroy(){if(Instance==this)Instance=null;}
}
}
