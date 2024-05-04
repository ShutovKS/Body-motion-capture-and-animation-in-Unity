#region

using UnityEngine;

#endregion

namespace Example_2___IK_Animation.Unity_chan
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField]
		private Vector3 focus = Vector3.zero;
		[SerializeField]
		private GameObject focusObj = null;

		public bool showInstWindow = true;

		private Vector3 oldPos;

		void setupFocusObject(string name)
		{
			GameObject obj = this.focusObj = new GameObject(name);
			obj.transform.position = this.focus;
			obj.transform.LookAt(this.transform.position);

			return;
		}

		void Start ()
		{
			if (this.focusObj == null)
				this.setupFocusObject("CameraFocusObject");

			Transform trans = this.transform;
			transform.parent = this.focusObj.transform;

			trans.LookAt(this.focus);

			return;
		}
	
		void Update ()
		{
			this.mouseEvent();

			return;
		}

		void OnGUI()
		{
			if(showInstWindow){
				GUI.Box(new Rect(Screen.width -210, Screen.height - 100, 200, 90), "Camera Operations");
				GUI.Label(new Rect(Screen.width -200, Screen.height - 80, 200, 30),"RMB / Alt+LMB: Tumble");
				GUI.Label(new Rect(Screen.width -200, Screen.height - 60, 200, 30),"MMB / Alt+Cmd+LMB: Track");
				GUI.Label(new Rect(Screen.width -200, Screen.height - 40, 200, 30),"Wheel / 2 Fingers Swipe: Dolly");
			}

		}

		void mouseEvent()
		{
			float delta = Input.GetAxis("Mouse ScrollWheel");
			if (delta != 0.0f)
				this.mouseWheelEvent(delta);

			if (Input.GetMouseButtonDown((int)MouseButtonDown.MBD_LEFT) ||
				Input.GetMouseButtonDown((int)MouseButtonDown.MBD_MIDDLE) ||
				Input.GetMouseButtonDown((int)MouseButtonDown.MBD_RIGHT))
				this.oldPos = Input.mousePosition;

			this.mouseDragEvent(Input.mousePosition);

			return;
		}

		void mouseDragEvent(Vector3 mousePos)
		{
			Vector3 diff = mousePos - oldPos;

			if(Input.GetMouseButton((int)MouseButtonDown.MBD_LEFT))
			{
				if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftCommand))
				{
					if (diff.magnitude > Vector3.kEpsilon)
						this.cameraTranslate(-diff / 100.0f);
				}
				else if (Input.GetKey(KeyCode.LeftAlt))
				{
					if (diff.magnitude > Vector3.kEpsilon)
						this.cameraRotate(new Vector3(diff.y, diff.x, 0.0f));
				}
			}
			else if (Input.GetMouseButton((int)MouseButtonDown.MBD_MIDDLE))
			{
				if (diff.magnitude > Vector3.kEpsilon)
					this.cameraTranslate(-diff / 100.0f);
			}
			else if (Input.GetMouseButton((int)MouseButtonDown.MBD_RIGHT))
			{
				if (diff.magnitude > Vector3.kEpsilon)
					this.cameraRotate(new Vector3(diff.y, diff.x, 0.0f));
			}
				
			this.oldPos = mousePos;	

			return;
		}

		public void mouseWheelEvent(float delta)
		{
			Vector3 focusToPosition = this.transform.position - this.focus;

			Vector3 post = focusToPosition * (1.0f + delta);

			if (post.magnitude > 0.01)
				this.transform.position = this.focus + post;

			return;
		}

		void cameraTranslate(Vector3 vec)
		{
			Transform focusTrans = this.focusObj.transform;

			vec.x *= -1;

			focusTrans.Translate(Vector3.right * vec.x);
			focusTrans.Translate(Vector3.up * vec.y);

			this.focus = focusTrans.position;

			return;
		}

		public void cameraRotate(Vector3 eulerAngle)
		{
			Quaternion q = Quaternion.identity;
 
			Transform focusTrans = this.focusObj.transform;
			focusTrans.localEulerAngles = focusTrans.localEulerAngles + eulerAngle;

			q.SetLookRotation (this.focus) ;

			return;
		}
	}
}