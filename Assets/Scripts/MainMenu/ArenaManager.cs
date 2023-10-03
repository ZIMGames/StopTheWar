using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] private List<Transform> slotTransformList;
    [SerializeField] private SelectHeroesUI selectHeroesUI;
    [SerializeField] private Transform pfHeroPreview;
    [SerializeField] private Button battleButton;
    [SerializeField] private Image battleBtnImage;
    [SerializeField] private Material blackWhiteMaterial;

    private static Dictionary<Transform, HeroSO> heroSlotDictionary;

    private void Start()
    {
        heroSlotDictionary = new Dictionary<Transform, HeroSO>();

        UpdateButtonVisual();

        foreach(Transform slotTransform in slotTransformList)
        {
            heroSlotDictionary[slotTransform] = null;
            slotTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                selectHeroesUI.ShowUnlockedHeroesPanel((HeroSO heroSO) =>
                {
                    SFXMusic.Instance.PlayCoinImpact();

                    SpawnHeroSlot(slotTransform, heroSO);
                });
            });
        }

        Deck deck = LoadDeck();
        if (deck != null)
        {
            ParseDeck(deck);
        }
    }

    private void SpawnHeroSlot(Transform slotTransform, HeroSO heroSO)
    {
        Transform prevSlotTransform = FindTransformFromHero(heroSO);
        if (prevSlotTransform != null)
        {
            ClearSlot(prevSlotTransform);
        }

        ClearSlot(slotTransform);

        if (heroSO != null)
        {
            Transform container = slotTransform.Find("container");
            Transform heroTransform = Instantiate(pfHeroPreview, container);
            heroTransform.Find("sprite").GetComponent<Image>().sprite = heroSO.sprite;
            slotTransform.Find("text").gameObject.SetActive(false);
            heroSlotDictionary[slotTransform] = heroSO;
        }

        UpdateButtonVisual();
    }

    private void UpdateButtonVisual()
    {
        var list = GetSelectedHeroList();
        bool hasNotNullHero = false;
        foreach (var hero in list)
        {
            if (hero != null)
                hasNotNullHero = true;
        }

        if (hasNotNullHero)
        {
            battleButton.enabled = true;
            battleBtnImage.material = null;
        }
        else
        {
            battleButton.enabled = false;
            battleBtnImage.material = blackWhiteMaterial;
        }
    }


    private void ClearSlot(Transform slotTransform)
    {
        Transform container = slotTransform.Find("container");
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        slotTransform.Find("text").gameObject.SetActive(true);

        heroSlotDictionary[slotTransform] = null;
    }

    private Transform FindTransformFromHero(HeroSO heroSO)
    {
        foreach (KeyValuePair<Transform, HeroSO> entry in heroSlotDictionary)
        {
            if (entry.Value == heroSO)
            {
                return entry.Key;
            }
        }
        return null;
    }

    public static List<Hero> GetSelectedHeroList()
    {
        List<Hero> selectedHeroList = new List<Hero>();
        foreach (var heroSO in heroSlotDictionary.Values.ToList()) 
        {
            if (heroSO != null)
            {
                selectedHeroList.Add(HeroManager.Instance.GetHero(heroSO));
            }
            else
            {
                selectedHeroList.Add(null);
            }
        }

        return selectedHeroList;
    }

    private void ParseDeck(Deck deck)
    {
        List<Transform> transformList = heroSlotDictionary.Keys.ToList();
        for (int i = 0; i < deck.heroTypeList.Count; i++)
        {
            string heroTypeString = deck.heroTypeList[i];
            if (heroTypeString != "None")
            {
                Hero.HeroType heroType = (Hero.HeroType)System.Enum.Parse(typeof(Hero.HeroType), heroTypeString);
                HeroSO heroSO = HeroManager.Instance.GetHeroSO(heroType);
                SpawnHeroSlot(transformList[i], heroSO);
            }
        }
    }

    private Deck LoadDeck()
    {
        if (PlayerPrefs.HasKey("selectedDeck"))
        {
            string json = PlayerPrefs.GetString("selectedDeck");
            Deck deck = JsonUtility.FromJson<Deck>(json);
            return deck;
        }
        else
        {
            return null;
        }
    }

    private void OnDestroy()
    {
        Save();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void Save()
    {
        if (!SaveManager.needSave)
            return;

        if (heroSlotDictionary == null)
            return;

        var heroList = GetSelectedHeroList();
        Deck deck = new Deck();
        foreach (Hero hero in heroList)
        {
            string heroTypeString = hero == null ? "None" : hero.GetHeroType().ToString();
            deck.heroTypeList.Add(heroTypeString);
        }
        string json = JsonUtility.ToJson(deck);
        PlayerPrefs.SetString("selectedDeck", json);
    }
}

[System.Serializable]
public class Deck
{
    public List<string> heroTypeList;

    public Deck()
    {
        heroTypeList = new List<string>();
    }
}
