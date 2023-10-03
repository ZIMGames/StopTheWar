using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    public event System.Action OnHeroDataChanged;

    public static HeroManager Instance { get; private set; }


    [SerializeField] private HeroListSO heroListSO;

    private Dictionary<HeroSO, Hero> heroDictionary;

    private void Awake()
    {
        Instance = this;

        heroDictionary = new Dictionary<HeroSO, Hero>();

        foreach (HeroSO heroSO in heroListSO.list)
        {
            string heroName = heroSO.heroName;
            HeroData heroData = LoadHeroData(heroName);
            if (heroSO.heroType == Hero.HeroType.UkrHero)
            {
                if (heroData == null)
                {
                    heroData = new HeroData { level = 0, unlocked = true };
                }
            }
            else
            {
                if (heroData == null)
                {
                    heroData = new HeroData { level = 0, unlocked = false };
                }
            }
            Hero hero = new Hero(heroData, heroSO.heroType, heroSO.heroRarity);
            hero.OnHeroDataChanged += Hero_OnHeroDataChanged;
            heroDictionary[heroSO] = hero;
        }
    }

    private void Hero_OnHeroDataChanged()
    {
        OnHeroDataChanged?.Invoke();
    }

    private void OnDestroy()
    {
        SaveAllHeroData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("pause");
            SaveAllHeroData();
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("quit");
        SaveAllHeroData();
    }

    private void SaveAllHeroData()
    {
        if (!SaveManager.needSave)
            return;

        foreach (var heroKV in heroDictionary)
        {
            HeroSO heroSO = heroKV.Key;
            Hero hero = heroKV.Value;
            SaveHeroData(heroSO.heroName, hero.GetHeroData());
        }
    }

    private void SaveHeroData(string heroName, HeroData heroData)
    {
        if (!SaveManager.needSave)
            return;

        string json = JsonUtility.ToJson(heroData);
        PlayerPrefs.SetString(heroName, json);
    }

    private HeroData LoadHeroData(string heroName)
    {
        if (PlayerPrefs.HasKey(heroName))
        {
            string json = PlayerPrefs.GetString(heroName);
            HeroData heroData = JsonUtility.FromJson<HeroData>(json);
            return heroData;
        }
        else
        {
            return null;
        }
    }

    public List<HeroSO> GetUnlockedUnderMaxLvlHeroList()
    {
        List<HeroSO> unlockedHeroList = new List<HeroSO>();
        foreach (HeroSO heroSO in heroListSO.list)
        {
            Hero hero = GetHero(heroSO);
            HeroData heroData = hero.GetHeroData();
            if (heroData.unlocked && !hero.IsMaxLevel())
            {
                unlockedHeroList.Add(heroSO);
            }
        }
        return unlockedHeroList;
    }

    public List<HeroSO> GetUnlockedHeroList()
    {
        List<HeroSO> unlockedHeroList = new List<HeroSO>();
        foreach (HeroSO heroSO in heroListSO.list)
        {
            Hero hero = GetHero(heroSO);
            HeroData heroData = hero.GetHeroData();
            if (heroData.unlocked)
            {
                unlockedHeroList.Add(heroSO);
            }
        }
        return unlockedHeroList;
    }

    public List<HeroSO> GetNewHeroList()
    {
        List<HeroSO> newHeroList = new List<HeroSO>();
        foreach (HeroSO heroSO in heroListSO.list)
        {
            HeroData heroData = GetHero(heroSO).GetHeroData();
            if (!heroData.unlocked)
            {
                newHeroList.Add(heroSO);
            }
        }
        return newHeroList;
    }

    public HeroSO GetRandomNewHeroSO()
    {
        var newHeroList = GetNewHeroList();
        if (newHeroList.Count == 0)
        {
            return null;
        }
        else
        {
            return newHeroList[Random.Range(0, newHeroList.Count)];
        }
    }

    public HeroSO GetRandomUnlockedUnderMaxLvlHeroSO()
    {
        var unlockedHeroList = GetUnlockedUnderMaxLvlHeroList();
        if (unlockedHeroList.Count == 0)
        {
            return null;
        }
        else
        {
            return unlockedHeroList[Random.Range(0, unlockedHeroList.Count)];
        }
    }

    public bool HasUnlockedUnderMaxLvlHeroSO()
    {
        return GetRandomUnlockedUnderMaxLvlHeroSO() != null;
    }

    public Hero GetHero(HeroSO heroSO)
    {
        return heroDictionary[heroSO];
    }

    public HeroSO GetHeroSO(Hero.HeroType heroType)
    {
        foreach (HeroSO heroSO in heroListSO.list)
        {
            if (heroSO.heroType == heroType)
            {
                return heroSO;
            }
        }
        Debug.LogError("there's no such hero type, suchka");
        return null;
    }
}