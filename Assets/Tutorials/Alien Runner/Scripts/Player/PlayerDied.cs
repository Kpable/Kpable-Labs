using UnityEngine;
using System.Collections;
namespace Kpable.Tutorials.AlienRunner
{
    public class PlayerDied : MonoBehaviour
    {

        public delegate void EndGame();
        public static event EndGame endGame;

        void PlayerDiedEndGame()
        {
            if (endGame != null)
            {
                endGame();
            }
            Destroy(gameObject);
        }


        void OnTriggerEnter2D(Collider2D target)
        {
            //Debug.Log("Triggered: " + target.name);
            if (target.name == "Collector")
            {
                PlayerDiedEndGame();
            }

        }
        void OnCollisionEnter2D(Collision2D target)
        {
            Debug.Log("Collided: " + target.gameObject.name);

            if (target.gameObject.tag == "Enemy")
            {
                PlayerDiedEndGame();
            }

        }
    }
}