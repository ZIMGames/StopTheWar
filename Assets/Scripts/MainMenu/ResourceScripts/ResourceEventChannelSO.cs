using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Resource Event Channel")]
public class ResourceEventChannelSO : ScriptableObject
{
	public UnityAction<ResourceTypeSO, int> OnEventRaised;

	public void RaiseEvent(ResourceTypeSO resourceType, int amount)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(resourceType, amount);
	}
}
