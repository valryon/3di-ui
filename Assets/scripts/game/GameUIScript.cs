using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIScript : MonoBehaviour
{
  public Text scoreLabel;
  public Text timeLabel;
  public Slider ammoSlider;

  private GameScript game;

  void Start()
  {
    game = FindObjectOfType<GameScript>();

    scoreLabel.text = "";
    timeLabel.text = "";
    ammoSlider.minValue = 0;
    ammoSlider.maxValue = game.barrelSize;
  }

  void Update()
  {
    scoreLabel.text = game.score.ToString("N6");
    timeLabel.text = game.timeLeft.ToString("00");
    ammoSlider.value = game.currentBarrel;
  }
}

