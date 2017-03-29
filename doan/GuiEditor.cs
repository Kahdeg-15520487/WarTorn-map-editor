using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace doan
{
    class GuiEditor
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

        Tile currTile = new Tile(TileType.Terrain.Void, TileType.Building.Void, new Vector2(10, 500), 0f);
        public Tile CurrTile
        {
            get
            {
                return currTile;
            }
        }

        GuiRect.GuiButton lastbutton = GuiRect.GuiButton.Void;

        StringBuilder log = new StringBuilder();
        private Texture2D temp_sprite_sheet;

        public GuiEditor()
        {

        }

        public void Initialize()
        {

        }

        public void AddEvent(MouseHandler mousehandler)
        {
            mousehandler.AddEvent(MouseEvent.Mouse1Pressed(GuiRect.GuiBoundary), Interact);
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager contentmanager)
        {
            defaultfont = contentmanager.Load<SpriteFont>("defaultfont");
            sprite_sheet = contentmanager.Load<Texture2D>("sprite\\sprite_sheet");
            temp_sprite_sheet = contentmanager.Load<Texture2D>("sprite\\temp_sprite_sheet");
        }

        public void Interact(Vector2 pos)
        {
            //log.Clear();
            if (isEditorMode)
            {
                foreach (var key in GuiRect.GuiButtonRect.Keys)
                {
                    if (GuiRect.GuiButtonRect[key].Contains(pos))
                    {
                        //log.Append(key.ToString());
                        //log.Append('\n');
                        switch (key)
                        {
                            case GuiRect.GuiButton.Delete:
                                break;
                            case GuiRect.GuiButton.Save:
                                break;
                            case GuiRect.GuiButton.RotateRight:
                                currTile.orientation += 90f;
                                break;
                            case GuiRect.GuiButton.RotateLeft:
                                currTile.orientation -= 90f;
                                break;

                            case GuiRect.GuiButton.Forest:
                                currTile.primarytile = TileType.Terrain.Forest;
                                break;
                            case GuiRect.GuiButton.Grass:
                                currTile.primarytile = TileType.Terrain.Grass;
                                break;
                            case GuiRect.GuiButton.Mountain:
                                currTile.primarytile = TileType.Terrain.Mountain;
                                break;
                            case GuiRect.GuiButton.Water:
                                currTile.primarytile = TileType.Terrain.Water;
                                break;

                            case GuiRect.GuiButton.Road:
                                currTile.secondarytile = TileType.Building.Road;
                                break;
                            case GuiRect.GuiButton.RoadIntersection4:
                                currTile.secondarytile = TileType.Building.RoadIntersection4;
                                break;
                            case GuiRect.GuiButton.RoadIntersection3:
                                currTile.secondarytile = TileType.Building.RoadIntersection3;
                                break;
                            case GuiRect.GuiButton.RoadTurn:
                                currTile.secondarytile = TileType.Building.RoadTurn;
                                break;

                            case GuiRect.GuiButton.City:
                                currTile.secondarytile = TileType.Building.City;
                                break;
                            case GuiRect.GuiButton.Barricade:
                                break;
                            default:
                                break;
                        }
                        lastbutton = key;
                        return;
                    }
                }
            }
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(defaultfont, log, new Vector2(50, 50), Color.White);

            //draw the tile indicator at bottom left of the screen
            Rectangle temp = SpriteRect.Void;
            {
                switch (currTile.primarytile)
                {
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
                spriteBatch.Draw(sprite_sheet, currTile.position+ Tile.centerOrigin, temp, Color.White, Utility.DegreeToRadian(currTile.orientation), Tile.centerOrigin, 1f, SpriteEffects.None, Layer.Gui - 0.1f);

                temp = SpriteRect.Void;
                switch (currTile.secondarytile)
                {
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
                        //temp = SpriteRect.RoadTurn;
                        break;
                    case TileType.Building.Barricade:
                        break;
                    default:
                        break;
                }
                spriteBatch.Draw(temp_sprite_sheet, currTile.position + Tile.centerOrigin, temp, Color.White, Utility.DegreeToRadian(currTile.orientation), Tile.centerOrigin, 1f, SpriteEffects.None, Layer.Gui);
            }

            //draw gui editor button
            if (isEditorMode)
            {
                temp = SpriteRect.Blank;
                foreach (var key in GuiRect.GuiButtonRect.Keys)
                {
                    switch (key)
                    {
                        case GuiRect.GuiButton.Delete:
                            break;
                        case GuiRect.GuiButton.Save:
                            break;
                        case GuiRect.GuiButton.RotateRight:
                            temp = SpriteRect.RotateRight;
                            break;
                        case GuiRect.GuiButton.RotateLeft:
                            temp = SpriteRect.RotateLeft;
                            break;

                        case GuiRect.GuiButton.Forest:
                            temp = SpriteRect.Forest;
                            break;
                        case GuiRect.GuiButton.Grass:
                            temp = SpriteRect.Grass;
                            break;
                        case GuiRect.GuiButton.Mountain:
                            temp = SpriteRect.Mountain;
                            break;
                        case GuiRect.GuiButton.Water:
                            temp = SpriteRect.Water;
                            break;                        

                        case GuiRect.GuiButton.RoadTurn:
                            temp = SpriteRect.RoadTurn;
                            break;

                        case GuiRect.GuiButton.Barricade:
                            break;
                        default:
                            break;
                    }
                    spriteBatch.Draw(sprite_sheet, new Vector2(GuiRect.GuiButtonRect[key].X, GuiRect.GuiButtonRect[key].Y), temp, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layer.Gui);
                    switch (key)
                    {
                        case GuiRect.GuiButton.Road:
                            break;
                        case GuiRect.GuiButton.RoadIntersection4:
                            break;
                        case GuiRect.GuiButton.RoadIntersection3:
                            break;
                        case GuiRect.GuiButton.City:
                        default:
                            break;
                    }
                    spriteBatch.Draw(temp_sprite_sheet, new Vector2(GuiRect.GuiButtonRect[key].X, GuiRect.GuiButtonRect[key].Y), temp, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layer.Gui);
                }

                //draw the selected sprite
                if (lastbutton != GuiRect.GuiButton.Void)
                {
                    temp = GuiRect.GuiButtonRect[lastbutton];
                    spriteBatch.Draw(sprite_sheet, new Vector2(temp.X, temp.Y), SpriteRect.Selected, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layer.Gui);
                }
            }
        }
    }
}
