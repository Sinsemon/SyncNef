using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class syncnef
{
    private static string srcPath;
    private static string dstPath;
    private static Regex jpgRegex = new Regex(@"(?i)\.jpg$");
    private static Regex nefRegex = new Regex(@"(?i)\.nef$");

    /// <summary>
    /// Compares two destinations. In one are JPG files in the other are NEF files.
    /// If a JPG file is deleted than the NEF files gets moved to a "deleted" subfolder of the second destination.
    /// </summary>
    /// <param name="args"> Command line Arguments. </param>
    public static void Main(string[] args)
    {
        //Assign relative or absolute paths
        if (!args[0].StartsWith("."))
        {
            srcPath = args[0];
        }
        else
        {
            srcPath = Path.GetFullPath(Environment.CurrentDirectory + args[0]);
        }

        if (!args[0].StartsWith("."))
        {
            dstPath = args[1];
        }
        else
        {
            dstPath = Path.GetFullPath(Environment.CurrentDirectory + args[1]);
        }

        List<string> srcFiles = new List<string>(Directory.GetFiles(srcPath)
            .Select(e => Path.GetFileName(e)));
        List<string> dstFiles = new List<string>(Directory.GetFiles(dstPath)
            .Select(e => Path.GetFileName(e)));

        List<string> origSrcFiles = new List<string>(srcFiles);
        List<string> origDstFiles = new List<string>(dstFiles);


        // Filter out unwanted files
        srcFiles = filter(srcFiles, jpgRegex.ToString());
        dstFiles = filter(dstFiles, nefRegex.ToString());


        // Remove Ending of files (i.e. r1b2.jpg)
        srcFiles = srcFiles.Select(e => Regex.Replace(e, @"(?i)(?:[rb][0-9]*)*\.jpg", "")).ToList();
        dstFiles = dstFiles.Select(e => Regex.Replace(e, @"(?i)\.nef", "")).ToList();


        // Create deleted folder 
        if (!Directory.Exists(dstPath + @"/deleted"))
        {
            Directory.CreateDirectory(dstPath + @"/deleted");
        }

        // Move unwanted files in deleted folder
        foreach (var dstFile in dstFiles)
        {
            if (!srcFiles.Exists(e => e.Equals(dstFile))) // Compare to filtered list to avoid deleting other files
            {
                var toDelete = origDstFiles.Find(e => e.Contains(dstFile));
                File.Move(dstPath + $"/{toDelete}", dstPath + $"/deleted/{toDelete}");
                Console.WriteLine($"deleted File: {toDelete}");
            }
        }
    }


    /// <summary>
    /// Filters out every file that does not match the given regex.
    /// </summary>
    /// <param name="files">List of files to filter</param>
    /// <param name="regex">Regex to match</param>
    /// <returns></returns>
    public static List<string> filter(List<string> files, string regex)
    {
        var _files = new List<string>(files);
        foreach (var file in files)
        {
            if (!Regex.IsMatch(file, regex))
            {
                _files.Remove(file);
                //Console.WriteLine(file);
            }
        }

        return _files;
    }
}