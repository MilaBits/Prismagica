// #if UNITY_EDITOR
// using UnityEditor;
// using UnityEditor.SceneManagement;
// using UnityEngine;
//
// namespace UI
// {
//     [CreateAssetMenu(menuName = "Alchemage/Note")]
//     public class Note : ScriptableObject
//     {
//         [Multiline(3)]
//         public string Description;
//
//         private Transform teleportTarget;
//
//         public Vector3 Location;
//
//         public TeleportPoint teleportPoint;
//
//         public string scene;
//
//         [ContextMenu()]
//         public void JumpTo()
//         {
//             EditorSceneManager.OpenScene($"Assets/Scenes/{scene}.unity");
//             SceneView.lastActiveSceneView.pivot = Location;
//         }
//     
//         [BoxGroup("Actions", false), Button]
//         public void TeleportTo()
//         {
// //        EditorSceneManager.OpenScene($"Assets/Scenes/{scene}.unity");
//             SceneView.lastActiveSceneView.pivot = Location;
//             GameObject.Find("Player").transform.SetPositionAndRotation(teleportPoint.position, teleportPoint.rotation);
//         }
//     }
// }
// #endif