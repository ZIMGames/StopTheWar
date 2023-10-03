using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxUI : MonoBehaviour
{
    [SerializeField] private Image pricePanelImage;
    [SerializeField] private Text priceText;
    [SerializeField] private ResourceAmount price;
    [SerializeField] private ResourceEventChannelSO resourceEventChannel;
    [SerializeField] private BoxSystem boxSystem;
    [SerializeField] private int boxIndex;
    [SerializeField] private Button button;

    private void Start()
    {
        priceText.text = price.amount.ToString();
        button.onClick.AddListener(() =>
        {
            ResourceManager.Instance.SpendResources(price);
            boxSystem.OpenBox(boxIndex);
        });
        resourceEventChannel.OnEventRaised += OnResourceAmountChanged;
        UpdateVisuals();
    }

    private void OnResourceAmountChanged(ResourceTypeSO resourceType, int amount)
    {
        UpdateVisuals();
    }

    private void OnDestroy()
    {
        resourceEventChannel.OnEventRaised -= OnResourceAmountChanged;
    }

    private void UpdateVisuals()
    {
        if (ResourceManager.Instance.CanAfford(price))
        {
            pricePanelImage.color = new Color32(56, 195, 162, 125);
            button.enabled = true;
        }
        else
        {
            pricePanelImage.color = new Color32(195, 56, 109, 125);
            button.enabled = false;
        }
    }
}
