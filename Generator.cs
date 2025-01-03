﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetExpertSystem
{
    internal class Generator
    {
        // интерфейс для генератора псевдослучайных величин
        public abstract class RandomNumberGenerator
        {
            public Variable.RandomVariable random_variable;
            public RandomNumberGenerator(Variable.RandomVariable random_variable)
            {
                this.random_variable = random_variable;
            }
            public abstract List<decimal> get(int N);
        }

        // генератора псевдослучайных величин
        public class SimpleRandomNumberGenerator : RandomNumberGenerator
        {
            private Random random;
            private List<decimal> array;
            public SimpleRandomNumberGenerator(Variable.RandomVariable random_variable)
                : base(random_variable)
            {
                this.random = new Random();
            }
            public override List<decimal> get(int N)
            {
                this.array = new List<decimal>();

                for (int i = 0; i < N; i++)
                {
                    // создаем элемент выборки равномерного распределения
                    array.Add( (decimal)this.random.NextDouble() );

                    // преобразуем элемент выборки в нужное распределение, прогоняя его через квантиль
                    array[i] = random_variable.quantile(array[i]);
                }

                return array;
            }
        }
    }
}
