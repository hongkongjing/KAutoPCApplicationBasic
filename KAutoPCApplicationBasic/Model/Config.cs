using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoPCApplicationBasic.Model
{
    public class ConfigInfo
    {
        public static string SaveFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Config.json";
        protected readonly object _obj = new object();

        [JsonProperty("1.Main Directory")]
        public string? MainDirectory { get; set; }
        public Config config = new Config();
        public ConfigInfo()
        {
            if (!File.Exists(SaveFilePath)) File.Create(SaveFilePath);


        }
        public void GetFromConfig()
        {
        }
        public void SetToConfig()
        {
        }

        public void Save(Config param)
        {

            string _s = JsonConvert.SerializeObject(param);
            string _sin = JToken.Parse(_s).ToString();
            File.WriteAllText(SaveFilePath, _sin, Encoding.UTF8);
        }
        public Config Load()
        {
            string _s = File.ReadAllText(SaveFilePath, Encoding.UTF8);

            var _t = JsonConvert.DeserializeObject<Config>(_s);


            return _t;
        }

        /// <summary>
        /// Save Json File
        /// </summary>
        /// <param name="fileName"> File path </param>
        /// <param name="param"> Data need to save</param>

    }
    public class Config
    {
        public static string SaveFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Config.json";
        public static string AIModelPath { get; set; } = "C:\\Users\\Bon\\Downloads\\best2.onnx";
        public static string[] LabelArray { get; set; } = new string[] { "Finish", "NotFinish", "Home", "Browser", "PlayStore", "SkipAd", "Switch", "SwitchClose", "ProgressBar", "Speaker", "TextCountDown" };
        
    }
}
