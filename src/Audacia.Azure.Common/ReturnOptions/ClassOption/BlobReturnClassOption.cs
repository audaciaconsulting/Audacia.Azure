using System;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace Audacia.Azure.Common.ReturnOptions.ClassOption
{
    public class BlobReturnClassOption<T> : IQueueReturnOption<T>
    {
        public T Result { get; set; }

        public T Parse(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                throw new ArgumentNullException(
                    jsonString,
                    "Json string is either null or empty and therefore cannot be deserialized into a object.");
            }

            return JsonConvert.DeserializeObject<T>(jsonString)!;
        }
    }
}
