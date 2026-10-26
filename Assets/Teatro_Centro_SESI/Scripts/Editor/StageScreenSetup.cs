using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace TeatroSESI {
[InitializeOnLoad] public static class StageScreenSetup {
 const string B="Assets/Teatro_Centro_SESI/";
 static StageScreenSetup(){EditorApplication.update+=Run;}
 static void Run(){if(EditorApplication.isPlaying||EditorApplication.isCompiling||EditorApplication.isUpdating||!File.Exists(B+"References/tela-palco.request"))return;AssetDatabase.DeleteAsset(B+"References/tela-palco.request");EditorSceneManager.SaveOpenScenes();var scene=EditorSceneManager.OpenScene(B+"Scenes/Teatro.unity",OpenSceneMode.Single);var all=scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).ToArray();var curtain=all.First(t=>t.name=="Fundo preto pregueado");var old=all.FirstOrDefault(t=>t.name=="Tela_Palco_Memorias_Arte");var g=old?old.gameObject:new GameObject("Tela_Palco_Memorias_Arte");g.transform.SetParent(curtain.parent,false);var bounds=curtain.GetComponent<Renderer>().bounds;g.transform.position=new Vector3(0,3.38f,bounds.min.z-.18f);
 string imagePath=B+"Textures/Maracannes_Memorias_Arte_Tela.png";var importer=AssetImporter.GetAtPath(imagePath) as TextureImporter;importer.npotScale=TextureImporterNPOTScale.None;importer.textureCompression=TextureImporterCompression.Uncompressed;importer.maxTextureSize=2048;importer.mipmapEnabled=true;importer.SaveAndReimport();var texture=AssetDatabase.LoadAssetAtPath<Texture2D>(imagePath);float width=8,height=width*texture.height/texture.width;
 string matPath=B+"Materials/Tela_Memorias_Arte.mat";var mat=AssetDatabase.LoadAssetAtPath<Material>(matPath);if(!mat){mat=new Material(Shader.Find("Universal Render Pipeline/Unlit"));AssetDatabase.CreateAsset(mat,matPath);}mat.SetTexture("_BaseMap",texture);mat.SetColor("_BaseColor",Color.white);
 if(!g.transform.Find("Imagem integral")){var q=GameObject.CreatePrimitive(PrimitiveType.Quad);q.name="Imagem integral";q.transform.SetParent(g.transform,false);q.transform.localPosition=new Vector3(0,0,-.052f);q.transform.localScale=new Vector3(width,height,1);q.GetComponent<Renderer>().sharedMaterial=mat;Object.DestroyImmediate(q.GetComponent<Collider>());var frame=GameObject.CreatePrimitive(PrimitiveType.Cube);frame.name="Tela com borda preta";frame.transform.SetParent(g.transform,false);frame.transform.localScale=new Vector3(width+.12f,height+.12f,.09f);frame.GetComponent<Renderer>().sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>(B+"Materials/Estrutura_antracite.mat");for(int s=-1;s<=1;s+=2){float y=3.38f+height/2;var cable=GameObject.CreatePrimitive(PrimitiveType.Cylinder);cable.name="Cabo de suspensão";cable.transform.SetParent(g.transform,false);cable.transform.position=new Vector3(s*3.65f,(y+6.9f)/2,g.transform.position.z);cable.transform.localScale=new Vector3(.018f,(6.9f-y)/2,.018f);cable.GetComponent<Renderer>().sharedMaterial=frame.GetComponent<Renderer>().sharedMaterial;Object.DestroyImmediate(cable.GetComponent<Collider>());}}
 if(!g.GetComponent<StageScreenVerification>())g.AddComponent<StageScreenVerification>();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();File.WriteAllText(B+"References/tela-palco-test.request","run");EditorApplication.isPlaying=true;}
}
}

