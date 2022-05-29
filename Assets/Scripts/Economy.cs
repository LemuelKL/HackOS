using System.Collections;
using System.Collections.Generic;

public static class Economy
{
    // with ref, to usd
    public static Dictionary<string, double> CurrencyExchange = new Dictionary<string, double>(){
        {"USD", 1},
        {"HKD", 1/7.8},
        {"BTC", 306109.46},
        {"ETH", 2995.5},
    };
}
