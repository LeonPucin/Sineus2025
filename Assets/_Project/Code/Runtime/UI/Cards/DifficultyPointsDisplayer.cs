using UnityEngine;

namespace UI.Cards
{
    public class DifficultyPointsDisplayer : MonoBehaviour
    {
        [SerializeField] private GameObject[] _allPoints;
        
        public void Display(int count)
        {
            for (int i = 0; i < _allPoints.Length; i++)
                _allPoints[i].SetActive(i < count);
        }
    }
}