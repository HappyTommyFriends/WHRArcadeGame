using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoresControllerScrollInputReceiver : Receiver
{
    HighScoresController controller;

    public HighScoresControllerScrollInputReceiver(HighScoresController controller) {
      this.controller = controller;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Receive(Message message) {
      controller.EntryComplete(message._string);
    }
}
