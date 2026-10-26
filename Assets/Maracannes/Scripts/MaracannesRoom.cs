using UnityEngine;
namespace Maracannes {
public sealed class MaracannesRoom : MonoBehaviour {
 [Range(1,5)] public int number;
 public string theme;
 public Bounds bounds;
 public bool Contains(Vector3 p)=>bounds.Contains(p);
 void OnDrawGizmosSelected(){Gizmos.color=new Color(.9f,.6f,.2f,.3f);Gizmos.DrawWireCube(bounds.center,bounds.size);}
}
}

