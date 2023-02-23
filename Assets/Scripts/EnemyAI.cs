using System.Collections.Generic;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject player;
    [SerializeField] private GenerateMap map;
    Vector2Int wayPoint;
    List<int> path;
    float targetRotation;
    Vector2Int position;
    float speed = 0.03f;
    const int size = 64;
    float timer = 0;
    private int[,] mapLayout;
    private void ClearMap()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (mapLayout[i, j] > 0 || mapLayout[i, j] == -2)
                    mapLayout[i, j] = 0;
            }
        }
    }

    // returns path to the tile with coordinates (x,y)
    private List<int> FindPath(Vector2Int target)
    {
        List<int> Path = new List<int>();
        mapLayout[position.x, position.y] = -2;
        List<Vector2Int> directions = new List<Vector2Int>();
        directions.Add(new Vector2Int(1, 0));
        directions.Add(new Vector2Int(-1, 0));
        directions.Add(new Vector2Int(0, 1));
        directions.Add(new Vector2Int(0, -1));

        Queue<Vector2Int> tileQueue = new Queue<Vector2Int>();
        tileQueue.Enqueue(position);

        while (tileQueue.Count > 0)
        {
            Vector2Int current = tileQueue.Dequeue();
            foreach (Vector2Int dir in directions)
            {
                int nextX = current.x + dir.x;
                int nextY = current.y + dir.y;
                if (nextX >= size || nextX < 0 || nextY >= size || nextY < 0)
                {
                    break;
                }
                if (mapLayout[nextX, nextY] == 0)
                {
                    tileQueue.Enqueue(new Vector2Int(nextX, nextY));
                    mapLayout[nextX, nextY] = CoordinatesToIndex(current);
                    // backtracking  
                    if (nextX == target.x && nextY == target.y)
                    {
                        current.x = target.x;
                        current.y = target.y;
                        while (!(current.x == position.x && current.y == position.y))
                        {
                            Path.Add(CoordinatesToIndex(current));
                            int tileIndex = mapLayout[current.x, current.y];
                            current = IndexToCoordinates(tileIndex);
                        }
                        Path.Reverse();
                        ClearMap();
                        return Path;
                    }
                }
            }
        }
        ClearMap();
        return Path;
    }
    private Vector2Int IndexToCoordinates(int index)
    {
        return new Vector2Int(index % size, index / size);
    }
    private int CoordinatesToIndex(Vector2Int coordinates)
    {
        return coordinates.x + size * coordinates.y;
    }
    private bool CanDirectlyHit()
    {
        int layermask = (1 << 7) + (1 << 8);
        layermask = ~layermask;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, Mathf.Infinity, layermask);
        if (hit.transform.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }

    private void Start()
    {
        mapLayout = map.mapLayout;
        position = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        path = FindPath(new Vector2Int(Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.y)));
        wayPoint = IndexToCoordinates(path[0]);
        path.RemoveAt(0);
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) timer = 0;
    }
    private void FixedUpdate()
    {
        if(timer == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed);
            if ((Vector2)transform.position == wayPoint)
            {
                if (CanDirectlyHit())
                {
                    targetRotation = -Vector3.SignedAngle(player.transform.position - transform.position, Vector3.up, Vector3.forward);
                    transform.rotation = Quaternion.Euler(0,0,targetRotation);
                    Instantiate(bullet,transform.position,transform.rotation);
                    timer = 2f;
                }
                else
                {
                    path.Clear();
                    position = wayPoint;
                    path = FindPath(new Vector2Int(Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.y)));
                    // cant find a path or already at player's location
                    if (path.Count == 0)
                    {
                        wayPoint = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
                    }
                    else
                    {
                        wayPoint = IndexToCoordinates(path[0]);
                        path.RemoveAt(0);
                        targetRotation = -Vector3.SignedAngle(new Vector3(wayPoint.x, wayPoint.y, 0) - transform.position, Vector3.up, Vector3.forward);
                        transform.rotation = Quaternion.Euler(0, 0, targetRotation);
                    }
                }
                
            }
        }
    }
}
