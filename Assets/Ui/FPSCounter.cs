using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FPSCounter : MonoBehaviour
{
    private TMP_Text fpsText;
    public int numCachedFrames = 20;
    private float[] frameCache;
    private int bufferIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        fpsText = gameObject.GetComponent<TMP_Text>();
        frameCache = new float[numCachedFrames];
    }

    // Update is called once per frame
    void Update()
    {
        //save current time
        frameCache[bufferIndex] = Time.realtimeSinceStartup;
        //compare to oldest frame: next one mod 20, so just increment
        int pastIndex = (bufferIndex == numCachedFrames - 1) ? 0 : bufferIndex + 1;


        int avgFramerate = (int) ((float) (numCachedFrames-1) / (frameCache[bufferIndex] - frameCache[pastIndex]));
        
        fpsText.text = avgFramerate.ToString();

        //increments bufferIndex
        bufferIndex = pastIndex;
    }
}
