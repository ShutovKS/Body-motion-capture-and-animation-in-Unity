#region

using UnityEditor;
using UnityEngine;

#endregion

namespace Example_2___IK_Animation.Unity_chan.Editor
{
	public class CreateLocatorHere
	{
		[MenuItem("GameObject/Create Locator Here")]
		static void CreateGameObjectAsChild ()
		{
			GameObject go = new GameObject ("Locator_");
			go.transform.parent = Selection.activeTransform;
			go.transform.localPosition = Vector3.zero;
		}

		[MenuItem("GameObject/Create Locator Here",true)]
		static bool ValidateCreateGameObjectAsChild ()
		{
			return Selection.activeTransform != null;
		}

		[MenuItem("CONTEXT/Transform/Create Locator Here")]
		static void CreateGameObjectAsChild (MenuCommand command)
		{
			Transform tr = (Transform)command.context;
			GameObject go = new GameObject ("Locator_");
			go.transform.parent = tr;
			go.transform.localPosition = Vector3.zero;
		}
	}
}