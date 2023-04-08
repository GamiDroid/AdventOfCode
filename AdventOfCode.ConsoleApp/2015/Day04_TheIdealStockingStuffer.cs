using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.ConsoleApp._2015;
internal class Day04_TheIdealStockingStuffer
{
    [Part(1)]
    public void ExecutePart01()
    {
        var anwser = FindNumberWhereMD5HashBeginsWith("bgvyzdsv", "00000");
        Console.WriteLine("The anwser is: {0}", anwser);
    }

    [Part(2)]
    public void ExecutePart02()
    {
        var anwser = FindNumberWhereMD5HashBeginsWith("bgvyzdsv", "000000");
        Console.WriteLine("The anwser is: {0}", anwser);
    }

    public static int FindNumberWhereMD5HashBeginsWith(string secretKey, string startsWith)
    {
        int counter = -1;
        string hex;
        do
        {
            counter++;

            var input = secretKey + counter.ToString();

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = MD5.HashData(inputBytes);

            hex = Convert.ToHexString(hashBytes);
        }
        while (!hex.StartsWith(startsWith));

        Console.WriteLine("hex: " + hex);

        return counter;
    }
}
