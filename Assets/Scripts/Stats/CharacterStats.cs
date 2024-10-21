
using System.Collections;
using UnityEngine;
public enum StatType
{
    strength,
    agility,
    intelegence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightingDamage
}
public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;
    [Header("Major Stats")]
    public Stat strength; // 1 point increase damage by 1 and crit.power by 1%
    public Stat agility; // 1 point increase evasion by 1% and crit.power by 1%
    public Stat intelligence; //1 point increase magic damage by 1 and magic resistance by 3%
    public Stat vitality; // 1 point  increase health by 5 points
    

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower; // default value 150%
    

    [Header("defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;


    [SerializeField] private float ailmentDuration = 4;
    public bool isIgnited; //does damage over time
    public bool isChilled; // reduce armor by 20%
    public bool isShocked; // reduce accuracy by 20%

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;




    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;
    //new
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;

    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead {get; private set;}
    public bool isInvincible {get; private set;}
    public bool isVulnerable {get; private set;}
    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth=GetMaxHealthValue();
        fx=GetComponent<EntityFX>();
        
    }
    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;


        if (ignitedTimer < 0)
            isIgnited = false;
        if (chilledTimer < 0)
            isChilled = false;
        if (shockedTimer < 0)
            isShocked = false;
        if(isIgnited)
            ApplyIgniteDamage();
    }

    public void MakeVulnerablefor(float _duration) => StartCoroutine(VulnerableForCoroutine(_duration));
    
    private IEnumerator VulnerableForCoroutine(float _duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;

    }
    public virtual void IncreaseStatBy(int _modifier,float _duration, Stat _statsToModify)
    {
        StartCoroutine(StatModSCoroutine(_modifier,_duration, _statsToModify));
    }

    private IEnumerator StatModSCoroutine(int _modifier,float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);
        _statToModify.RemoveModifier(_modifier);
    }


    public virtual void DoDamage(CharacterStats _targetStats)
    {
        bool critialStrike = false;
        if(_targetStats.isInvincible)
            return;
        if (TargetCanAvoidAttack(_targetStats))
        {
            _targetStats.fx.CreatePupUpText("Avoided",Color.yellow);
            return;
        }
        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);
        int totalDamage = damage.GetValue() + strength.GetValue();
        if(CanCrit())
        {
            totalDamage = CalculateCriticalDammage(totalDamage);
            critialStrike = true;

        }
        fx.CreateHitFX(_targetStats.transform,critialStrike);

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        //call take damage of _targetStat Note (Dont have take damage of this object)
        _targetStats.TakeDamage(totalDamage);
        //if inventory current weapon has fire effect
        DoMagicalDamage(_targetStats);// remove if you dont want to apply magic hit on primary attack

    }
    #region Magical Damage And ailments
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();


        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        AttemptyToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightingDamage);

    }

    private void AttemptyToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        Entity entity= _targetStats.GetEntityFromCharacterStats(_targetStats);
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;
        //random property(Ignite, chill, Shock) when it equal to
        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (UnityEngine.Random.value < .3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .4f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

        }
        //can Ignited apply
        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .5f));
        

        _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilment(bool _ignite, bool _chilled, bool _shock)
    {
        bool canApplyIgnite =!isIgnited && !isChilled && !isShocked;
        bool canApplyChill =!isIgnited && !isChilled && !isShocked;
        bool canApplyShock =!isIgnited && !isChilled;

        if(_ignite && canApplyIgnite)
        {
            isIgnited=_ignite;
            ignitedTimer = ailmentDuration;
            fx.IgniteFxFor(ailmentDuration);
        }
        if(_chilled && canApplyChill)
        {
            isChilled=_chilled;
            chilledTimer = ailmentDuration;
            float slowPercenttage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercenttage,ailmentDuration);
            fx.ChillFxFor(ailmentDuration);
        }
        if(_shock && canApplyShock)
        {
            if(!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;
                //find closest target, only among the enemies
                //instatnitate thunder strike
                // setup thunder strike
                HitNearestTargetWithShockStrike();

            }
        }
    }

    public void ApplyShock(bool _shock)
    {
        if(isShocked)
            return;
        isShocked = _shock;
        shockedTimer = ailmentDuration;
        fx.ShockFxFor(ailmentDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }
    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);
            if (currentHealth < 0 && !isDead)
                Die();
            igniteDamageTimer = igniteDamageCooldown;
        }
    }
    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;
    #endregion
    public virtual void TakeDamage(int _damage)
    {
        if(isInvincible)
            return;
        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine(fx.FlashFX());

        if(currentHealth<=0 && !isDead)
            Die();
        
    }
    
    public virtual void IncreaseHealthBy(int _amount)
    {
        if(currentHealth < GetMaxHealthValue())
            if(currentHealth + _amount >GetMaxHealthValue())
                PlayerManager.instance.player.fx.CreatePupUpText("Heal: +"+(GetMaxHealthValue()-currentHealth).ToString(),Color.green);
        else
            PlayerManager.instance.player.fx.CreatePupUpText("Heal: +"+_amount,Color.green);
        currentHealth += _amount;
        
        if(currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();
        
        if(onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        if(isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.1f);
        
       
        currentHealth -= _damage;
        if(_damage > 0)
            fx.CreatePupUpText("-"+_damage.ToString(),Color.red);
        if(onHealthChanged != null)
            onHealthChanged();
    }
    protected virtual void Die()
    {
        isDead =true;
    }
    #region Stat calculations        
    public void KillEntity()
    {
        if(!isDead)
            Die();
    }
    public void MakeInvincible(bool _invivible) => isInvincible=_invivible;
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if(_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    // armar magic
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        if(totalMagicalDamage <= 0)
            Debug.Log("RESIST MAGIC DAMAGE");
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public virtual void OnEvasion()
    {
        
    }
    
    protected  bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        Entity entity = GetEntityFromCharacterStats(_targetStats);


        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (isShocked)
            totalEvasion += 20;
        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }
        return false;
    }

    

    protected bool CanCrit()
    {
        int totalCritcalChance = critChance.GetValue() + agility.GetValue();
        if(Random.Range(0,100) <= totalCritcalChance)
            return true;
        return false;
    }
    protected int CalculateCriticalDammage(int _damage)
    {
        float totalCritPower =(critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCritPower;
        return Mathf.RoundToInt(critDamage);
    }
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    #endregion


    private Entity GetEntityFromCharacterStats(CharacterStats _targetStats)
    {
        Entity entity = _targetStats.GetComponent<Enemy>();
        if (entity == null)
            entity = _targetStats.GetComponent<Player>();
        return entity;
    }

    public Stat GetStat(StatType _statType)
    {
        if(_statType == StatType.strength) return strength;
        else if(_statType == StatType.agility) return agility;
        else if(_statType == StatType.intelegence) return intelligence;
        else if(_statType == StatType.vitality) return vitality;
        else if(_statType == StatType.damage) return damage;
        else if(_statType == StatType.critChance) return critChance;
        else if(_statType == StatType.critPower) return critPower;
        else if(_statType == StatType.health) return maxHealth;
        else if(_statType == StatType.armor) return armor;
        else if(_statType == StatType.evasion) return evasion;
        else if(_statType == StatType.magicRes) return magicResistance;
        else if(_statType == StatType.fireDamage) return fireDamage;
        else if(_statType == StatType.iceDamage) return iceDamage;
        else if(_statType == StatType.lightingDamage) return lightingDamage;

        return null;
    }

}

