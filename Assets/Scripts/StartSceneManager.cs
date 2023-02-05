using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite[] _startFrames;

    private int _pageIndex = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_pageIndex > 0)
            {
                _pageIndex--;
            }
            else
            {
                _pageIndex = _startFrames.Length - 1;
            }

            _image.sprite = _startFrames[_pageIndex];
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _pageIndex++;
            _pageIndex = _pageIndex % _startFrames.Length;

            _image.sprite = _startFrames[_pageIndex];
        }
    }
}
