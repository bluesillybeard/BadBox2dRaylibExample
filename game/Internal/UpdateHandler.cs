using System.Collections.Generic;

namespace game{
    class UpdateHandler{

        public UpdateHandler(){
            current = new Queue<ISprite>();
            next = new Queue<ISprite>();
        }
        private Queue<ISprite> current;
        private Queue<ISprite> next;

        /*
        schedules a sprite to be updated in the current update. A sprite updating itself infinitely in the same update cycle is generally a bad idea.
        Can be used for instantly propagating things, like how redstone behaves in Minecraft.
        */
        public void ScheduleCurrentUpdate(ISprite sprite){
            current.Enqueue(sprite);
        }

        /*
        schedules a list of sprites to be updated in the current update. A sprite updating itself infinitely in the same update cycle is generally a bad idea.
        Can be used for instantly propagating things, like how redstone behaves in Minecraft.
        */
        public void ScheduleCurrentUpdate(ICollection<ISprite> sprites){
            foreach(ISprite sprite in sprites){
                ScheduleCurrentUpdate(sprite);
            }
        }

        /*
        schedules a sprite to be updated in the next update;
        */
        public void ScheduleNextUpdate(ISprite sprite){
            next.Enqueue(sprite);
        }

                /*
        schedules a list of sprites to be updated in the next update;
        */
        public void ScheduleNextUpdate(ICollection<ISprite> sprites){
            foreach(ISprite sprite in sprites){
                ScheduleCurrentUpdate(sprite);
            }
        }

        /*
        updates everything in the queue. Sprites are responsible for scheduling their own updates; if one does not then it will no longer be updated.
        Automatically updates 
        */
        public int Update(ICollection<ISprite> globalSprites, double updateTime){
            //keep track of how many updates were done
            int numUpdates = 0;
            while(current.Count > 0){
                current.Dequeue().Update(globalSprites, this, updateTime);
                numUpdates++;
            }
            //swap current and next
            Queue<ISprite> temp = current;
            current = next;
            next = temp;
            return numUpdates;
        }
    }
}