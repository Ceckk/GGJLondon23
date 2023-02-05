using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI _score;

    void Update()
    {
        if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "end_loop")
        {
            _score.gameObject.SetActive(true);
            _score.text = PlayerPrefs.GetInt("score", 0).ToString();

            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}