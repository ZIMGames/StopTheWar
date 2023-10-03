using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour
{
    [SerializeField] private Text barText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text attackDamageText;
    [SerializeField] private Text healthAmountText;
    [SerializeField] private GameObject lockedPanel;
    [SerializeField] private BarUI barUI;
    [SerializeField] private GameObject heroGameObject;
    [SerializeField] private Image levelCircleImage;
    [SerializeField] private Image rarityPanelImage;


    private Hero hero;

    public void Setup(Hero hero)
    {
        this.hero = hero;
        hero.OnHeroDataChanged += UpdateVisuals;

        //barUI.Setup(hero.GetHeroData().cards, hero.GetExpToNextLevel()); //TODO: get rid of the situtations where it turns into max level

        UpdateVisuals();
    }

    public void PlayLvlupAnimation()
    {
        heroGameObject.LeanScale(new Vector3(0.9f, 1.1f), 0.4f).setOnComplete(() =>
        {
            heroGameObject.LeanScale(new Vector3(1, 1), 0.4f);
        });
        LeanTween.colorText(attackDamageText.rectTransform, Color.green, .4f).setOnComplete(() =>
        {
            LeanTween.colorText(attackDamageText.rectTransform, Color.white, .4f);
        });
        LeanTween.colorText(healthAmountText.rectTransform, Color.green, .4f).setOnComplete(() =>
        {
            LeanTween.colorText(healthAmountText.rectTransform, Color.white, .4f);
        });
        levelCircleImage.gameObject.LeanColor(new Color32(255, 244, 76, 255), .4f).setOnComplete(() =>
        {
            levelCircleImage.color = new Color32(255, 76, 122, 255);
        });
    }

    private void UpdateVisuals()
    {
        if (hero.IsMaxLevel())
        {
            barText.text = "MAX LEVEL!";
            barUI.SetFillImageColor(new Color32(90, 224, 224, 255));
            barUI.Setup(1, 1);
        }
        else
        {
            int cards = hero.GetHeroData().cards;
            int maxCards = hero.GetExpToNextLevel();
            barUI.Setup(cards, maxCards);
            if (cards >= maxCards)
            {
                barText.text = "UPGRADE!";
                barUI.SetFillImageColor(new Color32(224, 174, 90, 255));
            }
            else
            {
                barText.text = cards + " / " + maxCards;
                barUI.SetFillImageColor(new Color32(161, 224, 90, 255));
            }
        }


        levelText.text = hero.GetHeroData().level.ToString();
        attackDamageText.text = hero.GetAttackDamage().ToString();
        healthAmountText.text = hero.GetHealthAmount().ToString();

        rarityPanelImage.color = hero.GetHeroRarity() == Hero.Rarity.Common ? new Color32(95, 139, 178, 255) : new Color32(130, 95, 178, 255);

        if (hero.GetHeroData().unlocked)
        {
            lockedPanel.SetActive(false);
        }
        else
        {
            lockedPanel.SetActive(true);
        }
    }
}
