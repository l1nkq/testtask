using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NewTypes;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextAsset _areaDataFile;
    [SerializeField] private TextAsset _areaConnectionsFile;
    [SerializeField] private int _currentAreaID;
    [SerializeField] private List<Sprite> _backgrounds = new List<Sprite>();
    [SerializeField] private Image _bgCanvasImage;
    [SerializeField] private GameObject _bubbleText;
    [SerializeField] private GameObject _boxText;
    [SerializeField] private List<GameObject> _buttonPanels;
    private Dictionary<int, AreaData> areasDataDic = new Dictionary<int, AreaData>(); 
    private Dictionary<int, List<int>> areasConDic = new Dictionary<int, List<int>>(); 

    private InputSystem _inputSystem;
    private TouchInfo _touchInfo;

    void Awake()
    {
        _inputSystem = new InputSystem();
    }
    void Start()
    {
        areasDataDic = GetComponent<JSONAreaDataReader>().Read(_areaDataFile);
        areasConDic = GetComponent<JSONAreaConnectionReader>().Read(_areaConnectionsFile);

        LoadArea(_currentAreaID);
    }

    void Update()
    {
        _touchInfo = _inputSystem.ReadInput();

        if (_touchInfo.Phase == TouchPhase.Began && !_touchInfo.IsInteractableUI)
        {
            ChangeArea();
        }
        if (Input.GetKeyDown("a"))
        {
            Debug.Log(_bubbleText.GetComponentInChildren<TextMeshProUGUI>().GetPreferredValues());
        }
    }

    private void ChangeArea()
    {
        if (areasConDic.ContainsKey(_currentAreaID))
        {
            int buttonsNum = 0;
            List<string> buttonNames = new List<string>();
            foreach (int id in areasConDic[_currentAreaID])
            {
                if (areasDataDic[id].choice_description != "") 
                {
                    buttonsNum++;
                    buttonNames.Add(areasDataDic[id].choice_description);
                }
            }
            if (buttonsNum == 0) 
                LoadArea(areasConDic[_currentAreaID][0]);
            else
            {
                ShowButtons(buttonsNum, buttonNames);
            }
        }
        else Debug.LogError("Area Connection ID " + _currentAreaID + " not found!");    
    }

    private void ShowButtons(int buttonsNum, List<string> buttonNames)
    {
        _boxText.SetActive(false);
        _bubbleText.SetActive(false);
        _buttonPanels[buttonsNum-1].SetActive(true);
        //TextMeshProUGUI[] buttonTexts = _buttonPanels[buttonsNum-1].GetComponentsInChildren<TextMeshProUGUI>();
        Button[] buttons = _buttonPanels[buttonsNum-1].GetComponentsInChildren<Button>();
        for (int i = 0; i < buttonsNum; i++)
        {   
            int id = areasConDic[_currentAreaID][i];
            buttons[i].onClick.AddListener(() => ButtonClick(id));
            buttons[i].GetComponent<TextArea>().ChangeText(buttonNames[i]);
            //buttonTexts[i].text = buttonNames[i];
        }
    }

    private void DisableElements()
    {
        _boxText.SetActive(false);
        _bubbleText.SetActive(false);
        foreach (GameObject panel in _buttonPanels)
        {
            panel.SetActive(false);
        }
    }
    private bool LoadArea(int areaID)
    {
        DisableElements();

        if (areasDataDic.ContainsKey(areaID))
        {
            //Load BG Image
            foreach(Sprite bg in _backgrounds)
            {
                if (bg.name == areasDataDic[areaID].card.image.file_id)
                {
                    _bgCanvasImage.sprite = bg;

                    float multiH = Screen.height / bg.rect.height;
                    float multiW = Screen.width / bg.rect.width;
                    if (multiW < multiH)
                    {
                        //need adjust by width
                        float offset = (bg.rect.width * multiH - Screen.width) / 2;
                        _bgCanvasImage.rectTransform.sizeDelta = new Vector2(offset, 0);
                    }
                    else
                    {
                        //need adjust by height
                        float offset = (bg.rect.height * multiW - Screen.height) / 2;
                        _bgCanvasImage.rectTransform.sizeDelta = new Vector2(0, offset);
                    }
                }
            }
            //Load Speech Image
            if (areasDataDic[areaID].visualisations.Length > 0)
            {
                switch (areasDataDic[areaID].visualisations[0].id)
                {
                    case (int)SpeechType.Box:
                        _boxText.SetActive(true);
                        _boxText.GetComponent<TextArea>().ChangeText(areasDataDic[areaID].description);
                        break;
                    case (int)SpeechType.Bubble:
                        _bubbleText.SetActive(true);
                        _bubbleText.GetComponent<TextArea>().ChangeText(areasDataDic[areaID].description);
                        break;
                    default:
                        Debug.LogError("No speech Image for ID" + areasDataDic[areaID].visualisations[0].id + "!");
                        break;
                }
            }
            _currentAreaID = areaID;
            return true;
        }
        else
        {
            Debug.LogError("Area ID " + areaID + " not found!");
            return false;
        }
    }

    public void ButtonClick(int areaID)
    {
        LoadArea(areaID);
    }

}
