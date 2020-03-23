using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeExtractor.FilterSize
{
    public class Filter
    {
        internal OperatorEnum.Operators _Operator;
        internal long _Value;

        public Filter(OperatorEnum.Operators Operator, long Value)
        {
            _Operator = Operator;
            _Value = Value;
        }
    }
}
