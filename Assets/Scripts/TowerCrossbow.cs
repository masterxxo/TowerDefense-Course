using UnityEngine;

public class TowerCrossbow : Tower
{
    private CrossbowVisuals visuals;
    
    [Header("Crossbow Details")] 
    [SerializeField] private Transform gunPoint;

    protected override void Awake()
    {
        base.Awake();
        visuals = GetComponent<CrossbowVisuals>();
    }
    
    protected override void Attack()
    {
        Vector3 directionToEnemy = DirectionToEnemy(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity))
        {
            towerHead.forward = directionToEnemy;
            Debug.Log(hitInfo.collider.gameObject.name + " was attacked!");
            Debug.DrawLine(gunPoint.position, hitInfo.point, Color.red);
            visuals.PlayAttackVFX(gunPoint.position, hitInfo.point);
        }
    }
}
