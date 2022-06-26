[System.Serializable]
public class AreaData
{
    public string description;
    public string choice_description;
    public int id;
    public Visualisations[] visualisations;
    public Card card;
}
[System.Serializable]
public struct Visualisations
{
    public string title;
    public string description;
    public int id;
}
[System.Serializable]
public struct Card
{
    public int id;
    public int queststep_id;
    public ImageData image;
    public string updated_at;
}
[System.Serializable]
public struct ImageData
{
    public string file_id;
}
