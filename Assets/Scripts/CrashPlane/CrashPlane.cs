using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashPlane : MonoBehaviour
{
    public static System.Action onWinCallback = () => { Debug.Log("win"); } ;
    public static System.Action onLoseCallback = () => { Debug.Log("lose"); };

    public static int totalPlanes = 7;
    public static int enemyPlanes = 2;
    public static float timeSpawnRate = 4f;
    public static float planeSpeed = 2f;
    public static float bulletSpeed = 10f;

    public static void SetStaticParams(int _totalPlanes, int _enemyPlanes, float _timeSpawnRate, float _planeSpeed, float _bulletSpeed)
    {
        totalPlanes = _totalPlanes;
        enemyPlanes = _enemyPlanes;
        timeSpawnRate = _timeSpawnRate;
        planeSpeed = _planeSpeed;
        bulletSpeed = _bulletSpeed;
    }

    [SerializeField] private Transform[] leftSpawnPointArray;
    [SerializeField] private Transform[] rightSpawnPointArray;
    [SerializeField] private Transform pfAlliedPlane;
    [SerializeField] private Transform pfEnemyPlane;
    [SerializeField] private BarUI barUI;


    private List<int> spawnPlaneOrderList;
    private int planesSpawned = 0;
    private int score = 0;

    public static bool isEnded { get; private set; }

    private void Start()
    {
        isEnded = false;

        barUI.Setup(0, totalPlanes);

        CodeMonkey.Utils.FunctionPeriodic.Create(() =>
        {
            SpawnNextPlane();
        }, () =>
        {
            if (isEnded)
            {
                return true;
            }
            if (planesSpawned >= totalPlanes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }, timeSpawnRate);

        List<int> enemyIndices = YouKo.UtilsClass.PickRandomIndices(totalPlanes, enemyPlanes);
        spawnPlaneOrderList = new List<int>(); //0 stands for allied plane, 1 - for enemy one
        for (int i = 0; i < totalPlanes; i++)
        {
            spawnPlaneOrderList.Add(0);
        }
        foreach (int index in enemyIndices)
        {
            spawnPlaneOrderList[index] = 1;
        }
    }

    private void SpawnNextPlane()
    {
        if (CrashPlane.isEnded)
            return;

        Debug.Log("spawning a plane");

        Vector3 spawnPos = leftSpawnPointArray[Random.Range(0, leftSpawnPointArray.Length)].position;
        Vector3 targetPos = rightSpawnPointArray[Random.Range(0, rightSpawnPointArray.Length)].position;

        CrashPlane_Plane plane;

        if (spawnPlaneOrderList[planesSpawned] == 0) //allied plane
        {
            plane = CrashPlane_Plane.Create(pfAlliedPlane, spawnPos, targetPos, planeSpeed, false, () =>
            {
                isEnded = true;
                onLoseCallback?.Invoke();
            }, () =>
            {
                IncrementScore();
            });
        }
        else //enemy plane
        {
            plane = CrashPlane_Plane.Create(pfEnemyPlane, spawnPos, targetPos, planeSpeed, true, () =>
            {
                IncrementScore();
            }, () =>
            {
                isEnded = true;
                onLoseCallback?.Invoke();
            });
        }

        

        planesSpawned++;
    }

    private void IncrementScore()
    {
        score++;
        barUI.SetValue(score);
        if (score == totalPlanes)
        {
            isEnded = true;
            onWinCallback?.Invoke();
        }
    }
}
