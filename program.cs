// See https://aka.ms/new-console-template for more information
using System;
using System.Text;

namespace TGHE_Delivery
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "--selftest")
            {
                DinicSelfTest.Run();
                return;
            }

            DeliveryInstance instance = InputParser.ParseFromStdIn();
            var controller = new DeliveryController(instance);
            long[] answers = controller.SolveAll();

            var output = new StringBuilder();
            for (int i = 0; i < answers.Length; i++)
                output.AppendLine(answers[i].ToString());

            Console.Write(output.ToString());
        }
    }
}