using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Mirror;

public class DefaultBullet : MonoBehaviour
{
    // Turret variable
    [HideInInspector] public Turret turret;
    [HideInInspector] public Cosmetic.Bullet bullet;

    // Bullet variables
    [HideInInspector] public float damage;
    [HideInInspector] public float speed;
    [HideInInspector] public int pierces;
    [HideInInspector] public float time;

    // Bullet movement variables
    [HideInInspector] public BaseEntity target;
    [HideInInspector] public bool tracking = false;
    [HideInInspector] public bool recycling = false;

    // Renderering variables
    [HideInInspector] public SpriteRenderer model;
    [HideInInspector] public TrailRenderer trail;

    // Ignore list
    [HideInInspector]
    public List<BaseEntity> ignoreList = new List<BaseEntity>();

    // Setup bullet
    public virtual void Setup(Turret turret, Cosmetic.Bullet bullet = null)
    {
        // Set turret SO
        this.turret = turret;
        this.bullet = bullet;

        // Apply research to variables
        damage = turret.damage * Research.damageBoost;
        pierces = turret.bulletPierces + Research.pierceBoost;

        // Set speed (and randomize if applicable)
        if (turret.randomizeSpeed) speed = Random.Range(turret.bulletSpeed - 5, turret.bulletSpeed + 5);
        else speed = turret.bulletSpeed;

        // Set bullet lifetime
        time = turret.bulletTime;

        // Get trail renderer component
        trail = GetComponent<TrailRenderer>();
        if (trail != null) trail.material = turret.material;
    }

    // Setup model
    public virtual void SetupModel(Sprite model = null)
    {
        // If model is not null, set it
        if (model != null)
        {
            this.model = GetComponent<SpriteRenderer>();
            this.model.sprite = model;
            this.model.material = turret.material;
        }
    }

    // Setup model
    public virtual void SetupModel(Cosmetic.Bullet cosmeticBullet)
    {
        // Get trail renderer component
        if (trail != null) trail.material = cosmeticBullet.material;

        // If model is not null, set it
        if (model != null && cosmeticBullet.applyModelToBullet)
        {
            model = GetComponent<SpriteRenderer>();
            model.sprite = cosmeticBullet.model;
            if (cosmeticBullet.applyMaterialToModel)
                model.material = turret.material;
        }
    }

    // Move bullet
    public virtual void Move()
    {
        if (tracking && target != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
        }
        else transform.position += transform.up * speed * Time.deltaTime;
    }

    // Hold info
    public virtual void DestroyBullet(Material material, BaseEntity entity = null)
    {
        if (entity != null)
            material = entity.GetMaterial();

        recycling = true;
        CreateParticle(material);
        Recycler.AddRecyclable(transform);
    }

    // Creates a particle and sets the material
    public void CreateParticle(Material material)
    {
        ParticleSystemRenderer holder = Instantiate(turret.bulletParticle, transform.position,
                transform.rotation).GetComponent<ParticleSystemRenderer>();
        holder.transform.rotation *= Quaternion.Euler(0, 0, 180f);
        holder.material = material;
        holder.trailMaterial = material;
    }

    // If a collision is detected, apply damage
    public virtual void OnCollision(BaseEntity entity)
    {
        if (!ignoreList.Contains(entity))
        {
            entity.DamageEntity(damage);

            pierces -= 1;
            if (pierces <= 0)
            {
                if (bullet != null) DestroyBullet(bullet.material, entity);
                else DestroyBullet(turret.material, entity);
            }
            else ignoreList.Add(entity);

            tracking = false;
        }
    }
}
