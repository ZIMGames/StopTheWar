using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSystem
{
    private List<Reward> rewardList;

    public RewardSystem()
    {
        rewardList = new List<Reward>();
    }

    public void AddReward(Reward reward)
    {
        rewardList.Add(reward);
    }

    public Reward GetNextReward()
    {
        if (rewardList.Count == 0) return null;

        Reward reward = rewardList[0];
        rewardList.RemoveAt(0);
        return reward;
    }

    public void Clear()
    {
        rewardList.Clear();
    }
}

public abstract class Reward
{
    public class BoxAnimation : Reward
    {
        public int index;
    }

    public class Coins : Reward
    {
        public int amount;
    }

    public class Gems : Reward
    {
        public int amount;
    }

    public class Hero : Reward
    {
        public HeroSO heroSO;
        public int amount;
    }

    public class NewHero : Reward
    {
        public HeroSO heroSO;
    }
}
