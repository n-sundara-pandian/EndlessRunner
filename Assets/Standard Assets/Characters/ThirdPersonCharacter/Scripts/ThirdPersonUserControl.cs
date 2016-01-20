using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

        
        private void Start()
        {
            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
				m_Jump = Input.GetKey(KeyCode.UpArrow);
            }
        }
        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
			float h = 1.0f; //CrossPlatformInputManager.GetAxis("Horizontal");
			float v = 0;
            bool crouch = Input.GetKey(KeyCode.DownArrow);
            m_Move = v*Vector3.forward + h*Vector3.right;
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }

    }
}
