using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Réglages")]
    public GameObject[] roomPrefabs;
    public GameObject corridorPrefab;
    public int maxRooms = 80;

    private List<Bounds> placedRoomBounds = new List<Bounds>();
    private List<Bounds> placedCorridorBounds = new List<Bounds>();

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        GameObject startRoom = Instantiate(roomPrefabs[0], Vector3.zero, Quaternion.identity, transform);
        Physics.SyncTransforms();
        placedRoomBounds.Add(GetPlacementBounds(startRoom));

        List<Transform> anchors = new List<Transform>();
        foreach (RoomAnchor a in startRoom.GetComponentsInChildren<RoomAnchor>())
            anchors.Add(a.transform);

        int roomCount = 1;

        while (roomCount < maxRooms && anchors.Count > 0)
        {
            Transform connectionAnchor = anchors[0];
            anchors.RemoveAt(0);

            RoomAnchor connectionAnchorScript = connectionAnchor.GetComponent<RoomAnchor>();
            AnchorDirection requiredDirection = OppositeDirection(connectionAnchorScript.direction);

            GameObject newRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], transform);
            RoomAnchor[] roomAnchors = newRoom.GetComponentsInChildren<RoomAnchor>();

            RoomAnchor matchingAnchor = null;
            foreach (RoomAnchor a in roomAnchors)
            {
                if (a.direction == requiredDirection)
                {
                    matchingAnchor = a;
                    break;
                }
            }

            if (matchingAnchor == null)
            {
                Debug.LogWarning($"❌ Aucun anchor compatible trouvé pour {newRoom.name}");
                Destroy(newRoom);
                continue;
            }

            // Rotation alignée
            Vector3 fromDir = DirectionToVector(matchingAnchor.direction);
            Vector3 toDir = -DirectionToVector(connectionAnchorScript.direction);
            Quaternion rotationNeeded = Quaternion.FromToRotation(fromDir, toDir);
            newRoom.transform.rotation = rotationNeeded;

            // Position alignée
            Vector3 rotatedAnchorWorldPos = matchingAnchor.transform.position;
            Vector3 offset = connectionAnchor.position - rotatedAnchorWorldPos;
            newRoom.transform.position += offset;

            Physics.SyncTransforms();
            Bounds newRoomBounds = GetPlacementBounds(newRoom);

            Debug.Log($"➡️ Tentative placement : {newRoom.name} à {newRoomBounds.center} / {newRoomBounds.size}");
            Debug.Log($"- Anchor de connexion : {connectionAnchorScript.direction} @ {connectionAnchor.position}");
            Debug.Log($"- Anchor matching     : {matchingAnchor.direction} @ {matchingAnchor.transform.position}");

            if (IsOverlapping(newRoomBounds))
            {
                Debug.LogWarning($"❌ Overlap détecté. Salle {newRoom.name} rejetée.");
                Destroy(newRoom);
                continue;
            }

            // Génération du couloir
            CorridorAnchor corridorAnchorA = connectionAnchorScript.linkedCorridorAnchor;
            CorridorAnchor corridorAnchorB = matchingAnchor.linkedCorridorAnchor;

            if (corridorAnchorA == null || corridorAnchorB == null)
            {
                Debug.LogWarning($"⚠️ CorridorAnchor manquant pour {newRoom.name}, rejeté.");
                Destroy(newRoom);
                continue;
            }

            Vector3 start = corridorAnchorA.transform.position;
            Vector3 end = corridorAnchorB.transform.position;
            Vector3 midpoint = (start + end) / 2f;
            Vector3 direction = (end - start).normalized;
            float distance = Vector3.Distance(start, end);

            GameObject corridor = Instantiate(corridorPrefab, midpoint, Quaternion.LookRotation(direction), transform);
            corridor.transform.localScale = new Vector3(corridor.transform.localScale.x, corridor.transform.localScale.y, distance);

            Physics.SyncTransforms();
            Bounds corridorBounds = GetPlacementBounds(corridor);

            // Si tu veux désactiver cette vérif TEMPORAIREMENT, commente ce bloc :
            if (IsOverlapping(corridorBounds))
            {
                Debug.LogWarning($"❌ Corridor overlap, rejet de {newRoom.name} + couloir.");
                Destroy(corridor);
                Destroy(newRoom);
                continue;
            }

            // Valide
            placedRoomBounds.Add(newRoomBounds);
            placedCorridorBounds.Add(corridorBounds);
            roomCount++;

            foreach (RoomAnchor a in roomAnchors)
            {
                if (a != matchingAnchor)
                    anchors.Add(a.transform);
            }
        }

        Debug.Log($"✔ Génération terminée : {roomCount} salle(s).");
    }

    bool IsOverlapping(Bounds testBounds)
    {
        Bounds test = testBounds;
        test.Expand(-1); // Tolérance

        foreach (Bounds room in placedRoomBounds)
        {
            if (room.Intersects(test))
            {
                Debug.LogWarning($"🟥 Overlap avec salle à {room.center}");
                return true;
            }
        }

        foreach (Bounds corridor in placedCorridorBounds)
        {
            if (corridor.Intersects(test))
            {
                Debug.LogWarning($"🟦 Overlap avec couloir à {corridor.center}");
                return true;
            }
        }

        return false;
    }

    Bounds GetPlacementBounds(GameObject obj)
    {
        PlacementVolume volume = obj.GetComponentInChildren<PlacementVolume>();
        if (volume != null)
        {
            BoxCollider box = volume.GetComponent<BoxCollider>();
            if (box != null)
            {
                Debug.Log($"📦 PlacementVolume trouvé : {box.bounds.center} / {box.bounds.size}");
                return box.bounds;
            }
        }

        // Fallback
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(obj.transform.position, Vector3.one * 10);

        Bounds combined = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            combined.Encapsulate(renderers[i].bounds);

        Debug.Log("⚠️ Fallback bounds utilisés.");
        return combined;
    }

    Vector3 DirectionToVector(AnchorDirection dir)
    {
        return dir switch
        {
            AnchorDirection.North => Vector3.forward,
            AnchorDirection.South => Vector3.back,
            AnchorDirection.East => Vector3.right,
            AnchorDirection.West => Vector3.left,
            _ => Vector3.forward
        };
    }

    AnchorDirection OppositeDirection(AnchorDirection dir)
    {
        return dir switch
        {
            AnchorDirection.North => AnchorDirection.South,
            AnchorDirection.South => AnchorDirection.North,
            AnchorDirection.East => AnchorDirection.West,
            AnchorDirection.West => AnchorDirection.East,
            _ => dir
        };
    }
}
