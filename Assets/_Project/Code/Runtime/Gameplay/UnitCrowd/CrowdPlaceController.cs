using System.Collections.Generic;
using System.Linq;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.UnitCrowd
{
    public class CrowdPlaceController : MonoBehaviour
    {
        [SerializeField] private Transform[] _unitPlaces;
        [SerializeField] private LayerMask _unitsMask;
        
        private Collider[] _unitsBuffer = new Collider[100];

        public void DistributeUnits(IReadOnlyList<Unit> units)
        {
            if (units.Count > _unitPlaces.Length)
            {
                Debug.LogError(
                    $"Not enough places to distribute units. Units count: {units.Count}, Places count: {_unitPlaces.Length}");
                return;
            }

            List<Transform> placesLeft = _unitPlaces.ToList();

            foreach (var unit in units)
            {
                var randomPlaceIndex = Random.Range(0, placesLeft.Count);
                var selectedPlace = placesLeft[randomPlaceIndex];
                placesLeft.RemoveAt(randomPlaceIndex);
                
                unit.transform.position = selectedPlace.position;
                unit.transform.rotation = selectedPlace.rotation;
            }
        }

        public Unit[] GetUnitsInRadius(Unit unit, float radius)
        {
            int nearUnits = Physics.OverlapSphereNonAlloc(unit.transform.position, radius, _unitsBuffer, _unitsMask);
            Unit[] result = new Unit[nearUnits];
            
            for (int i = 0; i < nearUnits; i++)
                result[i] = _unitsBuffer[i].GetComponent<Unit>();
            
            return result;
        }
    }
}