using UnityEngine;
namespace TeatroSESI {
public sealed class TeatroDimensions : MonoBehaviour {
 [Header("Metros — boca e profundidade conforme rider Firjan")]
 public float boca = 9.70f, alturaBoca = 4.48f, profundidade = 7.30f;
 [Header("Estimativas fotográficas ajustáveis")]
 public float larguraSala = 15.6f, alturaPalco = 1.05f, passoFileira = .88f;
 [Range(8,16)] public int fileiras = 12;
 [TextArea] public string notas = "Referência principal: IMG_8292. Balcão conforme fotos 13/15 e rider. Sala, desníveis e arranjo de assentos são estimativas; não levantamento cadastral. Após ajustar, use Teatro SESI > Atualizar teatro selecionado.";
}
}
