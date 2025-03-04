using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FeedbackText : MonoBehaviour
{
    [SerializeField] private TextMeshPro effectText;

    [Space]

    [Tooltip("The speed the text floats up in")]
    public float floatSpeed;
    [Tooltip("The amount of time the text stays without flickering")]
    public float stayTime;
    [Tooltip("The amount of time the text will flicker for before disappearing")]
    public float flickerTime;
    [Tooltip("How fast each flicker will be")]
    public float flickerRate;
    [Tooltip("The angle in which the text will move at")]
    public Vector2 movementAngle = Vector2.up;

    public async void StartTextEffect(string text, Color? color = null, float? stayTime = null)
    {
        effectText.text = text;

        if (color.HasValue)
            effectText.color = color.Value;

        if (stayTime.HasValue)
            this.stayTime = stayTime.Value;

        await Hover();

        await HoverAndFlicker();

        Destroy(gameObject);
    }

    public void SetColor(Color color)
    {
        effectText.color = color;
    }

    public void SetFontSize(float size)
    {
        effectText.fontSize = size;
    }

    private async Task Hover()
    {
        for (float t = 0; t < stayTime; t += Time.deltaTime)
        {
            if (transform == null) return;

            transform.Translate(movementAngle.normalized * floatSpeed * Time.deltaTime);
            await Task.Yield();
        }
    }

    private async Task HoverAndFlicker()
    {
        for (float t = 0; t < flickerTime; t += Time.deltaTime)
        {
            if (transform == null) return;

            transform.Translate(movementAngle.normalized * floatSpeed * Time.deltaTime);
            float currentFlickerTimeframe = t % (flickerRate * 2);
            effectText.gameObject.SetActive(currentFlickerTimeframe <= flickerRate ? true : false);
            await Task.Yield();
        }
    }
}
