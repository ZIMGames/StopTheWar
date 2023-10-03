using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WolfAndEggs : MonoBehaviour
{
    //0 - top left
    //1 - top right
    //2 - bottom right
    //3 - bottom left

    public enum State
    {
        Normal,
        Waiting,
        GameEnded
    }

    private State state;


    public static System.Action onWinCallback = () => { Debug.Log("win"); };
    public static System.Action onLoseCallback = () => { Debug.Log("lose"); };


    public static int waves = 7;
    public static float timeBetweenWaves = 1.5f;
    public static float moveSpeed;

    public static void SetStaticParams(int _waves, float _timeBetweenWaves, float _moveSpeed)
    {
        waves = _waves;
        timeBetweenWaves = _timeBetweenWaves;
        moveSpeed = _moveSpeed;
    }

    [SerializeField] private UnitGridCombat unitGridCombat;
    [SerializeField] private Transform pfUnitGridCombat;
    [SerializeField] private Image progressFillImage;
    private float progressFillAnimated;

    [Space]
    [SerializeField] private Transform[] spawnPointArray;

    private Queue<UnitGridCombat>[] spawnQueueArray;

    private int EnemiesKilled
    {
        get
        {
            return enemiesKilled;
        }
        set
        {
            enemiesKilled = value;

            if (enemiesKilled >= waves)
            {
                state = State.GameEnded;
                onWinCallback?.Invoke();
            }

            progressFillAnimated = (float)enemiesKilled / waves;
        }
    }
    private int enemiesKilled;

    private int enemyUnitsSpawned;


    private void Awake()
    {
        spawnQueueArray = new Queue<UnitGridCombat>[4];

        spawnQueueArray[0] = new Queue<UnitGridCombat>();
        spawnQueueArray[1] = new Queue<UnitGridCombat>();
        spawnQueueArray[2] = new Queue<UnitGridCombat>();
        spawnQueueArray[3] = new Queue<UnitGridCombat>();

        EnemiesKilled = 0;

        enemyUnitsSpawned = 0;

        state = State.Normal;
    }

    private void Start()
    {
        CodeMonkey.Utils.FunctionPeriodic.Create(SpawnEnemyUnit, () =>
        {
            if (state == State.GameEnded || enemyUnitsSpawned >= waves)
            {
                return true;
            }
            return false;
        }, timeBetweenWaves);
    }

    private void Update()
    {
        progressFillImage.fillAmount = Mathf.Lerp(progressFillImage.fillAmount, progressFillAnimated, 0.5f);
    }

    private void SpawnEnemyUnit()
    {
        enemyUnitsSpawned++;

        int direction = Random.Range(0, 4);

        Transform enemyUnitGridCombatTransform = Instantiate(pfUnitGridCombat, spawnPointArray[direction].position, Quaternion.identity);
        MoveVelocity moveVelocity = enemyUnitGridCombatTransform.GetComponent<MoveVelocity>();
        moveVelocity.SetMoveSpeed(moveSpeed);
        UnitGridCombat enemyUnitGridCombat = enemyUnitGridCombatTransform.GetComponent<UnitGridCombat>();
        //Debug.Log(unitGridCombat.GetPosition());
        enemyUnitGridCombat.MoveTo(unitGridCombat.GetPosition(), () =>
        {
            if (state != State.GameEnded)
            {
                Debug.Log("lose");
                state = State.GameEnded;
                onLoseCallback?.Invoke();
            }
        });
        spawnQueueArray[direction].Enqueue(enemyUnitGridCombat);
        
    }


    public void ChooseDirToShoot(int dir)
    {
        if (state == State.Waiting || state == State.GameEnded)
            return;

        if (spawnQueueArray[dir].Count > 0)
        {
            UnitGridCombat enemyUnitGridCombat = spawnQueueArray[dir].Dequeue();
            state = State.Waiting;
            unitGridCombat.AttackUnit(enemyUnitGridCombat, () =>
            {
                if (state != State.GameEnded)
                {
                    state = State.Normal;
                    enemyUnitGridCombat.Damage(unitGridCombat, 100);
                    EnemiesKilled++;
                }
            });
        }
    }
}
