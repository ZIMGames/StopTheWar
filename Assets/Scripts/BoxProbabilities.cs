using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoxProbabilities
{
    public static BoxProbabilityArithmetic boxProbability = new BoxProbabilityArithmetic(0.1f, new Dictionary<BoxSystem.BoxType, float>
    {
        { BoxSystem.BoxType.Free, 0.55f },
        { BoxSystem.BoxType.Big, 0.85f },
        { BoxSystem.BoxType.Mega, 1.3f },
    }, PlayerPrefs.GetFloat("boxProbability", 0.1f));

    public static bool DrawHeroRarity(Hero.Rarity heroRarity)
    {
        int threshold = 0;
        if (heroRarity == Hero.Rarity.Common)
        {
            threshold = 100;
        }
        else if (heroRarity == Hero.Rarity.Rare)
        {
            threshold = 50;
        }

        return Random.Range(0, 100) < threshold;
    }
}

public class BoxProbabilityArithmetic
{
    private readonly float p0;
    private readonly Dictionary<BoxSystem.BoxType, float> q;
    private float p;

    public BoxProbabilityArithmetic(float p0, Dictionary<BoxSystem.BoxType, float> q, float p)
    {
        this.p0 = p0;
        this.q = q;
        this.p = p;
    }

    public bool Draw(BoxSystem.BoxType boxType)
    {
        Debug.Log(p);
        if (Random.Range(0f, 100f) <= p)
        {
            //Reset();
            return true;
        }
        else
        {
            p += q[boxType];
            PlayerPrefs.SetFloat("boxProbability", p);
            return false;
        }
    }

    public void Reset()
    {
        p = p0;
        PlayerPrefs.SetFloat("boxProbability", p);
    }
}
