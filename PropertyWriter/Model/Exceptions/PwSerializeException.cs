using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model
{
    class PwSerializeMethodException : Exception
    {
        public PwSerializeMethodException(string message)
            : base(message)
        {
        }
    }

    class PwStructureMismatchException : Exception
    {
        public PwStructureMismatchException(string modelName, string propertyName)
        {
            ObjectName = modelName;
            PropertyName = propertyName;
        }

        public string ObjectName { get; }
        public string PropertyName { get; }

        public override string Message => $"<{ObjectName}> の \"{PropertyName}\" に対応するデータがファイル内に見つかりませんでした。";
    }
}
