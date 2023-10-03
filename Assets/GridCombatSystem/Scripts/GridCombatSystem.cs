using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using GridPathfindingSystem;

public class GridCombatSystem : MonoBehaviour {

    [SerializeField] private Transform rusHeroPrefab, ukrHeroPrefab, polandHeroPrefab, franceHeroPrefab, yobPrefab, yob2Prefab, gypsyPrefab, bayraktarPrefab, tankPrefab, rusTankPrefab, tank2Prefab, rusHero2Prefab, rusTank2Prefab;


    public static System.Action onWinCallback;
    public static System.Action onLoseCallback;
    public static System.Action<Hero.HeroType> onEnemyDiedCallback;


    private static List<HeroUnitGridInfo> alliedHeroUnitGridInfoList;
    private static List<HeroUnitGridInfo> enemyHeroUnitGridInfoList;
    private const int MAX_HERO_IN_ROW = 3;

    public static void SetParams(List<HeroUnitGridInfo> _alliedHeroUnitGridInfoList, List<HeroUnitGridInfo> _enemyHeroUnitGridInfoList)
    {
        alliedHeroUnitGridInfoList = _alliedHeroUnitGridInfoList;
        enemyHeroUnitGridInfoList = _enemyHeroUnitGridInfoList;
    }


    private List<IUnitGridCombat> unitGridCombatList = new List<IUnitGridCombat>();

    private State state;
    private IUnitGridCombat unitGridCombat;
    private List<IUnitGridCombat> blueTeamList;
    private List<IUnitGridCombat> redTeamList;
    private int blueTeamActiveUnitIndex;
    private int redTeamActiveUnitIndex;

    private List<Vector2Int> possibleMoves = new List<Vector2Int>();

    private enum State {
        WaitingForMove,
        WaitingForAttack,
        Busy,
        GameEnded
    }

    private void Awake() {
        state = State.WaitingForMove;
    }

    private void Start() {
        //unitGridCombatList = new List<IUnitGridCombat>() {
        //    SpawnUnitGridCombat(rusHeroPrefab, UnitGridCombat.Team.Red, 0, 0),
        //    SpawnUnitGridCombat(rusHeroPrefab, UnitGridCombat.Team.Red, 5, 0),
        //    SpawnUnitGridCombat(rusHeroPrefab, UnitGridCombat.Team.Red, 5, 1),
        //    SpawnUnitGridCombat(ukrHeroPrefab, UnitGridCombat.Team.Blue, 1, 5),
        //    SpawnUnitGridCombat(ukrHeroPrefab, UnitGridCombat.Team.Blue, 0, 5),
        //    SpawnUnitGridCombat(ukrHeroPrefab, UnitGridCombat.Team.Blue, 5, 5),
        //};

        //SPAWN ENEMY HEROES
        Vector2Int startEnemySpawnPos = new Vector2Int(3, 6);
        for (int i = 0; i < enemyHeroUnitGridInfoList.Count; i++)
        {
            var heroUnitGridInfo = enemyHeroUnitGridInfoList[i];
            if (heroUnitGridInfo == null)
            {
                continue;
            }

            int x = i % MAX_HERO_IN_ROW;
            int y = i / MAX_HERO_IN_ROW;

            IUnitGridCombat unitGridCombat = SpawnUnitGridCombat(GetUnitGridPrefab(heroUnitGridInfo.heroType), UnitGridCombat.Team.Red, x + startEnemySpawnPos.x, y + startEnemySpawnPos.y);
            unitGridCombat.SetHeroUnitGridInfo(heroUnitGridInfo);
            unitGridCombatList.Add(unitGridCombat);
        }

        //SPAWN ALLIED HEROES
        Vector2Int startAllySpawnPos = new Vector2Int(0, 0);
        for (int i = 0; i < alliedHeroUnitGridInfoList.Count; i++)
        {
            var heroUnitGridInfo = alliedHeroUnitGridInfoList[i];
            if (heroUnitGridInfo == null)
            {
                continue;
            }

            int x = i % MAX_HERO_IN_ROW;
            int y = i / MAX_HERO_IN_ROW;

            IUnitGridCombat unitGridCombat = SpawnUnitGridCombat(GetUnitGridPrefab(heroUnitGridInfo.heroType), UnitGridCombat.Team.Blue, x + startAllySpawnPos.x, y + startAllySpawnPos.y);
            unitGridCombat.SetHeroUnitGridInfo(heroUnitGridInfo);
            unitGridCombatList.Add(unitGridCombat);
        }






        blueTeamList = new List<IUnitGridCombat>();
        redTeamList = new List<IUnitGridCombat>();
        blueTeamActiveUnitIndex = -1;
        redTeamActiveUnitIndex = -1;

        // Set all UnitGridCombat on their GridPosition
        for (int i = 0; i < unitGridCombatList.Count; i++) {
            IUnitGridCombat unitGridCombat = unitGridCombatList[i];

            GameHandler_GridCombatSystem.Instance.GetGrid().GetGridObject(unitGridCombat.GetPosition())
                .SetUnitGridCombat(unitGridCombat);


            unitGridCombat.SetOnDeadCallback(() =>
            {
                GameHandler_GridCombatSystem.Instance.GetGrid().GetGridObject(unitGridCombat.GetPosition()).ClearUnitGridCombat();
                if (unitGridCombat.GetTeam() == UnitGridCombat.Team.Blue)
                {
                    int index = blueTeamList.IndexOf(unitGridCombat);
                    if (blueTeamActiveUnitIndex >= index)
                    {
                        blueTeamActiveUnitIndex--;
                    }
                    blueTeamList.RemoveAt(index);
                    if (blueTeamList.Count == 0)
                    {
                        state = State.GameEnded;
                        onLoseCallback?.Invoke();
                    }
                }
                else
                {
                    onEnemyDiedCallback?.Invoke(unitGridCombat.GetHeroUnitGridInfo().heroType);
                    int index = redTeamList.IndexOf(unitGridCombat);
                    if (redTeamActiveUnitIndex >= index)
                    {
                        redTeamActiveUnitIndex--;
                    }
                    redTeamList.RemoveAt(index);
                    if (redTeamList.Count == 0)
                    {
                        state = State.GameEnded;
                        onWinCallback?.Invoke();
                    }
                }
            });


            if (unitGridCombat.GetTeam() == UnitGridCombat.Team.Blue) {
                blueTeamList.Add(unitGridCombat);
            } else {
                redTeamList.Add(unitGridCombat);
            }
        }

        SelectNextActiveUnit();
        bool moveAvailable = UpdateValidMovePositions();
        if (!moveAvailable)
        {
            Process();
        }
    }

