using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextArea : MonoBehaviour
{
    [SerializeField] private float _defaultHeight;
    [SerializeField] private float _maxTextHeight;
    private TextMeshProUGUI _tmpro;
    private RectTransform _transform;

    void Awake()
    {
        _tmpro = GetComponentInChildren<TextMeshProUGUI>();
        _transform = this.gameObject.GetComponent<RectTransform>();
    }
    public void ChangeText(string text)
    {
        _tmpro.text = text;
        _tmpro.ForceMeshUpdate();
        float newHeight = _tmpro.GetPreferredValues().y;
        if (newHeight - _maxTextHeight > 0)
        {
            _transform.sizeDelta = new Vector2 (0, _defaultHeight + newHeight - _maxTextHeight);
        }
        else
        {
            _transform.sizeDelta = new Vector2 (0, _defaultHeight);
        }
    }
}
