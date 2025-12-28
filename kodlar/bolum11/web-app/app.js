const NEWS_ENDPOINT = "http://localhost:5295/api/NewsArticles"

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

function renderNewsList(container, items){
    if(!container) return;
    container.innerHTML = "";

    items.forEach(item => {
        const article = document.createElement("article");
        const h2 = document.createElement("h2");
        h2.textContent = item.title ?? "(Başlıksız)";
        const p = document.createElement("p");
        p.textContent = item.summary ?? "";
        article.appendChild(h2);
        article.appendChild(p);
        container.appendChild(article);
    });
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
        throw new Error(`Haber oluşturulurken bir hata oluştu: ${response.status} - ${response.statusText}  ${text}`);
    }
    const contentType = response.headers.get("Content-Type") || "";
    if(contentType.includes("application/json")){
        return response.json();
    }
    return {};
}

function bindCreateNewsForm(){
    const form = document.getElementById("create-news-form");
    if(!form) return;

    // submit eventini dinle
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
            const created =  await createNews(payload); // API çağrısı
            const createdId = created?.id ?? "";
            alert(`Haber oluşturuldu. ID: ${createdId ? `(id : ${createdId})` : ""}`);
            form.reset();
            const idEl = document.getElementById("id");
            if(idEl){
                idEl.value = "0";
            }
        }
        catch(err){
            console.error(err);
            alert(`Haber oluşturulurken bir hata oluştu: ${err.message || err}`);
        }
    });
}

document.addEventListener("DOMContentLoaded", () => {
    bindCreateNewsForm();
});

window.NewsClient = {
    getAllNews,
    createNews,
    renderNewsList
}

window.NewsClient.createNews = createNews;
window.NewsClient.bindCreateNewsForm = bindCreateNewsForm;
