using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Profile;
using UnityEngine;
namespace TeatroSESI {
[InitializeOnLoad] public static class MetavisionBrandingSetup {
 const string B="Assets/Teatro_Centro_SESI/UI/Branding/";
 static MetavisionBrandingSetup(){EditorApplication.update+=Tick;}
 static void Tick(){if(EditorApplication.isPlayingOrWillChangePlaymode||EditorApplication.isCompiling||EditorApplication.isUpdating||!File.Exists(B+"branding.request"))return;File.Delete(B+"branding.request");Apply();}
 [MenuItem("Teatro SESI/Identidade/Aplicar logo na abertura e ícones")]
 public static void Apply(){
  var active=BuildProfile.GetActiveBuildProfile();
  ApplyCurrent();
  if(active){
   try{BuildProfile.SetActiveBuildProfile(null);ApplyCurrent();}
   finally{BuildProfile.SetActiveBuildProfile(active);AssetDatabase.SaveAssets();}
  }
 }
 static void ApplyCurrent(){
  const string path=B+"Metavision_Logo_Dourada.png";
  var importer=AssetImporter.GetAtPath(path) as TextureImporter;
  if(importer==null)throw new System.Exception("Logo ainda não importada.");
  importer.textureType=TextureImporterType.Sprite;importer.spriteImportMode=SpriteImportMode.Single;
  importer.alphaIsTransparency=true;importer.mipmapEnabled=false;importer.npotScale=TextureImporterNPOTScale.None;
  importer.maxTextureSize=2048;importer.textureCompression=TextureImporterCompression.Uncompressed;importer.SaveAndReimport();
  var sprite=AssetDatabase.LoadAssetAtPath<Sprite>(path);var icon=AssetDatabase.LoadAssetAtPath<Texture2D>(path);
  if(!sprite||!icon)throw new System.Exception("Sprite da logo inválido.");
  PlayerSettings.SplashScreen.show=true;
  PlayerSettings.SplashScreen.showUnityLogo=true;
  PlayerSettings.SplashScreen.drawMode=PlayerSettings.SplashScreen.DrawMode.UnityLogoBelow;
  PlayerSettings.SplashScreen.animationMode=PlayerSettings.SplashScreen.AnimationMode.Static;
  PlayerSettings.SplashScreen.backgroundColor=new Color(.025f,.075f,.16f,1);
  PlayerSettings.SplashScreen.background=null;PlayerSettings.SplashScreen.backgroundPortrait=null;
  PlayerSettings.SplashScreen.overlayOpacity=0;
  PlayerSettings.SplashScreen.logos=new[]{PlayerSettings.SplashScreenLogo.Create(3f,sprite)};
  #pragma warning disable CS0618
  PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown,new[]{icon});
  foreach(var group in new[]{BuildTargetGroup.Standalone,BuildTargetGroup.WebGL}){var sizes=PlayerSettings.GetIconSizesForTargetGroup(group);if(sizes.Length>0)PlayerSettings.SetIconsForTargetGroup(group,Enumerable.Repeat(icon,sizes.Length).ToArray());}
  #pragma warning restore CS0618
  AssetDatabase.SaveAssets();
  bool valid=PlayerSettings.SplashScreen.show&&PlayerSettings.SplashScreen.logos.Length==1&&PlayerSettings.SplashScreen.logos[0].logo==sprite;
  File.WriteAllText(B+"branding-verificacao.txt",(valid?"PASS":"FAIL")+" — Sprite dourado transparente importado, logo configurada na Splash Screen por 3s, fundo azul-marinho, Made with Unity abaixo, ícone padrão e plataformas Windows/Web configurados. Configuração salva. Exibição de inicialização deve ser conferida na próxima build; Play Mode não reproduz automaticamente a splash nativa.\n");
  Debug.Log("Metavision: logo da abertura e ícones configurados.");
 }
}
}
