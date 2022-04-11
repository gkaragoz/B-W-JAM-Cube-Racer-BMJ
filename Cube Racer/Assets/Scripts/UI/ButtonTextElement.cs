using TMPro;
using UnityEngine;

public class ButtonTextElement : MonoBehaviour
{
    private TextMeshProUGUI _textMesh = null;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter()
    {
        _textMesh.color = Color.black;
    }
    
    public void OnPointerExit()
    {
        _textMesh.color = Color.white;
    }
}