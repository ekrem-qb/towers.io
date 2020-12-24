using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Attack : MonoBehaviour
{
    Player player;
    public float rotateSpeed;
    List<Block> nearBlocks = new List<Block>();
    Block nearestBlock;
    public ParticleSystem particles;
    void OnEnable()
    {
        player = this.GetComponentInParent<Player>();
    }
    void OnDisable()
    {
        particles.Stop();
    }
    void OnTriggerEnter(Collider other)
    {
        if (player)
        {
            Block block = other.GetComponent<Block>();
            if (block)
            {
                if (block.player != this.player)
                {
                    if (block.player)
                    {
                        nearBlocks.Add(block);
                    }
                }
            }
        }
    }
    void Update()
    {
        if (nearBlocks.Count > 0)
        {
            foreach (Block block in nearBlocks)
            {
                if (nearestBlock)
                {
                    if (block)
                    {
                        if (Vector3.Distance(this.transform.position, block.transform.position) <= Vector3.Distance(this.transform.position, nearestBlock.transform.position))
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
                if (nearestBlock.player)
                {
                    if (nearestBlock.player != this.player)
                    {
                        this.transform.LookAt(nearestBlock.transform);
                        if (!particles.isPlaying)
                        {
                            particles.Play();
                        }
                        Player enemy = nearestBlock.GetComponentInParent<Player>();
                        if (enemy)
                        {
                            enemy.AddHealth(-0.1f);
                        }
                    }
                }
                else
                {
                    particles.Stop();
                }
            }
            else
            {
                particles.Stop();
            }
        }
        else
        {
            particles.Stop();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (player)
        {
            Block block = other.GetComponent<Block>();
            if (block)
            {
                if (block.player)
                {
                    if (block.player != this.player)
                    {
                        nearBlocks.Remove(block);
                    }
                }
            }
        }
    }
}