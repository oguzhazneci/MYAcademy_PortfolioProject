// Bütün admin sayfalarında ortak kullanıcı deneyimi davranışları.

// Silme (ve benzeri geri alınamaz işlemler) için onay penceresi.
// Kullanım: <form ... data-confirm="Bu kaydı silmek istediğinize emin misiniz?">
document.addEventListener('submit', function (event) {
    const form = event.target;
    if (!form.matches('form[data-confirm]')) {
        return; // Onay istemeyen formlar normal şekilde gönderilir
    }

    // Formun gönderilmesini şimdilik durdur; kullanıcının cevabını bekle
    event.preventDefault();

    Swal.fire({
        title: 'Emin misiniz?',
        text: form.dataset.confirm,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Evet, sil',
        cancelButtonText: 'Vazgeç',
        confirmButtonColor: '#b44b4b',
        cancelButtonColor: '#8a8a85',
        reverseButtons: true,   // "Vazgeç" solda, "Evet" sağda
        focusCancel: true       // Enter'a yanlışlıkla basılırsa silme değil vazgeçme çalışsın
    }).then(function (result) {
        if (result.isConfirmed) {
            // form.submit() submit olayını yeniden tetiklemez; bu yüzden tekrar onay sorulmaz.
            form.submit();
        }
    });
});

// Başarı ve hata bildirimleri: _NotificationPartial'ın sayfaya yazdığı veriyi SweetAlert2 ile gösterir.
(function () {
    const holder = document.getElementById('adminNotification');
    if (!holder) {
        return; // Bu istekte gösterilecek bildirim yok
    }

    if (holder.dataset.error) {
        // Hata: kullanıcı okuyup kapatana kadar ekranda kalır (gözden kaçmamalı).
        Swal.fire({
            icon: 'error',
            title: 'İşlem yapılamadı',
            text: holder.dataset.error,
            confirmButtonText: 'Tamam',
            confirmButtonColor: '#2f5d4e'
        });
    } else if (holder.dataset.success) {
        // Başarı: köşede kısa süre görünüp kendiliğinden kaybolan küçük bildirim (toast).
        Swal.fire({
            toast: true,
            position: 'top-end',
            icon: 'success',
            title: holder.dataset.success,
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true
        });
    }
})();