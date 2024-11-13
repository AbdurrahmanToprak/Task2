namespace Task.Models
{
    public class UretimOperasyon
{
    public int KayitNo { get; set; }
    public DateTime Baslangic { get; set; }
    public DateTime Bitis { get; set; }
    public double ToplamSure { get; set; }
    public string? Durum { get; set; } // "URETIM" veya "DUROSU"
    public string? DurusNedeni { get; set; } // "ARIZA" veya ""
}
}
