// Program developed by @ianfgo on GitHub.
// The program language is Brazilian Portugues, pt-br. However, can be adapted for any language.
// This program is under CC0 1.0 Universal license, se more on "LICENSE" file.

/* In order to use this app on other language, just put a file named "words" on .exe folder.
    This file must contain almost every existing words in this language and they have to be separated by lines. */

namespace Termo
{
    using System;
    using System.Collections.Generic;

    // Main class
	public static class Initialize
	{
		public static void Main()
        {
            Console.WriteLine("Inicializando programa..."); // Starting program...
            Console.WriteLine("Digite a palavra inserida e depois uma sequência de 5 números, onde: 0 não contém essa letra na palavra; 1: contém mas não nessa posição; 2: contém essa letra nessa posição. Por exemplo: piada 01122. Eventualmente o jogo coloca o fundo escuro para letras repetidas que contém na palavra.\n"); // Type the entered word and then a sequence of 5 numbers, where: 0 does not contain this letter in the word; 1: contains but not at this position; 2: contains that letter in that position. For example: joke 01122. Eventually the game puts the dark background for repeated letters that contain in the word.
            while (true)
            {
                ProgramData.Run();

                while (true)
                {
                    string input = Console.ReadLine();
                    if (input.ToLower() == "reiniciar") break;

                    if (input.Count() > 9)
                    {
                        List<string> inputList = input.Split(" ").ToList();
                        Console.WriteLine("Tente a(s) palavra(s): " + Game.Guess(inputList[0], inputList[1])); // Try this/these word(s):
                    }
                    else
                    {
                        Console.WriteLine("O input foi dado errado"); // The input is wrong.
                    }
                }
            }
		}
	}

    // Game related functions
    public static class Game
	{
        public static string Guess(string word, string result)
        {
            List<string> newDictionary = new List<string>();
			foreach (var item in ProgramData.dictionary)
			{
                newDictionary.Add(item.ToLower());
			}

			for (int i = 0; i < 5; i++)
			{
                if (result[i] == '0')
				{
					foreach (var item in ProgramData.dictionary)
					{
                        if (item.ToLower().Contains(word.ToLower()[i]))
						{
                            if (newDictionary.Contains(item.ToLower()))
                            newDictionary.Remove(item.ToLower());
						}
					}
				}
                else if (result[i] == '1')
				{
                    foreach (var item in ProgramData.dictionary)
                    {
                        if (!item.ToLower().Contains(word[i]) || item.ToLower()[i] == word.ToLower()[i])
                        {
                            if (newDictionary.Contains(item.ToLower()))
                                newDictionary.Remove(item.ToLower());
                        }
                    }
                }
                else if (result[i] == '2')
				{
                    foreach (var item in ProgramData.dictionary)
                    {
                        if (item.ToLower()[i] != word.ToLower()[i])
                        {
                            if (newDictionary.Contains(item.ToLower()))
                                newDictionary.Remove(item.ToLower());
                        }
                    }
                }
			}

            ProgramData.dictionary = newDictionary;

            Random random = new Random();
            if (ProgramData.dictionary.Count == 0)
			{
                Console.WriteLine("Ops, acho que não conheço essa palavra ¯\\(°_o)/¯"); // Ops, i think i don't know this word.
                return "";
			}

            string wordResult = "";

            if (ProgramData.dictionary.Count <= 10)
            {
                foreach (var item in ProgramData.dictionary)
                {
                    wordResult += item + ", ";
                }
            }
            else wordResult = ProgramData.dictionary[random.Next(0, ProgramData.dictionary.Count)];

            return wordResult;
        }
    }

	public static class ProgramData
	{
		public static List<string> dictionary = new List<string>();

		public static void Run()
		{
			GetData();
		}

		static void GetData()
		{
			string directory = Environment.CurrentDirectory;
			string fileName = "/words.txt";
			string path = directory + fileName;

            string fiveWordsPath = Environment.CurrentDirectory + "/dictionary.txt";

            if (File.Exists(fiveWordsPath))
			{
                Console.WriteLine("Já existe um dicionário de palavras com 5 letras."); // There is already a 5 words dictionary. 
                string fileContent = File.ReadAllText(fiveWordsPath);
                dictionary = fileContent.Split("\n").ToList();

                Console.WriteLine("O dicionário de 5 letras contém "+dictionary.Count.ToString() + " palavras.\n"); // The 5 words dictionary contains {dictionary.Count.ToString()} words.
                return;
            }

			if (File.Exists(path))
			{
				Console.WriteLine("O arquivo existe e será lido."); // The file exists and is being read.
				ReadAllWordsFile(path);
                Console.WriteLine("Dicionário criado, pode começar o jogo."); // Dictionary created, you may start the game.

            }
			else
			{
				Console.WriteLine("Nenhum arquivo com o nome "+path+"."); // None file named {path}
            }
		}

		static void ReadAllWordsFile(string path)
		{
			string fileContent = File.ReadAllText(path);
			List<string> allWordsDictionary = fileContent.Split("\n").ToList();
			Console.WriteLine("Um total de " + allWordsDictionary.Count.ToString() + " palavras foram encontradas."); // {allWordsDictionary.Count.ToString()} were found.
            Console.WriteLine("Removendo acentos e letras maiúsculas..."); // Removing upper case and accents.

            List<string> defaultAllWordsDictionary = new List<string>();

			foreach (var item in allWordsDictionary)
			{
				string word = item;
				word.ToLower();
				defaultAllWordsDictionary.Add(Strings.RemoveDiacritics(word));
			}

			allWordsDictionary = defaultAllWordsDictionary;

			Console.WriteLine("Criando dicionário de palavras de 5 letras..."); // Creating 5 words dictionary

            dictionary.Clear();

			foreach (var item in allWordsDictionary) { if (item.Count() == 5) dictionary.Add(item.ToLower()); }
            File.WriteAllText(Environment.CurrentDirectory + "/dictionary.txt", String.Join("\n", dictionary.Distinct().ToList()));
		}
	}

