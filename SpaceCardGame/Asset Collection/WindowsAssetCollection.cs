using System;
using System.Collections.Generic;
using CelesteEngine;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Linq;

namespace SpaceCardGame
{
    public class WindowsAssetCollection : IAssetCollection
    {
        public List<string> GetAllXnbFilesInDirectory(ContentManager content, string directoryPath)
        {
            List<string> files = new List<string>();
            DirectoryInfo contentDirectory = new DirectoryInfo(content.RootDirectory);
            string directory = Path.Combine(contentDirectory.FullName, directoryPath);

            if (Directory.Exists(directory))
            {
                List<string> tempFiles = Directory.GetFiles(directory, "*.xnb*", SearchOption.AllDirectories).ToList();

                foreach (string file in tempFiles)
                {
                    files.Add(file.Replace(contentDirectory.FullName, ""));
                }
            }

            return files;
        }
    }
}
