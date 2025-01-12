# AR Measurement App

AR Measurement App, Unity kullanılarak geliştirilmiş bir artırılmış gerçeklik uygulamasıdır. Kullanıcıların bir yüzey üzerinde seçtikleri noktalar arasındaki mesafeyi, çevreyi ve alanı hesaplamasını sağlar. Uygulama, noktalar arasında çizgiler oluşturarak görsel bir geri bildirim sunar ve bir "Temizle" butonu ile ölçüm verilerini sıfırlama imkanı sunar.

## 🚀 Özellikler

- **AR Raycast**: Yüzey algılama ve dokunma olaylarını işleme.
- **Mesafe Hesaplama**: Seçilen iki nokta arasındaki mesafeyi hesaplar.
- **Çevre ve Alan Hesaplama**: Üç veya daha fazla nokta ile oluşturulan şeklin çevresini ve alanını hesaplar.
- **Dinamik Çizgi**: Seçilen noktalar arasında çizgi oluşturarak görsel bir rehberlik sağlar.
- **"Temizle" Butonu**: Tüm noktaları, çizgileri ve hesaplanan verileri temizler.

## 🛠️ Gereksinimler

- Unity 2022.3.55f1
- AR Foundation
- Desteklenen bir Android cihaz

## 📂 Proje Yapısı

- **Scripts/ARMeasurement.cs**: Ana işlevselliği sağlayan script.
- **Prefabs/Sphere.prefab**: Kullanıcı dokunduğunda oluşturulan nokta objesi.
- **Prefabs/Line.prefab**: Noktalar arasında oluşturulan çizgi objesi.
- **UI**: Mesafe ve ölçüm bilgilerini gösteren TextMeshPro elementleri ve "Temizle" butonu.

## 🔧 Kurulum

1. **Unity Projesini Açın**:
   - Unity Hub üzerinden projeyi açın.

2. **Proje Ayarları**:
   - **Player Settings > Other Settings > Active Input Handling** kısmını **Both** olarak ayarlayın.
   - Gerekli AR Foundation ve ARCore XR Plugin paketlerini yükleyin.

3. **Cihaz Bağlantısı**:
   - Android cihazınızı bağlayın ve **Build & Run** ile uygulamayı çalıştırın.

## 📖 Kullanım

1. Uygulamayı başlatın ve yüzeyin algılandığından emin olun (yüzeyde noktalar görünecek).
2. Yüzeye dokunarak noktalar ekleyin.
3. İki nokta eklediğinizde mesafeyi, üç veya daha fazla nokta eklediğinizde ise çevreyi ve alanı hesaplayabilirsiniz.
4. "Temizle" butonuna tıklayarak ölçümleri sıfırlayabilirsiniz.

## 📜 Lisans

Bu proje [MIT Lisansı](LICENSE) kapsamında lisanslanmıştır.


