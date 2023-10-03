using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    private enum State
    {
        Prepare,
        Interrupted,
        Spawned
    }

    [SerializeField] private Transform pfCoin;
    [SerializeField] private Transform targetTransform;

    private State state;

    private List<CoinSpawnerSingle> list;
    private int coins;
    private System.Action<int> onCoinReachedPosition;
    private int copiesArrived;
    private int coinsCopies;


    public bool IsEveryCopyArrived()
    {
        return copiesArrived == coinsCopies;
    }

    public IEnumerator Spawn(int coins, Vector3 spawnPos, float scatterRadius, Vector3 targetPos, System.Action<int> onCoinReachedPosition)
    {
        this.coins = coins;
        this.onCoinReachedPosition = onCoinReachedPosition;

        if (targetTransform != null)
        {
            targetPos = targetTransform.position;
        }

        int coinsCopiesMax = 40;
        copiesArrived = 0;
        coinsCopies = Mathf.Min(coins, coinsCopiesMax);
        int coinsPerCopy = coins / coinsCopies;

        state = State.Prepare;


        yield return new WaitForSeconds(.5f);



        if (state == State.Interrupted)
        {
            yield break;
        }


        list = new List<CoinSpawnerSingle>();
        for (int i = 0; i < coinsCopies; i++)
        {
            Transform coinTransform = Instantiate(pfCoin, spawnPos, Quaternion.identity);
            list.Add(coinTransform.GetComponent<CoinSpawnerSingle>());
            LeanTween.move(coinTransform.gameObject, spawnPos + CodeMonkey.Utils.UtilsClass.GetRandomDir() * Random.Range(0, scatterRadius), 0.5f);

            float travelTime = 0.7f;
            float extraTime = 0.6f / Mathf.Max(coinsCopies - 1, 1);
            if (i < coinsCopies - 1)
            {
                list[i].Setup(targetPos, travelTime + extraTime * i, .7f, () =>
                {
                    onCoinReachedPosition(coinsPerCopy);
                    copiesArrived++;
                });
            }
            else
            {
                list[i].Setup(targetPos, travelTime + extraTime * i, .7f, () =>
                {
                    onCoinReachedPosition(coinsPerCopy + coins % coinsCopies);
                    copiesArrived++;
                });
            }
        }
        state = State.Spawned;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Stop();
        }
    }

    public void Stop()
    {
        if (state == State.Spawned)
        {
            foreach (var coinSingle in list)
            {
                if (coinSingle != null)
                {
                    coinSingle.Stop();
                }
            }
            state = State.Interrupted;
            copiesArrived = coinsCopies;
        }
        else if (state == State.Prepare)
        {
            onCoinReachedPosition?.Invoke(coins);
            state = State.Interrupted;
            copiesArrived = coinsCopies;
        }
    }
    
}
