namespace LD58.Fruits
{
    public class CollectedFruit
    {
        public FruitData Data { get; }
        public int GrowStep { get; }

        public CollectedFruit(Fruit fruit)
        {
            Data = fruit.Data;
            GrowStep = fruit.GrowStep;
        }

        public CollectedFruit(FruitData fruit_data)
        {
            Data = fruit_data;
            GrowStep = fruit_data.GrowSteps.Count - 1;
        }
    }
}
