using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public interface IConfig
    {
        void Read();

        bool Loaded { get; set; }
    }
}
