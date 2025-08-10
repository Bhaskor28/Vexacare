let quantity = 1;

function updateQuantityDisplay() {
  document.getElementById("kit-quantity-value").textContent = quantity;
}

function increaseQty() {
  quantity++;
  updateQuantityDisplay();
}

function decreaseQty() {
  if (quantity > 1) {
    quantity--;
    updateQuantityDisplay();
  }
}
