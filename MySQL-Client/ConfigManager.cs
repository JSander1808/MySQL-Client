using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQL_Client {
    internal class ConfigManager {

        private string path = null!;

        public ConfigManager(string path) {
            this.path = path;
        }

        public void init() {
            try {
                if (!File.Exists(path)) {
                    string[] temppath = path.Split('\\');
                    string dicPath = string.Empty;
                    string filePath = temppath[temppath.Length - 1];
                    for (int i = 0; i < temppath.Length - 1; i++) {
                        dicPath += temppath[i] + "\\";
                    }
                    Directory.CreateDirectory(dicPath);
                    File.Create(dicPath + "\\" + filePath).Close();
                }
            }catch(Exception ex) {
                MainViewManager.setErrorMessage(ex.Message);
            }
        }

        public string get(string input) {
            try {
                string data;
                string[] dataAll = File.ReadAllLines(path);
                for (int i = 0; i < dataAll.Length; i++) {
                    if (dataAll[i].Split('^')[0] == input) {
                        return dataAll[i].Split('^')[1];
                    }
                }
                return "";
            }catch(Exception ex) {
                MainViewManager.setErrorMessage(ex.Message);
                return null;
            }
        }

        public void set(string command, string alias) {
            try {
                string[] dataAll = File.ReadAllLines(path);
                List<string> data = new List<string>();
                bool exists = false;
                for (int i = 0; i < dataAll.Length; i++) {
                    if (dataAll[i].Split('^')[0] == command) {
                        data.Add(command + "^" + alias);
                        exists = true;
                    } else {
                        data.Add(dataAll[i]);
                    }
                }
                if (!exists) {
                    data.Add(command + "^" + alias);
                }
                string[] writeData = new string[data.Count];
                for (int i = 0; i < data.Count; i++) {
                    writeData[i] = data[i];
                }
                File.WriteAllText(path, string.Empty);
                File.WriteAllLines(path, writeData);
            }catch(Exception ex) {
                MainViewManager.setErrorMessage(ex.Message);
            }
        }

        public void clear() {
            try {
                File.WriteAllText(path, string.Empty);
            }catch(Exception ex) {
                MainViewManager.setErrorMessage(ex.Message);
            }
        }

        public void remove(string command) {
            try {
                string[] dataAll = File.ReadAllLines(path);
                List<string> data = new List<string>();
                for (int i = 0; i < dataAll.Length; i++) {
                    if (dataAll[i].Split('^')[0] != command) {
                        data.Add(dataAll[i]);
                    }
                }
                string[] writeData = new string[data.Count];
                for (int i = 0; i < data.Count; i++) {
                    writeData[i] = data[i];
                }
                File.WriteAllText(path, string.Empty);
                File.WriteAllLines(path, writeData);
            }catch(Exception ex) {
                MainViewManager.setErrorMessage(ex.Message);
            }
        }

        public List<string> readLines() {
            try {
                string[] dataAll = File.ReadAllLines(path);
                List<string> data = new List<string>();
                for (int i = 0; i < dataAll.Length; i++) {
                    data.Add(dataAll[i]);
                }
                return data;
            }catch(Exception ex) {
                MainViewManager.setErrorMessage(ex.Message);
                return null;
            }
        }
    }
}
