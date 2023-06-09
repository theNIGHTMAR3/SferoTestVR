using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform
{
    public enum PlatformState
    {
        MOVING_TO_START,
        MOVING_TO_END,
        STAY_AT_START,
        STAY_AT_END
    }

    public class MovingPlatform : MonoBehaviour
    {
        public GameObject startPos;
        public GameObject endPos;

        public float movingTime = 1f;
        public float stayTime = 1f;

        [SerializeField] PlatformState state = PlatformState.STAY_AT_START;

        float startTime = 0; // one variable to track "timers"


        // Update is called once per frame
        void Update()
        {
            float interpolation = 0;  

            switch (state)
            {
                case PlatformState.MOVING_TO_START:
                    if(Time.time > startTime + movingTime)
                    {
                        state = PlatformState.STAY_AT_START;
                        startTime = Time.time;
                        interpolation = 0f; //position set on end
                    }
                    else
                    {
                        interpolation = 1 - (Time.time - startTime)/ movingTime; //interpolate position
                    }
                    
                    break;
                case PlatformState.MOVING_TO_END:
                    if (Time.time > startTime + movingTime)
                    {
                        state = PlatformState.STAY_AT_END;
                        startTime = Time.time;
                        interpolation = 1f; //position set on start
                    }
                    else
                    {
                        interpolation = (Time.time - startTime) / movingTime; //interpolate position
                    } 
                    break;
                case PlatformState.STAY_AT_START:
                    if (Time.time > startTime + stayTime)
                    {
                        state = PlatformState.MOVING_TO_END;
                        startTime = Time.time;
                    }
                    interpolation = 0f; //position set on start
                    break;
                case PlatformState.STAY_AT_END:
                    if (Time.time > startTime + stayTime)
                    {
                        state = PlatformState.MOVING_TO_START;
                        startTime = Time.time;
                    }
                    interpolation = 1f; //position set on end

                    break;
            }
            transform.position = Vector3.Lerp(startPos.transform.position, endPos.transform.position, interpolation);            
        }
    }
}