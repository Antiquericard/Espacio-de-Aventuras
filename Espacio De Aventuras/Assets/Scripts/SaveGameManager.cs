using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveGameManager {
    
	public static void Save (int level) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/levelSave.dat",FileMode.OpenOrCreate);
        bf.Serialize(file, level);
        file.Close();
	}
	

	public static int Load () {
		if(File.Exists(Application.persistentDataPath + "/levelSave.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/levelSave.dat", FileMode.Open);
            int level = (int)bf.Deserialize(file);
            file.Close();
            return level;
        }
        return 0;

	}

	[ContextMenu("Delete saved data")]
	public static void DeleteSavedData(){
		if (File.Exists (Application.persistentDataPath + "/levelSave.dat")) {
			File.Delete (Application.persistentDataPath + "/levelSave.dat");
		}
	}
}
