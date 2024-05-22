using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ballCountText;
    public static UIManager Instance;
    private int ballCount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    // void Update()
    // {
    //     
    // }

    public void UpdateBallCount(int cnt)
    {
        if (ballCount != cnt)
        {
            ballCount = cnt;
            ballCountText.text = ballCount.ToString();
        }
    }
}
