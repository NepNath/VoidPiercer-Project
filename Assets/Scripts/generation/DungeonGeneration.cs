using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Réglages de génération")]
    public GameObject[] roomPrefabs;
    public GameObject corridorPrefab;
    public int maxRooms = 10;

    private List<Bounds> placedRooms = new List<Bounds>();

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        // Placer la première salle
        GameObject startRoom = Instantiate(roomPrefabs[0], Vector3.zero, Quaternion.identity, transform);
        placedRooms.Add(GetManualBounds(startRoom));

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
                Destroy(newRoom);
                continue;
            }

            // 1. Appliquer la rotation pour aligner les anchors
            Vector3 fromDir = DirectionToVector(matchingAnchor.direction);
            Vector3 toDir = -DirectionToVector(connectionAnchorScript.direction);
            Quaternion rotationNeeded = Quaternion.FromToRotation(fromDir, toDir);
            newRoom.transform.rotation = rotationNeeded;

            // 2. Positionner la salle
            Vector3 rotatedAnchorWorldPos = matchingAnchor.transform.position;
            Vector3 offset = connectionAnchor.position - rotatedAnchorWorldPos;
            newRoom.transform.position += offset;

            // 3. Vérifier les overlaps
            Bounds newBounds = GetManualBounds(newRoom);
            if (!IsOverlapping(newBounds))
            {
                placedRooms.Add(newBounds);

                // Ajouter tous les autres anchors pour continuer la génération
                foreach (RoomAnchor a in roomAnchors)
                {
                    if (a != matchingAnchor)
                        anchors.Add(a.transform);
                }

                roomCount++;

                // === Génération du couloir ===
                CorridorAnchor corridorAnchorA = connectionAnchor.GetComponent<RoomAnchor>()?.linkedCorridorAnchor;
                CorridorAnchor corridorAnchorB = matchingAnchor.GetComponent<RoomAnchor>()?.linkedCorridorAnchor;

                if (corridorAnchorA != null && corridorAnchorB != null)
                {
                    Vector3 start = corridorAnchorA.transform.position;
                    Vector3 end = corridorAnchorB.transform.position;

                    Vector3 midpoint = (start + end) / 2f;
                    Vector3 direction = (end - start).normalized;
                    float distance = Vector3.Distance(start, end);

                    GameObject corridor = Instantiate(corridorPrefab, midpoint, Quaternion.LookRotation(direction), transform);

                    Vector3 scale = corridor.transform.localScale;
                    scale.z = distance;
                    corridor.transform.localScale = scale;
                }
                else
                {
                    Debug.LogWarning("CorridorAnchor manquant ou non lié.");
                }
            }
            else
            {
                Destroy(newRoom);
            }
        }

        Debug.Log($"Génération terminée : {roomCount} salle(s) générée(s).");
    }

    // Vérifie les overlaps avec les salles existantes
    bool IsOverlapping(Bounds newBounds)
    {
        foreach (Bounds existing in placedRooms)
        {
            if (existing.Intersects(newBounds))
                return true;
        }
        return false;
    }

    // Combine tous les Renderers pour créer les bounds
    Bounds GetManualBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(obj.transform.position, Vector3.one * 10);

        Bounds combined = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            combined.Encapsulate(renderers[i].bounds);

        return combined;
    }

    // Convertit AnchorDirection en Vector3
    Vector3 DirectionToVector(AnchorDirection dir)
    {
        return dir switch
        {
            AnchorDirection.North => Vector3.forward,
            AnchorDirection.South => Vector3.back,
            AnchorDirection.East  => Vector3.right,
            AnchorDirection.West  => Vector3.left,
            _ => Vector3.forward
        };
    }

    // Renvoie la direction opposée
    AnchorDirection OppositeDirection(AnchorDirection dir)
    {
        return dir switch
        {
            AnchorDirection.North => AnchorDirection.South,
            AnchorDirection.South => AnchorDirection.North,
            AnchorDirection.East  => AnchorDirection.West,
            AnchorDirection.West  => AnchorDirection.East,
            _ => dir
        };
    }
}
