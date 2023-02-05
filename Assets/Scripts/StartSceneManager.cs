using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite[] _startFrames;

    [SerializeField] private GameObject[] _pages;

    private int _pageIndex = 0;
    private int _menuIndex = 0;

    void Update()
    {
        if (_pageIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (_menuIndex)
                {
                    case 0:
                        SceneManager.LoadScene(1);
                        break;
                    case 1:
                        ChangePage(1);
                        break;
                    case 2:
                        ChangePage(2);
                        break;
                }
                return;
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_menuIndex > 0)
                {
                    _menuIndex--;
                }
                else
                {
                    _menuIndex = _startFrames.Length - 1;
                }

                _image.sprite = _startFrames[_menuIndex];
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                _menuIndex++;
                _menuIndex = _menuIndex % _startFrames.Length;

                _image.sprite = _startFrames[_menuIndex];
            }
        }
        else
        {
            if (Input.anyKeyDown)
            {
                ChangePage(0);
            }
        }
    }

    private void ChangePage(int index)
    {
        _pageIndex = index;

        for (int i = 0; i < _pages.Length; i++)
        {
            _pages[i].SetActive(i == index);
        }
    }
}
