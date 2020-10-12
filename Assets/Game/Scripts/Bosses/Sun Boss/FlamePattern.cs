using System;

namespace Bosses.Sun_Boss
{
    [Serializable]
    public class FlamePattern
    {
        public bool Flame0;
        public bool Flame1;
        public bool Flame2;
        public bool Flame3;
        public bool Flame4;
        public bool Flame5;
        public bool Flame6;
        public bool Flame7;
        public bool Flame8;
        public bool Flame9;
        public bool Flame10;
        public bool Flame11;
        public bool Flame12;
        public bool Flame13;
        public bool Flame14;
        public bool Flame15;

        public bool[] Flames => new bool[]
        {
            Flame0, Flame1, Flame2, Flame3,
            Flame4, Flame5, Flame6, Flame7,
            Flame8, Flame9, Flame10, Flame11,
            Flame12, Flame13, Flame14, Flame15
        };
    }
}