using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform
{
    public class MovingPlatform : FourActionStateMachine
    {
        public GameObject startPos;
        public GameObject endPos;

        public float movingTime = 1f;
        public float stayTime = 1f;

        Rigidbody rb;

        protected override void Start()
        {
            //rb = GetComponent<Rigidbody>();

            //set timers using human readeable names
            action1Time = stayTime; //waiting at start
            action2Time = movingTime; //moving to end
            action3Time = stayTime; //waiting at end
            action4Time = movingTime; //moving to start
            base.Start();
        }        

        //waiting at start
        protected override void Action1(float percentage)
        {
            transform.position = Vector3.Lerp(startPos.transform.position, endPos.transform.position, 0);
            //rb.MovePosition(Vector3.Lerp(startPos.transform.position, endPos.transform.position, 0));
        }

        //moving to end
        protected override void Action2(float percentage)
        {
            transform.position = Vector3.Lerp(startPos.transform.position, endPos.transform.position, percentage);
            //rb.MovePosition(Vector3.Lerp(startPos.transform.position, endPos.transform.position, percentage));
        }

        //waiting at end
        protected override void Action3(float percentage)
        {
            transform.position = Vector3.Lerp(startPos.transform.position, endPos.transform.position, 1);
            //rb.MovePosition(Vector3.Lerp(startPos.transform.position, endPos.transform.position, 1));
        }

        //moving to start
        protected override void Action4(float percentage)
        {
            transform.position = Vector3.Lerp(startPos.transform.position, endPos.transform.position, 1-percentage);
            //rb.MovePosition(Vector3.Lerp(startPos.transform.position, endPos.transform.position, 1-percentage));
        }
    }


}