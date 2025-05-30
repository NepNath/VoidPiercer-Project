using System.Collections.Generic;

public enum RoomType
{
    Start,
    Combat,
    Loot,
    Boss
}

public class RoomNode
{
    public int Id;
    public RoomType Type;
    public List<RoomNode> ConnectedRooms = new List<RoomNode>();

    public RoomNode(int id, RoomType type)
    {
        Id = id;
        Type = type;
    }
}