using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;

namespace game{
    class ObstacleSprite : ISprite{
        private Rectangle box; //the bounding box of this sprite
        private Color color; //the color of this sprite
        private Rectangle temp;
        private Body body;
        public ObstacleSprite(Rectangle box, Color color, World simWorld, BodyDef tempBD){
            this.box = box;
            this.color = color;

            tempBD.position = new Vector2(box.x, box.y);
            tempBD.type = BodyType.Static; //static body (platforms don't move)
            tempBD.bullet = false; //default
            tempBD.allowSleep = true; //default
            tempBD.fixedRotation = false;
            this.body = simWorld.CreateBody(tempBD);

            PolygonShape polyBox = new PolygonShape(box.width, box.height);
            FixtureDef fixture = new FixtureDef();
            fixture.shape = polyBox;
            body.CreateFixture(fixture);
        }

        public Rectangle getBoundingBox(){
            return box;
        }

        public void Render(Rectangle screen, int windowWidth, int windowHeight){   
            //translate and scale it based on the screen transform and the height and width of the window.
            //ISprite can do that for us though.
            temp.x = box.x;
            temp.y = box.y;
            temp.width = box.width;
            temp.height = box.height;
            ISprite.transformObject(ref temp, screen, windowWidth, windowHeight);
            Raylib.DrawRectanglePro(temp, new Vector2(0, 0), 0, color);
        }

        public void Update(ICollection<ISprite> sprites, UpdateHandler handler, double time){
            //handler.ScheduleNextUpdate(this); //make sure this sprite gets updated each tick
            //Vector2 pos = body.GetPosition();
            //box.x = pos.X;
            //box.y = pos.Y;
        }

        public Vector2 getPosition(){
            return new Vector2(box.x, box.y); //todo: temporary variable
        }

        public string getName(){
            return "playerSprite";
        }
    }
}