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

        // Filter out unwanted files
        srcFiles = filter(srcFiles, jpgRegex.ToString());
        dstFiles = filter(dstFiles, nefRegex.ToString());

    }

    public static List<string> filter(List<string> files, string regex)
    {
        var _files = new List<string>(files);
        foreach (var file in files)
        {
            if (!Regex.IsMatch(file, regex))
            {
                _files.Remove(file);
                Console.WriteLine(file);
            }
        }

        return _files;
    }
}