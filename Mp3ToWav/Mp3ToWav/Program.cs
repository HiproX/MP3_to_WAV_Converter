using System.Drawing;
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Versioning;
using NAudio.SoundFont;
using NAudio.Wave;

namespace Mp3ToWav
{
    class Program
    {
        public static int Main(string[] args)
        {
            string directory = null;
            if (args.Length == 1)
            {
                directory = Path.GetDirectoryName(args[0]);
            }
            else if (args.Length > 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: arguments more than One.");
                Console.ForegroundColor= ConsoleColor.Yellow;
                Console.WriteLine("Use One argument: the path to directory with MP3 files.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\nPress any key to exit.");
                Console.ReadKey();
                
                return -1;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Enter path to directory: ");
                Console.ForegroundColor = ConsoleColor.White;
                
                directory = Console.ReadLine();
                
                Console.WriteLine("\n");
            }

            if (Directory.Exists(directory))
            {
                var files = Directory.GetFiles(directory, "*.mp3").ToList();
                
                Console.WriteLine($" --> [ {directory} ]");
                
                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"    --> [ {Path.GetFileName(file)} ]");
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("\nAre you want to start convert ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("MP3");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" to ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("WAV");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" (Y/N)");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(": ");
                    Console.ForegroundColor = ConsoleColor.White;
                    
                    var key = Console.ReadKey();
                    Console.WriteLine("\n");
                    
                    if (key.Key != ConsoleKey.N)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("  Conversion started:");

                        int count = 1;
                        foreach (var file in files)
                        {
                            var fname = Path.GetFileNameWithoutExtension(file);
                            var oldName = $"{fname}.mp3";
                            var newName = $"{count++}.wav";

                            if (File.Exists(file))
                            {
                                ConvertMp3ToWav(file, $"{directory}\\{newName}");
                                
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write($"    + ");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write($"{oldName}");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(" --> ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"{newName}");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("    - ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("File ");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write($"{oldName}");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(" has been moved or deleted!");
                            }
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\n  W O R K  C O M P L E T E D !");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n\nPress any key to exit.");
                    Console.ReadKey();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"    --> < *.mp3 files not found>\n[!] Check the path and try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n\nPress any key to exit.");
                    Console.ReadKey();
                    return 0;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: path to directory doesn't exist.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Use One argument: the path to directory with MP3 files.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\nPress any key to exit.");
                Console.ReadKey();
                return -2;
            }
            
            return 0;
        }
        private static void ConvertMp3ToWav(string _inPath_, string _outPath_)
        {
            using (Mp3FileReader mp3 = new Mp3FileReader(_inPath_))
            {
                
                using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    WaveFileWriter.CreateWaveFile(_outPath_, pcm);
                }
            }
        }
    }
}