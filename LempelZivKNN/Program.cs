using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LempelZivKNN
{
    class Program
    {
        public static void Main(string[] args)
        {
            var deneme = File.ReadLines(@"deneme.txt");
            List<string> sekanslar = new List<string>();
            int matrisBoyutu = 0;
            foreach (var sekans in deneme)
            {
                sekanslar.Add(sekans.ToUpper());
                matrisBoyutu++;
            }
            
            List<int> compressed1;           
            float denklemUst;
            float denklemAlt;
            float uzaklik;
            float[,] uzaklikMatrisi = new float[matrisBoyutu, matrisBoyutu];
            List<int> compressed2;

            for (int i = 0; i < matrisBoyutu; i++)
            {
                compressed1 = Compress(sekanslar[i]);
                for (int j = 0; j < matrisBoyutu; j++)
                {
                    compressed2 = Compress(sekanslar[j]);
                    denklemUst = Math.Max(((Compress(sekanslar[i] + sekanslar[j]).Count) - compressed1.Count), (Compress(sekanslar[i] + sekanslar[j]).Count) - compressed2.Count);
                    denklemAlt = Math.Max(compressed1.Count, compressed2.Count);
                    uzaklik = denklemUst / denklemAlt;
                    Console.WriteLine("(" + (i+1) + "," + (j+1) + ") = " + uzaklik);
                    uzaklikMatrisi[i,j] = uzaklik;

                }
            }
            string dosyayaYaz = "";
            for (int i = 0; i < matrisBoyutu; i++)
            {
                dosyayaYaz = dosyayaYaz + "\n";
                Console.WriteLine("\n");
                for (int j = 0; j < matrisBoyutu; j++)
                {
                    Console.Write(uzaklikMatrisi[i, j] + " | ");
                    //matrisi_yaz.Write(uzaklikMatrisi[i, j]);
                    dosyayaYaz = dosyayaYaz + uzaklikMatrisi[i, j].ToString().Replace(',','.')+",";
                    

                }
            }
            File.WriteAllText(@"uzaklik_matrisi.txt", dosyayaYaz);
        }

        public static List<int> Compress(string uncompressed)  /***Veri sıkıştırma fonksiyonu***/
        {
            // sözlüğü oluşturma
            Dictionary<string, int> sozluk = new Dictionary<string, int>();
            sozluk.Add("A", 0);
            sozluk.Add("T", 1);
            sozluk.Add("G", 2);
            sozluk.Add("C", 3);

            string tutulanKelime = "";
            List<int> compressed = new List<int>();

            foreach (char siradakiHarf in uncompressed)
            {
                string yeniKelime = tutulanKelime + siradakiHarf;

                if (sozluk.ContainsKey(yeniKelime))
                {
                    tutulanKelime = yeniKelime;
                }
                else if (compressed.Contains(sozluk[tutulanKelime]))
                {
                    tutulanKelime = yeniKelime;
                    // yeniKelime sözlükte bulunmadigi için sözlüğe ekle
                    sozluk.Add(yeniKelime, sozluk.Count);
                }
                else
                {   // yeniKelime sözlükte bulunmadigi için sözlüğe ekle
                    sozluk.Add(yeniKelime, sozluk.Count);
                    compressed.Add(sozluk[tutulanKelime]);
                    tutulanKelime = siradakiHarf.ToString();
                }             
            }
            // döngü bittikten sonra, kaldıysa tutulanKelime'yi output'a ekle
            if (!string.IsNullOrEmpty(tutulanKelime))
                compressed.Add(sozluk[tutulanKelime]);

            /*sözlüğü ekrana yazdır
            Console.WriteLine("\nOLUŞTURULAN SÖZLÜK\n-------------");
            foreach (var kelime in sozluk)
            {
                
                    Console.WriteLine(kelime);
            }
            Console.WriteLine("----------------\n");*/
            return compressed;
        }

      
    }
}
