using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

namespace LD34.Utility {

    public class BalanceVars : MonoBehaviour {

        public TextAsset source;
        public bool loadOnAwake = true;
        public bool reportStatus = true;

        private void Awake() {
            if (loadOnAwake) Load();
        }

        public const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.SetField;

        public static bool IsValidField(FieldInfo field) {
            return !field.IsInitOnly && !field.IsLiteral && typeof(IConvertible).IsAssignableFrom(field.FieldType);
        }

        public static string GetTrimmedTypeName(Type type) {
            var dotty = type.FullName.Replace('+', '.');

            return !string.IsNullOrEmpty(type.Namespace)
                ? dotty.Substring(type.Namespace.Length + 1)
                : dotty;
        }

        public static string GetFieldPath(FieldInfo field) {
            return string.Concat(GetTrimmedTypeName(field.DeclaringType), ".", field.Name);
        }

        public void Load() {
            var fields = Assembly.GetExecutingAssembly().GetTypes()
                .SelectMany(type => type.GetFields(bindingFlags))
                .Where(field => IsValidField(field))
                .ToDictionary(field => GetFieldPath(field));

            var lines = source.text
                .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim());

            var scope = "";

            foreach (var line in lines) {
                if (line.StartsWith("//")) continue;

                if (line.StartsWith("[") && line.EndsWith("]")) {
                    scope = line.Length == 2 ? "" : line.Substring(1, line.Length - 2) + ".";
                    continue;
                }

                var tokens = line.Split(new[] { '=' }, 2);
                if (tokens.Length < 2) {
                    Debug.LogWarning("Invalid syntax: " + line, source);
                    continue;
                }

                var key = scope + tokens[0].Trim();
                var valString = tokens[1].Trim();

                FieldInfo field;
                if (!fields.TryGetValue(key, out field)) {
                    Debug.LogWarning("Balance var not found: " + key, source);
                    continue;
                }
                try {
                    var value = Convert.ChangeType(valString, field.FieldType);
                    field.SetValue(null, value);
                }
                catch (Exception e) {
                    Debug.LogWarning(string.Format("Cannot set balance var {0}: {1}", key, e), source);
                }
            }

            if (reportStatus) Debug.Log("Balance vars loaded: " + source.name, source);
        }
    }
}
