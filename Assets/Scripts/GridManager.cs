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
    void Start()
    {
        pivot = GetComponentInChildren<Pivot>();
        Block originBlock = this.transform.GetComponentInChildren<Block>();
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

        if (lastUsedRow > 0)
        {
            if (blockGrid[lastUsedRow].Count > 1)
            {
                if (blockGrid[lastUsedRow][lastUsedColumn].type == blockGrid[lastUsedRow][lastUsedColumn - 1].type)
                {
                    if (blockGrid[lastUsedRow][lastUsedColumn].scale == blockGrid[lastUsedRow][lastUsedColumn - 1].scale)
                    {
                        if (blockGrid[lastUsedRow][lastUsedColumn].type == blockGrid[lastUsedRow - 1][lastUsedColumn].type)
                        {
                            if (blockGrid[lastUsedRow][lastUsedColumn].scale == blockGrid[lastUsedRow - 1][lastUsedColumn].scale)
                            {
                                if (blockGrid[lastUsedRow][lastUsedColumn].type == blockGrid[lastUsedRow - 1][lastUsedColumn - 1].type)
                                {
                                    if (blockGrid[lastUsedRow][lastUsedColumn].scale == blockGrid[lastUsedRow - 1][lastUsedColumn - 1].scale)
                                    {
                                        StartCoroutine(Merge(block, blockGrid[lastUsedRow][lastUsedColumn - 1], blockGrid[lastUsedRow - 1][lastUsedColumn], blockGrid[lastUsedRow - 1][lastUsedColumn - 1]));

                                        blockGrid[lastUsedRow][lastUsedColumn - 1] = block;
                                        blockGrid[lastUsedRow - 1][lastUsedColumn] = block;
                                        blockGrid[lastUsedRow - 1][lastUsedColumn - 1] = block;
                                    }
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

    IEnumerator Merge(Block blockPivot, Block blockOnLeft, Block blockOnTop, Block blockOnLeftTop)
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(blockOnLeft.gameObject);
        Destroy(blockOnTop.gameObject);
        Destroy(blockOnLeftTop.gameObject);

        blockPivot.Merge();
    }
}