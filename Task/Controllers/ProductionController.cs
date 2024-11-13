using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Task.Models;

public class ProductionController : Controller
{
    public IActionResult Index()
    {
        List<UretimOperasyon> uretimOperasyon = new List<UretimOperasyon>
        {
            new UretimOperasyon { KayitNo = 1, Baslangic = new DateTime(2020, 5, 23, 7, 30, 0), Bitis = new DateTime(2020, 5, 23, 8, 30, 0), ToplamSure = 1, Durum = "URETIM", DurusNedeni = "" },
            new UretimOperasyon { KayitNo = 2, Baslangic = new DateTime(2020, 5, 23, 8, 30, 0), Bitis = new DateTime(2020, 5, 23, 12, 0, 0), ToplamSure = 3.5, Durum = "URETIM", DurusNedeni = "" },
            new UretimOperasyon { KayitNo = 3, Baslangic = new DateTime(2020, 5, 23, 12, 0, 0), Bitis = new DateTime(2020, 5, 23, 13, 0, 0), ToplamSure = 1, Durum = "URETIM", DurusNedeni = "" },
            new UretimOperasyon { KayitNo = 4, Baslangic = new DateTime(2020, 5, 23, 13, 0, 0), Bitis = new DateTime(2020, 5, 23, 13, 45, 0), ToplamSure = 0.75, Durum = "DURUŞ", DurusNedeni = "ARIZA" },
            new UretimOperasyon { KayitNo = 5, Baslangic = new DateTime(2020, 5, 23, 13, 45, 0), Bitis = new DateTime(2020, 5, 23, 17, 30, 0), ToplamSure = 3.75, Durum = "URETIM", DurusNedeni = "" }
        };

        List<StandartDurus> standardDurus = new List<StandartDurus>
        {
            new StandartDurus { Baslangic = new DateTime(2020, 5, 23, 10, 0, 0), Bitis = new DateTime(2020, 5, 23, 10, 15, 0), DurusNedeni = "Çay Molası" },
            new StandartDurus { Baslangic = new DateTime(2020, 5, 23, 12, 0, 0), Bitis = new DateTime(2020, 5, 23, 12, 30, 0), DurusNedeni = "Yemek Molası" },
            new StandartDurus { Baslangic = new DateTime(2020, 5, 23, 15, 0, 0), Bitis = new DateTime(2020, 5, 23, 15, 15, 0), DurusNedeni = "Çay Molası" }
        };

        var table3 = MergeAndFixData(uretimOperasyon, standardDurus);

        return View(table3);
    }

    private List<UretimOperasyon> MergeAndFixData(List<UretimOperasyon> operations, List<StandartDurus> duruslar)
    {
        List<UretimOperasyon> result = new List<UretimOperasyon>();

        foreach (var operation in operations)
        {
            if (operation.Durum == "URETIM")
            {
                var cakisma = duruslar.Where(d => operation.Baslangic < d.Bitis && operation.Bitis > d.Baslangic).ToList();

                if (cakisma.Any())
                {
                    foreach (var durus in cakisma)
                    {
                        result.Add(new UretimOperasyon
                        {
                            KayitNo = operation.KayitNo,
                            Baslangic = durus.Baslangic,
                            Bitis = durus.Bitis,
                            ToplamSure = (durus.Bitis - durus.Baslangic).TotalHours,
                            Durum = "DURUŞ",
                            DurusNedeni = durus.DurusNedeni
                        });
                    }

                    var kalan = cakisma.Last().Bitis;
                    if (kalan < operation.Bitis)
                    {
                        result.Add(new UretimOperasyon
                        {
                            KayitNo = operation.KayitNo,
                            Baslangic = kalan,
                            Bitis = operation.Bitis,
                            ToplamSure = (operation.Bitis - kalan).TotalHours,
                            Durum = "URETIM",
                            DurusNedeni = ""
                        });
                    }
                }
                else
                {
                    result.Add(operation);
                }
            }
            else
            {
                result.Add(operation); 
            }
        }

        return result.OrderBy(x => x.Baslangic).ToList();
    }
}
