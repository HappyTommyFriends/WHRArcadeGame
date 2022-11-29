using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    public float delayOut = 0.5f;
    public float delayIn = 0.25f;

    protected bool paused = false;

    public static Blinker blink(GameObject go) {
      return go.AddComponent<Blinker>();
    }

    // Start is called before the first frame update
    void Start()
    {
      Invoke("Hide", delayOut);
    }

    void Hide() {
      gameObject.transform.position = new Vector3(gameObject.transform.position.x + 100f, gameObject.transform.position.y, gameObject.transform.position.z);
      Invoke("Show", delayIn);
    }

    void Show() {
      gameObject.transform.position = new Vector3(gameObject.transform.position.x - 100f, gameObject.transform.position.y, gameObject.transform.position.z);
      if(paused)
        return;

      Invoke("Hide", delayOut);
    }

    public void Pause(float duration) {
      CancelInvoke("Unpause");
      Pause();
      Invoke("Unpause", duration);
    }

    public void Pause() {
      paused = true;
      CancelInvoke("Hide");
    }

    public void Unpause() {
      paused = false;
      Hide();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
