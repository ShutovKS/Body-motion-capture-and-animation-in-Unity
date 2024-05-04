#region

using UnityEngine;

#endregion

namespace Example_2___IK_Animation.Unity_chan
{
	public class RandomWind : MonoBehaviour
	{
		private SpringBone[] springBones;
		public bool isWindActive = true;

		void Start ()
		{
			springBones = GetComponent<SpringManager> ().springBones;
		}

		void Update ()
		{
			Vector3 force = Vector3.zero;
			if (isWindActive) {
				force = new Vector3 (Mathf.PerlinNoise (Time.time, 0.0f) * 0.005f, 0, 0);
			}

			for (int i = 0; i < springBones.Length; i++) {
				springBones [i].springForce = force;
			}
		}

		void OnGUI ()
		{
			Rect rect1 = new Rect (10, Screen.height - 40, 400, 30);
			isWindActive = GUI.Toggle (rect1, isWindActive, "Random Wind");
		}

	}
}