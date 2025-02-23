using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class Die : MonoBehaviour, IDieController
{
    public static Vector3 Checkpoint = Vector3.zero;
    
    [SerializeField] float gracePeriod = 0;
    
    private Health Health => GetComponent<Health>();

    public void SetCheckpoint(Vector3 checkpoint)
    {
        Checkpoint = checkpoint;
    }

    private void Awake()
    {
        transform.position = Checkpoint;
    }

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
