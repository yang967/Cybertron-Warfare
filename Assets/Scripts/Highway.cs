using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Highway : MonoBehaviour
{
    [SerializeField] GameObject[] bridge_;
    [SerializeField] GameObject car_pos_;
    [SerializeField] HighwayEntry entry;
    [SerializeField] Vector3 end;
    [SerializeField] Animator car_pos_animator;
    Animator animator_;
    int index;
    GameObject processed_car_;
    bool extended;

    private void Awake()
    {
        index = 0;
        processed_car_ = null;
        animator_ = GetComponent<Animator>();
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
        processed_car_.transform.eulerAngles = new Vector3(0, processed_car_.transform.eulerAngles.y, 0);
        processed_car_ = null;
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
