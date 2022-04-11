using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpriteAnimator : MonoBehaviour
{
    [Header("@Configurations")] 
    [SerializeField] private float speed = 1F;

    [SerializeField] private List<Sprite> frames = new List<Sprite>();

    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        var index = 0;
        
        while (true)
        {
            yield return new WaitForSeconds(1 / speed);

            GetComponent<Image>().sprite = frames[index % frames.Count];

            index++;
        }
    }
}