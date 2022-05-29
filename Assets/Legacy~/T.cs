// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UIElements;
// using UnityEditor.UIElements;
// using UnityEngine.EventSystems;

// public class T : MonoBehaviour
// {
//     public VisualElement Window;
//     public Button closeButton;
//     public VisualElement titleBar;
//     private bool beingDragged = false;
//     // Start is called before the first frame update
//     void Start()
//     {
//         var root = GetComponent<UIDocument>().rootVisualElement;
//         Window = root.Q<VisualElement>("Window");

//         closeButton = Window.Q<Button>("CloseButton");
//         closeButton.clicked += test;

//         titleBar = Window.Q<VisualElement>("TitleBar");
//         titleBar.RegisterCallback<PointerDownEvent>(BeginDrag);
//         titleBar.RegisterCallback<PointerUpEvent>(EndDrag);
//         titleBar.RegisterCallback<PointerMoveEvent>(Drag);
//         titleBar.RegisterCallback<PointerLeaveEvent>(Fix);
//     }
//     void test () {
//         Debug.Log("Test!");
//     }
//     private Vector3 holdPos;
//     void BeginDrag (PointerDownEvent e) {
//         Debug.Log("BEGIN DRAG");
//         holdPos = e.localPosition;
//         beingDragged = true;
//     }
//     void EndDrag (PointerUpEvent e) {
//         Debug.Log("END DRAG");
//         beingDragged = false;
//     }
//     void Drag(PointerMoveEvent e) {
//         if (beingDragged) {
//             Window.transform.position = e.position - holdPos;
//         }
//     }

//     void Fix(PointerLeaveEvent e)
//     {
//         if (beingDragged) {
//             Window.transform.position = e.position - holdPos;
//         }
//     }
// }
