using System.Collections.Generic;
using UnityEngine;

public class DungeonVisualizer : MonoBehaviour
{
    public DungeonGraph graph;
    public float spacing = 10f;       // grille logique
    public float roomSize = 10f;      // taille réelle des salles

    [Header("Prefabs de salles par type")]
    public GameObject startRoomPrefab;
    public GameObject bossRoomPrefab;
    public GameObject lootRoomPrefab;
    public GameObject combatRoomPrefab;

    [Header("Prefab de couloir")]
    public GameObject corridorPrefab;

    private Dictionary<int, Vector3> roomPositions = new Dictionary<int, Vector3>();

    void Start()
    {
        graph.GenerateGraph();
        GenerateLayout();
        DrawConnections();
    }

    void GenerateLayout()
    {
        roomPositions.Clear();

        for (int i = 0; i < graph.roomCount; i++)
        {
            Vector2Int gridPos = graph.roomPositions[i];
            Vector3 worldPos = new Vector3(gridPos.x * roomSize, 0f, gridPos.y * roomSize);
            roomPositions[i] = worldPos;

            GameObject prefabToUse = GetPrefabByType(graph.rooms[i].Type);
            GameObject roomInstance = Instantiate(prefabToUse, worldPos, Quaternion.identity, this.transform);
            roomInstance.name = $"Room_{i}_{graph.rooms[i].Type}";
        }
    }

    void DrawConnections()
    {
        foreach (var room in graph.rooms)
        {
            Vector3 from = roomPositions[room.Id];
            foreach (var target in room.ConnectedRooms)
            {
                if (room.Id < target.Id)
                {
                    Vector3 to = roomPositions[target.Id];

                    // Instancier un couloir entre les deux points
                    SpawnCorridor(from, to);
                }
            }
        }
    }

    void SpawnCorridor(Vector3 from, Vector3 to)
    {
        Vector3 direction = to - from;
        float distance = direction.magnitude;

        // Centrer le couloir entre les deux salles
        Vector3 midPoint = from + direction / 2f;

        // Instancier le couloir
        GameObject corridor = Instantiate(corridorPrefab, midPoint, Quaternion.identity, this.transform);

        // Orientation du couloir (X ou Z)
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            corridor.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // aligné sur X
            corridor.transform.localScale = new Vector3(distance, 1f, 1f);
        }
        else
        {
            corridor.transform.rotation = Quaternion.Euler(0f, 90f, 0f); // aligné sur Z
            corridor.transform.localScale = new Vector3(distance, 1f, 1f);
        }
    }

    GameObject GetPrefabByType(RoomType type)
    {
        return type switch
        {
            RoomType.Start => startRoomPrefab,
            RoomType.Boss => bossRoomPrefab,
            RoomType.Loot => lootRoomPrefab,
            _ => combatRoomPrefab
        };
    }
}
