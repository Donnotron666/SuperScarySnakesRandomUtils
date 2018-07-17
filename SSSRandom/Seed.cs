using UnityEngine;
using System.Collections.Generic;
using System;

namespace SSSRandom
{
    //A useful class for doing random operations on lists of objects.
	public class Seed
	{
		MersenneTwister Twister;
        public int SeedValue;
		public Seed (int seedValue) {
            this.SeedValue = seedValue;
			Twister = new MersenneTwister(seedValue);
		}

        public Seed(Seed parentSeed) : this(parentSeed.SeedValue) { }

        public Seed() : this((int)(DateTime.UtcNow.Ticks & uint.MaxValue)) { }

        
		public float NextFloat()
        {
			return Twister.GetFloat();
		}



		public bool RollAgainst(float ltOrEqualTo)
        {
			return NextFloat() <= ltOrEqualTo;
		}

		public float InRange(float min, float max)
        {
			return Twister.GetFloat(min, max);
		}

        public float Next(float max)
        {
            return InRange(0, max);
        }

            
		public int Next(int max)
        {
			return max > 0? Twister.GetInt(0, max - 1) : 0;
		}
        

        public void Advance(int numTimes = 1)
        {
            for( int i = 0; i< numTimes; i++)
            {
                NextFloat();
            }
        }

		public List<T> TakeFromList<T>(List<T> items, int num)
        {
			Dictionary<T, bool> taken = new Dictionary<T, bool>();

            num = num > items.Count ? items.Count : num;

			var buff = new List<T>();
			while (num > 0) {
				var item = RandomInList(items);
				if(item == null)
					return buff;
				
				if(!taken.ContainsKey(item)){
					buff.Add(item);
                    taken[item] = true;
					num--;
				}

			}

			return buff;
		}

        public List<T> RandomizeList<T>(List<T> items)
        {
            for( int i = 0; i< items.Count; i++)
            {
                int swapIndex = this.Next(items.Count - 1);
                var swapElement = items[swapIndex];
                var currentElement = items[i];
                items[i] = swapElement;
                items[swapIndex] = currentElement;
            }

            return items;
        }

        
		public T RandomInList<T>(List<T> items)
        {
			if(items.Count == 0)
				return default(T);
					
			return items[Next(items.Count)];
		}

		public Vector3 RandomUnitVector()
        {
			return new Vector3(NextFloat(), NextFloat(), 0f);
		}

	}
}

