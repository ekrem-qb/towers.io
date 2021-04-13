using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridManager : MonoBehaviour
{
    Pivot pivot;
    List<List<Block>> blockGrid = new List<List<Block>>();
    float sizeX;
    float sizeZ;
    int lastUsedRow = 0;
    int lastUsedColumn = 0;
    public List<Block> sameBlocks = new List<Block>();
    public List<Block> notSameBlocks = new List<Block>();
    public List<Block> sameFarBlocks = new List<Block>();
    void Awake()
    {
        pivot = this.GetComponent<Pivot>();
        Block originBlock = this.GetComponentInChildren<Block>();
        sizeX = originBlock.transform.localScale.x;
        sizeZ = originBlock.transform.localScale.z;
        blockGrid.Add(new List<Block>());
        blockGrid[0].Add(originBlock);
    }
    public void AddBlock(Block block)
    {
        block.transform.SetParent(this.transform);

        if (blockGrid.Count == blockGrid[blockGrid.Count - 1].Count)
        {
            if (blockGrid.Count > 1)
            {
                for (int z = 0; z < blockGrid.Count; z++)
                {
                    if (z == 0 && blockGrid[0].Count == blockGrid[blockGrid.Count - 1].Count)
                    {
                        blockGrid[0].Add(block);
                        lastUsedRow = 0;
                        block.SetTargetPosition(new Vector3((blockGrid[0].Count - 1) * sizeX, 0, 0));
                        break;
                    }
                    else
                    {
                        if (blockGrid[0].Count != blockGrid[z].Count)
                        {
                            blockGrid[z].Add(block);
                            lastUsedRow = z;
                            block.SetTargetPosition(new Vector3((blockGrid[z].Count - 1) * sizeX, 0, z * sizeZ));
                            break;
                        }
                    }
                }
            }
            else
            {
                blockGrid[0].Add(block);
                lastUsedRow = 0;
                block.SetTargetPosition(new Vector3((blockGrid[0].Count - 1) * sizeX, 0, 0));
            }

        }
        else if (blockGrid[0].Count == blockGrid[blockGrid.Count - 1].Count)
        {
            blockGrid.Add(new List<Block>());
            blockGrid[blockGrid.Count - 1].Add(block);
            lastUsedRow = blockGrid.Count - 1;
            block.SetTargetPosition(new Vector3(0, 0, (blockGrid.Count - 1) * sizeZ));
        }
        else if (blockGrid[0].Count == blockGrid[blockGrid.Count - 2].Count)
        {
            for (int x = 0; x < blockGrid[0].Count; x++)
            {
                if (x >= blockGrid[blockGrid.Count - 1].Count)
                {
                    blockGrid[blockGrid.Count - 1].Add(block);
                    lastUsedRow = blockGrid.Count - 1;
                    block.SetTargetPosition(new Vector3((blockGrid[blockGrid.Count - 1].Count - 1) * sizeX, 0, (blockGrid.Count - 1) * sizeZ));
                    break;
                }
            }
        }

        lastUsedColumn = blockGrid[lastUsedRow].IndexOf(block);

        sameBlocks.Clear();

        foreach (List<Block> checkBlockList in blockGrid)
        {
            foreach (Block checkBlock in checkBlockList)
            {
                if (checkBlock.type == block.type)
                {
                    if (checkBlock.scale == block.scale)
                    {
                        checkBlock.distanceToPivotBlock = Vector3.Distance(block.GetTargetPosition(), checkBlock.GetTargetPosition());
                        sameBlocks.Add(checkBlock);
                    }
                }
            }
        }
        if (sameBlocks.Count > 4)
        {
            sameBlocks.Sort(SortByDistance);
            sameBlocks.RemoveRange(4, sameBlocks.Count - 4);
        }

        if (sameBlocks.Count == 4)
        {
            foreach (Block originBlock in sameBlocks)
            {
                originBlock.nearSameBlocks.Clear();
                for (int x = originBlock.x - 1; x <= originBlock.x + 1; x++)
                {
                    for (int y = originBlock.y - 1; y <= originBlock.y + 1; y++)
                    {
                        if (x >= 0 && x < blockGrid.Count)
                        {
                            if (y >= 0 && y < blockGrid[x].Count && blockGrid[x].Count > 1)
                            {
                                if (blockGrid[x][y] != originBlock)
                                {
                                    if (blockGrid[x][y].type == originBlock.type && blockGrid[x][y].scale == originBlock.scale)
                                    {
                                        originBlock.nearSameBlocks.Add(blockGrid[x][y]);

                                        if (x == originBlock.x + 1)
                                        {
                                            if (y == originBlock.y + 1)
                                            {
                                                originBlock.topRight = blockGrid[x][y];
                                            }
                                            else if (y == originBlock.y)
                                            {
                                                originBlock.right = blockGrid[x][y];
                                            }
                                            else if (y == originBlock.y - 1)
                                            {
                                                originBlock.bottomRight = blockGrid[x][y];
                                            }
                                        }
                                        if (x == originBlock.x)
                                        {
                                            if (y == originBlock.y + 1)
                                            {
                                                originBlock.top = blockGrid[x][y];
                                            }
                                            else if (y == originBlock.y - 1)
                                            {
                                                originBlock.bottom = blockGrid[x][y];
                                            }
                                        }
                                        if (x == originBlock.x - 1)
                                        {
                                            if (y == originBlock.y + 1)
                                            {
                                                originBlock.topLeft = blockGrid[x][y];
                                            }
                                            else if (y == originBlock.y)
                                            {
                                                originBlock.left = blockGrid[x][y];
                                            }
                                            else if (y == originBlock.y - 1)
                                            {
                                                originBlock.bottomLeft = blockGrid[x][y];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Block bestBlock = null;
            int maxNearBlocks = 0;
            foreach (Block originBlock in sameBlocks)
            {
                if (originBlock.nearSameBlocks.Count > 1)
                {
                    if (Mathf.Abs(originBlock.nearSameBlocks[0].x - originBlock.nearSameBlocks[1].x) < 2
                     && Mathf.Abs(originBlock.nearSameBlocks[0].y - originBlock.nearSameBlocks[1].y) < 2)
                    {
                        bestBlock = originBlock;
                        break;
                    }
                    else if (originBlock.nearSameBlocks.Count > 2)
                    {
                        if (Mathf.Abs(originBlock.nearSameBlocks[0].x - originBlock.nearSameBlocks[2].x) < 2
                         && Mathf.Abs(originBlock.nearSameBlocks[0].y - originBlock.nearSameBlocks[2].y) < 2
                         || Mathf.Abs(originBlock.nearSameBlocks[1].x - originBlock.nearSameBlocks[2].x) < 2
                         && Mathf.Abs(originBlock.nearSameBlocks[1].y - originBlock.nearSameBlocks[2].y) < 2)
                        {
                            bestBlock = originBlock;
                            break;
                        }
                    }
                }
                if (originBlock.nearSameBlocks.Count > maxNearBlocks)
                {
                    maxNearBlocks = originBlock.nearSameBlocks.Count;
                    bestBlock = originBlock;
                }
            }

            if (bestBlock != null)
            {
                notSameBlocks.Clear();

                sameFarBlocks.Clear();
                sameFarBlocks.AddRange(sameBlocks);

                if ((bestBlock.bottom && bestBlock.bottomLeft) || (bestBlock.top && bestBlock.topLeft))
                {
                    if (bestBlock.bottom && bestBlock.bottomLeft)
                    {
                        sameFarBlocks.Remove(bestBlock.bottom);
                        sameFarBlocks.Remove(bestBlock.bottomLeft);
                    }
                    else if (bestBlock.top && bestBlock.topLeft)
                    {
                        sameFarBlocks.Remove(bestBlock.top);
                        sameFarBlocks.Remove(bestBlock.topLeft);
                    }
                    if (blockGrid[bestBlock.x - 1][bestBlock.y].scale == bestBlock.scale)
                    {
                        if (blockGrid[bestBlock.x - 1][bestBlock.y].type != bestBlock.type)
                        {
                            if (!notSameBlocks.Contains(blockGrid[bestBlock.x - 1][bestBlock.y]))
                            {
                                notSameBlocks.Add(blockGrid[bestBlock.x - 1][bestBlock.y]);
                            }
                        }
                    }
                    if (notSameBlocks.Count > 0)
                    {
                        goto foundNotSameBlocks;
                    }
                }
                if ((bestBlock.bottom && bestBlock.bottomRight) || (bestBlock.top && bestBlock.topRight))
                {
                    if (bestBlock.bottom && bestBlock.bottomRight)
                    {
                        sameFarBlocks.Remove(bestBlock.bottom);
                        sameFarBlocks.Remove(bestBlock.bottomRight);
                    }
                    else if (bestBlock.top && bestBlock.topRight)
                    {
                        sameFarBlocks.Remove(bestBlock.top);
                        sameFarBlocks.Remove(bestBlock.topRight);
                    }
                    if (bestBlock.y < blockGrid[bestBlock.x + 1].Count)
                    {
                        if (blockGrid[bestBlock.x + 1][bestBlock.y].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x + 1][bestBlock.y].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x + 1][bestBlock.y]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x + 1][bestBlock.y]);
                                }
                            }
                        }
                    }
                    if (notSameBlocks.Count > 0)
                    {
                        goto foundNotSameBlocks;
                    }
                }
                if ((bestBlock.left && bestBlock.bottomLeft) || (bestBlock.right && bestBlock.bottomRight))
                {
                    if (bestBlock.left && bestBlock.bottomLeft)
                    {
                        sameFarBlocks.Remove(bestBlock.left);
                        sameFarBlocks.Remove(bestBlock.bottomLeft);
                    }
                    else if (bestBlock.right && bestBlock.bottomRight)
                    {
                        sameFarBlocks.Remove(bestBlock.right);
                        sameFarBlocks.Remove(bestBlock.bottomRight);
                    }
                    if (blockGrid[bestBlock.x][bestBlock.y - 1].scale == bestBlock.scale)
                    {
                        if (blockGrid[bestBlock.x][bestBlock.y - 1].type != bestBlock.type)
                        {
                            if (!notSameBlocks.Contains(blockGrid[bestBlock.x][bestBlock.y - 1]))
                            {
                                notSameBlocks.Add(blockGrid[bestBlock.x][bestBlock.y - 1]);
                            }
                        }
                    }
                    if (notSameBlocks.Count > 0)
                    {
                        goto foundNotSameBlocks;
                    }
                }
                if ((bestBlock.left && bestBlock.topLeft) || (bestBlock.right && bestBlock.topRight))
                {
                    if (bestBlock.left && bestBlock.topLeft)
                    {
                        sameFarBlocks.Remove(bestBlock.left);
                        sameFarBlocks.Remove(bestBlock.topLeft);
                    }
                    else if (bestBlock.right && bestBlock.topRight)
                    {
                        sameFarBlocks.Remove(bestBlock.right);
                        sameFarBlocks.Remove(bestBlock.topRight);
                    }
                    if (bestBlock.y < blockGrid[bestBlock.x].Count - 1)
                    {
                        if (blockGrid[bestBlock.x][bestBlock.y + 1].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x][bestBlock.y + 1].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x][bestBlock.y + 1]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x][bestBlock.y + 1]);
                                }
                            }
                        }
                    }
                    if (notSameBlocks.Count > 0)
                    {
                        goto foundNotSameBlocks;
                    }
                }
                if (bestBlock.bottom && bestBlock.left)
                {
                    sameFarBlocks.Remove(bestBlock.bottom);
                    sameFarBlocks.Remove(bestBlock.left);
                    if (blockGrid[bestBlock.x - 1][bestBlock.y - 1].scale == bestBlock.scale)
                    {
                        if (blockGrid[bestBlock.x - 1][bestBlock.y - 1].type != bestBlock.type)
                        {
                            if (!notSameBlocks.Contains(blockGrid[bestBlock.x - 1][bestBlock.y - 1]))
                            {
                                notSameBlocks.Add(blockGrid[bestBlock.x - 1][bestBlock.y - 1]);
                            }
                        }
                    }
                    if (notSameBlocks.Count > 0)
                    {
                        goto foundNotSameBlocks;
                    }
                }
                if (bestBlock.bottom && bestBlock.right)
                {
                    sameFarBlocks.Remove(bestBlock.bottom);
                    sameFarBlocks.Remove(bestBlock.right);
                    if (blockGrid[bestBlock.x + 1][bestBlock.y - 1].scale == bestBlock.scale)
                    {
                        if (blockGrid[bestBlock.x + 1][bestBlock.y - 1].type != bestBlock.type)
                        {
                            if (!notSameBlocks.Contains(blockGrid[bestBlock.x + 1][bestBlock.y - 1]))
                            {
                                notSameBlocks.Add(blockGrid[bestBlock.x + 1][bestBlock.y - 1]);
                            }
                        }
                    }
                    if (notSameBlocks.Count > 0)
                    {
                        goto foundNotSameBlocks;
                    }
                }
                if (bestBlock.top && bestBlock.left)
                {
                    sameFarBlocks.Remove(bestBlock.top);
                    sameFarBlocks.Remove(bestBlock.left);
                    if (blockGrid[bestBlock.x - 1][bestBlock.y + 1].scale == bestBlock.scale)
                    {
                        if (blockGrid[bestBlock.x - 1][bestBlock.y + 1].type != bestBlock.type)
                        {
                            if (!notSameBlocks.Contains(blockGrid[bestBlock.x - 1][bestBlock.y + 1]))
                            {
                                notSameBlocks.Add(blockGrid[bestBlock.x - 1][bestBlock.y + 1]);
                            }
                        }
                    }
                    if (notSameBlocks.Count > 0)
                    {
                        goto foundNotSameBlocks;
                    }
                }
                if (bestBlock.top && bestBlock.right)
                {
                    sameFarBlocks.Remove(bestBlock.top);
                    sameFarBlocks.Remove(bestBlock.right);
                    if (bestBlock.y < blockGrid[bestBlock.x + 1].Count - 1)
                    {
                        if (blockGrid[bestBlock.x + 1][bestBlock.y + 1].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x + 1][bestBlock.y + 1].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x + 1][bestBlock.y + 1]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x + 1][bestBlock.y + 1]);
                                }
                            }
                        }
                    }
                    if (notSameBlocks.Count > 0)
                    {
                        goto foundNotSameBlocks;
                    }
                }
                if (bestBlock.bottomLeft || bestBlock.bottomRight)
                {
                    if (blockGrid[bestBlock.x][bestBlock.y - 1].scale == bestBlock.scale)
                    {
                        if (blockGrid[bestBlock.x][bestBlock.y - 1].type != bestBlock.type)
                        {
                            if (!notSameBlocks.Contains(blockGrid[bestBlock.x][bestBlock.y - 1]))
                            {
                                notSameBlocks.Add(blockGrid[bestBlock.x][bestBlock.y - 1]);
                            }
                        }
                    }
                    if (bestBlock.bottomLeft)
                    {
                        sameFarBlocks.Remove(bestBlock.bottomLeft);
                        if (blockGrid[bestBlock.x - 1][bestBlock.y].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x - 1][bestBlock.y].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x - 1][bestBlock.y]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x - 1][bestBlock.y]);
                                }
                            }
                        }
                    }
                    else if (bestBlock.bottomRight)
                    {
                        sameFarBlocks.Remove(bestBlock.bottomRight);
                        if (bestBlock.y < blockGrid[bestBlock.x + 1].Count)
                        {
                            if (blockGrid[bestBlock.x + 1][bestBlock.y].scale == bestBlock.scale)
                            {
                                if (blockGrid[bestBlock.x + 1][bestBlock.y].type != bestBlock.type)
                                {
                                    if (!notSameBlocks.Contains(blockGrid[bestBlock.x + 1][bestBlock.y]))
                                    {
                                        notSameBlocks.Add(blockGrid[bestBlock.x + 1][bestBlock.y]);
                                    }
                                }
                            }
                        }
                    }
                    if (notSameBlocks.Count > 0)
                    {
                        goto foundNotSameBlocks;
                    }
                }
                if (bestBlock.topLeft || bestBlock.topRight)
                {
                    if (bestBlock.y < blockGrid[bestBlock.x].Count - 1)
                    {
                        if (blockGrid[bestBlock.x][bestBlock.y + 1].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x][bestBlock.y + 1].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x][bestBlock.y + 1]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x][bestBlock.y + 1]);
                                }
                            }
                        }
                    }
                    if (bestBlock.topLeft)
                    {
                        sameFarBlocks.Remove(bestBlock.topLeft);
                        if (blockGrid[bestBlock.x - 1][bestBlock.y].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x - 1][bestBlock.y].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x - 1][bestBlock.y]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x - 1][bestBlock.y]);
                                }
                            }
                        }
                    }
                    else if (bestBlock.topRight)
                    {
                        sameFarBlocks.Remove(bestBlock.topRight);
                        if (blockGrid[bestBlock.x + 1][bestBlock.y].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x + 1][bestBlock.y].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x + 1][bestBlock.y]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x + 1][bestBlock.y]);
                                }
                            }
                        }
                    }
                    if (notSameBlocks.Count > 0)
                    {
                        goto foundNotSameBlocks;
                    }
                }
                if (bestBlock.bottom)
                {
                    sameFarBlocks.Remove(bestBlock.bottom);
                    if (bestBlock.x > 0)
                    {
                        if (blockGrid[bestBlock.x - 1][bestBlock.y - 1].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x - 1][bestBlock.y - 1].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x - 1][bestBlock.y - 1]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x - 1][bestBlock.y - 1]);
                                }
                            }
                        }
                        if (blockGrid[bestBlock.x - 1][bestBlock.y].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x - 1][bestBlock.y].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x - 1][bestBlock.y]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x - 1][bestBlock.y]);
                                }
                            }
                        }
                    }
                    else if (bestBlock.x < blockGrid.Count - 1)
                    {
                        if (blockGrid[bestBlock.x + 1][bestBlock.y - 1].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x + 1][bestBlock.y - 1].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x + 1][bestBlock.y - 1]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x + 1][bestBlock.y - 1]);
                                }
                            }
                        }
                        if (bestBlock.y < blockGrid[bestBlock.x + 1].Count)
                        {
                            if (blockGrid[bestBlock.x + 1][bestBlock.y].scale == bestBlock.scale)
                            {
                                if (blockGrid[bestBlock.x + 1][bestBlock.y].type != bestBlock.type)
                                {
                                    if (!notSameBlocks.Contains(blockGrid[bestBlock.x + 1][bestBlock.y]))
                                    {
                                        notSameBlocks.Add(blockGrid[bestBlock.x + 1][bestBlock.y]);
                                    }
                                }
                            }
                        }
                    }
                    if (notSameBlocks.Count > 0)
                    {
                        goto foundNotSameBlocks;
                    }
                }
                if (bestBlock.top)
                {
                    sameFarBlocks.Remove(bestBlock.top);
                    if (bestBlock.x > 0)
                    {
                        if (blockGrid[bestBlock.x - 1][bestBlock.y + 1].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x - 1][bestBlock.y + 1].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x - 1][bestBlock.y + 1]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x - 1][bestBlock.y + 1]);
                                }
                            }
                        }
                        if (blockGrid[bestBlock.x - 1][bestBlock.y].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x - 1][bestBlock.y].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x - 1][bestBlock.y]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x - 1][bestBlock.y]);
                                }
                            }
                        }
                    }
                    else if (bestBlock.x < blockGrid.Count - 1)
                    {
                        if (blockGrid[bestBlock.x + 1][bestBlock.y + 1].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x + 1][bestBlock.y + 1].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x + 1][bestBlock.y + 1]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x + 1][bestBlock.y + 1]);
                                }
                            }
                        }
                        if (blockGrid[bestBlock.x + 1][bestBlock.y].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x + 1][bestBlock.y].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x + 1][bestBlock.y]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x + 1][bestBlock.y]);
                                }
                            }
                        }
                    }
                    if (notSameBlocks.Count > 0)
                    {
                        goto foundNotSameBlocks;
                    }
                }
                if (bestBlock.left)
                {
                    sameFarBlocks.Remove(bestBlock.left);
                    if (bestBlock.y > 0)
                    {
                        if (blockGrid[bestBlock.x - 1][bestBlock.y - 1].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x - 1][bestBlock.y - 1].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x - 1][bestBlock.y - 1]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x - 1][bestBlock.y - 1]);
                                }
                            }
                        }
                        if (blockGrid[bestBlock.x][bestBlock.y - 1].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x][bestBlock.y - 1].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x][bestBlock.y - 1]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x][bestBlock.y - 1]);
                                }
                            }
                        }
                    }
                    else if (bestBlock.y < blockGrid[bestBlock.x].Count - 1)
                    {
                        if (blockGrid[bestBlock.x - 1][bestBlock.y + 1].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x - 1][bestBlock.y + 1].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x - 1][bestBlock.y + 1]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x - 1][bestBlock.y + 1]);
                                }
                            }
                        }
                        if (blockGrid[bestBlock.x][bestBlock.y + 1].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x][bestBlock.y + 1].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x][bestBlock.y + 1]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x][bestBlock.y + 1]);
                                }
                            }
                        }
                    }
                    if (notSameBlocks.Count > 0)
                    {
                        goto foundNotSameBlocks;
                    }
                }
                if (bestBlock.right)
                {
                    sameFarBlocks.Remove(bestBlock.right);
                    if (bestBlock.y > 0)
                    {
                        if (blockGrid[bestBlock.x + 1][bestBlock.y - 1].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x + 1][bestBlock.y - 1].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x + 1][bestBlock.y - 1]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x + 1][bestBlock.y - 1]);
                                }
                            }
                        }
                        if (blockGrid[bestBlock.x][bestBlock.y - 1].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x][bestBlock.y - 1].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x][bestBlock.y - 1]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x][bestBlock.y - 1]);
                                }
                            }
                        }
                    }
                    else if (bestBlock.y < blockGrid[bestBlock.x].Count - 1)
                    {
                        if (blockGrid[bestBlock.x + 1][bestBlock.y + 1].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x + 1][bestBlock.y + 1].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x + 1][bestBlock.y + 1]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x + 1][bestBlock.y + 1]);
                                }
                            }
                        }
                        if (blockGrid[bestBlock.x][bestBlock.y + 1].scale == bestBlock.scale)
                        {
                            if (blockGrid[bestBlock.x][bestBlock.y + 1].type != bestBlock.type)
                            {
                                if (!notSameBlocks.Contains(blockGrid[bestBlock.x][bestBlock.y + 1]))
                                {
                                    notSameBlocks.Add(blockGrid[bestBlock.x][bestBlock.y + 1]);
                                }
                            }
                        }
                    }
                }

            foundNotSameBlocks:

                sameFarBlocks.Remove(bestBlock);
                bestBlock = null;

                if (notSameBlocks.Count == sameFarBlocks.Count)
                {
                    for (int i = 0; i < sameFarBlocks.Count; i++)
                    {
                        SwapBlocks(sameFarBlocks[i], notSameBlocks[i]);
                    }
                }

                int max_x = 0;
                int max_y = 0;
                Block mergePivotBlock = null;

                foreach (Block checkBlock in sameBlocks)
                {
                    if (checkBlock.x > 0 && checkBlock.y > 0)
                    {
                        if (checkBlock.x > max_x || checkBlock.y > max_y)
                        {
                            max_x = checkBlock.x;
                            max_y = checkBlock.y;
                            mergePivotBlock = checkBlock;
                        }
                    }
                }
                if (mergePivotBlock != null)
                {
                    if (mergePivotBlock.type == blockGrid[mergePivotBlock.x][mergePivotBlock.y - 1].type
                     && mergePivotBlock.scale == blockGrid[mergePivotBlock.x][mergePivotBlock.y - 1].scale)
                    {
                        if (mergePivotBlock.type == blockGrid[mergePivotBlock.x - 1][mergePivotBlock.y].type
                         && mergePivotBlock.scale == blockGrid[mergePivotBlock.x - 1][mergePivotBlock.y].scale)
                        {
                            if (mergePivotBlock.type == blockGrid[mergePivotBlock.x - 1][mergePivotBlock.y - 1].type
                             && mergePivotBlock.scale == blockGrid[mergePivotBlock.x - 1][mergePivotBlock.y - 1].scale)
                            {
                                StartCoroutine(Merge(mergePivotBlock,
                                                     blockGrid[mergePivotBlock.x][mergePivotBlock.y - 1],
                                                     blockGrid[mergePivotBlock.x - 1][mergePivotBlock.y],
                                                     blockGrid[mergePivotBlock.x - 1][mergePivotBlock.y - 1]));

                                blockGrid[mergePivotBlock.x][mergePivotBlock.y - 1] = mergePivotBlock;
                                blockGrid[mergePivotBlock.x - 1][mergePivotBlock.y] = mergePivotBlock;
                                blockGrid[mergePivotBlock.x - 1][mergePivotBlock.y - 1] = mergePivotBlock;
                            }
                        }
                    }
                }
            }
        }

        if (pivot)
        {
            pivot.CenterCamera();
        }
    }
    int SortByDistance(Block a, Block b)
    {
        if (a.distanceToPivotBlock < b.distanceToPivotBlock)
        {
            return -1;
        }
        else if (a.distanceToPivotBlock > b.distanceToPivotBlock)
        {
            return 1;
        }
        return 0;
    }
    void SwapBlocks(Block blockA, Block blockB)
    {
        int x_A = blockA.x;
        int y_A = blockA.y;
        Vector3 pos_A = blockA.GetTargetPosition();

        int x_B = blockB.x;
        int y_B = blockB.y;
        Vector3 pos_B = blockB.GetTargetPosition();

        blockGrid[x_A][y_A] = blockB;
        blockB.SetTargetPosition(pos_A);

        blockGrid[x_B][y_B] = blockA;
        blockA.SetTargetPosition(pos_B);
    }
    IEnumerator Merge(Block blockPivot, Block blockOnLeft, Block blockOnTop, Block blockOnLeftTop)
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(blockOnLeft.gameObject);
        Destroy(blockOnTop.gameObject);
        Destroy(blockOnLeftTop.gameObject);

        blockPivot.Merge();
    }
}