using UnityEngine;

public class TowerCrossbow : Tower
{
    private CrossbowVisuals _visuals;
    
    [Header("Crossbow Details")] 
    [SerializeField] private Transform gunPoint;
    [SerializeField] private int damage;

    protected override void Awake()
    {
        base.Awake();
        _visuals = GetComponent<CrossbowVisuals>();
    }
    
    protected override void Attack()
    {
        Vector3 directionToEnemy = DirectionToEnemy(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity))
        {
            towerHead.forward = directionToEnemy;

            Enemy enemyTarget = null;
            IDamagable damagable = hitInfo.collider.GetComponent<IDamagable>();
            
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
                enemyTarget = currentEnemy;
            }

            _visuals.PlayAttackVFX(gunPoint.position, hitInfo.point, enemyTarget);
            _visuals.PlayReloadVFX(attackCooldown);
        }
    }
}
