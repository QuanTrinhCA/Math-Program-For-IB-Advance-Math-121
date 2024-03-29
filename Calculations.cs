﻿namespace CalucalatingNumbers
{
    internal class Calculations
    {
        public static List<List<List<double>>> GetAllPossibleFormulas(List<double> inputNumbers, List<double> valuesToMatch)
        {
            List<List<List<double>>> formulas = new();
            foreach (List<double> numbers in GetAllPossibleNumbers(inputNumbers))
            {
                foreach (List<double> formula in GetFormulas(numbers.First(), numbers.Skip(1).ToList(), new List<double>(), valuesToMatch))
                {
                    List<List<double>> currentNumberFormulas = new();
                    currentNumberFormulas.Add(numbers);
                    currentNumberFormulas.Add(formula);
                    formulas.Add(currentNumberFormulas);
                }
            }
            return formulas;
        }

        public static List<List<double>> GetAllPossibleNumbers(List<double> inputNumbers)
        {
            List<List<double>> result = new();
            foreach (List<double> allPossibleSingleElementNumbersWithoutInverse in GetAllPossibleSingleElementNumbersWithoutInverse(inputNumbers, new List<double>()))
            {
                foreach (List<double> allPossibleMultiElementNumbersWithoutInverse in GetAllPossibleMultiElementNumbersWithoutInverse(allPossibleSingleElementNumbersWithoutInverse.Take(1).ToList(), allPossibleSingleElementNumbersWithoutInverse.Skip(1).ToList()))
                {
                    result = result.Concat(GetAllPossibleMultiElementNumbersWithInverse(new List<double>(), allPossibleMultiElementNumbersWithoutInverse)).ToList();
                }
            }
            return result;
        }
        private static List<List<double>> GetAllPossibleSingleElementNumbersWithoutInverse(List<double> currentInputNumbers, List<double> currentOutputNumbers)
        {
            List<List<double>> result = new();
            if (currentInputNumbers.Count == 0)
            {
                return new List<List<double>> { currentOutputNumbers };
            }
            else
            {
                foreach (double inputNumber in currentInputNumbers)
                {
                    List<double> tempNumberList = new();
                    currentOutputNumbers.ForEach(x => tempNumberList.Add(x));
                    tempNumberList.Add(inputNumber);
                    result = result.Concat(GetAllPossibleSingleElementNumbersWithoutInverse(currentInputNumbers.Except(tempNumberList).ToList(), tempNumberList)).ToList();
                }
                return result;
            }
        }

        private static List<List<double>> GetAllPossibleMultiElementNumbersWithoutInverse(List<double> currentFirstInputNumbers, List<double> currentLastInputNumbers)
        {
            List<List<double>> result = new();

            if (currentLastInputNumbers.Count == 0)
            {
                return new List<List<double>> { currentFirstInputNumbers };
            }
            else
            {
                List<double> currentLastInputNumbersNotAddingNextNumber = currentLastInputNumbers.Skip(1).ToList();

                //Not adding the next number to the first number
                List<double> currentFirstInputNumbersNotAddingNextNumber = new();
                currentFirstInputNumbers.ForEach(x => currentFirstInputNumbersNotAddingNextNumber.Add(x));
                currentFirstInputNumbersNotAddingNextNumber.Add(currentLastInputNumbers.First());
                result = result.Concat(GetAllPossibleMultiElementNumbersWithoutInverse(currentFirstInputNumbersNotAddingNextNumber, currentLastInputNumbersNotAddingNextNumber)).ToList();

                //Adding the next number to the first number
                List<double> currentFirstInputNumbersAddingNextNumber = new();
                currentFirstInputNumbers.ForEach(x => currentFirstInputNumbersAddingNextNumber.Add(x));
                double tempNumber = currentFirstInputNumbersAddingNextNumber.Last() * Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(currentLastInputNumbers.First()))) + 1);
                tempNumber += currentLastInputNumbers.First();
                currentFirstInputNumbersAddingNextNumber.RemoveAt(currentFirstInputNumbersAddingNextNumber.Count - 1);
                currentFirstInputNumbersAddingNextNumber.Add(tempNumber);
                result = result.Concat(GetAllPossibleMultiElementNumbersWithoutInverse(currentFirstInputNumbersAddingNextNumber, currentLastInputNumbersNotAddingNextNumber)).ToList();
            }
            return result;
        }
        private static List<List<double>> GetAllPossibleMultiElementNumbersWithInverse(List<double> currentFirstInputNumbers, List<double> currentLastInputNumbers)
        {
            List<List<double>> result = new();

            if (currentLastInputNumbers.Count == 0)
            {
                return new List<List<double>> { currentFirstInputNumbers };
            }
            else
            {
                List<double> currentLastInputNumbersRemovingFirstNumber = currentLastInputNumbers.Skip(1).ToList();

                //Adding non inversed first number
                List<double> currentFirstInputNumbersAddingNotInversedNextNumber = new();
                currentFirstInputNumbers.ForEach(x => currentFirstInputNumbersAddingNotInversedNextNumber.Add(x));
                currentFirstInputNumbersAddingNotInversedNextNumber.Add(currentLastInputNumbers.First());
                result = result.Concat(GetAllPossibleMultiElementNumbersWithInverse(currentFirstInputNumbersAddingNotInversedNextNumber, currentLastInputNumbersRemovingFirstNumber)).ToList();

                //Adding inversed first number
                List<double> currentFirstInputNumbersAddingInversedNextNumber = new();
                currentFirstInputNumbers.ForEach(x => currentFirstInputNumbersAddingInversedNextNumber.Add(x));
                currentFirstInputNumbersAddingInversedNextNumber.Add(currentLastInputNumbers.First() * (-1));
                result = result.Concat(GetAllPossibleMultiElementNumbersWithInverse(currentFirstInputNumbersAddingInversedNextNumber, currentLastInputNumbersRemovingFirstNumber)).ToList();
            }
            return result;
        }
        private static List<List<double>> GetFormulas(double currentFirstInputNumber, List<double> currentLastInputNumbers, List<double> lastFormula, List<double> valuesToMatch)
        {
            List<List<double>> result = new();

            if (currentLastInputNumbers.Count == 0)
            {
                if (valuesToMatch.Contains(currentFirstInputNumber))
                {
                    List<double> finalFormula = new();
                    lastFormula.ForEach(x => finalFormula.Add(x));
                    finalFormula.Add(currentFirstInputNumber);
                    return new List<List<double>> { finalFormula };
                }
                else
                {
                    return new List<List<double>>();
                }
            }
            else
            {
                for (int i = 0; i <= 6; i++)
                {
                    List<double> currentFormula = new();
                    lastFormula.ForEach(x => currentFormula.Add(x));
                    currentFormula.Add(i);
                    try
                    {
                        double calculatedResult = GetCalculationResult(i, currentFirstInputNumber, currentLastInputNumbers.First());
                        result = result.Concat(GetFormulas(calculatedResult, currentLastInputNumbers.Skip(1).ToList(), currentFormula, valuesToMatch)).ToList();
                    }
                    finally { }
                }
            }
            return result;
        }

        private static double GetCalculationResult(int calculationType, double x, double y)
        {
            double result = 0;
            switch (calculationType)
            {
                // Addition
                case 0:
                    result = x + y;
                    break;
                // Substraction
                case 1:
                    result = x - y;
                    break;
                //Multiplication
                case 2:
                    result = x * y;
                    break;
                //Division x / y
                case 3:
                    result = x / y;
                    break;
                //Division y / x
                case 4:
                    result = y / x;
                    break;
                //Power x^y
                case 5:
                    result = Math.Pow(x, y);
                    break;
                //Power y^x
                case 6:
                    result = Math.Pow(y, x);
                    break;
            }
            return result;
        }
    }
}
