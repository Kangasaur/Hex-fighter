using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameTurn { Player, PlayerAction, Enemy }
public class PlayerMovement : MonoBehaviour
{
    public static GameTurn gameTurn = GameTurn.Player;
    [HideInInspector] public int actions = 3;
    [SerializeField] TextMeshProUGUI actionsText;
    TerrainManager terrain;
    Quaternion targetRot;
    Vector3 targetPos;
    void Start()
    {
        terrain = FindObjectOfType<TerrainManager>();
    }

    // Update is called once per frame
    void Update()
    {
        actionsText.text = "Actions: " + actions.ToString();
        switch (gameTurn)
        {
            case GameTurn.Player:
                if (Input.GetKeyDown(KeyCode.A))
                {
                    targetRot = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.down * 60);
                    gameTurn = GameTurn.PlayerAction;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    targetRot = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * 60);
                    gameTurn = GameTurn.PlayerAction;
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    TryMoveForward();
                }
                break;
            case GameTurn.PlayerAction:
                if (transform.rotation != targetRot)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.3f);
                    if (Quaternion.Angle(transform.rotation, targetRot) < 0.1f)
                    {
                        transform.rotation = targetRot;
                        gameTurn = GameTurn.Player;
                    }
                }
                else if (transform.position != targetPos)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);
                    if (Vector3.Distance(transform.position, targetPos) < 0.02f)
                    {
                        transform.position = targetPos;
                        gameTurn = GameTurn.Player;
                    }
                }
                break;
            case GameTurn.Enemy:
                actionsText.text = "Enemies' turn...";
                break;
        }
    }

    void TryMoveForward()
    {
        if (actions > 0)
        {
            Vector3 originalPosition = transform.position;
            targetPos = transform.TransformPoint(Vector3.forward * terrain.gridDistance);
            Collider[] collisions = Physics.OverlapBox(targetPos, new Vector3(0.25f, 0.5f, 0.25f));
            if (collisions.Length == 0) targetPos = originalPosition;
            else
            {
                foreach (Collider collision in collisions)
                {
                    switch(collision.gameObject.tag)
                    {
                        case "Terrain":
                            targetPos = collision.gameObject.transform.position + Vector3.up;
                            break;
                        case "Enemy":
                            targetPos = originalPosition;
                            break;
                        default:
                            break;
                    }
                }
            }
            if (targetPos != originalPosition)
            {
                actions--;
                gameTurn = GameTurn.PlayerAction;
            }
        }
    }
    public void EndTurn()
    {
        gameTurn = GameTurn.Enemy;
        Invoke("StartTurn", 0.2f);
    }
    
    public void StartTurn()
    {
        gameTurn = GameTurn.Player;
        actions = 3;
    }

}
