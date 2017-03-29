using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace doan
{
    class Map
    {
        bool isEditorMode = false;
        public bool IsEditorMode
        {
            get
            {
                return isEditorMode;
            }
            set
            {
                isEditorMode = value != isEditorMode ? value : isEditorMode;
            }
        }

        SpriteFont defaultfont;

        Texture2D sprite_sheet;
        GuiEditor referenceGuiEditor;

        int MapHeight = 13;
        int MapWidth = 25;

        List<Tile> MapTile;

        Vector2 origin;
        Vector2 offset = new Vector2(100, 100);

        Tile curr_selected = null;

        StringBuilder log = new StringBuilder();
        private Texture2D temp_sprite_sheet;

        /// <summary>
        /// constructor
        /// </summary>
        public Map()
        {
            origin = new Vector2(16,16);
        }

        public void Initialize(GuiEditor guieditor)
        {
            referenceGuiEditor = guieditor;
        }

        public void AddEvent(MouseHandler mousehandler)
        {
            mousehandler.AddEvent(MouseEvent.Mouse1Pressed(GuiRect.MapBoundary), Edit);
        }

        /// <summary>
        /// load content from disk
        /// </summary>
        /// <param name="contentmanager"></param>
        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager contentmanager)
        {
            defaultfont = contentmanager.Load<SpriteFont>("defaultfont");
            sprite_sheet = contentmanager.Load<Texture2D>("sprite\\sprite_sheet");
            temp_sprite_sheet = contentmanager.Load<Texture2D>("sprite\\temp_sprite_sheet");
        }

        /// <summary>
        /// load the map with name
        /// </summary>
        /// <param name="mapname">the name of map to load</param>
        void LoadMap(string mapname)
        {
            using (StreamReader file = File.OpenText(mapname))
            {
                MapTile = JsonConvert.DeserializeObject<List<Tile>>(file.ReadToEnd());
            }
        }

        /// <summary>
        /// create a dummy map to test
        /// </summary>
        public void test()
        {
            MapTile = new List<Tile>();

            //randomized map crate
            /*
            Array terrain = Enum.GetValues(typeof(TileType.Terrain));
            Array building = Enum.GetValues(typeof(TileType.Building));
            Random random = new Random();
            for (int i=0;i<25;i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    MapTile.Add(new Tile((TileType.Terrain)terrain.GetValue(random.Next(terrain.Length-1)+1), (TileType.Building)building.GetValue(random.Next(building.Length-1)+1),new Vector2(i*32,j*32),0f));
                }
            }
            //*/

            //fill the map with grass
            //*
            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    MapTile.Add(new Tile(TileType.Terrain.Grass, new Vector2(i * 32, j * 32), 0f));
                }
            }
            //*/

            SaveMap("test");
        }

        /// <summary>  
        ///  auto connect the road using approriate sprite,
        ///  only run when save map
        /// </summary>  
        void RoadConnector()
        {
            List<Tile> done = new List<Tile>();
            float orient = 0f;
            Tile curr_tile;
            List<Tile> surround;
            Vector2 up = new Vector2(0,-32);
            Vector2 down = new Vector2(0, 32);
            Vector2 left = new Vector2(-32, 0);
            Vector2 right = new Vector2(32, 0);
            foreach (var tile in MapTile)
            {
                if (tile.secondarytile == TileType.Building.Road)
                {
                    done.Add(tile);
                    curr_tile = MapTile.Find(x => x.position == tile.position);
                    surround = new List<Tile>();
                    surround.Add(MapTile.Find(x => x.position == tile.position + up));
                    surround.Add(MapTile.Find(x => x.position == tile.position + down));
                    surround.Add(MapTile.Find(x => x.position == tile.position + left));
                    surround.Add(MapTile.Find(x => x.position == tile.position + right));

                    switch (surround.CountNull())
                    {
                        case 0:
                            MapTile.Find(x => x.position == tile.position).secondarytile = TileType.Building.Void;
                            break;
                        case 1:
                            switch (surround.FindIndex(y => y.position.X != -1 && y.position.Y != -1 && y.primarytile != TileType.Terrain.Void && y.secondarytile != TileType.Building.Void))
                            {
                                case 0:
                                case 1:
                                    break;

                                case 2:
                                case 3:
                                    orient = 90f;
                                    break;                                    
                                default:
                                    break;
                            }
                            break;
                        case 2:

                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        default:
                            break;
                    }
                    curr_tile.orientation = orient;
                }
            }
        }

        /// <summary>
        /// save the current running map
        /// </summary>
        /// <param name="mapname">name of the map to save as</param>
        void SaveMap(string mapname)
        {
            using (StreamWriter file = File.CreateText(mapname))
            {
                file.Write(JsonConvert.SerializeObject(MapTile));
            }
        }

        /// <summary>
        /// handle game update
        /// </summary>
        /// <param name="gametime"></param>
        public void Update(GameTime gametime)
        {

        }

        /// <summary>
        /// handle input action from mouse. do not call as this function act on event
        /// </summary>
        /// <param name="pos">position of the action on screen</param>
        public void Edit(Vector2 pos)
        {
            Vector2 position = pos - offset;
            if ((position.X % 32 != 0) || (position.Y % 32 != 0))
                position = SnapToGrid(position);

            if (!MapTile.Contains(position))
            {
                if (isEditorMode)
                    MapTile.Add(new Tile(TileType.Terrain.Grass, position, 0f));
            }
            else
            {
                curr_selected = new Tile(MapTile.Find(x => x.position == position));
                if (isEditorMode)
                {
                    log.Clear();
                    log.Append(referenceGuiEditor.CurrTile.ToString());
                    EditTile(position, referenceGuiEditor.CurrTile);
                }
            }
        }

        /// <summary>
        /// Edit the tile at pos on the map. do not call as this function act on event
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="tile"></param>
        private void EditTile(Vector2 pos,Tile tile)
        {
            if ((pos.X % 32 != 0) || (pos.Y % 32 != 0))
                return;
            if (tile.primarytile == TileType.Terrain.Void && tile.secondarytile == TileType.Building.Void)
                return;
            if (MapTile.Contains(pos))
            {
                MapTile.Find(x => x.position == pos).Change(tile);
            }
        }

        /// <summary>
        /// handle drawing of sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(defaultfont, log, new Vector2(120,520), Color.White,0f,origin,1f,SpriteEffects.None,Layer.Gui);

            Rectangle temp = SpriteRect.Void;
            foreach (var tile in MapTile)
            {
                switch (tile.primarytile)
                {
                    case TileType.Terrain.Void:
                        break;
                    case TileType.Terrain.Mountain:
                        temp = SpriteRect.Mountain;
                        break;
                    case TileType.Terrain.Forest:
                        temp = SpriteRect.Forest;
                        break;
                    case TileType.Terrain.Water:
                        temp = SpriteRect.Water;
                        break;
                    case TileType.Terrain.Grass:
                        temp = SpriteRect.Grass;
                        break;
                    default:
                        break;
                }
                spriteBatch.Draw(sprite_sheet, offset + tile.position, temp, Color.White, Utility.DegreeToRadian(tile.orientation), origin, 1f, SpriteEffects.None, Layer.Terrain);

                temp = SpriteRect.Void;
                switch (tile.secondarytile)
                {
                    case TileType.Building.Void:
                        break;
                    case TileType.Building.City:
                        temp = SpriteRect.City;
                        break;
                    case TileType.Building.Road:
                        temp = SpriteRect.Road;
                        break;
                    case TileType.Building.RoadIntersection4:
                        temp = SpriteRect.RoadIntersection4;
                        break;
                    case TileType.Building.RoadIntersection3:
                        temp = SpriteRect.RoadIntersection3;
                        break;
                    case TileType.Building.RoadTurn:
                        temp = SpriteRect.RoadTurn;
                        break;
                    case TileType.Building.Barricade:
                        break;
                    default:
                        break;
                }
                spriteBatch.Draw(temp_sprite_sheet, offset + tile.position, temp, Color.White, Utility.DegreeToRadian(tile.orientation), origin, 1f, SpriteEffects.None, Layer.Building);
            }
            if (curr_selected != null)
            {
                spriteBatch.Draw(sprite_sheet, offset + curr_selected.position, SpriteRect.Selected, Color.White, 0f, origin, 1f, SpriteEffects.None, Layer.Gui);
                StringBuilder tempp = new StringBuilder();
                tempp.Append(curr_selected.primarytile.ToString());
                tempp.Append('\n');
                tempp.Append(curr_selected.secondarytile.ToString());
                spriteBatch.DrawString(defaultfont, tempp, new Vector2(10, Setting.Height - 50), Color.White);
            }
        }

        /// <summary>
        /// snap the input vector2 to approriate square on a grid
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        private Vector2 SnapToGrid(Vector2 pos, int factor = 32)
        {
            return new Vector2((int)Math.Round(((int)pos.X / (double)factor), MidpointRounding.AwayFromZero) * factor, (int)Math.Round(((int)pos.Y / (double)factor), MidpointRounding.AwayFromZero) * factor);
        }
    }

    class Tile
    {
        public TileType.Terrain primarytile;
        public TileType.Building secondarytile;
        public Vector2 position;
        public float orientation;

        public static Vector2 centerOrigin
        {
            get
            {
                return new Vector2(16, 16);
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="Tile">the terrain tile</param>
        /// <param name="Pos"></param>
        /// <param name="Orient"></param>
        public Tile(TileType.Terrain Tile,Vector2 Pos, float Orient)
        {
            primarytile = Tile;
            secondarytile = TileType.Building.Void;
            position = Pos;
            orientation = Orient;
        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="PrimaryTile">the terrain tile</param>
        /// <param name="SecondaryTile">the building tile</param>
        /// <param name="Pos"></param>
        /// <param name="Orient"></param>
        public Tile(TileType.Terrain PrimaryTile,TileType.Building SecondaryTile, Vector2 Pos, float Orient)
        {
            primarytile = PrimaryTile;
            secondarytile = SecondaryTile;
            position = Pos;
            orientation = Orient;
        }
        public Tile()
        {
            primarytile = TileType.Terrain.Void;
            secondarytile = TileType.Building.Void;
            position = new Vector2(-1, -1);
            orientation = 0f;
        }
        public Tile(Tile other)
        {
            primarytile = other.primarytile;
            secondarytile = other.secondarytile;
            position = other.position;
            orientation = other.orientation;
        }

        public void Change(Tile other)
        {
            if (GetHashCode() == other.GetHashCode())
                return;
            primarytile = other.primarytile;
            secondarytile = other.secondarytile;
            orientation = other.orientation;
        }

        public override int GetHashCode()
        {
            int hash = 60;
            hash = (hash * 199) + (int)primarytile;
            hash = (hash * 199) + (int)secondarytile;
            hash = (hash * 199) + position.GetHashCode();
            hash = (hash * 199) + orientation.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.Append(primarytile.ToString());
            output.Append('\n');
            output.Append(secondarytile.ToString());
            output.Append('\n');
            output.Append(position.ToString());
            output.Append('\n');
            output.Append(orientation);
            return output.ToString();
        }
    }

    static class MyExtensions
    {
        public static bool Contains(this List<Tile> list, Vector2 pos)
        {
            foreach (var item in list)
            {
                if (item.position == pos)
                {
                    return true;
                }
            }
            return false;
        }

        public static int CountNull(this List<Tile> list)
        {
            int count = 0;
            Vector2 pos = new Vector2(-1,-1);
            foreach (var item in list)
            {
                if ((item.position == pos) &&
                    (item.primarytile == TileType.Terrain.Void) &&
                    (item.secondarytile == TileType.Building.Void))
                {
                    count++;
                }
            }
            return count;
        }
    }
}
