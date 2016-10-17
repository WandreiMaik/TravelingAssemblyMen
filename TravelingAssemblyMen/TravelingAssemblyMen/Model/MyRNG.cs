using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelingAssemblyMen.Model
{
    public class MyRNG
    {
        private static Random _rng;

        private static bool _IsNotInitialized
        {
            get { return _rng == null; }
        }

        private static void _Initialize()
        {
            _rng = new Random(Environment.TickCount);
        } 

        public static int Next()
        {
            if (_IsNotInitialized)
            {
                _Initialize();
            }

            return _rng.Next();
        }

        public static int Next(int maxValue)
        {
            if (_IsNotInitialized)
            {
                _Initialize();
            }

            return _rng.Next(maxValue);
        }

        public static double NextDouble()
        {
            if (_IsNotInitialized)
            {
                _Initialize();
            }

            return _rng.NextDouble();
        }
    }
}
