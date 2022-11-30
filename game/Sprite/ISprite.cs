using Raylib_cs; //REEEE C# already has a Rectangle class, why did Raylib have to create their own version??
using System.Collections.Generic;
using System.Numerics;


namespace game{
    interface ISprite{
        Rectangle getBoundingBox();
        /// <summary>
        ///  The screen is a rectangle which describes the camera transform:
        ///  The camera's position and dimentions are relative to the positions in Box2D, in meters.
        /// </summary>
        void Render(Rectangle screen, int windowWidth, int windowHeight);

        void Update(ICollection<ISprite> sprites, UpdateHandler handler, double updateTime);

        Vector2 getPosition();

        string getName();

        /// <summary>
        ///  modifies obj to be translated into screen space from Box2D space.
        ///  Rectangle screen: a rectangle which describes the camera transform:
        ///  The camera's position and dimentions are relative to the positions in Box2D, in meters.
        /// </summary>
        public static void transformObject(ref Rectangle obj, Rectangle screen, int windowWidth, int windowHeight){
            //obj is in meter space (Box2D)
            //using the camera transform screen, we can turn that into pixel space

            //step one: translation to camera
            obj.x -= screen.x; //invert the screen coordinate, since we're translating it *into* screen space, not further away from it.
            obj.y -= screen.y;
            //step two: scale to screen and camera
            obj.x *= windowWidth/screen.width;
            obj.y *= windowHeight/screen.height;
            obj.width *= windowWidth/screen.width;
            obj.height *= windowHeight/screen.height;
            //the object is now translated, we can exit the function. obj has been translated, so we're done.
        }

        public static Vector2 getWorldPos(Vector2 windowPos){
            //the screen transform, but inverted.
            windowPos *= Program.screen.width/Program.width; //multiplying by reciprocal is equivalent to dividing and is more efficient (usually)
            windowPos.X += Program.screen.x;
            windowPos.Y += Program.screen.y;
            return windowPos;
        }

        public static string recStr(Rectangle r){
            return "{" + r.x + ", " + r.y + ", " + r.width + ", " + r.height + "}";
        }
    }
}