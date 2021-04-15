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
        this.transform.localEulerAngles = new Vector3(0, this.transform.localEulerAngles.y + (Time.deltaTime * rotateSpeed * 100), 0);
    }
    void RestoreHealth()
    {
        if (player)
        {
            player.AddHealth(healthValue);
        }
    }
}