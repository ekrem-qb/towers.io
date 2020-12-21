using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Attack : MonoBehaviour
{
    Player player;
    public float rotateSpeed;
    List<Block> nearBlocks = new List<Block>();
    public Block nearestBlock;
    void Start()
    {
        player = this.GetComponentInParent<Player>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (player)
        {
            Block block = other.GetComponent<Block>();
            if (block)
            {
                if (!block.transform.IsChildOf(this.transform.parent.parent))
                {
                    if (block.isConnected)
                    {
                        nearBlocks.Add(block);
                    }
                }
            }
        }
    }
    void Update()
    {
        foreach (Block block in nearBlocks)
        {
            if (nearestBlock)
            {
                if (block)
                {
                    if (Vector3.Distance(this.transform.position, block.transform.position) < Vector3.Distance(this.transform.position, nearestBlock.transform.position))
                    {
                        nearestBlock = block;
                    }
                }
            }
            else
            {
                nearestBlock = block;
            }
        }
        if (nearestBlock)
        {
            this.transform.LookAt(nearestBlock.transform);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (player)
        {
            Block block = other.GetComponent<Block>();
            if (block)
            {
                if (!block.transform.IsChildOf(this.transform.parent.parent))
                {
                    if (block.isConnected)
                    {
                        nearBlocks.Remove(block);
                    }
                }
            }
        }
    }
}