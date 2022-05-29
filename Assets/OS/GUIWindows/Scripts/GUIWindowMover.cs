using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
// This script was modified to use the new InputSystem

namespace Rellac.Windows
{
	/// <summary>
	/// Script to handle moving windows
	/// </summary>
	public class GUIWindowMover : GUIPointerObject
	{
		/// <summary>
		/// Window to move
		/// </summary>
		[Tooltip("Window to move")]
		[SerializeField] private RectTransform parentWindow = null;
		/// <summary>
		/// Mover is locked and unusable
		/// </summary>
		[Tooltip("Mover is locked and unusable")]
		[SerializeField] private bool isLocked = false;
		/// <summary>
		/// Fires when a window has been moved
		/// </summary>
		[Tooltip("Fires when a window has been moved")]
		[SerializeField] private UnityEvent onWindowMoved = null;

		[SerializeField] private Canvas canvas;
		/// <summary>
		/// Walk up the hierarchy till first canvas
		/// </summary>
		private void Awake()
		{
			if (canvas == null)
			{
				Transform testCanvasTransform = transform.parent;
				while (testCanvasTransform != null)
				{
					canvas = testCanvasTransform.GetComponent<Canvas>();
					if (canvas != null)
					{
						break;
					}
					testCanvasTransform = testCanvasTransform.parent;
				}
			}
		}

		private Vector2 mouseOffset;
		private bool isGrabbed = false;

		void Start()
		{
			onPointerDown.AddListener(SetIsGrabbed);
		}

		void Update()
		{
			if (!isGrabbed || isLocked) return;

			// Boundary detection
			Vector2 futurePosition = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()) + mouseOffset;
			float futureX = futurePosition.x;
			float futureY = futurePosition.y;
			float canvasH = canvas.GetComponent<RectTransform>().rect.height * canvas.scaleFactor;
			float canvasW = canvas.GetComponent<RectTransform>().rect.width * canvas.scaleFactor;
			float parentWindowW = parentWindow.rect.width * canvas.scaleFactor;
			float parentWindowH = parentWindow.rect.height * canvas.scaleFactor;
			futureX = Mathf.Clamp(futureX, parentWindowW / 2, canvasW - parentWindowW / 2);
			futureY = Mathf.Clamp(futureY, parentWindowH / 2, canvasH - parentWindowH / 2);
			parentWindow.position = new Vector2(futureX, futureY);

            if (Mouse.current.leftButton.wasReleasedThisFrame)
			{
				isGrabbed = false;
				if (onWindowMoved != null)
				{
					onWindowMoved.Invoke();
				}
			}
		}

		/// <summary>
		/// Toggle interactivity of handle
		/// </summary>
		/// <param name="input">is interactive</param>
		public void SetIsLocked(bool input)
		{
			isLocked = input;
			isGrabbed = false;
		}

		/// <summary>
		/// Trigger that window has started to be moved
		/// </summary>
		public void SetIsGrabbed()
		{
			mouseOffset = parentWindow.position - new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0);
			isGrabbed = true;
			parentWindow.SetAsLastSibling();
		}
	}
}