using System.Collections.Generic;
using UnityEngine;
public class Pivot : MonoBehaviour
{
    Player player;
    GridManager gridManager;
    List<List<Block>> grid;
    Vector3 targetPos;
    float connectionSpeed;
    void Awake()
    {
        player = this.GetComponentInParent<Player>();
        gridManager = this.GetComponent<GridManager>();
        grid = gridManager.blockGrid;
        connectionSpeed = grid[0][0].connectionSpeed;
    }
    void Update()
    {
        if (this.transform.localPosition != targetPos)
        {
            this.transform.localPosition = Vector3.SlerpUnclamped(this.transform.localPosition, targetPos, Time.deltaTime * connectionSpeed);
        }
    }
    public void CenterCamera()
    {
        float sizeX = 0;
        float sizeY = 0;

        foreach (List<Block> blockList in grid)
        {
            sizeX += blockList[0].targetScale.x / blockList[0].scale;
        }
        foreach (Block block in grid[0])
        {
            sizeY += block.targetScale.z / block.scale;
        }

        Vector2 center = new Vector2(sizeX / 2, sizeY / 2);

        targetPos = new Vector3((-center.y + 2.5f), 0, (-center.x + 2.5f));

        if (player.joystick)
        {
            float distance = (sizeX + sizeY) * 2;

            CameraController cameraController = Camera.main.GetComponent<CameraController>();
            cameraController.distance = 80 + distance;
        }
    }
}