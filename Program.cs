﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DRON;

public class Record
{
    public Guid Id { get; set; }
    public int Count { get; set; }
    public string Name { get; set; }
    public IEnumerable<Record> Objects { get; set; }
    public double Ratio { get; set; }
    public bool IsValid { get; set; }
    public Record Child { get; set; }
    public Dictionary<string, string> EmptyDict { get; set; }
}

class Program
{
    private static Record _record;
    static void Main(string[] args)
    {
        _record = new Record()
        {
            Id = new Guid("AE8F08BE-FE34-4743-B024-8F9CDBE84569"),
            Count = 1,
            Name = "name",
            Objects = new List<Record>
            {
                new Record()
            },
            Ratio = 0.8,
            IsValid = true,
            Child = new Record(),
        };
        Console.Write("Dron Deserialize Sync:  "); RunDeserialize();
        Console.Write("Json Deserialize Sync:  "); RunJsonDeserialize();
        Console.WriteLine();
        Console.Write("Dron Deserialize Async: "); RunAsyncDeserialize();
        Console.Write("Json Deserialize Async: "); RunJsonAsyncDeserialize();

        Console.WriteLine();
        Console.Write("Dron Serialize Sync:  "); RunSerialize();
        Console.Write("Json Serialize Sync:  "); RunJsonSerialize();
    }

    private const int TIMES = 1_000;
    private const string FILE_PREFIX = "record";
    private const string DRON_FILE = FILE_PREFIX + ".dron";
    private const string JSON_FILE = FILE_PREFIX + ".json";
    private static void RunDeserialize()
    {
        var stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < TIMES; ++i)
        {
            var record = Dron.Deserialize<Record>(
                File.ReadAllText(DRON_FILE)
            );
        }
        stopwatch.Stop();
        Console.WriteLine(stopwatch.Elapsed);
    }

    private static void RunJsonDeserialize()
    {
        var stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < TIMES; ++i)
        {
            JsonSerializer.Deserialize<Record>(File.ReadAllText(JSON_FILE));
        }
        stopwatch.Stop();
        Console.WriteLine(stopwatch.Elapsed);
    }
    
    private static void RunAsyncDeserialize()
    {
        var tasks = new Task[TIMES];
        var stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < TIMES; ++i)
        {
            tasks[i] = (
                Task.Run(
                    () => Dron.DeserializeAsync<Record>(
                        File.OpenRead(DRON_FILE)
                    )
                )
            );
        }
        Task.WaitAll(tasks);
        stopwatch.Stop();
        Console.WriteLine(stopwatch.Elapsed);
    }

    private static void RunJsonAsyncDeserialize()
    {
        var tasks = new Task[TIMES];
        var stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < TIMES; ++i)
        {
            tasks[i] = (
                Task.Run(
                    () => JsonSerializer.DeserializeAsync<Record>(
                        File.OpenRead(JSON_FILE)
                    )
                )
            );
        }
        Task.WaitAll(tasks);
        stopwatch.Stop();
        Console.WriteLine(stopwatch.Elapsed);
    }

    private static void RunSerialize()
    {
        var stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < TIMES; ++i)
        {
            var record = Dron.Serialize(_record);
        }
        stopwatch.Stop();
        Console.WriteLine(stopwatch.Elapsed);
    }

    private static void RunJsonSerialize()
    {
        var stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < TIMES; ++i)
        {
            JsonSerializer.Serialize(_record);
        }
        stopwatch.Stop();
        Console.WriteLine(stopwatch.Elapsed);
    }
}
