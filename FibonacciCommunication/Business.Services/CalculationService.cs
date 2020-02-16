using System;

namespace Services
{
    /// <summary>
    /// Convention is sequence have following numbers with indeces: 
    /// (index, value) = (0, 0), (1, 1), (2, 1), (3, 2), (4, 3), etc
    /// </summary>
    public class CalculationService
    {
        private int _lastCalculatedValue;
        private int _lastCalculatedIndex;
        private FibonacciNumber _lastNumber;

        public (int, int) CalculateNext(int number, int index)
        {
            var atIndex = GetFibonacciNumberWithIndex(index + 1);
            _lastCalculatedValue = atIndex.Item1;
            _lastCalculatedIndex = atIndex.Item2;

            return atIndex;
        }

        public FibonacciNumber GetNext(FibonacciNumber number)
        {
            var res = GetFibNumberByIndex(number.Index + 1);
            _lastNumber = res;
            return res;
        }

        private FibonacciNumber GetFibNumberByIndex(int index)
        {
            if (index < 0)
                throw new InvalidOperationException("Index should be >=0");

            if (index == 0)
                return new FibonacciNumber { Index = 0, Value = 0 };
            if (index == 1)
                return new FibonacciNumber { Index = 1, Value = 1 };

            var number = 0;
            var nextNumber = 1;
            var currentIndex = 1;

            while (currentIndex < index)
            {
                var temp = number + nextNumber;
                number = nextNumber;
                nextNumber = temp;

                currentIndex++;
            }

            return new FibonacciNumber { Index = index, Value = nextNumber };
        }

        private (int, int) GetFibonacciNumberWithIndex(int index)
        {
            if (index == 1)
                return (1, 2);
            if (index == 2)
                return (0,0);

            int number = 1;
            int nextNumber = 1;
            int currentIndex = 2;

            while(currentIndex < index)
            {
                var temp = number + nextNumber;
                number = nextNumber;
                nextNumber = temp;

                currentIndex++;
            }

            return (nextNumber, currentIndex - 1);
        }
    }
}
