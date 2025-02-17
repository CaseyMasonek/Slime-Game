using System;
using JetBrains.Annotations;

interface IDieController
{
    public void OnDie();
    [CanBeNull] public void OnTakeDamage();
}
