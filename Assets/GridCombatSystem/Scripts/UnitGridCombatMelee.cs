using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGridCombatMelee : UnitGridCombat
{

    public override void AttackUnit(IUnitGridCombat unitGridCombat, Action onAttackComplete)
    {
        state = State.Attacking;

        PunchUnit(unitGridCombat, () => {
            if (!unitGridCombat.IsDead())
            {
                KickUnit(unitGridCombat, () => {
                    if (!unitGridCombat.IsDead())
                    {
                        PunchUnit(unitGridCombat, () => {
                            state = State.Normal;
                            onAttackComplete();
                        });
                    }
                    else { state = State.Normal; onAttackComplete(); }
                });
            }
            else { state = State.Normal; onAttackComplete(); }
        });
        //CodeMonkey.Utils.UtilsClass.ShakeCamera(.6f, .1f);
    }

    private void PunchUnit(IUnitGridCombat unitGridCombat, Action onPunchComplete)
    {
        SFXMusic.Instance.PlayHitSound();

        GetComponent<IMoveVelocity>().Disable();
        Vector3 attackDir = (unitGridCombat.GetPosition() - transform.position).normalized;

        characterBase.PlayPunchAnimation(attackDir, (Vector3 vec) =>
        {
            unitGridCombat.Damage(this, UnityEngine.Random.Range(heroUnitGridInfo.attackDamage - 2, heroUnitGridInfo.attackDamage + 3));
        }, () =>
        {
            GetComponent<IMoveVelocity>().Enable();
            characterBase.PlayIdleAnim();
            onPunchComplete?.Invoke();
        });
    }

    private void KickUnit(IUnitGridCombat unitGridCombat, Action onKickComplete)
    {
        SFXMusic.Instance.PlayHitSound();

        GetComponent<IMoveVelocity>().Disable();
        Vector3 attackDir = (unitGridCombat.GetPosition() - transform.position).normalized;

        characterBase.PlayKickAnimation(attackDir, (Vector3 vec) =>
        {
            unitGridCombat.Damage(this, UnityEngine.Random.Range(heroUnitGridInfo.attackDamage - 2, heroUnitGridInfo.attackDamage + 3));
        }, () =>
        {
            GetComponent<IMoveVelocity>().Enable();
            characterBase.PlayIdleAnim();
            onKickComplete?.Invoke();
        });
    }
}
