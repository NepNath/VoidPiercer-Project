using System.Collections.Generic;
using UnityEngine;


public class DungeonGraph : MonoBehaviour
{
    public int roomCount = 10;
    public List<RoomNode> rooms = new List<RoomNode>();
    public Dictionary<int, Vector2Int> roomPositions = new Dictionary<int, Vector2Int>();
    private int gridRange = 10;


    public void GenerateGraph()
    {
        rooms.Clear();
        roomPositions.Clear();
        HashSet<Vector2Int> usedPositions = new HashSet<Vector2Int>();

        // Étape 1 - Générer les salles avec positions aléatoires sur grille
        for (int i = 0; i < roomCount; i++)
        {
            Vector2Int pos;
            do
            {
                pos = new Vector2Int(
                    Random.Range(-gridRange, gridRange),
                    Random.Range(-gridRange, gridRange)
                );
            } while (usedPositions.Contains(pos));

            usedPositions.Add(pos);
            roomPositions[i] = pos;

            RoomType type = RoomType.Combat;
            rooms.Add(new RoomNode(i, type));
        }

        // Étape 2 - Connexions (graphe connexe, uniquement H/V)
        List<int> connected = new List<int> { 0 };
        List<int> unconnected = new List<int>();
        for (int i = 1; i < roomCount; i++) unconnected.Add(i);

        while (unconnected.Count > 0)
        {
            bool connectionMade = false;

            foreach (int from in connected)
            {
                for (int i = 0; i < unconnected.Count; i++)
                {
                    int to = unconnected[i];
                    if (AreAligned(roomPositions[from], roomPositions[to]))
                    {
                        rooms[from].ConnectedRooms.Add(rooms[to]);
                        rooms[to].ConnectedRooms.Add(rooms[from]);

                        connected.Add(to);
                        unconnected.RemoveAt(i);
                        connectionMade = true;
                        break;
                    }
                }
                if (connectionMade) break;
            }

            // Cas rare : pas de salle alignée → forcer alignement
            if (!connectionMade)
            {
                int from = connected[Random.Range(0, connected.Count)];
                int to = unconnected[0];
                Vector2Int alignedPos = AlignToGrid(roomPositions[from], roomPositions[to]);
                roomPositions[to] = alignedPos;

                rooms[from].ConnectedRooms.Add(rooms[to]);
                rooms[to].ConnectedRooms.Add(rooms[from]);

                connected.Add(to);
                unconnected.RemoveAt(0);
            }
        }

        // Étape 3 - Attribution des types spéciaux

        // Salle de départ
        rooms[0].Type = RoomType.Start;

        // Salle de boss = une feuille (1 seule connexion, pas start)
        List<RoomNode> leafRooms = rooms.FindAll(r => r.ConnectedRooms.Count == 1 && r.Id != 0);
        if (leafRooms.Count > 0)
        {
            RoomNode bossRoom = leafRooms[Random.Range(0, leafRooms.Count)];
            bossRoom.Type = RoomType.Boss;
        }

        // Salle de loot = une salle libre
        List<RoomNode> lootCandidates = rooms.FindAll(r => r.Type == RoomType.Combat);
        if (lootCandidates.Count > 0)
        {
            RoomNode lootRoom = lootCandidates[Random.Range(0, lootCandidates.Count)];
            lootRoom.Type = RoomType.Loot;
        }
    }

    private bool AreAligned(Vector2Int a, Vector2Int b)
    {
        return a.x == b.x || a.y == b.y;
    }

    private Vector2Int AlignToGrid(Vector2Int source, Vector2Int target)
    {
        if (Random.value < 0.5f)
            return new Vector2Int(source.x, target.y);
        else
            return new Vector2Int(target.x, source.y);
    }
}