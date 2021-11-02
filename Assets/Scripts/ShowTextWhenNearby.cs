using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShowTextWhenNearby : MonoBehaviour
{

#region Variables
    // Start Variables
    private CircleCollider2D circle2d; 
    private GameObject sign_Canvas;
    private TextMeshProUGUI sign_Text;
    
    [Header("Sign Variables")]
    public float sign_ShowDuration;
    private Color32 startAlpha;
    private Color32 endAlpha;
    private float sign_LerpDuration;
    
#endregion    

#region Default Methods
    void Start()
    {
        // Grab the components and gameobjects
        circle2d = GetComponent<CircleCollider2D>();
        sign_Canvas = gameObject.transform.GetChild (0).gameObject;
        sign_Text = GetComponentInChildren<TextMeshProUGUI>();

        // Disable Canvas and children
        sign_Canvas.SetActive(false);

        // Set the variables for the sign opacity
        startAlpha = new Color32(0,0,0,0);
        endAlpha = new Color32(0,0,0,255);
    }
#endregion

#region Physics Methods
    void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(ShowText());
    }

    void OnTriggerExit2D(Collider2D other)
    {
        StartCoroutine(HideText());
    }
#endregion    

#region Custom Methods
    IEnumerator ShowText()
    {
        sign_Canvas.SetActive(true);
        while (sign_LerpDuration <= 1)
        {
            sign_Text.color = Color32.Lerp(startAlpha, endAlpha, sign_LerpDuration);
            sign_LerpDuration += Time.deltaTime/sign_ShowDuration;
            yield return null;
        }
    }

    IEnumerator HideText()
    {
        while (sign_LerpDuration >= 0)
        {
            sign_Text.color = Color32.Lerp(startAlpha, endAlpha, sign_LerpDuration);
            sign_LerpDuration -= Time.deltaTime/sign_ShowDuration;
            yield return null;
        }
        sign_Canvas.SetActive(false);
    }
#endregion

}
