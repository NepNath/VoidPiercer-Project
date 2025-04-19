using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
   Rigidbody rb;
   Movement move;
   WallActions wall;
   Dash dash;
   ConsoleDebugs CD;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        move = GetComponent<Movement>();
        wall = GetComponent<WallActions>();
        dash = GetComponent<Dash>();
        CD = GetComponent<ConsoleDebugs>();
    }

    void Update()
    {
        
    }
}
