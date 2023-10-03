using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewHeroPanel : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Image heroImage;

    private List<LTDescr> animationList = new List<LTDescr>();
    private int animationsCompleted;

    private System.Action callback;

    private bool finishedRolling;
    private float scaleMultiplierMin;
    private float scaleMultiplierMax;
    private float timerMax;
    private float scaleDeviationAnim;
    private int rolls;
    private float timer;

    public void Show(Reward.NewHero reward, System.Action callback)
    {
        this.callback = callback;
        gameObject.SetActive(true);

        animationList.Clear();
        animationsCompleted = 0;

        finishedRolling = false;
        scaleMultiplierMin = .2f;
        scaleMultiplierMax = 1f;
        timerMax = 3f;
        scaleDeviationAnim = 0.2f;
        rolls = 6;
        timer = 0;


        text.rectTransform.anchoredPosition = new Vector2(1250, -431);
        heroImage.sprite = reward.heroSO.sprite;
        heroImage.color = Color.black;
        animationList.Add(LeanTween.moveLocal(text.gameObject, new Vector3(-300, -431), .5f).setDelay(timerMax).setOnComplete(() =>
        {
            LeanTween.moveLocal(text.gameObject, new Vector3(0, -431), .2f).setOnComplete(() => { animationsCompleted++; });
        }));
    }

    public void Update()
    {
        if (!finishedRolling)
        {
            timer += Time.deltaTime;
            if (timer >= timerMax)
            {
                finishedRolling = true;
                heroImage.color = Color.white;
            }
            float scaleMultiplier = scaleMultiplierMin + timer * (scaleMultiplierMax - scaleMultiplierMin) / timerMax;
            heroImage.transform.localScale = new Vector3(1f + scaleDeviationAnim * Mathf.Sin(timer * 2 * Mathf.PI * rolls / timerMax), 1f) * scaleMultiplier;
        }
    }

    public void Click()
    {
        if (animationsCompleted == animationList.Count && finishedRolling)
        {
            BackgroundMusic.Unmute();
            gameObject.SetActive(false);
            callback();
        }
        //else
        //{
        //    foreach (var anim in animationList)
        //    {
        //        anim.reset();
        //    }

        //    text.rectTransform.anchoredPosition = new Vector2(0, -431);
        //    heroImage.transform.localScale = new Vector3(1, 1);
        //    heroImage.color = Color.white

        //    animationsCompleted = animationList.Count;
        //}
    }
}
