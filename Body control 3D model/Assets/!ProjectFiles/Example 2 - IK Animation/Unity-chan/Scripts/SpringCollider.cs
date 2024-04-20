#region

using UnityEngine;

#endregion

namespace Example_2___IK_Animation.Unity_chan
{
	public class SpringCollider : MonoBehaviour
	{
		public float radius = 0.5f;

		private void OnDrawGizmosSelected ()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere (transform.position, radius);
		}
	}
}