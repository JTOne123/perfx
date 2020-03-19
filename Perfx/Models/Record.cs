﻿namespace Perfx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ByteSizeLib;

    using CsvHelper.Configuration.Attributes;

    using MathNet.Numerics.Statistics;

    public class Record
    {
        public float id { get; set; }
        public DateTime timestamp { get; set; }
        public string url { get; set; }
        public string result { get; set; }
        public long? size_b { get; set; }
        public string size_str => size_b.HasValue ? $"{size_num_str}{size_unit}" : string.Empty;
        public double local_ms { get; set; }
        public double ai_ms { get; set; }
        public double local_s => Math.Round(this.local_ms / 1000, 2);
        public double ai_s => Math.Round(this.ai_ms / 1000, 2);
        public string op_Id { get; set; }
        public string ai_op_Id { get; set; }

        [Ignore]
        public RunInput input { get; set; }

        [Ignore]
        public string size_num_str => size_b.HasValue ? ByteSize.FromBytes(size_b.Value).LargestWholeNumberDecimalValue.ToString("F2") : string.Empty;

        [Ignore]
        public string size_unit => size_b.HasValue ? $"{ByteSize.FromBytes(size_b.Value).LargestWholeNumberDecimalSymbol}" : string.Empty;

        [Ignore]
        public double duration_ms => string.IsNullOrEmpty(ai_op_Id) ? local_ms : ai_ms;

        [Ignore]
        public string duration_ms_str => this.duration_ms.ToString("F2");

        [Ignore]
        public double duration_s => this.duration_ms / 1000;

        [Ignore]
        public int duration_s_round => (int)Math.Round(this.duration_s);

        [Ignore]
        public string duration_s_str => this.duration_s.ToString("F2");

        [Ignore]
        public string local_ms_str => this.local_ms.ToString("F2");

        [Ignore]
        public string local_s_str => this.local_s.ToString("F2");

        [Ignore]
        public string ai_ms_str => this.ai_ms.ToString("F2");

        [Ignore]
        public string ai_s_str => this.ai_s.ToString("F2");

        [Ignore]
        public List<PropertyInfo> Properties { get; } = typeof(Record).GetProperties().Where(p => p.GetCustomAttribute<IgnoreAttribute>() == null).ToList();
    }

    public class Run
    {
        public Run(IEnumerable<Record> records, string url)
        {
            this.records = records;
            this.url = url;
        }

        [Ignore]
        public IEnumerable<Record> records { get; set; }

        [Ignore]
        public List<Record> ok_records => this.records.Where(x => x.result.Contains("200"))?.ToList();

        [Ignore]
        public List<double> ok_records_durations_ms => this.ok_records.Select(x => x.duration_ms / 1000)?.ToList();

        [Ignore]
        public List<double> ok_records_size_kb => this.ok_records.Select(x => x.size_b.HasValue ? ByteSize.FromBytes(x.size_b.Value).KiloBytes : 0)?.ToList();

        public string url { get; set; }
        public double dur_min_s => this.ok_records_durations_ms.Count > 0 ? Math.Round(this.ok_records_durations_ms.Min(), 2) : 0;
        public double dur_max_s => this.ok_records_durations_ms.Count > 0 ? Math.Round(this.ok_records_durations_ms.Max(), 2) : 0;
        public double dur_mean_s => this.ok_records_durations_ms.Count > 0 ? Math.Round(this.ok_records_durations_ms.Mean(), 2) : 0;
        public double dur_median_s => this.ok_records_durations_ms.Count > 0 ? Math.Round(this.ok_records_durations_ms.Median(), 2) : 0;
        public double dur_std_dev_s => this.ok_records_durations_ms.Count > 0 ? Math.Round(this.ok_records_durations_ms.StandardDeviation(), 2) : 0;
        public double dur_90_perc_s => this.ok_records_durations_ms.Count > 0 ? Math.Round(this.ok_records_durations_ms.Percentile(90), 2) : 0;
        public double dur_95_perc_s => this.ok_records_durations_ms.Count > 0 ? Math.Round(this.ok_records_durations_ms.Percentile(95), 2) : 0;
        public double dur_99_perc_s => this.ok_records_durations_ms.Count > 0 ? Math.Round(this.ok_records_durations_ms.Percentile(99), 2) : 0;
        public double size_min_kb => this.ok_records_size_kb.Count > 0 ? Math.Round(this.ok_records_size_kb.Min(), 2) : 0;
        public double size_max_kb => this.ok_records_size_kb.Count > 0 ? Math.Round(this.ok_records_size_kb.Max(), 2) : 0;
        public double ok_200 => (int)Math.Round(((double)(this.ok_records.Count() / this.records.Count())) * 100);
        public double other_xxx => 100 - this.ok_200;

        [Ignore]
        public List<PropertyInfo> Properties { get; } = typeof(Run).GetProperties().Where(p => !p.Name.Equals(nameof(url)) && p.GetCustomAttribute<IgnoreAttribute>() == null).ToList();
    }

    public class RunInput
    {
        public float Id { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public string Query { get; set; }
        public string Body { get; set; }
        public string Headers { get; set; }
    }
}