    private Transform GetUnitGridPrefab(Hero.HeroType heroType)
    {
        switch(heroType)
        {
            case Hero.HeroType.RusHero: return rusHeroPrefab;
            case Hero.HeroType.RusHero2: return rusHero2Prefab;
            case Hero.HeroType.UkrHero: return ukrHeroPrefab;
            case Hero.HeroType.PolandHero: return polandHeroPrefab;
            case Hero.HeroType.FranceHero: return franceHeroPrefab;
            case Hero.HeroType.Tank: return tankPrefab;
            case Hero.HeroType.Tank2: return tank2Prefab;
            case Hero.HeroType.RusTank: return rusTankPrefab;
            case Hero.HeroType.RusTank2: return rusTank2Prefab;
            case Hero.HeroType.Gypsy: return gypsyPrefab;
            case Hero.HeroType.Yob: return yobPrefab;
            case Hero.HeroType.Yob2: return yob2Prefab;
            case Hero.HeroType.Bayraktar: return bayraktarPrefab;
        }
        return null;
    }

    private IUnitGridCombat SpawnUnitGridCombat(Transform prefab, UnitGridCombat.Team team, int x, int y)
    {
        

        var grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        Transform pfUnitGridTransform = Instantiate(prefab,
            grid.GetWorldPosition(x, y) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f,
            Quaternion.identity);
        IUnitGridCombat unitGridCombat = pfUnitGridTransform.GetComponent<IUnitGridCombat>();
        return unitGridCombat;
    }

    private void SelectNextActiveUnit() {
        if (unitGridCombat != null)
        {
            unitGridCombat.SetSelectedVisible(false);
        }

        if (unitGridCombat == null || unitGridCombat.GetTeam() == UnitGridCombat.Team.Red) {
            unitGridCombat = GetNextActiveUnit(UnitGridCombat.Team.Blue);
        } else {
            unitGridCombat = GetNextActiveUnit(UnitGridCombat.Team.Red);
        }
        unitGridCombat.SetSelectedVisible(true);

        GameHandler_GridCombatSystem.Instance.SetCameraFollowPosition(unitGridCombat.GetPosition());
    }

