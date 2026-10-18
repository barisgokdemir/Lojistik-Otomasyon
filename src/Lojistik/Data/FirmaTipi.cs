namespace Lojistik.Data;

/// <summary>
/// dbo.FirmaTipleri tablosundaki sabit kayıtlar (database/02_seed.sql ile aynı ID'ler).
/// Eski sürümde bu değerler formlara 1, 6, 7 gibi sihirli sayılarla gömülüydü.
/// </summary>
public enum FirmaTipi
{
    Tasiyici = 1,
    Alici = 2,
    Uretici = 3
}
