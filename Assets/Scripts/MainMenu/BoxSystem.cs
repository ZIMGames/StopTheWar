using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxSystem : MonoBehaviour
{
    public enum BoxType
    {
        Free,
        Big,
        Mega
    }

    [SerializeField] private ResourceTypeSO coins, gems;
    [SerializeField] private GameObject dropPanel;
    [SerializeField] private CoinsPanel coinsPanel;
    [SerializeField] private GemsPanel gemsPanel;
    [SerializeField] private HeroPanel heroPanel;
    [SerializeField] private NewHeroPanel newHeroPanel;
    [SerializeField] private BoxAnimationPanel boxAnimationPanel;


    private RewardSystem rewardSystem = new RewardSystem();

    public void OpenBox(int boxType)
    {
        rewardSystem.Clear();
        switch((BoxType)boxType)
        {
            case BoxType.Free:
                rewardSystem.AddReward(new Reward.BoxAnimation { index = 0 });

                rewardSystem.AddReward(new Reward.Coins { amount = Random.Range(4, 11) });

                if (HeroManager.Instance.HasUnlockedUnderMaxLvlHeroSO())
                {
                    rewardSystem.AddReward(new Reward.Hero { heroSO = HeroManager.Instance.GetRandomUnlockedUnderMaxLvlHeroSO(), amount = 8 });
                    rewardSystem.AddReward(new Reward.Hero { heroSO = HeroManager.Instance.GetRandomUnlockedUnderMaxLvlHeroSO(), amount = 6 });
                }

                if (BoxProbabilities.boxProbability.Draw(BoxType.Free))
                {
                    HeroSO randomNewHeroSO = HeroManager.Instance.GetRandomNewHeroSO();
                    if (randomNewHeroSO != null)
                    {
                        if (BoxProbabilities.DrawHeroRarity(randomNewHeroSO.heroRarity))
                        {
                            BoxProbabilities.boxProbability.Reset();
                            rewardSystem.AddReward(new Reward.NewHero { heroSO = randomNewHeroSO });
                        }
                    }
                }
                break;




            case BoxType.Big:
                rewardSystem.AddReward(new Reward.BoxAnimation { index = 1 });

                rewardSystem.AddReward(new Reward.Coins { amount = Random.Range(8, 22) });

                if (HeroManager.Instance.HasUnlockedUnderMaxLvlHeroSO())
                {
                    rewardSystem.AddReward(new Reward.Hero { heroSO = HeroManager.Instance.GetRandomUnlockedUnderMaxLvlHeroSO(), amount = 23 });
                    rewardSystem.AddReward(new Reward.Hero { heroSO = HeroManager.Instance.GetRandomUnlockedUnderMaxLvlHeroSO(), amount = 19 });
                }

                if (BoxProbabilities.boxProbability.Draw(BoxType.Big))
                {
                    HeroSO randomNewHeroSO = HeroManager.Instance.GetRandomNewHeroSO();
                    if (randomNewHeroSO != null)
                    {
                        if (BoxProbabilities.DrawHeroRarity(randomNewHeroSO.heroRarity))
                        {
                            BoxProbabilities.boxProbability.Reset();
                            rewardSystem.AddReward(new Reward.NewHero { heroSO = randomNewHeroSO });
                        }
                    }
                }

                //rewardSystem.AddReward(new Reward.Gems { amount = 10 });
                break;




            case BoxType.Mega:
                rewardSystem.AddReward(new Reward.BoxAnimation { index = 2 });

                rewardSystem.AddReward(new Reward.Coins { amount = 36 });

                if (HeroManager.Instance.HasUnlockedUnderMaxLvlHeroSO())
                {
                    rewardSystem.AddReward(new Reward.Hero { heroSO = HeroManager.Instance.GetRandomUnlockedUnderMaxLvlHeroSO(), amount = 20 });
                    rewardSystem.AddReward(new Reward.Hero { heroSO = HeroManager.Instance.GetRandomUnlockedUnderMaxLvlHeroSO(), amount = 15 });
                    rewardSystem.AddReward(new Reward.Hero { heroSO = HeroManager.Instance.GetRandomUnlockedUnderMaxLvlHeroSO(), amount = 10 });
                }

                if (Random.Range(1, 101) <= 9 || true)
                {
                    HeroSO randomNewHeroSO = HeroManager.Instance.GetRandomNewHeroSO();
                    if (randomNewHeroSO != null)
                    {
                        rewardSystem.AddReward(new Reward.NewHero { heroSO = randomNewHeroSO });
                    }
                }

                //rewardSystem.AddReward(new Reward.Gems { amount = 20 });
                break;
        }

        Process();
    }

/*    public void Click()
    {
        Process();
    }*/

    private void Process()
    {
        Reward reward = rewardSystem.GetNextReward();
        if (reward == null)
        {
            return;
        }
        else
        {
            ReceiveReward(reward);
        }
    }

   /* private void Show()
    {
        dropPanel.SetActive(true);
    }*/

    private void Hide()
    {
        dropPanel.SetActive(false);
        Debug.Log("hide");
    }

    private void ReceiveReward(Reward reward)
    {
        if (reward is Reward.BoxAnimation)
        {
            ReceiveBoxAnimationReward(reward as Reward.BoxAnimation);
            return;
        }
        if (reward is Reward.Coins)
        {
            ReceiveCoinsReward(reward as Reward.Coins);
            return;
        }
        if (reward is Reward.NewHero)
        {
            ReceiveNewHeroReward(reward as Reward.NewHero);
            return;
        }
        if (reward is Reward.Hero)
        {
            ReceiveHeroReward(reward as Reward.Hero);
            return;
        }
        if (reward is Reward.Gems)
        {
            ReceiveGemsReward(reward as Reward.Gems);
            return;
        }
    }

    private void ReceiveBoxAnimationReward(Reward.BoxAnimation reward)
    {
        SFXMusic.Instance.PlayBoxDrop();
        boxAnimationPanel.Show(reward, () => { Process(); });
        //Debug.Log("animating box opening");
    }
    private void ReceiveCoinsReward(Reward.Coins reward)
    {
        SFXMusic.Instance.PlayCoinsDrop();
        coinsPanel.Show(reward, () => { Process(); });
        //Debug.Log("coins");
        ResourceManager.Instance.AddResources(coins, reward.amount);
    }

    private void ReceiveNewHeroReward(Reward.NewHero reward)
    {
        BackgroundMusic.Mute();
        SFXMusic.Instance.PlayNewHeroSound();
        newHeroPanel.Show(reward, () => { Process(); });
        //Debug.Log("hero unlocked: " + reward.heroSO.heroName);
        HeroManager.Instance.GetHero(reward.heroSO).Unlock();
    }

    private void ReceiveHeroReward(Reward.Hero reward)
    {
        SFXMusic.Instance.PlayPowerPointsDrop();
        heroPanel.Show(reward, () => { Process(); });
        //Debug.Log("hero " + reward.amount);
        HeroManager.Instance.GetHero(reward.heroSO).AddCards(reward.amount);
    }

    private void ReceiveGemsReward(Reward.Gems reward)
    {
        SFXMusic.Instance.PlayGemsDrop();
        gemsPanel.Show(reward, () => { Process(); });
        //Debug.Log("gems");
        ResourceManager.Instance.AddResources(gems, reward.amount);
    }
}
