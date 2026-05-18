using UnityEngine;

public class Castle : MonoBehaviour
{
    private GameManager _gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().DestroyEnemy();
            if(_gameManager == null)
                _gameManager = FindAnyObjectByType<GameManager>();
            
            if(_gameManager != null)
                _gameManager.UpdateHp(-1);
        }
    }
}
