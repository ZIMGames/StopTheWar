using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private Text indexText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text coinsText;
    [SerializeField] private Image[] starImageArray;
    [SerializeField] private Button button;

    [Space]
    [SerializeField] private Image mainPanelImage;
    [SerializeField] private Image rewardPanelImage;
    [SerializeField] private Material blackWhiteMaterial;

    public void SetQuestSO(QuestSO questSO, int index, System.Action callback)
    {
        QuestDescriptionSO descr = questSO.questDescription;
        indexText.text = (index + 1).ToString();
        iconImage.sprite = descr.iconSprite;
        nameText.text = descr.nameString;
        coinsText.text = "+" + questSO.coins;

        for (int i = 0; i < questSO.difficulty; i++)
        {
            starImageArray[i].color = new Color(1, 1, 1, 1);
        }
        button.onClick.AddListener(() => { callback(); });
    }

    public void Activate()
    {
        button.enabled = true;
        mainPanelImage.material = null;
        rewardPanelImage.material = null;
        foreach (var starImage in starImageArray)
        {
            starImage.material = null;
        }
    }

    public void Inactivate()
    {
        button.enabled = false;
        mainPanelImage.material = blackWhiteMaterial;
        rewardPanelImage.material = blackWhiteMaterial;
        foreach (var starImage in starImageArray)
        {
            starImage.material = blackWhiteMaterial;
        }
    }
}
