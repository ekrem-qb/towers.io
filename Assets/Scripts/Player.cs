using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public FixedJoystick joystick;
    public Text healthMeter;
    public Text speedMeter;
    public Material material;
    float health = 100;
    float maxHealth = 100;
    float speed = 1;
    float minSpeed = 0.25f;
    void Update()
    {
        if (joystick)
        {
            this.transform.Translate(new Vector3(speed, 0, speed) * joystick.Horizontal);
            this.transform.Translate(new Vector3(-speed, 0, speed) * joystick.Vertical);
        }
    }
    public void AddSpeed(float addValue)
    {
        if (speed > minSpeed || addValue > 0)
        {
            speed += addValue;
            if (speedMeter)
            {
                speedMeter.text = "⇉ " + speed;
            }
        }
    }
    public void AddHealth(float addValue)
    {
        if (health < maxHealth || addValue < 0)
        {
            health += addValue;
            if (healthMeter)
            {
                healthMeter.text = "❤ " + health;
            }
        }
        if (health < 1)
        {
            foreach (Block block in this.GetComponentsInChildren<Block>())
            {
                block.Disconnect();
            }
            Destroy(this.gameObject);
        }
    }
    public void AddMaxHealth(float addValue)
    {
        maxHealth += addValue;
    }
    // void OnMouseDrag()
    // {
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     RaycastHit hit;
    //     if (Physics.Raycast(ray, out hit))
    //     {
    //         this.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
    //     }
    // }
}