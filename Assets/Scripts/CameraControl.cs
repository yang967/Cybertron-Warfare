using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float height_ = 50;
    [SerializeField] float speed_ = 70;
    [SerializeField] bool FixMode = false;

    [SerializeField] GameObject MapCam;

    void Awake()
    {
        transform.position = new Vector3(transform.position.x, height_, transform.position.z);
    }

    void Update()
    {
        CameraMove();
    }

    void CameraMove()
    {
        if (FixMode || GameManager.instance.isPaused)
            return;
        if (Input.mousePosition.y >= Screen.height - 5)
            Moveforward();
        if (Input.mousePosition.y <= 5)
            Movebackward();
        if (Input.mousePosition.x >= Screen.width - 5)
            Moveright();
        if (Input.mousePosition.x <= 5)
            Moveleft();

        float x = transform.position.x;
        float z = transform.position.z;

        Vector3 PlayerMiddle = GameManager.PlayerMiddlePosition;
        Vector3 Bound = GameManager.instance.getCameraBound();
        float x_bound = Bound.x / 2;
        float z_bound = Bound.z / 2;
        if (x + PlayerMiddle.x < -x_bound)
            x = -x_bound - PlayerMiddle.x;
        else if (x + PlayerMiddle.x > x_bound)
            x = x_bound - PlayerMiddle.x;

        if(z + PlayerMiddle.z < -z_bound) 
            z = -z_bound - PlayerMiddle.z;
        else if(z + PlayerMiddle.z > z_bound)
            z = z_bound - PlayerMiddle.z;

        transform.position = new Vector3(x, height_, z);
    }

    void Moveforward()
    {
        cancel_player_middle();
        Vector3 forwardMove = transform.forward;
        forwardMove.y = 0;
        forwardMove.Normalize();
        forwardMove *= speed_ * Time.deltaTime;
        transform.position += forwardMove;
    }

    void Movebackward()
    {
        cancel_player_middle();
        Vector3 backwardMove = transform.forward;
        backwardMove.y = 0;
        backwardMove.Normalize();
        backwardMove *= -speed_ * Time.deltaTime;
        transform.position += backwardMove;
    }
    void Moveleft()
    {
        cancel_player_middle();
        Vector3 leftMove = transform.right * -speed_ * Time.deltaTime;
        transform.position += leftMove;
    }
    void Moveright()
    {
        cancel_player_middle();
        Vector3 rightMove = transform.right * speed_ * Time.deltaTime;
        transform.position += rightMove;
    }

    public void setCameraSpeed(float speed)
    {
        speed_ = speed;
    }

    void cancel_player_middle()
    {
        GameManager.instance.CancelPlayerMiddle();
    }
}
