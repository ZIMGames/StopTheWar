using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using TMPro;

public class UnitGridCombatBayraktar : UnitGridCombat
{
    [SerializeField] private Animation[] animArray;
    [SerializeField] private Transform muzzleDummy;

    protected override void InitializeBase()
    {
        //Transform barInstanceTransform = Instantiate(GameAssets.i.pfBar, transform, false);
        //barInstanceTransform.localPosition = new Vector3(0, 10);
        //barTransform = barInstanceTransform.Find("Bar");
        //textMeshPro = barInstanceTransform.Find("Text").GetComponent<TextMeshPro>();
        //Color barColor = team == Team.Blue ? Color.green : Color.red;
        //barTransform.Find("BarIn").GetComponent<SpriteRenderer>().color = barColor;

        //Color barColor = team == Team.Blue ? Color.green : Color.red;
        //healthBar = new World_Bar(transform, new Vector3(0, 10), new Vector3(10, 1.3f), Color.grey, barColor, 1f, 10000, new World_Bar.Outline { color = Color.black, size = .5f });
    }

    public override void AttackUnit(IUnitGridCombat unitGridCombat, Action onAttackComplete)
    {
        Vector3 attackDir = (unitGridCombat.GetPosition() - transform.position).normalized;
        muzzleDummy.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(attackDir));

        state = State.Attacking;

        ShootUnit(unitGridCombat, () => {
            if (!unitGridCombat.IsDead())
            {
                ShootUnit(unitGridCombat, () => {
                    state = State.Normal;
                    onAttackComplete();
                });
            }
            else { state = State.Normal; onAttackComplete(); }
        });
    }

    private void ShootUnit(IUnitGridCombat unitGridCombat, Action onShootComplete)
    {
        SFXMusic.Instance.PlayRocketBarrage();

        foreach (var anim in animArray)
            anim.gameObject.SetActive(true);
        Vector3 attackDir = (unitGridCombat.GetPosition() - transform.position).normalized;

        foreach (var anim in animArray)
            anim.Play();

        unitGridCombat.Damage(this, UnityEngine.Random.Range(heroUnitGridInfo.attackDamage - 2, heroUnitGridInfo.attackDamage + 3));
        FunctionTimer.Create(() =>
        {
            //unitGridCombat.Damage(this, UnityEngine.Random.Range(heroUnitGridInfo.attackDamage - 2, heroUnitGridInfo.attackDamage + 3));
            FunctionTimer.Create(() =>
            {
                foreach (var anim in animArray)
                    anim.gameObject.SetActive(false);
                onShootComplete();
            }, animArray[0].clip.length / 2 + 0.5f);
        }, animArray[0].clip.length / 2);
        UtilsClass.ShakeCamera(.6f, .2f);
    }

    public override void Damage(IUnitGridCombat attacker, int damageAmount)
    {
        var attackerHeroType = attacker.GetHeroUnitGridInfo().heroType;
        if (attackerHeroType == Hero.HeroType.Yob || attackerHeroType == Hero.HeroType.Yob2 || attackerHeroType == Hero.HeroType.Gypsy)
        {
            damageAmount *= 2;
        }

        DamagePopup.Create(GetPosition(), damageAmount, false);
        healthSystem.Damage(damageAmount);
        if (healthSystem.IsDead())
        {
            onDeadCallback?.Invoke();
            Destroy(gameObject);
        }
    }
}
