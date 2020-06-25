// using System.Collections.Generic;
// using System.Linq;
// using Sirenix.OdinInspector;
// using Sirenix.OdinInspector.Editor;
// using UI;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// public class NotesWindow : OdinEditorWindow {
//     [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden),
//      ListDrawerSettings(CustomAddFunction = "AddNote"), PropertyOrder(5)]
//     public List<Note> Notes;
//
//     [SerializeField, FoldoutGroup("Settings"), PropertyOrder(10)]
//     private NoteTool NoteToolPrefab;
//
//     [SerializeField, FoldoutGroup("Settings"), PropertyOrder(10)]
//     private NoteTool NoteToolInstance;
//
//     private Texture2D boxTitleBg;
//     private Texture2D boxContentBg;
//
//     private GUIStyle titleStyle = new GUIStyle();
//     private GUIStyle contentStyle = new GUIStyle();
//
//     private float RefreshInterval = 2f;
//     private float RefreshCounter = 0f;
//
//     private string oldScene;
//     private string newScene;
//
//
//     [MenuItem("Alchemage/Notes")]
//     private static void OpenWindow() {
//         GetWindow<NotesWindow>().Show();
//     }
//
//     private void Awake() {
//         UpdateNotes();
//         InitializeStyle();
//         UpdateNotes();
//     }
//
//     private void Update() {
//         newScene = SceneManager.GetActiveScene().name;
//         if (oldScene != newScene) {
//             InitializeStyle();
//         }
//         
//         if (Input.GetKey(KeyCode.N) && NoteToolInstance == null) {
//             NoteToolInstance = Instantiate(GetWindow<NotesWindow>().NoteToolPrefab);
//         }
//
//         RefreshCounter += Time.deltaTime;
//         if (RefreshCounter >= RefreshInterval) {
//             UpdateNotes();
//             RefreshCounter = 0;
//         }
//
//         oldScene = SceneManager.GetActiveScene().name;
//     }
//
//     protected override void OnEnable() {
//         NoteToolPrefab = AssetDatabase.LoadAssetAtPath<NoteTool>("Assets/Prefabs/NoteTool.prefab");
//
//         base.OnEnable();
//         SceneView.duringSceneGui += OnSceneGUI;
//     }
//
//     [Button]
//     private void InitializeStyle() {
//         boxTitleBg = new Texture2D(1, 1);
//         boxTitleBg.SetPixel(0, 0, new Color(.25f, .25f, .25f, .9f));
//
//         titleStyle.normal.background = boxTitleBg;
//         titleStyle.normal.background.Apply();
//         titleStyle.normal.textColor = Color.white;
//         titleStyle.padding = new RectOffset(5, 5, 5, 0);
//
//         boxContentBg = new Texture2D(1, 1);
//         boxContentBg.SetPixel(0, 0, new Color(.2f, .2f, .2f, .7f));
//
//         contentStyle.normal.background = boxContentBg;
//         contentStyle.normal.background.Apply();
//         contentStyle.normal.textColor = Color.white;
//         contentStyle.padding = new RectOffset(5, 5, 5, 0);
//     }
//
//     private void OnDisable() {
//         //this used to be onscenegui
//         SceneView.duringSceneGui -= OnSceneGUI;
//     }
//
//     [Button, PropertyOrder(0)]
//     private void UpdateNotes() {
//         Notes = Resources.LoadAll<Note>("Notes/").ToList();
//     }
//
//     private void AddNote() {
//         AssetDatabase.CreateAsset(CreateInstance<Note>(), "Assets/Resources/Notes/New Note.asset");
//         UpdateNotes();
//     }
//
//     void OnSceneGUI(SceneView sceneView) {
//         Handles.color = Color.magenta;
//         float radius = .1f;
//
//         foreach (Note note in Notes) {
//             Handles.DrawWireArc(note.Location, Vector3.forward, Vector3.right, 360, radius);
//         }
//
//         Handles.BeginGUI();
//
//         foreach (Note note in Notes) {
//             if (note.scene != SceneManager.GetActiveScene().name) continue;
//
//             Vector2 pos = HandleUtility.WorldToGUIPoint(note.Location);
//             GUI.Box(new Rect(pos.x + 25, pos.y + 30, 200, 50), note.Description, contentStyle);
//             GUI.Box(new Rect(pos.x + 25, pos.y + 10, 200, 20), note.name, titleStyle);
//         }
//
//         Handles.EndGUI();
//
//         Event e = Event.current;
//
//         switch (e.type) {
//             case EventType.KeyDown:
//                 if (Event.current.keyCode == (KeyCode.N)) {
//                     Instantiate(GetWindow<NotesWindow>().NoteToolPrefab);
//                 }
//
//                 break;
//         }
//     }
// }