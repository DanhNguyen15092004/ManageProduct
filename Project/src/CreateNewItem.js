    const createItemButton = document.querySelector(".CreateNewItemButton");
    const form = document.querySelector(".ProductForm");
    const inputValue = document.querySelectorAll(".productInput")
    const submit = document.querySelector(".submitForm")
    const prodName = document.querySelector("#prodName");
    const price  = document.querySelector("#price")
    const prodType = document.querySelector("#ProdType")
    const dataApi ="http://localhost:8080/api/product" ;
    const valueArray = [];
    let formFlag = false;
    function toggleFormVisibility() {
    if (formFlag) {
        form.style.visibility = "hidden";
        form.style.pointerEvents = "none";
    } else {
        form.style.visibility = "visible";
        form.style.pointerEvents = "auto";
    }
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

        submit.addEventListener("click",(e)=>{
            e.preventDefault();

        const dataJson =  
        {
            productsName : prodName.value,
            currentPrice :parseInt(price.value),
            prodType : prodType.value
        }
        CreateNewItem(dataJson);
        prodName.value = "";
        price.value = "";
        prodType.value = "";
    })  
    async function CreateNewItem (dataJson)
    {
            await fetch(dataApi,{
                method: "POST",
                headers:{
                    "Content-Type": "application/json"
                },
                body : JSON.stringify(dataJson)
            }).then((response) => {
                if (!response.ok) {
                throw new Error("Network response was not ok");
                }
                return response.json();
            })
            .then((responseData) => {
                console.log(responseData);
            })
            .catch((error) => {
            console.error("There was a problem with the fetch operation:", error);
            })  
        }




