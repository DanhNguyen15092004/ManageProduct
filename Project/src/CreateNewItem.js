const createItemButton = document.querySelector(".CreateNewItemButton");
const form = document.querySelector(".ProductForm");
const inputValue = document.querySelectorAll(".productInput");
const submit = document.querySelector(".submitForm");
const prodName = document.querySelector("#prodName");
const price = document.querySelector("#price");
const prodType = document.querySelector("#ProdType");
const dataApi = "http://localhost:8080/api/product";
const valueArray = [];
let formFlag = false;

function toggleFormVisibility() {
    form.style.visibility = formFlag ? "hidden" : "visible";
    form.style.pointerEvents = formFlag ? "none" : "auto";
    formFlag = !formFlag;
}

createItemButton.addEventListener("click", (event) => {
    event.stopPropagation();
    toggleFormVisibility();
});

document.addEventListener("click", (event) => {
    if (formFlag && !form.contains(event.target)) {
        toggleFormVisibility();
    }
});

submit.addEventListener("click", async (e) => {
    if(prodName.value != "" &&price.value != "" && prodType.value != "" ){
        e.preventDefault();
        formFlag != formFlag;
        toggleFormVisibility();
        location.reload();
        const dataJson = {
            productsName: prodName.value,
            currentPrice: parseInt(price.value),
            prodType: prodType.value
        };
        await CreateNewItem(dataJson);
        prodName.value = "";
        price.value = "";
        prodType.value = "";
    }else{
        alert("Phải nhập đủ thông tin ");
        e.preventDefault();
    }
  
});

async function CreateNewItem(dataJson) {
    try {
        const response = await fetch(dataApi, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(dataJson)
        });

        if (!response.ok) {
            throw new Error("Network response was not ok");
        }
    } catch (error) {
        console.error("Error:", error);
    }
}
