/*
    Lojistik Otomasyonu - örnek veriler (tüm kayıtlar hayalidir)
    Çalıştırma: sqlcmd -S "(localdb)\MSSQLLocalDB" -i database\02_seed.sql
*/

USE Lojistik;
GO

-- Firma tipleri: ID'ler uygulamadaki FirmaTipi enum'u ile aynı olmalıdır
SET IDENTITY_INSERT dbo.FirmaTipleri ON;
INSERT INTO dbo.FirmaTipleri (FirmaTipiID, FirmaTipiAdi) VALUES
    (1, N'Taşıyıcı'),
    (2, N'Alıcı'),
    (3, N'Üretici');
SET IDENTITY_INSERT dbo.FirmaTipleri OFF;

INSERT INTO dbo.AliciFirmalar (FirmaAdi, Adres, Telefon) VALUES
    (N'Marmara Market A.Ş.',   N'Kocaeli / İzmit',     N'0262 000 00 01'),
    (N'Ege Toptan Gıda',       N'İzmir / Bornova',     N'0232 000 00 02'),
    (N'Anadolu Yapı Market',   N'Ankara / Ostim',      N'0312 000 00 03');

INSERT INTO dbo.UreticiFirmalar (FirmaAdi, Adres, Telefon) VALUES
    (N'Körfez Plastik San.',   N'Kocaeli / Dilovası',  N'0262 000 00 11'),
    (N'Bursa Tekstil Ltd.',    N'Bursa / Nilüfer',     N'0224 000 00 12');

INSERT INTO dbo.TasiyiciFirmalar (FirmaAdi, TasimaSekli, Adres, Telefon) VALUES
    (N'Hızlı Nakliyat',        N'Kara Yolu',  N'İstanbul / Tuzla',     N'0216 000 00 21'),
    (N'Gökyüzü Kargo',         N'Hava Yolu',  N'İstanbul / Arnavutköy', N'0212 000 00 22'),
    (N'Mavi Liman Denizcilik', N'Deniz Yolu', N'Mersin / Akdeniz',     N'0324 000 00 23');

INSERT INTO dbo.Urunler (UrunAdi, UrunTipi, StokMiktari) VALUES
    (N'PET Şişe 1L',       N'Ambalaj',  12000),
    (N'Pamuk Kumaş Rulo',  N'Tekstil',  340),
    (N'Endüstriyel Palet', N'Lojistik', 850);

INSERT INTO dbo.Bolumler (BolumAdi, FirmaTipiID) VALUES
    (N'Operasyon',      1),
    (N'Satış',          2),
    (N'Satın Alma',     3),
    (N'Muhasebe',       NULL);

INSERT INTO dbo.Personel (AdSoyad, Telefon, BolumID) VALUES
    (N'Ayşe Yılmaz',  N'0555 000 00 01', 1),
    (N'Mehmet Demir', N'0555 000 00 02', 2),
    (N'Zeynep Kaya',  N'0555 000 00 03', 3);

INSERT INTO dbo.Sevkiyatlar (SevkiyatAdi, Ulke, UrunID, TasiyiciFirmaID) VALUES
    (N'SVK-2026-001', N'Almanya',    1, 1),
    (N'SVK-2026-002', N'Hollanda',   3, 3),
    (N'SVK-2026-003', N'Azerbaycan', 2, 2);
GO
