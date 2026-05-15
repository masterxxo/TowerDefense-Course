using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _maxHP;
    [SerializeField] private int _currentHP;
    
    private UI_InGame _uiInGame;

    private void Awake()
    {
        _uiInGame = FindAnyObjectByType<UI_InGame>();
    }

    private void Start()
    {
        _currentHP = _maxHP;
        _uiInGame.UpdateHealthPointsUI(_currentHP, _maxHP);
    }

    public void UpdateHp(int value)
    {
       _currentHP += value; 
       _uiInGame.UpdateHealthPointsUI(_currentHP, _maxHP);
    }
}
