using System;
using System.Collections.Generic;
using ToastIOS;

namespace Caelum
{
    public static class Funktionen
    {
        public static Dictionary<String, Action<String>> actions = new Dictionary<String, Action<String>>()
        {
            {"Toast", s => Toast.MakeText(s, Toast.LENGTH_SHORT).Show() },
            {"Log", s => Console.WriteLine(s) }
        };
    }
}