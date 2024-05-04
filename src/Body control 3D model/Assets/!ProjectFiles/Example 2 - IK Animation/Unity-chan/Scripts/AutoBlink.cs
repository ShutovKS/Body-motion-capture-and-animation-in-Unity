#region

using System.Collections;
using UnityEngine;

#endregion

namespace Example_2___IK_Animation.Unity_chan
{
	public class AutoBlink : MonoBehaviour
	{

		public bool isActive = true;
		public SkinnedMeshRenderer ref_SMR_EYE_DEF;
		public SkinnedMeshRenderer ref_SMR_EL_DEF;
		public float ratio_Close = 85.0f;
		public float ratio_HalfClose = 20.0f;

		[HideInInspector]
		public float
			ratio_Open = 0.0f;
		private bool timerStarted = false;
		private bool isBlink = false;

		public float timeBlink = 0.4f;
		private float timeRemining = 0.0f;

		public float threshold = 0.3f;
		public float interval = 3.0f;


		enum Status
		{
			Close,
			HalfClose,
			Open
		}


		private Status eyeStatus;

		void Awake ()
		{
		}


		void Start ()
		{
			ResetTimer ();
			StartCoroutine ("RandomChange");
		}

		void ResetTimer ()
		{
			timeRemining = timeBlink;
			timerStarted = false;
		}

		void Update ()
		{
			if (!timerStarted) {
				eyeStatus = Status.Close;
				timerStarted = true;
			}
			if (timerStarted) {
				timeRemining -= Time.deltaTime;
				if (timeRemining <= 0.0f) {
					eyeStatus = Status.Open;
					ResetTimer ();
				} else if (timeRemining <= timeBlink * 0.3f) {
					eyeStatus = Status.HalfClose;
				}
			}
		}

		void LateUpdate ()
		{
			if (isActive) {
				if (isBlink) {
					switch (eyeStatus) {
					case Status.Close:
						SetCloseEyes ();
						break;
					case Status.HalfClose:
						SetHalfCloseEyes ();
						break;
					case Status.Open:
						SetOpenEyes ();
						isBlink = false;
						break;
					}
				}
			}
		}

		void SetCloseEyes ()
		{
			ref_SMR_EYE_DEF.SetBlendShapeWeight (6, ratio_Close);
			ref_SMR_EL_DEF.SetBlendShapeWeight (6, ratio_Close);
		}

		void SetHalfCloseEyes ()
		{
			ref_SMR_EYE_DEF.SetBlendShapeWeight (6, ratio_HalfClose);
			ref_SMR_EL_DEF.SetBlendShapeWeight (6, ratio_HalfClose);
		}

		void SetOpenEyes ()
		{
			ref_SMR_EYE_DEF.SetBlendShapeWeight (6, ratio_Open);
			ref_SMR_EL_DEF.SetBlendShapeWeight (6, ratio_Open);
		}

		IEnumerator RandomChange ()
		{
			while (true) {
				float _seed = Random.Range (0.0f, 1.0f);
				if (!isBlink) {
					if (_seed > threshold) {
						isBlink = true;
					}
				}

				yield return new WaitForSeconds (interval);
			}
		}
	}
}