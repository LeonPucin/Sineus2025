using System.Collections.Generic;

namespace DoubleDCore.Analytics
{
    public interface IMetricService
    {
        public void Goal(string eventID);

        public void Goal(string eventID, Dictionary<string, object> fields);
    }
}