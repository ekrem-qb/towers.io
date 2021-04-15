using System.Collections.Generic;
using UnityEngine;
public class GridManager : MonoBehaviour
{
    Pivot pivot;
    public List<List<Block>> blockGrid = new List<List<Block>>();
    public List<Block> sameBlocks = new List<Block>();
    public List<Block> notSameBlocks = new List<Block>();
    public List<Block> sameFarBlocks = new List<Block>();
    public Block bestBlock;
    void Awake()
    {
        pivot = this.GetComponent<Pivot>();
        Block originBlock = this.GetComponentInChildren<Block>();
        blockGrid.Add(new List<Block>());
        blockGrid[0].Add(originBlock);
    }
    public void AddBlock(Block block)
    {
        block.transform.SetParent(this.transform);

        if (blockGrid[0].Count < 2)
        {
            blockGrid[0].Add(block);
            block.SetTargetPosition(new Vector2Int(0, 1));
        }
        else if (blockGrid[0].Count / blockGrid.Count == 2)
        {
            blockGrid.Add(new List<Block>());
            blockGrid[blockGrid.Count - 1].Add(block);
            block.SetTargetPosition(new Vector2Int(blockGrid.Count - 1, 0));
        }
        else if (blockGrid[0].Count > blockGrid[blockGrid.Count - 1].Count)
        {
            for (int x = 1; x < blockGrid.Count; x++)
            {
                if (blockGrid[x - 1].Count - blockGrid[x].Count >= 2)
                {
                    if (x < blockGrid.Count - 1)
                    {
                        if (x % 2 != 0)
                        {
                            blockGrid[x].Add(block);
                            block.SetTargetPosition(new Vector2Int(x, blockGrid[x].Count - 1));
                            break;
                        }
                        else if (blockGrid[x].Count - blockGrid[x + 1].Count == 1)
                        {
                            blockGrid[x + 1].Add(block);
                            block.SetTargetPosition(new Vector2Int(x + 1, blockGrid[x + 1].Count - 1));
                            break;
                        }
                        else
                        {
                            blockGrid[x].Add(block);
                            block.SetTargetPosition(new Vector2Int(x, blockGrid[x].Count - 1));
                            break;
                        }
                    }
                    else
                    {
                        if (x % 2 != 0)
                        {
                            blockGrid[x].Add(block);
                            block.SetTargetPosition(new Vector2Int(x, blockGrid[x].Count - 1));
                            break;
                        }
                        else
                        {
                            blockGrid.Add(new List<Block>());
                            blockGrid[blockGrid.Count - 1].Add(block);
                            block.SetTargetPosition(new Vector2Int(blockGrid.Count - 1, 0));
                            break;
                        }
                    }
                }
                else if ((blockGrid[0].Count - blockGrid[x].Count) % 2 != 0)
                {
                    if (blockGrid[x].Count % 2 != 0)
                    {
                        blockGrid[x].Add(block);
                        block.SetTargetPosition(new Vector2Int(x, blockGrid[x].Count - 1));
                        break;
                    }
                    else if (x % 2 != 0)
                    {
                        blockGrid[x].Add(block);
                        block.SetTargetPosition(new Vector2Int(x, blockGrid[x].Count - 1));
                        break;
                    }
                    else
                    {
                        blockGrid[x - 2].Add(block);
                        block.SetTargetPosition(new Vector2Int(x - 2, blockGrid[x - 2].Count - 1));
                        break;
                    }
                }
            }
        }
        else if (blockGrid.Count - blockGrid[0].Count == 2)
        {
            blockGrid[0].Add(block);
            block.SetTargetPosition(new Vector2Int(0, blockGrid[0].Count - 1));
        }
        else if (blockGrid[0].Count == blockGrid[blockGrid.Count - 1].Count)
        {
            blockGrid.Add(new List<Block>());
            blockGrid[blockGrid.Count - 1].Add(block);
            block.SetTargetPosition(new Vector2Int(blockGrid.Count - 1, 0));
        }

        bestBlock = null;
        sameBlocks.Clear();

        foreach (List<Block> checkBlockList in blockGrid)
        {
            foreach (Block checkBlock in checkBlockList)
            {
                if (checkBlock.type == block.type)
                {
                    if (checkBlock.scale == block.scale)
                    {
                        checkBlock.badNeighbours = 0;
                        checkBlock.goodNeighbours = 0;
                        checkBlock.top = null;
                        checkBlock.topLeft = null;
                        checkBlock.left = null;
                        checkBlock.bottomLeft = null;
                        checkBlock.topRight = null;
                        checkBlock.right = null;
                        checkBlock.bottomRight = null;
                        checkBlock.bottom = null;
                        for (int x = checkBlock.x - 1; x <= checkBlock.x + 1; x++)
                        {
                            for (int y = checkBlock.y - 1; y <= checkBlock.y + 1; y++)
                            {
                                if (x >= 0 && x < blockGrid.Count)
                                {
                                    if (y >= 0 && y < blockGrid[x].Count && blockGrid[x].Count > 1)
                                    {
                                        if (blockGrid[x][y] != checkBlock)
                                        {
                                            if (blockGrid[x][y].scale == checkBlock.scale)
                                            {
                                                checkBlock.goodNeighbours++;
                                                if (blockGrid[x][y].type == checkBlock.type)
                                                {
                                                    if (x == checkBlock.x + 1)
                                                    {
                                                        if (x % 2 != 0)
                                                        {
                                                            if (y == checkBlock.y - 1)
                                                            {
                                                                if (y % 2 == 0)
                                                                {
                                                                    checkBlock.topRight = blockGrid[x][y];
                                                                }
                                                            }
                                                            else if (y == checkBlock.y)
                                                            {
                                                                checkBlock.right = blockGrid[x][y];
                                                            }
                                                            else if (y == checkBlock.y + 1)
                                                            {
                                                                if (y % 2 != 0)
                                                                {
                                                                    checkBlock.bottomRight = blockGrid[x][y];
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (x == checkBlock.x)
                                                    {
                                                        if (y == checkBlock.y - 1)
                                                        {
                                                            if (y % 2 == 0)
                                                            {
                                                                checkBlock.top = blockGrid[x][y];
                                                            }
                                                        }
                                                        else if (y == checkBlock.y + 1)
                                                        {
                                                            if (y % 2 != 0)
                                                            {
                                                                checkBlock.bottom = blockGrid[x][y];
                                                            }
                                                        }
                                                    }
                                                    else if (x == checkBlock.x - 1)
                                                    {
                                                        if (x % 2 == 0)
                                                        {
                                                            if (y == checkBlock.y - 1)
                                                            {
                                                                if (y % 2 == 0)
                                                                {
                                                                    checkBlock.topLeft = blockGrid[x][y];
                                                                }
                                                            }
                                                            else if (y == checkBlock.y)
                                                            {
                                                                checkBlock.left = blockGrid[x][y];
                                                            }
                                                            else if (y == checkBlock.y + 1)
                                                            {
                                                                if (y % 2 != 0)
                                                                {
                                                                    if (blockGrid[x].Count == blockGrid[x + 1].Count)
                                                                    {
                                                                        checkBlock.bottomLeft = blockGrid[x][y];
                                                                    }

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                checkBlock.badNeighbours++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        checkBlock.badNeighbours++;
                                    }
                                }
                                else
                                {
                                    checkBlock.badNeighbours++;
                                }
                            }
                        }
                        sameBlocks.Add(checkBlock);
                    }
                }
            }
        }
        if (sameBlocks.Count >= 4)
        {
            sameBlocks.Sort(SortByNeighbours);
            bestBlock = sameBlocks[0];

            foreach (Block sameBlock in sameBlocks)
            {
                sameBlock.distanceToBestBlock = Vector2.Distance(sameBlock.GetTargetPosition(), bestBlock.GetTargetPosition());
            }

            if (sameBlocks.Count > 4)
            {
                sameBlocks.Sort(SortByDistance);
                sameBlocks.RemoveRange(4, sameBlocks.Count - 4);
            }

            if (sameBlocks.Count == 4)
            {
                if (bestBlock != null)
                {
                    notSameBlocks.Clear();

                anotherBestBlock:
                    sameFarBlocks.Clear();
                    sameFarBlocks.AddRange(sameBlocks);

                    if ((bestBlock.top && bestBlock.topLeft) || (bestBlock.bottom && bestBlock.bottomLeft))
                    {
                        if (bestBlock.top && bestBlock.topLeft)
                        {
                            sameFarBlocks.Remove(bestBlock.top);
                            sameFarBlocks.Remove(bestBlock.topLeft);
                        }
                        else if (bestBlock.bottom && bestBlock.bottomLeft)
                        {
                            sameFarBlocks.Remove(bestBlock.bottom);
                            sameFarBlocks.Remove(bestBlock.bottomLeft);
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
                    if ((bestBlock.top && bestBlock.topRight) || (bestBlock.bottom && bestBlock.bottomRight))
                    {
                        if (bestBlock.top && bestBlock.topRight)
                        {
                            sameFarBlocks.Remove(bestBlock.top);
                            sameFarBlocks.Remove(bestBlock.topRight);
                        }
                        else if (bestBlock.bottom && bestBlock.bottomRight)
                        {
                            sameFarBlocks.Remove(bestBlock.bottom);
                            sameFarBlocks.Remove(bestBlock.bottomRight);
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
                    if (bestBlock.top && bestBlock.left)
                    {
                        sameFarBlocks.Remove(bestBlock.top);
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
                    if (bestBlock.top && bestBlock.right)
                    {
                        sameFarBlocks.Remove(bestBlock.top);
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
                    if (bestBlock.bottom && bestBlock.left)
                    {
                        sameFarBlocks.Remove(bestBlock.bottom);
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
                    if (bestBlock.bottom && bestBlock.right)
                    {
                        sameFarBlocks.Remove(bestBlock.bottom);
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
                    if (bestBlock.topLeft || bestBlock.topRight)
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
                    if (bestBlock.bottomLeft || bestBlock.bottomRight)
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
                    if (bestBlock.top)
                    {
                        sameFarBlocks.Remove(bestBlock.top);
                        if (bestBlock.x > 0)
                        {
                            if (bestBlock.x % 2 != 0)
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
                            if (notSameBlocks.Count > 1)
                            {
                                goto foundNotSameBlocks;
                            }
                            else
                            {
                                notSameBlocks.Clear();
                            }
                        }
                        if (bestBlock.x < blockGrid.Count - 1)
                        {
                            if (bestBlock.x % 2 == 0)
                            {
                                if (bestBlock.y < blockGrid[bestBlock.x + 1].Count - 1)
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
                            if (notSameBlocks.Count > 1)
                            {
                                goto foundNotSameBlocks;
                            }
                            else
                            {
                                notSameBlocks.Clear();
                            }
                        }
                    }
                    if (bestBlock.bottom)
                    {
                        sameFarBlocks.Remove(bestBlock.bottom);
                        if (bestBlock.x > 0)
                        {
                            if (bestBlock.x % 2 != 0)
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
                            if (notSameBlocks.Count > 1)
                            {
                                goto foundNotSameBlocks;
                            }
                            else
                            {
                                notSameBlocks.Clear();
                            }
                        }
                        if (bestBlock.x < blockGrid.Count - 1)
                        {
                            if (bestBlock.x % 2 == 0)
                            {
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
                            if (notSameBlocks.Count > 1)
                            {
                                goto foundNotSameBlocks;
                            }
                            else
                            {
                                notSameBlocks.Clear();
                            }
                        }
                    }
                    if (bestBlock.left)
                    {
                        sameFarBlocks.Remove(bestBlock.left);
                        if (bestBlock.y > 0)
                        {
                            if (bestBlock.y % 2 != 0)
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
                            if (notSameBlocks.Count > 1)
                            {
                                goto foundNotSameBlocks;
                            }
                            else
                            {
                                notSameBlocks.Clear();
                            }
                        }
                        if (bestBlock.y < blockGrid[bestBlock.x].Count - 1)
                        {
                            if (bestBlock.y % 2 == 0)
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
                            if (notSameBlocks.Count > 1)
                            {
                                goto foundNotSameBlocks;
                            }
                            else
                            {
                                notSameBlocks.Clear();
                            }
                        }
                    }
                    if (bestBlock.right)
                    {
                        sameFarBlocks.Remove(bestBlock.right);
                        if (bestBlock.y > 0)
                        {
                            if (bestBlock.y % 2 != 0)
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
                            if (notSameBlocks.Count > 1)
                            {
                                goto foundNotSameBlocks;
                            }
                            else
                            {
                                notSameBlocks.Clear();
                            }
                        }
                        if (bestBlock.y < blockGrid[bestBlock.x + 1].Count - 1)
                        {
                            if (bestBlock.y % 2 == 0)
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
                            if (notSameBlocks.Count > 1)
                            {
                                goto foundNotSameBlocks;
                            }
                            else
                            {
                                notSameBlocks.Clear();
                            }
                        }
                    }
                    if (!bestBlock.top && !bestBlock.bottom && !bestBlock.left && !bestBlock.right && !bestBlock.topLeft && !bestBlock.bottomLeft && !bestBlock.topRight && !bestBlock.bottomRight)
                    {
                        if (bestBlock.x > 0)
                        {
                            if (bestBlock.y > 0)
                            {
                                if (bestBlock.y % 2 != 0)
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
                                }
                                if (bestBlock.x % 2 != 0)
                                {
                                    if (bestBlock.y % 2 != 0)
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
                                if (notSameBlocks.Count > 2)
                                {
                                    goto foundNotSameBlocks;
                                }
                                else
                                {
                                    notSameBlocks.Clear();
                                }
                            }
                            if (bestBlock.y < blockGrid[bestBlock.x].Count - 1)
                            {
                                if (bestBlock.y % 2 == 0)
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
                                if (bestBlock.x % 2 != 0)
                                {
                                    if (bestBlock.y % 2 == 0)
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
                                if (notSameBlocks.Count > 2)
                                {
                                    goto foundNotSameBlocks;
                                }
                                else
                                {
                                    notSameBlocks.Clear();
                                }
                            }
                        }
                        if (bestBlock.x < blockGrid.Count - 1)
                        {
                            if (bestBlock.y > 0)
                            {
                                if (bestBlock.y % 2 != 0)
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
                                }
                                if (bestBlock.x % 2 == 0)
                                {
                                    if (bestBlock.y < blockGrid[bestBlock.x + 1].Count - 1)
                                    {
                                        if (bestBlock.y % 2 != 0)
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
                                if (notSameBlocks.Count > 2)
                                {
                                    goto foundNotSameBlocks;
                                }
                                else
                                {
                                    notSameBlocks.Clear();
                                }
                            }
                            if (bestBlock.y < blockGrid[bestBlock.x].Count - 1)
                            {
                                if (bestBlock.y % 2 == 0)
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
                                if (bestBlock.x % 2 == 0)
                                {
                                    if (bestBlock.y < blockGrid[bestBlock.x + 1].Count - 1)
                                    {
                                        if (bestBlock.y % 2 == 0)
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
                                if (notSameBlocks.Count > 2)
                                {
                                    goto foundNotSameBlocks;
                                }
                                else
                                {
                                    notSameBlocks.Clear();
                                }
                            }
                        }
                    }

                    if (notSameBlocks.Count == 0)
                    {
                        if (sameBlocks.IndexOf(bestBlock) < sameBlocks.Count - 1)
                        {
                            bestBlock = sameBlocks[sameBlocks.IndexOf(bestBlock) + 1];
                            goto anotherBestBlock;
                        }
                    }

                foundNotSameBlocks:

                    sameFarBlocks.Remove(bestBlock);

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
                                    Merge(mergePivotBlock,
                                                         blockGrid[mergePivotBlock.x][mergePivotBlock.y - 1],
                                                         blockGrid[mergePivotBlock.x - 1][mergePivotBlock.y],
                                                         blockGrid[mergePivotBlock.x - 1][mergePivotBlock.y - 1]);

                                    blockGrid[mergePivotBlock.x][mergePivotBlock.y - 1] = mergePivotBlock;
                                    blockGrid[mergePivotBlock.x - 1][mergePivotBlock.y] = mergePivotBlock;
                                    blockGrid[mergePivotBlock.x - 1][mergePivotBlock.y - 1] = mergePivotBlock;
                                }
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
    int SortByNeighbours(Block a, Block b)
    {
        if ((a.badNeighbours - a.goodNeighbours) < (b.badNeighbours - b.goodNeighbours))
        {
            return -1;
        }
        else if ((a.badNeighbours - a.goodNeighbours) > (b.badNeighbours - b.goodNeighbours))
        {
            return 1;
        }
        return 0;
    }
    int SortByDistance(Block a, Block b)
    {
        if (a.distanceToBestBlock < b.distanceToBestBlock)
        {
            return -1;
        }
        else if (a.distanceToBestBlock > b.distanceToBestBlock)
        {
            return 1;
        }
        return 0;
    }
    void SwapBlocks(Block blockA, Block blockB)
    {
        int x_A = blockA.x;
        int y_A = blockA.y;
        Vector2Int pos_A = blockA.GetTargetPosition();

        int x_B = blockB.x;
        int y_B = blockB.y;
        Vector2Int pos_B = blockB.GetTargetPosition();

        blockGrid[x_A][y_A] = blockB;
        blockB.SetTargetPosition(pos_A);

        blockGrid[x_B][y_B] = blockA;
        blockA.SetTargetPosition(pos_B);
    }
    void Merge(Block blockPivot, Block blockOnLeft, Block blockOnTop, Block blockOnLeftTop)
    {

        if (blockOnLeft)
        {
            Destroy(blockOnLeft.gameObject);
        }
        if (blockOnTop)
        {
            Destroy(blockOnTop.gameObject);
        }
        if (blockOnLeftTop)
        {
            Destroy(blockOnLeftTop.gameObject);
        }

        blockPivot.Merge();
    }
}