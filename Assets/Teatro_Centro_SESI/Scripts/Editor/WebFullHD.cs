using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
namespace TeatroSESI {
public static class WebFullHD {
 [PostProcessBuild(100)] public static void Apply(BuildTarget target,string output){
  if(target!=BuildTarget.WebGL)return;
  var path=Path.Combine(output,"index.html");if(!File.Exists(path))return;
  string html=File.ReadAllText(path);
  html=Regex.Replace(html,"(<canvas[^>]*width=)[\"']?[0-9]+[\"']?","${1}1920");
  html=Regex.Replace(html,"(<canvas[^>]*height=)[\"']?[0-9]+[\"']?","${1}1080");
  // The Unity loader otherwise changes the drawing buffer to the CSS dimensions.
  html=html.Replace("// config.matchWebGLToCanvasSize = false;","config.matchWebGLToCanvasSize = false;");
  if(!html.Contains("Metavision1080.css"))html=html.Replace("</head>","<link rel=\"stylesheet\" href=\"TemplateData/Metavision1080.css\">\n</head>");
  Directory.CreateDirectory(Path.Combine(output,"TemplateData"));
  File.Copy("Assets/Teatro_Centro_SESI/UI/Web1080/Viewport.css.txt",Path.Combine(output,"TemplateData/Metavision1080.css"),true);
  File.WriteAllText(path,html);
 }
}
}
