using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxAnimationPanel : MonoBehaviour
{
   /* [SerializeField] private Text text;*/
    [SerializeField] private Image boxImage;
    [SerializeField] private Image shadowImage;
    [SerializeField] private List<Sprite> boxSpriteList;

    private List<LTDescr> animationList = new List<LTDescr>();
    private int animationsCompleted;

    private System.Action callback;

    public void Show(Reward.BoxAnimation reward, System.Action callback)
    {
        this.callback = callback;
        gameObject.SetActive(true);

        animationList.Clear();
        animationsCompleted = 0;

        boxImage.sprite = boxSpriteList[reward.index];

        boxImage.rectTransform.localPosition = new Vector3(0, 500);
        shadowImage.transform.localScale = new Vector3(1.5f, 1.5f);

        /*animationList.Add(LeanTween.scale(text.gameObject, new Vector3(1.2f, 1.2f), .5f).setOnComplete(() =>
        {
            LeanTween.scale(text.gameObject, new Vector3(1f, 1f), .5f).setOnComplete(() => { animationsCompleted++; });
        }));*/
        animationList.Add(LeanTween.moveLocalY(boxImage.gameObject, 0, .3f).setOnComplete(() =>
        {
            animationsCompleted++;
        }));
        animationList.Add(LeanTween.scale(shadowImage.gameObject, new Vector3(1, 1), .3f).setOnComplete(() =>
        {
            animationsCompleted++;
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

            /*text.transform.localScale = new Vector3(1, 1);*/
            boxImage.transform.localPosition = new Vector3(0, 0);
            shadowImage.transform.localScale = new Vector3(1, 1);

            animationsCompleted = animationList.Count;
        }
    }
}
