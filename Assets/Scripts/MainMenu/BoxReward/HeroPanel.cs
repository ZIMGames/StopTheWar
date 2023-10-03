using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroPanel : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Image heroImage;
    [SerializeField] private RectTransform downPanelRectTransform;

    [SerializeField] private BarUI barUI;
    [SerializeField] private Text barText;
    [SerializeField] private CoinSpawner coinSpawner;

    private List<LTDescr> animationList = new List<LTDescr>();
    private int animationsCompleted;

    private System.Action callback;

    public void Show(Reward.Hero reward, System.Action callback)
    {
        this.callback = callback;
        gameObject.SetActive(true);

        animationList.Clear();
        animationsCompleted = 0;

        Hero hero = HeroManager.Instance.GetHero(reward.heroSO);

        text.text = "+" + reward.amount;
        heroImage.sprite = reward.heroSO.sprite;
        barUI.Setup(hero.GetHeroData().cards, hero.GetExpToNextLevel()); //TODO: get rid of the situtations where it turns into max level
        barUI.OnValueChanged += BarUI_OnValueChanged;
        BarUI_OnValueChanged();

        StartCoroutine(coinSpawner.Spawn(reward.amount, new Vector3(10, -30), 7f, new Vector3(-17.5f, 30.5f), (int value) =>
        {
            barUI.AddValue(value);
            if (!SFXMusic.Instance.IsPlaying())
            {
                SFXMusic.Instance.PlayCoinImpact();
            }
        }));


        downPanelRectTransform.anchoredPosition = new Vector2(-230, -420);
        animationList.Add(LeanTween.moveLocal(downPanelRectTransform.gameObject, new Vector3(0, -420), .5f).setOnComplete(
            () => { animationsCompleted++; }));
        animationList.Add(LeanTween.scale(heroImage.gameObject, new Vector3(1.2f, 1.2f), .5f).setOnComplete(() =>
        {
            LeanTween.scale(heroImage.gameObject, new Vector3(1f, 1f), .5f).setOnComplete(() => { animationsCompleted++; });
        }));
    }

    private void BarUI_OnValueChanged()
    {
        if (barUI.GetValue() < barUI.GetMaxValue())
        {
            barText.text = barUI.GetValue() + " / " + barUI.GetMaxValue();
            barUI.SetFillImageColor(new Color32(161, 224, 90, 255));
        }
        else
        {
            barText.text = "UPGRADE!";
            barUI.SetFillImageColor(new Color32(224, 174, 90, 255));
        }
    }


    public void Click()
    {
        if (animationsCompleted == animationList.Count && coinSpawner.IsEveryCopyArrived())
        {
            gameObject.SetActive(false);
            callback();
        }
        else
        {
            foreach (var anim in animationList)
            {
                anim.reset();
            }

            downPanelRectTransform.anchoredPosition = new Vector2(0, -420);
            heroImage.transform.localScale = new Vector3(1, 1);

            animationsCompleted = animationList.Count;



            coinSpawner.Stop();
        }
    }
}
