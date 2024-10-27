using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringerTriggers : Enemy_AnimationTriggers
{
    private Enemy_DeathBringer enemy_DeathBringer => GetComponentInParent<Enemy_DeathBringer>();
    private void Relocate() => enemy_DeathBringer.FindPosition();
    private void MakeInvisible() => enemy_DeathBringer.fx.MakeTransprent(true);
    private void MakeVisible() => enemy_DeathBringer.fx.MakeTransprent(false);

}
 