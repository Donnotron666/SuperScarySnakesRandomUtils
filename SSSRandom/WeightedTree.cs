using System.Collections.Generic;

namespace SSSRandom
{
    public class WeightedTree<T>
    {
        public List<WeightedTree<T>> Branches;

        public T Value;
        public float Weight;
        
        public WeightedTree(T value, float weight)
        {
            this.Value = value;
            this.Weight = weight;
        }

        public WeightedTree(List<WeightedTree<T>> branches, float weight)
        {
            this.Branches = branches;
            this.Weight = weight;
        }

        public T Select(int seedValue)
        {
            return Select(new Seed(seedValue));
        }

        private bool IsValueNode 
        {
            get {
                return Value != null;
            }
        }
        
        public T Select(Seed seed)
        {
            if(!IsValueNode)
            {
                var table = new WeightedTable<WeightedTree<T>>();
                Branches.ForEach(p => table.Add(p, p.Weight));
                var selection = table.Select(seed);
                return selection.Select(seed);

            } else
            {
                return Value;
            }
            
        }
    }

    
}
