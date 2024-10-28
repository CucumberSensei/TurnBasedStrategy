using System;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ShootAction.OnAnyShoot += OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        SwordAction.OnAnySwordHit += SwordAction_OnAnySwordHit;
    }

    private void SwordAction_OnAnySwordHit(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(2f);
    }

    private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(5f);
    }

    private void OnAnyShoot(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake();
    }
}
