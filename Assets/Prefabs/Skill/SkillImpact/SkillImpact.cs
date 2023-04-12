using System;
using System.Collections.Generic;
using CartoonFX;
using UnityEngine;
using UnityEngine.Serialization;


[Serializable]
public enum EffectType
{
    Stun,
}



[Serializable]
public class StatScale
{
    public StatType statType;
    public float scaleFactor;
}



[Serializable]
public class SkillEffect
{
    public EffectType effectType;
    public float duration;



    public SkillEffect GetCopy()
    {
        SkillEffect copy = new SkillEffect
        {
            effectType = effectType,
            duration = duration
        };
        return copy;
    }
}



public class SkillImpact : MonoBehaviour,ICollider
{
    
    [HideInInspector] public ControllerBase creator;
    [SerializeField] private float startLifeTime;
    [SerializeField] private ParticleSystem collisionHitEffect;
    [SerializeField] private float baseDamage;
    [SerializeField] private float lifeTimeAfterHit;
    [SerializeField] public List<SkillEffect> effects;
    [SerializeField] public List<StatScale> statScales;




    [HideInInspector] public float scaledDamage;

    //can be null
    [SerializeField] private GameObject skillEffectContainer;
    
    protected Collider Collider;


    protected virtual void Start()
    {
        Collider = GetComponent<Collider>();
    }


    protected virtual void Update()
    {
        if (startLifeTime < 0)
        {
            Destroy(gameObject);
        }
        else
        {
            startLifeTime -= Time.deltaTime;
        }
    }

    public ColliderType GetColliderType()
    {
        return ColliderType.Impact;
    }


    public void ScaleDamage(CharacterStats characterStats)
    {

        scaledDamage = baseDamage;
        
        foreach (StatScale statScale in statScales)
        {
            switch (statScale.statType)
            {
                case StatType.Str:
                    scaledDamage += statScale.scaleFactor * characterStats.Str; 
                    break;
                
                case StatType.Int:
                    scaledDamage += statScale.scaleFactor * characterStats.Int; 
                    break;
                
                case StatType.Dex:
                    scaledDamage += statScale.scaleFactor * characterStats.Dex; 
                    break;
                
                default:
                    Debug.LogError("Unknown stattype");
                    break;
            }
            
            
        }
        
    }

    public void HitOccurred()
    {
        SelfDestroy();
    }

    
    protected void SelfDestroy()
    {
        if(skillEffectContainer)skillEffectContainer.gameObject.SetActive(false);
        if(collisionHitEffect) collisionHitEffect.gameObject.SetActive(true);
        HandleCameraShake();
        if(collisionHitEffect) collisionHitEffect.Play();
        startLifeTime = lifeTimeAfterHit;

        if (Collider)
        {
            Collider.enabled = false;
        }

        if (this is Projectile projectile)
        {
            projectile.projectileSpeed = 0;
        }

    }

    private void HandleCameraShake()
    {
        if(collisionHitEffect == null) return;
        
        if (collisionHitEffect.TryGetComponent(out CFXR_Effect effect))
        {
            if (creator is AIController)
            {
                effect.cameraShake.enabled = false;
            }
            else if(creator is PlayerControl)
            {
                effect.cameraShake.enabled = true;
            }
        }
    }
    
}
