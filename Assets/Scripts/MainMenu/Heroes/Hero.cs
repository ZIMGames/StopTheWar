using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero
{
    public static int[] expToNextLevel = new int[] { 10, 20, 50, 70, 120, 200, 300, 500, 800 };
    public static Dictionary<HeroType, List<int>> heroTypeHealthAmount = new Dictionary<HeroType, List<int>> {
        {HeroType.UkrHero, new List<int> { 60, 66, 72, 80, 88, 100, 110, 120, 132, 150} },
        {HeroType.FranceHero, new List<int> { 100, 110, 120, 130, 140, 150, 170, 190, 210, 240} },
        {HeroType.PolandHero, new List<int> { 60, 66, 72, 80, 88, 100, 110, 120, 132, 150} },
        {HeroType.Yob, new List<int> { 40, 44, 50, 55, 60, 66, 72, 80, 88, 100} },
        {HeroType.Yob2, new List<int> { 40, 44, 50, 55, 60, 66, 72, 80, 88, 100} },
        {HeroType.Gypsy, new List<int> { 40, 44, 50, 55, 60, 66, 72, 80, 88, 100} },
        {HeroType.Tank, new List<int> { 150, 165, 180, 200, 220, 240, 265, 300, 330, 370} },
        {HeroType.Tank2, new List<int> { 200, 220, 240, 280, 300, 320, 340, 360, 390, 420} },
        {HeroType.Bayraktar, new List<int> { 50, 60, 70, 80, 90, 100, 110, 120, 140, 160} },
    };
    public static Dictionary<HeroType, List<int>> heroTypeAttackDamage = new Dictionary<HeroType, List<int>> {
        {HeroType.UkrHero, new List<int> { 6, 7, 8, 8, 9, 10, 11, 13, 15, 20} },
        {HeroType.FranceHero, new List<int> { 10, 12, 14, 16, 18, 20, 22, 25, 28, 32} },
        {HeroType.PolandHero, new List<int> { 6, 7, 8, 8, 9, 10, 11, 13, 15, 20} },
        {HeroType.Yob, new List<int> { 5, 6, 7, 8, 8, 9, 9, 10, 11, 14} },
        {HeroType.Yob2, new List<int> { 5, 6, 7, 8, 8, 9, 9, 10, 11, 14} },
        {HeroType.Gypsy, new List<int> { 5, 6, 7, 8, 8, 9, 9, 10, 11, 14} },
        {HeroType.Tank, new List<int> { 10, 12, 14, 16, 18, 20, 22, 24, 26, 29} },
        {HeroType.Tank2, new List<int> { 15, 17, 19, 21, 24, 27, 30, 34, 38, 42} },
        {HeroType.Bayraktar, new List<int> { 15, 17, 20, 22, 25, 27, 30, 33, 36, 40} },
    };

    public static Dictionary<HeroType, int> heroTypeAttackDistance = new Dictionary<HeroType, int> {
        {HeroType.UkrHero, 5 },
        {HeroType.FranceHero, 5 },
        {HeroType.PolandHero, 5 },
        {HeroType.Yob, 3 },
        {HeroType.Yob2, 3},
        {HeroType.Gypsy, 3 },
        {HeroType.Tank, 6 },
        {HeroType.Tank2, 6 },
        {HeroType.Bayraktar, 6 },
    };

    public static Dictionary<HeroType, int> heroTypeMoveDistance = new Dictionary<HeroType, int> {
        {HeroType.UkrHero, 4},
        {HeroType.FranceHero, 4 },
        {HeroType.PolandHero, 4 },
        {HeroType.Yob, 7 },
        {HeroType.Yob2, 7},
        {HeroType.Gypsy, 7 },
        {HeroType.Tank, 3 },
        {HeroType.Tank2, 3 },
        {HeroType.Bayraktar, 5 },
    };


    public event System.Action OnHeroDataChanged;

    public enum HeroType
    {
        UkrHero,
        PolandHero,
        FranceHero,
        Tank,
        Tank2,
        Yob,
        Yob2,
        Gypsy,
        RusHero,
        RusTank,
        Bayraktar,
        RusHero2,
        RusTank2
    }
    public enum Rarity
    {
        Common,
        Rare
    }

    protected HeroType heroType;
    protected Rarity heroRarity;
    private HeroData heroData;

    public Hero(HeroData heroData, HeroType heroType, Rarity heroRarity)
    {
        this.heroData = heroData;
        this.heroType = heroType;
        this.heroRarity = heroRarity;
    }


    public void AddCards(int amount)
    {
        heroData.cards += amount;
        OnHeroDataChanged?.Invoke();
    }

    public void Unlock()
    {
        Debug.Log(heroType.ToString() + " unlocked!");
        heroData.unlocked = true;
        OnHeroDataChanged?.Invoke();
    }

    public void Upgrade()
    {
        if (CanUpgrade())
        {
            heroData.cards -= GetExpToNextLevel();
            heroData.level++;
            OnHeroDataChanged?.Invoke();
        }
    }

    public bool CanUpgrade()
    {
        return !IsMaxLevel() && heroData.cards >= GetExpToNextLevel();
    }

    public int GetExpToNextLevel()
    {
        return expToNextLevel[heroData.level];
    }

    public bool IsMaxLevel()
    {
        return heroData.level == expToNextLevel.Length;
    }

    public HeroData GetHeroData()
    {
        return heroData;
    }

    public HeroType GetHeroType()
    {
        return heroType;
    }

    public Rarity GetHeroRarity()
    {
        return heroRarity;
    }

    public virtual int GetAttackDamage()
    {
        Debug.Log(heroType.ToString());
        return heroTypeAttackDamage[heroType][heroData.level];
    }

    public virtual int GetHealthAmount() => heroTypeHealthAmount[heroType][heroData.level];

    public virtual int GetAttackDistance() => heroTypeAttackDistance[heroType];

    public virtual int GetMoveDistanceMax() => heroTypeMoveDistance[heroType];
}

[System.Serializable]
public class HeroData
{
    public int level;
    public bool unlocked;
    public int cards;
}

