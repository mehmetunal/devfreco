using Dev.Core.IO.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Dev.Core.IO
{
    internal struct FileSizeFormat
    {
        internal const string Ffs0 = "0 bytes";
        internal const string Ffsb = "{0:##.##} bytes";
        internal const string Ffsgb = "{0:##.##} gb";
        internal const string Ffskb = "{0:##.##} kb";
        internal const string Ffsmb = "{0:##.##} mb";
        internal const string FileIsReadOnly = "File is read only";
    }

    public enum FolderType
    {
        Folder,
        Unknown,
        Period
    }

    public class FilesManager : IFilesManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Delete(string path)
        {
            var fi = new FileInfo(path);
            if (fi.Exists == false)
                throw new ArgumentNullException("File not found");

            if (fi.Attributes == FileAttributes.Directory)
            {
                ReadOnlyFolderDelete(path);
            }
            else
            {
                fi.Attributes = FileAttributes.Normal;
                System.IO.File.Delete(path);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DirectoryInfo FolderCreat(string path)
            => !Directory.Exists(path)
                ? Directory.CreateDirectory(path)
                : throw new FileLoadException("The record already exists.");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        public string GetFolderType(DirectoryInfo directoryInfo)
            => (String.IsNullOrEmpty(directoryInfo.Extension))
                ? ((directoryInfo.Attributes == FileAttributes.Directory)
                    ? FolderType.Folder.ToString()
                    : FolderType.Unknown.ToString())
                : directoryInfo.Extension.Replace(FolderType.Period.ToString(), String.Empty)
                    ?.ToLowerInvariant();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newFolderName"></param>
        /// <returns></returns>
        /// <exception cref="FileLoadException"></exception>
        public string FolderEditName(string path, string newFolderName)
        {
            var fi = new FileInfo(path);

            if (Directory.Exists(path))
                throw new FileLoadException("Such a record could not be reached");

            var newPath = fi.FullName.Replace(fi.Name, newFolderName);

            var newTempPath = fi.FullName.Replace(fi.Name, newFolderName + "_Temp");

            Directory.Move(path, newTempPath);

            Directory.Move(newTempPath, newPath);

            return newPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string FormatFileSize(long bytes)
        {
            decimal size = 0;
            string result;

            if (bytes >= 1073741824)
            {
                size = Decimal.Divide(bytes, 1073741824);
                result =
                    String.Format(
                        CultureInfo.InvariantCulture,
                        FileSizeFormat.Ffsgb,
                        size);
            }
            else if (bytes >= 1048576)
            {
                size = Decimal.Divide(bytes, 1048576);
                result =
                    String.Format(
                        CultureInfo.InvariantCulture,
                        FileSizeFormat.Ffsmb,
                        size);
            }
            else if (bytes >= 1024)
            {
                size = Decimal.Divide(bytes, 1024);
                result =
                    String.Format(
                        CultureInfo.InvariantCulture,
                        FileSizeFormat.Ffskb,
                        size);
            }
            else if (bytes > 0 & bytes < 1024)
            {
                size = bytes;
                result =
                    String.Format(
                        CultureInfo.InvariantCulture,
                        FileSizeFormat.Ffsb,
                        size);
            }
            else
            {
                result = FileSizeFormat.Ffs0;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createdPath"></param>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="FileLoadException"></exception>
        public async Task<FileResponseModel> FilesCreatAsync(string createdPath, IFormFile file, string fileName)
        {
            var model = new FileResponseModel();
            model.FileInfo = new FileInfo(file.FileName);

            var newName = $"{fileName}{model.FileInfo.Extension}";
            model.FileName = newName;

            var path = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", createdPath, model.FileName);
            model.Path = path;

            if (!Directory.Exists(model.Path))
            {
                model.FileSize = FormatFileSize(file.OpenReadStream().Length);
                await Save(file, model);
                model.IsFolder = false;
                model.IsSuccess = true;
                return model;
            }

            throw new FileLoadException("The record already exists.");
        }

        public async Task<FileResponseModel> FilesEditNameAsync(string path, IFormFile file, string newFileName)
        {
            if (Directory.Exists(path))
                throw new FileLoadException("Such a record could not be reached");

            var model = new FileResponseModel();
            model.FileInfo = new FileInfo(file.FileName);

            var fi = new FileInfo(path) { Attributes = FileAttributes.Normal };

            System.IO.File.Delete(path);

            var fileInfo = new FileInfo(file.FileName);
            model.FileInfo = fileInfo;

            var newName = $"{newFileName}{fileInfo.Extension}";
            model.FileName = newFileName;

            var newPath = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", fi.DirectoryName, newName);
            model.Path = newPath;

            model.FileSize = FormatFileSize(file.OpenReadStream().Length);
            await Save(file, model);
            model.IsFolder = false;
            model.IsSuccess = true;

            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task Save(IFormFile file, FileResponseModel model)
        {
            if (file.Length > 0)
            {
                using (var stream = System.IO.File.Create(model.Path))
                {
                    await file.CopyToAsync(stream);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private void ReadOnlyFolderDelete(string path)
        {
            var di = new DirectoryInfo(path);
            var folders = new Stack<DirectoryInfo>();
            DirectoryInfo folder;

            // Add to the stack.
            folders.Push(di);

            while (folders.Count > 0)
            {
                // Get the folder and set all attributes to normal.
                folder = folders.Pop();
                folder.Attributes = FileAttributes.Normal;

                // Add to the stack.
                foreach (var dir in folder.GetDirectories())
                {
                    folders.Push(dir);
                }

                // Set and delete all of the files.
                foreach (var fi in folder.GetFiles())
                {
                    fi.Attributes = FileAttributes.Normal;
                    fi.Delete();
                }
            }

            // Delete the folder and all sub-folders.
            di.Delete(true);
        }
    }
}