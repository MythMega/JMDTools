using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMDTools
{
    internal class SaveManager
    {
        public string saveLocation { get; set; }
        public string oldFileContent { get; set; }
        public string? newFileContent { get; set; }
        public string projectName { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="projectName"></param>
        public SaveManager(string projectName)
        {
            this.projectName = projectName;
            saveLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "JMD", this.projectName, "base.save");
            if(File.Exists(saveLocation))
            {
                oldFileContent = File.ReadAllText(saveLocation);
            }
            else
            {
                oldFileContent = String.Empty;
            }
        }


        /// <summary>
        /// Will read a property to return a unique value
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public string GetUniqueValue(string propertyName)
        {
            string filePath = saveLocation;

            // Vérifier si le fichier existe
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Save file does not exist!");
            }

            // Lire toutes les lignes du fichier
            string[] lines = File.ReadAllLines(filePath);

            // Parcourir chaque ligne et extraire la valeur associée à la propriété
            foreach (string line in lines)
            {
                // Séparer la ligne en propriété et valeur en utilisant le signe "=" comme délimiteur
                string[] parts = line.Split('=');

                // Vérifier si la propriété correspond
                if (parts.Length == 2 && parts[0].Trim().Equals(propertyName))
                {
                    // Retourner la valeur associée à la propriété
                    return parts[1].Trim();
                }
            }

            // Si la propriété n'est pas trouvée, retourner une valeur par défaut ou générer une exception selon vos besoins
            throw new ArgumentException($"La propriété '{propertyName}' n'a pas été trouvée dans le fichier server.properties.");
        }


        /// <summary>
        /// Will find a property and write the given value
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <exception cref="FileNotFoundException"></exception>
        public void SetUniqueValue(string property, string value)
        {
            string filePath = this.saveLocation;

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Save file does not exist!");
            }

            // Lire le contenu du fichier
            string[] lines = File.ReadAllLines(filePath);

            // Parcourir toutes les lignes du fichier
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                // Vérifier si la ligne contient la variable "white-list"
                if (line.StartsWith(property))
                {
                    // Modifier la valeur de la variable à "true"
                    lines[i] = property + "=" + value;
                    break;
                }
            }

            // Écrire le contenu modifié dans le fichier
            File.WriteAllLines(filePath, lines);
        }


        /// <summary>
        /// Will read a property to return multiples values
        /// </summary>
        /// <param name="property"></param>
        /// <param name="separationChar"></param>
        /// <returns></returns>
        public List<string> GetMultiplesValue(string property, char separationChar=';')
        {
            return GetUniqueValue(property).Split(separationChar).ToList();
        }


        /// <summary>
        /// Will find a property and write the given values
        /// </summary>
        /// <param name="property"></param>
        /// <param name="values"></param>
        /// <param name="separationChar"></param>
        public void SetMultiplesValue(string property, List<string> values, char separationChar = ';')
        {
            SetUniqueValue(property, String.Join(separationChar, values));
        }
    }
}
