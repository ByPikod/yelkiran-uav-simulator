using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaneController : MonoBehaviour
{

    [Serializable]
    public struct Ball
    {
        public GameObject ballObject;
        public Button dropButton;
    }

    [Serializable]
    public struct EnginePowerController
    {
        public TextMeshProUGUI textToUpdate;
        public Slider slider;
    }

    [SerializeField]
    public EnginePowerController enginePowerController;
    
    [SerializeField]
    public Ball ball;

    [SerializeField]
    public TextMeshProUGUI speedTextToUpdate;

    private Rigidbody rb;
    private int enginePower = 0;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();

        enginePowerController.slider.onValueChanged.AddListener(
            delegate
            {
                UpdateEnginePower();
            } 
        );

        ball.dropButton.onClick.AddListener(
            delegate
            {
                DropTheBall();
            }
        );

        UpdateEnginePower();

    }

    private void FixedUpdate()
    {
        if (transform.position.z > 300)
            transform.position = new Vector3(transform.position.x, transform.position.y, -300);
        rb.AddForce(Vector3.forward * (enginePower * 140));
        speedTextToUpdate.text = String.Format("{0}\nkm/h", Math.Floor(rb.velocity.magnitude));
    }

    public void UpdateEnginePower()
    {
        enginePower = (int)(enginePowerController.slider.value * 100); // update actual engine power
        enginePowerController.textToUpdate.text = String.Format("Engine Power ({0}):", enginePower); // update text
    }

    public void DropTheBall()
    {
        GameObject createdBall = Instantiate(ball.ballObject);
        createdBall.name = "Ball";
        createdBall.transform.position = transform.position;
        Destroy(ball.ballObject);
        ball.ballObject = createdBall;
    }

}
