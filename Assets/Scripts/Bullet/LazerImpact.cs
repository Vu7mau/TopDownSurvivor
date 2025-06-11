using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerImpact : BulletImpact
{
    protected override void HandleSoundEffect()
    {
      SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.lazerExplosion,this.transform);
    }
}
