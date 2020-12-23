using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Zone : MonoBehaviour
{
    List<Block> blocksOutside = new List<Block>();
    void OnTriggerEnter(Collider other)
    {
        Block block = other.GetComponentInParent<Block>();
        if (block)
        {
            blocksOutside.Remove(block);
        }
    }
    void Update()
    {
        Vector3 prevScale = this.transform.localScale;
        this.transform.localScale = new Vector3(prevScale.x - 0.1f, prevScale.y, prevScale.z - 0.1f);

        foreach (Block block in blocksOutside)
        {
            if (block)
            {
                if (block.isConnected)
                {
                    Player player = block.GetComponentInParent<Player>();
                    if (player)
                    {
                        player.AddHealth(-0.1f);
                    }
                }
                else
                {
                    Destroy(block.gameObject);
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        Block block = other.GetComponentInParent<Block>();
        if (block)
        {
            blocksOutside.Add(block);
        }
    }
}
