using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroesUI : MonoBehaviour
{
    [SerializeField] private HeroListSO heroListSO;

    [SerializeField] private Transform pfHeroUI;
    [SerializeField] private Transform container;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Image upgradeButtonImage;
    [SerializeField] private Material blackNWhiteMaterial;
    [SerializeField] private TabState tabState;

    private class HeroTransformClass
    {
        public Hero hero;
        public HeroUI heroUI;
        public Transform heroTransform;
    }

    private List<HeroTransformClass> heroTransformList = new List<HeroTransformClass>();
    private int currentHeroIndex = 0;

    private void Start()
    {
        foreach (HeroSO heroSO in heroListSO.list)
        {
            Hero hero = HeroManager.Instance.GetHero(heroSO);

            Transform heroTransform = Instantiate(pfHeroUI, container);
            heroTransform.Find("sprite").GetComponent<Image>().sprite = heroSO.sprite;
            HeroUI heroUI = heroTransform.GetComponent<HeroUI>();
            heroUI.Setup(hero);
            heroTransformList.Add(new HeroTransformClass { hero = hero, heroTransform = heroTransform, heroUI = heroUI });
        }
        upgradeButton.onClick.AddListener(() => {
            UpgradeHero();
            SFXMusic.Instance.PlayUpgrade();
        });

        tabState.onEnableCallback = () => {
            UpdateHeroUpgradeAvailability();
        };
        UpdateHeroUpgradeAvailability();
    }

    public void Left()
    {
        if (currentHeroIndex > 0)
        {
            currentHeroIndex--;
            MoveContainer();
        }
    }

    public void Right()
    {
        if (currentHeroIndex < heroTransformList.Count - 1)
        {
            currentHeroIndex++;
            MoveContainer();
        }
    }

    private LTDescr prevAnim = null;

    private void UpgradeHero()
    {
        heroTransformList[currentHeroIndex].hero.Upgrade();
        heroTransformList[currentHeroIndex].heroUI.PlayLvlupAnimation();
        UpdateHeroUpgradeAvailability();
    }

    private void UpdateHeroUpgradeAvailability()
    {
        if (heroTransformList[currentHeroIndex].hero.CanUpgrade())
        {
            upgradeButton.enabled = true;
            upgradeButtonImage.material = null;
        }
        else
        {
            upgradeButton.enabled = false;
            upgradeButtonImage.material = blackNWhiteMaterial;
        }
    }

    private void MoveContainer()
    {
        upgradeButton.enabled = false;
        upgradeButtonImage.material = blackNWhiteMaterial;

        if (prevAnim != null)
        {
            prevAnim.reset();
        }

        float centerPosX = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height) * .5f).x;
        float currentPosX = heroTransformList[currentHeroIndex].heroTransform.position.x;
        float dirX = centerPosX - currentPosX;
        Vector3 targetPos = container.position + new Vector3(dirX, 0, 0);
        prevAnim = container.LeanMove(targetPos, .5f).setOnComplete(() =>
        {
            UpdateHeroUpgradeAvailability();
            prevAnim = null;
        });
    }
}
