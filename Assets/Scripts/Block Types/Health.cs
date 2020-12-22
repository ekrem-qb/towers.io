using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Health : MonoBehaviour
{
    Player player;
    public float healthValue;
    public float rotateSpeed;
    void OnEnable()
    {
        player = this.GetComponentInParent<Player>();
        if (player)
        {
            InvokeRepeating("RestoreHealth", 0, 0.5f);
        }
    }
    void Update()
    {
        this.transform.localEulerAngles = new Vector3(0, this.transform.localEulerAngles.y + 1, 0);
    }
    void RestoreHealth()
    {
        player.AddHealth(healthValue);
    }
}