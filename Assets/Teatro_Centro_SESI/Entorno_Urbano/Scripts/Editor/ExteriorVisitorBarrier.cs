using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TeatroSESI.Urban
{
    [InitializeOnLoad]
    public static class ExteriorVisitorBarrier
    {
        const string Request = UrbanBuilder.B + "References/exterior-barrier.request";
        static ExteriorVisitorBarrier() { EditorApplication.update += Tick; }
        static void Tick()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating || !File.Exists(Request)) return;
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (scene.path != "Assets/Teatro_Centro_SESI/Scenes/Teatro.unity") return;
            File.Delete(Request);
            Install();
        }

        [MenuItem("Teatro SESI/Entorno urbano/Aplicar barreira de circulação")]
        public static void Install()
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (scene.path != "Assets/Teatro_Centro_SESI/Scenes/Teatro.unity") return;
            // Save a copy including the user's pending edits before adding anything.
            string backup = UrbanBuilder.B + "References/Teatro_Antes_Barreira.unity";
            if (!File.Exists(backup)) EditorSceneManager.SaveScene(scene, backup, true);
            var root = scene.GetRootGameObjects().FirstOrDefault(g => g.name == "Barreiras_Invisiveis_Area_Externa");
            if (!root) { root = new GameObject("Barreiras_Invisiveis_Area_Externa"); Undo.RegisterCreatedObjectUndo(root, "Barreira externa"); }
            // Full-height invisible walls keep visitors within the finished entrance area.
            Wall(root.transform, "Limite_Rua_Outro_Lado", new Vector3(0, 14, -58), new Vector3(132, 32, 2));
            Wall(root.transform, "Limite_Lateral_Esquerda", new Vector3(-65, 14, 3.5f), new Vector3(2, 32, 125));
            Wall(root.transform, "Limite_Lateral_Direita", new Vector3(65, 14, 3.5f), new Vector3(2, 32, 125));
            Wall(root.transform, "Limite_Fundos", new Vector3(0, 14, 65), new Vector3(132, 32, 2));
            Physics.SyncTransforms();
            bool valid = root.GetComponentsInChildren<BoxCollider>().Length == 4 && root.GetComponentsInChildren<BoxCollider>().All(c => c.enabled && !c.isTrigger);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            File.WriteAllText(UrbanBuilder.B + "References/exterior-barrier.txt", (valid ? "PASS" : "FAIL") + " - quatro BoxColliders sólidos, sem Renderer; limite da rua z=-58m, laterais x=+-65m, fundos z=65m; altura 32m, espessura 2m. Entrada e interiores preservados. Cena salva.\n");
            Debug.Log("Barreiras externas aplicadas e cena Teatro salva.");
        }
        static void Wall(Transform parent, string name, Vector3 center, Vector3 size)
        {
            var child = parent.Find(name);
            if (!child) { child = new GameObject(name).transform; child.SetParent(parent, false); Undo.RegisterCreatedObjectUndo(child.gameObject, "Barreira externa"); }
            child.position = center; child.rotation = Quaternion.identity; child.localScale = Vector3.one;
            var box = child.GetComponent<BoxCollider>();
            if (!box) box = Undo.AddComponent<BoxCollider>(child.gameObject);
            Undo.RecordObject(box, "Ajustar limite externo"); box.center = Vector3.zero; box.size = size; box.isTrigger = false; box.enabled = true;
            child.gameObject.layer = 0; child.gameObject.SetActive(true);
        }
    }
}
