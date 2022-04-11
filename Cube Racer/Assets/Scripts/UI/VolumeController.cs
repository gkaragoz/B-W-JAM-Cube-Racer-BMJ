using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Sprite Mute = null;
    [SerializeField] private Sprite LowVolume = null;
    [SerializeField] private Sprite HighVolume = null;

    [SerializeField] private Slider slider = null;
    
    private Image _target = null;

    private void Awake()
    {
        _target = GetComponent<Image>();
    }

    public void Update()
    {
        if (slider.value == 0F)
            _target.sprite = Mute;
        else if (slider.value > 0F && slider.value < .25F)
            _target.sprite = LowVolume;
        else if (slider.value > .25F)
            _target.sprite = HighVolume;
    }

    public void ValidateVolume()
    {
        GameManager.OnVolumeChange?.Invoke(slider.value);
    }
}
