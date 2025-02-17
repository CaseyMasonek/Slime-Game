using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class Die : MonoBehaviour, IDieController
{
    [SerializeField] float gracePeriod = 0;
    
    private Health Health => GetComponent<Health>();

    public void OnDie()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnTakeDamage()
    {
        StartCoroutine(GracePeriod());
    }

    private IEnumerator GracePeriod()
    {
        Health.isInvincible = true;
        yield return new WaitForSeconds(gracePeriod);
        Health.isInvincible = false;
    }
}
