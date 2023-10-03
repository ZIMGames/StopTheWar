using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroUnitGridInfo
{
    public int attackDamage;
    public int healthAmount;
    public int attackDistance;
    public int moveDistanceMax;
    public Hero.HeroType heroType;

    public HeroUnitGridInfo(Hero hero)
    {
        attackDamage = hero.GetAttackDamage();
        healthAmount = hero.GetHealthAmount();
        attackDistance = hero.GetAttackDistance();
        moveDistanceMax = hero.GetMoveDistanceMax();
        heroType = hero.GetHeroType();
    }

    public HeroUnitGridInfo(HeroUnitGridInfoSO heroUnitGridCombatSO)
    {
        attackDamage = heroUnitGridCombatSO.attackDamage;
        healthAmount = heroUnitGridCombatSO.healthAmount;
        attackDistance = heroUnitGridCombatSO.attackDistance;
        moveDistanceMax = heroUnitGridCombatSO.moveDistanceMax;
        heroType = heroUnitGridCombatSO.heroType;
    }

    public static List<HeroUnitGridInfo> GetHeroListConverted(List<Hero> heroList)
    {
        List<HeroUnitGridInfo> convertedHeroList = new List<HeroUnitGridInfo>();
        foreach (var hero in heroList)
        {
            if (hero == null)
            {
                convertedHeroList.Add(null);
            }
            else
            {
                convertedHeroList.Add(new HeroUnitGridInfo(hero));
            }
        }
        return convertedHeroList;
    }

    public static List<HeroUnitGridInfo> GetHeroListConverted(List<HeroUnitGridInfoSO> heroList)
    {
        List<HeroUnitGridInfo> convertedHeroList = new List<HeroUnitGridInfo>();
        foreach (var hero in heroList)
        {
            if (hero == null)
            {
                convertedHeroList.Add(null);
            }
            else
            {
                convertedHeroList.Add(new HeroUnitGridInfo(hero));
            }
        }
        return convertedHeroList;
    }
}