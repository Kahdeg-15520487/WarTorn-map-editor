using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace doan
{
    static class TileType
    {
        public enum Terrain
        {
            Void,
            Mountain,
            Forest,
            Water,
            Grass
        }
        public enum Building
        {
            Void,
            City,
            Road,
            RoadIntersection4,
            RoadIntersection3,
            RoadTurn,
            Barricade
        }
    }

    static class Layer
    {
        public const float
            Terrain = 0.0f,
            Building = 0.1f,
            Unit = 0.2f,
            Gui = 0.3f;

    }

    enum MouseButtonState
    {
        Mouse1,
        Mouse2,
        Mouse3,
        Void,
        Release
    }

    static class SpriteRect
    {
        public static Rectangle Void = new Rectangle(0, 96, 32, 32);
        public static Rectangle Selected = new Rectangle(0, 128, 32, 32);
        public static Rectangle RotateRight = new Rectangle(32, 128, 32, 32);
        public static Rectangle RotateLeft = new Rectangle(64, 128, 32, 32);
        public static Rectangle Blank = new Rectangle(0, 160, 32, 32);

        public static Rectangle Forest = new Rectangle(0, 64, 32, 32);
        public static Rectangle Grass = new Rectangle(32, 64, 32, 32);
        public static Rectangle Mountain = new Rectangle(64, 64, 32, 32);
        public static Rectangle Water = new Rectangle(96, 64, 32, 32);

        //public static Rectangle Road = new Rectangle(0, 0, 32, 32);
        //public static Rectangle RoadIntersection4 = new Rectangle(32, 0, 32, 32);
        //public static Rectangle RoadIntersection3 = new Rectangle(64, 0, 32, 32);
        public static Rectangle RoadTurn = new Rectangle(96, 0, 32, 32);

        //public static Rectangle City = new Rectangle(0, 32, 32, 32);
        public static Rectangle Barricade = new Rectangle(0, 0, 32, 32);

        public static Rectangle City = new Rectangle(0, 0, 60, 60);
        public static Rectangle Road = new Rectangle(60, 0, 60, 60);
        public static Rectangle RoadIntersection4 = new Rectangle(120, 0, 60, 60);
        public static Rectangle RoadIntersection3 = new Rectangle(180, 0, 60, 60);
    }
    
    static class GuiRect
    {
        public static Rectangle GuiBoundary;
        public static Rectangle MapBoundary;

        public enum GuiButton
        {
            Void,Delete,Save,RotateRight,RotateLeft,Forest,Grass,Mountain,Water,Road,RoadIntersection4,RoadIntersection3,RoadTurn,City,Barricade
        }

        public static Dictionary<GuiButton, Rectangle> GuiButtonRect = new Dictionary<GuiButton, Rectangle>();

        public static void Initialize()
        {
            GuiBoundary = new Rectangle(64, 10, 432, 32);
            MapBoundary = new Rectangle(90, 90, 792, 410);

            int offset = 32;

            GuiButtonRect.Add(GuiButton.Delete,new Rectangle(offset+=32, 10, 32, 32));
            GuiButtonRect.Add(GuiButton.Save,new Rectangle(offset += 32, 10, 32, 32));
            GuiButtonRect.Add(GuiButton.RotateRight, new Rectangle(offset += 32, 10, 32, 32));
            GuiButtonRect.Add(GuiButton.RotateLeft, new Rectangle(offset += 32, 10, 32, 32));
            offset += 16;
            GuiButtonRect.Add(GuiButton.Forest,new Rectangle(offset += 32, 10, 32, 32));
            GuiButtonRect.Add(GuiButton.Grass,new Rectangle(offset += 32, 10, 32, 32));
            GuiButtonRect.Add(GuiButton.Mountain,new Rectangle(offset += 32, 10, 32, 32));
            GuiButtonRect.Add(GuiButton.Water,new Rectangle(offset += 32, 10, 32, 32));
            offset += 16;
            GuiButtonRect.Add(GuiButton.Road,new Rectangle(offset += 32, 10, 32, 32));
            GuiButtonRect.Add(GuiButton.RoadIntersection4,new Rectangle(offset += 32, 10, 32, 32));
            GuiButtonRect.Add(GuiButton.RoadIntersection3,new Rectangle(offset += 32, 10, 32, 32));
            GuiButtonRect.Add(GuiButton.RoadTurn,new Rectangle(offset += 32, 10, 32, 32));
            offset += 16;
            GuiButtonRect.Add(GuiButton.City,new Rectangle(offset += 32, 10, 32, 32));
            GuiButtonRect.Add(GuiButton.Barricade,new Rectangle(offset += 32, 10, 32, 32));

            GuiBoundary.Width = offset;
        }
    }
}
