using System.Runtime.Serialization;

namespace BNE.Core.Helpers
{
    public class SerializationInfo
    {

        public static T GetValue<T>(System.Runtime.Serialization.SerializationInfo info, string name)
        {
            foreach (SerializationEntry entry in info)
            {
                if (entry.Name.Equals(name))
                {
                    var value = info.GetValue(name, typeof(T));

                    if (value != null)
                        return (T)value;
                }
            }

            return default(T);
        }

    }
}
