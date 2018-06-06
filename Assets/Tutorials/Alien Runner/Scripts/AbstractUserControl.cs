using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(AbstractCharacter))]
public class AbstractUserControl : MonoBehaviour {

    private AbstractCharacter m_Character;
    private bool m_Jump, action;

    private void Awake()
    {
        m_Character = GetComponent<AbstractCharacter>();
    }


    private void Update()
    {
        if (!m_Jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if (!action)
        {
            // Read the submit input in Update so button presses aren't missed.
            action = CrossPlatformInputManager.GetButtonDown("Submit");
        }
    }


    private void FixedUpdate()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        // Pass all parameters to the character control script.
        m_Character.Move(h, v, action, m_Jump);
        m_Jump = action = false;
    }
}
