using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Die : IDieController
{
    public static Vector3 Checkpoint;

    public event Action OnDie;

    private void Enable()
    {
        OnDie += DieAction;
    }

    private void Disable()
    {
        OnDie -= DieAction;
    }
    
    public void DieAction()
    {
        SceneManager.SetActiveScene(SceneManager.GetActiveScene());
    }
}
