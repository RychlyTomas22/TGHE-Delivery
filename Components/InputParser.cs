// Components/InputParser.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TGHE_Delivery
{
    internal static class InputParser
    {
        public static DeliveryInstance ParseFromStdIn()
        {
            using var reader = new StreamReader(Console.OpenStandardInput());

            int[] header = ParseInts(ReadNonEmptyLine(reader));
            if (header.Length != 3)
                throw new InvalidOperationException("First line must contain exactly 3 integers: ne nr nv.");

            int plantCount = header[0];
            int substationCount = header[1];
            int wireCount = header[2];

            if (plantCount < 0 || substationCount < 0 || wireCount < 0)
                throw new InvalidOperationException("Counts must be non-negative.");

            var plants = new List<Plant>(plantCount);
            for (int i = 0; i < plantCount; i++)
            {
                int[] powerLine = ParseInts(ReadNonEmptyLine(reader));
                if (powerLine.Length != 1)
                    throw new InvalidOperationException("Each plant power must be on its own line.");

                int power = powerLine[0];
                if (power < 0) throw new InvalidOperationException("Plant power must be non-negative.");

                plants.Add(new Plant(id: i, power: power));
            }

            var substations = new List<Substation>(substationCount);
            for (int i = 0; i < substationCount; i++)
            {
                substations.Add(new Substation(id: plantCount + i));
            }

            int nodeCount = plantCount + substationCount;

            var wires = new List<Wire>(wireCount);
            for (int i = 0; i < wireCount; i++)
            {
                int[] wireLine = ParseInts(ReadNonEmptyLine(reader));
                if (wireLine.Length != 3)
                    throw new InvalidOperationException("Each wire line must contain exactly 3 integers: a b cap.");

                int a = wireLine[0];
                int b = wireLine[1];
                int capacity = wireLine[2];

                if ((uint)a >= (uint)nodeCount || (uint)b >= (uint)nodeCount)
                    throw new InvalidOperationException($"Wire endpoint out of range: {a}, {b} (0..{nodeCount - 1}).");
                if (capacity < 0)
                    throw new InvalidOperationException("Wire capacity must be non-negative.");

                wires.Add(new Wire(a, b, capacity));
            }

            int[] nLine = ParseInts(ReadNonEmptyLine(reader));
            if (nLine.Length != 1)
                throw new InvalidOperationException("Location count N must be on its own line.");

            int locationCount = nLine[0];
            if (locationCount < 0) throw new InvalidOperationException("N must be non-negative.");

            var locations = new List<Location>(locationCount);
            for (int i = 0; i < locationCount; i++)
            {
                int[] attachments = ParseInts(ReadNonEmptyLine(reader));

                if (attachments.Length < 1 || attachments.Length > 3)
                    throw new InvalidOperationException("Each location line must contain 1..3 attachment node indices.");

                for (int j = 0; j < attachments.Length; j++)
                {
                    int node = attachments[j];
                    if ((uint)node >= (uint)nodeCount)
                        throw new InvalidOperationException($"Attachment out of range: {node} (0..{nodeCount - 1}).");
                }

                locations.Add(new Location(attachments));
            }

            return new DeliveryInstance
            {
                Plants = plants,
                Substations = substations,
                Wires = wires,
                Locations = locations
            };
        }

        private static string ReadNonEmptyLine(StreamReader reader)
        {
            while (true)
            {
                string? line = reader.ReadLine();
                if (line is null) throw new EndOfStreamException("Unexpected EOF.");
                if (!string.IsNullOrWhiteSpace(line)) return line;
            }
        }

        private static int[] ParseInts(string line)
        {
            string[] parts = line.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            var values = new int[parts.Length];

            for (int i = 0; i < parts.Length; i++)
                values[i] = int.Parse(parts[i]);

            return values;
        }
    }
}

