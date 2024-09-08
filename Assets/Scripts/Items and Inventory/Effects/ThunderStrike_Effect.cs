using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike efftect", menuName = "Data/Item effect/Thunder strike")]

public class ThunderStrike_Effect : Item_Effect
{
    [SerializeField] private GameObject thunderStrikePrefab;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);

        Destroy(newThunderStrike,1f);
    }
}
