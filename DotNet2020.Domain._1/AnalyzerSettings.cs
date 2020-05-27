using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._1
{
    class AnalyzerSettings
    {
        public int MaxParamsCount { get; set; }
        public int MaxLineLength { get; set; }
        public int MaxMethodLength { get; set; }

        /// <summary>Максимально допустимое число методов в классе</summary>
        public int MaxClassСapacity { get; set; }
        public string Language { get; set; }
    }
}