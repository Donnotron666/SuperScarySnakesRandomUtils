using System.Collections.Generic;

namespace SSSRandom
{
    public class WeightedTable<T>
    {
        private List<T> Options = new List<T>();
        private List<float> Weights = new List<float>();

        private float sum = 0f;

        public void Add(T option, float pct)
        {
            Options.Add(option);
            Weights.Add(pct);

            sum += pct;
        }

        public void Remove(T option)
        {
            var index = Options.IndexOf(option);
            Options.RemoveAt(index);
            var contrib = Weights[index];
            Weights.RemoveAt(index);
            sum -= contrib;
        }
            
        public T Select(Seed seed)
        {
            var weightedRandom = seed.Next(sum);
            var index = 0;
            for (; index < Weights.Count; index++)
            {
                if (weightedRandom < Weights[index])
                {
                    break;
                }

                weightedRandom -= Weights[index];

            }
           
            return Options[index];
        }
    }
}
