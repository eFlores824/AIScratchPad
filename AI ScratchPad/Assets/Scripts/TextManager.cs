using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

    public Text BlueText;
    public Text RedText;

    private int BlueScore;
    private int RedScore;

    private void setBlueScore()
    {
        BlueText.text = "Blue Score: " + BlueScore;
    }

    private void setRedScore()
    {
        RedText.text = "Red Score: " + RedScore;
    }

    public void addBlueScore(int increase)
    {
        BlueScore += increase;
        setBlueScore();
    }

    public void addRedScore(int increase)
    {
        RedScore += increase;
        setRedScore();
    }

    public void removeBlueScore(int increase)
    {
        BlueScore -= increase;
        setBlueScore();
    }

    public void removeRedScore(int increase)
    {
        RedScore -= increase;
        setRedScore();
    }
}
