"use strict";

const NEWS_ENDPOINT = "http://localhost:5295/api/NewsArticles";

async function getAllNews() {
    const response = await fetch(NEWS_ENDPOINT, {
        method: "GET",
        headers: {
            "Accept": "application/json"
        },
    });

    if(!response.ok){
        const text = await response.text().catch(() => "");
        throw new Error(`Haberleri çekerken bir hata: ${response.status} - ${response.statusText}  ${text}`);
    }

    const data = await response.json();
    return Array.isArray(data) ? data : [];
}

async function getNewsById(id){
    if(!id && id !== 0) throw new Error("Geçersiz id");
    const response = await fetch(`${NEWS_ENDPOINT}/${id}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if(!response.ok){
        const text = await response.text().catch(() => "");
        throw new Error(`Haber yüklenemedi: ${response.status} - ${response.statusText}  ${text}`);
    }
    return response.json();
}

async function createNews(payload){
    const response = await fetch(NEWS_ENDPOINT,{
        method: "POST",
        headers:{
            "Accept": "application/json",
            "Content-Type": "application/json; charset=utf-8"
        },
        body: JSON.stringify(payload)
    });
    if(!response.ok){
        const text = await response.text().catch(() => ""); 
        throw new Error(`Haber oluşturulamadı: ${response.status} - ${response.statusText}  ${text}`);
    }
    const contentType = response.headers.get("Content-Type") || "";
    if(contentType.includes("application/json")){
        return response.json();
    }
    return {};
}

async function updateNews(id, payload){
    if(!id && id !== 0) throw new Error("Geçersiz id");
    const response = await fetch(`${NEWS_ENDPOINT}/${id}`,{
        method: "PUT",
        headers:{
            "Accept": "application/json",
            "Content-Type": "application/json; charset=utf-8"
        },
        body: JSON.stringify(payload)
    });
    if(!response.ok){
        const text = await response.text().catch(() => "");
        throw new Error(`Haber güncellenemedi: ${response.status} - ${response.statusText}  ${text}`);
    }
    const contentType = response.headers.get("Content-Type") || "";
    if(contentType.includes("application/json")){
        return response.json();
    }
    return {};
}

async function deleteNews(id){
    if(!id && id !== 0) throw new Error("Geçersiz id");
    const response = await fetch(`${NEWS_ENDPOINT}/${id}`,{
        method: "DELETE",
        headers: { "Accept": "application/json" }
    });
    if(!response.ok){
        const text = await response.text().catch(() => "");
        throw new Error(`Haber silinemedi: ${response.status} - ${response.statusText}  ${text}`);
    }
    return true;
}

function fmtDate(iso){
    if(!iso) return "";
    const d = new Date(iso);
    if(Number.isNaN(d.getTime())) return "";
    return d.toLocaleString(undefined, {year:'numeric', month:'short', day:'2-digit', hour:'2-digit', minute:'2-digit'});
}

function renderNewsList(container, items){
    if(!container) return;
    container.innerHTML = "";

    if(!Array.isArray(items) || items.length === 0){
        container.innerHTML = '<div class="empty">Henüz haber yok.</div>';
        return;
    }

    items.forEach(item => {
        const article = document.createElement("article");
        article.className = "card";

        const h2 = document.createElement("h2");
        h2.className = "card-title";
        h2.textContent = item.title ?? "(Başlıksız)";

        const meta = document.createElement("div");
        meta.className = "card-meta";
        const statusMap = {0: 'Taslak', 1: 'Yayında'};
        const statusText = statusMap[item.status] ?? '';
        meta.textContent = [
            item.authorName ? `Yazar: ${item.authorName}` : null,
            item.publishedAt ? `Tarih: ${fmtDate(item.publishedAt)}` : null,
            statusText ? `Durum: ${statusText}` : null
        ].filter(Boolean).join(' · ');

        const p = document.createElement("p");
        p.className = "card-summary";
        p.textContent = item.summary ?? "";

        const actions = document.createElement("div");
        actions.className = "card-actions";

        const detailLink = document.createElement('a');
        detailLink.href = `details.html?id=${encodeURIComponent(item.id)}`;
        detailLink.className = 'btn btn-ghost';
        detailLink.textContent = 'Detay';

        const editLink = document.createElement('a');
        editLink.href = `update-news.html?id=${encodeURIComponent(item.id)}`;
        editLink.className = 'btn btn-primary';
        editLink.textContent = 'Güncelle';

        const delBtn = document.createElement('button');
        delBtn.type = 'button';
        delBtn.className = 'btn btn-danger';
        delBtn.textContent = 'Sil';
        delBtn.addEventListener('click', async () => {
            const ok = window.confirm(`"${item.title ?? 'Bu haber'}" silinsin mi?`);
            if(!ok) return;
            try{
                await deleteNews(item.id);
                article.remove();
                if(!container.querySelector('.card')){
                    container.innerHTML = '<div class="empty">Haber bulunamadı.</div>';
                }
            }catch(err){
                console.error(err);
                alert(`Silme işlemi başarısız: ${err.message || err}`);
            }
        });

        actions.appendChild(detailLink);
        actions.appendChild(editLink);
        actions.appendChild(delBtn);

        article.appendChild(h2);
        article.appendChild(meta);
        article.appendChild(p);
        article.appendChild(actions);

        container.appendChild(article);
    });
}

function bindCreateNewsForm(){
    const form = document.getElementById("create-news-form");
    if(!form) return;

    form.addEventListener("submit", async (e) => {
        e.preventDefault();

        const id = parseInt((document.getElementById("id")?.value || "0")) || 0;
        const title = document.getElementById("title")?.value.trim() || "";
        const slug = document.getElementById("slug")?.value.trim() || "";
        const summary = document.getElementById("summary")?.value.trim() || "";
        const content = document.getElementById("content")?.value.trim() || "";
        const publishedAtRaw = document.getElementById("publishedAt")?.value || "";
        const status = parseInt((document.getElementById("status")?.value || "0")) || 0;
        const authorName = document.getElementById("authorName")?.value.trim() || "";

        let publishedAt = null;
        if(publishedAtRaw){
            const dt = new Date(publishedAtRaw);
            if(!isNaN(dt)){
                publishedAt = dt.toISOString();
            }
        }

        const payload = {id, title, slug, summary, content, publishedAt, status, authorName};
        try{
            const created =  await createNews(payload);
            const createdId = created?.id ?? "";
            alert(`Haber oluşturuldu. ${createdId ? `(id: ${createdId})` : ""}`);
            form.reset();
            const idEl = document.getElementById("id");
            if(idEl){
                idEl.value = "0";
            }
        }
        catch(err){
            console.error(err);
            alert(`Haber oluşturulamadı: ${err.message || err}`);
        }
    });
}

function qs(name){
    const url = new URL(window.location.href);
    return url.searchParams.get(name);
}

function fillFormWithNews(prefix, news){
    const set = (id, value) => {
        const el = document.getElementById(id);
        if(!el) return;
        if(el.type === 'datetime-local'){
            if(value){
                const dt = new Date(value);
                if(!Number.isNaN(dt.getTime())){
                    const pad = n => String(n).padStart(2,'0');
                    const loc = `${dt.getFullYear()}-${pad(dt.getMonth()+1)}-${pad(dt.getDate())}T${pad(dt.getHours())}:${pad(dt.getMinutes())}:${pad(dt.getSeconds())}`;
                    el.value = loc;
                }
            }
        }else{
            el.value = value ?? '';
        }
    };
    set("id", news.id ?? 0);
    set("title", news.title);
    set("slug", news.slug);
    set("summary", news.summary);
    set("content", news.content);
    set("publishedAt", news.publishedAt);
    set("status", news.status);
    set("authorName", news.authorName);
}

function bindUpdateNewsForm(){
    const form = document.getElementById('update-news-form');
    if(!form) return;

    const idRaw = qs('id');
    const id = idRaw ? parseInt(idRaw) : NaN;
    if(Number.isNaN(id)){
        alert('Geçersiz haber id');
        return;
    }

    (async () => {
        try{
            const data = await getNewsById(id);
            fillFormWithNews('', data);
        }catch(err){
            console.error(err);
            alert(`Haber yüklenemedi: ${err.message || err}`);
        }
    })();

    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        const title = document.getElementById("title")?.value.trim() || "";
        const slug = document.getElementById("slug")?.value.trim() || "";
        const summary = document.getElementById("summary")?.value.trim() || "";
        const content = document.getElementById("content")?.value.trim() || "";
        const publishedAtRaw = document.getElementById("publishedAt")?.value || "";
        const status = parseInt((document.getElementById("status")?.value || "0")) || 0;
        const authorName = document.getElementById("authorName")?.value.trim() || "";

        let publishedAt = null;
        if(publishedAtRaw){
            const dt = new Date(publishedAtRaw);
            if(!isNaN(dt)){
                publishedAt = dt.toISOString();
            }
        }

        const payload = { id, title, slug, summary, content, publishedAt, status, authorName };
        try{
            await updateNews(id, payload);
            alert('Haber güncellendi.');
        }catch(err){
            console.error(err);
            alert(`Güncelleme başarısız: ${err.message || err}`);
        }
    });
}

document.addEventListener("DOMContentLoaded", () => {
    bindCreateNewsForm();
    bindUpdateNewsForm();
});

window.NewsClient = {
    getAllNews,
    renderNewsList,
    getNewsById,
    updateNews,
    deleteNews,
    createNews,
    bindCreateNewsForm,
    bindUpdateNewsForm
};
