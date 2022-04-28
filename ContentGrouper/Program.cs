// See https://aka.ms/new-console-template for more information

using System.Globalization;

Console.WriteLine("Hello, World!");

static void ProcessDirectory(string targetDirectory)
{
    // Process the list of files found in the directory.
    string [] fileEntries = Directory.GetFiles(targetDirectory);
    Parallel.ForEach(fileEntries, ProcessFile);

    // Recurse into subdirectories of this directory.
    string [] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
    foreach(string subdirectory in subdirectoryEntries)
        ProcessDirectory(subdirectory);
}

// Insert logic for processing found files here.
static void ProcessFile(string path)
{
    var updated = File.GetLastWriteTime(path);
    var monthName = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(updated.Month).ToLower();
    var extension = path.Split(".").Last();
    var resultPath = $"D:/root/history/photos/{updated.Year}/{monthName}";
    var resultFilePath = $"D:/root/history/photos/{updated.Year}/{monthName}/{Guid.NewGuid()}.{extension}";
    if (!Directory.Exists(resultPath))
    {
        Directory.CreateDirectory(resultPath);
    }

    try
    {
        File.Copy(path, resultFilePath);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
    
    Console.WriteLine("Processed file '{0}'. Updated: '{1}.' Month: {2}", path, updated, monthName);
}

var path = @"D:\root\history\photos\unsorted";

if(File.Exists(path))
{
    // This path is a file
    ProcessFile(path);
}
else if(Directory.Exists(path))
{
    // This path is a directory
    ProcessDirectory(path);
}
else
{
    Console.WriteLine("{0} is not a valid file or directory.", path);
}