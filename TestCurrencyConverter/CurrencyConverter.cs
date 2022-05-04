using System;

namespace TestCurrencyConverter
{
    public class CurrencyConverter : ICurrencyConverter
    {
        public readonly Dictionary<(string, string), double> convertRates; // todo : change public to private

        public CurrencyConverter()
        {
            convertRates = new Dictionary<(string, string), double>();
        }

        public void ClearConfiguration()
        {
            convertRates.Clear();
        }

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            if (convertRates.ContainsKey((fromCurrency, toCurrency)))
                return amount * convertRates[(fromCurrency, toCurrency)];

            if (convertRates.ContainsKey((toCurrency, fromCurrency)))
                return amount / convertRates[(toCurrency, fromCurrency)];

            throw new Exception("convert rate not found");
        }

        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            ClearConfiguration();

            foreach (var conversionRate in conversionRates)
                AddRelation(conversionRate.Item1, conversionRate.Item2, conversionRate.Item3);
        }

        public void AddRelation(string from, string to, double rate)
        {
            if (convertRates.ContainsKey((from, to)) || convertRates.ContainsKey((to, from)))
                return;

            convertRates.Add((from, to), rate);
            ManageRelation(from, to);
        }

        private void ManageRelation(string first, string second)
        {
            var rate = convertRates[(first, second)];
            var firstCurrencyRelations = convertRates
                .Where(c => (c.Key.Item1 == first && c.Key.Item2 != second)
                || (c.Key.Item2 == first && c.Key.Item1 != second)).ToList();

            foreach (var relation in firstCurrencyRelations)
            {
                if (relation.Key.Item1 == first)
                {
                    var newRate = rate / relation.Value;
                    AddRelation(relation.Key.Item2, second, newRate);
                }
                else // item2 == first
                {
                    var newRate = rate * relation.Value;
                    AddRelation(relation.Key.Item1, second, newRate);
                }
            }

            var secondCurrencyRelations = convertRates
                .Where(c => (c.Key.Item1 == second && c.Key.Item2 != first)
                || (c.Key.Item2 == second && c.Key.Item1 != first)).ToList();

            foreach (var relation in secondCurrencyRelations)
            {
                if (relation.Key.Item1 == second)
                {
                    var newRate = rate * relation.Value;
                    AddRelation(first, relation.Key.Item2, newRate);
                }
                else // item2 == second
                {
                    var newRate = rate / relation.Value;
                    AddRelation(relation.Key.Item1, first, newRate);
                }
            }
        }
    }
}
