using System;
using System.IO;
using System.Text;
using TSKGames.Save;
using UnityEngine;

namespace Archipelago.Helpers.Save {
    public class GameSaver : IGameSaveFile {
        public SaveDataExtension data;

        public GameSaver() { }
        public GameSaver(SaveDataExtension data) { this.data = data; }

        private static int VersionToInt() {
            return Core.Static.Version.Major * 100 + Core.Static.Version.Minor * 10 + Core.Static.Version.Build;
        }

        public override void Read() {
            try {
                var stream = new MemoryStream(GetDatas());
                var binReader = new BinaryReader(stream);
                var version = binReader.ReadInt32();
                if (version != VersionToInt()) return;

                var size = binReader.ReadInt32();
                var bytes = binReader.ReadBytes(size);

                binReader.Close();
                stream.Close();

                string s = Encoding.UTF8.GetString(bytes);
                bytes = Convert.FromBase64String(s);
                data = JsonUtility.FromJson<SaveDataExtension>(Encoding.UTF8.GetString(bytes));
            } catch {
                data = null;
                Core.Static.Error("Failed to read save extension");
            }
        }

        public override void Write() {
            var bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            string s = Convert.ToBase64String(bytes);
            bytes = Encoding.UTF8.GetBytes(s);

            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(VersionToInt());
            binaryWriter.Write(bytes.Length);
            binaryWriter.Write(bytes);

            SetDatas(memoryStream.ToArray());
            binaryWriter.Close();
            memoryStream.Close();
        }
    }
}
