using UnityEngine;
using UnityEngine.UI;

public class ElementCounter : MonoBehaviour
{
    public InputField CountInput;
    public Image Image;

    public int MaxCount;

    private int count;
    public int Count
    {
        get { return count; }
        set
        {
            count = value;
            CountInput.text = count.ToString();
        }
    }

    public void Start()
    {
        Count = 0;
    }

    public void UpCounter()
    {
        ++Count;

        if (Count > MaxCount)
        {
            Count = MaxCount;
        }
    }

    public void DownCounter()
    {
        --Count;

        if(Count < 0)
        {
            Count = 0;
        }
    }
}
