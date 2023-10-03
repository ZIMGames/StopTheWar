using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHeroesUI : MonoBehaviour
{
    [SerializeField] private Transform pfHero;
    [SerializeField] private Transform container;


    public void ShowUnlockedHeroesPanel(System.Action<HeroSO> onHeroSelected)
    {
        var unlockedHeroList = HeroManager.Instance.GetUnlockedHeroList();
/*        if (exceptionHeroList != null)
        {
            foreach (HeroSO heroSO in exceptionHeroList)
            {
                if (unlockedHeroList.Contains(heroSO))
                {
                    unlockedHeroList.Remove(heroSO);
                }
            }
        }*/

        if (unlockedHeroList.Count == 0)
        {
            Debug.Log("Open more boxes!");
            return;
        }

        gameObject.SetActive(true);


        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }


        foreach (HeroSO heroSO in unlockedHeroList)
        {
            Transform heroTransform = Instantiate(pfHero, container);
            heroTransform.Find("sprite").GetComponent<Image>().sprite = heroSO.sprite;
            heroTransform.Find("background").GetComponent<Image>().color = heroSO.heroRarity == Hero.Rarity.Common ? new Color32(95, 139, 178, 255) : new Color32(130, 95, 178, 255);
            heroTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                onHeroSelected?.Invoke(heroSO);
                HidePanel();
            });
        }
    }

    private void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
