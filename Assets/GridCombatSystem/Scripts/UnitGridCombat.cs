using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using TMPro;

public interface IUnitGridCombat
{
    void AttackUnit(IUnitGridCombat unitGridCombat, Action onAttackComplete);
    //bool CanAttackUnit(IUnitGridCombat unitGridCombat);
    void Damage(IUnitGridCombat attacker, int damageAmount);
    bool IsDead();
    void MoveTo(Vector3 targetPosition, Action onReachedPosition);
    Vector3 GetPosition();
    UnitGridCombat.Team GetTeam();
    bool IsEnemy(IUnitGridCombat unitGridCombat);
    void SetHeroUnitGridInfo(HeroUnitGridInfo heroUnitGridInfo);
    HeroUnitGridInfo GetHeroUnitGridInfo();
    void SetOnDeadCallback(Action onDeadCallback);
    void SetSelectedVisible(bool visible);
}

public class UnitGridCombat : MonoBehaviour, IUnitGridCombat {

    [SerializeField] protected Team team;

    protected Character_Base characterBase;
    protected HealthSystem healthSystem;
    protected GameObject selectedGameObject;
    protected IMovePosition movePosition;
    protected State state;
    //protected World_Bar healthBar;
    protected TextMeshPro textMeshPro;
    protected Transform barTransform;

    protected HeroUnitGridInfo heroUnitGridInfo;

    public enum Team {
        Blue,
        Red
    }

    protected enum State {
        Normal,
        Moving,
        Attacking
    }

    protected void Awake() {
        movePosition = GetComponent<IMovePosition>();
        InitializeBase();

        Transform barInstanceTransform = Instantiate(GameAssets.i.pfBar, transform, false);
        barInstanceTransform.localPosition = new Vector3(0, 10);
        barTransform = barInstanceTransform.Find("Bar");
        textMeshPro = barInstanceTransform.Find("Text").GetComponent<TextMeshPro>();
        Color32 barColor = team == Team.Blue ? new Color32(15, 203, 37, 255) : new Color32(255, 0, 0, 255);
        barTransform.Find("BarIn").GetComponent<SpriteRenderer>().color = barColor;

        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
        state = State.Normal;
    }

    protected virtual void InitializeBase()
    {
        characterBase = GetComponent<Character_Base>();

        //Transform barInstanceTransform = Instantiate(GameAssets.i.pfBar, transform, false);
        //barInstanceTransform.localPosition = new Vector3(0, 10);
        //barTransform = barInstanceTransform.Find("Bar");
        //textMeshPro = barInstanceTransform.Find("Text").GetComponent<TextMeshPro>();
        //Color barColor = team == Team.Blue ? Color.green : Color.red;
        //barTransform.Find("BarIn").GetComponent<SpriteRenderer>().color = barColor;
        //healthBar = new World_Bar(transform, new Vector3(0, 10), new Vector3(10, 1.3f), Color.grey, barColor, 1f, 10000, new World_Bar.Outline { color = Color.black, size = .5f });
    }

    protected void HealthSystem_OnHealthChanged(object sender, EventArgs e) {
        UpdateVisuals();
        //healthBar.SetSize(healthSystem.GetHealthNormalized());
    }

    protected void UpdateVisuals()
    {
        textMeshPro.SetText(healthSystem.GetHealth().ToString());
        barTransform.localScale = new Vector3(healthSystem.GetHealthNormalized(), barTransform.localScale.y);
    }

    protected void Update() {
        switch (state) {
            case State.Normal:
                break;
            case State.Moving:
                break;
            case State.Attacking:
                break;
        }
    }

    public void SetSelectedVisible(bool visible) {
        selectedGameObject.SetActive(visible);
    }

    public void MoveTo(Vector3 targetPosition, Action onReachedPosition) {
        state = State.Moving;
        movePosition.SetMovePosition(targetPosition + new Vector3(1, 1), () =>
        {
            state = State.Normal;
            onReachedPosition();
        });
        //movePosition.SetMovePosition(targetPosition, () =>
        //{
        //    state = State.Normal;
        //    onReachedPosition();
        //});
    }

