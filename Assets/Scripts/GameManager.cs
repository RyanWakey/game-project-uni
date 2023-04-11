using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerMovementController player;
    [SerializeField] private int lives = 3;
    private float respawnTime = 3.0f;
    public static GameManager instance {  get; private set; }

    public void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
        {
            instance = this;
        }
    }

    public void playerDestroyed()
    {
        this.lives--;

        if(this.lives == 0)
        {
            //End
        } else
        {
            Invoke("RespawnPlayer", this.respawnTime);
        }

       
    }

    private void RespawnPlayer()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.SetActive(true);
        this.player.EnableInvulnerability();
    }
}
