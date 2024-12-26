using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetExpertSystem
{
    internal class Variable
    {
        // интерфейс для любой случайной величины
        public interface RandomVariable
        {
            // плотность вероятности в точке x
            public decimal pdf(decimal x);

            // интегральная функция распределения в точке x
            public decimal cdf(decimal x);

            // квантиль уровня alpha
            public decimal quantile(decimal alpha);
        }

        // класс равномерной случайной величины
        public class UniformRandomVariable : RandomVariable
        {
            // левая и правая граница непрерывного равномерного распределения
            private decimal left, right;

            public UniformRandomVariable(decimal _left, decimal _right)
            {
                this.left = _left;
                this.right = _right;
            }

            public decimal pdf(decimal x)
            {
                if (x < this.left && x > this.right)
                    return (decimal)0.0;
                else
                    return (decimal)1.0 / (this.right - this.left);
            }

            public decimal cdf(decimal x)
            {
                if (x < this.left)
                    return (decimal)0.0;
                else if (x >= this.left && x < this.right)
                    return (x - this.left) / (this.right - this.left);
                else
                    return (decimal)1.0;
            }

            public decimal quantile(decimal alpha)
            {
                return alpha * (this.right - this.left) + this.left;
            }
        }

        // класс нормальной случайной величины
        public class NormalRandomVariable : RandomVariable
        {
            // параметры сдвига и масштаба нормального распределения
            private decimal location, scale;

            public NormalRandomVariable(decimal _location, decimal _scale)
            {
                this.location = _location;
                this.scale = _scale;
            }

            public decimal pdf(decimal x)
            {
                decimal z = (x - (decimal)location) / (decimal)scale;
                return (decimal)(Math.Exp((double)((decimal)-0.5 * z * z)) / (Math.Sqrt(2 * Math.PI) * (double)scale));
            }

            public decimal cdf(decimal x)
            {
                decimal z = (x - location) / scale;
                if( z <= 0 )
                {
                    return (decimal)(0.852 * Math.Exp(-Math.Pow(((double)-z + 1.5774) / 2.0637, 2.34)));
                }
                return 1 - (decimal)(0.852 * Math.Exp(-Math.Pow(((double)z + 1.5774) / 2.0637, 2.34)));
            }

            public decimal quantile(decimal alpha)
            {
                return location + (decimal)4.91 * scale * (decimal)(Math.Pow((double)alpha, 0.14) - Math.Pow(1 - (double)alpha, 0.14));
            }
        }

        // класс экспоненциальной случайной величины
        public class ExponentialRandomVariable : RandomVariable
        {
            // параметр интенсивности (обратный коэффициент масштаба) экспоненциального распределения
            private decimal intensity;

            public ExponentialRandomVariable(decimal _intensity)
            {
                this.intensity = _intensity;
            }

            public decimal pdf(decimal x)
            {
                if (x >= 0)
                {
                    return intensity * (decimal)Math.Exp((double)-intensity * (double)x);
                }
                return 0;
            }

            public decimal cdf(decimal x)
            {
                if (x >= 0)
                {
                    return 1 - (decimal)Math.Exp((double)-intensity * (double)x);
                }
                return 0;
            }

            public decimal quantile(decimal alpha)
            {
                return - (decimal)Math.Log(1 - (double)alpha) / intensity;
            }
        }

        // класс двойной экспоненциальной случайной величины (распределение Лапласа)
        public class LaplaceRandomVariable : RandomVariable
        {
            // параметры сдвига и масштаба распределения Лапласа
            private decimal location, scale;

            public LaplaceRandomVariable(decimal _location, decimal _scale)
            {
                this.location = _location;
                this.scale = _scale;
            }

            public decimal pdf(decimal x)
            {
                return (decimal)0.5 * scale * (decimal)Math.Exp((double)-scale * Math.Abs((double)x - (double)location));
            }

            public decimal cdf(decimal x)
            {
                if (x <= location)
                {
                    return (decimal)0.5 * (decimal)Math.Exp((double)scale * ((double)x - (double)location));
                }
                return 1 - (decimal)0.5 * (decimal)Math.Exp((double)-scale * ((double)x - (double)location));
            }

            public decimal quantile(decimal alpha)
            {
                decimal x = location + (decimal)Math.Log((double)alpha * 2) / scale;
                if (x <= location)
                {
                    return x;
                }
                return location - (decimal)Math.Log(2 - (double)alpha * 2) / scale;
            }
        }

        // класс случайной величины распределения Коши
        public class CauchyRandomVariable : RandomVariable
        {
            // параметры сдвига и масштаба распределения Коши
            private decimal location, scale;

            public CauchyRandomVariable(decimal _location, decimal _scale)
            {
                this.location = _location;
                this.scale = _scale;
            }

            public decimal pdf(decimal x)
            {
                return scale / (decimal)(Math.PI * (Math.Pow((double)x - (double)location, 2) + Math.Pow((double)scale, 2)));
            }

            public decimal cdf(decimal x)
            {
                return (decimal)0.5 + (decimal)Math.Atan(((double)x - (double)location) / (double)scale) / (decimal)Math.PI;
            }

            public decimal quantile(decimal alpha)
            {
                return location + scale * (decimal)Math.Tan(Math.PI * ((double)alpha - (double)0.5));
            }
        }
    }
}
