using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ConfigEditor.DataAccess
{
    public class ConfigRepository
    {
        // ######################################### //
        #region "member fields"

        private const string Key4InputMeasurements = "inputMeasurementKeys";
        private const string Key4VsmConfig = "VsmConnectString";

        private readonly Configuration _config;
        private string _inputMeasKeysString;

        #endregion

        // ######################################### //
        #region "constructor"

        public ConfigRepository(string filePath)
        {
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = filePath };
            _config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            var setting = _config.AppSettings.Settings[Key4VsmConfig];

            var keyvaluePairs = setting.Value.ParseKeyValuePairs();
            _inputMeasKeysString =
                !keyvaluePairs.ContainsKey(Key4InputMeasurements)
                ? "" : keyvaluePairs[Key4InputMeasurements];

            OtherSettings = (from kv in keyvaluePairs
                             where !kv.Key.Equals(Key4InputMeasurements)
                             select kv).ToArray();
        }

        #endregion

        // ######################################### //
        #region "public API"

        public IEnumerable<KeyValuePair<string, string>> OtherSettings { get; set; }

        public IEnumerable<string> InputMeasurementKeys
        {
            get
            {
                return from segment in _inputMeasKeysString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries) select segment.Trim();
            }
            set
            {
                _inputMeasKeysString =
                    value.Aggregate(new StringBuilder(),
                    (sb, s) => sb.Append(s).Append(";"),
                    sb => sb.ToString());
            }
        }

        public void Save()
        {
            var sb = new StringBuilder();

            var allsettings = OtherSettings.Concat(
                Enumerable.Repeat(
                new KeyValuePair<string, string>(Key4InputMeasurements, _inputMeasKeysString), 1));
            foreach (var kv in allsettings)
            {
                sb.Append(kv.Key).Append("=");
                if (kv.Value.Contains(';'))
                {
                    sb.Append("{").Append(kv.Value).Append("}");
                }
                else
                {
                    sb.Append(kv.Value);
                }
                sb.Append(";");
            }
            _config.AppSettings.Settings[Key4VsmConfig].Value = sb.ToString();
            _config.Save();
        }

        #endregion

    }
}
