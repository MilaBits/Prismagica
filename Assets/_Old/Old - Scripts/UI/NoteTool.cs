// #if UNITY_EDITOR
// using TMPro;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// namespace UI
// {
//     public class NoteTool : MonoBehaviour
//     {
//         [SerializeField]
//         private TMP_InputField TitleField = null;
//
//         [SerializeField]
//         private TMP_InputField DescriptionField = null;
//
//         private Movement playerMovement;
//
//         private void OnEnable()
//         {
//             playerMovement = GameObject.Find("Player").GetComponent<Movement>();
//             playerMovement.enabled = false;
//         }
//
//         public void Submit()
//         {
//             Note note = Note.CreateInstance<Note>();
//
//             Transform player = GameObject.Find("Player").transform;
//             note.Location = player.transform.position;
//             note.teleportPoint = new TeleportPoint(player.position, player.rotation);
//
//             note.name = TitleField.text;
//             note.Description = DescriptionField.text;
//             note.scene = SceneManager.GetActiveScene().name;
//
//             AssetDatabase.CreateAsset(note, $"Assets/Resources/Notes/{note.name}.asset");
//             Close();
//         }
//
//         public void Close()
//         {
//             playerMovement.enabled = true;
//             Destroy(gameObject);
//         }
//     }
// }
// #endif