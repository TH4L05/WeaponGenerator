using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class UiBar : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image bar;
    [SerializeField] private TextMeshProUGUI barText;
    [SerializeField] private PlayableDirector feedback;
    [SerializeField] private string textFormat = "000";
    [SerializeField] public bool billboardBar;
    private Camera playerCamera;

    #endregion

    #region UnityFunctions

    private void Start()
    {
        if (billboardBar)
        {
            var player = Game.instance.player.GetComponent<Player>();
            if (player == null) return;
            playerCamera = player.CameraMain;
        }   
    }

    void LateUpdate()
    {
        if (playerCamera != null && billboardBar)
        {
            transform.LookAt(transform.position + playerCamera.transform.forward);
        }
    }

    #endregion

    public void UpdateBar(float value, float maxValue)
    {
        //Debug.Log($"UiBarUpdate new Values are: {value} / {maxValue}");

        if (bar != null)
        {
            bar.fillAmount = value / maxValue;
        }
        if (barText != null)
        {
            barText.text = value.ToString(textFormat);
        }
        if (feedback != null)
        {
            feedback.Play();
        }
    }
}
