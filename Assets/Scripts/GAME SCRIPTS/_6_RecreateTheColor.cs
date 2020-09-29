using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _6_RecreateTheColor : MonoBehaviour {
    public GameObject Circle;
    public GameObject CirclePlayer;
    public GameObject sliderRed;
    public GameObject sliderGreen;
    public GameObject sliderBlue;
    public Button acceptButton;

    private Color RandomColorCircle;
    private Color PlayerColor;
    // Start is called before the first frame update
    void Start () {
        sliderRed.GetComponent<Slider>().onValueChanged.AddListener (delegate {updatePlayerColor ();});
        sliderGreen.GetComponent<Slider>().onValueChanged.AddListener (delegate {updatePlayerColor ();});
        sliderBlue.GetComponent<Slider>().onValueChanged.AddListener (delegate {updatePlayerColor ();});
        acceptButton.onClick.AddListener (delegate {compareColors ();});

        RandomColorCircle = new Color (
            Random.Range (0f, 1f),
            Random.Range (0f, 1f),
            Random.Range (0f, 1f)
        );

        Circle.GetComponent<Button> ().image.color = RandomColorCircle;
        updatePlayerColor();

    }

    public void compareColors(){
        float suma = Mathf.Abs(RandomColorCircle.r - PlayerColor.r) 
        + Mathf.Abs(RandomColorCircle.g - PlayerColor.g) 
        + Mathf.Abs(RandomColorCircle.b - PlayerColor.b);
        print("SUMA: " + suma);
        float percDiferente = 100 * suma * 100 / 255;
        print("PERC: " + percDiferente + "% de difencia");
    }

    public void updatePlayerColor(){
        PlayerColor = new Color (
            sliderRed.GetComponent<Slider> ().value,
            sliderGreen.GetComponent<Slider> ().value,
            sliderBlue.GetComponent<Slider> ().value
        );
        CirclePlayer.GetComponent<Button> ().image.color = PlayerColor;
    }
}