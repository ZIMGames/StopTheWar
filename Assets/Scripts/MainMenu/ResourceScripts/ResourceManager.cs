using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager Instance { get; private set; }

    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;
    [SerializeField] private ResourceTypeListSO resourceTypeList;
    [SerializeField] private ResourceEventChannelSO ResourceChangedEvent;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

        InitializeResourceAmountDictionary();
    }

    private void InitializeResourceAmountDictionary()
    {
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            string resourceTypeName = resourceType.shortName;
            resourceAmountDictionary[resourceType] = PlayerPrefs.GetInt(resourceTypeName, 0);

            if (ResourceChangedEvent != null)
                ResourceChangedEvent.RaiseEvent(resourceType, GetResourceAmount(resourceType));
        }
    }

    private void SaveResourceAmountDictionary()
    {
        if (!SaveManager.needSave)
            return;

        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            string resourceTypeName = resourceType.shortName;
            PlayerPrefs.SetInt(resourceTypeName, resourceAmountDictionary[resourceType]);
        }
    }

    private void OnDestroy()
    {
        SaveResourceAmountDictionary();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveResourceAmountDictionary();
        }
    }

    private void OnApplicationQuit()
    {
        SaveResourceAmountDictionary();
    }

    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceAmountDictionary[resourceType];
    }

    public void AddResources(ResourceTypeSO resourceType, int amount)
    {
        resourceAmountDictionary[resourceType] += amount;

        if (ResourceChangedEvent != null)
            ResourceChangedEvent.RaiseEvent(resourceType, GetResourceAmount(resourceType));
    }

    public void AddResources(ResourceAmount resourceAmount)
    {
        AddResources(resourceAmount.resourceType, resourceAmount.amount);
    }

    public bool CanAfford(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            if (!CanAfford(resourceAmount))
            {
                return false;
            }
        }
        return true;
    }

    public bool CanAfford(ResourceAmount resourceAmount)
    {
        ResourceTypeSO _resourceType = resourceAmount.resourceType;
        int _amount = resourceAmount.amount;
        return GetResourceAmount(_resourceType) >= _amount;
    }

    public void SpendResources(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            SpendResources(resourceAmount);
        }
    }

    public void SpendResources(ResourceAmount resourceAmount)
    {
        ResourceTypeSO _resourceType = resourceAmount.resourceType;
        int _amount = resourceAmount.amount;
        resourceAmountDictionary[_resourceType] -= _amount;

        if (ResourceChangedEvent != null)
            ResourceChangedEvent.RaiseEvent(_resourceType, GetResourceAmount(_resourceType));
    }

    public ResourceTypeSO GetResourceTypeSO(int index) 
    {
        return resourceTypeList.list[index];
    }
}
