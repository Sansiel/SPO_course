using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;


namespace SPO5
{
    class Program
    {
        const int N = 2000;

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();

            string text = File.ReadAllText("test.txt");
            string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            sw = Stopwatch.StartNew();
            var hashOrdered = HashOrdered(words);
            sw.Stop();
            Console.WriteLine("Вставка элементов. Комбинация хэш-адресации с упорядоченным списком: {0} ", sw.Elapsed);

            sw = Stopwatch.StartNew();
            var rehashable = Rehashable(words);
            sw.Stop();
            Console.WriteLine("Вставка элементов. Рехэширование с помощью произведения:  {0} ", sw.Elapsed);

            string tecWord = "";
            while (!(tecWord = Console.ReadLine()).Equals("--stop"))
            {
                Console.WriteLine("results:");
                string res = "";

                sw = Stopwatch.StartNew();
                res = Search(hashOrdered, tecWord);
                sw.Stop();
                Console.WriteLine(res);
                Console.WriteLine("Поиск. Комбинация хэш-адресации с упорядоченным списком: {0} ", sw.Elapsed);

                sw = Stopwatch.StartNew();
                res = Search(rehashable, tecWord);
                sw.Stop();
                Console.WriteLine(res);
                Console.WriteLine("Поиск. Рехэширование с помощью произведения: {0} ", sw.Elapsed);

                Console.WriteLine();
            }

        }

        static List<List<string>> HashOrdered(string[] words)
        {
            List<List<string>> hash = new List<List<string>>(N / 10);
            for (int i = 0; i < N / 10; i++) { hash.Add(null); }

            foreach (string word in words)
            {
                int hashKey = GetHash(word, N / 10);
                List<string> list = new List<string>();
                if (hash[hashKey] == null)
                {
                    list.Add(word);
                    hash[hashKey] = list;
                }
                else
                {
                    list = hash[hashKey];
                    list.Add(word);
                    list.Sort();
                    hash[hashKey] = list;
                }
            }

            return hash;
        }

        static List<string> Rehashable(string[] words)
        {
            List<string> hash = new List<string>(N);

            for (int i = 0; i < N; i++) { hash.Add(null); }

            foreach (string word in words)
            {
                int hashKey = GetHash(word, N);
                if (hash[hashKey] == null) { hash[hashKey] = word; }
                else
                {
                    int i = 1;
                    int newHashKey;
                    while (i <= N)
                    {
                        newHashKey = ReHash(hashKey, i, N);
                        if (hash[newHashKey] != null)
                        {
                            hash[hashKey] = word;
                            break;
                        }
                        else { i += 1; }
                    }
                }
            }

            return hash;
        }


        static int GetHash(string word, int length)
        {
            int res = 0;
            for (int i = 0; i < word.Length; i++)
            {
                res += word[i] * (i + 1);  //Хэш-функция
            }
            return res % length;
        }

        static int ReHash(int curentHash, int index, int length)
        {
            return (curentHash * index) % length; //в отчёте будет - hi(A) = (h(A) * i) mod N
        }


        static string Search(List<List<string>> hash, string word)
        {
            string res = "   Слово не найдено";
            int hashKey = GetHash(word, N / 10);
            if (hash[hashKey] != null)
            {
                foreach (string str in hash[hashKey])
                {
                    if (str.Equals(word))
                    {
                        res = "   Значение Хэша: " + hashKey + "\n   Искомое слово " + word;
                        break;
                    }
                }
            }
            return res;
        }

        static string Search(List<string> hash, string word)
        {
            string res = "   Слово не найдено";
            int hashKey = GetHash(word, N);
            int i = 1;
            while (hash[hashKey] != null && i <= N)
            {
                if (hash[hashKey].Equals(word))
                {
                    res = "   Значение Хэша: " + hashKey + "\n   Искомое слово " + word;
                    break;
                }
                else 
                {
                    hashKey = ReHash(hashKey, i, N);
                    i += 1;
                }
            }

            return res;
        }

    }
}
