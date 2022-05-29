using System.Collections;
using System.Collections.Generic;

public class HackTool
{
    public string name;
    public string commandName;
    public string descripton;
    public double price;
    public int expCost;
    public HackTool(string name, string commandName, string description, double price, int expCost)
    {
        this.name = name;
        this.commandName = commandName;
        this.descripton = description;
        this.price = price;
        this.expCost = expCost;
    }
}
