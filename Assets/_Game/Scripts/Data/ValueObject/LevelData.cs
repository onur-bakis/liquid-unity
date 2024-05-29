using System;
using System.Collections.Generic;
using Scripts.Controller;
using UnityEngine.Serialization;

namespace Scripts.Data.ValueObject
{
    [Serializable]
    public struct LevelData
    {
        public string title;
        public int glassBallNumber;
        public List<GlassBaseBall> glassBaseBalls;
    }
}