namespace LD58.Fruits
{
    public class CollectedFruit
    {
        public FruitData Data { get; }
        public int GrowStep { get; }

        public CollectedFruit(
            FruitData data,
            int grow_step
            )
        {
            Data = data;
            GrowStep = grow_step;
        }
    }
}
