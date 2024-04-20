#region

using UnityEngine;

#endregion

namespace Example_2___IK_Animation.Unity_chan
{
	public class ThirdPersonCamera : MonoBehaviour
	{
		public float smooth = 3f;
		Transform standardPos;
		Transform frontPos;
		Transform jumpPos;

		bool bQuickSwitch = false;


		void Start ()
		{
			standardPos = GameObject.Find ("CamPos").transform;
		
			if (GameObject.Find ("FrontPos"))
				frontPos = GameObject.Find ("FrontPos").transform;

			if (GameObject.Find ("JumpPos"))
				jumpPos = GameObject.Find ("JumpPos").transform;

			transform.position = standardPos.position;	
			transform.forward = standardPos.forward;	
		}
	
		void FixedUpdate ()
		{
		
			if (Input.GetButton ("Fire1")) {
				setCameraPositionFrontView ();
			} else if (Input.GetButton ("Fire2")) {
				setCameraPositionJumpView ();
			} else {
				setCameraPositionNormalView ();
			}
		}

		void setCameraPositionNormalView ()
		{
			if (bQuickSwitch == false) {
				transform.position = Vector3.Lerp (transform.position, standardPos.position, Time.fixedDeltaTime * smooth);	
				transform.forward = Vector3.Lerp (transform.forward, standardPos.forward, Time.fixedDeltaTime * smooth);
			} else {
				transform.position = standardPos.position;	
				transform.forward = standardPos.forward;
				bQuickSwitch = false;
			}
		}
	
		void setCameraPositionFrontView ()
		{
			bQuickSwitch = true;
			transform.position = frontPos.position;	
			transform.forward = frontPos.forward;
		}

		void setCameraPositionJumpView ()
		{
			bQuickSwitch = false;
			transform.position = Vector3.Lerp (transform.position, jumpPos.position, Time.fixedDeltaTime * smooth);	
			transform.forward = Vector3.Lerp (transform.forward, jumpPos.forward, Time.fixedDeltaTime * smooth);		
		}
	}
}