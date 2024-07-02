using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] buttons;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip interactSound;
    private RectTransform arrow;
    private int currentPosition;

    private void Awake()
    {
        arrow = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        currentPosition = 0;
        ChangePosition(0);
    }

    private void Update()
    {
        //Change the position of the selection arrow
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            print("Up menu" + currentPosition + " btns: " + buttons.Length);
            ChangePosition(-1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            print("Down menu" + currentPosition + " btns: " + buttons.Length);
            ChangePosition(1);
        }

        //Interact with current option
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.P))
        {
            print("Interact onclick");
            Interact();
        }
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0)
        {
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySound(changeSound);
            }
            else
            {
                print("SoundManager.instance is null");
            }
        }

        if (currentPosition < 0)
        {
            print("Less than 0 ok: " + currentPosition);
            currentPosition = buttons.Length - 1;
        }
        else if (currentPosition > buttons.Length - 1)
        {
            print("More than length ok: " + currentPosition);
            currentPosition = 0;
        }

        AssignPosition();
    }

    private void AssignPosition()
    {
        //Assign the Y position of the current option to the arrow (basically moving it up and down)
        arrow.position = new Vector3(arrow.position.x, buttons[currentPosition].position.y);
    }

    private void Interact()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(interactSound);
        }
        else
        {
            print("SoundManager.instance is null");
        }

        //Access the button component on each option and call its function
        buttons[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}