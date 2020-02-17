using Services.Models;
using System;

namespace Services.Implementations
{
    /// <summary>
    /// Convention is sequence have following numbers with indeces: 
    /// (index, value) = (0, 0), (1, 1), (2, 1), (3, 2), (4, 3), (5, 5), (6, 8), etc
    /// </summary>
    public class CalculationService
    {
        public CalculationService() { }

        /// <summary>
        /// Used as a state for next calculation
        /// </summary>
        public FibonacciNumber LastSend { get; set; }

        /// <summary>
        /// Returns fibonacci number at given index, used for initial calculation
        /// </summary>
        public FibonacciNumber CalculateNextByIndex(int index)
        {
            var nextNumber = GetFibNumberByIndex(index);
            UpdateState(nextNumber);
            return nextNumber;
        }

        /// <summary>
        /// Stateful calculation for given communication flow: 
        /// as we send a number and receive next one it is enough to calculate next value
        /// </summary>
        public FibonacciNumber CalculateNextWithState(FibonacciNumber received)
        {
            if (LastSend is null)
                throw new InvalidOperationException("Can not calculate using state, it is not yet initialized");

            var nextNumber = new FibonacciNumber { Index = received.Index++, Value = received.Value + LastSend.Value };
            UpdateState(nextNumber);
            return nextNumber;
        }

        private void UpdateState(FibonacciNumber received)
        {
            if (LastSend is null)
                LastSend = received;
            else
            {
                LastSend.Index = received.Index;
                LastSend.Value = received.Value;
            }
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
    }
}