    // Code from Stack Overflow (class Strings) - https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
    public static class Strings
    {
        static Dictionary<string, string> foreign_characters = new Dictionary<string, string>
    {
        { "äæǽ", "ae" },
        { "öœ", "oe" },
        { "ü", "ue" },
        { "Ä", "Ae" },
        { "Ü", "Ue" },
        { "Ö", "Oe" },
        { "ÀÁÂÃÄÅǺĀĂĄǍΑΆẢẠẦẪẨẬẰẮẴẲẶА", "A" },
        { "àáâãåǻāăąǎªαάảạầấẫẩậằắẵẳặа", "a" },
        { "Б", "B" },
        { "б", "b" },
        { "ÇĆĈĊČ", "C" },
        { "çćĉċč", "c" },
        { "Д", "D" },
        { "д", "d" },
        { "ÐĎĐΔ", "Dj" },
        { "ðďđδ", "dj" },
        { "ÈÉÊËĒĔĖĘĚΕΈẼẺẸỀẾỄỂỆЕЭ", "E" },
        { "èéêëēĕėęěέεẽẻẹềếễểệеэ", "e" },
        { "Ф", "F" },
        { "ф", "f" },
        { "ĜĞĠĢΓГҐ", "G" },
        { "ĝğġģγгґ", "g" },
        { "ĤĦ", "H" },
        { "ĥħ", "h" },
        { "ÌÍÎÏĨĪĬǏĮİΗΉΊΙΪỈỊИЫ", "I" },
        { "ìíîïĩīĭǐįıηήίιϊỉịиыї", "i" },
        { "Ĵ", "J" },
        { "ĵ", "j" },
        { "ĶΚК", "K" },
        { "ķκк", "k" },
        { "ĹĻĽĿŁΛЛ", "L" },
        { "ĺļľŀłλл", "l" },
        { "М", "M" },
        { "м", "m" },
        { "ÑŃŅŇΝН", "N" },
        { "ñńņňŉνн", "n" },
        { "ÒÓÔÕŌŎǑŐƠØǾΟΌΩΏỎỌỒỐỖỔỘỜỚỠỞỢО", "O" },
        { "òóôõōŏǒőơøǿºοόωώỏọồốỗổộờớỡởợо", "o" },
        { "П", "P" },
        { "п", "p" },
        { "ŔŖŘΡР", "R" },
        { "ŕŗřρр", "r" },
        { "ŚŜŞȘŠΣС", "S" },
        { "śŝşșšſσςс", "s" },
        { "ȚŢŤŦτТ", "T" },
        { "țţťŧт", "t" },
        { "ÙÚÛŨŪŬŮŰŲƯǓǕǗǙǛŨỦỤỪỨỮỬỰУ", "U" },
        { "ùúûũūŭůűųưǔǖǘǚǜυύϋủụừứữửựу", "u" },
        { "ÝŸŶΥΎΫỲỸỶỴЙ", "Y" },
        { "ýÿŷỳỹỷỵй", "y" },
        { "В", "V" },
        { "в", "v" },
        { "Ŵ", "W" },
        { "ŵ", "w" },
        { "ŹŻŽΖЗ", "Z" },
        { "źżžζз", "z" },
        { "ÆǼ", "AE" },
        { "ß", "ss" },
        { "Ĳ", "IJ" },
        { "ĳ", "ij" },
        { "Œ", "OE" },
        { "ƒ", "f" },
        { "ξ", "ks" },
        { "π", "p" },
        { "β", "v" },
        { "μ", "m" },
        { "ψ", "ps" },
        { "Ё", "Yo" },
        { "ё", "yo" },
        { "Є", "Ye" },
        { "є", "ye" },
        { "Ї", "Yi" },
        { "Ж", "Zh" },
        { "ж", "zh" },
        { "Х", "Kh" },
        { "х", "kh" },
        { "Ц", "Ts" },
        { "ц", "ts" },
        { "Ч", "Ch" },
        { "ч", "ch" },
        { "Ш", "Sh" },
        { "ш", "sh" },
        { "Щ", "Shch" },
        { "щ", "shch" },
        { "ЪъЬь", "" },
        { "Ю", "Yu" },
        { "ю", "yu" },
        { "Я", "Ya" },
        { "я", "ya" },
    };

        public static char RemoveDiacritics(this char c)
        {
            foreach (KeyValuePair<string, string> entry in foreign_characters)
            {
                if (entry.Key.IndexOf(c) != -1)
                {
                    return entry.Value[0];
                }
            }
            return c;
        }

        public static string RemoveDiacritics(this string s)
        {
            //StringBuilder sb = new StringBuilder ();
            string text = "";

            foreach (char c in s)
            {
                int len = text.Length;

                foreach (KeyValuePair<string, string> entry in foreign_characters)
                {
                    if (entry.Key.IndexOf(c) != -1)
                    {
                        text += entry.Value;
                        break;
                    }
                }

                if (len == text.Length)
                {
                    text += c;
                }
            }
            return text;
        }
    }
}