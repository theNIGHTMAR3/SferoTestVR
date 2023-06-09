using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingPath : MonoBehaviour
{
    [SerializeField] GameObject[] platforms;
    [SerializeField] float platformOffset=1;
    [SerializeField] float extendingTime=1;
    [SerializeField] float stayTime=1;
    [SerializeField] float scaleDif=0.05f;


    private void Start()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].transform.position = Vector3.zero;
            platforms[i].transform.localScale = Vector3.one * (float)(1f - scaleDif * (i+1));            
        }

        StartCoroutine(Extend());
    }

    /// <summary>
    /// coroutine which extends the path
    /// </summary>    
    IEnumerator Extend()
    {
        float startTime = Time.time;
        while(Time.time < startTime + extendingTime)
        {
            float percentage = (Time.time - startTime) / (extendingTime);
            //interpolate positions
            for (int i = 0; i < platforms.Length; i++)
            {
                Vector3 pos = platforms[i].transform.localPosition;
                pos.x = Mathf.Lerp(0, platformOffset, percentage)*(i+1);
                platforms[i].transform.localPosition = pos;
            }
            yield return null;
        }

        //set final position
        for (int i = 0; i < platforms.Length; i++)
        {
            Vector3 pos = platforms[i].transform.localPosition;
            pos.x = platformOffset * (i+1);
            platforms[i].transform.localPosition = pos;
        }
        StartCoroutine(WaitToShorten());
    }

    /// <summary>
    /// coroutine which extends the path
    /// </summary>    
    IEnumerator WaitToShorten()
    {
        float startTime = Time.time;
        while (Time.time < startTime + stayTime)
        {            
            yield return null;
        }
        StartCoroutine(Shorten());
    }

    /// <summary>
    /// coroutine which extends the path
    /// </summary>    
    IEnumerator Shorten()
    {
        float startTime = Time.time;
        while (Time.time < startTime + extendingTime)
        {
            float percentage = (Time.time - startTime) / (extendingTime);
            //interpolate positions
            for (int i = 0; i < platforms.Length; i++)
            {
                Vector3 pos = platforms[i].transform.localPosition;
                pos.x = Mathf.Lerp(platformOffset, 0, percentage) * (i + 1);
                platforms[i].transform.localPosition = pos;
            }
            yield return null;
        }

        //set final position
        for (int i = 0; i < platforms.Length; i++)
        {            
            platforms[i].transform.localPosition = Vector3.zero;
        }

        StartCoroutine(WaitToExtend());
    }

    /// <summary>
    /// coroutine which extends the path
    /// </summary>    
    IEnumerator WaitToExtend()
    {
        float startTime = Time.time;
        while (Time.time < startTime + stayTime)
        {
            yield return null;
        }
        StartCoroutine(Extend());
    }
}
