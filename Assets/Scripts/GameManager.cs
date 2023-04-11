using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerMovementController player;
    private int lives = 3;
    private float respawnTime = 3.0f;

    public void playerDestroyed()
    {
        this.lives--;
        if(this.lives == 0)
        {
            //End
        }

        Invoke("RespawnPlayer", this.respawnTime);
    }

    private void Respawn()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.SetActive(true);
    }
}
