﻿using LLama.Common;
using System.Text.Json;
using LLama.Abstractions;

namespace LLama.Unittest
{
    public class ModelsParamsTests
    {
        [Fact]
        public void SerializeRoundTripSystemTextJson()
        {
            var expected = new ModelParams("abc/123")
            {
                BatchSize = 17,
                ContextSize = 42,
                Seed = 42,
                GpuLayerCount = 111,
                TensorSplits = { [0] = 3 },
                MetadataOverrides =
                {
                    MetadataOverride.Create("hello", true),
                    MetadataOverride.Create("world", 17),
                }
            };

            var json = JsonSerializer.Serialize(expected);
            var actual = JsonSerializer.Deserialize<ModelParams>(json)!;

            // Cannot compare splits with default equality, check they are sequence equal and then set to null
            Assert.Equal((IEnumerable<float>)expected.TensorSplits, expected.TensorSplits);
            actual.TensorSplits = null!;
            expected.TensorSplits = null!;

            // Check encoding is the same
            var b1 = expected.Encoding.GetBytes("Hello");
            var b2 = actual.Encoding.GetBytes("Hello");
            Assert.True(b1.SequenceEqual(b2));

            Assert.Equal(expected, actual);
        }

        //[Fact]
        //public void SerializeRoundTripNewtonsoft()
        //{
        //    var expected = new ModelParams("abc/123")
        //    {
        //        BatchSize = 17,
        //        ContextSize = 42,
        //        Seed = 42,
        //        GpuLayerCount = 111,
        //        LoraAdapters =
        //        {
        //            new("abc", 1),
        //            new("def", 0)
        //        },
        //        TensorSplits = { [0] = 3 }
        //    };

        //    var settings = new Newtonsoft.Json.JsonSerializerSettings();

        //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(expected, settings);
        //    var actual = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelParams>(json, settings)!;

        //    // Cannot compare splits with default equality, check they are sequence equal and then set to null
        //    Assert.Equal((IEnumerable<float>)expected.TensorSplits, expected.TensorSplits);
        //    actual.TensorSplits = null!;
        //    expected.TensorSplits = null!;

        //    Assert.Equal(expected, actual);
        //}
    }
}
