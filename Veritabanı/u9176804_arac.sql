-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Anamakine: localhost:3306
-- Üretim Zamanı: 31 May 2023, 23:09:55
-- Sunucu sürümü: 10.3.38-MariaDB-cll-lve
-- PHP Sürümü: 8.1.16

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Veritabanı: `u9176804_arac`
--

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `araclar`
--

CREATE TABLE `araclar` (
  `id` int(11) NOT NULL,
  `plaka` varchar(11) NOT NULL,
  `marka` varchar(50) NOT NULL,
  `model` varchar(50) NOT NULL,
  `yil` int(11) NOT NULL,
  `renk` varchar(20) NOT NULL,
  `km` bigint(20) NOT NULL,
  `yakit` varchar(10) NOT NULL,
  `sanziman` varchar(10) NOT NULL,
  `g_fiyat` int(11) NOT NULL,
  `h_fiyat` int(11) NOT NULL,
  `a_fiyat` int(11) NOT NULL,
  `durum` int(11) NOT NULL DEFAULT 0
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_turkish_ci;

--
-- Tablo döküm verisi `araclar`
--

INSERT INTO `araclar` (`id`, `plaka`, `marka`, `model`, `yil`, `renk`, `km`, `yakit`, `sanziman`, `g_fiyat`, `h_fiyat`, `a_fiyat`, `durum`) VALUES
(1, '06FT1929', 'HONDA', 'CRV', 2009, 'SIYAH', 301000, 'LPG+BENZIN', 'OTOMATIK', 750, 2500, 15000, 1),
(2, '34BM140', 'RENAULT', 'CLIO', 2023, 'TURUNCU', 20000, 'DIZEL', 'OTOMATIK', 560, 2000, 12500, 0);

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `kullanici`
--

CREATE TABLE `kullanici` (
  `id` int(11) NOT NULL,
  `ad` varchar(75) NOT NULL,
  `soyad` varchar(75) NOT NULL,
  `eposta` varchar(75) NOT NULL,
  `sifre` varchar(75) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_turkish_ci;

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `musteri`
--

CREATE TABLE `musteri` (
  `id` int(11) NOT NULL,
  `tc` bigint(11) NOT NULL,
  `ad` varchar(50) NOT NULL,
  `soyad` varchar(50) NOT NULL,
  `cinsiyet` varchar(11) DEFAULT NULL,
  `dogum_tarihi` date NOT NULL,
  `telefon` bigint(20) NOT NULL,
  `eposta` varchar(100) DEFAULT NULL,
  `adres` varchar(254) DEFAULT NULL,
  `sicil_no` int(11) NOT NULL,
  `ehliyet_tarih` date NOT NULL,
  `kan_grubu` varchar(11) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_turkish_ci;

--
-- Tablo döküm verisi `musteri`
--

INSERT INTO `musteri` (`id`, `tc`, `ad`, `soyad`, `cinsiyet`, `dogum_tarihi`, `telefon`, `eposta`, `adres`, `sicil_no`, `ehliyet_tarih`, `kan_grubu`) VALUES
(1, 38764611804, 'Bahadir', 'Tilki', 'Erkek', '2001-08-23', 905388106358, 'bahadirtilki@outlook.com', 'B.Çekmece / Istanbul', 123019, '2021-12-27', '0 Rh-'),
(3, 26280979294, 'Ismail', 'Nedanli', 'Erkek', '2002-09-17', 905321005657, 'ismailn@gmail.com', 'Muratli / Tekirdag', 129018, '2021-07-15', '0 Rh+'),
(4, 73541289616, 'Nazli', 'Gökbay', 'Kadin', '2005-01-06', 905368106589, 'nazligokby@gmail.com', 'B.Çekmece / Istanbul', 123105, '2023-05-24', 'A Rh+'),
(5, 12603319410, 'Nurhayat', 'Kurt', 'Kadin', '2003-08-27', 905543788999, 'nurhkurt@gmail.com', 'Kizilay / Ankara', 859019, '2022-01-20', 'AB Rh+');

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `resim`
--

CREATE TABLE `resim` (
  `arac_resim_id` int(11) NOT NULL,
  `arac_id` int(11) NOT NULL,
  `resim_adi` varchar(254) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_turkish_ci;

--
-- Tablo döküm verisi `resim`
--

INSERT INTO `resim` (`arac_resim_id`, `arac_id`, `resim_adi`) VALUES
(1, 1, 'Honda_CR-V_(black).JPG'),
(2, 2, 'clio-fiyat.jpg');

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `sozlesme`
--

CREATE TABLE `sozlesme` (
  `sozlesme_id` int(11) NOT NULL,
  `musteri_id` int(11) NOT NULL,
  `arac_id` int(11) NOT NULL,
  `fiyat` int(11) NOT NULL,
  `tarih` date NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_turkish_ci;

--
-- Tablo döküm verisi `sozlesme`
--

INSERT INTO `sozlesme` (`sozlesme_id`, `musteri_id`, `arac_id`, `fiyat`, `tarih`) VALUES
(1, 1, 1, 15000, '2023-05-31'),
(2, 4, 2, 560, '2023-05-31'),
(3, 1, 1, 2500, '2023-05-31'),
(4, 1, 2, 4000, '2023-05-31'),
(5, 3, 1, 5000, '2023-05-31'),
(6, 4, 1, 2500, '2023-05-31'),
(7, 5, 2, 2000, '2023-05-31'),
(8, 3, 1, 2500, '2023-05-31'),
(9, 3, 1, 1500, '2023-05-31');

--
-- Dökümü yapılmış tablolar için indeksler
--

--
-- Tablo için indeksler `araclar`
--
ALTER TABLE `araclar`
  ADD PRIMARY KEY (`id`);

--
-- Tablo için indeksler `kullanici`
--
ALTER TABLE `kullanici`
  ADD PRIMARY KEY (`id`);

--
-- Tablo için indeksler `musteri`
--
ALTER TABLE `musteri`
  ADD PRIMARY KEY (`id`);

--
-- Tablo için indeksler `resim`
--
ALTER TABLE `resim`
  ADD PRIMARY KEY (`arac_resim_id`);

--
-- Tablo için indeksler `sozlesme`
--
ALTER TABLE `sozlesme`
  ADD PRIMARY KEY (`sozlesme_id`);

--
-- Dökümü yapılmış tablolar için AUTO_INCREMENT değeri
--

--
-- Tablo için AUTO_INCREMENT değeri `araclar`
--
ALTER TABLE `araclar`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Tablo için AUTO_INCREMENT değeri `kullanici`
--
ALTER TABLE `kullanici`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Tablo için AUTO_INCREMENT değeri `musteri`
--
ALTER TABLE `musteri`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Tablo için AUTO_INCREMENT değeri `resim`
--
ALTER TABLE `resim`
  MODIFY `arac_resim_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Tablo için AUTO_INCREMENT değeri `sozlesme`
--
ALTER TABLE `sozlesme`
  MODIFY `sozlesme_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
