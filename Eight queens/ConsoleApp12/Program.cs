using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EightQueens
{
    public class EightQueens
    {
        private int satir;
        private int sutun;

        public EightQueens(int satir, int sutun)
        {
            this.satir = satir;
            this.sutun = sutun;
        }

        public void play()
        {
            satir++;
        }

        public int satir_al()
        {
            return satir;
        }

        public int sutun_al()
        {
            return sutun;
        }

        public bool Tehdit_Durumu(EightQueens e)
        {
            //Satranç tahtasının satır ve sutun kontrolü
            if (satir == e.satir_al() || sutun == e.sutun_al())
            {
                return true;
            }
            //Satranç tahtasının çarpraz alanlarının kontrolü
            else if (Math.Abs(satir - e.satir_al()) == Math.Abs(sutun - e.sutun_al()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class TepeTırmanma_RandomRestart
    {
        private static int Random_Restarts = 0;
        private static int yer_Degistirme = 0;
        private static int sezgisel = 0;

        public static EightQueens[] Tahta_Uret()
        {
            EightQueens[] baslangic_tahtasi = new EightQueens[8];//8 Vezir olduğu için
            Random rastgele = new Random();
            for (int i = 0; i < 8; i++)
            {
                baslangic_tahtasi[i] = new EightQueens(rastgele.Next(8), i);
            }
            return baslangic_tahtasi;
        }
        public static void Durumu_Goster(EightQueens[] durum)
        {
            int[,] Gecici_Tahta = new int[8, 8];
            for (int i = 0; i < 8; i++)
            {
                Gecici_Tahta[durum[i].satir_al(), durum[i].sutun_al()] = 1; //Vezirler 1 ile gösterilecek
            }
            Console.WriteLine();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Console.Write(Gecici_Tahta[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public static int SezgiselBulmaca(EightQueens[] durum)
        {
            int sezgisel = 0;
            for (int i = 0; i < durum.Length; i++)
            {
                for (int j = i + 1; j < durum.Length; j++)
                {
                    if (durum[i].Tehdit_Durumu(durum[j]))
                    {
                        sezgisel++;
                    }
                }
            }
            return sezgisel;
        }

        public static EightQueens[] sonrakiTahta(EightQueens[] SimdikiTahta)
        {
            EightQueens[] sonrakiTahta = new EightQueens[8];
            EightQueens[] Temp_Tahta = new EightQueens[8];
            int simdikiSezgisel = SezgiselBulmaca(SimdikiTahta);
            int en_iyi_Sezgisel = simdikiSezgisel;
            int gecici_H;

            for (int i = 0; i < 8; i++)
            {
                sonrakiTahta[i] = new EightQueens(SimdikiTahta[i].satir_al(), SimdikiTahta[i].sutun_al());
                Temp_Tahta[i] = sonrakiTahta[i];
            }

            for (int i = 0; i < 8; i++)//tüm sutunları hareket ettirelim
            {
                if (i > 0)
                {
                    Temp_Tahta[i - 1] = new EightQueens(SimdikiTahta[i - 1].satir_al(), SimdikiTahta[i - 1].sutun_al());
                }
                Temp_Tahta[i] = new EightQueens(0, Temp_Tahta[i].sutun_al());
                for (int j = 0; j < 8; j++)//tum satirlari hareket ettirelim
                {
                    gecici_H = SezgiselBulmaca(Temp_Tahta);//Sezgiseli alalım.
                    //gecici satranç tahtası en iyi tahta mı kontrol edelim.
                    if (gecici_H < en_iyi_Sezgisel)
                    {
                        en_iyi_Sezgisel = gecici_H;
                        for (int k = 0; k < 8; k++)
                        {
                            //gecici satranç tahtasını en iyi tahta olarak alalim.
                            sonrakiTahta[k] = new EightQueens(Temp_Tahta[k].satir_al(), Temp_Tahta[k].sutun_al());
                        }
                    }
                    //Veziri hareket ettir
                    if (Temp_Tahta[i].satir_al() != 7)
                    {
                        Temp_Tahta[i].play();
                        yer_Degistirme++;
                    }

                }
            }

            //Simdiki tahta ile en iyi tahta aynı sezgisele mi sahip kontrol edelim.Sonra rastgele bir tahta oluşturup en iyi tahtaya atayalım.
            if (en_iyi_Sezgisel == simdikiSezgisel)
            {
                Random_Restarts++;
                sonrakiTahta = Tahta_Uret();
                sezgisel = SezgiselBulmaca(sonrakiTahta);
            }
            else
            {
                sezgisel = en_iyi_Sezgisel;
            }
            return sonrakiTahta;
        }
        public static void Main(String[] args)
        {
            int Simdiki_Sezgisel;
            double toplam_yer_degisim = 0;
            double toplam_random_restart = 0;
            double toplam_süre = 0;
            EightQueens[] Simdiki_Tahta;
            Console.WriteLine("Yer Değiştirme Restart Sayısı İşlem Süresi(MiliSaniye)" + " Son Satır Ortalamalar");
            for (int i = 0; i < 15; i++)
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                Simdiki_Tahta = Tahta_Uret();
                Simdiki_Sezgisel = SezgiselBulmaca(Simdiki_Tahta);
                while (Simdiki_Sezgisel != 0)
                {
                    Simdiki_Tahta = sonrakiTahta(Simdiki_Tahta);
                    Simdiki_Sezgisel = sezgisel;
                }
                watch.Stop();
                Console.WriteLine(string.Format("{0,-13} | {1,-12} | {2,-9}", yer_Degistirme, Random_Restarts, watch.Elapsed));
                toplam_yer_degisim += yer_Degistirme;
                toplam_random_restart += Random_Restarts;
                toplam_süre += watch.Elapsed.TotalMilliseconds;
                Random_Restarts = 0;
                yer_Degistirme = 0;
            }
            Console.WriteLine(string.Format("{0,-13} | {1,-12} | {2,-9}", (toplam_yer_degisim / 15).ToString(), (toplam_random_restart / 15).ToString(), (toplam_süre / 15).ToString()));
            Console.ReadKey();
        }
    }
}
