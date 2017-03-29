using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace doan
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    class GameManager : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Map map;
        MouseHandler mouseHandler;
        GuiEditor guieditor;
        
        SpriteFont defaultfont;

        KeyboardState previousKeyboardState = new KeyboardState();
        KeyboardState keyboardState = new KeyboardState();

        MouseState previousMouseState = new MouseState();
        MouseState mouseState = new MouseState();

        public GameManager()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = Setting.Width;
            graphics.PreferredBackBufferHeight = Setting.Height;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            {
                mouseHandler = new MouseHandler();
                mouseHandler.Initialize();

                GuiRect.Initialize();
                guieditor = new GuiEditor();
                guieditor.AddEvent(mouseHandler);

                map = new Map();
                map.Initialize(guieditor);
                map.AddEvent(mouseHandler);
                map.test(); //hardcoded map to test
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            defaultfont = Content.Load<SpriteFont>("defaultfont");

            map.LoadContent(Content);
            mouseHandler.SetFont(defaultfont);
            guieditor.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            {
                map.Update(gameTime);
                mouseHandler.Update();
            }

            {
                keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyUp(Keys.E) && previousKeyboardState.IsKeyDown(Keys.E))
                {
                    guieditor.IsEditorMode = !guieditor.IsEditorMode;
                    map.IsEditorMode = guieditor.IsEditorMode;
                }

                previousKeyboardState = keyboardState;
            }

            base.Update(gameTime);
        }

        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            {
                map.Draw(spriteBatch);
                mouseHandler.Draw(spriteBatch);
                guieditor.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }

    class InputWrapper
    {
        KeyboardState keyboardState;
        KeyboardState previousKeyboardState;
        MouseState mouseState;
        MouseState previousMouseState;

        public InputWrapper()
        {
            keyboardState = new KeyboardState();
            previousKeyboardState = new KeyboardState();
            mouseState = new MouseState();
            previousMouseState = new MouseState();
        }

        public InputWrapper(KeyboardState kbstate,KeyboardState prekbstate,MouseState mstate,MouseState premstate)
        {
            keyboardState = kbstate;
            previousKeyboardState = prekbstate;
            mouseState = mstate;
            previousMouseState = premstate;
        }
    }
}
