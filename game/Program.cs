using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;


//{
//    Rectangle r = new Rectangle(77, 206, 19, 23);
//    game.ISprite.transformObject(r, new Rectangle(63, 181, 77, 71), 800, 600);
//    System.Console.WriteLine(game.ISprite.recStr(r));
//}

namespace game
{
    
    class Program
    {
        public const float updatePeriod = 1.0f/30.0f;
        
        public static int width = 800;
        public static int height = 600;
        public static Vector2 zero;

        private static UpdateHandler handler;
        private static List<ISprite> sprites;

        public static Rectangle screen;
        static void Main(string[] args)
        {
            Raylib.InitWindow(800, 600, "some kind of game I guess");
            Raylib.SetTargetFPS(60);
            handler = new UpdateHandler();
            sprites = new List<ISprite>();
            zero = new Vector2(0, 0);
            screen = new Rectangle(-50, 0, 100*((float)width/(float)height), 100);

            World world = new World(new Vector2(0, 0));
            BodyDef body = new BodyDef();
            FixtureDef fixture = new FixtureDef();

            sprites.Add(new PlayerSprite(new Rectangle(2, 2, 0.7f, 1.7f), Color.WHITE, world, body));
            sprites.Add(new ObstacleSprite(new Rectangle(1, 8, 4f, 0.25f), Color.RED, world, body));

            handler.ScheduleNextUpdate(sprites); //schedule all of the sprites for next update
            double lastUpdate = Raylib.GetTime();
            while(!Raylib.WindowShouldClose()){
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLANK);
                foreach(ISprite sprite in sprites){
                    sprite.Render(screen, width, height);
                }
                Raylib.EndDrawing();

                if(Raylib.GetTime() - lastUpdate > updatePeriod){
                    lastUpdate = Raylib.GetTime();
                    handler.Update(sprites, lastUpdate);
                    world.Step(updatePeriod, 8, 4);

                    //camera controls (very basic lol)
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_LEFT)){
                        screen.x -= 0.5f;
                    }
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT)){
                        screen.x += 0.5f;
                    }
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_UP)){
                        screen.y -= 0.5f;
                    }
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_DOWN)){
                        screen.y += 0.5f;
                    }
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_BRACKET)){
                        screen.width *= 0.9f;
                        screen.height *= 0.9f;

                    }
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_BRACKET)){
                        screen.width *= 1.1f;
                        screen.height *= 1.1f;                    
                    }
                }


            }
        }
    }
/*
    class Program{

        const float TimeStep = 1.0f/20.0f;
        const int VelocityIterations = 6;
        const int PositionIterations = 2;
        static void Main(string[] args){
            //create the fabric of realioty itself
            World world = new World(new Vector2(0, -10));

            //create the body definition, which is exactly not what it sounds like
            BodyDef groundBodyDef = new BodyDef();
            groundBodyDef.position = new Vector2(0, -10);

            //create the body
            Body groundBody = world.CreateBody(groundBodyDef);

            //create the shape
            PolygonShape groundBox = new PolygonShape(50.0f, 10.0f);

            //attach the body and the shape.
            groundBody.CreateFixture(groundBox);


            //create a dynamic body, using a similar process as above
            BodyDef bodyDef = new BodyDef();
            bodyDef.type = BodyType.Dynamic;
            bodyDef.position = new Vector2(0.0f, 4.0f);
            Body body = world.CreateBody(bodyDef);

            PolygonShape dynamicBox = new PolygonShape(1.0f, 1.0f);

            FixtureDef fixtureDef = new FixtureDef();
            fixtureDef.shape = dynamicBox;
            fixtureDef.density = 1.0f;
            fixtureDef.friction = 0.3f;

            body.CreateFixture(fixtureDef);

            //actually simulate it:
            for (uint i = 0; i < 60; ++i){
                    world.Step(TimeStep, VelocityIterations, PositionIterations);
                    Vector2 position = body.GetPosition();
                    float angle = body.GetAngle();
                        System.Console.WriteLine("{0:F} {1:F} {2:F}\n", position.X, position.Y, angle);
            }

        }
    }
    */
}
