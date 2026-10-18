/*
    Lojistik Otomasyonu - veritabanı şeması (SQL Server 2019+ / LocalDB)
    Çalıştırma: sqlcmd -S "(localdb)\MSSQLLocalDB" -i database\01_schema.sql
    Script tekrar çalıştırılabilir; mevcut tabloları silip yeniden oluşturur.
*/

IF DB_ID(N'Lojistik') IS NULL
BEGIN
    CREATE DATABASE Lojistik COLLATE Turkish_CI_AS;
END
GO

USE Lojistik;
GO

-- Bağımlılık sırasının tersine silme
DROP TABLE IF EXISTS dbo.Sevkiyatlar;
DROP TABLE IF EXISTS dbo.Personel;
DROP TABLE IF EXISTS dbo.Bolumler;
DROP TABLE IF EXISTS dbo.Urunler;
DROP TABLE IF EXISTS dbo.TasiyiciFirmalar;
DROP TABLE IF EXISTS dbo.UreticiFirmalar;
DROP TABLE IF EXISTS dbo.AliciFirmalar;
DROP TABLE IF EXISTS dbo.FirmaTipleri;
GO

CREATE TABLE dbo.FirmaTipleri
(
    FirmaTipiID   INT IDENTITY(1, 1) NOT NULL,
    FirmaTipiAdi  NVARCHAR(50)       NOT NULL,
    CONSTRAINT PK_FirmaTipleri PRIMARY KEY (FirmaTipiID),
    CONSTRAINT UQ_FirmaTipleri_Ad UNIQUE (FirmaTipiAdi)
);

CREATE TABLE dbo.AliciFirmalar
(
    AliciFirmaID  INT IDENTITY(1, 1) NOT NULL,
    FirmaAdi      NVARCHAR(150)      NOT NULL,
    Adres         NVARCHAR(300)      NULL,
    Telefon       NVARCHAR(20)       NULL,
    FirmaTipiID   INT                NOT NULL CONSTRAINT DF_AliciFirmalar_Tip DEFAULT (2),
    CONSTRAINT PK_AliciFirmalar PRIMARY KEY (AliciFirmaID),
    CONSTRAINT FK_AliciFirmalar_FirmaTipleri FOREIGN KEY (FirmaTipiID) REFERENCES dbo.FirmaTipleri (FirmaTipiID)
);

CREATE TABLE dbo.UreticiFirmalar
(
    UreticiFirmaID INT IDENTITY(1, 1) NOT NULL,
    FirmaAdi       NVARCHAR(150)      NOT NULL,
    Adres          NVARCHAR(300)      NULL,
    Telefon        NVARCHAR(20)       NULL,
    FirmaTipiID    INT                NOT NULL CONSTRAINT DF_UreticiFirmalar_Tip DEFAULT (3),
    CONSTRAINT PK_UreticiFirmalar PRIMARY KEY (UreticiFirmaID),
    CONSTRAINT FK_UreticiFirmalar_FirmaTipleri FOREIGN KEY (FirmaTipiID) REFERENCES dbo.FirmaTipleri (FirmaTipiID)
);

CREATE TABLE dbo.TasiyiciFirmalar
(
    TasiyiciFirmaID INT IDENTITY(1, 1) NOT NULL,
    FirmaAdi        NVARCHAR(150)      NOT NULL,
    TasimaSekli     NVARCHAR(20)       NOT NULL,
    Adres           NVARCHAR(300)      NULL,
    Telefon         NVARCHAR(20)       NULL,
    FirmaTipiID     INT                NOT NULL CONSTRAINT DF_TasiyiciFirmalar_Tip DEFAULT (1),
    CONSTRAINT PK_TasiyiciFirmalar PRIMARY KEY (TasiyiciFirmaID),
    CONSTRAINT FK_TasiyiciFirmalar_FirmaTipleri FOREIGN KEY (FirmaTipiID) REFERENCES dbo.FirmaTipleri (FirmaTipiID),
    CONSTRAINT CK_TasiyiciFirmalar_TasimaSekli CHECK (TasimaSekli IN (N'Kara Yolu', N'Hava Yolu', N'Deniz Yolu'))
);

CREATE TABLE dbo.Urunler
(
    UrunID       INT IDENTITY(1, 1) NOT NULL,
    UrunAdi      NVARCHAR(150)      NOT NULL,
    UrunTipi     NVARCHAR(100)      NULL,
    StokMiktari  INT                NOT NULL CONSTRAINT DF_Urunler_Stok DEFAULT (0),
    CONSTRAINT PK_Urunler PRIMARY KEY (UrunID),
    CONSTRAINT CK_Urunler_Stok CHECK (StokMiktari >= 0)
);

CREATE TABLE dbo.Bolumler
(
    BolumID      INT IDENTITY(1, 1) NOT NULL,
    BolumAdi     NVARCHAR(100)      NOT NULL,
    FirmaTipiID  INT                NULL,
    CONSTRAINT PK_Bolumler PRIMARY KEY (BolumID),
    CONSTRAINT FK_Bolumler_FirmaTipleri FOREIGN KEY (FirmaTipiID) REFERENCES dbo.FirmaTipleri (FirmaTipiID)
);

CREATE TABLE dbo.Personel
(
    PersonelID  INT IDENTITY(1, 1) NOT NULL,
    AdSoyad     NVARCHAR(150)      NOT NULL,
    Telefon     NVARCHAR(20)       NULL,
    BolumID     INT                NOT NULL,
    CONSTRAINT PK_Personel PRIMARY KEY (PersonelID),
    CONSTRAINT FK_Personel_Bolumler FOREIGN KEY (BolumID) REFERENCES dbo.Bolumler (BolumID)
);

CREATE TABLE dbo.Sevkiyatlar
(
    SevkiyatID       INT IDENTITY(1, 1) NOT NULL,
    SevkiyatAdi      NVARCHAR(150)      NOT NULL,
    Ulke             NVARCHAR(100)      NOT NULL,
    UrunID           INT                NOT NULL,
    TasiyiciFirmaID  INT                NOT NULL,
    OlusturmaTarihi  DATETIME2(0)       NOT NULL CONSTRAINT DF_Sevkiyatlar_Tarih DEFAULT (SYSDATETIME()),
    CONSTRAINT PK_Sevkiyatlar PRIMARY KEY (SevkiyatID),
    CONSTRAINT FK_Sevkiyatlar_Urunler FOREIGN KEY (UrunID) REFERENCES dbo.Urunler (UrunID),
    CONSTRAINT FK_Sevkiyatlar_TasiyiciFirmalar FOREIGN KEY (TasiyiciFirmaID) REFERENCES dbo.TasiyiciFirmalar (TasiyiciFirmaID)
);
GO

-- Yabancı anahtar indeksleri (SQL Server bunları otomatik oluşturmaz)
CREATE INDEX IX_Bolumler_FirmaTipiID         ON dbo.Bolumler (FirmaTipiID);
CREATE INDEX IX_Personel_BolumID             ON dbo.Personel (BolumID);
CREATE INDEX IX_Sevkiyatlar_UrunID           ON dbo.Sevkiyatlar (UrunID);
CREATE INDEX IX_Sevkiyatlar_TasiyiciFirmaID  ON dbo.Sevkiyatlar (TasiyiciFirmaID);
GO
