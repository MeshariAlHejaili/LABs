using UnityEngine;
using UnityEngine.UI;

namespace Worksheet5And6
{
    public class DifficultyButton : MonoBehaviour
    {
        [Tooltip("Set this in the Inspector: 1 for Easy, 2 for Medium, 3 for Hard")]
        [SerializeField] private int difficulty; 
        private Button button;

        void Start()
        {
            button = GetComponent<Button>();
            // Add a listener so when clicked, it calls SetDifficulty
            button.onClick.AddListener(SetDifficulty);
        }

        void SetDifficulty()
        {
            // Passes the difficulty to GameManager and starts the game
            GameManager.Instance.StartGame(difficulty);
        }
    }
}
