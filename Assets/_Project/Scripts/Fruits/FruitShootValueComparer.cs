using System.Collections.Generic;

namespace LD58.Fruits
{
    public class FruitShootValueComparer : IComparer<FruitData>
    {
        public int Compare(FruitData left_fruit, FruitData right_fruit)
        {
            int shoot_value_comparison = left_fruit.ShootValue.CompareTo(right_fruit.ShootValue);

            if (shoot_value_comparison == 0)
            {
                return right_fruit.CurrencyValue.CompareTo(left_fruit.CurrencyValue);
            }

            return shoot_value_comparison;
        }
    }
}
