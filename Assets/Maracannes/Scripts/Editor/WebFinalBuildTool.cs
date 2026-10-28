using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEditor.Build.Profile;
using System.Security.Cryptography;
namespace Maracannes {
[InitializeOnLoad] public static class WebFinalBuildTool {
 const string B="Assets/Maracannes/Documentation/";
 static WebFinalBuildTool(){EditorApplication.update+=Tick;}
 static void Tick(){if(EditorApplication.isPlayingOrWillChangePlaymode||EditorApplication.isCompiling||EditorApplication.isUpdating)return;if(File.Exists(B+"web-audit.request")){File.Delete(B+"web-audit.request");Audit();}if(File.Exists(B+"web-final.request")){File.Delete(B+"web-final.request");Build();}}
 static void Audit(){var r=BuildReport.GetLatestReport();if(r==null){File.WriteAllText(B+"web-assets-audit.txt","Relatório anterior indisponível");return;}var sizes=new Dictionary<string,ulong>();foreach(var p in r.packedAssets)foreach(var a in p.contents){if(!sizes.ContainsKey(a.sourceAssetPath))sizes[a.sourceAssetPath]=0;sizes[a.sourceAssetPath]+=a.packedSize;}File.WriteAllLines(B+"web-assets-audit.txt",sizes.OrderByDescending(a=>a.Value).Take(100).Select(a=>(a.Value/1048576f).ToString("F2")+" MB | "+a.Key));}
 static void Build(){BuildFinal();}
 public static void BuildFinal(){
  var notes=new List<string>();
  string backup="Builds/WebFinal_Backup";Directory.CreateDirectory(backup);
  const string theater="Assets/Teatro_Centro_SESI/Scenes/Teatro.unity";
  var protectedPaths=new HashSet<string>(AssetDatabase.GetDependencies(theater,true));
  var protectedHashes=protectedPaths.Where(File.Exists).SelectMany(p=>new[]{p,p+".meta"}).Where(File.Exists).Distinct().ToDictionary(p=>p,Hash);
  File.WriteAllLines(backup+"/cinema-protegido.txt",protectedHashes.Select(p=>p.Value+" | "+p.Key));
  var profile=AssetDatabase.LoadAssetAtPath<BuildProfile>("Assets/Settings/Build Profiles/Web - Desktop - Release.asset");if(profile)BuildProfile.SetActiveBuildProfile(profile);
  string settingsPath="Assets/Settings/Build Profiles/Web - Desktop - Release.asset";
  if(!File.Exists(backup+"/Perfil_Web_Antes.asset.txt"))File.Copy(settingsPath,backup+"/Perfil_Web_Antes.asset.txt");
  var paths=AssetDatabase.GetDependencies(new[]{AcervoTravel.Acervo,"Assets/MenuPackage/Menu.unity"},true);
  foreach(var path in paths.Distinct()){
   if(protectedPaths.Contains(path)||!path.StartsWith("Assets/Maracannes/Images/"))continue;
   var imp=AssetImporter.GetAtPath(path) as TextureImporter;if(!imp)continue;
   string metaBackup=backup+"/"+path+".meta";Directory.CreateDirectory(Path.GetDirectoryName(metaBackup));if(!File.Exists(metaBackup))File.Copy(path+".meta",metaBackup);
   var web=imp.GetPlatformTextureSettings("WebGL");web.overridden=true;web.maxTextureSize=path.EndsWith("S4_002.png")?8192:1024;web.format=TextureImporterFormat.Automatic;web.textureCompression=TextureImporterCompression.CompressedHQ;web.crunchedCompression=true;web.compressionQuality=85;imp.SetPlatformTextureSettings(web);imp.SaveAndReimport();notes.Add("WEB TEXTURE "+path+" | limite "+web.maxTextureSize+" | compressão HQ/Crunch");
  }
  PlayerSettings.WebGL.compressionFormat=WebGLCompressionFormat.Brotli;
  PlayerSettings.WebGL.decompressionFallback=true;
  EditorUserBuildSettings.development=false;AssetDatabase.SaveAssets();
  var changed=protectedHashes.Where(p=>!File.Exists(p.Key)||Hash(p.Key)!=p.Value).Select(p=>p.Key).ToArray();
  if(changed.Length>0)throw new Exception("Proteção do cinema detectou alteração: "+string.Join(", ",changed));
  notes.Add("PASS Cinema preservado: hashes idênticos para "+protectedHashes.Count+" arquivos e metadados de dependências.");File.WriteAllLines(B+"web-final-otimizacao.txt",notes);
  var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=EditorBuildSettings.scenes.Where(s=>s.enabled).Select(s=>s.path).ToArray(),locationPathName="buidfinal",target=BuildTarget.WebGL,options=BuildOptions.None});
  notes.Add("BUILD "+report.summary.result+" | erros "+report.summary.totalErrors+" | duração "+report.summary.totalTime);
  if(report.summary.result!=BuildResult.Succeeded){File.WriteAllLines(B+"web-final-build.txt",notes);throw new Exception("Build Web falhou");}
  foreach(var f in Directory.GetFiles("buidfinal","*",SearchOption.AllDirectories))notes.Add("ARQUIVO "+new FileInfo(f).Length+" bytes | "+f);
  bool fits=Directory.GetFiles("buidfinal","*",SearchOption.AllDirectories).All(f=>new FileInfo(f).Length<200000000);
  notes.Add((fits?"PASS":"FAIL")+" Todos os arquivos abaixo de 200.000.000 bytes.");
  notes.Add(protectedHashes.All(p=>File.Exists(p.Key)&&Hash(p.Key)==p.Value)?"PASS Cinema também preservado após build":"FAIL Cinema alterado após build");
  File.WriteAllLines(B+"web-final-build.txt",notes);if(!fits)throw new Exception("Build ainda excede limite por arquivo");
 }
 static string Hash(string p){using(var sha=SHA256.Create())using(var stream=File.OpenRead(p))return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-","");}
}
}
