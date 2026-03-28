namespace LD58.Fruits
{
    public interface IFruitCollisionTarget
    {
        public void OnCollision(Fruit fruit) { }
        public void OnTrigger(Fruit fruit) { }
    }
}
