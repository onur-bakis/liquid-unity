using System;
using System.Collections.Generic;
using Scripts.Controller;
using UnityEngine.Serialization;

namespace Scripts.Data.ValueObject
{
    [Serializable]
    public struct LevelData
    {
        public List<GlassBallRow> levelData;
        public List<ObiRopeController> obiRopeControllers;
    }

    [Serializable]
    public struct GlassBallRow
    {
        public GlassBaseBall[] boardColumn;
    }
}