    //public bool CanAttackUnit(IUnitGridCombat unitGridCombat) {
    //    return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) < 50f;
    //}

    public virtual void AttackUnit(IUnitGridCombat unitGridCombat, Action onAttackComplete) {
        state = State.Attacking;

        ShootUnit(unitGridCombat, () => {
            if (!unitGridCombat.IsDead()) {
                ShootUnit(unitGridCombat, () => {
                    if (!unitGridCombat.IsDead()) {
                        ShootUnit(unitGridCombat, () => {
                            if (!unitGridCombat.IsDead()) {
                                ShootUnit(unitGridCombat, () => {
                                    state = State.Normal;
                                    onAttackComplete();
                                });
                            } else { state = State.Normal; onAttackComplete(); }
                        });
                    } else { state = State.Normal; onAttackComplete(); }
                });
            } else { state = State.Normal; onAttackComplete(); }
        });
    }

    private void ShootUnit(IUnitGridCombat unitGridCombat, Action onShootComplete) {
        SFXMusic.Instance.PlayShootSound();

        GetComponent<IMoveVelocity>().Disable();
        Vector3 attackDir = (unitGridCombat.GetPosition() - transform.position).normalized;

        UtilsClass.ShakeCamera(.6f, .1f);
        //GameHandler_GridCombatSystem.Instance.ScreenShake();
        //UtilsClass.ShakeCamera(1f, .2f);
        characterBase.PlayShootAnimation(attackDir, (Vector3 vec) => {
            //Shoot_Flash.AddFlash(vec);
            WeaponTracer.Create(vec, unitGridCombat.GetPosition() + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(-2f, 4f));
            if (heroUnitGridInfo == null)
            {
                unitGridCombat.Damage(this, UnityEngine.Random.Range(8, 16));
            }
            else
            {
                unitGridCombat.Damage(this, UnityEngine.Random.Range(heroUnitGridInfo.attackDamage - 2, heroUnitGridInfo.attackDamage + 3));
            }
        }, () => {
            characterBase.PlayIdleAnim();
            GetComponent<IMoveVelocity>().Enable();

            onShootComplete();
        });
    }

    public virtual void Damage(IUnitGridCombat attacker, int damageAmount) {
        Vector3 bloodDir = (GetPosition() - attacker.GetPosition()).normalized;
        Blood_Handler.SpawnBlood(GetPosition(), bloodDir);

        DamagePopup.Create(GetPosition(), damageAmount, false);
        if (healthSystem == null)
        {
            healthSystem = new HealthSystem(100);
        }
        healthSystem.Damage(damageAmount);
        if (healthSystem.IsDead()) {
            onDeadCallback?.Invoke();
            FlyingBody.Create(GameAssets.i.pfEnemyFlyingBody, GetPosition(), bloodDir);
            Destroy(gameObject);
        } else {
            // Knockback
            //transform.position += bloodDir * 5f;
        }
    }

    protected Action onDeadCallback;

    public void SetOnDeadCallback(Action onDeadCallback)
    {
        this.onDeadCallback = onDeadCallback;
    }

    public bool IsDead() {
        return healthSystem.IsDead();
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public Team GetTeam() {
        return team;
    }

    public bool IsEnemy(IUnitGridCombat unitGridCombat) {
        return unitGridCombat.GetTeam() != team;
    }

    public void SetHeroUnitGridInfo(HeroUnitGridInfo heroUnitGridInfo)
    {
        this.heroUnitGridInfo = heroUnitGridInfo;
        healthSystem = new HealthSystem(heroUnitGridInfo.healthAmount);
        UpdateVisuals();
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    public HeroUnitGridInfo GetHeroUnitGridInfo()
    {
        return heroUnitGridInfo;
    }
}
