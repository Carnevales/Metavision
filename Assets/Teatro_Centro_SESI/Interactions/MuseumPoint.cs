using UnityEngine;
namespace TeatroSESI {
public enum MuseumAction { Ticket, Gate, Exhibit }
public sealed class MuseumPoint : MonoBehaviour {
 public MuseumAction action;
 public string title;
 [TextArea(4,12)] public string body;
 [TextArea] public string source;
 public Texture2D image;
 [Tooltip("Enquadramento da fotografia: X, Y, largura e altura normalizados entre 0 e 1.")]
 public Rect photoRegion=new Rect(0,0,1,1);
 public string credit;
 public float radius=2.2f;
 public string Prompt {get {return action==MuseumAction.Ticket?"[E] RETIRAR INGRESSO":action==MuseumAction.Gate?"[E] ENTRAR NO TEATRO":"[E] EXPLORAR EXPOSIÇÃO";}}
}
}
