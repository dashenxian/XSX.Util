using System;
using System.Collections.Generic;
using System.Text;

namespace XSX.Guids
{
    public class SequentialGuidGeneratorOptions
    {
        /// <summary>
        /// Default value: <see cref="SequentialGuidType.SequentialAtEnd"/>.
        /// </summary>
        public SequentialGuidType DefaultSequentialGuidType { get; set; }

        public SequentialGuidGeneratorOptions()
        {
            DefaultSequentialGuidType = SequentialGuidType.SequentialAtEnd;
        }
    }
}
