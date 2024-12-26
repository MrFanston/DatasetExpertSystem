using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetExpertSystem
{
    internal class Assessment
    {
        // класс для всех оценок
        public abstract class Estimation
        {
            protected List<decimal> array;
            public Estimation(List<decimal> array)
            {
                this.array = array;
            }
        }

        // эмпирическая функция распределения
        public class EDF : Estimation
        {
            new List<decimal> array;
            public EDF(List<decimal> array)
                : base(array)
            {
                this.array = array;
            }

            public int heaviside_function(decimal x)
            {
                if (x > 0)
                    return 1;
                else
                    return 0;
            }
            public decimal value(decimal x) 
            {
                decimal summ = 0;
                for(int i=0; i < array.Count(); i++)
                {   
                    summ += heaviside_function(x - array[i]);
                }
                return summ/ array.Count();
            }
        }

        // непараметрическая случайная величина
        public class SmoothedRandomVariable : Assessment.Estimation, Variable.RandomVariable
        {

            decimal k1(decimal x)
            {
                if (Math.Abs(x) <= 1)
                {
                    return - 1.5M * x;
                }
                else
                {
                    return 0;
                }
            }
            decimal k(decimal x)
            {
                if(Math.Abs(x) <= 1)
                {
                    return 0.75M * (1 - x * x);
                }
                else
                {
                    return 0;
                }
            }

            decimal K(decimal x)
            {
                if(x < -1)
                {
                    return 0;
                }
                else if(-1 <= x && x < 1)
                {
                    return 0.5M + 0.75M * (x - (decimal)Math.Pow((double)x, 3) / 3);
                }
                else
                { 
                    return 1; 
                }
            }

            // значение параметра размытости
            public decimal h;

            decimal calculateOptimalH(List<decimal> array, decimal eps)
            {
                decimal iteration(decimal prevH)
                {
                    decimal result = 0;

                    for(int i = 0; i < array.Count(); i++)
                    {
                        decimal numenator = 0;
                        decimal denominator = 0;

                        for(int j = 0; j < array.Count(); j++)
                        {
                            if(i != j)
                            {
                                numenator += k1((array[i] - array[j]) / prevH) * (array[i] - array[j]);
                                denominator += k((array[i] - array[j]) / prevH);
                            }
                        }

                        if(denominator == 0)
                        {
                            denominator = 0.000001M;
                        }
                        result += numenator / denominator;
                    }

                    return result;
                }

                decimal prevH = deviation(array);
                decimal curH = - iteration(prevH) / array.Count;

                while( Math.Abs(curH - prevH) > eps )
                {
                    decimal temp = prevH;
                    prevH = curH;
                    curH = -iteration(temp) / array.Count;
                    
                }

                return curH;
            }

            decimal deviation(List<decimal> array)
            {
                decimal sum_squared_diff = 0;

                decimal mean = 0;
                foreach (decimal num in array)
                {
                    mean+= num;
                }
                mean = mean / array.Count;

                foreach (decimal num in array)
                {
                    sum_squared_diff += (decimal)Math.Pow((double)(num - mean), 2);
                }

                return (decimal)Math.Pow((double)(sum_squared_diff / (array.Count - 1)), 0.5);
            }

            public SmoothedRandomVariable(List<decimal> array, decimal h = -1)
                : base(array)
            {
                if(h == -1)
                {
                    this.h = calculateOptimalH(array, 0.01M);
                }
                else
                {
                    this.h = h;
                }
            }

            public decimal pdf(decimal x)
            {
                decimal mean = 0;
                for (int i = 0; i < array.Count(); i++)
                {
                    mean += k((x - array[i]) / h);
                }
                return mean / (array.Count() * h);
            }

            public decimal cdf(decimal x)
            {
                decimal mean = 0;
                for (int i = 0; i < array.Count(); i++)
                {
                    mean += K((x - array[i]) / h);
                }
                return mean / array.Count();
            }

            public decimal quantile(decimal alpha)
            {
                return 0.0M;
            }
        }

        // гистограмма
        public class Histogram : Estimation
        {
            protected class Interval
            {
                decimal a;
                decimal b;
                public Interval(decimal a, decimal b)
                {
                    this.a = a;
                    this.b = b;
                }

                public bool is_in(decimal x)
                {
                    return x >= this.a && x <= this.b;
                }
            }

            // количество подинтервалов
            int m;
            public Histogram(List<decimal> array, int m)
                : base(array)
            {
                this.m = m;
                construct_intervals();
            }

            decimal left_boundary_of_intervals;
            decimal right_boundary_of_intervals;

            // массив с границами каждого из интервалов
            List<(decimal, decimal, decimal)> intervals;

            private void construct_intervals()
            {
                left_boundary_of_intervals = array[0];
                right_boundary_of_intervals = array[array.Count() - 1];

                // вычисляем размах значений выборки
                decimal range = right_boundary_of_intervals - left_boundary_of_intervals;

                // длина каждого из интервалов
                decimal length_interval = range / m;

                // массив с подинтервалами (левая граница, правая граница и количество элементов внутри подинтервала)
                intervals = new List<(decimal, decimal, decimal)>(m);

                // задаем правые и левые границы подинтервалов
                for (int i = 0; i < m; i++)
                {
                    intervals.Add((left_boundary_of_intervals + i * length_interval, left_boundary_of_intervals + (i + 1) * length_interval, 0));
                }

                // формируем высоты интервалов
                // перебор подинтервалов
                int j = 0;
                for (int i = 0; i < m; i++)
                {
                    int count = 0;
                    Interval interval = new Interval(intervals[i].Item1, intervals[i].Item2);
                    // поиск количества элементов входящих в подинтервал
                    for (; j < array.Count(); j++)
                    {
                        if (interval.is_in(array[j]))
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    intervals[i] = (intervals[i].Item1, intervals[i].Item2, (decimal)count / (((decimal)array.Count()) * length_interval));
                }
            }


            public decimal Value(decimal x)
            {
                for (int i = 0; i < m; ++i)
                {
                    Interval interval = new Interval(intervals[i].Item1, intervals[i].Item2);
                    if (interval.is_in(x))
                    {
                        return intervals[i].Item3;
                    }
                }
                return 0;
            }
        }

    }
}
