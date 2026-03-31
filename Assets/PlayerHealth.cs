using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour

{
    public float MaxHealth = 100;
    private float Health;
    private bool canReceiveDamage = true;
    public float invicibilityTimer = 2;

    public delegate void HealthChangedHandler(float newhealth, float amountChanged);
    public event HealthChangedHandler OnHealthChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddDamage(float damage)
    {
        if (canReceiveDamage)
        {
            Health -= damage;
            OnHealthChanged?.Invoke(Health,-damage);
            canReceiveDamage = false;
            StartCoroutine(InvincibilityTimer(invicibilityTimer, ResetInvincibility));

        }

        Debug.Log(Health);
    }

    IEnumerator InvincibilityTimer(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback.Invoke();
    }

    private void ResetInvincibility()
    {
        canReceiveDamage = true;
    }


    public void AddHealth(float addhealth)
    {
        Health += addhealth;
        OnHealthChanged?.Invoke(Health, addhealth);
        Debug.Log(Health);
    }
}