    private IUnitGridCombat GetNextActiveUnit(UnitGridCombat.Team team) {
        if (team == UnitGridCombat.Team.Blue) {
            blueTeamActiveUnitIndex = (blueTeamActiveUnitIndex + 1) % blueTeamList.Count;
            if (blueTeamList[blueTeamActiveUnitIndex] == null || blueTeamList[blueTeamActiveUnitIndex].IsDead()) {
                // Unit is Dead, get next one
                return GetNextActiveUnit(team);
            } else {
                return blueTeamList[blueTeamActiveUnitIndex];
            }
        } else {
            redTeamActiveUnitIndex = (redTeamActiveUnitIndex + 1) % redTeamList.Count;
            if (redTeamList[redTeamActiveUnitIndex] == null || redTeamList[redTeamActiveUnitIndex].IsDead()) {
                // Unit is Dead, get next one
                return GetNextActiveUnit(team);
            } else {
                return redTeamList[redTeamActiveUnitIndex];
            }
        }
    }

    private bool UpdateValidAttackPositions()
    {
        Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();

        // Get Unit Grid Position X, Y
        grid.GetXY(unitGridCombat.GetPosition(), out int unitX, out int unitY);

        // Set entire Tilemap to Invisible
        GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
            MovementTilemap.TilemapObject.TilemapSprite.None
        );

