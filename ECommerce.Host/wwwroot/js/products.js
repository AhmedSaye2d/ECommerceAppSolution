// js/products.js

// تحميل المنتجات من API
async function loadProducts() {
    const container = document.getElementById("products");
    container.innerHTML = "Loading products...";

    try {
        // رابط الـ API اللي بيجيب المنتجات
        const response = await fetch("https://localhost:44317/api/products");
        if (!response.ok) throw new Error("Failed to load products");

        const products = await response.json();

        // عرض المنتجات
        container.innerHTML = "";
        products.forEach(p => {
            const card = document.createElement("div");
            card.classList.add("product-card");
            card.innerHTML = `
        <img src="${p.image || 'images/default.jpg'}" alt="${p.name}">
        <h3>${p.name}</h3>
        <p>${p.description || ''}</p>
        <strong>${p.price} EGP</strong>
        <button onclick='addToCart(${JSON.stringify(p)})'>Add to Cart</button>
      `;
            container.appendChild(card);
        });

    } catch (err) {
        container.innerHTML = "❌ Error loading products.";
        console.error(err);
    }
}

// إضافة منتج للسلة
function addToCart(product) {
    let cart = JSON.parse(localStorage.getItem("cart")) || [];
    cart.push(product);
    localStorage.setItem("cart", JSON.stringify(cart));
    alert(product.name + " added to cart!");
}

// تحميل المنتجات عند فتح الصفحة
window.onload = loadProducts;
