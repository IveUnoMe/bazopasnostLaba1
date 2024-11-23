using System.IO;
using System;

namespace Pratkicheskaya1
{
    internal static class Drive
    {
        internal static void ShowDriveInfo()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine($"Название: {drive.Name}");
                Console.WriteLine($"Тип: {drive.DriveType}");

                if (drive.IsReady)
                {
                    Console.WriteLine($"Файловая система: {drive.DriveFormat}");

                    double totalSizeGB = drive.TotalSize / (1024.0 * 1024 * 1024);
                    double freeSpaceGB = drive.TotalFreeSpace / (1024.0 * 1024 * 1024);

                    Console.WriteLine($"Объем диска: {totalSizeGB:F2} ГБ");
                    Console.WriteLine($"Свободное пространство: {freeSpaceGB:F2} ГБ");
                    Console.WriteLine($"Метка: {drive.VolumeLabel}");
                }
                Console.WriteLine();
            }
        }
    }
}