        // Reset Entire Grid ValidAttacks
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                grid.GetGridObject(x, y).SetIsValidMovePosition(false);
            }
        }

        possibleMoves.Clear();
        int maxAttackDistance = unitGridCombat.GetHeroUnitGridInfo().attackDistance;
        //int maxAttackDistance = 5;
        for (int x = unitX - maxAttackDistance; x <= unitX + maxAttackDistance; x++)
        {
            for (int y = unitY - maxAttackDistance; y <= unitY + maxAttackDistance; y++)
            {
                //verify x, y
                if (x < 0 || y < 0 || x >= grid.GetWidth() || y >= grid.GetHeight())
                    continue;


                if (ManhattenDistance(unitX, unitY, x, y) <= maxAttackDistance)
                {
                    GridObject gridObject = grid.GetGridObject(x, y);

                    if (gridObject.GetUnitGridCombat() != null)
                    {
                        if (unitGridCombat.IsEnemy(gridObject.GetUnitGridCombat()))
                        {
                            GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetTilemapSprite(
                            x, y, MovementTilemap.TilemapObject.TilemapSprite.Attack);

                            gridObject.SetIsValidMovePosition(true);
                            possibleMoves.Add(new Vector2Int(x, y));
                        }
                    }
                }
            }
        }
        return possibleMoves.Count > 0;
    }

    private int ManhattenDistance(int startX, int startY, int endX, int endY)
    {
        return Mathf.Abs(startX - endX) + Mathf.Abs(startY - endY);
    }

    private bool UpdateValidMovePositions()
    {
        Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        GridPathfinding gridPathfinding = GameHandler_GridCombatSystem.Instance.gridPathfinding;

        // Get Unit Grid Position X, Y
        grid.GetXY(unitGridCombat.GetPosition(), out int unitX, out int unitY);

        // Set entire Tilemap to Invisible
        GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
            MovementTilemap.TilemapObject.TilemapSprite.None
        );

        // Reset Entire Grid ValidMovePositions
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                grid.GetGridObject(x, y).SetIsValidMovePosition(false);
            }
        }

        possibleMoves.Clear();
        int maxMoveDistance = unitGridCombat.GetHeroUnitGridInfo().moveDistanceMax;
        Debug.Log(maxMoveDistance);
        //int maxMoveDistance = 5;
        for (int x = unitX - maxMoveDistance; x <= unitX + maxMoveDistance; x++) {
            for (int y = unitY - maxMoveDistance; y <= unitY + maxMoveDistance; y++) {
                //verify x, y
                if (x < 0 || y < 0 || x >= grid.GetWidth() || y >= grid.GetHeight())
                    continue;

                if (gridPathfinding.IsWalkable(x, y)) {
                    if (gridPathfinding.HasPath(unitX, unitY, x, y)) {
                        if (gridPathfinding.GetPath(unitX, unitY, x, y).Count <= maxMoveDistance) {
                            if (grid.GetGridObject(x, y).GetUnitGridCombat() == null)
                            {
                                // Set Tilemap Tile to Move
                                GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetTilemapSprite(
                                    x, y, MovementTilemap.TilemapObject.TilemapSprite.Move);

                                grid.GetGridObject(x, y).SetIsValidMovePosition(true);
                                possibleMoves.Add(new Vector2Int(x, y));
                            }
                        } 
                    }
                }
            }
        }
        return possibleMoves.Count > 0;
    }

    private void Update() {
        switch (state)
        {
            case State.WaitingForMove:
                if (unitGridCombat.GetTeam() == UnitGridCombat.Team.Blue)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();

                        grid.GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);
                        GridObject gridObject = grid.GetGridObject(x, y);

                        if (gridObject.GetIsValidMovePosition())
                        {
                            state = State.Busy;

                            grid.GetGridObject(unitGridCombat.GetPosition()).ClearUnitGridCombat();
                            gridObject.SetUnitGridCombat(unitGridCombat);

                            Vector3 targetPos = new Vector3(x + .5f, y + .5f) * grid.GetCellSize();
                            unitGridCombat.MoveTo(targetPos, () => {
                                if (state != State.GameEnded)
                                {
                                    state = State.WaitingForMove;
                                    Process();
                                }
                            });
                        }
                    }
                }
                else
                {
                    state = State.Busy;
                    FunctionTimer.Create(() =>
                    {
                        var randomMove = possibleMoves[Random.Range(0, possibleMoves.Count)];

                        Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
                        GridObject gridObject = grid.GetGridObject(randomMove.x, randomMove.y);
                        grid.GetGridObject(unitGridCombat.GetPosition()).ClearUnitGridCombat();
                        gridObject.SetUnitGridCombat(unitGridCombat);

                        Vector3 targetPos = new Vector3(randomMove.x + .5f, randomMove.y + .5f) * grid.GetCellSize();
                        unitGridCombat.MoveTo(targetPos, () => {
                            if (state != State.GameEnded)
                            {
                                state = State.WaitingForMove;
                                Process();
                            }
                        });
                    }, .6f);
                }
                break;
            case State.WaitingForAttack:
                if (unitGridCombat.GetTeam() == UnitGridCombat.Team.Blue)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
                        GridObject gridObject = grid.GetGridObject(UtilsClass.GetMouseWorldPosition());

                        if (gridObject.GetIsValidMovePosition())
                        {
                            state = State.Busy;

                            unitGridCombat.AttackUnit(gridObject.GetUnitGridCombat(), () => {
                                if (state != State.GameEnded)
                                {
                                    state = State.WaitingForAttack;
                                    Process();
                                }
                            });
                        }
                    }
                }
                else
                {
                    state = State.Busy;
                    FunctionTimer.Create(() =>
                    {
                        var randomMove = possibleMoves[Random.Range(0, possibleMoves.Count)];
                        Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
                        GridObject gridObject = grid.GetGridObject(randomMove.x, randomMove.y);
                        unitGridCombat.AttackUnit(gridObject.GetUnitGridCombat(), () => {
                            if (state != State.GameEnded)
                            {
                                state = State.WaitingForAttack;
                                Process();
                            }
                        });
                    }, .6f);
                }
                break;
        }
    }

    private void Process()
    {
        //move -> attack -> turnover + move -> attack 
        if (state == State.WaitingForMove)
        {
            state = State.WaitingForAttack;
            bool moveAvailable = UpdateValidAttackPositions();
            if (!moveAvailable)
            {
                Process();
            }
        }
        else if (state == State.WaitingForAttack)
        {
            TurnOver();
            state = State.WaitingForMove;
            bool moveAvailable = UpdateValidMovePositions();
            if (!moveAvailable)
            {
                Process();
            }
        }
    }

    private void TurnOver() {
        SelectNextActiveUnit();
    }



    public class GridObject {

        private Grid<GridObject> grid;
        private int x;
        private int y;
        private bool isValidMovePosition;
        private IUnitGridCombat unitGridCombat;

        public GridObject(Grid<GridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetIsValidMovePosition(bool set) {
            isValidMovePosition = set;
        }

        public bool GetIsValidMovePosition() {
            return isValidMovePosition;
        }

        public void SetUnitGridCombat(IUnitGridCombat unitGridCombat) {
            this.unitGridCombat = unitGridCombat;
        }

        public void ClearUnitGridCombat() {
            SetUnitGridCombat(null);
        }

        public IUnitGridCombat GetUnitGridCombat() {
            return unitGridCombat;
        }

    }

}
