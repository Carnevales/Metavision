using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TeatroSESI {
// Controlador exclusivo da câmera de inspeção. Não depende de alterações no InputSystem_Actions.
[RequireComponent(typeof(CharacterController))]
public sealed class VisitaCentroSESI : MonoBehaviour {
 public Camera cameraVisita;
 public float velocidade=2.6f;
 public Vector3[] percurso;
 public bool visitaGuiada;
 public float sensibilidade=.10f;
 public float suavizacao=.045f;
 public bool autoWalking;
 public Vector3 autoTarget;
 MuseumExperience museum;Vector2 mouseSmooth,mouseVelocity;
 public string etapa="Área externa";
 CharacterController motor; float queda,vertical,parado; int ponto; Vector3 anterior;
 bool verificar; List<string> etapas=new List<string>();
 const string Teste="Assets/Teatro_Centro_SESI/References/percurso-test.request";
 void Start(){
  motor=GetComponent<CharacterController>(); anterior=transform.position;museum=GetComponent<MuseumExperience>();
  verificar=File.Exists(Teste)&&File.ReadAllText(Teste).Trim()=="run";
  if(verificar){visitaGuiada=true;ponto=0;File.WriteAllText(Teste,"running");}
 }
 void Update(){
  var keyboard=Keyboard.current;var mouse=Mouse.current;
  if(museum){
   Vector3 move=Vector3.zero;
   if(!museum.BlockInput){
    if(autoWalking){var delta=autoTarget-transform.position;delta.y=0;if(delta.magnitude>.15f){move=delta.normalized*velocidade;transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(delta),140*Time.deltaTime);}}
    else if(keyboard!=null){float x=(keyboard.dKey.isPressed?1:0)-(keyboard.aKey.isPressed?1:0),z=(keyboard.wKey.isPressed?1:0)-(keyboard.sKey.isPressed?1:0);move=transform.TransformDirection(new Vector3(x,0,z).normalized)*velocidade;}
    if(!autoWalking&&mouse!=null&&Cursor.lockState==CursorLockMode.Locked){mouseSmooth=Vector2.SmoothDamp(mouseSmooth,mouse.delta.ReadValue(),ref mouseVelocity,suavizacao);transform.Rotate(0,mouseSmooth.x*sensibilidade,0);vertical=Mathf.Clamp(vertical-mouseSmooth.y*sensibilidade,-78,78);cameraVisita.transform.localRotation=Quaternion.Euler(vertical,0,0);}
   }
   if(motor.isGrounded&&queda<0)queda=-2;queda+=Physics.gravity.y*Time.deltaTime;motor.Move((move+Vector3.up*queda)*Time.deltaTime);return;
  }
  if(keyboard!=null&&keyboard.escapeKey.wasPressedThisFrame){visitaGuiada=false;Cursor.lockState=CursorLockMode.None;}
  Vector3 movimento=Vector3.zero;
  if(visitaGuiada&&percurso!=null&&ponto<percurso.Length){
   var alvo=percurso[ponto];var delta=alvo-transform.position;delta.y=0;
   if(delta.magnitude<.22f){etapas.Add("Ponto "+ponto+": "+transform.position.ToString("F2"));ponto++;parado=0;if(ponto==percurso.Length){visitaGuiada=false;etapa="Palco";Concluir(true);}}
   else {movimento=delta.normalized*velocidade;transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(delta),100*Time.deltaTime);cameraVisita.transform.localRotation=Quaternion.Lerp(cameraVisita.transform.localRotation,Quaternion.identity,Time.deltaTime*3);}
   etapa=ponto<2?"Fachada e entrada":ponto<4?"Hall de entrada":ponto<6?"Corredor e porta do teatro":ponto<9?"Plateia":"Palco";
  }else if(keyboard!=null){
   float x=(keyboard.dKey.isPressed?1:0)-(keyboard.aKey.isPressed?1:0),z=(keyboard.wKey.isPressed?1:0)-(keyboard.sKey.isPressed?1:0);
   movimento=transform.TransformDirection(new Vector3(x,0,z).normalized)*velocidade;
   if(mouse!=null&&mouse.rightButton.isPressed){var look=mouse.delta.ReadValue();transform.Rotate(0,look.x*.12f,0);vertical=Mathf.Clamp(vertical-look.y*.12f,-80,80);cameraVisita.transform.localRotation=Quaternion.Euler(vertical,0,0);}
  }
  if(motor.isGrounded&&queda<0)queda=-2;queda+=Physics.gravity.y*Time.deltaTime;
  motor.Move((movimento+Vector3.up*queda)*Time.deltaTime);
  if(verificar&&visitaGuiada){if(Vector3.Distance(anterior,transform.position)<.003f)parado+=Time.deltaTime;else parado=0;if(parado>6){visitaGuiada=false;Concluir(false);}}
  anterior=transform.position;
 }
 void Concluir(bool passou){
  if(!verificar)return;
  File.WriteAllText("Assets/Teatro_Centro_SESI/References/percurso-runtime.txt",(passou?"PASS":"FAIL")+" — CharacterController percorreu "+ponto+"/"+percurso.Length+" pontos.\n"+string.Join("\n",etapas)+"\nPosição final: "+transform.position+"\nTeste com colisões reais, gravidade e degraus, sem teletransporte entre pontos.");
  File.WriteAllText(Teste,"done");verificar=false;
  #if UNITY_EDITOR
  UnityEditor.EditorApplication.delayCall+=()=>UnityEditor.EditorApplication.isPlaying=false;
  #endif
 }
 void OnGUI(){
  if(museum)return;
  var style=new GUIStyle(GUI.skin.box){fontSize=15,alignment=TextAnchor.MiddleLeft};
  GUI.Box(new Rect(16,Screen.height-80,550,64),"  "+etapa+"\n  WASD: andar   |   botão direito: olhar   |   Esc: parar visita",style);
  if(GUI.Button(new Rect(16,16,210,34),visitaGuiada?"Parar visita":"Percorrer entrada → palco")){visitaGuiada=!visitaGuiada;if(visitaGuiada){motor.enabled=false;transform.position=new Vector3(0,.05f,-33);transform.rotation=Quaternion.identity;motor.enabled=true;queda=0;ponto=0;}}
 }
}
}
