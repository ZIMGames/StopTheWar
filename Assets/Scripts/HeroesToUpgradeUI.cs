using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroesToUpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject heroesToUpgradeGameObject;
    [SerializeField] private Text heroesToUpgradeText;

    private void Start()
    {
        UpdateVisuals();

        HeroManager.Instance.OnHeroDataChanged += HeroManager_OnHeroDataChanged;
    }

    private void HeroManager_OnHeroDataChanged()
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        int count = 0;
        foreach (HeroSO heroSO in HeroManager.Instance.GetUnlockedHeroList())
        {
            Hero hero = HeroManager.Instance.GetHero(heroSO);
            if (hero.CanUpgrade())
            {
                count++;
            }
        }

        if (count > 0)
        {
            heroesToUpgradeGameObject.SetActive(true);
            heroesToUpgradeText.text = "" + count;
        }
        else
        {
            heroesToUpgradeGameObject.SetActive(false);
        }
    }
}
