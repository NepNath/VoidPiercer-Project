using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public Rigidbody rb;
    public Movement move;
    public WallActions wall;
    public Dash dash;
    public ConsoleDebugs CD;
    public PlayerHealthManager health;

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI Velocity;
    [SerializeField] TextMeshProUGUI Health;
    [SerializeField] TextMeshProUGUI DashCt;
    [SerializeField] TextMeshProUGUI WallJ;
    [SerializeField] TextMeshProUGUI drag;



    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        move = GetComponent<Movement>();
        wall = GetComponent<WallActions>();
        dash = GetComponent<Dash>();
        CD = GetComponent<ConsoleDebugs>();
        health = GetComponent<PlayerHealthManager>();
    }

    void Update()
    {

        //vitesse joueur
        if(Velocity != null)
        {
            Velocity.text = "Vitesse : " + rb.linearVelocity.magnitude.ToString("F2");
        }
        //linear damping joueur
        if(drag != null)
        {
            drag.text = "drag : " + rb.linearDamping.ToString("F2");
        }
        //vie du joueur
        if(Health != null)
        {
            Health.text = "Health : " + health.playerHealth;
        }
        //dash restant
        if(dash != null)
        {
            DashCt.text = "Dash Left : " + dash.dashCount;
        }
        //Walljump restant
        if(WallJ != null)
        {
            WallJ.text = "Wall Jump Left : " + wall.wallJumpCount;
        }


    }


}
