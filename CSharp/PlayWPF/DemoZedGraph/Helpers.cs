using System;
using System.Drawing;

namespace DemoZedGraph
{
    static class Helpers
    {
        private static Color[] _colors;

        static Helpers()
        {
            _colors = new[] 
            {
                Color.Red,
                Color.Crimson,
                Color.Blue,
                Color.Green,
                Color.Black,
                Color.SteelBlue,
                Color.Purple,
                Color.Navy,
                Color.DarkOrange,
                Color.Brown,
                Color.Orange,
                Color.Firebrick,
                Color.DimGray,
                Color.Cyan,
                Color.DarkGreen,
                Color.RoyalBlue,
                Color.Gold,
                Color.SeaGreen,
                Color.Gray,
                Color.LightGray,
                Color.Yellow,
                Color.DarkBlue,
                Color.DarkRed,
                Color.GreenYellow,
                Color.Violet,
                Color.Wheat,
                Color.Indigo,
                Color.Tomato,
                Color.SlateGray,
                Color.SpringGreen,
                Color.Wheat,
                Color.Orchid,
                Color.DarkOrchid,
                Color.OrangeRed,
                Color.Azure,
                Color.DarkOliveGreen,
            };
        }

        public static Color GetColor(int index)
        {
            return _colors[index % _colors.Length];
        }

        public static DateTime KeepUntilMinute(this DateTime oriTime)
        {
            return new DateTime(oriTime.Year, oriTime.Month, oriTime.Day, oriTime.Hour, oriTime.Minute, 0);
        }
    }//Helpers
}
