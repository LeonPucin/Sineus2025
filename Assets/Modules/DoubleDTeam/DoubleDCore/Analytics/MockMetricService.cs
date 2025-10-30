using System.Collections.Generic;
using System.Linq;
using DoubleDCore.Extensions;
using UnityEngine;

namespace DoubleDCore.Analytics
{
    public class MockMetricService : IMetricService
    {
        public void Goal(string eventID)
        {
            Debug.Log($"Goal: {eventID}");
        }

        public void Goal(string eventID, Dictionary<string, object> fields)
        {
            Debug.Log($"Goal: {eventID} with fields:\n" +
                      $"{fields.Select(x => $"{x.Key}: {x.Value}").Glue("\n")}");
        }
    }
}