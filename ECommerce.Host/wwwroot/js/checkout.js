function checkout() {
  let cart = JSON.parse(localStorage.getItem("cart")) || [];
  if (cart.length === 0) {
    alert("Cart is empty!");
    return;
  }

  let total = cart.reduce((sum, item) => sum + item.price, 0);
  alert(`Payment Successful! Total: ${total} EGP`);
  localStorage.removeItem("cart");
}

document.addEventListener("DOMContentLoaded", () => {
  let cart = JSON.parse(localStorage.getItem("cart")) || [];
  const container = document.getElementById("cart-items");
  container.innerHTML = cart
    .map(c => `<p>${c.name} - ${c.price} EGP</p>`)
    .join("");
});
