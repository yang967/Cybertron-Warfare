using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Highway : MonoBehaviour
{
    [SerializeField] GameObject[] bridge_;
    [SerializeField] GameObject car_pos_;
    [SerializeField] float angular_speed_ = 0.5f;
    [SerializeField] float car_speed_ = 120.0f;
    [SerializeField] HighwayEntry entry;
    [SerializeField] Vector3 end;
    [SerializeField] Animator car_pos_animator;
    Animator animator_;
    int index;
    int car_index_;
    bool start_car_;
    GameObject processed_car_;
    Quaternion car_from_;
    Vector3 init_pos_;
    Quaternion init_rotation_;
    float timecount_;
    Vector3 next_;
    bool extended;
    float car_height_;

    private void Awake()
    {
        index = 0;
        car_index_ = 0;
        start_car_ = false;
        processed_car_ = null;
        animator_ = GetComponent<Animator>();
        init_pos_ = car_pos_.transform.position;
        init_rotation_ = car_pos_.transform.rotation;
        timecount_ = 0;
        extended = false;
    }

    private void FixedUpdate()
    {
        
        /*if (start_car_)
        {
            car_pos_.transform.rotation = Quaternion.Lerp(car_from_, Quaternion.LookRotation(next_ - car_pos_.transform.position), timecount_ * angular_speed_);
            timecount_ += Time.deltaTime;
            car_pos_.transform.position = Vector3.MoveTowards(car_pos_.transform.position, next_, car_speed_ * Time.deltaTime);
            if(Mathf.Abs(next_.x - car_pos_.transform.position.x) < 0.001f && Mathf.Abs(next_.z - car_pos_.transform.position.z) < 0.001f)
            {
                if(next_.Equals(end))
                {
                    processed_car_.GetComponent<NavMeshAgent>().enabled = true;
                    processed_car_.transform.GetChild(0).GetChild(0).GetComponent<CarControl>().OutOfHighWay();
                    start_car_ = false;
                    processed_car_.transform.eulerAngles = new Vector3(0, processed_car_.transform.eulerAngles.y, 0);
                    processed_car_ = null;
                    car_index_ = 0;
                    animator_.SetTrigger("reset");

                }
                if (car_index_ >= bridge_.Length - 1)
                {
                    next_ = end;
                }
                else
                {
                    car_index_++;
                    next_ = bridge_[car_index_].transform.position + bridge_[car_index_].transform.forward * 20 + new Vector3(0, car_height_, 0);
                }
            }
        }*/
    }

    public void ExitHighway()
    {
        processed_car_.GetComponent<NavMeshAgent>().enabled = true;
        processed_car_.transform.GetChild(0).GetChild(0).GetComponent<CarControl>().OutOfHighWay();
        start_car_ = false;
        processed_car_.transform.eulerAngles = new Vector3(0, processed_car_.transform.eulerAngles.y, 0);
        processed_car_ = null;
        car_index_ = 0;
        animator_.SetTrigger("reset");
    }

    public void ActivateNext()
    {
        
        if(extended)
        {
            bridge_[index].GetComponent<Animator>().SetTrigger("reset");
            if(index > 0)
                index--;
        }
        else
        {
            bridge_[index].GetComponent<Animator>().SetTrigger("extend");
            if(index < bridge_.Length - 1)
                index++;
        }
    }

    public void FinishReset()
    {
        extended = false;
        entry.reseted();
    }

    public GameObject getCarPos()
    {
        return car_pos_;
    }

    public void startCar()
    {
        if (processed_car_ == null)
            return;
        car_index_ = 0;
        car_from_ = car_pos_.transform.rotation;
        next_ = bridge_[0].transform.position + bridge_[0].transform.forward * 20;
        start_car_ = true;
    }

    public void Extended()
    {
        extended = true;
    }

    public void initialize(GameObject car)
    {
        if(car.transform.GetChild(0).GetChild(0).GetComponent<CarControl>() == null)
        {
            Debug.Log("Target Object is not a Car");
            return;
        }

        GameManager.instance.PlayerMiddle();
        processed_car_ = car;
        //car_height_ = processed_car_.GetComponent<NavMeshAgent>().baseOffset;
        //car_pos_.transform.position = init_pos_ + new Vector3(0, car_height_, 0);
        //car_pos_.transform.rotation = init_rotation_;
        animator_.SetTrigger("extend");
        car_pos_animator.SetTrigger("Start");
    }
}
