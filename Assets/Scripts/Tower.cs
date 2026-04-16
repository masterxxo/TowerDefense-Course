using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
   public Enemy currentEnemy;

   [SerializeField] protected float attackCooldown = 1f;
   protected float LastTimeAttacked;

   [Header("Tower Setup")]
   [SerializeField] protected EnemyType enemyPriorityType = EnemyType.None;
   [SerializeField] protected Transform towerHead;

   [SerializeField] protected float rotationSpeed = 10f;
   [SerializeField] protected float attackRange = 2.5f;
   [SerializeField] protected LayerMask whatIsEnemy;

   [Space]
   [Tooltip("Enabling ths allows tower to change target between attacks")]
   [SerializeField] private bool dynamicTargetChange = false;
   private float _targetCheckInterval = .1f;
   private float _lastTimeChecked;

   private bool _canRotate;

   protected virtual void Update()
   {
      UpdateTargetIfNeeded();
      if (currentEnemy == null)
      {
         currentEnemy = FindEnemyWithinRange();
         return;
      }

      if (CanAttack())
      {
         Attack();
      }

      if (Vector3.Distance(currentEnemy.GetCenterPoint(), transform.position) > attackRange)
      {
         currentEnemy = null;
      }
      
      RotateTowardsEnemy();
   }

   protected virtual void Awake()
   {
      EnableRotation(true);
   }

   private void UpdateTargetIfNeeded()
   {
      if (!dynamicTargetChange)
      {
         return;
      }

      if (Time.time > _lastTimeChecked + _targetCheckInterval)
      {
         _lastTimeChecked = Time.time;
         currentEnemy = FindEnemyWithinRange();
      }
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

   protected Enemy FindEnemyWithinRange()
   {
      List<Enemy> priorityTargets = new List<Enemy>();
      List<Enemy> enemiesInRange = new List<Enemy>();
      Collider[] enemiesAround = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);
      foreach (Collider enemy in enemiesAround)
      {
         Enemy newEnemyComponent = enemy.GetComponent<Enemy>();
         EnemyType newEnemyType = newEnemyComponent.GetEnemyType();
         if (newEnemyType == enemyPriorityType)
         {
            priorityTargets.Add(newEnemyComponent);
         }
         else
         {
            enemiesInRange.Add(newEnemyComponent);
         }
         
      }
      
      
      if (priorityTargets.Count > 0)
      {
         return GetMostAdvancedEnemy(priorityTargets);
      }

      if (enemiesInRange.Count > 0)
      {
         return GetMostAdvancedEnemy(enemiesInRange);
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

      Vector3 directionToEnemy = DirectionToEnemy(towerHead);

      Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
      
      Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime).eulerAngles;
      towerHead.rotation = Quaternion.Euler(rotation);
   }

   protected Vector3 DirectionToEnemy(Transform startPoint)
   {
      return (currentEnemy.GetCenterPoint() - startPoint.position).normalized;
   }

   protected virtual void OnDrawGizmos()
   {
      Gizmos.DrawWireSphere(transform.position, attackRange);
   }
}
