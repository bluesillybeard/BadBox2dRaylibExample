using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;

namespace game{
    class PlayerSprite : ISprite{
        private Rectangle box; //the bounding box of this sprite
        private Color color; //the color of this sprite
        private double lastUpdate;//used for smooth animation frames between updates
        private Rectangle previousBox; //used for smooth animation frames between updates
        private Rectangle temp;
        private Body body;
        public PlayerSprite(Rectangle box, Color color, World simWorld, BodyDef tempBD){
            this.box = box;
            this.previousBox = new Rectangle(box.x, box.y, box.width, box.height);
            this.color = color;

            

            tempBD.position = new Vector2(box.x, box.y);
            tempBD.type = BodyType.Dynamic; //dynamic body (player does in fact move around)
            tempBD.bullet = false; //default
            tempBD.allowSleep = true; //default
            tempBD.fixedRotation = true; //no rotation
            this.body = simWorld.CreateBody(tempBD);

            PolygonShape polyBox = new PolygonShape(box.width, box.height);

            FixtureDef fixture = new FixtureDef();
            fixture.shape = polyBox;
            fixture.density = 1f;
            fixture.friction = 0.1f;
            body.CreateFixture(fixture);
        }

        public Rectangle getBoundingBox(){
            return box;
        }

        public void Render(Rectangle screen, int windowWidth, int windowHeight){   
            //determine the position this frame using linear interpolation
            float endWeight = (float)((Raylib.GetTime() - lastUpdate)/Program.updatePeriod);
            temp.x = (box.x*endWeight + previousBox.x*(1 - endWeight));
            temp.y = (box.y*endWeight + previousBox.y*(1 - endWeight));
            temp.width = box.width;
            temp.height = box.height;
            //now translate and scale it based on the screen transform and the height and width of the window.
            //ISprite can do that for us though.
            ISprite.transformObject(ref temp, screen, windowWidth, windowHeight);
            Raylib.DrawRectanglePro(temp, new Vector2(0, 0), 0, color);
        }

        public void Update(ICollection<ISprite> sprites, UpdateHandler handler, double time){
            handler.ScheduleNextUpdate(this); //make sure this sprite gets updated each tick
            lastUpdate = time;
            
            previousBox.x = box.x; //update previous position
            previousBox.y = box.y;

            if(Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON)) body.ApplyForceToCenter((ISprite.getWorldPos(Raylib.GetMousePosition()) - new Vector2(box.x, box.y))*8);
            //if the left mouse button is down, apply a force towards it.
            body.SetLinearDampling(1);
            Vector2 pos = body.GetPosition();
            box.x = pos.X;
            box.y = pos.Y;
        }

        public Vector2 getPosition(){
            return new Vector2(box.x, box.y); //todo: temporary variable
        }

        public string getName(){
            return "playerSprite";
        }
    }
}