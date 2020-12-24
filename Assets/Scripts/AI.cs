using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AI : MonoBehaviour
{
    List<Block> nearBlocks = new List<Block>();
    List<Player> nearEnemies = new List<Player>();
    Block nearestBlock = null;
    Player nearestEnemy = null;
    Player player;
    CapsuleCollider coll;
    Quaternion targetRotation;
    void Awake()
    {
        player = this.GetComponent<Player>();
        coll = this.GetComponent<CapsuleCollider>();
        targetRotation = this.transform.rotation;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root != this.transform)
        {
            Block block = other.GetComponent<Block>();
            if (block)
            {
                if (block.player)
                {
                    if (block.player != this.player)
                    {
                        nearEnemies.Add(block.player);
                    }
                }
                else
                {
                    nearBlocks.Add(block);
                }
            }
        }
    }
    void Update()
    {
        if (nearestEnemy)
        {
            float health = this.player.GetMaxHealth();
            float enemyHealth = nearestEnemy.GetMaxHealth();


            if (enemyHealth < health)
            {
                // if (100 - ((enemyHealth / health) * 100) > 25)
                // {
                targetRotation = Quaternion.LookRotation(nearestEnemy.transform.position - transform.position);
                // }
            }
            else
            {
                // if (100 - ((health / enemyHealth) * 100) > 25)
                // {
                targetRotation = Quaternion.Inverse(Quaternion.LookRotation(nearestEnemy.transform.position - transform.position));
                // Vector3 angles = this.transform.eulerAngles;
                // angles.y += 180;
                // this.transform.eulerAngles = angles;
                // }
            }
        }
        else
        {
            if (nearestBlock)
            {
                if (nearestBlock.player)
                {
                    if (nearestBlock.player)
                    {
                        if (nearestBlock.player != this.player)
                        {
                            nearEnemies.Add(nearestBlock.player);
                        }
                    }
                    nearBlocks.Remove(nearestBlock);
                    nearestBlock = null;
                }
                else
                {
                    targetRotation = Quaternion.LookRotation(nearestBlock.transform.position - transform.position);
                }
            }
            else
            {
                // Vector3 randomPoint = new Vector3(Random.Range(0f, 250f), 0, Random.Range(0f, 250f));
                // targetRotation = Quaternion.LookRotation(randomPoint - transform.position);

                if (nearEnemies.Count > 0)
                {
                    FindNearestEnemy();
                }
                else
                {
                    FindNearestBlock();
                }
            }
        }
        RaycastHit hit;
        Vector3 laserPosA = this.transform.position + (this.transform.forward * 50);
        laserPosA.y += 800;
        Vector3 laserPosB = Vector3.down;
        if (Physics.Raycast(laserPosA, laserPosB, out hit))
        {
            if (hit.collider.CompareTag("Finish"))
            {
                Debug.DrawRay(laserPosA, laserPosB * hit.distance, Color.yellow);
            }
            else
            {
                Debug.DrawRay(laserPosA, laserPosB * hit.distance, Color.white);

                Vector3 angles = targetRotation.eulerAngles;
                angles.y++;
                targetRotation.eulerAngles = angles;
                // targetRotation = Quaternion.Inverse(targetRotation);
                // nearBlocks.Remove(nearestBlock);
                // nearestBlock = null;
                // nearEnemies.Remove(nearestEnemy);
                // nearestEnemy = null;
            }
        }
        else
        {
            Debug.DrawRay(laserPosA, laserPosB * 1000, Color.white);

            Vector3 angles = targetRotation.eulerAngles;
            angles.y++;
            targetRotation.eulerAngles = angles;
            // targetRotation = Quaternion.Inverse(targetRotation);
            // nearBlocks.Remove(nearestBlock);
            // nearestBlock = null;
            // nearEnemies.Remove(nearestEnemy);
            // nearestEnemy = null;
        }
        this.transform.rotation = Quaternion.SlerpUnclamped(this.transform.rotation, targetRotation, player.rotateSpeed * Time.deltaTime);
        this.transform.position += this.transform.forward;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.root != this.transform)
        {
            Block block = other.GetComponent<Block>();
            if (block)
            {
                if (block.player)
                {
                    if (block.player != this.player)
                    {
                        nearEnemies.Remove(block.player);
                        if (nearestEnemy == block.player)
                        {
                            nearestEnemy = null;
                        }
                    }
                }
                else
                {
                    nearBlocks.Remove(block);
                    if (nearestBlock == block)
                    {
                        nearestBlock = null;
                    }
                }
            }
        }
    }
    void FindNearestBlock()
    {
        foreach (Block block in nearBlocks)
        {
            if (block)
            {
                if (nearestBlock)
                {
                    if (Vector3.Distance(this.transform.position, block.transform.position) <= Vector3.Distance(this.transform.position, nearestBlock.transform.position))
                    {
                        nearestBlock = block;
                    }
                }
                else
                {
                    nearestBlock = block;
                }
            }
            else
            {
                StartCoroutine(RemoveBlockFromList(block));
            }
        }
    }
    void FindNearestEnemy()
    {
        foreach (Player enemy in nearEnemies)
        {
            if (enemy)
            {
                if (nearestEnemy)
                {
                    if (Vector3.Distance(this.transform.position, enemy.transform.position) <= Vector3.Distance(this.transform.position, nearestEnemy.transform.position))
                    {
                        nearestEnemy = enemy;
                    }
                }
                else
                {
                    nearestEnemy = enemy;
                }
            }
            else
            {
                StartCoroutine(RemoveEnemyFromList(enemy));
            }
        }
    }
    IEnumerator RemoveBlockFromList(Block block)
    {
        yield return new WaitForEndOfFrame();

        nearBlocks.Remove(block);
    }
    IEnumerator RemoveEnemyFromList(Player enemy)
    {
        yield return new WaitForEndOfFrame();

        nearEnemies.Remove(enemy);
    }
    public void ExpandRadius(float value)
    {
        if (coll)
        {
            coll.radius += value;
        }
    }
}