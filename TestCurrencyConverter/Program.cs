// See https://aka.ms/new-console-template for more information
using TestCurrencyConverter;

var currencyConverter = new CurrencyConverter();

currencyConverter.AddRelation("cad", "gbp", .8);
currencyConverter.AddRelation("usd", "cad", 1.3);
currencyConverter.AddRelation("usd", "eur", 1.2);
currencyConverter.AddRelation("usd", "ir", .2);

currencyConverter.UpdateConfiguration(new List<Tuple<string, string, double>>()
{
    new Tuple<string, string, double>("cad", "gbp", .8),
    new Tuple<string, string, double>("usd", "cad", 1.3),
    new Tuple<string, string, double>("usd", "eur", 1.2),
});

foreach (var item in currencyConverter.convertRates)
    Console.WriteLine($"{item.Key.Item1} to {item.Key.Item2} : {item.Value}");

Console.WriteLine();

Console.WriteLine(currencyConverter.Convert("gbp", "eur", 3));
Console.WriteLine(currencyConverter.Convert("eur", "gbp", 3));

Console.Read();