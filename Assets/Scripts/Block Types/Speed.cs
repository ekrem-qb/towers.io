using UnityEngine;
public class Speed : MonoBehaviour
{
    Player player;
    public float speedValue;
    public float rotateSpeed;
    void Start()
    {
        player = this.GetComponentInParent<Player>();
        if (player)
        {
            player.AddSpeed(speedValue);
        }
    }
    void Update()
    {
        if (player && player.joystick)
        {
            float joystickDelta = Mathf.Atan2(player.joystick.Horizontal, player.joystick.Vertical) * Mathf.Rad2Deg;

            if (joystickDelta != 0)
            {
                this.transform.localEulerAngles = new Vector3(0, Mathf.LerpAngle(this.transform.localEulerAngles.y, -45 + joystickDelta, Time.deltaTime * rotateSpeed), 0);
            }
        }
    }
}