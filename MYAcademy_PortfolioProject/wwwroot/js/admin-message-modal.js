// Mesaj detayı modalı: Gelen Kutusu ve Gösterge Paneli tarafından ortak kullanılır.
// Gerekenler: sayfada _MessageModalPartial ve data-message-id taşıyan ".btn-open-message" butonları.
(function () {
    const modalElement = document.getElementById('messageModal');
    if (!modalElement) {
        return; // Modal bu sayfada yoksa hiçbir şey yapma
    }

    const messageModal = new bootstrap.Modal(modalElement);
    const modalBody = document.getElementById('messageModalBody');
    const openMessageUrl = modalElement.dataset.openUrl;
    const antiForgeryToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

    // Olay delegasyonu: tek bir dinleyici, sayfadaki bütün "oku" butonlarını yakalar.
    document.addEventListener('click', async (event) => {
        const button = event.target.closest('.btn-open-message');
        if (!button) {
            return;
        }

        modalBody.innerHTML = '<div class="text-center text-muted py-4"><span class="spinner-border spinner-border-sm me-2"></span>Yükleniyor...</div>';
        messageModal.show();

        try {
            const response = await fetch(`${openMessageUrl}/${button.dataset.messageId}`, {
                method: 'POST',
                headers: { 'RequestVerificationToken': antiForgeryToken }
            });
            if (!response.ok) {
                throw new Error(response.status);
            }

            modalBody.innerHTML = await response.text();
            markRowAsRead(button.closest('[data-message-row]'));
        } catch (error) {
            modalBody.innerHTML = '<div class="alert alert-danger mb-0">Mesaj yüklenemedi. Sayfayı yenileyip tekrar deneyin.</div>';
        }
    });

    // Satırı, sayfayı yenilemeden "okundu" görünümüne geçirir.
    // Her sayfa kendi işaretini kullanır; olmayanlar sessizce atlanır.
    function markRowAsRead(row) {
        if (!row || row.dataset.unread !== 'true') {
            return;
        }
        row.dataset.unread = 'false';

        // Gelen Kutusu: yeşil zemin, nokta ve toggle ikonu
        row.classList.remove('message-unread');
        row.querySelector('.unread-dot')?.remove();
        const toggleIcon = row.querySelector('.btn-toggle-read i');
        if (toggleIcon) {
            toggleIcon.className = 'bi bi-envelope';
        }

        // Gösterge Paneli: "Yeni" rozeti → "Okundu"
        const badge = row.querySelector('.message-status-badge');
        if (badge) {
            badge.textContent = 'Okundu';
            badge.classList.replace('c-rose', 'c-blue');
        }

        updateUnreadCounts(-1);
    }

    // data-unread-count taşıyan bütün sayaçları günceller (başlık, sidebar rozeti...).
    function updateUnreadCounts(change) {
        document.querySelectorAll('[data-unread-count]').forEach(el => {
            const newValue = Math.max(0, parseInt(el.textContent) + change);
            el.textContent = newValue;
            if (newValue === 0 && el.hasAttribute('data-remove-when-zero')) {
                el.remove();
            }
        });
    }
})();