namespace Slice
{
    public struct SliceSet
    {
        public SliceSet(SliceType value)
        {
            Value = value;
        }

        public SliceType Value { get; set; }

        public void Add(SliceSet sliceSet)
        {
            Value |= sliceSet.Value;
        }

        public bool CanAdd(SliceSet sliceSet)
        {
            return (Value & sliceSet.Value) == 0;
        }

        public bool IsFull()
        {
            return Value == SliceType.Max;
        }
        
        public int Capacity()
        {
            var count = 0;
            var bitmap = (int) Value;
            while (bitmap>0)
            {
                count += bitmap & 1;
                bitmap >>= 1;
            }

            return count;
        }
    }
}