using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
   public Transform currentEnemy;

   [SerializeField] protected float attackCooldown = 1f;
   protected float LastTimeAttacked;
   
   [Header("Tower Setup")]
   [SerializeField] protected Transform towerHead;

   [SerializeField] protected float rotationSpeed = 10f;
   [SerializeField] protected float attackRange = 2.5f;
   [SerializeField] protected LayerMask whatIsEnemy;

   private bool _canRotate;

   protected virtual void Update()
   {
      if (currentEnemy == null)
      {
         currentEnemy = FindRandomEnemyWithinRange();
         return;
      }

      if (CanAttack())
      {
         Attack();
      }

      if (Vector3.Distance(currentEnemy.position, transform.position) > attackRange)
      {
         currentEnemy = null;
      }
      
      RotateTowardsEnemy();
   }

   protected virtual void Awake()
   {
      
   }

   protected virtual void Attack()
   {
      Debug.Log("Attacking");
   }

   public void EnableRotation(bool enable)
   {
      _canRotate = enable;
   }

   protected bool CanAttack()
   {
      if (currentEnemy == null)
      {
         return false;
      }
      
      if (Time.time > LastTimeAttacked + attackCooldown)
      {
         LastTimeAttacked = Time.time;
         return true;
      }
      
      return false;
   }

   protected Transform FindRandomEnemyWithinRange()
   {
      List<Enemy> enemiesInRange = new List<Enemy>();
      Enemy fastestEnemy = null;
      Collider[] enemiesAround = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);
      foreach (Collider enemy in enemiesAround)
      {
         Enemy newEnemyComponent = enemy.GetComponent<Enemy>();
         enemiesInRange.Add(newEnemyComponent);
      }

      if (enemiesInRange.Count <= 0)
      {
         return null;
      }
      
      fastestEnemy = GetMostAdvancedEnemy(enemiesInRange);

      if (fastestEnemy != null)
      {
         return fastestEnemy.transform;
      }

      return null;
   }

   private Enemy GetMostAdvancedEnemy(List<Enemy> targets)
   {
      float fastestEnemyDistance = float.MaxValue;
      Enemy fastestEnemy = null;
      
      foreach (Enemy enemy in targets)
      {
         float currentDistance = enemy.DistanceToFinishLine();
         if (currentDistance < fastestEnemyDistance)
         {
            fastestEnemyDistance = currentDistance;
            fastestEnemy = enemy;
         }
      }

      return fastestEnemy;
   }

   protected virtual void RotateTowardsEnemy()
   {
      if (!_canRotate)
      {
         return;
      }
      
      if (currentEnemy == null)
      {
         return;
      }
      
      Vector3 directionToEnemy = currentEnemy.position - towerHead.position;

      Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
      
      Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime).eulerAngles;
      towerHead.rotation = Quaternion.Euler(rotation);
   }

   protected Vector3 DirectionToEnemy(Transform startPoint)
   {
      return (currentEnemy.position - startPoint.position).normalized;
   }

   protected virtual void OnDrawGizmos()
   {
      Gizmos.DrawWireSphere(transform.position, attackRange);
   }
}
