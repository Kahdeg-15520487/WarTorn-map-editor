using System;
using STA.INIfile;
using System.Collections.Generic;

namespace doan
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Setting.Width = 1000;
            Setting.Height = 600;
            using (var game = new GameManager())
                game.Run();
        }

        //TODO: load and save setting
        //  INIFile MyINIFile = new INIFile(<filepath>);
        //  int Value1 = MyINIFile.GetValue("Section1","Value1",0);
        //  bool Value2 = MyINIFile.GetValue("Section1", "Value2", false);
        //  double Value3 = MyINIFile.GetValue("Section1", "Value3", (double)0);
        //  byte[] Value4 = MyINIFile.GetValue("Section1", "Value4", (byte[])null);
        //  MyINIFile.SetValue("Section1","Value1", Value1);
        //	MyINIFile.SetValue("Section1","Value2", Value2);
        //	MyINIFile.SetValue("Section1","Value3", Value3);
        //	MyINIFile.SetValue("Section1","Value4", Value4);
        //  MyINIFile.Flush();
    }

    public static class Setting
    {
        private static int width = 0;
        private static int height = 0;

        public static int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }
        
        public static int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }
    }
}