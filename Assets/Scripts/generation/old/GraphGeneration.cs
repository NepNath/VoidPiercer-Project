// using UnityEngine;
// using System.Collections.Generic;

// public class GraphGeneration : MonoBehaviour
// {
//     [Header("Réglages")]
//     public GameObject[] roomPrefabs;
//     public GameObject corridorPrefab;
//     public int roomCount = 10;
//     public Vector2Int gridSpacing = new Vector2Int(20, 20); // Distance logique entre chaque salle

//     private Dictionary<Vector2Int, RoomNode> dungeonMap = new();

//     void Start()
//     {
//         GenerateLogicalGraph();
//         InstantiateRooms();
//         InstantiateCorridors();
//     }

//     void GenerateLogicalGraph()
//     {
//         dungeonMap.Clear();

//         Vector2Int currentPos = Vector2Int.zero;
//         RoomNode startNode = new RoomNode(currentPos);
//         dungeonMap.Add(currentPos, startNode);

//         Queue<RoomNode> queue = new Queue<RoomNode>();
//         queue.Enqueue(startNode);

//         int created = 1;

//         while (created < roomCount && queue.Count > 0)
//         {
//             RoomNode current = queue.Dequeue();
//             int branches = Random.Range(1, 4); // 1 à 3 connexions

//             for (int i = 0; i < branches; i++)
//             {
//                 if (created >= roomCount)
//                     break;

//                 Vector2Int dir = GetRandomDirection();
//                 Vector2Int newPos = current.gridPosition + dir;

//                 if (dungeonMap.ContainsKey(newPos))
//                     continue;

//                 RoomNode newNode = new RoomNode(newPos);
//                 dungeonMap.Add(newPos, newNode);

//                 current.neighbors.Add(newNode);
//                 newNode.neighbors.Add(current);

//                 queue.Enqueue(newNode);
//                 created++;
//             }
//         }
//     }

//     void InstantiateRooms()
//     {
//         foreach (var pair in dungeonMap)
//         {
//             Vector2 worldPos = Vector2.Scale(pair.Key, gridSpacing);
//             GameObject room = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], new Vector3(worldPos.x, 0f, worldPos.y), Quaternion.identity, transform);
//             pair.Value.instantiatedRoom = room;
//         }
//     }

//     void InstantiateCorridors()
//     {
//         HashSet<(RoomNode, RoomNode)> processed = new();

//         foreach (var pair in dungeonMap)
//         {
//             RoomNode node = pair.Value;

//             foreach (RoomNode neighbor in node.neighbors)
//             {
//                 if (processed.Contains((neighbor, node)) || processed.Contains((node, neighbor)))
//                     continue;

//                 GameObject roomA = node.instantiatedRoom;
//                 GameObject roomB = neighbor.instantiatedRoom;

//                 RoomAnchor anchorA = FindAnchorByDirection(roomA, node.gridPosition, neighbor.gridPosition);
//                 RoomAnchor anchorB = FindAnchorByDirection(roomB, neighbor.gridPosition, node.gridPosition);

//                 if (anchorA == null || anchorB == null)
//                 {
//                     Debug.LogWarning("⚠️ Anchor non trouvé entre deux salles connectées.");
//                     continue;
//                 }

//                 CorridorAnchor corridorAnchorA = anchorA.linkedCorridorAnchor;
//                 CorridorAnchor corridorAnchorB = anchorB.linkedCorridorAnchor;

//                 if (corridorAnchorA == null || corridorAnchorB == null)
//                 {
//                     Debug.LogWarning("⚠️ CorridorAnchor manquant sur un anchor.");
//                     continue;
//                 }

//                 Vector3 start = corridorAnchorA.transform.position;
//                 Vector3 end = corridorAnchorB.transform.position;

//                 // Corridors en L uniquement
//                 if (Mathf.Abs(start.x - end.x) > 0.1f && Mathf.Abs(start.z - end.z) > 0.1f)
//                 {
//                     Vector3 corner = new Vector3(end.x, start.y, start.z); // angle droit
//                     CreateCorridor(start, corner);
//                     CreateCorridor(corner, end);
//                 }
//                 else
//                 {
//                     CreateCorridor(start, end);
//                 }

//                 processed.Add((node, neighbor));
//             }
//         }
//     }

//     void CreateCorridor(Vector3 start, Vector3 end)
//     {
//         Vector3 midpoint = (start + end) / 2f;
//         Vector3 direction = (end - start).normalized;
//         float distance = Vector3.Distance(start, end);

//         GameObject corridor = Instantiate(corridorPrefab, midpoint, Quaternion.LookRotation(direction), transform);
//         Vector3 scale = corridor.transform.localScale;
//         scale.z = distance;
//         corridor.transform.localScale = scale;
//     }

//     RoomAnchor FindAnchorByDirection(GameObject room, Vector2Int from, Vector2Int to)
//     {
//         Vector2Int delta = to - from;

//         AnchorDirection neededDir;

//         if (delta == Vector2Int.up) neededDir = AnchorDirection.North;
//         else if (delta == Vector2Int.down) neededDir = AnchorDirection.South;
//         else if (delta == Vector2Int.left) neededDir = AnchorDirection.West;
//         else if (delta == Vector2Int.right) neededDir = AnchorDirection.East;
//         else
//         {
//             Debug.LogWarning("⚠️ Direction non cardinal (diagonale ?)");
//             return null;
//         }

//         RoomAnchor[] anchors = room.GetComponentsInChildren<RoomAnchor>();
//         foreach (RoomAnchor a in anchors)
//         {
//             if (a.direction == neededDir)
//                 return a;
//         }

//         return null;
//     }

//     Vector2Int GetRandomDirection()
//     {
//         Vector2Int[] directions = {
//             Vector2Int.up,
//             Vector2Int.down,
//             Vector2Int.left,
//             Vector2Int.right
//         };

//         return directions[Random.Range(0, directions.Length)];
//     }
// }

// // Classe du graphe
// public class RoomNode
// {
//     public Vector2Int gridPosition;
//     public List<RoomNode> neighbors = new();
//     public GameObject instantiatedRoom;

//     public RoomNode(Vector2Int position)
//     {
//         gridPosition = position;
//     }
// }
