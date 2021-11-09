using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oculus
{
	/// <summary>
	/// Spawns all interactable tools that are specified for a scene.
	/// </summary>
	public class InteractableToolsCreator : MonoBehaviour
	{
		[SerializeField] private Transform[] LeftHandTools = null;
		[SerializeField] private Transform[] RightHandTools = null;

		private void Awake()			
		{
			if (LeftHandTools != null && LeftHandTools.Length > 0)
			{
				StartCoroutine(AttachToolsToHands(LeftHandTools, false));
			}

			if (RightHandTools != null && RightHandTools.Length > 0)
			{
				StartCoroutine(AttachToolsToHands(RightHandTools, true));
			}
		}

		private IEnumerator AttachToolsToHands(Transform[] toolObjects, bool isRightHand)
		{
			HandsManager handsManagerObj = null;
			while ((handsManagerObj = HandsManager.Instance) == null || !handsManagerObj.IsInitialized())
			{
				yield return null;
			}

			// create set of tools per hand to be safe
			HashSet<Transform> toolObjectSet = new HashSet<Transform>();
			foreach (Transform toolTransform in toolObjects)
			{
				toolObjectSet.Add(toolTransform.transform);
			}

			foreach (Transform toolObject in toolObjectSet)
			{
				OVRSkeleton handSkeletonToAttachTo =
				  isRightHand ? handsManagerObj.RightHandSkeleton : handsManagerObj.LeftHandSkeleton;
				while (handSkeletonToAttachTo == null || handSkeletonToAttachTo.Bones == null)
				{
					yield return null;
				}

				AttachToolToHandTransform(toolObject, isRightHand);
			}
		}

		private void AttachToolToHandTransform(Transform tool, bool isRightHanded)
		{
			var newTool = Instantiate(tool).transform;
			newTool.localPosition = Vector3.zero;
			var toolComp = newTool.GetComponent<InteractableTool>();
			toolComp.IsRightHandedTool = isRightHanded;
			// Initialize only AFTER settings have been applied!
			toolComp.Initialize();
		}
	}
}