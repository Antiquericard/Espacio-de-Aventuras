
/* 
 * Resume of this project.
 * Copyright (C) Ricardo Ruiz Anaya & Nicolás Robayo Moreno 2017
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveGameManager {
    
	#region Public Methods

	// Guardado de la partida.
	/// <summary>
	/// Guardado de la partida.
	/// </summary>
	/// <returns>.</returns>
	public static void Save (int level) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/levelSave.dat",FileMode.OpenOrCreate);
        bf.Serialize(file, level);
        file.Close();
	}
	
	// Carga de la partida guardada.
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

	// Borrado de los datos guardados.
	[ContextMenu("Delete saved data")]
	public static void DeleteSavedData(){
		if (File.Exists (Application.persistentDataPath + "/levelSave.dat")) {
			File.Delete (Application.persistentDataPath + "/levelSave.dat");
		}
	}

	#endregion

}
