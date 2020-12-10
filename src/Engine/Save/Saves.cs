namespace Engine.Save
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// Defines the <see cref="Saves{S, P}" />
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="P"></typeparam>
    public class Saves<S, P> where S : Savefile<P> where P : SavePreview
    {
        /// <summary>
        /// Defines the path
        /// </summary>
        private readonly string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saves{S, P}"/> class.
        /// </summary>
        /// <param name="path">The path<see cref="string"/></param>
        public Saves(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Writes the save to a file
        /// </summary>
        /// <param name="save">The save<see cref="S"/></param>
        public void WriteSave(S save)
        {
            WriteFile(save, "Save" + save.Index);
        }

        /// <summary>
        /// Writes the previews to a file
        /// </summary>
        /// <param name="previews">The previews<see cref="List{P}"/></param>
        public void WritePreviews(List<P> previews)
        {
            WriteFile(previews, "Previews");
        }

        /// <summary>
        /// Reads a save from file based on the index
        /// </summary>
        /// <param name="index">The index<see cref="int"/></param>
        /// <returns>The <see cref="S"/></returns>
        public S ReadSave(int index)
        {
            return ReadFile<S>("Save" + index);
        }

        /// <summary>
        /// Reads save previews from file
        /// </summary>
        /// <returns>The <see cref="List{P}"/></returns>
        public List<P> ReadSavePreviews()
        {
            return ReadFile<List<P>>("Previews");
        }

        /// <summary>
        /// Deletes a save file if it exists
        /// </summary>
        /// <param name="index">The index<see cref="int"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool DeleteSave(int index)
        {
            if (File.Exists("Save" + index))
            {
                File.Delete("Save" + index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Writes an object to a file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj<see cref="T"/></param>
        /// <param name="fileName">The fileName<see cref="string"/></param>
        private void WriteFile<T>(T obj, string fileName)
        {
            using (Stream stream = File.Open(path + "/" + fileName, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, obj);
            }
        }

        /// <summary>
        /// Reads an object from a file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">The fileName<see cref="string"/></param>
        /// <returns>The <see cref="T"/></returns>
        private T ReadFile<T>(string fileName)
        {
            using (Stream stream = File.Open(path + "/" + fileName, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
