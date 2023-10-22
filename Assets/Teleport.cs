using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Collidable
{
    public float destinationX;
    public float destinationY;
    public AudioSource warpSound;

    void Update()
    {
        if(CollidingWith(player.gameObject.transform)){
            warpSound.Play();
            player.TeleportTo(destinationX, destinationY);
        }
    }
}
