using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HighlightGlow : MonoBehaviour
{
    [SerializeField] private Color startColor = Color.white;
    [SerializeField] private Color endColor = Color.blue;
    [SerializeField] private float alpha = 0.08f;
    [SerializeField] private float time = 0.4f;
    private MeshRenderer _renderer;

    private bool doHighlight;

    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (doHighlight)
        {
            float lerp = Mathf.PingPong(Time.time, time) / time;
            Color currentColor = Color.Lerp(startColor, endColor, lerp);
            currentColor.a = alpha*3;
            _renderer.material.color = currentColor;
        }
    }

    private void LateUpdate()
    {
        if (!doHighlight)
        {
            Color currentColor = startColor;
            currentColor.a = alpha;
            _renderer.material.color = currentColor;
        }
    }

    public void DoHighLight(bool highlight)
    {
        doHighlight = highlight;
    }
}
