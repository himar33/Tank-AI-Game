using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

namespace Complete
{
    public class TankShooting : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify the different players.
        public Rigidbody m_Shell;                   // Prefab of the shell.
        public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
        public Slider m_AimSlider;                  // A child of the tank that displays the current launch force.
        public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
        public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
        public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
        public float m_LaunchForce = 300f;        // The force given to the shell if the fire button is held for the max charge time.
        public Transform m_EnemyPosition;
        public LookAtConstraint m_TurretLook;

        public Text bulletText;
        public float m_Bullets = 5;

        private string m_FireButton;                // The input axis that is used for launching shells.
        private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
        private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
        private bool m_Fired;                       // Whether or not the shell has been launched with this button press.
        private float m_Angle;                      //The angle value to set at the bullet transform rotation
        private float shotTime = 4;


        private void Start ()
        {
            // The fire axis is based on the player number.
            m_FireButton = "Fire" + m_PlayerNumber;
            m_Bullets = 5;
           
        }


        private void Update ()
        {
            if (m_Fired)
            {
                shotTime -= Time.deltaTime;
            }

            if (IsAbleToShot(m_FireTransform.position, m_EnemyPosition.position, m_LaunchForce, out m_Angle) && !m_Fired && m_Bullets > 0)
            {
                // ... launch the shell.
                Fire ();
                m_Bullets--;
                bulletText.text = m_Bullets.ToString();
            }

            if (m_Fired && shotTime <= 0)
            {
                m_Fired = false;
                shotTime = 4;
            }
        }


        private void Fire ()
        {
            // Set the fired flag so only Fire is only called once.
            m_Fired = true;

            // Create an instance of the shell and store a reference to it's rigidbody.
            Rigidbody shellInstance =
                Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            //Change fire transform rotation to new angle
            float angleDegrees = m_Angle * Mathf.Rad2Deg;
            m_FireTransform.transform.eulerAngles = new Vector3(
                angleDegrees,
                m_FireTransform.transform.eulerAngles.y,
                m_FireTransform.transform.eulerAngles.z
            );

            //Set bullet velocity
            shellInstance.velocity = m_LaunchForce * m_FireTransform.forward;

            // Change the clip to the firing clip and play it.
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play ();

            // Reset the launch force.  This is a precaution in case of missing button events.
            m_CurrentLaunchForce = m_LaunchForce;
        }

        bool IsAbleToShot(Vector3 from, Vector3 to, float speed, out float angle)
        {

            float xx = to.x - from.x;
            float xz = to.z - from.z;
            float x = Mathf.Sqrt(xx * xx + xz * xz);
            float y = from.y - to.y;

            float v = speed;
            float g = Physics.gravity.y;

            float sqrt = (v * v * v * v) - (g * (g * (x * x) + 2 * y * (v * v)));

            // Not enough range
            if (sqrt < 0)
            {
                angle = 0.0f;
                return false;
            }

            angle = Mathf.Atan(((v * v) - Mathf.Sqrt(sqrt)) / (g * x));
            return true;
        }
    }
}