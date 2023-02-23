using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    const int size = 64;
    [SerializeField] private GameObject emptyTile;
    [SerializeField] private GameObject obstacleTile;
    public int[,] mapLayout = new int[size, size];
    private void Awake()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if(i == 5 && j == 7 ||
                   i == 7 && j == 6 ||
                   i == 7 && j == 5 ||
                   i == 7 && j == 4 ||
                   i == 7 && j == 3 ||
                   i == 5 && j == 6 ||
                   i == 5 && j == 4 ||
                   i == 5 && j == 3 ||
                   i == 3 && j == 2 ||
                   i == 3 && j == 1 )
                {
                    mapLayout[i, j] = -1;
                }
                else
                {
                    mapLayout[i, j] = 0;
                }
            }
        }
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                if (mapLayout[i, j] == -1)
                {
                    var tile = Instantiate(obstacleTile, new Vector3(i, j, 0), Quaternion.identity);
                }
                else
                {
                    var tile = Instantiate(emptyTile, new Vector3(i, j, 0), Quaternion.identity);
                }
            }
        }
    }

}
