document.addEventListener('DOMContentLoaded', () => {
    // navigation
    document.querySelectorAll('nav a[href^="#"]').forEach((link) => {
        link.addEventListener('click', (e) => {
            const id = link.getAttribute('href');
            const target = id ? document.querySelector(id) : null;
            if (target) {
                e.preventDefault();
                target.scrollIntoView({ behavior: 'smooth', block: 'start' });
                history.pushState(null, '', id); // URL'yi güncelle (isteğe bağlı)
            }
        });
    });

    // tablo
    const personelTable = document.querySelector('#personel table');
    if (personelTable) {
        personelTable.addEventListener('click', (e) => {
            const row = e.target.closest('tbody tr');
            if (!row) return;
            const cells = row.querySelectorAll('td');
            if (cells.length>=3){
                const adSoyad = cells[1].textContent.trim();
                const unvan = cells[2].textContent.trim();
                alert(`Seçilen : ${adSoyad} - ${unvan}`);
            }
        });
    }
});