using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemsPanel : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Image gemsImage;

    private List<LTDescr> animationList = new List<LTDescr>();
    private int animationsCompleted;

    private System.Action callback;

    public void Show(Reward.Gems reward, System.Action callback)
    {
        this.callback = callback;
        gameObject.SetActive(true);

        animationList.Clear();
        animationsCompleted = 0;

        text.text = "x" + reward.amount;


        animationList.Add(LeanTween.scale(text.gameObject, new Vector3(1.2f, 1.2f), .5f).setOnComplete(() =>
        {
            LeanTween.scale(text.gameObject, new Vector3(1f, 1f), .5f).setOnComplete(() => { animationsCompleted++; });
        }));
        animationList.Add(LeanTween.scale(gemsImage.gameObject, new Vector3(1.2f, 1.2f), .5f).setOnComplete(() =>
        {
            LeanTween.scale(gemsImage.gameObject, new Vector3(1f, 1f), .5f).setOnComplete(() => { animationsCompleted++; });
        }));
    }

    public void Click()
    {
        if (animationsCompleted == animationList.Count)
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

            text.transform.localScale = new Vector3(1, 1);
            gemsImage.transform.localScale = new Vector3(1, 1);

            animationsCompleted = animationList.Count;
        }
    }
}
