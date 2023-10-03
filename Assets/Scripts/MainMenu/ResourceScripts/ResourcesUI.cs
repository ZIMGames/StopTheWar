using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour
{
    private Dictionary<ResourceTypeSO, Text> resourceTextDictionary;

    [SerializeField] private ResourceTypeListSO resourceTypeList;
    [SerializeField] private ResourceEventChannelSO ResourceChangedEvent;

    private void Awake()
    {
        resourceTextDictionary = new Dictionary<ResourceTypeSO, Text>();

        Transform resourceTemplate = transform.Find("resourceTemplate");

        Text resourceText = resourceTemplate.Find("text").GetComponent<Text>();
        resourceTextDictionary[resourceTypeList.list[0]] = resourceText;

        //resourceTemplate.gameObject.SetActive(false);

        //int index = 0;
        //foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        //{       
        //    Transform resourceTransform = Instantiate(resourceTemplate, transform);

        //    float offsetAmount = -370f;
        //    resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index++, 0);

        //    Text resourceText = resourceTransform.Find("text").GetComponent<Text>();            
        //    resourceTextDictionary[resourceType] = resourceText;

        //    //resourceTransform.Find("currency").GetComponent<Text>().text = resourceType.shortName;
        //    resourceTransform.Find("background").GetComponent<Image>().color = resourceType.color;
        //    resourceTransform.Find("currencyImage").GetComponent<Image>().sprite = resourceType.sprite;

        //    Button resourceButton = resourceTransform.Find("button").GetComponent<Button>();

        //    switch (resourceType.type)
        //    {
        //        case ResourceType.Gold:
        //            resourceButton.onClick.AddListener(() => {
        //                //AdGoldMenu.Show();
        //            });
        //            break;
        //        case ResourceType.Silver:
        //            resourceButton.onClick.AddListener(() => {
        //                //AdSilverMenu.Show();
        //            });
        //            break;
        //        default:
        //            Debug.LogError("there is no such resource type, please revise resources scriptable objects and add appropriate type");
        //            break;
        //    }

        //    resourceTransform.gameObject.SetActive(true);
        //}

        ResourceChangedEvent.OnEventRaised += UpdateResourceAmount;
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        ResourceChangedEvent.OnEventRaised -= UpdateResourceAmount;
    }

    private void UpdateResourceAmount(ResourceTypeSO resourceType, int amount)
    {
        resourceTextDictionary[resourceType].text = amount.ToString();
    }
}
